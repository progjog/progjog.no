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

        var serverVersion = new MariaDbServerVersion(new Version(10, 5, 23)); 

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
