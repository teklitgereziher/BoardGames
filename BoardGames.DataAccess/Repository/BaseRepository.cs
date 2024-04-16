using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.DatabContext;

namespace BoardGames.DataAccess.Repository
{
  public class BaseRepository : IRepository
  {
    public BoardGamesDbContext Context { get; private set; }

    public BaseRepository(BoardGamesDbContext context)
    {
      Context = context;
    }

    public IQueryable<TEntity> Query<TEntity>() where TEntity : class
    {
      return Context.Set<TEntity>();
    }

    public async Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
    {
      Context.Set<TEntity>().Add(entity);
      await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
      Context.Set<TEntity>().Update(entity);
      await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
    {
      Context.Set<TEntity>().Remove(entity);
      await Context.SaveChangesAsync();
    }
  }
}
