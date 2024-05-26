using BoardGames.DataContract.Models;
using BoardGames.RestApi.DTOs;

namespace BoardGames.RestApi.Services.Interfaces
{
  public interface IBoardGameService
  {
    Task<BoardGame> GetBoardGameAsync(int boardGameId);
    Task<(List<BoardGame>, int)> GetBoardGamesAsync(
      string filterQuery, int pageIndex,
      int pageSize, string sortColumn,
      string sortOrder);
    Task<BoardGame> AddBoardGameAsync(BoardGameDTO boardGameDto);
    Task AddBoardGamesAsync(List<BoardGame> boardGames);
    Task<BoardGame> UpdateBoardGameAsync(int boardGameId, UpdateBoardGameDTO model);
    Task<BoardGame> DeleteBoardGameAsync(int boardGameId);
  }
}
