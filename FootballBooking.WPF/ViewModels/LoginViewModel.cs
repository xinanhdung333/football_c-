using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FootballBooking.WPF.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthenticationService _authService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    public LoginViewModel(AuthenticationService authService, IServiceProvider serviceProvider)
    {
        _authService = authService;
        _serviceProvider = serviceProvider;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Vui lòng nhập đầy đủ email và mật khẩu.";
            return;
        }

        IsBusy = true;
        ErrorMessage = string.Empty;

        try
        {
            var user = await _authService.AuthenticateAsync(Email, Password);
            if (user == null)
            {
                ErrorMessage = "Email hoặc mật khẩu không chính xác.";
                return;
            }

            // TODO: Store user in a session service
            // For now, we can use a static property similar to WinForms if needed
            // FootballBooking.UI.CurrentUser.User = user; 

            if (user.Role == UserRole.Admin || user.Role == UserRole.Boss)
            {
                // Open Admin Window
                var adminView = _serviceProvider.GetRequiredService<Views.AdminView>();
                adminView.Show();

                // Close Login Window
                Application.Current.Windows.OfType<Views.LoginView>().FirstOrDefault()?.Close();
            }
            else
            {
                // Open Main Window
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
                
                // Close Login Window (handled in View code-behind or via a service)
                Application.Current.Windows.OfType<Views.LoginView>().FirstOrDefault()?.Close();
            }
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
    private void Register()
    {
        var registerView = _serviceProvider.GetRequiredService<Views.RegisterView>();
        registerView.ShowDialog();
    }
}
