using ReportsProject.Domain.Interfaces.Repositories;

namespace ReportsProject.DAL.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
	{
		private readonly AppDbContext _dbContext;

		public BaseRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<TEntity> CreateAsync(TEntity entity)
		{
			ArgumentNullException.ThrowIfNull(entity);

			await _dbContext.AddAsync(entity);
			await _dbContext.SaveChangesAsync();

			return entity;
		}

		public IQueryable<TEntity> GetAll()
		{
			return _dbContext.Set<TEntity>();
		}

		public async Task<TEntity> RemoveAsync(TEntity entity)
		{
			ArgumentNullException.ThrowIfNull(entity);

			_dbContext.Remove(entity);
			await _dbContext.SaveChangesAsync();
		
			return entity;
		}

		public async Task<TEntity> UpdateAsync(TEntity entity)
		{
			ArgumentNullException.ThrowIfNull(entity);

			_dbContext.Update(entity);
			await _dbContext.SaveChangesAsync();

			return entity;
		}
	}
}
