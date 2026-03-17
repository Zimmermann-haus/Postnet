using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ApplicationForFiscalPrinter.Data;

public static class DbConnectionFactory
{
    public static IDbConnection Create()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        return new SqlConnection(
            config.GetConnectionString("SqlServer"));
    }
}
