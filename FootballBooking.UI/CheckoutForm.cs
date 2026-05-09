using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.Models;

namespace FootballBooking.UI;

public partial class CheckoutForm : Form
{
    private int _cartId;
    private decimal _totalAmount;
    private IEnumerable<CartItem> _items = Enumerable.Empty<CartItem>();
    private CheckoutService _checkoutService = null!;
    private ServiceRepository _serviceRepository = null!;

    public CheckoutForm()
    {
        InitializeComponent();
    }

    public CheckoutForm(CheckoutService checkoutService, ServiceRepository serviceRepository) : this()
    {
        _checkoutService = checkoutService;
        _serviceRepository = serviceRepository;
    }

    public void InitializeCart(int cartId, decimal totalAmount, IEnumerable<CartItem> items)
    {
        _cartId = cartId;
        _totalAmount = totalAmount;
        _items = items;

        lblTotal.Text = $"Tổng tiền: {_totalAmount:C}";
        LoadCartItems();
    }

    private async void LoadCartItems()
    {
        listViewItems.Items.Clear();

        foreach (var item in _items)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(item.ServiceId);
            if (service is not null)
            {
                var listItem = new ListViewItem(service.Name);
                listItem.SubItems.Add(item.Quantity.ToString());
                listItem.SubItems.Add(service.Price.ToString("C"));
                listItem.SubItems.Add(item.Price.ToString("C"));
                var discountPercent = service.Price > 0 ? Math.Max(0, (1 - item.Price / service.Price) * 100) : 0;
                listItem.SubItems.Add(discountPercent > 0 ? $"{discountPercent:0}%" : "");
                listItem.SubItems.Add((item.Quantity * item.Price).ToString("C"));
                listViewItems.Items.Add(listItem);
            }
        }
    }

    private async void btnConfirmPayment_Click(object sender, EventArgs e)
    {
        var paymentMethod = cmbPaymentMethod.Text;
        if (string.IsNullOrEmpty(paymentMethod))
        {
            MessageBox.Show("Vui lòng chọn phương thức thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var orderItems = _items.Select(item => new OrderItem
            {
                ServiceId = item.ServiceId,
                Quantity = item.Quantity,
                Price = item.Price,
                Status = OrderStatus.Pending
            });

            var paymentStatus = paymentMethod == "Tiền mặt" ? PaymentStatus.Success : PaymentStatus.Pending;
            var orderId = await _checkoutService.CreateOrderWithPaymentAsync(
                CurrentUser.User!.Id,
                _cartId,
                _totalAmount,
                paymentMethod,
                orderItems,
                paymentStatus);

            MessageBox.Show($"Thanh toán thành công! Mã đơn hàng: {orderId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi thanh toán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
