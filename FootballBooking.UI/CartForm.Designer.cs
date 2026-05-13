namespace FootballBooking.UI;

partial class CartForm
{
    private System.ComponentModel.IContainer components = null;
    private ListView listViewCart;
    private Label lblTitle;
    private Label lblNewQuantity;
    private NumericUpDown numNewQuantity;
    private Button btnRemoveItem;
    private Button btnUpdateQuantity;
    private Button btnCheckout;
    private Button btnClearCart;
    private Button btnBack;
    private Label lblTotalPrice;

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
        listViewCart = new ListView();
        lblTitle = new Label();
        lblNewQuantity = new Label();
        numNewQuantity = new NumericUpDown();
        btnRemoveItem = new Button();
        btnUpdateQuantity = new Button();
        btnCheckout = new Button();
        btnClearCart = new Button();
        btnBack = new Button();
        lblTotalPrice = new Label();

        ((System.ComponentModel.ISupportInitialize)numNewQuantity).BeginInit();
        SuspendLayout();

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        lblTitle.Location = new Point(12, 12);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(110, 32);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Giỏ hàng";

        // listViewCart
        listViewCart.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader() { Text = "ID", Width = 50 },
            new ColumnHeader() { Text = "Tên", Width = 200 },
            new ColumnHeader() { Text = "Số lượng", Width = 80 },
            new ColumnHeader() { Text = "Đơn giá", Width = 100 },
            new ColumnHeader() { Text = "Thành tiền", Width = 100 }
        });
        listViewCart.FullRowSelect = true;
        listViewCart.GridLines = true;
        listViewCart.Location = new Point(12, 50);
        listViewCart.Name = "listViewCart";
        listViewCart.Size = new Size(760, 250);
        listViewCart.TabIndex = 1;
        listViewCart.UseCompatibleStateImageBehavior = false;
        listViewCart.View = View.Details;

        // lblTotalPrice
        lblTotalPrice.AutoSize = true;
        lblTotalPrice.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        lblTotalPrice.Location = new Point(12, 310);
        lblTotalPrice.Name = "lblTotalPrice";
        lblTotalPrice.Size = new Size(120, 25);
        lblTotalPrice.TabIndex = 2;
        lblTotalPrice.Text = "Tổng cộng: 0 ₫";

        // lblNewQuantity
        lblNewQuantity.AutoSize = true;
        lblNewQuantity.Location = new Point(12, 345);
        lblNewQuantity.Name = "lblNewQuantity";
        lblNewQuantity.Size = new Size(130, 15);
        lblNewQuantity.TabIndex = 3;
        lblNewQuantity.Text = "Số lượng mới:";

        // numNewQuantity
        numNewQuantity.Location = new Point(150, 340);
        numNewQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numNewQuantity.Name = "numNewQuantity";
        numNewQuantity.Size = new Size(100, 23);
        numNewQuantity.TabIndex = 4;
        numNewQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // btnRemoveItem
        btnRemoveItem.Location = new Point(12, 410);
        btnRemoveItem.Name = "btnRemoveItem";
        btnRemoveItem.Size = new Size(120, 40);
        btnRemoveItem.TabIndex = 5;
        btnRemoveItem.Text = "Xóa mục";
        btnRemoveItem.UseVisualStyleBackColor = true;
        btnRemoveItem.Click += BtnRemoveItem_Click;

        // btnUpdateQuantity
        btnUpdateQuantity.Location = new Point(140, 410);
        btnUpdateQuantity.Name = "btnUpdateQuantity";
        btnUpdateQuantity.Size = new Size(120, 40);
        btnUpdateQuantity.TabIndex = 6;
        btnUpdateQuantity.Text = "Cập nhật";
        btnUpdateQuantity.UseVisualStyleBackColor = true;
        btnUpdateQuantity.Click += BtnUpdateQuantity_Click;

        // btnClearCart
        btnClearCart.Location = new Point(268, 410);
        btnClearCart.Name = "btnClearCart";
        btnClearCart.Size = new Size(120, 40);
        btnClearCart.TabIndex = 7;
        btnClearCart.Text = "Xóa tất cả";
        btnClearCart.UseVisualStyleBackColor = true;
        btnClearCart.Click += BtnClearCart_Click;

        // btnCheckout
        btnCheckout.BackColor = Color.Green;
        btnCheckout.ForeColor = Color.White;
        btnCheckout.Location = new Point(500, 410);
        btnCheckout.Name = "btnCheckout";
        btnCheckout.Size = new Size(140, 40);
        btnCheckout.TabIndex = 8;
        btnCheckout.Text = "Tiếp tục thanh toán";
        btnCheckout.UseVisualStyleBackColor = false;
        btnCheckout.Click += BtnCheckout_Click;

        // btnBack
        btnBack.Location = new Point(652, 410);
        btnBack.Name = "btnBack";
        btnBack.Size = new Size(120, 40);
        btnBack.TabIndex = 9;
        btnBack.Text = "Quay lại";
        btnBack.UseVisualStyleBackColor = true;
        btnBack.Click += BtnBack_Click;

        // CartForm
        AutoScaleDimensions = new SizeF(7, 15);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 461);
        Controls.Add(btnBack);
        Controls.Add(btnCheckout);
        Controls.Add(btnClearCart);
        Controls.Add(btnUpdateQuantity);
        Controls.Add(btnRemoveItem);
        Controls.Add(numNewQuantity);
        Controls.Add(lblNewQuantity);
        Controls.Add(lblTotalPrice);
        Controls.Add(listViewCart);
        Controls.Add(lblTitle);
        Name = "CartForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Giỏ hàng";
        ((System.ComponentModel.ISupportInitialize)numNewQuantity).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
