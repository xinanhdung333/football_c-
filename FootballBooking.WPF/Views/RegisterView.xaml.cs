using System.Windows;
using FootballBooking.WPF.ViewModels;

namespace FootballBooking.WPF.Views;

public partial class RegisterView : Window
{
    public RegisterView(RegisterViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void PassBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is RegisterViewModel viewModel)
        {
            viewModel.Password = ((System.Windows.Controls.PasswordBox)sender).Password;
        }
    }

    private void ConfirmPassBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is RegisterViewModel viewModel)
        {
            viewModel.ConfirmPassword = ((System.Windows.Controls.PasswordBox)sender).Password;
        }
    }
}
