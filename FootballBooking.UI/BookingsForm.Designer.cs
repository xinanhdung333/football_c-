namespace FootballBooking.UI;

partial class BookingsForm
{
    private System.ComponentModel.IContainer components = null;
    private ListView listViewBookings;
    private Button btnCancelBooking;
    private Button btnRefresh;
    private Button btnBack;
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
        listViewBookings = new ListView();
        btnCancelBooking = new Button();
        btnRefresh = new Button();
        btnBack = new Button();
        lblTitle = new Label();

        SuspendLayout();

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        lblTitle.Location = new Point(12, 12);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(150, 32);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Lịch sử đặt sân";

        // listViewBookings
        listViewBookings.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader() { Text = "ID", Width = 50 },
            new ColumnHeader() { Text = "Tên sân", Width = 150 },
            new ColumnHeader() { Text = "Ngày đặt", Width = 100 },
            new ColumnHeader() { Text = "Thời gian", Width = 100 },
            new ColumnHeader() { Text = "Giá", Width = 100 },
            new ColumnHeader() { Text = "Trạng thái", Width = 80 }
        });
        listViewBookings.FullRowSelect = true;
        listViewBookings.GridLines = true;
        listViewBookings.Location = new Point(12, 50);
        listViewBookings.Name = "listViewBookings";
        listViewBookings.Size = new Size(760, 350);
        listViewBookings.TabIndex = 1;
        listViewBookings.UseCompatibleStateImageBehavior = false;
        listViewBookings.View = View.Details;

        // btnCancelBooking
        btnCancelBooking.Location = new Point(12, 410);
        btnCancelBooking.Name = "btnCancelBooking";
        btnCancelBooking.Size = new Size(120, 40);
        btnCancelBooking.TabIndex = 2;
        btnCancelBooking.Text = "Hủy đặt sân";
        btnCancelBooking.UseVisualStyleBackColor = true;
        btnCancelBooking.Click += BtnCancelBooking_Click;

        // btnRefresh
        btnRefresh.Location = new Point(140, 410);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(120, 40);
        btnRefresh.TabIndex = 3;
        btnRefresh.Text = "Làm mới";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += BtnRefresh_Click;

        // btnBack
        btnBack.Location = new Point(652, 410);
        btnBack.Name = "btnBack";
        btnBack.Size = new Size(120, 40);
        btnBack.TabIndex = 4;
        btnBack.Text = "Quay lại";
        btnBack.UseVisualStyleBackColor = true;
        btnBack.Click += BtnBack_Click;

        // BookingsForm
        AutoScaleDimensions = new SizeF(7, 15);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 461);
        Controls.Add(btnBack);
        Controls.Add(btnRefresh);
        Controls.Add(btnCancelBooking);
        Controls.Add(listViewBookings);
        Controls.Add(lblTitle);
        Name = "BookingsForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Lịch sử đặt sân";
        ResumeLayout(false);
        PerformLayout();
    }
}
