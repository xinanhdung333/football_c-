using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FootballBooking.BLL;
using FootballBooking.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace FootballBooking.WPF.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly BookingService _bookingService;
    private readonly CheckoutService _checkoutService;
    private readonly FeedbackService _feedbackService;

    [ObservableProperty]
    private string _userName = "User Test"; // Mock for now

    [ObservableProperty]
    private ObservableCollection<BookingWithDetails> _myBookings = new();

    [ObservableProperty]
    private ObservableCollection<Order> _myOrders = new();

    [ObservableProperty]
    private bool _isBusy;

    public ProfileViewModel(BookingService bookingService, CheckoutService checkoutService, FeedbackService feedbackService)
    {
        _bookingService = bookingService;
        _checkoutService = checkoutService;
        _feedbackService = feedbackService;
        _ = LoadUserDataAsync();
    }

    [RelayCommand]
    private async Task LoadUserDataAsync()
    {
        IsBusy = true;
        try
        {
            int userId = 1; // Hardcoded userId = 1
            
            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
            // Convert to BookingWithDetails if needed, or just use Booking
            // For simplicity, let's assume we want to show details
            var allBookings = await _bookingService.GetAllBookingsWithDetailsAsync();
            MyBookings = new ObservableCollection<BookingWithDetails>(allBookings.Where(b => b.UserId == userId));

            var orders = await _checkoutService.GetOrdersByUserIdAsync(userId);
            MyOrders = new ObservableCollection<Order>(orders);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải thông tin cá nhân: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
