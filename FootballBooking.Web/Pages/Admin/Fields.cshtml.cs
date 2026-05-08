using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Admin;

public class FieldsModel : PageModel
{
    private readonly FieldManagementService _fieldService;

    public FieldsModel(FieldManagementService fieldService)
    {
        _fieldService = fieldService;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<Field> Fields { get; set; } = new List<Field>();

    [BindProperty]
    public int FieldId { get; set; }

    [BindProperty]
    public decimal PricePerHour { get; set; }

    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        CurrentUser = currentUser;
        Fields = await _fieldService.GetFieldsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (FieldId <= 0 || PricePerHour <= 0)
        {
            StatusMessage = "Giá phải lớn hơn 0 và sân phải được chọn.";
        }
        else
        {
            await _fieldService.UpdateFieldPriceAsync(FieldId, PricePerHour);
            StatusMessage = "Cập nhật giá sân thành công.";
        }

        CurrentUser = currentUser;
        Fields = await _fieldService.GetFieldsAsync();
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
