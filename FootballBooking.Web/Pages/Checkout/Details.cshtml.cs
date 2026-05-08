using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Checkout;

public class DetailsModel : PageModel
{
    private readonly CheckoutService _checkoutService;
    private readonly ServiceInventoryService _serviceService;

    public DetailsModel(CheckoutService checkoutService, ServiceInventoryService serviceService)
    {
        _checkoutService = checkoutService;
        _serviceService = serviceService;
    }

    public User? CurrentUser { get; set; }
    public Order? Order { get; set; }
    public IEnumerable<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public IEnumerable<Service> Services { get; set; } = new List<Service>();

    public async Task<IActionResult> OnGetAsync(int orderId)
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

        Order = await _checkoutService.GetOrderByIdAsync(orderId);
        if (Order == null || Order.UserId != CurrentUser.Id)
        {
            return RedirectToPage("/Account/Profile");
        }

        OrderItems = await _checkoutService.GetOrderItemsByOrderIdAsync(orderId);
        Services = await _serviceService.GetServicesAsync();
        return Page();
    }
}
