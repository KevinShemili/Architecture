using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence {
	public class DatabaseContext : DbContext {
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {

		}

		public DbSet<TestEntity> TestEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
