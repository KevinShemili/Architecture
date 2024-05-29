using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class TestEntityConfiguration : IEntityTypeConfiguration<TestEntity>
    {
        public void Configure(EntityTypeBuilder<TestEntity> builder)
        {
            builder.ToTable(nameof(TestEntity))
                   .HasKey(x => x.Id);

            builder
                .Property(x => x.TestDecimal)
                .HasPrecision(18, 4);
        }
    }
}
