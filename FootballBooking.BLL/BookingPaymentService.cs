using FootballBooking.DAL;
using FootballBooking.Models;

namespace FootballBooking.BLL;

public class BookingPaymentService
{
    private const decimal MomoMinAmount = 10000m;
    private const decimal MomoMaxAmount = 50000000m;

    private readonly BookingRepository _bookingRepository;
    private readonly BookingPaymentRepository _paymentRepository;

    public BookingPaymentService(BookingRepository bookingRepository, BookingPaymentRepository paymentRepository)
    {
        _bookingRepository = bookingRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<BookingPayment?> GetLatestPaymentAsync(int bookingId)
    {
        return await _paymentRepository.GetLatestByBookingIdAsync(bookingId);
    }

    public async Task<int> CreateCashPaymentAsync(int bookingId, int userId)
    {
        var booking = await GetUserBookingAsync(bookingId, userId);
        if (booking.Status is BookingStatus.Completed or BookingStatus.Cancelled or BookingStatus.Expired)
        {
            throw new InvalidOperationException("Khong the thanh toan booking da hoan tat, da huy hoac da het han.");
        }

        var latest = await _paymentRepository.GetLatestByBookingIdAsync(bookingId);
        if (latest?.Status == PaymentStatus.Success)
        {
            throw new InvalidOperationException("Booking nay da duoc thanh toan.");
        }

        var payment = new BookingPayment
        {
            BookingId = bookingId,
            PaymentMethod = "Tien mat",
            Amount = booking.TotalPrice,
            Status = PaymentStatus.Success,
            PaidAt = DateTime.Now
        };

        var paymentId = await _paymentRepository.CreateAsync(payment);
        if (booking.Status == BookingStatus.Pending)
        {
            booking.Status = BookingStatus.Confirmed;
            await _bookingRepository.UpdateBookingAsync(booking);
        }

        return paymentId;
    }

    public async Task<int> CreateMomoPendingPaymentAsync(int bookingId, int userId)
    {
        var booking = await GetUserBookingAsync(bookingId, userId);
        if (booking.TotalPrice < MomoMinAmount || booking.TotalPrice > MomoMaxAmount)
        {
            throw new InvalidOperationException("MoMo chi ho tro giao dich tu 10.000 den 50.000.000 VND.");
        }

        var payment = new BookingPayment
        {
            BookingId = bookingId,
            PaymentMethod = "MoMo",
            MomoOrderId = $"BOOKING-{bookingId}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}",
            Amount = booking.TotalPrice,
            Status = PaymentStatus.Pending
        };

        return await _paymentRepository.CreateAsync(payment);
    }

    private async Task<Booking> GetUserBookingAsync(int bookingId, int userId)
    {
        var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
        if (booking is null || booking.UserId != userId)
        {
            throw new InvalidOperationException("Khong tim thay booking cua ban.");
        }

        return booking;
    }
}
