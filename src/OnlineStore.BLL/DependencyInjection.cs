using Microsoft.Extensions.DependencyInjection;
using OnlineStore.DAL;

namespace OnlineStore.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services, string connectionString)
    {
        services.AddDataAccess(connectionString);
        return services;
    }
}
