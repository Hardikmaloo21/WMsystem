// WMS.Infrastructure/DependencyInjection.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Persistence;
using WMS.Infrastructure.UnitOfWork;

namespace WMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<WmsDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(WmsDbContext).Assembly.FullName)
                      .EnableRetryOnFailure(3)
            ));

        services.AddScoped<IUnitOfWork, WMS.Infrastructure.UnitOfWork.UnitOfWork>();

        return services;
    }
}
