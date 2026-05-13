namespace FootballBooking.UI;

partial class FeedbackForm
{
    private System.ComponentModel.IContainer components = null;
    private ListView listViewFeedbacks;
    private Label lblTitle;
    private Label lblBookingId;
    private TextBox txtBookingId;
    private Label lblRating;
    private NumericUpDown numRating;
    private Label lblComment;
    private RichTextBox rtbComment;
    private Button btnAddFeedback;
    private Button btnRefresh;
    private Button btnBack;

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
        listViewFeedbacks = new ListView();
        lblTitle = new Label();
        lblBookingId = new Label();
        txtBookingId = new TextBox();
        lblRating = new Label();
        numRating = new NumericUpDown();
        lblComment = new Label();
        rtbComment = new RichTextBox();
        btnAddFeedback = new Button();
        btnRefresh = new Button();
        btnBack = new Button();

        ((System.ComponentModel.ISupportInitialize)numRating).BeginInit();
        SuspendLayout();

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        lblTitle.Location = new Point(12, 12);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(120, 32);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Phản hồi";

        // listViewFeedbacks
        listViewFeedbacks.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader() { Text = "ID", Width = 40 },
            new ColumnHeader() { Text = "Booking ID", Width = 80 },
            new ColumnHeader() { Text = "Đánh giá", Width = 60 },
            new ColumnHeader() { Text = "Bình luận", Width = 300 },
            new ColumnHeader() { Text = "Ngày tạo", Width = 120 },
            new ColumnHeader() { Text = "Trả lời", Width = 150 }
        });
        listViewFeedbacks.FullRowSelect = true;
        listViewFeedbacks.GridLines = true;
        listViewFeedbacks.Location = new Point(12, 50);
        listViewFeedbacks.Name = "listViewFeedbacks";
        listViewFeedbacks.Size = new Size(760, 250);
        listViewFeedbacks.TabIndex = 1;
        listViewFeedbacks.UseCompatibleStateImageBehavior = false;
        listViewFeedbacks.View = View.Details;

        // lblBookingId
        lblBookingId.AutoSize = true;
        lblBookingId.Location = new Point(12, 310);
        lblBookingId.Name = "lblBookingId";
        lblBookingId.Size = new Size(80, 15);
        lblBookingId.TabIndex = 2;
        lblBookingId.Text = "Booking ID:";

        // txtBookingId
        txtBookingId.Location = new Point(100, 305);
        txtBookingId.Name = "txtBookingId";
        txtBookingId.Size = new Size(100, 23);
        txtBookingId.TabIndex = 3;

        // lblRating
        lblRating.AutoSize = true;
        lblRating.Location = new Point(220, 310);
        lblRating.Name = "lblRating";
        lblRating.Size = new Size(60, 15);
        lblRating.TabIndex = 4;
        lblRating.Text = "Đánh giá:";

        // numRating
        numRating.Location = new Point(290, 305);
        numRating.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numRating.Name = "numRating";
        numRating.Size = new Size(80, 23);
        numRating.TabIndex = 5;
        numRating.Value = new decimal(new int[] { 5, 0, 0, 0 });

        // lblComment
        lblComment.AutoSize = true;
        lblComment.Location = new Point(12, 340);
        lblComment.Name = "lblComment";
        lblComment.Size = new Size(70, 15);
        lblComment.TabIndex = 6;
        lblComment.Text = "Bình luận:";

        // rtbComment
        rtbComment.Location = new Point(12, 360);
        rtbComment.Name = "rtbComment";
        rtbComment.Size = new Size(358, 60);
        rtbComment.TabIndex = 7;
        rtbComment.Text = "";

        // btnAddFeedback
        btnAddFeedback.Location = new Point(12, 430);
        btnAddFeedback.Name = "btnAddFeedback";
        btnAddFeedback.Size = new Size(120, 40);
        btnAddFeedback.TabIndex = 8;
        btnAddFeedback.Text = "Gửi phản hồi";
        btnAddFeedback.UseVisualStyleBackColor = true;
        btnAddFeedback.Click += BtnAddFeedback_Click;

        // btnRefresh
        btnRefresh.Location = new Point(140, 430);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(120, 40);
        btnRefresh.TabIndex = 9;
        btnRefresh.Text = "Làm mới";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += BtnRefresh_Click;

        // btnBack
        btnBack.Location = new Point(652, 430);
        btnBack.Name = "btnBack";
        btnBack.Size = new Size(120, 40);
        btnBack.TabIndex = 10;
        btnBack.Text = "Quay lại";
        btnBack.UseVisualStyleBackColor = true;
        btnBack.Click += BtnBack_Click;

        // FeedbackForm
        AutoScaleDimensions = new SizeF(7, 15);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 481);
        Controls.Add(btnBack);
        Controls.Add(btnRefresh);
        Controls.Add(btnAddFeedback);
        Controls.Add(rtbComment);
        Controls.Add(lblComment);
        Controls.Add(numRating);
        Controls.Add(lblRating);
        Controls.Add(txtBookingId);
        Controls.Add(lblBookingId);
        Controls.Add(listViewFeedbacks);
        Controls.Add(lblTitle);
        Name = "FeedbackForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Quản lý phản hồi";
        ((System.ComponentModel.ISupportInitialize)numRating).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
