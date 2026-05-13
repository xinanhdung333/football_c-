using FootballBooking.BLL;
using FootballBooking.Models;

namespace FootballBooking.UI;

public partial class FeedbackForm : Form
{
    private FeedbackService _feedbackService = null!;
    private int _userId;

    public FeedbackForm()
    {
        InitializeComponent();
    }

    public FeedbackForm(FeedbackService feedbackService, int userId) : this()
    {
        _feedbackService = feedbackService;
        _userId = userId;
        LoadFeedbacks();
    }

    private async void LoadFeedbacks()
    {
        try
        {
            var feedbacks = await _feedbackService.GetFeedbacksByUserIdAsync(_userId);
            listViewFeedbacks.Items.Clear();

            foreach (var feedback in feedbacks)
            {
                var item = new ListViewItem(feedback.Id.ToString());
                item.SubItems.Add(feedback.BookingId.ToString());
                item.SubItems.Add(feedback.Rating.ToString());
                item.SubItems.Add(feedback.Comment?.Substring(0, Math.Min(50, feedback.Comment.Length)) + "...");
                item.SubItems.Add(feedback.CreatedDate.ToString("dd/MM/yyyy HH:mm"));
                item.SubItems.Add(feedback.AdminReply ?? "Chưa trả lời");
                listViewFeedbacks.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải phản hồi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAddFeedback_Click(object sender, EventArgs e)
    {
        try
        {
            int bookingId = int.Parse(txtBookingId.Text);
            int rating = (int)numRating.Value;
            string comment = rtbComment.Text;

            if (string.IsNullOrWhiteSpace(comment))
            {
                MessageBox.Show("Vui lòng nhập bình luận", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // TODO: Call add feedback service
            MessageBox.Show("Phản hồi đã được gửi", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            txtBookingId.Clear();
            numRating.Value = 5;
            rtbComment.Clear();
            LoadFeedbacks();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnRefresh_Click(object sender, EventArgs e)
    {
        LoadFeedbacks();
    }

    private void BtnBack_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
