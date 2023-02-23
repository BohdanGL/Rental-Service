using Microsoft.EntityFrameworkCore;
using RentalService.Domain.Entities;
using RentalService.Persistence;

namespace RentalService.Application.Tests
{
    public class TestBase
    {
        protected RentalServiceContext Context { get; private set; }

        public TestBase()
        {
            Context = CreateDbContext();
        }

        private RentalServiceContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<RentalServiceContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new RentalServiceContext(options);
            context.Database.EnsureCreated();

            context.AddRange(Facilities);
            context.AddRange(Equipments);
            context.AddRange(Contracts);

            context.SaveChanges();

            return context;
        }

        private static readonly List<Facility> Facilities = new()
        {
            new Facility { Name = "Facility 1", AvailableSquare = 100 },
            new Facility { Name = "Facility 3", AvailableSquare = 300 },
            new Facility { Name = "Facility 3", AvailableSquare = 300 },
            new Facility { Name = "Facility 4", AvailableSquare = 400 },
        };

        private static readonly List<Equipment> Equipments = new()
        {
            new Equipment { Name = "Equipment 1", Square = 10 },
            new Equipment { Name = "Equipment 3", Square = 15 },
            new Equipment { Name = "Equipment 3", Square = 20 },
            new Equipment { Name = "Equipment 4", Square = 35 },
        };

        private static readonly List<Contract> Contracts = new()
        {
            new Contract { Facility = Facilities.ElementAt(0), Equipment = Equipments.ElementAt(3), EquipmentCount = 4 },
            new Contract { Facility = Facilities.ElementAt(1), Equipment = Equipments.ElementAt(2), EquipmentCount = 3 },
            new Contract { Facility = Facilities.ElementAt(2), Equipment = Equipments.ElementAt(1), EquipmentCount = 2 },
            new Contract { Facility = Facilities.ElementAt(3), Equipment = Equipments.ElementAt(0), EquipmentCount = 1 },
        };
    }
}
