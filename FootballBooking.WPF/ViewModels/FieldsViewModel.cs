using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FootballBooking.BLL;
using FootballBooking.Models;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace FootballBooking.WPF.ViewModels;

public partial class FieldsViewModel : ObservableObject
{
    private readonly FieldManagementService _fieldService;
    private readonly IServiceProvider _serviceProvider;

    private List<Field> _allFields = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Field> _fields = new();

    [ObservableProperty]
    private Field? _selectedField;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private DateTime _bookingDate = DateTime.Today;

    [ObservableProperty]
    private int _startHour = DateTime.Now.Hour + 1;

    [ObservableProperty]
    private int _duration = 1;

    public FieldsViewModel(FieldManagementService fieldService, IServiceProvider serviceProvider)
    {
        _fieldService = fieldService;
        _serviceProvider = serviceProvider;
        _ = LoadFieldsAsync();
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterFields();
    }

    private void FilterFields()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            Fields = new ObservableCollection<Field>(_allFields);
        }
        else
        {
            var filtered = _allFields.Where(f => 
                f.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || 
                (f.Location?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
            Fields = new ObservableCollection<Field>(filtered);
        }
    }

    [RelayCommand]
    private async Task LoadFieldsAsync()
    {
        IsBusy = true;
        try
        {
            var fieldsList = await _fieldService.GetFieldsAsync();
            _allFields = fieldsList.ToList();
            FilterFields();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải danh sách sân: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void ViewDetails()
    {
        if (SelectedField == null)
        {
            MessageBox.Show("Vui lòng chọn một sân để xem chi tiết.");
            return;
        }

        string details = $"ID: {SelectedField.Id}\n" +
                         $"Tên sân: {SelectedField.Name}\n" +
                         $"Địa điểm: {SelectedField.Location}\n" +
                         $"Giá/giờ: {SelectedField.PricePerHour:C}\n" +
                         $"Trạng thái: {SelectedField.Status}\n" +
                         $"File ảnh: {SelectedField.Image ?? "Trống"}";

        MessageBox.Show(details, "Chi tiết sân bóng");
    }

    [RelayCommand]
    private async Task BookFieldAsync()
    {
        if (SelectedField == null)
        {
            MessageBox.Show("Vui lòng chọn một sân để đặt.");
            return;
        }

        if (BookingDate < DateTime.Today)
        {
            MessageBox.Show("Ngày đặt sân không thể ở quá khứ.");
            return;
        }

        var startTime = TimeSpan.FromHours(StartHour);
        var endTime = startTime.Add(TimeSpan.FromHours(Duration));

        var result = MessageBox.Show($"Xác nhận đặt sân {SelectedField.Name}?\nNgày: {BookingDate:dd/MM/yyyy}\nGiờ: {startTime:hh\\:mm} - {endTime:hh\\:mm}", 
                                     "Xác nhận đặt sân", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result != MessageBoxResult.Yes) return;

        IsBusy = true;
        try
        {
            // Hardcoded userId = 1 for now
            int userId = 1;
            
            var bookingService = _serviceProvider.GetRequiredService<BookingService>();
            await bookingService.CreateBookingAsync(userId, SelectedField.Id, BookingDate, startTime, endTime, "Đặt sân từ WPF");

            MessageBox.Show($"Đặt sân {SelectedField.Name} thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            await LoadFieldsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi đặt sân: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
