using FootballBooking.BLL;
using FootballBooking.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FootballBooking.UI;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        using var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddScoped<UserRepository>();
                services.AddScoped<FieldRepository>();
                services.AddScoped<ServiceRepository>();
                services.AddScoped<BookingRepository>();
                services.AddScoped<CartRepository>();
                services.AddScoped<OrderRepository>();
                services.AddScoped<PaymentRepository>();
                services.AddScoped<ServiceDiscountRepository>();

                services.AddScoped<AuthenticationService>();
                services.AddScoped<FieldManagementService>();
                services.AddScoped<ServiceInventoryService>();
                services.AddScoped<BookingService>();
                services.AddScoped<ServiceDiscountService>();
                services.AddScoped<CartService>();
                services.AddScoped<CheckoutService>();

                services.AddScoped<LoginForm>();
                services.AddScoped<RegisterForm>();
                services.AddScoped<AdminForm>();
                services.AddScoped<MainForm>();
                services.AddScoped<CheckoutForm>();
            })
            .Build();

        var loginForm = host.Services.GetRequiredService<LoginForm>();
        Application.Run(loginForm);
    }
}
