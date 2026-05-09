namespace FootballBooking.UI;

partial class AdminForm
{
    private System.ComponentModel.IContainer components = null;
    private TabControl tabControl;
    private TabPage tabFields;
    private TabPage tabServices;
    private TabPage tabDiscounts;
    private ListView listViewFields;
    private ListView listViewServices;
    private ListView listViewDiscounts;
    private Button btnUpdateFieldPrice;
    private Button btnUpdateStock;
    private Button btnCreateDiscount;
    private Button btnDeleteDiscount;
    private Button btnLogout;
    private TextBox txtNewPrice;
    private TextBox txtNewStock;
    private TextBox txtDiscountNote;
    private ComboBox cmbDiscountService;
    private DateTimePicker discountStartPicker;
    private DateTimePicker discountEndPicker;
    private NumericUpDown numDiscountPercent;
    private CheckBox chkDiscountActive;
    private Label lblNewPrice;
    private Label lblNewStock;
    private Label lblDiscountService;
    private Label lblDiscountStart;
    private Label lblDiscountEnd;
    private Label lblDiscountPercent;
    private Label lblDiscountNote;

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
        tabDiscounts = new TabPage();
        listViewFields = new ListView();
        listViewServices = new ListView();
        listViewDiscounts = new ListView();
        btnUpdateFieldPrice = new Button();
        btnUpdateStock = new Button();
        btnCreateDiscount = new Button();
        btnDeleteDiscount = new Button();
        btnLogout = new Button();
        txtNewPrice = new TextBox();
        txtNewStock = new TextBox();
        txtDiscountNote = new TextBox();
        cmbDiscountService = new ComboBox();
        discountStartPicker = new DateTimePicker();
        discountEndPicker = new DateTimePicker();
        numDiscountPercent = new NumericUpDown();
        chkDiscountActive = new CheckBox();
        lblNewPrice = new Label();
        lblNewStock = new Label();
        lblDiscountService = new Label();
        lblDiscountStart = new Label();
        lblDiscountEnd = new Label();
        lblDiscountPercent = new Label();
        lblDiscountNote = new Label();

