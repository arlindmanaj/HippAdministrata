using HippAdministrata.Models.JunctionTables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Configurations
{
    public class CarDriversConfiguration : IEntityTypeConfiguration<CarDrivers>
    {
        public void Configure(EntityTypeBuilder<CarDrivers> entity)
        {
            entity.HasKey(cd => new { cd.DriverId, cd.CarId });

            entity.HasOne(cd => cd.Driver)
                  .WithMany()
                  .HasForeignKey(cd => cd.DriverId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
