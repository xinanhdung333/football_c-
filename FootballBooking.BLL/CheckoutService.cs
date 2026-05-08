using FootballBooking.DAL;
using FootballBooking.Models;
using Microsoft.Data.SqlClient;

namespace FootballBooking.BLL;

public class CheckoutService
{
    private readonly OrderRepository _orderRepository;
    private readonly PaymentRepository _paymentRepository;
    private readonly ServiceRepository _serviceRepository;

    public CheckoutService(OrderRepository orderRepository, PaymentRepository paymentRepository, ServiceRepository serviceRepository)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<int> CreateOrderWithPaymentAsync(int userId, int? cartId, decimal totalAmount, string paymentMethod, IEnumerable<OrderItem> items, PaymentStatus paymentStatus)
    {
        var itemList = items.ToList();
        var serviceSnapshot = new Dictionary<int, Service>();
        foreach (var item in itemList)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(item.ServiceId)
                          ?? throw new InvalidOperationException($"Dịch vụ #{item.ServiceId} không tồn tại.");
            if (service.Quantity < item.Quantity)
            {
                throw new InvalidOperationException($"Dịch vụ '{service.Name}' không đủ số lượng tồn kho.");
            }
            serviceSnapshot[item.ServiceId] = service;
        }

        var order = new Order
        {
            UserId = userId,
            CartId = cartId,
            TotalAmount = totalAmount,
            PaymentMethod = paymentMethod,
            Status = paymentStatus == PaymentStatus.Success ? OrderStatus.Completed : OrderStatus.Pending
        };

        await using var connection = SqlConnectionFactory.CreateConnection();
        await connection.OpenAsync();
        await using var transaction = connection.BeginTransaction();

        try
        {
            var orderId = await _orderRepository.CreateOrderAsync(order, connection, transaction);

            foreach (var item in itemList)
            {
                item.OrderId = orderId;
                await _orderRepository.CreateOrderItemAsync(item, connection, transaction);

                var service = serviceSnapshot[item.ServiceId];
                var newQuantity = Math.Max(service.Quantity - item.Quantity, 0);
                await _serviceRepository.UpdateQuantityAsync(item.ServiceId, newQuantity, connection, transaction);
            }

            var payment = new Payment
            {
                OrderId = orderId,
                Amount = totalAmount,
                Status = paymentStatus,
                PaidAt = paymentStatus == PaymentStatus.Pending ? null : DateTime.Now
            };

            await _paymentRepository.CreatePaymentAsync(payment, connection, transaction);
            await transaction.CommitAsync();
            return orderId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await _orderRepository.GetOrderByIdAsync(orderId);
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
    {
        return await _orderRepository.GetOrderItemsByOrderIdAsync(orderId);
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
    {
        return await _orderRepository.GetOrdersByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllOrdersAsync();
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        return await _orderRepository.UpdateOrderStatusAsync(orderId, status);
    }
}
