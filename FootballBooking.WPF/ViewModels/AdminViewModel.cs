using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FootballBooking.BLL;
using FootballBooking.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace FootballBooking.WPF.ViewModels;

public partial class AdminViewModel : ObservableObject
{
    private readonly FieldManagementService _fieldService;
    private readonly ServiceInventoryService _serviceService;
    private readonly ServiceDiscountService _discountService;
    private readonly CheckoutService _checkoutService;
    private readonly BookingService _bookingService;

    [ObservableProperty]
    private ObservableCollection<Field> _fields = new();

    [ObservableProperty]
    private ObservableCollection<Service> _services = new();

    [ObservableProperty]
    private ObservableCollection<ServiceDiscount> _discounts = new();

    [ObservableProperty]
    private ObservableCollection<Order> _orders = new();

    [ObservableProperty]
    private ObservableCollection<BookingWithDetails> _allBookings = new();

    [ObservableProperty]
    private Field? _selectedField;

    [ObservableProperty]
    private Service? _selectedService;

    [ObservableProperty]
    private Order? _selectedOrder;

    [ObservableProperty]
    private BookingWithDetails? _selectedBooking;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private decimal _newFieldPrice;

    [ObservableProperty]
    private string _newFieldName = string.Empty;

    [ObservableProperty]
    private string _newFieldLocation = string.Empty;

    [ObservableProperty]
    private int _newServiceStock;

    [ObservableProperty]
    private string _newServiceName = string.Empty;

    [ObservableProperty]
    private decimal _newServicePrice;

    public AdminViewModel(
        FieldManagementService fieldService,
        ServiceInventoryService serviceService,
        ServiceDiscountService discountService,
        CheckoutService checkoutService,
        BookingService bookingService)
    {
        _fieldService = fieldService;
        _serviceService = serviceService;
        _discountService = discountService;
        _checkoutService = checkoutService;
        _bookingService = bookingService;

        _ = LoadAllDataAsync();
    }

    [RelayCommand]
    private async Task LoadAllDataAsync()
    {
        IsBusy = true;
        try
        {
            var fieldsList = await _fieldService.GetFieldsAsync();
            Fields = new ObservableCollection<Field>(fieldsList);

            var servicesList = await _serviceService.GetServicesAsync();
            Services = new ObservableCollection<Service>(servicesList);

            var discountsList = await _discountService.GetDiscountsAsync();
            Discounts = new ObservableCollection<ServiceDiscount>(discountsList);

            var ordersList = await _checkoutService.GetAllOrdersAsync();
            Orders = new ObservableCollection<Order>(ordersList);

            var bookingsList = await _bookingService.GetAllBookingsWithDetailsAsync();
            AllBookings = new ObservableCollection<BookingWithDetails>(bookingsList);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task UpdateFieldPriceAsync()
    {
        if (SelectedField == null) return;

        try
        {
            await _fieldService.UpdateFieldPriceAsync(SelectedField.Id, NewFieldPrice);
            MessageBox.Show("Cập nhật giá thành công!");
            await LoadAllDataAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task UpdateServiceStockAsync()
    {
        if (SelectedService == null) return;

        try
        {
            await _serviceService.UpdateStockAsync(SelectedService.Id, NewServiceStock);
            MessageBox.Show("Cập nhật tồn kho thành công!");
            await LoadAllDataAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task AddFieldAsync()
    {
        if (string.IsNullOrWhiteSpace(NewFieldName))
        {
            MessageBox.Show("Vui lòng nhập tên sân.");
            return;
        }
        try
        {
            var field = new Field { Name = NewFieldName, Location = NewFieldLocation, PricePerHour = NewFieldPrice };
            await _fieldService.CreateFieldAsync(field);
            MessageBox.Show("Thêm sân bóng mới thành công!");
            NewFieldName = string.Empty;
            NewFieldLocation = string.Empty;
            await LoadAllDataAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    [RelayCommand]
    private async Task DeleteFieldAsync()
    {
        if (SelectedField == null) return;
        var res = MessageBox.Show($"Xóa sân {SelectedField.Name}?", "Xác nhận", MessageBoxButton.YesNo);
        if (res != MessageBoxResult.Yes) return;
        
        try
        {
            await _fieldService.DeleteFieldAsync(SelectedField.Id);
            MessageBox.Show("Xóa sân thành công!");
            await LoadAllDataAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    [RelayCommand]
    private async Task AddServiceAsync()
    {
        if (string.IsNullOrWhiteSpace(NewServiceName))
        {
            MessageBox.Show("Vui lòng nhập tên dịch vụ.");
            return;
        }
        try
        {
            var service = new Service { Name = NewServiceName, Price = NewServicePrice, Quantity = NewServiceStock, Status = ServiceStatus.Active };
            await _serviceService.CreateServiceAsync(service);
            MessageBox.Show("Thêm dịch vụ mới thành công!");
            NewServiceName = string.Empty;
            await LoadAllDataAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    [RelayCommand]
    private async Task DeleteServiceAsync()
    {
        if (SelectedService == null) return;
        var res = MessageBox.Show($"Xóa dịch vụ {SelectedService.Name}?", "Xác nhận", MessageBoxButton.YesNo);
        if (res != MessageBoxResult.Yes) return;
        
        try
        {
            await _serviceService.DeleteServiceAsync(SelectedService.Id);
            MessageBox.Show("Xóa dịch vụ thành công!");
            await LoadAllDataAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    [RelayCommand]
    private async Task CompleteOrderAsync()
    {
        if (SelectedOrder == null) return;
        try
        {
            await _checkoutService.UpdateOrderStatusAsync(SelectedOrder.Id, OrderStatus.Completed);
            MessageBox.Show("Đơn hàng đã hoàn thành!");
            await LoadAllDataAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    [RelayCommand]
    private async Task CancelOrderAsync()
    {
        if (SelectedOrder == null) return;
        var res = MessageBox.Show($"Hủy đơn hàng #{SelectedOrder.Id}?", "Xác nhận", MessageBoxButton.YesNo);
        if (res != MessageBoxResult.Yes) return;
        try
        {
            await _checkoutService.UpdateOrderStatusAsync(SelectedOrder.Id, OrderStatus.Cancelled);
            MessageBox.Show("Đơn hàng đã bị hủy!");
            await LoadAllDataAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    [RelayCommand]
    private async Task ConfirmBookingAsync()
    {
        if (SelectedBooking == null) return;
        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(SelectedBooking.Id);
            if (booking != null)
            {
                booking.Status = BookingStatus.Confirmed;
                await _bookingService.UpdateBookingAsync(booking);
                MessageBox.Show("Đã xác nhận lịch đặt sân!");
                await LoadAllDataAsync();
            }
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    [RelayCommand]
    private async Task CancelBookingByAdminAsync()
    {
        if (SelectedBooking == null) return;
        var res = MessageBox.Show($"Hủy lịch đặt sân của {SelectedBooking.UserName}?", "Xác nhận", MessageBoxButton.YesNo);
        if (res != MessageBoxResult.Yes) return;
        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(SelectedBooking.Id);
            if (booking != null)
            {
                booking.Status = BookingStatus.Cancelled;
                await _bookingService.UpdateBookingAsync(booking);
                MessageBox.Show("Lịch đặt sân đã bị hủy!");
                await LoadAllDataAsync();
            }
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }
}
