using HippAdministrata.Models.JunctionTables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Configurations
{
    public class SalesPersonClientProductConfiguration : IEntityTypeConfiguration<SalesPersonClientProduct>
    {
        public void Configure(EntityTypeBuilder<SalesPersonClientProduct> entity)
        {
            entity.HasKey(spcp => spcp.Id);

            entity.HasOne(spcp => spcp.SalesPersonClient)
                  .WithMany(spc => spc.Products)
                  .HasForeignKey(spcp => spcp.SalesPersonClientId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(spcp => spcp.Product)
                  .WithMany()
                  .HasForeignKey(spcp => spcp.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
