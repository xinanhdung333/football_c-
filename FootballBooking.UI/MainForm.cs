using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FootballBooking.UI;

public partial class MainForm : Form
{
    private FieldManagementService _fieldService = null!;
    private BookingService _bookingService = null!;
    private CartService _cartService = null!;
    private ServiceInventoryService _serviceService = null!;
    private IServiceProvider _serviceProvider = null!;

    public MainForm()
    {
        InitializeComponent();
    }

    public MainForm(
        FieldManagementService fieldService,
        BookingService bookingService,
        CartService cartService,
        ServiceInventoryService serviceService,
        IServiceProvider serviceProvider) : this()
    {
        _fieldService = fieldService;
        _bookingService = bookingService;
        _cartService = cartService;
        _serviceService = serviceService;
        _serviceProvider = serviceProvider;

        LoadFields();
        LoadServices();
        LoadCart();
    }

    private async void LoadFields()
    {
        try
        {
            var fields = await _fieldService.GetFieldsAsync();
            listViewFields.Items.Clear();

            foreach (var field in fields.Where(f => f.Status == FieldStatus.Active))
            {
                var item = new ListViewItem(field.Id.ToString());
                item.SubItems.Add(field.Name);
                item.SubItems.Add(field.Location);
                item.SubItems.Add(field.PricePerHour.ToString("C"));
                item.SubItems.Add(field.Description ?? "");
                listViewFields.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải danh sách sân: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void LoadServices()
    {
        try
        {
            var services = await _serviceService.GetServicesAsync();
            listViewServices.Items.Clear();

            foreach (var service in services.Where(s => s.Status == ServiceStatus.Active))
            {
                var item = new ListViewItem(service.Id.ToString());
                item.SubItems.Add(service.Name);
                item.SubItems.Add(service.Description ?? "");
                item.SubItems.Add(service.Price.ToString("C"));
                item.SubItems.Add(service.Quantity.ToString());
                listViewServices.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải danh sách dịch vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void LoadCart()
    {
        try
        {
            if (CurrentUser.User is null) return;

            var items = await _cartService.GetCartItemsAsync(CurrentUser.User.Id);
            listViewCart.Items.Clear();

            foreach (var item in items)
            {
                var service = await _serviceService.GetServiceByIdAsync(item.ServiceId);
                if (service is not null)
                {
                    var listItem = new ListViewItem(service.Name);
                    listItem.SubItems.Add(item.Quantity.ToString());
                    listItem.SubItems.Add(item.Price.ToString("C"));
                    listItem.SubItems.Add((item.Quantity * item.Price).ToString("C"));
                    listViewCart.Items.Add(listItem);
                }
            }

            var total = await _cartService.CalculateCartTotalAsync(CurrentUser.User.Id);
            lblTotal.Text = $"Tổng tiền: {total:C}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải giỏ hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnBookField_Click(object sender, EventArgs e)
    {
        if (listViewFields.SelectedItems.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn sân để đặt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (CurrentUser.User is null)
        {
            MessageBox.Show("Vui lòng đăng nhập trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedId = int.Parse(listViewFields.SelectedItems[0].Text);
        var bookingDate = dateTimePicker.Value.Date;
        var startTime = new TimeSpan((int)numStartHour.Value, (int)numStartMinute.Value, 0);
        var endTime = new TimeSpan((int)numEndHour.Value, (int)numEndMinute.Value, 0);

        try
        {
            var bookingId = await _bookingService.CreateBookingAsync(CurrentUser.User.Id, selectedId, bookingDate, startTime, endTime, txtNote.Text);
            MessageBox.Show($"Đặt sân thành công! Mã đặt sân: {bookingId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadFields();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Lỗi đặt sân", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnAddToCart_Click(object sender, EventArgs e)
    {
        if (listViewServices.SelectedItems.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn dịch vụ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (CurrentUser.User is null)
        {
            MessageBox.Show("Vui lòng đăng nhập trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedId = int.Parse(listViewServices.SelectedItems[0].Text);
        var quantity = (int)numQuantity.Value;

        try
        {
            await _cartService.AddToCartAsync(CurrentUser.User.Id, selectedId, quantity);
            MessageBox.Show("Đã thêm vào giỏ hàng!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadCart();
            LoadServices();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Lỗi thêm vào giỏ hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnCheckout_Click(object sender, EventArgs e)
    {
        if (CurrentUser.User is null) return;

        var items = await _cartService.GetCartItemsAsync(CurrentUser.User.Id);
        if (!items.Any())
        {
            MessageBox.Show("Giỏ hàng trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cart = await _cartService.GetOrCreateCartAsync(CurrentUser.User.Id);
        var total = await _cartService.CalculateCartTotalAsync(CurrentUser.User.Id);

        var checkoutForm = _serviceProvider.GetRequiredService<CheckoutForm>();
        checkoutForm.InitializeCart(cart.Id, total, items);
        if (checkoutForm.ShowDialog() == DialogResult.OK)
        {
            await _cartService.ClearCartAsync(CurrentUser.User.Id);
            LoadCart();
            LoadServices();
            MessageBox.Show("Thanh toán thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void btnLogout_Click(object sender, EventArgs e)
    {
        CurrentUser.User = null;
        var loginForm = _serviceProvider.GetRequiredService<LoginForm>();
        loginForm.Show();
        this.Close();
    }
}