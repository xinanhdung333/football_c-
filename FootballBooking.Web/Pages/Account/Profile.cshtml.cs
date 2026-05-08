using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Account;

public class ProfileModel : PageModel
{
    private readonly BookingService _bookingService;
    private readonly CheckoutService _checkoutService;
    private readonly FeedbackService _feedbackService;
    private readonly FieldManagementService _fieldService;
    private readonly UserRepository _userRepository;

    public ProfileModel(
        BookingService bookingService,
        CheckoutService checkoutService,
        FeedbackService feedbackService,
        FieldManagementService fieldService,
        UserRepository userRepository)
    {
        _bookingService = bookingService;
        _checkoutService = checkoutService;
        _feedbackService = feedbackService;
        _fieldService = fieldService;
        _userRepository = userRepository;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<Booking> Bookings { get; set; } = new List<Booking>();
    public IEnumerable<Order> Orders { get; set; } = new List<Order>();
    public IEnumerable<FeedbackWithField> Feedbacks { get; set; } = new List<FeedbackWithField>();
    public IEnumerable<Field> Fields { get; set; } = new List<Field>();

    public async Task<IActionResult> OnGetAsync()
    {
        // Check if user is logged in
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            return RedirectToPage("/Account/Login");
        }

        CurrentUser = JsonSerializer.Deserialize<User>(userJson);
        if (CurrentUser == null)
        {
            return RedirectToPage("/Account/Login");
        }

        // Refresh current user from database to show latest changes
        var refreshedUser = await _userRepository.GetUserByIdAsync(CurrentUser.Id);
        if (refreshedUser != null)
        {
            CurrentUser = refreshedUser;
            HttpContext.Session.SetString("CurrentUser", JsonSerializer.Serialize(CurrentUser));
        }

        // Load user data
        Bookings = await _bookingService.GetBookingsByUserIdAsync(CurrentUser.Id);
        Orders = await _checkoutService.GetOrdersByUserIdAsync(CurrentUser.Id);
        Feedbacks = await _feedbackService.GetFeedbacksByUserIdAsync(CurrentUser.Id);
        Fields = await _fieldService.GetFieldsAsync();

        return Page();
    }

    public string GetFieldName(int fieldId)
    {
        var field = Fields.FirstOrDefault(f => f.Id == fieldId);
        return field?.Name ?? "Không xác định";
    }

    public string GetFieldLocation(int fieldId)
    {
        var field = Fields.FirstOrDefault(f => f.Id == fieldId);
        return field?.Location ?? "";
    }
}