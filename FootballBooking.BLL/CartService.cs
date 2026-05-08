using FootballBooking.DAL;
using FootballBooking.Models;

namespace FootballBooking.BLL;

public class CartService
{
    private readonly CartRepository _cartRepository;
    private readonly ServiceRepository _serviceRepository;
    private readonly ServiceDiscountService _discountService;

    public CartService(CartRepository cartRepository, ServiceRepository serviceRepository, ServiceDiscountService discountService)
    {
        _cartRepository = cartRepository;
        _serviceRepository = serviceRepository;
        _discountService = discountService;
    }

    public async Task<Cart> GetOrCreateCartAsync(int userId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart is null)
        {
            var cartId = await _cartRepository.CreateCartAsync(userId);
            cart = new Cart { Id = cartId, UserId = userId };
        }

        return cart;
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId)
    {
        var cart = await GetOrCreateCartAsync(userId);
        return await _cartRepository.GetCartItemsAsync(cart.Id);
    }

    public async Task AddToCartAsync(int userId, int serviceId, int quantity)
    {
        var service = await _serviceRepository.GetServiceByIdAsync(serviceId);
        if (service is null)
        {
            throw new InvalidOperationException("Dịch vụ không tồn tại.");
        }

        if (service.Quantity < quantity)
        {
            throw new InvalidOperationException("Không đủ số lượng tồn kho.");
        }

        var cart = await GetOrCreateCartAsync(userId);
        var existingItem = await _cartRepository.GetCartItemByServiceIdAsync(cart.Id, serviceId);

        if (existingItem is not null)
        {
            var newQuantity = existingItem.Quantity + quantity;
            if (service.Quantity < newQuantity)
            {
                throw new InvalidOperationException("Không đủ số lượng tồn kho.");
            }
            await _cartRepository.UpdateCartItemQuantityAsync(existingItem.Id, newQuantity);
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ServiceId = serviceId,
                Quantity = quantity,
                Price = (await _discountService.GetPriceQuoteAsync(service)).FinalPrice
            };

            await _cartRepository.AddCartItemAsync(cartItem);
        }
    }

    public async Task UpdateCartItemQuantityAsync(int userId, int cartItemId, int quantity)
    {
        if (quantity < 1)
        {
            throw new InvalidOperationException("Số lượng phải lớn hơn 0.");
        }

        var cart = await GetOrCreateCartAsync(userId);
        var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
        if (cartItem is null || cartItem.CartId != cart.Id)
        {
            throw new InvalidOperationException("Mục giỏ hàng không hợp lệ.");
        }

        var service = await _serviceRepository.GetServiceByIdAsync(cartItem.ServiceId);
        if (service is null)
        {
            throw new InvalidOperationException("Dịch vụ không tồn tại.");
        }

        if (service.Quantity < quantity)
        {
            throw new InvalidOperationException("Không đủ số lượng tồn kho.");
        }

        await _cartRepository.UpdateCartItemQuantityAsync(cartItemId, quantity);
    }

    public async Task RemoveCartItemAsync(int userId, int cartItemId)
    {
        var cart = await GetOrCreateCartAsync(userId);
        var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
        if (cartItem is null || cartItem.CartId != cart.Id)
        {
            throw new InvalidOperationException("Mục giỏ hàng không hợp lệ.");
        }

        await _cartRepository.RemoveCartItemAsync(cartItemId);
    }

    public async Task ClearCartAsync(int userId)
    {
        var cart = await GetOrCreateCartAsync(userId);
        await _cartRepository.ClearCartAsync(cart.Id);
    }

    public async Task<decimal> CalculateCartTotalAsync(int userId)
    {
        var items = await GetCartItemsAsync(userId);
        return items.Sum(item => item.Quantity * item.Price);
    }
}
