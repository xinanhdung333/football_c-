using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Account;

public class LoginModel : PageModel
{
    private readonly AuthenticationService _authService;

    public LoginModel(AuthenticationService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var user = await _authService.AuthenticateAsync(Input.Email, Input.Password);
            if (user == null)
            {
                ErrorMessage = "Email hoặc mật khẩu không đúng.";
                return Page();
            }

            // Store user in session
            HttpContext.Session.SetString("CurrentUser", JsonSerializer.Serialize(user));

            if (user.Role == UserRole.Admin || user.Role == UserRole.Boss)
            {
                return RedirectToPage("/admin/statistics");
            }

            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Lỗi đăng nhập: {ex.Message}";
            return Page();
        }
    }
}