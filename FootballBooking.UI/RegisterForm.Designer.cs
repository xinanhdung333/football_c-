namespace FootballBooking.UI;

partial class RegisterForm
{
    private System.ComponentModel.IContainer components = null;
    private Label lblName;
    private TextBox txtName;
    private Label lblEmail;
    private TextBox txtEmail;
    private Label lblPhone;
    private TextBox txtPhone;
    private Label lblPassword;
    private TextBox txtPassword;
    private Label lblConfirmPassword;
    private TextBox txtConfirmPassword;
    private Button btnRegister;

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
        lblName = new Label();
        txtName = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        lblConfirmPassword = new Label();
        txtConfirmPassword = new TextBox();
        btnRegister = new Button();

        SuspendLayout();

        lblName.AutoSize = true;
        lblName.Location = new Point(30, 30);
        lblName.Name = "lblName";
        lblName.Size = new Size(45, 15);
        lblName.TabIndex = 0;
        lblName.Text = "Họ tên:";

        txtName.Location = new Point(100, 25);
        txtName.Name = "txtName";
        txtName.Size = new Size(200, 23);
        txtName.TabIndex = 1;

        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(30, 70);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(35, 15);
        lblEmail.TabIndex = 2;
        lblEmail.Text = "Email:";

        txtEmail.Location = new Point(100, 65);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(200, 23);
        txtEmail.TabIndex = 3;

        lblPhone.AutoSize = true;
        lblPhone.Location = new Point(30, 110);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(40, 15);
        lblPhone.TabIndex = 4;
        lblPhone.Text = "Điện thoại:";

        txtPhone.Location = new Point(100, 105);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(200, 23);
        txtPhone.TabIndex = 5;

        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(30, 150);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(56, 15);
        lblPassword.TabIndex = 6;
        lblPassword.Text = "Mật khẩu:";

        txtPassword.Location = new Point(100, 145);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '*';
        txtPassword.Size = new Size(200, 23);
        txtPassword.TabIndex = 7;

        lblConfirmPassword.AutoSize = true;
        lblConfirmPassword.Location = new Point(30, 190);
        lblConfirmPassword.Name = "lblConfirmPassword";
        lblConfirmPassword.Size = new Size(90, 15);
        lblConfirmPassword.TabIndex = 8;
        lblConfirmPassword.Text = "Xác nhận mật khẩu:";

        txtConfirmPassword.Location = new Point(130, 185);
        txtConfirmPassword.Name = "txtConfirmPassword";
        txtConfirmPassword.PasswordChar = '*';
        txtConfirmPassword.Size = new Size(170, 23);
        txtConfirmPassword.TabIndex = 9;

        btnRegister.Location = new Point(150, 230);
        btnRegister.Name = "btnRegister";
        btnRegister.Size = new Size(100, 30);
        btnRegister.TabIndex = 10;
        btnRegister.Text = "Đăng ký";
        btnRegister.UseVisualStyleBackColor = true;
        btnRegister.Click += btnRegister_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(350, 280);
        Controls.Add(btnRegister);
        Controls.Add(txtConfirmPassword);
        Controls.Add(lblConfirmPassword);
        Controls.Add(txtPassword);
        Controls.Add(lblPassword);
        Controls.Add(txtPhone);
        Controls.Add(lblPhone);
        Controls.Add(txtEmail);
        Controls.Add(lblEmail);
        Controls.Add(txtName);
        Controls.Add(lblName);
        Name = "RegisterForm";
        Text = "Đăng ký tài khoản";
        StartPosition = FormStartPosition.CenterParent;
        ResumeLayout(false);
        PerformLayout();
    }
}