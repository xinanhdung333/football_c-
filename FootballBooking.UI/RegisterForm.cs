using FootballBooking.BLL;
using FootballBooking.DAL;

namespace FootballBooking.UI;

public partial class RegisterForm : Form
{
    private AuthenticationService _authService = null!;

    public RegisterForm()
    {
        InitializeComponent();
    }

    public RegisterForm(AuthenticationService authService) : this()
    {
        _authService = authService;
    }

    private async void btnRegister_Click(object sender, EventArgs e)
    {
        var name = txtName.Text.Trim();
        var email = txtEmail.Text.Trim();
        var phone = txtPhone.Text.Trim();
        var password = txtPassword.Text;
        var confirmPassword = txtConfirmPassword.Text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (password != confirmPassword)
        {
            MessageBox.Show("Mật khẩu xác nhận không khớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var userId = await _authService.RegisterUserAsync(name, email, phone, password);
            MessageBox.Show($"Đăng ký thành công! ID người dùng: {userId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi đăng ký: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}