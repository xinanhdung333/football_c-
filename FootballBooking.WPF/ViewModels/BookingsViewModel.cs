using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FootballBooking.BLL;
using FootballBooking.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace FootballBooking.WPF.ViewModels;

public partial class BookingsViewModel : ObservableObject
{
    private readonly BookingService _bookingService;
    private readonly FeedbackService _feedbackService;

    [ObservableProperty]
    private ObservableCollection<BookingWithDetails> _bookings = new();

    [ObservableProperty]
    private BookingWithDetails? _selectedBooking;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _feedbackMessage = string.Empty;

    [ObservableProperty]
    private byte _feedbackRating = 5;

    public BookingsViewModel(BookingService bookingService, FeedbackService feedbackService)
    {
        _bookingService = bookingService;
        _feedbackService = feedbackService;
        _ = LoadBookingsAsync();
    }

    [RelayCommand]
    private async Task LoadBookingsAsync()
    {
        IsBusy = true;
        try
        {
            // Hardcoded ID for now
            int userId = 1;
            var allBookingsWithDetails = await _bookingService.GetAllBookingsWithDetailsAsync();
            var myBookings = allBookingsWithDetails.Where(b => b.UserId == userId).ToList();
            
            Bookings = new ObservableCollection<BookingWithDetails>(myBookings);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải lịch sử đặt sân: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelBookingAsync()
    {
        if (SelectedBooking == null)
        {
            MessageBox.Show("Vui lòng chọn một lịch đặt sân để hủy.");
            return;
        }

        var result = MessageBox.Show("Bạn có chắc chắn muốn hủy đặt sân này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            try
            {
                await _bookingService.CancelBookingAsync(SelectedBooking.Id, SelectedBooking.UserId);
                MessageBox.Show("Đã hủy đặt sân thành công.");
                await LoadBookingsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hủy: {ex.Message}");
            }
        }
    }

    [RelayCommand]
    private async Task SubmitFeedbackAsync()
    {
        if (SelectedBooking == null)
        {
            MessageBox.Show("Vui lòng chọn một lượt đặt sân để đánh giá.");
            return;
        }

        if (string.IsNullOrWhiteSpace(FeedbackMessage))
        {
            MessageBox.Show("Vui lòng nhập nội dung đánh giá.");
            return;
        }

        IsBusy = true;
        try
        {
            await _feedbackService.SubmitFeedbackAsync(SelectedBooking.UserId, SelectedBooking.Id, null, FeedbackMessage, FeedbackRating);
            MessageBox.Show("Cảm ơn bạn đã gửi đánh giá!");
            FeedbackMessage = string.Empty;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi gửi đánh giá: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
