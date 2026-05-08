using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Text.Json;

namespace FootballBooking.Web.Pages;

public class ServicesModel : PageModel
{
    private readonly ServiceInventoryService _serviceService;
    private readonly CartService _cartService;
    private readonly FeedbackService _feedbackService;
    private readonly ServiceDiscountService _discountService;

    public ServicesModel(
        ServiceInventoryService serviceService,
        CartService cartService,
        FeedbackService feedbackService,
        ServiceDiscountService discountService)
    {
        _serviceService = serviceService;
        _cartService = cartService;
        _feedbackService = feedbackService;
        _discountService = discountService;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<Service> Services { get; set; } = new List<Service>();
    public IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();
    public decimal CartTotal { get; set; }
    public int CartItemCount { get; set; }
    public IReadOnlyDictionary<int, ServiceRatingSummary> RatingSummaries { get; set; } = new Dictionary<int, ServiceRatingSummary>();
    public IReadOnlyDictionary<int, ServicePriceQuote> PriceQuotes { get; set; } = new Dictionary<int, ServicePriceQuote>();

    public async Task OnGetAsync()
    {
        // Get current user from session
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (!string.IsNullOrEmpty(userJson))
        {
            CurrentUser = JsonSerializer.Deserialize<User>(userJson);
        }

        // Load services
        Services = await _serviceService.GetServicesAsync();
        PriceQuotes = (await Task.WhenAll(Services.Select(async service => await _discountService.GetPriceQuoteAsync(service))))
            .ToDictionary(quote => quote.ServiceId);

        var feedbacks = await _feedbackService.GetAllFeedbacksWithDetailsAsync();
        RatingSummaries = feedbacks
            .Where(feedback => feedback.ServiceId.HasValue)
            .GroupBy(feedback => feedback.ServiceId!.Value)
            .ToDictionary(
                group => group.Key,
                group => new ServiceRatingSummary(
                    Math.Round(group.Average(feedback => feedback.Rating), 1),
                    group.Count()));

        if (CurrentUser != null)
        {
            CartItems = await _cartService.GetCartItemsAsync(CurrentUser.Id);
            CartTotal = await _cartService.CalculateCartTotalAsync(CurrentUser.Id);
            CartItemCount = CartItems.Sum(item => item.Quantity);
        }
    }

    public ServiceRatingSummary GetRatingSummary(int serviceId)
    {
        return RatingSummaries.TryGetValue(serviceId, out var summary)
            ? summary
            : new ServiceRatingSummary(0, 0);
    }

    public ServicePriceQuote GetPriceQuote(Service service)
    {
        return PriceQuotes.TryGetValue(service.Id, out var quote)
            ? quote
            : new ServicePriceQuote(service.Id, service.Price, service.Price, 0, null);
    }
}

public record ServiceRatingSummary(double Average, int Count);
