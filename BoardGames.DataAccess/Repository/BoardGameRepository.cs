using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BoardGames.DataAccess.Repository
{
  public class BoardGameRepository : IBoardGameRepository
  {
    private readonly IRepository _repository;

    public BoardGameRepository(IRepository repository)
    {
      _repository = repository;
    }

    public async Task<List<BoardGame>> GetBoardGamesListAsync()
    {
      return await _repository
        .Query<BoardGame>()
        .ToListAsync();
    }

    public async Task<Dictionary<int, BoardGame>> GetBoardGamesDictAsync()
    {
      return await _repository
        .Query<BoardGame>()
        .ToDictionaryAsync(b => b.BoardGameId);
    }

    public async Task<(List<BoardGame>, int)> GetBoardGamesAsync(
      string filterQuery,
      int pageIndex,
      int pageSize,
      string sortColumn,
      string sortOrder)
    {
      var games = _repository
        .Query<BoardGame>()
        .OrderBy($"{sortColumn} {sortOrder}")
        .AsQueryable();

      if (!string.IsNullOrEmpty(filterQuery))
      {
        games = games.Where(b => b.Name.Contains(filterQuery));
      }

      var gameCount = games.Count();
      var gamesList = await games
        .Skip(pageIndex * pageSize)
        .Take(pageSize)
        .ToListAsync();

      return (gamesList, gameCount);
    }

    public async Task InsertBoardGamesAsync(List<BoardGame> boardGames)
    {
      await _repository.InsertAsync(boardGames);
    }
  }
}
