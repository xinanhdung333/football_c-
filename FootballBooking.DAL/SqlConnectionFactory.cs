using Microsoft.Data.SqlClient;

namespace FootballBooking.DAL;

public static class SqlConnectionFactory
{
    public static SqlConnection CreateConnection()
    {
        return new SqlConnection(DatabaseConfig.GetConnectionString());
    }
}
