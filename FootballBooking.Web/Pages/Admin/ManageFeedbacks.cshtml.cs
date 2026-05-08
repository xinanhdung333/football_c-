using FootballBooking.BLL;
using FootballBooking.Models;
using FootballBooking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FootballBooking.Web.Pages.Admin;

public class ManageFeedbacksModel : PageModel
{
    private readonly FeedbackService _feedbackService;

    public ManageFeedbacksModel(FeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<FeedbackViewModel> Feedbacks { get; set; } = new List<FeedbackViewModel>();
    public string? StatusMessage { get; set; }

    [BindProperty]
    public int FeedbackId { get; set; }

    [BindProperty]
    public string ReplyMessage { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        CurrentUser = currentUser;
        await LoadFeedbacks();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteFeedbackAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (FeedbackId <= 0)
        {
            StatusMessage = "ID phản hồi không hợp lệ.";
            await LoadFeedbacks();
            return Page();
        }

        try
        {
            await _feedbackService.DeleteFeedbackAsync(FeedbackId);
            StatusMessage = "Phản hồi đã được xóa thành công.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }

        await LoadFeedbacks();
        return Page();
    }

    public async Task<IActionResult> OnPostReplyFeedbackAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (FeedbackId <= 0)
        {
            StatusMessage = "ID phản hồi không hợp lệ.";
            await LoadFeedbacks();
            return Page();
        }

        try
        {
            await _feedbackService.ReplyFeedbackAsync(FeedbackId, currentUser.Id, ReplyMessage);
            StatusMessage = "Đã gửi phản hồi tới người dùng.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }

        await LoadFeedbacks();
        return Page();
    }

    private async Task LoadFeedbacks()
    {
        var feedbacks = await _feedbackService.GetAllFeedbacksWithDetailsAsync();
        Feedbacks = feedbacks.Select(f => new FeedbackViewModel
        {
            Id = f.Id,
            UserFullName = f.UserName,
            UserUsername = f.UserEmail.Split('@')[0],
            BookingId = f.BookingId,
            ServiceId = f.ServiceId,
            Message = f.Message,
            Rating = f.Rating,
            CreatedAt = f.CreatedAt,
            ReplyMessage = f.ReplyMessage,
            RepliedByName = f.RepliedByName,
            RepliedAt = f.RepliedAt
        });
    }

    private User? GetCurrentUser()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        return userJson != null ? System.Text.Json.JsonSerializer.Deserialize<User>(userJson) : null;
    }
}
