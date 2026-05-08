using FootballBooking.BLL;
using FootballBooking.Models;
using FootballBooking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Admin;

public class ManageBookingsModel : PageModel
{
    private readonly BookingService _bookingService;

    public ManageBookingsModel(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<BookingViewModel> Bookings { get; set; } = new List<BookingViewModel>();
    public string? StatusMessage { get; set; }

    [BindProperty]
    public BookingStatus Status { get; set; }

    [BindProperty]
    public int BookingId { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!TryLoadCurrentUser())
        {
            return RedirectToPage("/Account/Login");
        }

        CurrentUser = GetCurrentUser();
        await LoadBookings();
        return Page();
    }

    public async Task<IActionResult> OnGetExportExcelAsync()
    {
        if (!TryLoadCurrentUser())
        {
            return RedirectToPage("/Account/Login");
        }

        await LoadBookings();

        var csv = new StringBuilder();
        csv.AppendLine("Booking Id,Khách hàng,Sân,Ngày đặt,Giờ bắt đầu,Giờ kết thúc,Tổng tiền,Trạng thái");

        foreach (var booking in Bookings)
        {
            csv.AppendLine($"{booking.Id},\"{booking.UserFullName}\",\"{booking.FieldName}\",{booking.BookingDate:dd/MM/yyyy},{booking.StartTime},{booking.EndTime},{booking.TotalAmount:N0},\"{booking.Status}\"");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", "DanhSachDatSan.csv");
    }

    public async Task<IActionResult> OnPostUpdateBookingAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (BookingId <= 0)
        {
            StatusMessage = "ID đặt sân không hợp lệ.";
            await LoadBookings();
            return Page();
        }

        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(BookingId);
            if (booking == null)
            {
                StatusMessage = "Không tìm thấy đơn đặt sân.";
                await LoadBookings();
                return Page();
            }

            booking.Status = Status;
            await _bookingService.UpdateBookingAsync(booking);
            StatusMessage = "Đơn đặt sân đã được cập nhật thành công.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }

        await LoadBookings();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteBookingAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (BookingId <= 0)
        {
            StatusMessage = "ID đặt sân không hợp lệ.";
            await LoadBookings();
            return Page();
        }

        try
        {
            await _bookingService.DeleteBookingAsync(BookingId);
            StatusMessage = "Đơn đặt sân đã được xóa thành công.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }

        await LoadBookings();
        return Page();
    }

    private async Task LoadBookings()
    {
        var bookings = await _bookingService.GetAllBookingsWithDetailsAsync();
        Bookings = bookings.Select(b => new BookingViewModel
        {
            Id = b.Id,
            UserFullName = b.UserName,
            UserUsername = b.UserEmail.Split('@')[0],
            FieldName = b.FieldName,
            BookingDate = b.BookingDate,
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            TotalAmount = b.TotalPrice,
            Status = b.Status
        });
    }

    private bool TryLoadCurrentUser()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
            return false;

        var currentUser = JsonSerializer.Deserialize<User>(userJson);
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
            return false;

        CurrentUser = currentUser;
        return true;
    }

    private User? GetCurrentUser()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        return userJson != null ? JsonSerializer.Deserialize<User>(userJson) : null;
    }
}