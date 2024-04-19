using BoardGames.DataContract.Models;

namespace BoardGames.RestApi.Services
{
  public interface IBoardGameService
  {
    Task<List<BoardGame>> GetBoardGameAsync();
    Task<(List<BoardGame>, int)> GetBoardGamesAsync(
      string filterQuery, int pageIndex,
      int pageSize, string sortColumn,
      string sortOrder);
    Task AddBoardGamesAsync(List<BoardGame> boardGames);
  }
}
