using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FootballBooking.BLL;
using System.Windows;

namespace FootballBooking.WPF.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly AuthenticationService _authService;

    [ObservableProperty]
    private string _fullName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _phone = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    public RegisterViewModel(AuthenticationService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Email) || 
            string.IsNullOrWhiteSpace(Phone) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Vui lòng điền đầy đủ thông tin.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Mật khẩu xác nhận không khớp.";
            return;
        }

        IsBusy = true;
        ErrorMessage = string.Empty;

        try
        {
            var userId = await _authService.RegisterUserAsync(FullName, Email, Phone, Password);
            MessageBox.Show($"Đăng ký thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            
            // Close the window
            Application.Current.Windows.OfType<Views.RegisterView>().FirstOrDefault()?.Close();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Lỗi: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void BackToLogin()
    {
        Application.Current.Windows.OfType<Views.RegisterView>().FirstOrDefault()?.Close();
    }
}
