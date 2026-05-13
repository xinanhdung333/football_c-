using FootballBooking.BLL;

namespace FootballBooking.UI;

public partial class ChatbotForm : Form
{
    private List<(string User, string Message)> _chatHistory = new();

    public ChatbotForm()
    {
        InitializeComponent();
    }

    private void BtnSendMessage_Click(object sender, EventArgs e)
    {
        string userMessage = txtMessage.Text.Trim();
        
        if (string.IsNullOrWhiteSpace(userMessage))
        {
            return;
        }

        // Add user message to chat
        AddMessageToChat("Bạn", userMessage, Color.Blue);
        _chatHistory.Add(("User", userMessage));

        txtMessage.Clear();

        // Simulate chatbot response
        string response = GetChatbotResponse(userMessage);
        AddMessageToChat("Chatbot", response, Color.Green);
        _chatHistory.Add(("Chatbot", response));
    }

    private void AddMessageToChat(string sender, string message, Color color)
    {
        rtbChat.SelectionColor = color;
        rtbChat.AppendText($"{sender}: ");
        
        rtbChat.SelectionColor = Color.Black;
        rtbChat.AppendText($"{message}\n\n");

        // Auto scroll to bottom
        rtbChat.SelectionStart = rtbChat.Text.Length;
        rtbChat.ScrollToCaret();
    }

    private string GetChatbotResponse(string userMessage)
    {
        // TODO: Integrate with AI chatbot service
        
        string lowerMessage = userMessage.ToLower();

        if (lowerMessage.Contains("xin chào") || lowerMessage.Contains("hello"))
        {
            return "Xin chào! Tôi là trợ lý AI của ứng dụng Football Booking. Tôi có thể giúp bạn gì?";
        }
        else if (lowerMessage.Contains("đặt sân") || lowerMessage.Contains("booking"))
        {
            return "Để đặt sân, bạn có thể:\n1. Xem danh sách sân bóng\n2. Chọn sân và thời gian\n3. Thêm vào giỏ hàng\n4. Tiến hành thanh toán";
        }
        else if (lowerMessage.Contains("giá") || lowerMessage.Contains("chi phí"))
        {
            return "Giá cả các sân bóng thay đổi theo loại sân và thời gian. Bạn có thể xem chi tiết giá tại phần 'Danh sách sân'.";
        }
        else if (lowerMessage.Contains("liên hệ") || lowerMessage.Contains("hỗ trợ"))
        {
            return "Bạn có thể liên hệ với chúng tôi qua email: support@footballbooking.com hoặc hotline: 1900-xxxx";
        }
        else
        {
            return "Xin lỗi, tôi chưa hiểu câu hỏi của bạn. Vui lòng thử lại!";
        }
    }

    private void BtnClearChat_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("Bạn có chắc chắn muốn xóa lịch sử trò chuyện?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            rtbChat.Clear();
            _chatHistory.Clear();
        }
    }

    private void BtnClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
