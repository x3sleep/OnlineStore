using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.DAL.Data;

namespace OnlineStore.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<StoreDbContext>(options => options.UseSqlite(connectionString));
        return services;
    }
}
