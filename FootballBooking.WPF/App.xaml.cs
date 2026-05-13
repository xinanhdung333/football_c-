using System.Windows;
using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.WPF.ViewModels;
using FootballBooking.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FootballBooking.WPF;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Repositories
                services.AddScoped<UserRepository>();
                services.AddScoped<FieldRepository>();
                services.AddScoped<ServiceRepository>();
                services.AddScoped<BookingRepository>();
                services.AddScoped<CartRepository>();
                services.AddScoped<OrderRepository>();
                services.AddScoped<PaymentRepository>();
                services.AddScoped<ServiceDiscountRepository>();
                services.AddScoped<FeedbackRepository>();

                // Services
                services.AddScoped<AuthenticationService>();
                services.AddScoped<FieldManagementService>();
                services.AddScoped<ServiceInventoryService>();
                services.AddScoped<BookingService>();
                services.AddScoped<ServiceDiscountService>();
                services.AddScoped<CartService>();
                services.AddScoped<CheckoutService>();
                services.AddScoped<FeedbackService>();

                // ViewModels
                services.AddTransient<LoginViewModel>();
                services.AddTransient<RegisterViewModel>();
                services.AddTransient<MainViewModel>();
                services.AddTransient<FieldsViewModel>();
                services.AddTransient<ServicesViewModel>();
                services.AddTransient<CartViewModel>();
                services.AddTransient<BookingsViewModel>();
                services.AddTransient<AdminViewModel>();
                services.AddTransient<ProfileViewModel>();

                // Views
                services.AddTransient<LoginView>();
                services.AddTransient<RegisterView>();
                services.AddTransient<FieldsView>();
                services.AddTransient<ServicesView>();
                services.AddTransient<CartView>();
                services.AddTransient<BookingsView>();
                services.AddTransient<AdminView>();
                services.AddTransient<ProfileView>();
                services.AddTransient<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        var loginView = _host.Services.GetRequiredService<LoginView>();
        loginView.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }
        base.OnExit(e);
    }
}
