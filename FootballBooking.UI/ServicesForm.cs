using FootballBooking.BLL;
using FootballBooking.Models;

namespace FootballBooking.UI;

public partial class ServicesForm : Form
{
    private ServiceInventoryService _serviceService = null!;

    public ServicesForm()
    {
        InitializeComponent();
    }

    public ServicesForm(ServiceInventoryService serviceService) : this()
    {
        _serviceService = serviceService;
        LoadServices();
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
                item.SubItems.Add(service.Description);
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

    private void BtnViewDetails_Click(object sender, EventArgs e)
    {
        if (listViewServices.SelectedItems.Count > 0)
        {
            var selectedItem = listViewServices.SelectedItems[0];
            int serviceId = int.Parse(selectedItem.Text);
            
            string details = $"ID: {serviceId}\n";
            details += $"Tên dịch vụ: {selectedItem.SubItems[1].Text}\n";
            details += $"Mô tả: {selectedItem.SubItems[2].Text}\n";
            details += $"Giá: {selectedItem.SubItems[3].Text}\n";
            details += $"Số lượng: {selectedItem.SubItems[4].Text}";
            
            MessageBox.Show(details, "Chi tiết dịch vụ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show("Vui lòng chọn một dịch vụ để xem chi tiết", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnAddToCart_Click(object sender, EventArgs e)
    {
        if (listViewServices.SelectedItems.Count > 0)
        {
            try
            {
                int quantity = (int)numQuantity.Value;
                if (quantity <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng lớn hơn 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedItem = listViewServices.SelectedItems[0];
                MessageBox.Show($"Đã thêm {quantity} x {selectedItem.SubItems[1].Text} vào giỏ hàng", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                numQuantity.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Vui lòng chọn một dịch vụ để thêm vào giỏ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnRefresh_Click(object sender, EventArgs e)
    {
        LoadServices();
    }

    private void BtnBack_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
