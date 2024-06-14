using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class EmailTokenEntityConfiguration : IEntityTypeConfiguration<EmailToken>
    {
        public void Configure(EntityTypeBuilder<EmailToken> builder)
        {
            builder.HasOne(x => x.User)
                   .WithMany(x => x.EmailTokens)
                   .HasForeignKey(x => x.UserId);                   
        }
    }
}
