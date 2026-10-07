using Microsoft.Extensions.DependencyInjection;
using OnlineStore.BLL.Services;
using OnlineStore.DAL;

namespace OnlineStore.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services, string connectionString)
    {
        services.AddDataAccess(connectionString);
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();
        return services;
    }

    public static void InitializeStorage(this IServiceProvider services) => services.EnsureDatabaseCreated();
}
