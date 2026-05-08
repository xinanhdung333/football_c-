using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Account;

public class OrdersModel : PageModel
{
    private readonly CheckoutService _checkoutService;

    public OrdersModel(CheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<Order> Orders { get; set; } = new List<Order>();

    public async Task<IActionResult> OnGetAsync()
    {
        // Get current user from session
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (!string.IsNullOrEmpty(userJson))
        {
            CurrentUser = JsonSerializer.Deserialize<User>(userJson);
        }

        if (CurrentUser == null)
        {
            return Redirect("/Account/Login");
        }

        // Load orders
        Orders = await _checkoutService.GetOrdersByUserIdAsync(CurrentUser.Id);

        return Page();
    }
}