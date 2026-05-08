using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Admin;

public class DashboardModel : PageModel
{
    private readonly FieldManagementService _fieldService;
    private readonly UserManagementService _userManagementService;
    private readonly ServiceInventoryService _serviceInventoryService;
    private readonly AnalyticsService _analyticsService;

    public DashboardModel(
        FieldManagementService fieldService,
        UserManagementService userManagementService,
        ServiceInventoryService serviceInventoryService,
        AnalyticsService analyticsService)
    {
        _fieldService = fieldService;
        _userManagementService = userManagementService;
        _serviceInventoryService = serviceInventoryService;
        _analyticsService = analyticsService;
    }

    [BindProperty(SupportsGet = true)]
    public DateTime? FromDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? ToDate { get; set; }

    public User? CurrentUser { get; set; }
    public int TotalUsers { get; set; }
    public int TotalFields { get; set; }
    public int TotalServices { get; set; }
    public AnalyticsSummary Summary { get; set; } = new();
    public IReadOnlyList<MonthlyRevenueMetric> MonthlyRevenue { get; set; } = Array.Empty<MonthlyRevenueMetric>();

    public async Task<IActionResult> OnGetAsync()
    {
        if (!TryLoadCurrentUser())
        {
            return RedirectToPage("/Account/Login");
        }

        await LoadDashboardDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnGetExportReportAsync()
    {
        if (!TryLoadCurrentUser())
        {
            return RedirectToPage("/Account/Login");
        }

        await LoadDashboardDataAsync();

        var csv = new StringBuilder();
        csv.AppendLine("From Date,To Date,Total Revenue,Booking Revenue,Service Revenue,Booking Count,Order Count");
        csv.AppendLine($"{Summary.FromDate:yyyy-MM-dd},{Summary.ToDate:yyyy-MM-dd},{Summary.TotalRevenue:F2},{Summary.BookingRevenue:F2},{Summary.ServiceRevenue:F2},{Summary.BookingCount},{Summary.OrderCount}");
        csv.AppendLine();
        csv.AppendLine("Top Fields");
        csv.AppendLine("Field,Bookings,Revenue");
        foreach (var field in Summary.TopFields)
        {
            csv.AppendLine($"\"{field.FieldName}\",{field.BookingCount},{field.Revenue:F2}");
        }

        csv.AppendLine();
        csv.AppendLine("Top Services");
        csv.AppendLine("Service,Quantity,Revenue");
        foreach (var service in Summary.TopServices)
        {
            csv.AppendLine($"\"{service.ServiceName}\",{service.Quantity},{service.Revenue:F2}");
        }

        csv.AppendLine();
        csv.AppendLine("Monthly Revenue");
        csv.AppendLine("Month,Booking Revenue,Service Revenue,Total Revenue");
        foreach (var point in MonthlyRevenue)
        {
            csv.AppendLine($"{point.Month},{point.BookingRevenue:F2},{point.ServiceRevenue:F2},{point.TotalRevenue:F2}");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", $"ThongKe_{Summary.FromDate:yyyyMMdd}_{Summary.ToDate:yyyyMMdd}.csv");
    }

    private async Task LoadDashboardDataAsync()
    {
        var fields = await _fieldService.GetFieldsAsync();
        var users = await _userManagementService.GetAllUsersAsync();
        var services = await _serviceInventoryService.GetServicesAsync();

        TotalFields = fields.Count();
        TotalUsers = users.Count();
        TotalServices = services.Count();

        var toDate = ToDate?.Date ?? DateTime.Today;
        var fromDate = FromDate?.Date ?? toDate.AddDays(-29);
        if (fromDate > toDate)
        {
            fromDate = toDate.AddDays(-29);
        }

        FromDate = fromDate;
        ToDate = toDate;
        Summary = await _analyticsService.GetSummaryAsync(fromDate, toDate);
        MonthlyRevenue = await _analyticsService.GetMonthlyRevenueAsync(12);
    }

    private bool TryLoadCurrentUser()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            return false;
        }

        CurrentUser = JsonSerializer.Deserialize<User>(userJson);
        return CurrentUser is not null && (CurrentUser.Role == UserRole.Admin || CurrentUser.Role == UserRole.Boss);
    }
}
