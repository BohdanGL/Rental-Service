using Microsoft.EntityFrameworkCore;
using RentalService.Domain.Entities;

namespace RentalService.Persistence
{
    public partial class RentalServiceContext : DbContext
    {
        public RentalServiceContext(DbContextOptions<RentalServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Facility> Facilities { get; set; } = null!;
        public virtual DbSet<Equipment> Equipments { get; set; } = null!;
        public virtual DbSet<Contract> Contracts { get; set; } = null!;
    }
}
