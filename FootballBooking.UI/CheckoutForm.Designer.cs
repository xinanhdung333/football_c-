namespace FootballBooking.UI;

partial class CheckoutForm
{
    private System.ComponentModel.IContainer components = null;
    private ListView listViewItems;
    private Label lblTotal;
    private Label lblPaymentMethod;
    private ComboBox cmbPaymentMethod;
    private Button btnConfirmPayment;
    private Button btnCancel;

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
        listViewItems = new ListView();
        lblTotal = new Label();
        lblPaymentMethod = new Label();
        cmbPaymentMethod = new ComboBox();
        btnConfirmPayment = new Button();
        btnCancel = new Button();

        SuspendLayout();

        // listViewItems
        listViewItems.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader() { Text = "Dịch vụ", Width = 200 },
            new ColumnHeader() { Text = "Số lượng", Width = 80 },
            new ColumnHeader() { Text = "Giá gốc", Width = 90 },
            new ColumnHeader() { Text = "Đơn giá", Width = 90 },
            new ColumnHeader() { Text = "Giảm", Width = 60 },
            new ColumnHeader() { Text = "Thành tiền", Width = 100 }
        });
        listViewItems.FullRowSelect = true;
        listViewItems.GridLines = true;
        listViewItems.Location = new Point(12, 12);
        listViewItems.Name = "listViewItems";
        listViewItems.Size = new Size(656, 200);
        listViewItems.TabIndex = 0;
        listViewItems.UseCompatibleStateImageBehavior = false;
        listViewItems.View = View.Details;

        // lblTotal
        lblTotal.AutoSize = true;
        lblTotal.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotal.Location = new Point(12, 230);
        lblTotal.Name = "lblTotal";
        lblTotal.Size = new Size(80, 21);
        lblTotal.TabIndex = 1;
        lblTotal.Text = "Tổng tiền: 0đ";

        // lblPaymentMethod
        lblPaymentMethod.AutoSize = true;
        lblPaymentMethod.Location = new Point(12, 270);
        lblPaymentMethod.Name = "lblPaymentMethod";
        lblPaymentMethod.Size = new Size(150, 15);
        lblPaymentMethod.TabIndex = 2;
        lblPaymentMethod.Text = "Phương thức thanh toán:";

        // cmbPaymentMethod
        cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPaymentMethod.FormattingEnabled = true;
        cmbPaymentMethod.Items.AddRange(new object[] { "Tiền mặt", "Ví MoMo" });
        cmbPaymentMethod.Location = new Point(170, 265);
        cmbPaymentMethod.Name = "cmbPaymentMethod";
        cmbPaymentMethod.Size = new Size(150, 23);
        cmbPaymentMethod.TabIndex = 3;

        // btnConfirmPayment
        btnConfirmPayment.Location = new Point(500, 260);
        btnConfirmPayment.Name = "btnConfirmPayment";
        btnConfirmPayment.Size = new Size(100, 30);
        btnConfirmPayment.TabIndex = 4;
        btnConfirmPayment.Text = "Xác nhận";
        btnConfirmPayment.UseVisualStyleBackColor = true;
        btnConfirmPayment.Click += btnConfirmPayment_Click;

        // btnCancel
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(610, 260);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(60, 30);
        btnCancel.TabIndex = 5;
        btnCancel.Text = "Hủy";
        btnCancel.UseVisualStyleBackColor = true;

        AcceptButton = btnConfirmPayment;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(680, 310);
        Controls.Add(btnCancel);
        Controls.Add(btnConfirmPayment);
        Controls.Add(cmbPaymentMethod);
        Controls.Add(lblPaymentMethod);
        Controls.Add(lblTotal);
        Controls.Add(listViewItems);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CheckoutForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Xác nhận thanh toán";
        ResumeLayout(false);
        PerformLayout();
    }
}
