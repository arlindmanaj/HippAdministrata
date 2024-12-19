using HippAdministrata.Models.JunctionTables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using HippAdministrata.Models.Domains;

namespace HippAdministrata.Configurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> entity)
        {
            entity.HasKey(w => w.Id);

            entity.Property(w => w.Location)
                  .IsRequired()
                  .HasConversion<string>();

           
        }
    }
}
