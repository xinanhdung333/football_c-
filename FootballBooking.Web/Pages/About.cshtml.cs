using FootballBooking.DAL;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FootballBooking.Web.Pages;

public class AboutModel : PageModel
{
    private readonly FieldRepository _fieldRepository;
    private readonly UserRepository _userRepository;
    private readonly BookingRepository _bookingRepository;

    public int TotalFields { get; set; }
    public int TotalUsers { get; set; }
    public int TotalBookings { get; set; }

    public AboutModel(
        FieldRepository fieldRepository,
        UserRepository userRepository,
        BookingRepository bookingRepository)
    {
        _fieldRepository = fieldRepository;
        _userRepository = userRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task OnGetAsync()
    {
        // Get statistics
        var fields = await _fieldRepository.GetAllFieldsAsync();
        TotalFields = fields?.Count() ?? 0;

        var users = await _userRepository.GetAllUsersAsync();
        TotalUsers = users?.Count() ?? 0;

        var bookings = await _bookingRepository.GetAllBookingsAsync();
        TotalBookings = bookings?.Count() ?? 0;
    }
}
