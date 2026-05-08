using FootballBooking.BLL;
using FootballBooking.Models;
using FootballBooking.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace FootballBooking.Web.Pages.Admin;

public class ManageServicesModel : PageModel
{
    private readonly ServiceInventoryService _serviceService;
    private readonly IWebHostEnvironment _environment;

    public ManageServicesModel(ServiceInventoryService serviceService, IWebHostEnvironment environment)
    {
        _serviceService = serviceService;
        _environment = environment;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<ServiceViewModel> Services { get; set; } = new List<ServiceViewModel>();
    public string? StatusMessage { get; set; }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string Description { get; set; } = string.Empty;

    [BindProperty]
    public decimal Price { get; set; }

    [BindProperty]
    public int Quantity { get; set; }

    [BindProperty]
    public ServiceStatus Status { get; set; }

    [BindProperty]
    public int ServiceId { get; set; }

    [BindProperty]
    public IFormFile? ImageFile { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        CurrentUser = currentUser;
        await LoadServices();
        return Page();
    }

    public async Task<IActionResult> OnPostCreateServiceAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (string.IsNullOrWhiteSpace(Name) || Price <= 0 || Quantity < 0)
        {
            StatusMessage = "Vui lòng điền đầy đủ thông tin.";
            await LoadServices();
            return Page();
        }

        try
        {
            var imagePath = await SaveImageFileAsync(ImageFile);
            if (string.IsNullOrEmpty(imagePath))
            {
                StatusMessage = "Vui lòng chọn ảnh dịch vụ.";
                await LoadServices();
                return Page();
            }

            var newService = new Service
            {
                Name = Name,
                Description = Description,
                Price = Price,
                Quantity = Quantity,
                Status = Status,
                Image = imagePath
            };

            await _serviceService.CreateServiceAsync(newService);
            StatusMessage = "Dịch vụ đã được tạo thành công.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }

        await LoadServices();
        return Page();
    }

    public async Task<IActionResult> OnPostUpdateServiceAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (ServiceId <= 0 || string.IsNullOrWhiteSpace(Name) || Price <= 0 || Quantity < 0)
        {
            StatusMessage = "Vui lòng điền đầy đủ thông tin.";
            await LoadServices();
            return Page();
        }

        try
        {
            var service = await _serviceService.GetServiceByIdAsync(ServiceId);
            if (service == null)
            {
                StatusMessage = "Không tìm thấy dịch vụ.";
                await LoadServices();
                return Page();
            }

            service.Name = Name;
            service.Description = Description;
            service.Price = Price;
            service.Quantity = Quantity;
            service.Status = Status;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var imagePath = await SaveImageFileAsync(ImageFile);
                if (!string.IsNullOrEmpty(imagePath))
                {
                    service.Image = imagePath;
                }
            }

            await _serviceService.UpdateServiceAsync(service);
            StatusMessage = "Dịch vụ đã được cập nhật thành công.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }

        await LoadServices();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteServiceAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (ServiceId <= 0)
        {
            StatusMessage = "ID dịch vụ không hợp lệ.";
            await LoadServices();
            return Page();
        }

        try
        {
            await _serviceService.DeleteServiceAsync(ServiceId);
            StatusMessage = "Dịch vụ đã được xóa thành công.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }

        await LoadServices();
        return Page();
    }

    private async Task LoadServices()
    {
        var services = await _serviceService.GetServicesAsync();
        Services = services.Select(s => new ServiceViewModel
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Price = s.Price,
            Quantity = s.Quantity,
            Status = s.Status,
            Image = s.Image
        });
    }

    private User? GetCurrentUser()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        return userJson != null ? System.Text.Json.JsonSerializer.Deserialize<User>(userJson) : null;
    }

    private async Task<string?> SaveImageFileAsync(IFormFile? imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            return null;
        }

        var uploadFolder = Path.Combine(_environment.WebRootPath, "images", "services");
        Directory.CreateDirectory(uploadFolder);

        var extension = Path.GetExtension(imageFile.FileName);
        var fileName = $"service-{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadFolder, fileName);

        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(fileStream);

        return $"/images/services/{fileName}";
    }
}
