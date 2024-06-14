using Domain.Entities;
using Infrastructure.EntityConfigurations.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class PermissionEntityConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasMany(x => x.Roles)
                   .WithMany(x => x.Permissions)
                   .UsingEntity<RolePermission>(x => x.HasKey(x => x.Id));

            SeedData(builder);
        }

        private static void SeedData(EntityTypeBuilder<Permission> builder)
        {
            builder.HasData(Permissions.SeedPermissions);
        }
    }
}
