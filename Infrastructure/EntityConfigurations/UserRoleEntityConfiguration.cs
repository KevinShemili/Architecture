using Domain.Entities;
using Infrastructure.EntityConfigurations.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.UserRoles)
                   .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Role)
                   .WithMany(x => x.UserRoles)
                   .HasForeignKey(x => x.RoleId);
            
            SeedData(builder);
        }

        private static void SeedData(EntityTypeBuilder<UserRole> builder)
        {
            int id = 1;

            foreach (var role in Roles.SeedRoles)
            {
                builder.HasData(new UserRole
                {
                    Id = id,
                    UserId = Users.Administrator.Id,
                    RoleId = role.Id,
                    AssignedByName = "system"
                });
                id++;
            }
        }
    }
}
