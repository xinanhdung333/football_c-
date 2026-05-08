using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages;

public class RemoveFromCartModel : PageModel
{
    private readonly CartService _cartService;

    public RemoveFromCartModel(CartService cartService)
    {
        _cartService = cartService;
    }

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int cartItemId)
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            ErrorMessage = "Vui lòng đăng nhập trước.";
            return Page();
        }

        var user = JsonSerializer.Deserialize<User>(userJson);
        if (user == null)
        {
            ErrorMessage = "Phiên đăng nhập không hợp lệ.";
            return Page();
        }

        try
        {
            await _cartService.RemoveCartItemAsync(user.Id, cartItemId);
            SuccessMessage = "Đã xóa sản phẩm khỏi giỏ hàng!";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        return Page();
    }
}
