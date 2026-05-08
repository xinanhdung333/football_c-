using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Feedback;

public class CreateModel : PageModel
{
    private readonly FeedbackService _feedbackService;
    private readonly BookingService _bookingService;
    private readonly FieldManagementService _fieldService;

    public CreateModel(FeedbackService feedbackService, BookingService bookingService, FieldManagementService fieldService)
    {
        _feedbackService = feedbackService;
        _bookingService = bookingService;
        _fieldService = fieldService;
    }

    public User? CurrentUser { get; set; }
    public Field? BookingField { get; set; }

    [BindProperty]
    public int BookingId { get; set; }

    [BindProperty]
    [Range(1, 5, ErrorMessage = "Vui lòng chọn đánh giá từ 1 đến 5 sao")]
    public byte Rating { get; set; }

    [BindProperty]
    [StringLength(500, ErrorMessage = "Bình luận không được vượt quá 500 ký tự")]
    public string Comment { get; set; } = string.Empty;

    public string? StatusMessage { get; set; }

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

        var booking = await _bookingService.GetBookingByIdAsync(bookingId);
        if (booking == null || booking.UserId != CurrentUser.Id)
        {
            return RedirectToPage("/Account/Profile");
        }

        BookingId = bookingId;
        BookingField = await _fieldService.GetFieldByIdAsync(booking.FieldId);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
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

        var booking = await _bookingService.GetBookingByIdAsync(BookingId);
        if (booking == null || booking.UserId != CurrentUser.Id)
        {
            return RedirectToPage("/Account/Profile");
        }

        if (!ModelState.IsValid)
        {
            BookingField = await _fieldService.GetFieldByIdAsync(booking.FieldId);
            return Page();
        }

        try
        {
            await _feedbackService.SubmitFeedbackAsync(CurrentUser.Id, BookingId, null, Comment, Rating);
            StatusMessage = "Cảm ơn bạn đã đánh giá! Đánh giá của bạn đã được lưu.";
            return RedirectToPage("/Account/Profile");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Lỗi: {ex.Message}");
            BookingField = await _fieldService.GetFieldByIdAsync(booking.FieldId);
            return Page();
        }
    }
}
