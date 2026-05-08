using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages;

[IgnoreAntiforgeryToken]
public class AddToCartModel : PageModel
{
    private readonly CartService _cartService;
    private readonly ServiceInventoryService _serviceService;

    public AddToCartModel(CartService cartService, ServiceInventoryService serviceService)
    {
        _cartService = cartService;
        _serviceService = serviceService;
    }

    public Service? Service { get; set; }
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    [BindProperty]
    public int Quantity { get; set; }

    public async Task<IActionResult> OnGetAsync(int serviceId)
    {
        Service = await _serviceService.GetServiceByIdAsync(serviceId);
        if (Service == null)
        {
            ErrorMessage = "Dịch vụ không tồn tại.";
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int serviceId)
    {
        Service = await _serviceService.GetServiceByIdAsync(serviceId);
        if (Service == null)
        {
            ErrorMessage = "Dịch vụ không tồn tại.";
            return Page();
        }

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
            await _cartService.AddToCartAsync(user.Id, serviceId, Quantity);
            SuccessMessage = "Đã thêm vào giỏ hàng thành công!";
            return Page();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
    }
}
