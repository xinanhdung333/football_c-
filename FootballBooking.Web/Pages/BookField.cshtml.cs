using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages;

public class BookFieldModel : PageModel
{
    private readonly FieldManagementService _fieldService;
    private readonly BookingService _bookingService;

    public BookFieldModel(FieldManagementService fieldService, BookingService bookingService)
    {
        _fieldService = fieldService;
        _bookingService = bookingService;
    }

    public IEnumerable<Field> Fields { get; set; } = new List<Field>();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    [BindProperty]
    public BookingInput Input { get; set; } = new();

    public class BookingInput
    {
        public int FieldId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Note { get; set; }
    }

    public async Task OnGetAsync()
    {
        Fields = await _fieldService.GetFieldsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Fields = await _fieldService.GetFieldsAsync();
            return Page();
        }

        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            ErrorMessage = "Vui lòng đăng nhập trước";
            Fields = await _fieldService.GetFieldsAsync();
            return Page();
        }

        var user = JsonSerializer.Deserialize<User>(userJson);
        if (user == null)
        {
            ErrorMessage = "Phiên đăng nhập không hợp lệ";
            Fields = await _fieldService.GetFieldsAsync();
            return Page();
        }

        try
        {
            var bookingId = await _bookingService.CreateBookingAsync(
                user.Id,
                Input.FieldId,
                Input.BookingDate,
                Input.StartTime,
                Input.EndTime,
                Input.Note);

            SuccessMessage = $"Đặt sân thành công! Mã đặt sân: {bookingId}";
            Fields = await _fieldService.GetFieldsAsync();
            return Page();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            Fields = await _fieldService.GetFieldsAsync();
            return Page();
        }
    }
}
