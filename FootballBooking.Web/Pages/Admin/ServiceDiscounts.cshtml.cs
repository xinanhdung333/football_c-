using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Admin;

public class ServiceDiscountsModel : PageModel
{
    private readonly ServiceDiscountService _discountService;
    private readonly ServiceInventoryService _serviceService;

    public ServiceDiscountsModel(ServiceDiscountService discountService, ServiceInventoryService serviceService)
    {
        _discountService = discountService;
        _serviceService = serviceService;
    }

    public IEnumerable<ServiceDiscount> Discounts { get; set; } = new List<ServiceDiscount>();
    public IEnumerable<Service> Services { get; set; } = new List<Service>();
    public string? StatusMessage { get; set; }

    [BindProperty]
    public int? ServiceId { get; set; }

    [BindProperty]
    public TimeSpan StartTime { get; set; }

    [BindProperty]
    public TimeSpan EndTime { get; set; }

    [BindProperty]
    public decimal Multiplier { get; set; }

    [BindProperty]
    public string? Note { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!IsAdmin())
        {
            return RedirectToPage("/Account/Login");
        }

        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        if (!IsAdmin())
        {
            return RedirectToPage("/Account/Login");
        }

        try
        {
            await _discountService.CreateDiscountAsync(new ServiceDiscount
            {
                ServiceId = ServiceId,
                StartTime = StartTime,
                EndTime = EndTime,
                Multiplier = Multiplier,
                Note = Note,
                IsActive = true
            });
            StatusMessage = "Da tao rule giam gia dich vu.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Loi: {ex.Message}";
        }

        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        if (!IsAdmin())
        {
            return RedirectToPage("/Account/Login");
        }

        await _discountService.DeleteDiscountAsync(id);
        StatusMessage = "Da xoa rule giam gia.";
        await LoadDataAsync();
        return Page();
    }

    public string GetServiceName(int? serviceId)
    {
        if (!serviceId.HasValue)
        {
            return "Tat ca dich vu";
        }

        return Services.FirstOrDefault(service => service.Id == serviceId.Value)?.Name ?? $"Dich vu #{serviceId}";
    }

    private async Task LoadDataAsync()
    {
        Services = await _serviceService.GetServicesAsync();
        Discounts = await _discountService.GetDiscountsAsync();
    }

    private bool IsAdmin()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            return false;
        }

        var user = JsonSerializer.Deserialize<User>(userJson);
        return user?.Role is UserRole.Admin or UserRole.Boss;
    }
}
