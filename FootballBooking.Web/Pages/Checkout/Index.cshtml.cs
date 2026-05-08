using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Checkout;

public class IndexModel : PageModel
{
    private readonly CartService _cartService;
    private readonly ServiceInventoryService _serviceService;
    private readonly CheckoutService _checkoutService;

    public IndexModel(
        CartService cartService,
        ServiceInventoryService serviceService,
        CheckoutService checkoutService)
    {
        _cartService = cartService;
        _serviceService = serviceService;
        _checkoutService = checkoutService;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();
    public IEnumerable<Service> Services { get; set; } = new List<Service>();
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public string? ErrorMessage { get; set; }

    [BindProperty]
    public string PaymentMethod { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        // Check if user is logged in
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

        // Load cart items
        CartItems = await _cartService.GetCartItemsAsync(CurrentUser.Id);
        Services = await _serviceService.GetServicesAsync();

        if (!CartItems.Any())
        {
            // Don't redirect, just show empty cart message
            Subtotal = 0;
            Total = 0;
            return Page();
        }

        Subtotal = await _cartService.CalculateCartTotalAsync(CurrentUser.Id);
        Total = Subtotal; // No additional fees for now

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Check if user is logged in
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

        // Load cart items
        CartItems = await _cartService.GetCartItemsAsync(CurrentUser.Id);
        Services = await _serviceService.GetServicesAsync();

        if (!CartItems.Any())
        {
            ErrorMessage = "Giỏ hàng trống.";
            return Page();
        }

        if (string.IsNullOrEmpty(PaymentMethod))
        {
            ErrorMessage = "Vui lòng chọn phương thức thanh toán.";
            return Page();
        }

        try
        {
            var cart = await _cartService.GetOrCreateCartAsync(CurrentUser.Id);
            var total = await _cartService.CalculateCartTotalAsync(CurrentUser.Id);

            // Convert cart items to order items
            var orderItems = CartItems.Select(item => new OrderItem
            {
                ServiceId = item.ServiceId,
                Quantity = item.Quantity,
                Price = item.Price,
                Status = OrderStatus.Pending
            }).ToList();

            // Determine payment status
            var paymentStatus = PaymentMethod == "Tiền mặt" ? PaymentStatus.Success : PaymentStatus.Pending;

            // Create order and payment
            var orderId = await _checkoutService.CreateOrderWithPaymentAsync(
                CurrentUser.Id,
                cart.Id,
                total,
                PaymentMethod,
                orderItems,
                paymentStatus);

            // Clear cart
            await _cartService.ClearCartAsync(CurrentUser.Id);

            // Redirect to success page
            return RedirectToPage("/Checkout/Success", new { orderId });
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Lỗi thanh toán: {ex.Message}";
            Subtotal = await _cartService.CalculateCartTotalAsync(CurrentUser.Id);
            Total = Subtotal;
            return Page();
        }
    }
}