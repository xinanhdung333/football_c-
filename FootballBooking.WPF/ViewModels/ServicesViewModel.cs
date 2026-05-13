using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FootballBooking.BLL;
using FootballBooking.Models;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace FootballBooking.WPF.ViewModels;

public partial class ServicesViewModel : ObservableObject
{
    private readonly ServiceInventoryService _serviceService;
    private readonly ServiceDiscountService _discountService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private ServiceWithDiscount? _selectedService;

    [ObservableProperty]
    private int _quantity = 1;

    [ObservableProperty]
    private bool _isBusy;

    public ServicesViewModel(ServiceInventoryService serviceService, 
                            ServiceDiscountService discountService,
                            IServiceProvider serviceProvider)
    {
        _serviceService = serviceService;
        _discountService = discountService;
        _serviceProvider = serviceProvider;
        _ = LoadServicesAsync();
    }

    public class ServiceWithDiscount : Service
    {
        public decimal DiscountPercent { get; set; }
        public decimal DiscountedPrice => Price * (1 - DiscountPercent / 100);
        public bool HasDiscount => DiscountPercent > 0;
    }

    [RelayCommand]
    private async Task LoadServicesAsync()
    {
        IsBusy = true;
        try
        {
            var servicesList = await _serviceService.GetServicesAsync();
            var discounts = await _discountService.GetActiveDiscountsAsync();
            
            Services.Clear();
            foreach (var s in servicesList)
            {
                var discount = discounts.FirstOrDefault(d => d.ServiceId == s.Id);
                Services.Add(new ServiceWithDiscount
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    Image = s.Image,
                    Quantity = s.Quantity,
                    Status = s.Status,
                    DiscountPercent = discount?.DiscountPercent ?? 0
                });
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải danh sách dịch vụ: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void ViewDetails()
    {
        if (SelectedService == null)
        {
            MessageBox.Show("Vui lòng chọn một dịch vụ để xem chi tiết.");
            return;
        }

        string details = $"ID: {SelectedService.Id}\n" +
                         $"Tên dịch vụ: {SelectedService.Name}\n" +
                         $"Mô tả: {SelectedService.Description}\n" +
                         $"Giá: {SelectedService.Price:C}\n" +
                         $"Số lượng hiện có: {SelectedService.Quantity}\n" +
                         $"File ảnh: {SelectedService.Image ?? "Trống"}";

        MessageBox.Show(details, "Chi tiết dịch vụ");
    }

    [RelayCommand]
    private async Task AddToCartAsync()
    {
        if (SelectedService == null)
        {
            MessageBox.Show("Vui lòng chọn một dịch vụ để thêm vào giỏ.");
            return;
        }

        if (Quantity <= 0)
        {
            MessageBox.Show("Vui lòng nhập số lượng lớn hơn 0.");
            return;
        }

        if (Quantity > SelectedService.Quantity)
        {
            MessageBox.Show("Số lượng yêu cầu vượt quá tồn kho.");
            return;
        }

        IsBusy = true;
        try
        {
            int userId = 1;
            // Add to cart without deducting inventory yet
            await _serviceProvider.GetRequiredService<CartService>().AddToCartAsync(userId, SelectedService.Id, Quantity);
            
            MessageBox.Show($"Đã thêm {Quantity} x {SelectedService.Name} vào giỏ hàng!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            Quantity = 1;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi thêm vào giỏ hàng: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
