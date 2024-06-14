using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasOne(x => x.User)
                   .WithMany(x => x.RefreshTokens)
                   .HasForeignKey(x => x.UserId);
        }
    }
}
