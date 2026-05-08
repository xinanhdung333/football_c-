using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Bookings;

public class DetailsModel : PageModel
{
    private readonly BookingService _bookingService;
    private readonly FieldManagementService _fieldService;
    private readonly BookingPaymentService _paymentService;

    public DetailsModel(BookingService bookingService, FieldManagementService fieldService, BookingPaymentService paymentService)
    {
        _bookingService = bookingService;
        _fieldService = fieldService;
        _paymentService = paymentService;
    }

    public User? CurrentUser { get; set; }
    public Booking? Booking { get; set; }
    public Field? Field { get; set; }
    public BookingPayment? LatestPayment { get; set; }
    public string? StatusMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int bookingId)
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            return RedirectToPage("/Account/Login");
        }

        CurrentUser = JsonSerializer.Deserialize<User>(userJson);
        if (CurrentUser == null)
        {
            return RedirectToPage("/Account/Login");
        }

        if (!await LoadBookingAsync(bookingId))
        {
            return RedirectToPage("/Account/Profile");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostPayCashAsync(int bookingId)
    {
        if (!TryLoadCurrentUser())
        {
            return RedirectToPage("/Account/Login");
        }

        try
        {
            await _paymentService.CreateCashPaymentAsync(bookingId, CurrentUser!.Id);
            StatusMessage = "Da ghi nhan thanh toan tien mat va xac nhan booking.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        if (!await LoadBookingAsync(bookingId))
        {
            return RedirectToPage("/Account/Profile");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostPayMomoAsync(int bookingId)
    {
        if (!TryLoadCurrentUser())
        {
            return RedirectToPage("/Account/Login");
        }

        try
        {
            await _paymentService.CreateMomoPendingPaymentAsync(bookingId, CurrentUser!.Id);
            StatusMessage = "Da tao giao dich MoMo dang cho xu ly. Can cau hinh MoMo sandbox de redirect va xac thuc IPN.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        if (!await LoadBookingAsync(bookingId))
        {
            return RedirectToPage("/Account/Profile");
        }

        return Page();
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

    private async Task<bool> LoadBookingAsync(int bookingId)
    {
        Booking = await _bookingService.GetBookingByIdAsync(bookingId);
        if (Booking == null || Booking.UserId != CurrentUser?.Id)
        {
            return false;
        }

        Field = await _fieldService.GetFieldByIdAsync(Booking.FieldId);
        LatestPayment = await _paymentService.GetLatestPaymentAsync(Booking.Id);
        return true;
    }
}
