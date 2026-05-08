using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Text.Json;

namespace FootballBooking.Web.Pages;

public class IndexModel : PageModel
{
    private readonly FieldManagementService _fieldService;
    private readonly CartService _cartService;
    private readonly BookingService _bookingService;

    public IndexModel(
        FieldManagementService fieldService,
        CartService cartService,
        BookingService bookingService)
    {
        _fieldService = fieldService;
        _cartService = cartService;
        _bookingService = bookingService;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<Field> Fields { get; set; } = new List<Field>();
    public IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();
    public decimal CartTotal { get; set; }
    public int CartItemCount { get; set; }
    public int BookingCount { get; set; }

    public async Task OnGetAsync()
    {
        // Get current user from session
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (!string.IsNullOrEmpty(userJson))
        {
            CurrentUser = JsonSerializer.Deserialize<User>(userJson);
        }

        // Load data
        Fields = await _fieldService.GetFieldsAsync();

        if (CurrentUser != null)
        {
            CartItems = await _cartService.GetCartItemsAsync(CurrentUser.Id);
            CartTotal = await _cartService.CalculateCartTotalAsync(CurrentUser.Id);
            CartItemCount = CartItems.Sum(item => item.Quantity);
            BookingCount = (await _bookingService.GetBookingsByUserIdAsync(CurrentUser.Id)).Count();
        }
    }
}
