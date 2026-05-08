using FootballBooking.DAL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Account;

public class EditProfileModel : PageModel
{
    private readonly UserRepository _userRepository;

    public EditProfileModel(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User? CurrentUser { get; set; }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Phone { get; set; } = string.Empty;

    [BindProperty]
    public string? Avt { get; set; }

    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToPage("/Account/Login");
        }

        CurrentUser = currentUser;
        Name = currentUser.Name;
        Email = currentUser.Email;
        Phone = currentUser.Phone;
        Avt = currentUser.Avt;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToPage("/Account/Login");
        }

        currentUser.Name = Name;
        currentUser.Phone = Phone;
        currentUser.Avt = string.IsNullOrWhiteSpace(Avt) ? null : Avt;

        await _userRepository.UpdateUserAsync(currentUser);

        HttpContext.Session.SetString("CurrentUser", JsonSerializer.Serialize(currentUser));

        return RedirectToPage("/Account/Profile");
    }

    private User? GetCurrentUser()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        return string.IsNullOrEmpty(userJson)
            ? null
            : JsonSerializer.Deserialize<User>(userJson);
    }
}
