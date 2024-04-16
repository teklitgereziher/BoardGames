using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task InsertBoardGamesAsync(List<BoardGame> boardGames)
    {
      await _repository.InsertAsync(boardGames);
    }
  }
}
