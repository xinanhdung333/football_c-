using FootballBooking.BLL;
using FootballBooking.Models;
using FootballBooking.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace FootballBooking.Web.Pages.Admin;

public class ManageFieldsModel : PageModel
{
    private readonly FieldManagementService _fieldService;
    private readonly IWebHostEnvironment _environment;

    public ManageFieldsModel(FieldManagementService fieldService, IWebHostEnvironment environment)
    {
        _fieldService = fieldService;
        _environment = environment;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<FieldViewModel> Fields { get; set; } = new List<FieldViewModel>();
    public string? StatusMessage { get; set; }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string Location { get; set; } = string.Empty;

    [BindProperty]
    public decimal PricePerHour { get; set; }

    [BindProperty]
    public TimeSpan? PeakStartTime { get; set; }

    [BindProperty]
    public TimeSpan? PeakEndTime { get; set; }

    [BindProperty]
    public decimal? PeakPricePerHour { get; set; }

    [BindProperty]
    public FieldStatus Status { get; set; }

    [BindProperty]
    public int FieldId { get; set; }

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
        await LoadFields();
        return Page();
    }

    public async Task<IActionResult> OnPostCreateFieldAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Location) || PricePerHour <= 0)
        {
            StatusMessage = "Vui lòng điền đầy đủ thông tin.";
            await LoadFields();
            return Page();
        }

        try
        {
            var imagePath = await SaveImageFileAsync(ImageFile);
            if (string.IsNullOrEmpty(imagePath))
            {
                StatusMessage = "Vui lòng chọn ảnh sân bóng.";
                await LoadFields();
                return Page();
            }

            var newField = new Field
            {
                Name = Name,
                Location = Location,
                PricePerHour = PricePerHour,
                PeakStartTime = PeakStartTime,
                PeakEndTime = PeakEndTime,
                PeakPricePerHour = PeakPricePerHour,
                Status = Status,
                Image = imagePath
            };

            await _fieldService.CreateFieldAsync(newField);
            StatusMessage = "Sân bóng đã được tạo thành công.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }

        await LoadFields();
        return Page();
    }

    public async Task<IActionResult> OnPostUpdateFieldAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (FieldId <= 0 || string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Location) || PricePerHour <= 0)
        {
            StatusMessage = "Vui lòng điền đầy đủ thông tin.";
            await LoadFields();
            return Page();
        }

        try
        {
            var field = await _fieldService.GetFieldByIdAsync(FieldId);
            if (field == null)
            {
                StatusMessage = "Không tìm thấy sân bóng.";
                await LoadFields();
                return Page();
            }

            field.Name = Name;
            field.Location = Location;
            field.PricePerHour = PricePerHour;
            field.PeakStartTime = PeakStartTime;
            field.PeakEndTime = PeakEndTime;
            field.PeakPricePerHour = PeakPricePerHour;
            field.Status = Status;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var imagePath = await SaveImageFileAsync(ImageFile);
                if (!string.IsNullOrEmpty(imagePath))
                {
                    field.Image = imagePath;
                }
            }

            await _fieldService.UpdateFieldAsync(field);
            StatusMessage = "Sân bóng đã được cập nhật thành công.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }

        await LoadFields();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteFieldAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (FieldId <= 0)
        {
            StatusMessage = "ID sân bóng không hợp lệ.";
            await LoadFields();
            return Page();
        }

        try
        {
            await _fieldService.DeleteFieldAsync(FieldId);
            StatusMessage = "Sân bóng đã được xóa thành công.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }

        await LoadFields();
        return Page();
    }

    private async Task LoadFields()
    {
        var fields = await _fieldService.GetFieldsAsync();
        Fields = fields.Select(f => new FieldViewModel
        {
            Id = f.Id,
            Name = f.Name,
            Location = f.Location,
            PricePerHour = f.PricePerHour,
            PeakStartTime = f.PeakStartTime,
            PeakEndTime = f.PeakEndTime,
            PeakPricePerHour = f.PeakPricePerHour,
            Status = f.Status,
            Image = f.Image
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

        var uploadFolder = Path.Combine(_environment.WebRootPath, "images", "fields");
        Directory.CreateDirectory(uploadFolder);

        var extension = Path.GetExtension(imageFile.FileName);
        var fileName = $"field-{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadFolder, fileName);

        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(fileStream);

        return $"/images/fields/{fileName}";
    }
}
