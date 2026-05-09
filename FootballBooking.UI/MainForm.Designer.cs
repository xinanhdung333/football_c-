namespace FootballBooking.UI;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private TabControl tabControl;
    private TabPage tabFields;
    private TabPage tabServices;
    private TabPage tabCart;
    private ListView listViewFields;
    private ListView listViewServices;
    private ListView listViewCart;
    private Button btnBookField;
    private Button btnAddToCart;
    private Button btnCheckout;
    private Button btnLogout;
    private DateTimePicker dateTimePicker;
    private NumericUpDown numStartHour;
    private NumericUpDown numStartMinute;
    private NumericUpDown numEndHour;
    private NumericUpDown numEndMinute;
    private NumericUpDown numQuantity;
    private TextBox txtNote;
    private Label lblTotal;
    private Label lblStartTime;
    private Label lblEndTime;
    private Label lblQuantity;
    private Label lblNote;

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
        tabControl = new TabControl();
        tabFields = new TabPage();
        tabServices = new TabPage();
        tabCart = new TabPage();
        listViewFields = new ListView();
        listViewServices = new ListView();
        listViewCart = new ListView();
        btnBookField = new Button();
        btnAddToCart = new Button();
        btnCheckout = new Button();
        btnLogout = new Button();
        dateTimePicker = new DateTimePicker();
        numStartHour = new NumericUpDown();
        numStartMinute = new NumericUpDown();
        numEndHour = new NumericUpDown();
        numEndMinute = new NumericUpDown();
        numQuantity = new NumericUpDown();
        txtNote = new TextBox();
        lblTotal = new Label();
        lblStartTime = new Label();
        lblEndTime = new Label();
        lblQuantity = new Label();
        lblNote = new Label();

        tabControl.SuspendLayout();
        tabFields.SuspendLayout();
        tabServices.SuspendLayout();
        tabCart.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numStartHour).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numStartMinute).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numEndHour).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numEndMinute).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numQuantity).BeginInit();
        SuspendLayout();

        // tabControl
        tabControl.Controls.Add(tabFields);
        tabControl.Controls.Add(tabServices);
        tabControl.Controls.Add(tabCart);
        tabControl.Location = new Point(12, 12);
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(776, 426);
        tabControl.TabIndex = 0;

        // tabFields
        tabFields.Controls.Add(lblNote);
        tabFields.Controls.Add(txtNote);
        tabFields.Controls.Add(lblEndTime);
        tabFields.Controls.Add(numEndMinute);
        tabFields.Controls.Add(numEndHour);
        tabFields.Controls.Add(lblStartTime);
        tabFields.Controls.Add(numStartMinute);
        tabFields.Controls.Add(numStartHour);
        tabFields.Controls.Add(dateTimePicker);
        tabFields.Controls.Add(btnBookField);
        tabFields.Controls.Add(listViewFields);
        tabFields.Location = new Point(4, 22);
        tabFields.Name = "tabFields";
        tabFields.Padding = new Padding(3);
        tabFields.Size = new Size(768, 400);
        tabFields.TabIndex = 0;
        tabFields.Text = "Đặt sân";
        tabFields.UseVisualStyleBackColor = true;

        // listViewFields
        listViewFields.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader() { Text = "ID", Width = 50 },
            new ColumnHeader() { Text = "Tên sân", Width = 150 },
            new ColumnHeader() { Text = "Địa điểm", Width = 150 },
            new ColumnHeader() { Text = "Giá/giờ", Width = 100 },
            new ColumnHeader() { Text = "Mô tả", Width = 200 }
        });
        listViewFields.FullRowSelect = true;
        listViewFields.GridLines = true;
        listViewFields.Location = new Point(6, 6);
        listViewFields.Name = "listViewFields";
        listViewFields.Size = new Size(756, 200);
        listViewFields.TabIndex = 0;
        listViewFields.UseCompatibleStateImageBehavior = false;
        listViewFields.View = View.Details;

        // dateTimePicker
        dateTimePicker.Location = new Point(100, 220);
        dateTimePicker.Name = "dateTimePicker";
        dateTimePicker.Size = new Size(150, 23);
        dateTimePicker.TabIndex = 1;

        // lblStartTime
        lblStartTime.AutoSize = true;
        lblStartTime.Location = new Point(6, 250);
        lblStartTime.Name = "lblStartTime";
        lblStartTime.Size = new Size(60, 15);
        lblStartTime.TabIndex = 2;
        lblStartTime.Text = "Giờ bắt đầu:";

        // numStartHour
        numStartHour.Location = new Point(100, 245);
        numStartHour.Maximum = 23;
        numStartHour.Name = "numStartHour";
        numStartHour.Size = new Size(50, 23);
        numStartHour.TabIndex = 3;

        // numStartMinute
        numStartMinute.Location = new Point(160, 245);
        numStartMinute.Maximum = 59;
        numStartMinute.Name = "numStartMinute";
        numStartMinute.Size = new Size(50, 23);
        numStartMinute.TabIndex = 4;

        // lblEndTime
        lblEndTime.AutoSize = true;
        lblEndTime.Location = new Point(6, 280);
        lblEndTime.Name = "lblEndTime";
        lblEndTime.Size = new Size(50, 15);
        lblEndTime.TabIndex = 5;
        lblEndTime.Text = "Giờ kết thúc:";

        // numEndHour
        numEndHour.Location = new Point(100, 275);
        numEndHour.Maximum = 23;
        numEndHour.Name = "numEndHour";
        numEndHour.Size = new Size(50, 23);
        numEndHour.TabIndex = 6;

        // numEndMinute
        numEndMinute.Location = new Point(160, 275);
        numEndMinute.Maximum = 59;
        numEndMinute.Name = "numEndMinute";
        numEndMinute.Size = new Size(50, 23);
        numEndMinute.TabIndex = 7;

        // lblNote
        lblNote.AutoSize = true;
        lblNote.Location = new Point(6, 310);
        lblNote.Name = "lblNote";
        lblNote.Size = new Size(35, 15);
        lblNote.TabIndex = 8;
        lblNote.Text = "Ghi chú:";

        // txtNote
        txtNote.Location = new Point(100, 305);
        txtNote.Multiline = true;
        txtNote.Name = "txtNote";
        txtNote.Size = new Size(300, 50);
        txtNote.TabIndex = 9;

        // btnBookField
        btnBookField.Location = new Point(100, 365);
        btnBookField.Name = "btnBookField";
        btnBookField.Size = new Size(100, 30);
        btnBookField.TabIndex = 10;
        btnBookField.Text = "Đặt sân";
        btnBookField.UseVisualStyleBackColor = true;
        btnBookField.Click += btnBookField_Click;

        // tabServices
        tabServices.Controls.Add(lblQuantity);
        tabServices.Controls.Add(numQuantity);
        tabServices.Controls.Add(btnAddToCart);
        tabServices.Controls.Add(listViewServices);
        tabServices.Location = new Point(4, 22);
        tabServices.Name = "tabServices";
        tabServices.Padding = new Padding(3);
        tabServices.Size = new Size(768, 400);
        tabServices.TabIndex = 1;
        tabServices.Text = "Dịch vụ";
        tabServices.UseVisualStyleBackColor = true;

        // listViewServices
        listViewServices.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader() { Text = "ID", Width = 50 },
            new ColumnHeader() { Text = "Tên dịch vụ", Width = 150 },
            new ColumnHeader() { Text = "Mô tả", Width = 170 },
            new ColumnHeader() { Text = "Giá gốc", Width = 90 },
            new ColumnHeader() { Text = "Giá sau giảm", Width = 100 },
            new ColumnHeader() { Text = "Giảm", Width = 70 },
            new ColumnHeader() { Text = "Tồn kho", Width = 80 }
        });
        listViewServices.FullRowSelect = true;
        listViewServices.GridLines = true;
        listViewServices.Location = new Point(6, 6);
        listViewServices.Name = "listViewServices";
        listViewServices.Size = new Size(756, 300);
        listViewServices.TabIndex = 0;
        listViewServices.UseCompatibleStateImageBehavior = false;
        listViewServices.View = View.Details;

        // lblQuantity
        lblQuantity.AutoSize = true;
        lblQuantity.Location = new Point(6, 320);
        lblQuantity.Name = "lblQuantity";
        lblQuantity.Size = new Size(50, 15);
        lblQuantity.TabIndex = 1;
        lblQuantity.Text = "Số lượng:";

        // numQuantity
        numQuantity.Location = new Point(80, 315);
        numQuantity.Minimum = 1;
        numQuantity.Name = "numQuantity";
        numQuantity.Size = new Size(60, 23);
        numQuantity.TabIndex = 2;
        numQuantity.Value = 1;

        // btnAddToCart
        btnAddToCart.Location = new Point(160, 315);
        btnAddToCart.Name = "btnAddToCart";
        btnAddToCart.Size = new Size(100, 30);
        btnAddToCart.TabIndex = 3;
        btnAddToCart.Text = "Thêm vào giỏ";
        btnAddToCart.UseVisualStyleBackColor = true;
        btnAddToCart.Click += btnAddToCart_Click;

        // tabCart
        tabCart.Controls.Add(lblTotal);
        tabCart.Controls.Add(btnCheckout);
        tabCart.Controls.Add(listViewCart);
        tabCart.Location = new Point(4, 22);
        tabCart.Name = "tabCart";
        tabCart.Padding = new Padding(3);
        tabCart.Size = new Size(768, 400);
        tabCart.TabIndex = 2;
        tabCart.Text = "Giỏ hàng";
        tabCart.UseVisualStyleBackColor = true;

        // listViewCart
        listViewCart.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader() { Text = "Dịch vụ", Width = 200 },
            new ColumnHeader() { Text = "Số lượng", Width = 80 },
            new ColumnHeader() { Text = "Đơn giá", Width = 100 },
            new ColumnHeader() { Text = "Thành tiền", Width = 100 }
        });
        listViewCart.FullRowSelect = true;
        listViewCart.GridLines = true;
        listViewCart.Location = new Point(6, 6);
        listViewCart.Name = "listViewCart";
        listViewCart.Size = new Size(756, 300);
        listViewCart.TabIndex = 0;
        listViewCart.UseCompatibleStateImageBehavior = false;
        listViewCart.View = View.Details;

        // lblTotal
        lblTotal.AutoSize = true;
        lblTotal.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotal.Location = new Point(6, 320);
        lblTotal.Name = "lblTotal";
        lblTotal.Size = new Size(80, 21);
        lblTotal.TabIndex = 1;
        lblTotal.Text = "Tổng tiền: 0đ";

        // btnCheckout
        btnCheckout.Location = new Point(600, 315);
        btnCheckout.Name = "btnCheckout";
        btnCheckout.Size = new Size(100, 30);
        btnCheckout.TabIndex = 2;
        btnCheckout.Text = "Thanh toán";
        btnCheckout.UseVisualStyleBackColor = true;
        btnCheckout.Click += btnCheckout_Click;

        // btnLogout
        btnLogout.Location = new Point(700, 450);
        btnLogout.Name = "btnLogout";
        btnLogout.Size = new Size(80, 30);
        btnLogout.TabIndex = 1;
        btnLogout.Text = "Đăng xuất";
        btnLogout.UseVisualStyleBackColor = true;
        btnLogout.Click += btnLogout_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 490);
        Controls.Add(btnLogout);
        Controls.Add(tabControl);
        Name = "MainForm";
        Text = "Football Booking - Trang chủ";
        StartPosition = FormStartPosition.CenterScreen;
        tabControl.ResumeLayout(false);
        tabFields.ResumeLayout(false);
        tabFields.PerformLayout();
        tabServices.ResumeLayout(false);
        tabServices.PerformLayout();
        tabCart.ResumeLayout(false);
        tabCart.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numStartHour).EndInit();
        ((System.ComponentModel.ISupportInitialize)numStartMinute).EndInit();
        ((System.ComponentModel.ISupportInitialize)numEndHour).EndInit();
        ((System.ComponentModel.ISupportInitialize)numEndMinute).EndInit();
        ((System.ComponentModel.ISupportInitialize)numQuantity).EndInit();
        ResumeLayout(false);
    }
}
