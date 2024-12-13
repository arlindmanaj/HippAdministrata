using HippAdministrata.Models.Domains;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> entity)
        {
            entity.HasKey(r => r.RoleId);
            entity.Property(r => r.RoleName).IsRequired();
        }
    }
}
