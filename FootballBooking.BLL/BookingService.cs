using FootballBooking.DAL;
using FootballBooking.Models;

namespace FootballBooking.BLL;

public class BookingService
{
    private readonly BookingRepository _bookingRepository;
    private readonly FieldRepository _fieldRepository;

    public BookingService(BookingRepository bookingRepository, FieldRepository fieldRepository)
    {
        _bookingRepository = bookingRepository;
        _fieldRepository = fieldRepository;
    }

    public async Task<bool> HasConflictAsync(int fieldId, DateTime bookingDate, TimeSpan startTime, TimeSpan endTime)
    {
        return await _bookingRepository.HasScheduleConflictAsync(fieldId, bookingDate, startTime, endTime);
    }

    public async Task<int> CreateBookingAsync(int userId, int fieldId, DateTime bookingDate, TimeSpan startTime, TimeSpan endTime, string? note)
    {
        if (await HasConflictAsync(fieldId, bookingDate, startTime, endTime))
        {
            throw new InvalidOperationException("Yêu cầu đặt sân bị trùng giờ.");
        }

        var field = await _fieldRepository.GetFieldByIdAsync(fieldId);
        if (field is null)
        {
            throw new InvalidOperationException("Sân không tồn tại.");
        }

        var duration = (decimal)(endTime - startTime).TotalHours;
        if (duration <= 0)
        {
            throw new ArgumentException("Thời lượng thuê sân phải lớn hơn 0.");
        }

        var booking = new Booking
        {
            UserId = userId,
            FieldId = fieldId,
            BookingDate = bookingDate.Date,
            StartTime = startTime,
            EndTime = endTime,
            TotalPrice = CalculateBookingPrice(field, startTime, endTime),
            Status = BookingStatus.Pending,
            Note = note
        };

        return await _bookingRepository.CreateBookingAsync(booking);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
    {
        return await _bookingRepository.GetBookingsByUserIdAsync(userId);
    }

    public async Task<Booking?> GetBookingByIdAsync(int bookingId)
    {
        return await _bookingRepository.GetBookingByIdAsync(bookingId);
    }

    public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
    {
        return await _bookingRepository.GetAllBookingsAsync();
    }

    public async Task<IEnumerable<BookingWithDetails>> GetAllBookingsWithDetailsAsync()
    {
        return await _bookingRepository.GetAllBookingsWithDetailsAsync();
    }

    public async Task UpdateBookingAsync(Booking booking)
    {
        var currentBooking = await _bookingRepository.GetBookingByIdAsync(booking.Id);
        if (currentBooking is null)
        {
            throw new InvalidOperationException("Đơn đặt sân không tồn tại.");
        }

        EnsureValidStatusTransition(currentBooking.Status, booking.Status, booking.BookingDate, booking.EndTime);
        await _bookingRepository.UpdateBookingAsync(booking);
    }

    public async Task CancelBookingAsync(int bookingId, int userId)
    {
        var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
        if (booking is null || booking.UserId != userId)
        {
            throw new InvalidOperationException("Không tìm thấy đơn đặt sân của bạn.");
        }

        if (booking.Status is BookingStatus.Completed or BookingStatus.Cancelled or BookingStatus.Expired)
        {
            throw new InvalidOperationException("Không thể hủy đơn đặt sân đã hoàn tất, đã hủy hoặc đã hết hạn.");
        }

        booking.Status = BookingStatus.Cancelled;
        await _bookingRepository.UpdateBookingAsync(booking);
    }

    public async Task DeleteBookingAsync(int id)
    {
        await _bookingRepository.DeleteBookingAsync(id);
    }

    public static decimal CalculateBookingPrice(Field field, TimeSpan startTime, TimeSpan endTime)
    {
        var duration = (decimal)(endTime - startTime).TotalHours;
        if (!field.PeakStartTime.HasValue || !field.PeakEndTime.HasValue || !field.PeakPricePerHour.HasValue)
        {
            return Math.Round(duration * field.PricePerHour, 2);
        }

        var peakStart = field.PeakStartTime.Value;
        var peakEnd = field.PeakEndTime.Value;
        if (peakEnd <= peakStart)
        {
            return Math.Round(duration * field.PricePerHour, 2);
        }

        var overlapStart = startTime > peakStart ? startTime : peakStart;
        var overlapEnd = endTime < peakEnd ? endTime : peakEnd;
        var peakHours = overlapEnd > overlapStart
            ? (decimal)(overlapEnd - overlapStart).TotalHours
            : 0;
        var normalHours = duration - peakHours;

        return Math.Round(normalHours * field.PricePerHour + peakHours * field.PeakPricePerHour.Value, 2);
    }

    private static void EnsureValidStatusTransition(BookingStatus currentStatus, BookingStatus nextStatus, DateTime bookingDate, TimeSpan endTime)
    {
        if (currentStatus == nextStatus)
        {
            return;
        }

        if (currentStatus is BookingStatus.Completed or BookingStatus.Cancelled or BookingStatus.Expired)
        {
            throw new InvalidOperationException("Không thể cập nhật đơn đặt sân đã hoàn tất, đã hủy hoặc đã hết hạn.");
        }

        var rank = new Dictionary<BookingStatus, int>
        {
            [BookingStatus.Pending] = 0,
            [BookingStatus.Confirmed] = 1,
            [BookingStatus.InProgress] = 2,
            [BookingStatus.Completed] = 3,
            [BookingStatus.Cancelled] = 99,
            [BookingStatus.Expired] = 99
        };

        if (nextStatus is not BookingStatus.Cancelled and not BookingStatus.Expired &&
            rank[nextStatus] < rank[currentStatus])
        {
            throw new InvalidOperationException("Không thể chuyển lùi trạng thái đặt sân.");
        }

        if (nextStatus == BookingStatus.InProgress && currentStatus != BookingStatus.Confirmed)
        {
            throw new InvalidOperationException("Chỉ đơn đã xác nhận mới được chuyển sang đang diễn ra.");
        }

        if (nextStatus == BookingStatus.Completed && currentStatus != BookingStatus.InProgress)
        {
            throw new InvalidOperationException("Chỉ đơn đang diễn ra mới được chuyển sang hoàn thành.");
        }

        var scheduledEnd = bookingDate.Date.Add(endTime);
        if (nextStatus == BookingStatus.Completed && DateTime.Now < scheduledEnd)
        {
            throw new InvalidOperationException("Chưa thể hoàn thành đơn trước giờ kết thúc sân.");
        }
    }
}
