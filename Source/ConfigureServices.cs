namespace progjog;

using progjog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public static class ConfigureServices
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {

        // Add services to the container.
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();
    }

    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }
}
