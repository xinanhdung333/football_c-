using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballBooking.Web.Controllers;

[ApiController]
[Route("api/public")]
public class PublicController : ControllerBase
{
    private readonly FieldManagementService _fieldManagementService;
    private readonly ServiceInventoryService _serviceInventoryService;
    private readonly BookingService _bookingService;

    public PublicController(
        FieldManagementService fieldManagementService,
        ServiceInventoryService serviceInventoryService,
        BookingService bookingService)
    {
        _fieldManagementService = fieldManagementService;
        _serviceInventoryService = serviceInventoryService;
        _bookingService = bookingService;
    }

    [HttpGet("fields")]
    public async Task<IActionResult> GetFields([FromQuery] string? location = null, [FromQuery] string? status = null)
    {
        var fields = await _fieldManagementService.GetFieldsAsync();
        var normalizedStatus = status?.Trim().ToLowerInvariant();
        var normalizedLocation = location?.Trim().ToLowerInvariant();

        var data = fields.Where(field =>
        {
            var locationMatched = string.IsNullOrWhiteSpace(normalizedLocation) ||
                                  field.Location.Contains(normalizedLocation, StringComparison.OrdinalIgnoreCase);
            var statusMatched = string.IsNullOrWhiteSpace(normalizedStatus) ||
                                field.Status.ToString().Equals(normalizedStatus, StringComparison.OrdinalIgnoreCase);
            return locationMatched && statusMatched;
        });

        return Ok(data);
    }

    [HttpGet("services")]
    public async Task<IActionResult> GetServices([FromQuery] string? query = null, [FromQuery] bool inStockOnly = false)
    {
        var services = await _serviceInventoryService.GetServicesAsync();
        var normalizedQuery = query?.Trim();

        var data = services.Where(service =>
        {
            var queryMatched = string.IsNullOrWhiteSpace(normalizedQuery) ||
                               service.Name.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase) ||
                               (!string.IsNullOrWhiteSpace(service.Description) &&
                                service.Description.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase));
            var stockMatched = !inStockOnly || service.Quantity > 0;
            return queryMatched && stockMatched;
        });

        return Ok(data);
    }

    [HttpGet("availability")]
    public async Task<IActionResult> CheckAvailability([FromQuery] int fieldId, [FromQuery] DateTime date, [FromQuery] TimeSpan start, [FromQuery] TimeSpan end)
    {
        if (fieldId <= 0)
        {
            return BadRequest(new { message = "fieldId không hợp lệ." });
        }

        if (end <= start)
        {
            return BadRequest(new { message = "Khung giờ không hợp lệ." });
        }

        var hasConflict = await _bookingService.HasConflictAsync(fieldId, date.Date, start, end);
        return Ok(new
        {
            fieldId,
            date = date.Date,
            start,
            end,
            isAvailable = !hasConflict
        });
    }
}
