using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FootballBooking.Web.Pages.Checkout;

public class ExportOrderInvoiceModel : PageModel
{
    private readonly CheckoutService _checkoutService;
    private readonly ServiceInventoryService _serviceService;
    private readonly PdfService _pdfService;

    public ExportOrderInvoiceModel(
        CheckoutService checkoutService,
        ServiceInventoryService serviceService,
        PdfService pdfService)
    {
        _checkoutService = checkoutService;
        _serviceService = serviceService;
        _pdfService = pdfService;
    }

    public async Task<IActionResult> OnGetAsync(int orderId)
    {
        // Get current user
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
            return RedirectToPage("/Account/Login");

        var currentUser = System.Text.Json.JsonSerializer.Deserialize<User>(userJson);
        if (currentUser == null)
            return RedirectToPage("/Account/Login");

        // Get order
        var order = await _checkoutService.GetOrderByIdAsync(orderId);
        if (order == null || (order.UserId != currentUser.Id && currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
            return NotFound();

        // Get order items
        var orderItems = await _checkoutService.GetOrderItemsByOrderIdAsync(orderId);
        if (orderItems == null || !orderItems.Any())
            return NotFound();

        // Get services info
        var services = await _serviceService.GetServicesAsync();
        if (services == null)
            return NotFound();

        // Generate PDF
        var pdfBytes = _pdfService.GenerateOrderInvoice(order, orderItems, currentUser, services);

        // Return PDF file
        return File(pdfBytes, "application/pdf", $"HoaDon_DichVu_{order.Id}.pdf");
    }
}