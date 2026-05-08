using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages;

public class UpdateCartModel : PageModel
{
    private readonly CartService _cartService;
    private readonly ServiceInventoryService _serviceService;

    public UpdateCartModel(CartService cartService, ServiceInventoryService serviceService)
    {
        _cartService = cartService;
        _serviceService = serviceService;
    }

    public IEnumerable<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
    public decimal CartTotal { get; set; }
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    [BindProperty]
    public int CartItemId { get; set; }

    [BindProperty]
    public int Quantity { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadCartData();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            ErrorMessage = "Vui lòng đăng nhập trước.";
            await LoadCartData();
            return Page();
        }

        var user = JsonSerializer.Deserialize<User>(userJson);
        if (user == null)
        {
            ErrorMessage = "Phiên đăng nhập không hợp lệ.";
            await LoadCartData();
            return Page();
        }

        try
        {
            await _cartService.UpdateCartItemQuantityAsync(user.Id, CartItemId, Quantity);
            SuccessMessage = "Cập nhật giỏ hàng thành công.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        await LoadCartData();
        return Page();
    }

    public async Task<IActionResult> OnPostRemoveAsync(int cartItemId)
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            ErrorMessage = "Vui lòng đăng nhập trước.";
            await LoadCartData();
            return Page();
        }

        var user = JsonSerializer.Deserialize<User>(userJson);
        if (user == null)
        {
            ErrorMessage = "Phiên đăng nhập không hợp lệ.";
            await LoadCartData();
            return Page();
        }

        try
        {
            await _cartService.RemoveCartItemAsync(user.Id, cartItemId);
            SuccessMessage = "Đã xóa sản phẩm khỏi giỏ hàng.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        await LoadCartData();
        return Page();
    }

    private async Task LoadCartData()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (!string.IsNullOrEmpty(userJson))
        {
            var user = JsonSerializer.Deserialize<User>(userJson);
            if (user != null)
            {
                var cartItems = await _cartService.GetCartItemsAsync(user.Id);
                CartItems = await Task.WhenAll(cartItems.Select(async item =>
                {
                    var service = await _serviceService.GetServiceByIdAsync(item.ServiceId);
                    return new CartItemViewModel
                    {
                        Id = item.Id,
                        ServiceId = item.ServiceId,
                        ServiceName = service?.Name ?? "Dịch vụ không tồn tại",
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                }));

                CartTotal = await _cartService.CalculateCartTotalAsync(user.Id);
            }
        }
    }

    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
