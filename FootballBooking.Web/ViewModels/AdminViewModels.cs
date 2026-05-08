using FootballBooking.Models;

namespace FootballBooking.Web.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class FieldViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal PricePerHour { get; set; }
    public TimeSpan? PeakStartTime { get; set; }
    public TimeSpan? PeakEndTime { get; set; }
    public decimal? PeakPricePerHour { get; set; }
    public FieldStatus Status { get; set; }
    public string? Image { get; set; }
}

public class BookingViewModel
{
    public int Id { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public string UserUsername { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public DateTime? BookingDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public decimal? TotalAmount { get; set; }
    public BookingStatus Status { get; set; }
}

public class ServiceViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public ServiceStatus Status { get; set; }
    public string? Image { get; set; }
}

public class FeedbackViewModel
{
    public int Id { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public string UserUsername { get; set; } = string.Empty;
    public int? BookingId { get; set; }
    public int? ServiceId { get; set; }
    public string Message { get; set; } = string.Empty;
    public byte Rating { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? ReplyMessage { get; set; }
    public string? RepliedByName { get; set; }
    public DateTime? RepliedAt { get; set; }
}
