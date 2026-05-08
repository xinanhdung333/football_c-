using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FootballBooking.Web.Pages.Bookings;

public class ExportBookingInvoiceModel : PageModel
{
    private readonly BookingService _bookingService;
    private readonly FieldManagementService _fieldService;
    private readonly PdfService _pdfService;

    public ExportBookingInvoiceModel(
        BookingService bookingService,
        FieldManagementService fieldService,
        PdfService pdfService)
    {
        _bookingService = bookingService;
        _fieldService = fieldService;
        _pdfService = pdfService;
    }

    public async Task<IActionResult> OnGetAsync(int bookingId)
    {
        // Get current user
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
            return RedirectToPage("/Account/Login");

        var currentUser = System.Text.Json.JsonSerializer.Deserialize<User>(userJson);
        if (currentUser == null)
            return RedirectToPage("/Account/Login");

        // Get booking
        var booking = await _bookingService.GetBookingByIdAsync(bookingId);
        if (booking == null || (booking.UserId != currentUser.Id && currentUser.Role != UserRole.Admin && currentUser.Role != UserRole.Boss))
            return NotFound();

        // Get field info
        var fields = await _fieldService.GetFieldsAsync();
        var field = fields?.FirstOrDefault(f => f.Id == booking.FieldId);
        if (field == null)
            return NotFound();

        // Generate PDF
        var pdfBytes = _pdfService.GenerateBookingInvoice(booking, field, currentUser);

        // Return PDF file
        return File(pdfBytes, "application/pdf", $"HoaDon_DatSan_{booking.Id}.pdf");
    }
}