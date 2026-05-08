namespace FootballBooking.Models;

public enum BookingStatus { Pending, Confirmed, InProgress, Completed, Cancelled, Expired }
public enum FieldStatus { Active, Inactive }
public enum ServiceStatus { Active, Inactive }
public enum OrderStatus { Pending, Confirmed, Processing, InProgress, Completed, Cancelled }
public enum PaymentStatus { Pending, Success, Failed, Refunded }
public enum UserRole { User, Admin, Boss }

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Avt { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Password { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class Field
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal PricePerHour { get; set; }
    public TimeSpan? PeakStartTime { get; set; }
    public TimeSpan? PeakEndTime { get; set; }
    public decimal? PeakPricePerHour { get; set; }
    public string? Image { get; set; }
    public FieldStatus Status { get; set; } = FieldStatus.Active;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Image { get; set; }
    public int Quantity { get; set; }
    public ServiceStatus Status { get; set; } = ServiceStatus.Active;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FieldId { get; set; }
    public DateTime BookingDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public string? Note { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public decimal DurationHours => (decimal)(EndTime - StartTime).TotalHours;
}

public class BookingWithDetails
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FieldId { get; set; }
    public DateTime BookingDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public string? Note { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public string FieldLocation { get; set; } = string.Empty;
}

public class BookingServiceItem
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int ServiceId { get; set; }
    public int Quantity { get; set; } = 1;
}

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ServiceId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal Price { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? CartId { get; set; }
    public decimal TotalAmount { get; set; }
    public string? PaymentMethod { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ServiceId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal Price { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string? MomoOrderId { get; set; }
    public string? MomoTransId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime? PaidAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class BookingPayment
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? MomoOrderId { get; set; }
    public string? MomoTransId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime? PaidAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ServiceDiscount
{
    public int Id { get; set; }
    public int? ServiceId { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public decimal Multiplier { get; set; } = 1;
    public string? Note { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public decimal DiscountPercent => Math.Round((1 - Multiplier) * 100, 2);
}

public class Feedback
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? BookingId { get; set; }
    public int? ServiceId { get; set; }
    public string Message { get; set; } = string.Empty;
    public byte Rating { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? ReplyMessage { get; set; }
    public int? RepliedBy { get; set; }
    public DateTime? RepliedAt { get; set; }
}

public class FeedbackWithField : Feedback
{
    public string? FieldName { get; set; }
    public string? RepliedByName { get; set; }
}

public class FeedbackWithDetails
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? BookingId { get; set; }
    public int? ServiceId { get; set; }
    public string Message { get; set; } = string.Empty;
    public byte Rating { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string? ReplyMessage { get; set; }
    public int? RepliedBy { get; set; }
    public DateTime? RepliedAt { get; set; }
    public string? RepliedByName { get; set; }
}

public class UserSpending
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalBooking { get; set; }
    public decimal TotalServices { get; set; }
    public decimal TotalSpent => TotalBooking + TotalServices;
    public DateTime? LastUpdate { get; set; }
}
