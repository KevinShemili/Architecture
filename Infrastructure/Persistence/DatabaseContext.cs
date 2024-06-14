using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<EmailToken> EmailTokens { get; set; }
        public DbSet<PasswordToken> PasswordTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntityBase).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(GetSoftDeleteFilter(entityType.ClrType));
                    
                    modelBuilder.Entity(entityType.ClrType).HasIndex(nameof(AuditableEntityBase.IsDeleted))
                                                           .HasFilter("IsDeleted = 0");
                }
            }

            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }

        private static LambdaExpression GetSoftDeleteFilter(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var property = Expression.Property(parameter, nameof(AuditableEntityBase.IsDeleted));
            var condition = Expression.Equal(property, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parameter);
            return lambda;
        }
    }
}
