using Domain.Entities;
using Infrastructure.EntityConfigurations.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(x => x.Roles)
                   .WithMany(x => x.Users)
                   .UsingEntity<UserRole>(x => x.HasKey(x => x.Id));

            builder.HasMany(x => x.RefreshTokens)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.EmailTokens)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.PasswordTokens)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId);

            SeedData(builder);
        }

        private static void SeedData(EntityTypeBuilder<User> builder)
        {
            builder.HasData(Users.SeedUsers);
        }
    }
}
