using HippAdministrata.Models.JunctionTables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Configurations
{
    public class EmployeeProductLabelConfiguration : IEntityTypeConfiguration<EmployeeProductLabel>
    {
        public void Configure(EntityTypeBuilder<EmployeeProductLabel> entity)
        {
            entity.HasKey(epl => new { epl.EmployeeId, epl.ProductId });

            entity.HasOne(epl => epl.Employee)
                .WithMany(e => e.EmployeeProductLabels)
                .HasForeignKey(epl => epl.EmployeeId);

            entity.HasOne(epl => epl.Product)
                .WithMany(p => p.EmployeeProductLabels)
                .HasForeignKey(epl => epl.ProductId);
        }
    }
}
