using BoardGames.DataContract.Models;

namespace BoardGames.RestApi.Services
{
  public interface IBoardGameService
  {
    Task<List<BoardGame>> GetBoardGameAsync();
    Task AddBoardGamesAsync(List<BoardGame> boardGames);
  }
}
