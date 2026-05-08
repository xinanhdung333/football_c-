namespace FootballBooking.UI;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null;
    private Label lblEmail;
    private TextBox txtEmail;
    private Label lblPassword;
    private TextBox txtPassword;
    private Button btnLogin;
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
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        btnLogin = new Button();
        btnRegister = new Button();

        SuspendLayout();

        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(50, 50);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(35, 15);
        lblEmail.TabIndex = 0;
        lblEmail.Text = "Email:";

        txtEmail.Location = new Point(120, 45);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(200, 23);
        txtEmail.TabIndex = 1;

        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(50, 90);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(56, 15);
        lblPassword.TabIndex = 2;
        lblPassword.Text = "Mật khẩu:";

        txtPassword.Location = new Point(120, 85);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '*';
        txtPassword.Size = new Size(200, 23);
        txtPassword.TabIndex = 3;

        btnLogin.Location = new Point(120, 130);
        btnLogin.Name = "btnLogin";
        btnLogin.Size = new Size(75, 30);
        btnLogin.TabIndex = 4;
        btnLogin.Text = "Đăng nhập";
        btnLogin.UseVisualStyleBackColor = true;
        btnLogin.Click += btnLogin_Click;

        btnRegister.Location = new Point(220, 130);
        btnRegister.Name = "btnRegister";
        btnRegister.Size = new Size(75, 30);
        btnRegister.TabIndex = 5;
        btnRegister.Text = "Đăng ký";
        btnRegister.UseVisualStyleBackColor = true;
        btnRegister.Click += btnRegister_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(400, 200);
        Controls.Add(btnRegister);
        Controls.Add(btnLogin);
        Controls.Add(txtPassword);
        Controls.Add(lblPassword);
        Controls.Add(txtEmail);
        Controls.Add(lblEmail);
        Name = "LoginForm";
        Text = "Đăng nhập - Football Booking";
        StartPosition = FormStartPosition.CenterScreen;
        ResumeLayout(false);
        PerformLayout();
    }
}