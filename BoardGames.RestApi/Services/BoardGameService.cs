using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.Models;

namespace BoardGames.RestApi.Services
{
  public class BoardGameService : IBoardGameService
  {
    private IBoardGameRepository _boardGameRepo;

    public BoardGameService(IBoardGameRepository repository)
    {
      _boardGameRepo = repository;
    }

    public async Task<List<BoardGame>> GetBoardGameAsync()
    {
      return await _boardGameRepo.GetBoardGamesListAsync();
    }

    public async Task AddBoardGamesAsync(List<BoardGame> boardGames)
    {
      await _boardGameRepo.InsertBoardGamesAsync(boardGames);
    }
  }
}
