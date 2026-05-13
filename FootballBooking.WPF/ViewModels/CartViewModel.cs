using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FootballBooking.BLL;
using FootballBooking.Models;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace FootballBooking.WPF.ViewModels;

public partial class CartViewModel : ObservableObject
{
    private readonly CartService _cartService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private ObservableCollection<CartItem> _cartItems = new();

    [ObservableProperty]
    private CartItem? _selectedItem;

    [ObservableProperty]
    private decimal _totalPrice;

    [ObservableProperty]
    private bool _isBusy;

    public CartViewModel(CartService cartService, IServiceProvider serviceProvider)
    {
        _cartService = cartService;
        _serviceProvider = serviceProvider;
        _ = LoadCartAsync();
    }

    [RelayCommand]
    private async Task LoadCartAsync()
    {
        IsBusy = true;
        try
        {
            // Using a hardcoded ID for now, should come from a Session service
            int userId = 1; 
            var items = await _cartService.GetCartItemsAsync(userId);
            
            CartItems.Clear();
            decimal total = 0;
            foreach (var item in items)
            {
                CartItems.Add(item);
                total += item.Price * item.Quantity;
            }
            TotalPrice = total;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải giỏ hàng: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RemoveItemAsync()
    {
        if (SelectedItem == null) return;

        var result = MessageBox.Show("Bạn có chắc chắn muốn xóa mục này khỏi giỏ hàng?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            // TODO: Call service to remove
            MessageBox.Show("Mục đã được xóa khỏi giỏ hàng (Mock)");
            await LoadCartAsync();
        }
    }

    [RelayCommand]
    private async Task CheckoutAsync()
    {
        if (CartItems.Count == 0)
        {
            MessageBox.Show("Giỏ hàng trống.");
            return;
        }

        var result = MessageBox.Show($"Xác nhận thanh toán đơn hàng trị giá {TotalPrice:C}?", "Thanh toán", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result != MessageBoxResult.Yes) return;

        IsBusy = true;
        try
        {
            int userId = 1;
            var cart = await _serviceProvider.GetRequiredService<CartService>().GetOrCreateCartAsync(userId);
            
            var orderItems = CartItems.Select(ci => new OrderItem
            {
                ServiceId = ci.ServiceId,
                Quantity = ci.Quantity,
                Price = ci.Price
            });

            await _serviceProvider.GetRequiredService<CheckoutService>().CreateOrderWithPaymentAsync(
                userId, cart.Id, TotalPrice, "Tiền mặt", orderItems, PaymentStatus.Success);

            await _cartService.ClearCartAsync(userId);
            
            MessageBox.Show("Đặt hàng thành công!");
            await LoadCartAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi thanh toán: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RemoveItemAsync(CartItem item)
    {
        if (item == null) return;
        try
        {
            int userId = 1;
            await _cartService.RemoveFromCartAsync(userId, item.ServiceId);
            await LoadCartAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    [RelayCommand]
    private async Task IncreaseQuantityAsync(CartItem item)
    {
        if (item == null) return;
        try
        {
            int userId = 1;
            await _cartService.AddToCartAsync(userId, item.ServiceId, 1);
            await LoadCartAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    [RelayCommand]
    private async Task DecreaseQuantityAsync(CartItem item)
    {
        if (item == null || item.Quantity <= 1) return;
        try
        {
            int userId = 1;
            await _cartService.UpdateCartItemQuantityAsync(userId, item.ServiceId, item.Quantity - 1);
            await LoadCartAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }
}
