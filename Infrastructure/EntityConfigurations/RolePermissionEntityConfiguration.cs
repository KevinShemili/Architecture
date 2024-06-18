using Domain.Entities;
using Infrastructure.EntityConfigurations.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class RolePermissionEntityConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Role)
                   .WithMany(x => x.RolePermissions)
                   .HasForeignKey(x => x.RoleId);

            builder.HasOne(x => x.Permission)
                   .WithMany(x => x.RolePermissions)
                   .HasForeignKey(x => x.PermissionId);

            SeedData(builder);
        }

        private static void SeedData(EntityTypeBuilder<RolePermission> builder)
        {
            int id = 1;

            foreach (var permission in Permissions.SeedAdminPermissions)
            {
                builder.HasData(new RolePermission
                {
                    Id = id,
                    RoleId = Roles.Administrator.Id,
                    PermissionId = permission.Id,
                    AssignedByName = "system",
                    DateCreated = DateTime.UtcNow
                });

                id++;
            }
        }
    }
}
