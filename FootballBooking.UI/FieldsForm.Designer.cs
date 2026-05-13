namespace FootballBooking.UI;

partial class FieldsForm
{
    private System.ComponentModel.IContainer components = null;
    private ListView listViewFields;
    private Label lblTitle;
    private Button btnViewDetails;
    private Button btnBookField;
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
        listViewFields = new ListView();
        lblTitle = new Label();
        btnViewDetails = new Button();
        btnBookField = new Button();
        btnRefresh = new Button();
        btnBack = new Button();

        SuspendLayout();

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        lblTitle.Location = new Point(12, 12);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(140, 32);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Danh sách sân";

        // listViewFields
        listViewFields.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader() { Text = "ID", Width = 50 },
            new ColumnHeader() { Text = "Tên sân", Width = 150 },
            new ColumnHeader() { Text = "Địa điểm", Width = 150 },
            new ColumnHeader() { Text = "Loại", Width = 100 },
            new ColumnHeader() { Text = "Giá/giờ", Width = 100 },
            new ColumnHeader() { Text = "Trạng thái", Width = 80 }
        });
        listViewFields.FullRowSelect = true;
        listViewFields.GridLines = true;
        listViewFields.Location = new Point(12, 50);
        listViewFields.Name = "listViewFields";
        listViewFields.Size = new Size(760, 350);
        listViewFields.TabIndex = 1;
        listViewFields.UseCompatibleStateImageBehavior = false;
        listViewFields.View = View.Details;

        // btnViewDetails
        btnViewDetails.Location = new Point(12, 410);
        btnViewDetails.Name = "btnViewDetails";
        btnViewDetails.Size = new Size(120, 40);
        btnViewDetails.TabIndex = 2;
        btnViewDetails.Text = "Xem chi tiết";
        btnViewDetails.UseVisualStyleBackColor = true;
        btnViewDetails.Click += BtnViewDetails_Click;

        // btnBookField
        btnBookField.Location = new Point(140, 410);
        btnBookField.Name = "btnBookField";
        btnBookField.Size = new Size(120, 40);
        btnBookField.TabIndex = 3;
        btnBookField.Text = "Đặt sân";
        btnBookField.UseVisualStyleBackColor = true;
        btnBookField.Click += BtnBookField_Click;

        // btnRefresh
        btnRefresh.Location = new Point(268, 410);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(120, 40);
        btnRefresh.TabIndex = 4;
        btnRefresh.Text = "Làm mới";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += BtnRefresh_Click;

        // btnBack
        btnBack.Location = new Point(652, 410);
        btnBack.Name = "btnBack";
        btnBack.Size = new Size(120, 40);
        btnBack.TabIndex = 5;
        btnBack.Text = "Quay lại";
        btnBack.UseVisualStyleBackColor = true;
        btnBack.Click += BtnBack_Click;

        // FieldsForm
        AutoScaleDimensions = new SizeF(7, 15);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 461);
        Controls.Add(btnBack);
        Controls.Add(btnRefresh);
        Controls.Add(btnBookField);
        Controls.Add(btnViewDetails);
        Controls.Add(listViewFields);
        Controls.Add(lblTitle);
        Name = "FieldsForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Danh sách sân";
        ResumeLayout(false);
        PerformLayout();
    }
}
