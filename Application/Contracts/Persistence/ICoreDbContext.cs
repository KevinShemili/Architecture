using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Contracts.Persistence {
	public interface ICoreDbContext {
		Task<EntityEntry<TEntity>> CreateAsync<TEntity>(TEntity entity, bool commitChanges = false, CancellationToken cancellationToken = default) where TEntity : EntityBase;

		Task<bool> DeleteAsync<TEntity>(TEntity entity, bool commitChanges = false, CancellationToken cancellationToken = default) where TEntity : EntityBase;

		DbSet<T> EntityAsDbSet<T>() where T : class;

		Task HardDeleteAsync<TEntity>(TEntity entity, bool commitChanges = false, CancellationToken cancellationToken = default) where TEntity : EntityBase;

		IQueryable<TEntity> TableNoTracking<TEntity>() where TEntity : EntityBase;

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

		Task<TEntity> Update<TEntity>(TEntity entity, bool commitChanges = false, CancellationToken cancellationToken = default) where TEntity : EntityBase;
	}
}
