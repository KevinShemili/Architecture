using Application.Contracts.Persistence;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence {
	public class CoreDbContext : ICoreDbContext {

		private readonly DatabaseContext _databaseContext;

		public CoreDbContext(DatabaseContext databaseContext) {
			_databaseContext = databaseContext;
		}

		public async Task<EntityEntry<TEntity>> CreateAsync<TEntity>(TEntity entity, bool commitChanges = true, 
			CancellationToken cancellationToken = default) where TEntity : EntityBase {

			ArgumentNullException.ThrowIfNull(entity);

			if (entity is AuditableEntityBase auditableEntity) {
				auditableEntity.DateCreated = DateTime.UtcNow;
			}

			var createdEntity = _databaseContext.Set<TEntity>().Add(entity);

			if (commitChanges) {
				_ = await _databaseContext.SaveChangesAsync(cancellationToken);
			}

			return createdEntity;
		}

		public async Task<bool> DeleteAsync<TEntity>(TEntity entity, bool commitChanges = true, 
			CancellationToken cancellationToken = default) where TEntity : EntityBase {

			ArgumentNullException.ThrowIfNull(entity);

			entity.IsDeleted = true;

			return (await Update(entity, commitChanges, cancellationToken)).IsDeleted;
		}

		public DbSet<T> EntityAsDbSet<T>() where T : class {
			return _databaseContext.Set<T>();
		}

		public async Task HardDeleteAsync<TEntity>(TEntity entity, bool commitChanges = true, 
			CancellationToken cancellationToken = default) where TEntity : EntityBase {

			ArgumentNullException.ThrowIfNull(entity);

			_ = _databaseContext.Set<TEntity>().Remove(entity);

			if (commitChanges)
				_ = await _databaseContext.SaveChangesAsync(cancellationToken);
		}

		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {

			foreach (var entry in _databaseContext.ChangeTracker.Entries<AuditableEntityBase>()) {
				switch (entry.State) {
					case EntityState.Added:
					entry.Entity.DateCreated = DateTime.UtcNow;
					entry.Entity.IsDeleted = false;
					break;

					case EntityState.Modified:
					entry.Entity.DateUpdated = DateTime.UtcNow;
					break;
				}
			}

			return await _databaseContext.SaveChangesAsync(cancellationToken);
		}

		public IQueryable<TEntity> TableNoTracking<TEntity>() where TEntity : EntityBase {
			return EntityAsDbSet<TEntity>().AsNoTracking();
		}

		public async Task<TEntity> Update<TEntity>(TEntity entity, bool commitChanges = true, 
			CancellationToken cancellationToken = default) where TEntity : EntityBase {

			ArgumentNullException.ThrowIfNull(entity);

			if (entity is AuditableEntityBase auditableEntity) {
				auditableEntity.DateCreated = DateTime.UtcNow;
			}

			_ = _databaseContext.Set<TEntity>().Update(entity);

			if (commitChanges)
				_ = await _databaseContext.SaveChangesAsync(cancellationToken);

			return entity;
		}
	}
}
