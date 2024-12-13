using HippAdministrata.Models.Domains;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using HippAdministrata.Models.JunctionTables;

namespace HippAdministrata.Configurations
{
    public class OrderHistoryConfiguration : IEntityTypeConfiguration<OrderHistory>
    {
        public void Configure(EntityTypeBuilder<OrderHistory> entity)
        {
            entity.HasKey(oh => oh.Id);

            entity.HasOne(oh => oh.Order)
                  .WithMany()
                  .HasForeignKey(oh => oh.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oh => oh.UpdatedByEmployee)
                  .WithMany()
                  .HasForeignKey(oh => oh.UpdatedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
