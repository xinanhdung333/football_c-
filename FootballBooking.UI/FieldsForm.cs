using FootballBooking.BLL;
using FootballBooking.Models;

namespace FootballBooking.UI;

public partial class FieldsForm : Form
{
    private FieldManagementService _fieldService = null!;

    public FieldsForm()
    {
        InitializeComponent();
    }

    public FieldsForm(FieldManagementService fieldService) : this()
    {
        _fieldService = fieldService;
        LoadFields();
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
                item.SubItems.Add(field.Type);
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

    private void BtnViewDetails_Click(object sender, EventArgs e)
    {
        if (listViewFields.SelectedItems.Count > 0)
        {
            var selectedItem = listViewFields.SelectedItems[0];
            int fieldId = int.Parse(selectedItem.Text);
            
            string details = $"ID: {fieldId}\n";
            details += $"Tên sân: {selectedItem.SubItems[1].Text}\n";
            details += $"Địa điểm: {selectedItem.SubItems[2].Text}\n";
            details += $"Loại: {selectedItem.SubItems[3].Text}\n";
            details += $"Giá/giờ: {selectedItem.SubItems[4].Text}\n";
            details += $"Trạng thái: {selectedItem.SubItems[5].Text}";
            
            MessageBox.Show(details, "Chi tiết sân bóng", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show("Vui lòng chọn một sân để xem chi tiết", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnBookField_Click(object sender, EventArgs e)
    {
        if (listViewFields.SelectedItems.Count > 0)
        {
            var selectedItem = listViewFields.SelectedItems[0];
            int fieldId = int.Parse(selectedItem.Text);
            
            MessageBox.Show($"Chuyển đến trang đặt sân cho sân: {selectedItem.SubItems[1].Text}", "Đặt sân", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // TODO: Open BookField form or page
        }
        else
        {
            MessageBox.Show("Vui lòng chọn một sân để đặt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnRefresh_Click(object sender, EventArgs e)
    {
        LoadFields();
    }

    private void BtnBack_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
