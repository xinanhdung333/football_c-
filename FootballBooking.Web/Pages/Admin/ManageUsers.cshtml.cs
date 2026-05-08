using FootballBooking.BLL;
using FootballBooking.Models;
using FootballBooking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FootballBooking.Web.Pages.Admin;

public class ManageUsersModel : PageModel
{
    private readonly UserManagementService _userService;

    public ManageUsersModel(UserManagementService userService)
    {
        _userService = userService;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<UserViewModel> Users { get; set; } = new List<UserViewModel>();
    public string? StatusMessage { get; set; }

    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string FullName { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    public UserRole Role { get; set; }

    [BindProperty]
    public int UserId { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        CurrentUser = currentUser;
        await LoadUsers();
        return Page();
    }

    public async Task<IActionResult> OnPostCreateUserAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Password))
        {
            StatusMessage = "Vui lòng điền đầy đủ thông tin.";
            await LoadUsers();
            return Page();
        }

        try
        {
            var newUser = new User
            {
                Name = FullName,
                Email = Email,
                Phone = "", // Default empty phone
                Password = Password,
                Role = Role,
                CreatedAt = DateTime.Now
            };

            await _userService.CreateUserAsync(newUser);
            StatusMessage = "Người dùng đã được tạo thành công.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }

        await LoadUsers();
        return Page();
    }

    public async Task<IActionResult> OnPostUpdateUserAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (UserId <= 0 || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(FullName))
        {
            StatusMessage = "Vui lòng điền đầy đủ thông tin.";
            await LoadUsers();
            return Page();
        }

        try
        {
            var user = await _userService.GetUserByIdAsync(UserId);
            if (user == null)
            {
                StatusMessage = "Không tìm thấy người dùng.";
                await LoadUsers();
                return Page();
            }

            user.Name = FullName;
            user.Email = Email;
            user.Role = Role;

            await _userService.UpdateUserAsync(user);
            StatusMessage = "Người dùng đã được cập nhật thành công.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }

        await LoadUsers();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteUserAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null || (currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        if (UserId <= 0)
        {
            StatusMessage = "ID người dùng không hợp lệ.";
            await LoadUsers();
            return Page();
        }

        try
        {
            await _userService.DeleteUserAsync(UserId);
            StatusMessage = "Người dùng đã được xóa thành công.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }

        await LoadUsers();
        return Page();
    }

    private async Task LoadUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        Users = users.Select(u => new UserViewModel
        {
            Id = u.Id,
            Username = u.Email.Split('@')[0], // Use email prefix as username
            Email = u.Email,
            FullName = u.Name,
            Role = u.Role,
            CreatedAt = u.CreatedAt
        });
    }

    private User? GetCurrentUser()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        return userJson != null ? System.Text.Json.JsonSerializer.Deserialize<User>(userJson) : null;
    }
}