namespace FootballBooking.UI;

partial class ChatbotForm
{
    private System.ComponentModel.IContainer components = null;
    private RichTextBox rtbChat;
    private TextBox txtMessage;
    private Button btnSendMessage;
    private Button btnClearChat;
    private Button btnClose;
    private Label lblTitle;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        rtbChat = new RichTextBox();
        txtMessage = new TextBox();
        btnSendMessage = new Button();
        btnClearChat = new Button();
        btnClose = new Button();
        lblTitle = new Label();

        SuspendLayout();

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        lblTitle.Location = new Point(12, 12);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(150, 32);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Trợ lý AI Chatbot";

        // rtbChat
        rtbChat.BackColor = Color.White;
        rtbChat.Location = new Point(12, 50);
        rtbChat.Name = "rtbChat";
        rtbChat.ReadOnly = true;
        rtbChat.Size = new Size(760, 320);
        rtbChat.TabIndex = 1;
        rtbChat.Text = "";

        // txtMessage
        txtMessage.Location = new Point(12, 380);
        txtMessage.Name = "txtMessage";
        txtMessage.PlaceholderText = "Nhập tin nhắn của bạn...";
        txtMessage.Size = new Size(680, 23);
        txtMessage.TabIndex = 2;

        // btnSendMessage
        btnSendMessage.Location = new Point(700, 380);
        btnSendMessage.Name = "btnSendMessage";
        btnSendMessage.Size = new Size(72, 23);
        btnSendMessage.TabIndex = 3;
        btnSendMessage.Text = "Gửi";
        btnSendMessage.UseVisualStyleBackColor = true;
        btnSendMessage.Click += BtnSendMessage_Click;

        // btnClearChat
        btnClearChat.Location = new Point(12, 410);
        btnClearChat.Name = "btnClearChat";
        btnClearChat.Size = new Size(120, 40);
        btnClearChat.TabIndex = 4;
        btnClearChat.Text = "Xóa trò chuyện";
        btnClearChat.UseVisualStyleBackColor = true;
        btnClearChat.Click += BtnClearChat_Click;

        // btnClose
        btnClose.Location = new Point(652, 410);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 40);
        btnClose.TabIndex = 5;
        btnClose.Text = "Đóng";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += BtnClose_Click;

        // ChatbotForm
        AutoScaleDimensions = new SizeF(7, 15);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 461);
        Controls.Add(btnClose);
        Controls.Add(btnClearChat);
        Controls.Add(btnSendMessage);
        Controls.Add(txtMessage);
        Controls.Add(rtbChat);
        Controls.Add(lblTitle);
        Name = "ChatbotForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Trợ lý AI Chatbot";
        ResumeLayout(false);
        PerformLayout();
    }
}
