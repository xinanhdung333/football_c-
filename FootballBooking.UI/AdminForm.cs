using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.Models;

namespace FootballBooking.UI;

public partial class AdminForm : Form
{
    private FieldManagementService _fieldService = null!;
    private ServiceInventoryService _serviceService = null!;
    private ServiceDiscountService _discountService = null!;
    private List<Service> _services = new();

    public AdminForm()
    {
        InitializeComponent();
    }

    public AdminForm(
        FieldManagementService fieldService,
        ServiceInventoryService serviceService,
        ServiceDiscountService discountService) : this()
    {
        _fieldService = fieldService;
        _serviceService = serviceService;
        _discountService = discountService;

        LoadFields();
        LoadServices();
    }

    private async void LoadFields()
    {
        try
        {
            var fields = await _fieldService.GetFieldsAsync();
            listViewFields.Items.Clear();

            foreach (var field in fields)
            {
                var item = new ListViewItem(field.Id.ToString());
                item.SubItems.Add(field.Name);
                item.SubItems.Add(field.Location);
                item.SubItems.Add(field.PricePerHour.ToString("C"));
                item.SubItems.Add(field.Status.ToString());
                listViewFields.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Loi tai danh sach san: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void LoadServices()
    {
        try
        {
            var services = (await _serviceService.GetServicesAsync()).ToList();
            _services = services;

            listViewServices.Items.Clear();
            cmbDiscountService.Items.Clear();
            cmbDiscountService.Items.Add(new ServiceComboItem(null, "Tat ca dich vu"));

            foreach (var service in services)
            {
                var item = new ListViewItem(service.Id.ToString());
                item.SubItems.Add(service.Name);
                item.SubItems.Add(service.Description ?? "");
                item.SubItems.Add(service.Price.ToString("C"));
                item.SubItems.Add(service.Quantity.ToString());
                item.SubItems.Add(service.Status.ToString());
                listViewServices.Items.Add(item);

                cmbDiscountService.Items.Add(new ServiceComboItem(service.Id, service.Name));
            }

            if (cmbDiscountService.SelectedIndex < 0)
            {
                cmbDiscountService.SelectedIndex = 0;
            }

            LoadDiscounts();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Loi tai danh sach dich vu: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void LoadDiscounts()
    {
        try
        {
            var discounts = await _discountService.GetDiscountsAsync();
            listViewDiscounts.Items.Clear();

            foreach (var discount in discounts)
            {
                var serviceName = discount.ServiceId.HasValue
                    ? _services.FirstOrDefault(service => service.Id == discount.ServiceId.Value)?.Name ?? $"#{discount.ServiceId.Value}"
                    : "Tat ca dich vu";

                var item = new ListViewItem(discount.Id.ToString()) { Tag = discount.Id };
                item.SubItems.Add(serviceName);
                item.SubItems.Add($"{discount.StartTime:hh\\:mm} - {discount.EndTime:hh\\:mm}");
                item.SubItems.Add($"{discount.DiscountPercent:0}%");
                item.SubItems.Add(discount.Note ?? "");
                item.SubItems.Add(discount.IsActive ? "Active" : "Inactive");
                listViewDiscounts.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Loi tai danh sach giam gia: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnUpdateFieldPrice_Click(object sender, EventArgs e)
    {
        if (listViewFields.SelectedItems.Count == 0)
        {
            MessageBox.Show("Vui long chon san de cap nhat gia.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedId = int.Parse(listViewFields.SelectedItems[0].Text);
        if (decimal.TryParse(txtNewPrice.Text, out var newPrice))
        {
            try
            {
                await _fieldService.UpdateFieldPriceAsync(selectedId, newPrice);
                MessageBox.Show("Cap nhat gia thanh cong!", "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi cap nhat: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Gia khong hop le.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnUpdateStock_Click(object sender, EventArgs e)
    {
        if (listViewServices.SelectedItems.Count == 0)
        {
            MessageBox.Show("Vui long chon dich vu de cap nhat ton kho.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedId = int.Parse(listViewServices.SelectedItems[0].Text);
        if (int.TryParse(txtNewStock.Text, out var newStock))
        {
            try
            {
                await _serviceService.UpdateStockAsync(selectedId, newStock);
                MessageBox.Show("Cap nhat ton kho thanh cong!", "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadServices();
                LoadDiscounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi cap nhat: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("So luong khong hop le.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnCreateDiscount_Click(object sender, EventArgs e)
    {
        if (numDiscountPercent.Value <= 0 || numDiscountPercent.Value >= 100)
        {
            MessageBox.Show("Phan tram giam phai nam trong khoang 1-99.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var serviceId = (cmbDiscountService.SelectedItem as ServiceComboItem)?.ServiceId;
        var discountPercent = numDiscountPercent.Value;

        try
        {
            await _discountService.CreateDiscountAsync(new ServiceDiscount
            {
                ServiceId = serviceId,
                StartTime = discountStartPicker.Value.TimeOfDay,
                EndTime = discountEndPicker.Value.TimeOfDay,
                Multiplier = Math.Round(1 - discountPercent / 100m, 2),
                Note = txtDiscountNote.Text.Trim(),
                IsActive = chkDiscountActive.Checked
            });

            txtDiscountNote.Clear();
            numDiscountPercent.Value = 10;
            LoadDiscounts();
            MessageBox.Show("Da tao giam gia dich vu.", "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Loi tao giam gia: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnDeleteDiscount_Click(object sender, EventArgs e)
    {
        if (listViewDiscounts.SelectedItems.Count == 0)
        {
            MessageBox.Show("Vui long chon rule giam gia.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var discountId = int.Parse(listViewDiscounts.SelectedItems[0].Text);

        try
        {
            await _discountService.DeleteDiscountAsync(discountId);
            LoadDiscounts();
            MessageBox.Show("Da xoa rule giam gia.", "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Loi xoa giam gia: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnLogout_Click(object sender, EventArgs e)
    {
        CurrentUser.User = null;
        var loginForm = new LoginForm();
        loginForm.Show();
        this.Close();
    }

    private sealed class ServiceComboItem
    {
        public ServiceComboItem(int? serviceId, string name)
        {
            ServiceId = serviceId;
            Name = name;
        }

        public int? ServiceId { get; }
        private string Name { get; }

        public override string ToString() => Name;
    }
}
