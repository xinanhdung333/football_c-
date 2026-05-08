using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Admin;

public class InventoryModel : PageModel
{
    private readonly ServiceInventoryService _serviceService;

    public InventoryModel(ServiceInventoryService serviceService)
    {
        _serviceService = serviceService;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<Service> Services { get; set; } = new List<Service>();

    [BindProperty]
    public int ServiceId { get; set; }

    [BindProperty]
    public int Quantity { get; set; }

    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        CurrentUser = currentUser;
        Services = await _serviceService.GetServicesAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (ServiceId <= 0 || Quantity < 0)
        {
            StatusMessage = "Số lượng phải là số không âm và dịch vụ phải được chọn.";
        }
        else
        {
            await _serviceService.UpdateStockAsync(ServiceId, Quantity);
            StatusMessage = "Cập nhật tồn kho thành công.";
        }

        CurrentUser = currentUser;
        Services = await _serviceService.GetServicesAsync();
        return Page();
    }

    private User? GetCurrentUser()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        return string.IsNullOrEmpty(userJson)
            ? null
            : JsonSerializer.Deserialize<User>(userJson);
    }
}
