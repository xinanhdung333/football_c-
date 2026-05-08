using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FootballBooking.UI;

public partial class LoginForm : Form
{
    private AuthenticationService _authService = null!;
    private IServiceProvider _serviceProvider = null!;

    public LoginForm()
    {
        InitializeComponent();
    }

    public LoginForm(AuthenticationService authService, IServiceProvider serviceProvider) : this()
    {
        _authService = authService;
        _serviceProvider = serviceProvider;
    }

    private async void btnLogin_Click(object sender, EventArgs e)
    {
        var email = txtEmail.Text.Trim();
        var password = txtPassword.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Vui lòng nhập email và mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var user = await _authService.AuthenticateAsync(email, password);
            if (user is null)
            {
                MessageBox.Show("Email hoặc mật khẩu không đúng.", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Store current user (in a real app, use dependency injection or session management)
            CurrentUser.User = user;

            if (user.Role == UserRole.Admin || user.Role == UserRole.Boss)
            {
                var adminForm = new AdminForm();
                adminForm.Show();
            }
            else
            {
                var mainForm = _serviceProvider.GetRequiredService<MainForm>();
                mainForm.Show();
            }

            this.Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
        var registerForm = _serviceProvider.GetRequiredService<RegisterForm>();
        registerForm.ShowDialog();
    }
}