using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.Models;

namespace FootballBooking.UI;

public partial class AdminForm : Form
{
    private FieldManagementService _fieldService = null!;
    private ServiceInventoryService _serviceService = null!;

    public AdminForm()
    {
        InitializeComponent();
    }

    public AdminForm(FieldManagementService fieldService, ServiceInventoryService serviceService) : this()
    {
        _fieldService = fieldService;
        _serviceService = serviceService;

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
            MessageBox.Show($"Lỗi tải danh sách sân: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void LoadServices()
    {
        try
        {
            var services = await _serviceService.GetServicesAsync();
            listViewServices.Items.Clear();

            foreach (var service in services)
            {
                var item = new ListViewItem(service.Id.ToString());
                item.SubItems.Add(service.Name);
                item.SubItems.Add(service.Description ?? "");
                item.SubItems.Add(service.Price.ToString("C"));
                item.SubItems.Add(service.Quantity.ToString());
                item.SubItems.Add(service.Status.ToString());
                listViewServices.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải danh sách dịch vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnUpdateFieldPrice_Click(object sender, EventArgs e)
    {
        if (listViewFields.SelectedItems.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn sân để cập nhật giá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedId = int.Parse(listViewFields.SelectedItems[0].Text);
        if (decimal.TryParse(txtNewPrice.Text, out var newPrice))
        {
            try
            {
                await _fieldService.UpdateFieldPriceAsync(selectedId, newPrice);
                MessageBox.Show("Cập nhật giá thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Giá không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnUpdateStock_Click(object sender, EventArgs e)
    {
        if (listViewServices.SelectedItems.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn dịch vụ để cập nhật tồn kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedId = int.Parse(listViewServices.SelectedItems[0].Text);
        if (int.TryParse(txtNewStock.Text, out var newStock))
        {
            try
            {
                await _serviceService.UpdateStockAsync(selectedId, newStock);
                MessageBox.Show("Cập nhật tồn kho thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadServices();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Số lượng không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void btnLogout_Click(object sender, EventArgs e)
    {
        CurrentUser.User = null;
        var loginForm = new LoginForm();
        loginForm.Show();
        this.Close();
    }
}