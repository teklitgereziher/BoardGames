using BoardGames.DataContract.DatabContext;

namespace BoardGames.DataAccess.Interfaces
{
  public interface IRepository
  {
    BoardGamesDbContext Context { get; }
    IQueryable<TEntity> Query<TEntity>() where TEntity : class;
    Task InsertAsync<TEntity>(TEntity entity) where TEntity : class;
    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;
    Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
  }
}
