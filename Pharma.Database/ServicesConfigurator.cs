using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Pharma.Database
{
    [ExcludeFromCodeCoverage]
    public static class ServicesConfigurator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connString)
        {
            services.AddDbContext<PharmaContext>(options =>
            {
                options.UseNpgsql(connString);
            });

            return services;
        }
    }
}
