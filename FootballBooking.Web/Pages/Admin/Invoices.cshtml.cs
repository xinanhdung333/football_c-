using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Admin;

public class InvoicesModel : PageModel
{
    private readonly BookingService _bookingService;

    public InvoicesModel(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<BookingInvoiceViewModel> Bookings { get; set; } = new List<BookingInvoiceViewModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        if (!TryLoadCurrentUser())
        {
            return RedirectToPage("/Account/Login");
        }

        await LoadBookingsAsync();
        return Page();
    }

    public async Task<IActionResult> OnGetExportExcelAsync()
    {
        if (!TryLoadCurrentUser())
        {
            return RedirectToPage("/Account/Login");
        }

        await LoadBookingsAsync();

        var csv = new StringBuilder();
        csv.AppendLine("Booking Id,Khách hàng,Email,Field,Ngày đặt,Giờ bắt đầu,Giờ kết thúc,Tổng tiền,Trạng thái");

        foreach (var booking in Bookings)
        {
            csv.AppendLine($"{booking.Id},\"{booking.UserName}\",\"{booking.UserEmail}\",\"{booking.FieldName}\",{booking.BookingDate:dd/MM/yyyy},{booking.StartTime},{booking.EndTime},{booking.TotalAmount:N0},\"{booking.Status}\"");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", "DanhSachHoaDonDatSan.csv");
    }

    private bool TryLoadCurrentUser()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
            return false;

        CurrentUser = JsonSerializer.Deserialize<User>(userJson);
        if (CurrentUser == null || (CurrentUser.Role != UserRole.Admin && CurrentUser.Role != UserRole.Boss))
            return false;

        return true;
    }

    private async Task LoadBookingsAsync()
    {
        var bookings = await _bookingService.GetAllBookingsWithDetailsAsync();

        Bookings = bookings.Select(b => new BookingInvoiceViewModel
        {
            Id = b.Id,
            UserName = b.UserName,
            UserEmail = b.UserEmail,
            FieldName = b.FieldName,
            BookingDate = b.BookingDate,
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            TotalAmount = b.TotalPrice,
            Status = b.Status
        });
    }

    public class BookingInvoiceViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; }
    }
}
