using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace AnimalRegistryODataApi.Configurations;

public static class DatabaseConfiguration
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var mySqlVersion = new MySqlServerVersion(new Version(8, 0, 11));

        services.AddDbContext<AnimalRegistryContext>(options =>
            options.UseMySql(connectionString, mySqlVersion));
    }
}