        tabControl.SuspendLayout();
        tabFields.SuspendLayout();
        tabServices.SuspendLayout();
        tabDiscounts.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numDiscountPercent).BeginInit();
        SuspendLayout();

        // tabControl
        tabControl.Controls.Add(tabFields);
        tabControl.Controls.Add(tabServices);
        tabControl.Controls.Add(tabDiscounts);
        tabControl.Location = new Point(12, 12);
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(776, 426);
        tabControl.TabIndex = 0;

        // tabFields
        tabFields.Controls.Add(lblNewPrice);
        tabFields.Controls.Add(txtNewPrice);
        tabFields.Controls.Add(btnUpdateFieldPrice);
        tabFields.Controls.Add(listViewFields);
        tabFields.Location = new Point(4, 22);
        tabFields.Name = "tabFields";
        tabFields.Padding = new Padding(3);
        tabFields.Size = new Size(768, 400);
        tabFields.TabIndex = 0;
        tabFields.Text = "Quản lý sân";
        tabFields.UseVisualStyleBackColor = true;

        // listViewFields
        listViewFields.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader() { Text = "ID", Width = 50 },
            new ColumnHeader() { Text = "Tên sân", Width = 150 },
            new ColumnHeader() { Text = "Địa điểm", Width = 150 },
            new ColumnHeader() { Text = "Giá/giờ", Width = 100 },
            new ColumnHeader() { Text = "Trạng thái", Width = 100 }
        });
        listViewFields.FullRowSelect = true;
        listViewFields.GridLines = true;
        listViewFields.Location = new Point(6, 6);
        listViewFields.Name = "listViewFields";
        listViewFields.Size = new Size(756, 300);
        listViewFields.TabIndex = 0;
        listViewFields.UseCompatibleStateImageBehavior = false;
        listViewFields.View = View.Details;

        // lblNewPrice
        lblNewPrice.AutoSize = true;
        lblNewPrice.Location = new Point(6, 320);
        lblNewPrice.Name = "lblNewPrice";
        lblNewPrice.Size = new Size(60, 15);
        lblNewPrice.TabIndex = 1;
        lblNewPrice.Text = "Giá mới:";

        // txtNewPrice
        txtNewPrice.Location = new Point(80, 315);
        txtNewPrice.Name = "txtNewPrice";
        txtNewPrice.Size = new Size(100, 23);
        txtNewPrice.TabIndex = 2;

        // btnUpdateFieldPrice
        btnUpdateFieldPrice.Location = new Point(200, 315);
        btnUpdateFieldPrice.Name = "btnUpdateFieldPrice";
        btnUpdateFieldPrice.Size = new Size(120, 30);
        btnUpdateFieldPrice.TabIndex = 3;
        btnUpdateFieldPrice.Text = "Cập nhật giá";
        btnUpdateFieldPrice.UseVisualStyleBackColor = true;
        btnUpdateFieldPrice.Click += btnUpdateFieldPrice_Click;

        // tabServices
        tabServices.Controls.Add(lblNewStock);
        tabServices.Controls.Add(txtNewStock);
        tabServices.Controls.Add(btnUpdateStock);
        tabServices.Controls.Add(listViewServices);
        tabServices.Location = new Point(4, 22);
        tabServices.Name = "tabServices";
        tabServices.Padding = new Padding(3);
        tabServices.Size = new Size(768, 400);
        tabServices.TabIndex = 1;
        tabServices.Text = "Quản lý dịch vụ";
        tabServices.UseVisualStyleBackColor = true;

        // listViewServices
        listViewServices.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader() { Text = "ID", Width = 50 },
            new ColumnHeader() { Text = "Tên dịch vụ", Width = 150 },
            new ColumnHeader() { Text = "Mô tả", Width = 200 },
            new ColumnHeader() { Text = "Giá", Width = 100 },
            new ColumnHeader() { Text = "Tồn kho", Width = 80 },
            new ColumnHeader() { Text = "Trạng thái", Width = 80 }
        });
        listViewServices.FullRowSelect = true;
        listViewServices.GridLines = true;
        listViewServices.Location = new Point(6, 6);
        listViewServices.Name = "listViewServices";
        listViewServices.Size = new Size(756, 300);
        listViewServices.TabIndex = 0;
        listViewServices.UseCompatibleStateImageBehavior = false;
        listViewServices.View = View.Details;

        // lblNewStock
        lblNewStock.AutoSize = true;
        lblNewStock.Location = new Point(6, 320);
        lblNewStock.Name = "lblNewStock";
        lblNewStock.Size = new Size(70, 15);
        lblNewStock.TabIndex = 1;
        lblNewStock.Text = "Tồn kho mới:";

        // txtNewStock
        txtNewStock.Location = new Point(90, 315);
        txtNewStock.Name = "txtNewStock";
        txtNewStock.Size = new Size(80, 23);
        txtNewStock.TabIndex = 2;

        // btnUpdateStock
        btnUpdateStock.Location = new Point(190, 315);
        btnUpdateStock.Name = "btnUpdateStock";
        btnUpdateStock.Size = new Size(120, 30);
        btnUpdateStock.TabIndex = 3;
        btnUpdateStock.Text = "Cập nhật tồn kho";
        btnUpdateStock.UseVisualStyleBackColor = true;
        btnUpdateStock.Click += btnUpdateStock_Click;

        // tabDiscounts
        tabDiscounts.Controls.Add(btnDeleteDiscount);
        tabDiscounts.Controls.Add(chkDiscountActive);
        tabDiscounts.Controls.Add(btnCreateDiscount);
        tabDiscounts.Controls.Add(txtDiscountNote);
        tabDiscounts.Controls.Add(lblDiscountNote);
        tabDiscounts.Controls.Add(numDiscountPercent);
        tabDiscounts.Controls.Add(lblDiscountPercent);
        tabDiscounts.Controls.Add(discountEndPicker);
        tabDiscounts.Controls.Add(lblDiscountEnd);
        tabDiscounts.Controls.Add(discountStartPicker);
        tabDiscounts.Controls.Add(lblDiscountStart);
        tabDiscounts.Controls.Add(cmbDiscountService);
        tabDiscounts.Controls.Add(lblDiscountService);
        tabDiscounts.Controls.Add(listViewDiscounts);
        tabDiscounts.Location = new Point(4, 22);
        tabDiscounts.Name = "tabDiscounts";
        tabDiscounts.Padding = new Padding(3);
        tabDiscounts.Size = new Size(768, 400);
        tabDiscounts.TabIndex = 2;
        tabDiscounts.Text = "Giảm giá dịch vụ";
        tabDiscounts.UseVisualStyleBackColor = true;

        // listViewDiscounts
        listViewDiscounts.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader() { Text = "ID", Width = 50 },
            new ColumnHeader() { Text = "Dịch vụ", Width = 160 },
            new ColumnHeader() { Text = "Khung giờ", Width = 120 },
            new ColumnHeader() { Text = "Giảm", Width = 70 },
            new ColumnHeader() { Text = "Ghi chú", Width = 220 },
            new ColumnHeader() { Text = "Trạng thái", Width = 90 }
        });
        listViewDiscounts.FullRowSelect = true;
        listViewDiscounts.GridLines = true;
        listViewDiscounts.Location = new Point(6, 6);
        listViewDiscounts.Name = "listViewDiscounts";
        listViewDiscounts.Size = new Size(756, 240);
        listViewDiscounts.TabIndex = 0;
        listViewDiscounts.UseCompatibleStateImageBehavior = false;
        listViewDiscounts.View = View.Details;

        // lblDiscountService
        lblDiscountService.AutoSize = true;
        lblDiscountService.Location = new Point(6, 260);
        lblDiscountService.Name = "lblDiscountService";
        lblDiscountService.Size = new Size(48, 15);
        lblDiscountService.TabIndex = 1;
        lblDiscountService.Text = "Dịch vụ:";

        // cmbDiscountService
        cmbDiscountService.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDiscountService.FormattingEnabled = true;
        cmbDiscountService.Location = new Point(80, 256);
        cmbDiscountService.Name = "cmbDiscountService";
        cmbDiscountService.Size = new Size(180, 23);
        cmbDiscountService.TabIndex = 2;

        // lblDiscountStart
        lblDiscountStart.AutoSize = true;
        lblDiscountStart.Location = new Point(280, 260);
        lblDiscountStart.Name = "lblDiscountStart";
        lblDiscountStart.Size = new Size(50, 15);
        lblDiscountStart.TabIndex = 3;
        lblDiscountStart.Text = "Bắt đầu:";

        // discountStartPicker
        discountStartPicker.CustomFormat = "HH:mm";
        discountStartPicker.Format = DateTimePickerFormat.Custom;
        discountStartPicker.Location = new Point(340, 256);
        discountStartPicker.Name = "discountStartPicker";
        discountStartPicker.ShowUpDown = true;
        discountStartPicker.Size = new Size(80, 23);
        discountStartPicker.TabIndex = 4;

        // lblDiscountEnd
        lblDiscountEnd.AutoSize = true;
        lblDiscountEnd.Location = new Point(440, 260);
        lblDiscountEnd.Name = "lblDiscountEnd";
        lblDiscountEnd.Size = new Size(55, 15);
        lblDiscountEnd.TabIndex = 5;
        lblDiscountEnd.Text = "Kết thúc:";

        // discountEndPicker
        discountEndPicker.CustomFormat = "HH:mm";
        discountEndPicker.Format = DateTimePickerFormat.Custom;
        discountEndPicker.Location = new Point(505, 256);
        discountEndPicker.Name = "discountEndPicker";
        discountEndPicker.ShowUpDown = true;
        discountEndPicker.Size = new Size(80, 23);
        discountEndPicker.TabIndex = 6;
        discountEndPicker.Value = new DateTime(2000, 1, 1, 23, 0, 0, 0);

        // lblDiscountPercent
        lblDiscountPercent.AutoSize = true;
        lblDiscountPercent.Location = new Point(6, 292);
        lblDiscountPercent.Name = "lblDiscountPercent";
        lblDiscountPercent.Size = new Size(50, 15);
        lblDiscountPercent.TabIndex = 7;
        lblDiscountPercent.Text = "Giảm %:";

        // numDiscountPercent
        numDiscountPercent.Location = new Point(80, 288);
        numDiscountPercent.Maximum = 99;
        numDiscountPercent.Minimum = 1;
        numDiscountPercent.Name = "numDiscountPercent";
        numDiscountPercent.Size = new Size(70, 23);
        numDiscountPercent.TabIndex = 8;
        numDiscountPercent.Value = 10;

        // lblDiscountNote
        lblDiscountNote.AutoSize = true;
        lblDiscountNote.Location = new Point(170, 292);
        lblDiscountNote.Name = "lblDiscountNote";
        lblDiscountNote.Size = new Size(48, 15);
        lblDiscountNote.TabIndex = 9;
        lblDiscountNote.Text = "Ghi chú:";

        // txtDiscountNote
        txtDiscountNote.Location = new Point(230, 288);
        txtDiscountNote.Name = "txtDiscountNote";
        txtDiscountNote.Size = new Size(220, 23);
        txtDiscountNote.TabIndex = 10;

        // btnCreateDiscount
        btnCreateDiscount.Location = new Point(460, 286);
        btnCreateDiscount.Name = "btnCreateDiscount";
        btnCreateDiscount.Size = new Size(90, 28);
        btnCreateDiscount.TabIndex = 11;
        btnCreateDiscount.Text = "Tạo giảm";
        btnCreateDiscount.UseVisualStyleBackColor = true;
        btnCreateDiscount.Click += btnCreateDiscount_Click;

        // chkDiscountActive
        chkDiscountActive.AutoSize = true;
        chkDiscountActive.Checked = true;
        chkDiscountActive.CheckState = CheckState.Checked;
        chkDiscountActive.Location = new Point(600, 258);
        chkDiscountActive.Name = "chkDiscountActive";
        chkDiscountActive.Size = new Size(59, 19);
        chkDiscountActive.TabIndex = 12;
        chkDiscountActive.Text = "Active";
        chkDiscountActive.UseVisualStyleBackColor = true;

        // btnDeleteDiscount
        btnDeleteDiscount.Location = new Point(560, 286);
        btnDeleteDiscount.Name = "btnDeleteDiscount";
        btnDeleteDiscount.Size = new Size(80, 28);
        btnDeleteDiscount.TabIndex = 13;
        btnDeleteDiscount.Text = "Xóa";
        btnDeleteDiscount.UseVisualStyleBackColor = true;
        btnDeleteDiscount.Click += btnDeleteDiscount_Click;

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
        Name = "AdminForm";
        Text = "Football Booking - Quản lý";
        StartPosition = FormStartPosition.CenterScreen;
        tabControl.ResumeLayout(false);
        tabFields.ResumeLayout(false);
        tabFields.PerformLayout();
        tabServices.ResumeLayout(false);
        tabServices.PerformLayout();
        tabDiscounts.ResumeLayout(false);
        tabDiscounts.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numDiscountPercent).EndInit();
        ResumeLayout(false);
    }
}
