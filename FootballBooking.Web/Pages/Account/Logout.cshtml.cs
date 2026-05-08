using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FootballBooking.Web.Pages.Account;

public class LogoutModel : PageModel
{
    public IActionResult OnGet()
    {
        // Clear session and remove session cookie
        HttpContext.Session.Clear();
        Response.Cookies.Delete(".AspNetCore.Session");

        // Redirect to login page
        return RedirectToPage("/Account/Login");
    }
}