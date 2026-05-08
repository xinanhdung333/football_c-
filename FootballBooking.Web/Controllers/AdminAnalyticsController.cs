using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FootballBooking.Web.Controllers;

[ApiController]
[Route("api/admin/analytics")]
public class AdminAnalyticsController : ControllerBase
{
    private readonly AnalyticsService _analyticsService;

    public AdminAnalyticsController(AnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        if (!TryGetAdmin(out _))
        {
            return Unauthorized(new { message = "Bạn cần đăng nhập bằng tài khoản admin." });
        }

        var to = toDate?.Date ?? DateTime.Today;
        var from = fromDate?.Date ?? to.AddDays(-29);
        if (from > to)
        {
            return BadRequest(new { message = "Khoảng thời gian không hợp lệ." });
        }

        var summary = await _analyticsService.GetSummaryAsync(from, to);
        return Ok(summary);
    }

    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthly([FromQuery] int months = 12)
    {
        if (!TryGetAdmin(out _))
        {
            return Unauthorized(new { message = "Bạn cần đăng nhập bằng tài khoản admin." });
        }

        var data = await _analyticsService.GetMonthlyRevenueAsync(months);
        return Ok(data);
    }

    private bool TryGetAdmin(out User? user)
    {
        user = null;
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            return false;
        }

        user = JsonSerializer.Deserialize<User>(userJson);
        return user is not null && (user.Role == UserRole.Admin || user.Role == UserRole.Boss);
    }
}
