namespace FootballBooking.UI;

partial class AdminForm
{
    private System.ComponentModel.IContainer components = null;
    private TabControl tabControl;
    private TabPage tabFields;
    private TabPage tabServices;
    private ListView listViewFields;
    private ListView listViewServices;
    private Button btnUpdateFieldPrice;
    private Button btnUpdateStock;
    private Button btnLogout;
    private TextBox txtNewPrice;
    private TextBox txtNewStock;
    private Label lblNewPrice;
    private Label lblNewStock;

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
        listViewFields = new ListView();
        listViewServices = new ListView();
        btnUpdateFieldPrice = new Button();
        btnUpdateStock = new Button();
        btnLogout = new Button();
        txtNewPrice = new TextBox();
        txtNewStock = new TextBox();
        lblNewPrice = new Label();
        lblNewStock = new Label();

        tabControl.SuspendLayout();
        tabFields.SuspendLayout();
        tabServices.SuspendLayout();
        SuspendLayout();

        // tabControl
        tabControl.Controls.Add(tabFields);
        tabControl.Controls.Add(tabServices);
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
        ResumeLayout(false);
    }
}