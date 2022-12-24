using Microsoft.Extensions.DependencyInjection;
using PgBackup.Services;
namespace PgBackup
{
    public static class DiExtensions
    {
        public static IServiceCollection AddPgBackupServices(
            this IServiceCollection services)
        {
            services.AddScoped<IPgDumpService, PgDumpService>();

            return services;
        }
    }

}