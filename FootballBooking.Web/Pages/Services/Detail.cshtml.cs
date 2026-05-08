using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FootballBooking.Web.Pages.Services;

public class DetailModel : PageModel
{
    private readonly ServiceInventoryService _serviceService;
    private readonly FeedbackRepository _feedbackRepository;

    public Service? Service { get; set; }
    public List<ServiceReview> Reviews { get; set; } = new();

    public DetailModel(
        ServiceInventoryService serviceService,
        FeedbackRepository feedbackRepository)
    {
        _serviceService = serviceService;
        _feedbackRepository = feedbackRepository;
    }

    public async Task<IActionResult> OnGetAsync(int serviceId)
    {
        var services = await _serviceService.GetServicesAsync();
        Service = services?.FirstOrDefault(s => s.Id == serviceId);

        if (Service == null)
            return NotFound();

        // Get reviews for this service
        var feedbacks = await _feedbackRepository.GetFeedbacksByServiceIdAsync(serviceId);
        if (feedbacks?.Any() == true)
        {
            Reviews = feedbacks
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new ServiceReview
                {
                    Rating = f.Rating,
                    Message = f.Message,
                    CreatedAt = f.CreatedAt
                })
                .ToList();
        }

        return Page();
    }
}

public class ServiceReview
{
    public string? UserName { get; set; }
    public byte Rating { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
}
