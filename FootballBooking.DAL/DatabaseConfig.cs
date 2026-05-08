using Microsoft.Data.SqlClient;

namespace FootballBooking.DAL;

public static class DatabaseConfig
{
    public static string GetConnectionString()
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = @".\SQLEXPRESS",
            InitialCatalog = "football_booking",
            IntegratedSecurity = true,
            TrustServerCertificate = true,
            ConnectTimeout = 30
        };

        return builder.ConnectionString;
    }
}
