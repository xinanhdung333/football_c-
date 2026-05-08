using FootballBooking.BLL;
using FootballBooking.DAL;

namespace FootballBooking.UI;

public partial class Form1 : Form
{
    private readonly FieldManagementService _fieldService;
    private readonly BookingService _bookingService;

    public Form1()
    {
        InitializeComponent();
        _fieldService = new FieldManagementService(new FieldRepository());
        _bookingService = new BookingService(new BookingRepository(), new FieldRepository());
        RefreshFieldGrid();
    }

    private async void RefreshFieldGrid()
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
            MessageBox.Show($"Không thể tải dữ liệu sân: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        RefreshFieldGrid();
    }

    private async void btnNewBooking_Click(object sender, EventArgs e)
    {
        if (listViewFields.SelectedItems.Count == 0)
        {
            MessageBox.Show("Chọn một sân để đặt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var selectedId = int.Parse(listViewFields.SelectedItems[0].Text);
        var bookingDate = DateTime.Today.AddDays(1);
        var startTime = new TimeSpan(18, 0, 0);
        var endTime = new TimeSpan(20, 0, 0);

        try
        {
            var bookingId = await _bookingService.CreateBookingAsync(1, selectedId, bookingDate, startTime, endTime, "Đặt sân demo từ UI");
            MessageBox.Show($"Đã tạo booking #{bookingId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshFieldGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Lỗi đặt sân", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
