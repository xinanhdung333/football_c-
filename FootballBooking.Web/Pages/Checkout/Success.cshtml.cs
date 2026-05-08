using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FootballBooking.Web.Pages.Checkout;

public class SuccessModel : PageModel
{
    private readonly CheckoutService _checkoutService;

    public SuccessModel(CheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    public Order? Order { get; set; }

    public async Task<IActionResult> OnGetAsync(int orderId)
    {
        if (orderId <= 0)
        {
            return RedirectToPage("/Index");
        }

        try
        {
            Order = await _checkoutService.GetOrderByIdAsync(orderId);
            if (Order == null)
            {
                return RedirectToPage("/Index");
            }
        }
        catch (Exception)
        {
            return RedirectToPage("/Index");
        }

        return Page();
    }
}