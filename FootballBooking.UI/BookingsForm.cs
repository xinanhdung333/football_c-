using FootballBooking.BLL;
using FootballBooking.Models;

namespace FootballBooking.UI;

public partial class BookingsForm : Form
{
    private BookingService _bookingService = null!;
    private int _userId;

    public BookingsForm()
    {
        InitializeComponent();
    }

    public BookingsForm(BookingService bookingService, int userId) : this()
    {
        _bookingService = bookingService;
        _userId = userId;
        LoadBookings();
    }

    private async void LoadBookings()
    {
        try
        {
            var bookings = await _bookingService.GetBookingsByUserIdAsync(_userId);
            listViewBookings.Items.Clear();

            foreach (var booking in bookings)
            {
                var item = new ListViewItem(booking.Id.ToString());
                item.SubItems.Add(booking.FieldName);
                item.SubItems.Add(booking.BookingDate.ToString("dd/MM/yyyy"));
                item.SubItems.Add($"{booking.StartTime:HH:mm} - {booking.EndTime:HH:mm}");
                item.SubItems.Add(booking.TotalPrice.ToString("C"));
                item.SubItems.Add(booking.Status);
                listViewBookings.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải lịch sử đặt sân: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCancelBooking_Click(object sender, EventArgs e)
    {
        if (listViewBookings.SelectedItems.Count > 0)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn hủy đặt sân này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var selectedItem = listViewBookings.SelectedItems[0];
                int bookingId = int.Parse(selectedItem.Text);
                
                // TODO: Call cancel booking service
                MessageBox.Show("Đặt sân đã được hủy", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadBookings();
            }
        }
        else
        {
            MessageBox.Show("Vui lòng chọn một đặt sân để hủy", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnRefresh_Click(object sender, EventArgs e)
    {
        LoadBookings();
    }

    private void BtnBack_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
