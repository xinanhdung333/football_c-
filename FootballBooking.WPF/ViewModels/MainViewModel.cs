using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace FootballBooking.WPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private UserControl? _currentView;

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        // Start with Fields view
        NavigateToFields();
    }

    [RelayCommand]
    private void NavigateToFields()
    {
        CurrentView = _serviceProvider.GetRequiredService<Views.FieldsView>();
    }

    [RelayCommand]
    private void NavigateToServices()
    {
        CurrentView = _serviceProvider.GetRequiredService<Views.ServicesView>();
    }

    [RelayCommand]
    private void NavigateToCart()
    {
        CurrentView = _serviceProvider.GetRequiredService<Views.CartView>();
    }

    [RelayCommand]
    private void NavigateToBookings()
    {
        CurrentView = _serviceProvider.GetRequiredService<Views.BookingsView>();
    }

    [RelayCommand]
    private void NavigateToProfile()
    {
        CurrentView = _serviceProvider.GetRequiredService<Views.ProfileView>();
    }

    [RelayCommand]
    private void Logout()
    {
        // Open Login View and Close Main Window
        var loginView = _serviceProvider.GetRequiredService<Views.LoginView>();
        loginView.Show();
        
        System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Close();
    }
}
