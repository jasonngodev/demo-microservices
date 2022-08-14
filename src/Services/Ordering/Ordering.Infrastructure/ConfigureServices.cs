using Contracts.Common.Interfaces;
using Contracts.Services;
using Infrastructure.Common;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Interfaces;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<OrderContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"),
                builder => builder.MigrationsAssembly(typeof(OrderContext).Assembly.FullName));
        });

        serviceCollection.AddScoped<OrderContextSeed>();
        serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
        serviceCollection.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

        serviceCollection.AddScoped(typeof(ISmtpEmailService), typeof(SmtpEmailService));
        
        return serviceCollection;
    }
}