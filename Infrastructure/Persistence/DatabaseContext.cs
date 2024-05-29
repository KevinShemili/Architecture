using Domain.Entities;
using Domain.Entities.IdentityExtensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence {
	public class DatabaseContext : IdentityDbContext<User> {

		public DatabaseContext(DbContextOptions<DatabaseContext> options) 
            : base(options) {

		}

		public DbSet<TestEntity> TestEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("identity");

            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }

    }
}
