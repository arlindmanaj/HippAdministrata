using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using HippAdministrata.Models.Domains;

namespace HippAdministrata.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity.HasKey(o => o.Id);

            entity.HasOne(o => o.Client)
                  .WithMany(c => c.Orders)
                  .HasForeignKey(o => o.ClientId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(o => o.SalesPerson)
                  .WithMany(s => s.Orders)
                  .HasForeignKey(o => o.SalesPersonId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(o => o.Employee)
                  .WithMany()
                  .HasForeignKey(o => o.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(o => o.Driver)
                  .WithMany()
                  .HasForeignKey(o => o.DriverId)
                  .OnDelete(DeleteBehavior.Restrict);

           

            entity.HasOne(o => o.Product)
               .WithMany() // Assuming a Product can be part of multiple Orders
               .HasForeignKey(o => o.ProductId)
               .OnDelete(DeleteBehavior.Restrict); // Prevent cascade deletes on Product
        }
    }

}
