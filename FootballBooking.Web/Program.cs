using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.Models;
using FootballBooking.Web.Services;

namespace FootballBooking.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddControllers();

        // Add session support
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        // Register business logic services
        builder.Services.AddScoped<FieldManagementService>();
        builder.Services.AddScoped<ServiceInventoryService>();
        builder.Services.AddScoped<CartService>();
        builder.Services.AddScoped<BookingService>();
        builder.Services.AddScoped<AuthenticationService>();
        builder.Services.AddScoped<FeedbackService>();
        builder.Services.AddScoped<CheckoutService>();
        builder.Services.AddScoped<PdfService>();
        builder.Services.AddScoped<UserManagementService>();
        builder.Services.AddScoped<AnalyticsService>();
        builder.Services.AddScoped<ServiceDiscountService>();
        builder.Services.AddScoped<BookingPaymentService>();
        builder.Services.AddSingleton<JsonKnowledgeChatbotService>();

        // Register data access repositories
        builder.Services.AddScoped<FieldRepository>();
        builder.Services.AddScoped<ServiceRepository>();
        builder.Services.AddScoped<CartRepository>();
        builder.Services.AddScoped<BookingRepository>();
        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<FeedbackRepository>();
        builder.Services.AddScoped<OrderRepository>();
        builder.Services.AddScoped<PaymentRepository>();
        builder.Services.AddScoped<ServiceDiscountRepository>();
        builder.Services.AddScoped<BookingPaymentRepository>();

        var app = builder.Build();

        // Seed admin account if it does not exist.
        using (var scope = app.Services.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();
            var adminEmail = "admin@footballbooking.local";
            var adminPassword = "Admin@123";
            var existingAdmin = await userRepository.GetUserByEmailAsync(adminEmail);
            if (existingAdmin is null)
            {
                await userRepository.CreateUserAsync(new User
                {
                    Name = "Administrator",
                    Email = adminEmail,
                    Phone = "0123456789",
                    Password = adminPassword,
                    Role = UserRole.Admin
                });
            }
            else
            {
                if (existingAdmin.Role != UserRole.Admin && existingAdmin.Role != UserRole.Boss)
                {
                    await userRepository.UpdateUserRoleAsync(existingAdmin.Id, UserRole.Admin);
                }

                if (existingAdmin.Password != adminPassword)
                {
                    await userRepository.UpdateUserPasswordAsync(existingAdmin.Id, adminPassword);
                }
            }
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseSession();
        app.UseAuthorization();

        app.MapControllers();
        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        await app.RunAsync();
    }
}
