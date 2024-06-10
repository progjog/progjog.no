namespace progjog;

using progjog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using progjog.Services;

public static class ConfigureServices
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {

        // Add services to the container.
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        ServerVersion serverVersion;

#warning Don't use try catch for logic. Juse set the version and the databaseclient in appsettings
        try
        {
            serverVersion = new MySqlServerVersion(ServerVersion.AutoDetect(connectionString));
        }
        catch (ArgumentException)
        {
            serverVersion = new MariaDbServerVersion(ServerVersion.AutoDetect(connectionString));
        }

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, serverVersion));

        services.AddDatabaseDeveloperPageExceptionFilter();
    }

    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }

    public static void AddTrainingSessionService(this IServiceCollection services)
    {
        services.AddScoped<ITrainingSessionService, TrainingSessionService>();
    }
}
