using FootballBooking.DAL;
using FootballBooking.Models;

namespace FootballBooking.BLL;

public class AnalyticsService
{
    private readonly BookingRepository _bookingRepository;
    private readonly OrderRepository _orderRepository;
    private readonly FieldRepository _fieldRepository;
    private readonly ServiceRepository _serviceRepository;

    public AnalyticsService(
        BookingRepository bookingRepository,
        OrderRepository orderRepository,
        FieldRepository fieldRepository,
        ServiceRepository serviceRepository)
    {
        _bookingRepository = bookingRepository;
        _orderRepository = orderRepository;
        _fieldRepository = fieldRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<AnalyticsSummary> GetSummaryAsync(DateTime fromDate, DateTime toDate)
    {
        var normalizedFrom = fromDate.Date;
        var normalizedTo = toDate.Date;

        var bookings = (await _bookingRepository.GetAllBookingsAsync())
            .Where(booking => booking.BookingDate.Date >= normalizedFrom && booking.BookingDate.Date <= normalizedTo)
            .ToList();

        var orders = (await _orderRepository.GetAllOrdersAsync())
            .Where(order =>
            {
                var createdAt = order.CreatedAt?.Date;
                return createdAt.HasValue && createdAt.Value >= normalizedFrom && createdAt.Value <= normalizedTo;
            })
            .ToList();

        var fields = (await _fieldRepository.GetAllFieldsAsync()).ToDictionary(field => field.Id, field => field.Name);
        var services = (await _serviceRepository.GetAllServicesAsync()).ToDictionary(service => service.Id, service => service.Name);

        var bookingRevenue = bookings.Sum(booking => booking.TotalPrice);
        var orderRevenue = orders.Sum(order => order.TotalAmount);

        var orderItems = new List<OrderItem>();
        foreach (var order in orders)
        {
            var items = await _orderRepository.GetOrderItemsByOrderIdAsync(order.Id);
            orderItems.AddRange(items);
        }

        var topFields = bookings
            .GroupBy(booking => booking.FieldId)
            .Select(group =>
            {
                fields.TryGetValue(group.Key, out var fieldName);
                return new TopFieldMetric
                {
                    FieldId = group.Key,
                    FieldName = fieldName ?? $"Sân #{group.Key}",
                    BookingCount = group.Count(),
                    Revenue = group.Sum(item => item.TotalPrice)
                };
            })
            .OrderByDescending(item => item.BookingCount)
            .ThenByDescending(item => item.Revenue)
            .Take(5)
            .ToList();

        var topServices = orderItems
            .GroupBy(item => item.ServiceId)
            .Select(group =>
            {
                services.TryGetValue(group.Key, out var serviceName);
                return new TopServiceMetric
                {
                    ServiceId = group.Key,
                    ServiceName = serviceName ?? $"Dịch vụ #{group.Key}",
                    Quantity = group.Sum(item => item.Quantity),
                    Revenue = group.Sum(item => item.Price * item.Quantity)
                };
            })
            .OrderByDescending(item => item.Quantity)
            .ThenByDescending(item => item.Revenue)
            .Take(5)
            .ToList();

        var bookingStatusCounts = bookings
            .GroupBy(booking => booking.Status)
            .ToDictionary(group => group.Key.ToString(), group => group.Count());

        var orderStatusCounts = orders
            .GroupBy(order => order.Status)
            .ToDictionary(group => group.Key.ToString(), group => group.Count());

        return new AnalyticsSummary
        {
            FromDate = normalizedFrom,
            ToDate = normalizedTo,
            BookingCount = bookings.Count,
            OrderCount = orders.Count,
            BookingRevenue = bookingRevenue,
            ServiceRevenue = orderRevenue,
            TotalRevenue = bookingRevenue + orderRevenue,
            TopFields = topFields,
            TopServices = topServices,
            BookingStatusCounts = bookingStatusCounts,
            OrderStatusCounts = orderStatusCounts
        };
    }

    public async Task<IReadOnlyList<MonthlyRevenueMetric>> GetMonthlyRevenueAsync(int months)
    {
        var safeMonths = Math.Clamp(months, 1, 24);
        var currentMonthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        var bookings = (await _bookingRepository.GetAllBookingsAsync()).ToList();
        var orders = (await _orderRepository.GetAllOrdersAsync()).ToList();

        var result = new List<MonthlyRevenueMetric>();
        for (var index = safeMonths - 1; index >= 0; index--)
        {
            var monthStart = currentMonthStart.AddMonths(-index);
            var monthEndExclusive = monthStart.AddMonths(1);

            var bookingRevenue = bookings
                .Where(booking => booking.BookingDate.Date >= monthStart && booking.BookingDate.Date < monthEndExclusive)
                .Sum(booking => booking.TotalPrice);

            var serviceRevenue = orders
                .Where(order =>
                {
                    var createdAt = order.CreatedAt;
                    return createdAt.HasValue && createdAt.Value >= monthStart && createdAt.Value < monthEndExclusive;
                })
                .Sum(order => order.TotalAmount);

            result.Add(new MonthlyRevenueMetric
            {
                Month = monthStart.ToString("MM/yyyy"),
                BookingRevenue = bookingRevenue,
                ServiceRevenue = serviceRevenue,
                TotalRevenue = bookingRevenue + serviceRevenue
            });
        }

        return result;
    }
}

public class AnalyticsSummary
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int BookingCount { get; set; }
    public int OrderCount { get; set; }
    public decimal BookingRevenue { get; set; }
    public decimal ServiceRevenue { get; set; }
    public decimal TotalRevenue { get; set; }
    public IReadOnlyList<TopFieldMetric> TopFields { get; set; } = Array.Empty<TopFieldMetric>();
    public IReadOnlyList<TopServiceMetric> TopServices { get; set; } = Array.Empty<TopServiceMetric>();
    public IReadOnlyDictionary<string, int> BookingStatusCounts { get; set; } = new Dictionary<string, int>();
    public IReadOnlyDictionary<string, int> OrderStatusCounts { get; set; } = new Dictionary<string, int>();
}

public class TopFieldMetric
{
    public int FieldId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public int BookingCount { get; set; }
    public decimal Revenue { get; set; }
}

public class TopServiceMetric
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Revenue { get; set; }
}

public class MonthlyRevenueMetric
{
    public string Month { get; set; } = string.Empty;
    public decimal BookingRevenue { get; set; }
    public decimal ServiceRevenue { get; set; }
    public decimal TotalRevenue { get; set; }
}
