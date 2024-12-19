using HippAdministrata.Models.JunctionTables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using HippAdministrata.Models.Requests;
using System.Reflection.Emit;

namespace HippAdministrata.Configurations
{
    public class ClientOrderRequestConfiguration : IEntityTypeConfiguration<ClientOrderRequest>
    {
        public void Configure(EntityTypeBuilder<ClientOrderRequest> entity)
        {
            
                entity.HasKey(cor => cor.Id);

                entity.HasOne(cor => cor.Client)
                      .WithMany()
                      .HasForeignKey(cor => cor.ClientId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cor => cor.SalesPerson)
                      .WithMany()
                      .HasForeignKey(cor => cor.SalesPersonId)
                      .OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}
