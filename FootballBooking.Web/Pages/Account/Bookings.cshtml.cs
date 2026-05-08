using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Account;

public class BookingsModel : PageModel
{
    private readonly BookingService _bookingService;
    private readonly FieldManagementService _fieldService;

    public BookingsModel(
        BookingService bookingService,
        FieldManagementService fieldService)
    {
        _bookingService = bookingService;
        _fieldService = fieldService;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<Booking> Bookings { get; set; } = new List<Booking>();
    public IEnumerable<Field> Fields { get; set; } = new List<Field>();
    public string? StatusMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!TryLoadCurrentUser())
        {
            return Redirect("/Account/Login");
        }

        await LoadDataAsync(CurrentUser!.Id);
        return Page();
    }

    public async Task<IActionResult> OnPostCancelAsync(int bookingId)
    {
        if (!TryLoadCurrentUser())
        {
            return Redirect("/Account/Login");
        }

        try
        {
            await _bookingService.CancelBookingAsync(bookingId, CurrentUser!.Id);
            StatusMessage = "Da huy lich dat san thanh cong.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        await LoadDataAsync(CurrentUser!.Id);
        return Page();
    }

    public string GetFieldName(int fieldId)
    {
        var field = Fields.FirstOrDefault(f => f.Id == fieldId);
        return field?.Name ?? $"San #{fieldId}";
    }

    public string GetFieldLocation(int fieldId)
    {
        var field = Fields.FirstOrDefault(f => f.Id == fieldId);
        return field?.Location ?? "Chua co dia chi";
    }

    private bool TryLoadCurrentUser()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            return false;
        }

        CurrentUser = JsonSerializer.Deserialize<User>(userJson);
        return CurrentUser != null;
    }

    private async Task LoadDataAsync(int userId)
    {
        Bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
        Fields = await _fieldService.GetFieldsAsync();
    }
}
