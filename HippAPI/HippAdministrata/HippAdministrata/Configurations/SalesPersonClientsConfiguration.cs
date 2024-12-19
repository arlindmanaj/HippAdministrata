using HippAdministrata.Models.JunctionTables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Configurations
{
    public class SalesPersonClientsConfiguration : IEntityTypeConfiguration<SalesPersonClients>
    {
        public void Configure(EntityTypeBuilder<SalesPersonClients> entity)
        {
            entity.HasKey(spc => spc.Id); // Sole primary key

            // Relationship with SalesPerson
            entity.HasOne(spc => spc.SalesPerson)
                  .WithMany(sp => sp.SalesPersonsClients)
                  .HasForeignKey(spc => spc.SalesPersonId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relationship with Client
            entity.HasOne(spc => spc.Client)
                  .WithMany(c => c.SalesPersonsClients)
                  .HasForeignKey(spc => spc.ClientId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(spc => spc.Products)
             .WithOne(spcp => spcp.SalesPersonClient)
             .HasForeignKey(spcp => spcp.SalesPersonClientId);
        }
    }
}
