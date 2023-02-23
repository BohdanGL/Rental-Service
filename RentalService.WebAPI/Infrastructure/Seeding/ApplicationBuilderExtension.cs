using Microsoft.EntityFrameworkCore;
using RentalService.Domain.Entities;
using RentalService.Persistence;

namespace RentalService.WebAPI.Infrastructure.Seeding
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> SeedDatabaseAsync(this IApplicationBuilder appBuilder)
        {
            using (var scope = appBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<RentalServiceContext>();

                using (var transaction = context.Database.BeginTransaction())
                {
                    await SeedFacilities(context);
                    await SeedEquipments(context);

                    await transaction.CommitAsync();
                }
            }

            return appBuilder;
        }

        private static async Task SeedFacilities(RentalServiceContext context)
        {
            Facility[] facilities =
            {
                new Facility { Name = "Facility 1", AvailableSquare = 100 },
                new Facility { Name = "Facility 2", AvailableSquare = 200 },
                new Facility { Name = "Facility 3", AvailableSquare = 300 },
                new Facility { Name = "Facility 4", AvailableSquare = 400 },
            };

            await context.AddRangeAndSaveAsync(facilities);
        }

        private static async Task SeedEquipments(RentalServiceContext context)
        {
            Equipment[] equipments =
            {
                new Equipment { Name = "Equipment 1", Square = 10 },
                new Equipment { Name = "Equipment 2", Square = 15 },
                new Equipment { Name = "Equipment 3", Square = 20 },
                new Equipment { Name = "Equipment 4", Square = 35 },
            };

            await context.AddRangeAndSaveAsync(equipments);
        }

        private static async Task AddRangeAndSaveAsync<T>(this RentalServiceContext context, IEnumerable<T> entities)
        where T : class
        {
            if (!await context.Set<T>().AnyAsync())
            {
                context.AddRange((IEnumerable<object>)entities);
                await context.SaveChangesAsync();
            }
        }
    }
}
