using BoardGames.DataContract.Models;

namespace BoardGames.DataAccess.Interfaces
{
  public interface IBoardGameRepository
  {
    Task<List<BoardGame>> GetBoardGamesListAsync();
    Task<Dictionary<int, BoardGame>> GetBoardGamesDictAsync();
    Task InsertBoardGamesAsync(List<BoardGame> boardGames);
  }
}
