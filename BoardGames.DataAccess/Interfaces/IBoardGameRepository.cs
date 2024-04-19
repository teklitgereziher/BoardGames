using BoardGames.DataContract.Models;

namespace BoardGames.DataAccess.Interfaces
{
  public interface IBoardGameRepository
  {
    Task<List<BoardGame>> GetBoardGamesListAsync();
    Task<Dictionary<int, BoardGame>> GetBoardGamesDictAsync();
    Task<(List<BoardGame>, int)> GetBoardGamesAsync(
      string filterQuery, int pageIndex, int pageSize,
      string sortColumn, string sortOrder);
    Task InsertBoardGamesAsync(List<BoardGame> boardGames);
  }
}
