using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class PasswordTokenEntityConfiguration : IEntityTypeConfiguration<PasswordToken>
    {
        public void Configure(EntityTypeBuilder<PasswordToken> builder)
        {
            builder.HasOne(x => x.User)
                   .WithMany(x => x.PasswordTokens)
                   .HasForeignKey(x => x.UserId);
        }
    }
}
