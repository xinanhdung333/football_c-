namespace FootballBooking.UI;

partial class ServicesForm
{
    private System.ComponentModel.IContainer components = null;
    private ListView listViewServices;
    private Label lblTitle;
    private Label lblQuantity;
    private NumericUpDown numQuantity;
    private Button btnViewDetails;
    private Button btnAddToCart;
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
        listViewServices = new ListView();
        lblTitle = new Label();
        lblQuantity = new Label();
        numQuantity = new NumericUpDown();
        btnViewDetails = new Button();
        btnAddToCart = new Button();
        btnRefresh = new Button();
        btnBack = new Button();

        ((System.ComponentModel.ISupportInitialize)numQuantity).BeginInit();
        SuspendLayout();

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        lblTitle.Location = new Point(12, 12);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(130, 32);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Danh sách dịch vụ";

        // listViewServices
        listViewServices.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader() { Text = "ID", Width = 50 },
            new ColumnHeader() { Text = "Tên dịch vụ", Width = 150 },
            new ColumnHeader() { Text = "Mô tả", Width = 250 },
            new ColumnHeader() { Text = "Giá", Width = 100 },
            new ColumnHeader() { Text = "Số lượng", Width = 80 }
        });
        listViewServices.FullRowSelect = true;
        listViewServices.GridLines = true;
        listViewServices.Location = new Point(12, 50);
        listViewServices.Name = "listViewServices";
        listViewServices.Size = new Size(760, 300);
        listViewServices.TabIndex = 1;
        listViewServices.UseCompatibleStateImageBehavior = false;
        listViewServices.View = View.Details;

        // lblQuantity
        lblQuantity.AutoSize = true;
        lblQuantity.Location = new Point(12, 365);
        lblQuantity.Name = "lblQuantity";
        lblQuantity.Size = new Size(70, 15);
        lblQuantity.TabIndex = 2;
        lblQuantity.Text = "Số lượng:";

        // numQuantity
        numQuantity.Location = new Point(100, 360);
        numQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numQuantity.Name = "numQuantity";
        numQuantity.Size = new Size(100, 23);
        numQuantity.TabIndex = 3;
        numQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // btnViewDetails
        btnViewDetails.Location = new Point(12, 410);
        btnViewDetails.Name = "btnViewDetails";
        btnViewDetails.Size = new Size(120, 40);
        btnViewDetails.TabIndex = 4;
        btnViewDetails.Text = "Xem chi tiết";
        btnViewDetails.UseVisualStyleBackColor = true;
        btnViewDetails.Click += BtnViewDetails_Click;

        // btnAddToCart
        btnAddToCart.Location = new Point(140, 410);
        btnAddToCart.Name = "btnAddToCart";
        btnAddToCart.Size = new Size(120, 40);
        btnAddToCart.TabIndex = 5;
        btnAddToCart.Text = "Thêm vào giỏ";
        btnAddToCart.UseVisualStyleBackColor = true;
        btnAddToCart.Click += BtnAddToCart_Click;

        // btnRefresh
        btnRefresh.Location = new Point(268, 410);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(120, 40);
        btnRefresh.TabIndex = 6;
        btnRefresh.Text = "Làm mới";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += BtnRefresh_Click;

        // btnBack
        btnBack.Location = new Point(652, 410);
        btnBack.Name = "btnBack";
        btnBack.Size = new Size(120, 40);
        btnBack.TabIndex = 7;
        btnBack.Text = "Quay lại";
        btnBack.UseVisualStyleBackColor = true;
        btnBack.Click += BtnBack_Click;

        // ServicesForm
        AutoScaleDimensions = new SizeF(7, 15);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 461);
        Controls.Add(btnBack);
        Controls.Add(btnRefresh);
        Controls.Add(btnAddToCart);
        Controls.Add(btnViewDetails);
        Controls.Add(numQuantity);
        Controls.Add(lblQuantity);
        Controls.Add(listViewServices);
        Controls.Add(lblTitle);
        Name = "ServicesForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Danh sách dịch vụ";
        ((System.ComponentModel.ISupportInitialize)numQuantity).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
