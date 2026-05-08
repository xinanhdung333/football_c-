using FootballBooking.DAL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Admin;

public class ChatbotModel : PageModel
{
    private readonly UserRepository _userRepository;

    public ChatbotModel(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User? CurrentUser { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            return RedirectToPage("/Account/Login");
        }

        CurrentUser = JsonSerializer.Deserialize<User>(userJson);
        if (CurrentUser == null || (CurrentUser.Role != UserRole.Admin && CurrentUser.Role != UserRole.Boss))
        {
            return RedirectToPage("/Account/Login");
        }

        return Page();
    }
}
