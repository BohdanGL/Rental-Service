using Microsoft.EntityFrameworkCore;
using RentalService.Persistence;

namespace RentalService.WebAPI.Infrastructure.Db
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> SetupDatabaseAsync(this IApplicationBuilder appBuilder)
        {
            using (var scope = appBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<RentalServiceContext>();
                await context.Database.MigrateAsync();
            }

            return appBuilder;
        }
    }
}
