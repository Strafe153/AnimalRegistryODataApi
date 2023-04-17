using DataAccess;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace AnimalRegistryODataApi.Configurations;

public static class MigrationsConfiguration
{
    public static void ApplyDatabaseMigration(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AnimalRegistryContext>();

        while (!dbContext.CanConnect())
        {
            Thread.Sleep(5000);
        }

        dbContext.Database.Migrate();
    }

    private static bool CanConnect(this AnimalRegistryContext dbContext)
    {
        var connecion = dbContext.Database.GetDbConnection();
        var masterConnection = new MySqlConnection(connecion.ConnectionString);

        try
        {
            masterConnection.Open();
            masterConnection.Close();
        }
        catch (MySqlException)
        {
            return false;
        }
        finally
        {
            masterConnection?.Dispose();
        }

        return true;
    }
}
