using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.Models;
using BoardGames.RestApi.DTOs;
using BoardGames.RestApi.Services.Interfaces;

namespace BoardGames.RestApi.Services
{
  public class BoardGameService : IBoardGameService
  {
    private readonly IBoardGameRepository _boardGameRepo;
    private readonly ILogger<BoardGameService> _logger;

    public BoardGameService(
      IBoardGameRepository repository,
      ILogger<BoardGameService> logger)
    {
      _boardGameRepo = repository;
      _logger = logger;
    }

    public async Task<BoardGame> GetBoardGameAsync(int boardGameId)
    {
      return await _boardGameRepo.GetBoardGameAsync(boardGameId);
    }

    public async Task<(List<BoardGame>, int)> GetBoardGamesAsync(
      string filterQuery,
      int pageIndex,
      int pageSize,
      string sortColumn,
      string sortOrder)
    {
      return await _boardGameRepo.GetBoardGamesAsync(
        filterQuery,
        pageIndex,
        pageSize,
        sortColumn,
        sortOrder);
    }

    public async Task<BoardGame> AddBoardGameAsync(BoardGameDTO boardGameDto)
    {
      var now = DateTime.UtcNow;
      var boardGame = new BoardGame
      {
        BoardGameId = 331788,
        Name = boardGameDto.Name,
        Year = boardGameDto.Year,
        MinPlayers = boardGameDto.MinPlayers,
        MaxPlayers = boardGameDto.MaxPlayers,
        PlayTime = boardGameDto.PlayTime,
        MinAge = boardGameDto.MinAge,
        UsersRated = boardGameDto.UsersRated,
        RatingAverage = boardGameDto.RatingAverage,
        BGGRank = boardGameDto.BGGRank,
        ComplexityAverage = boardGameDto.ComplexityAverage,
        OwnedUsers = boardGameDto.OwnedUsers,
        CreatedDate = now,
        LastModifiedDate = now,
      };

      return await _boardGameRepo.InsertBoardGameAsync(boardGame);
    }

    public async Task AddBoardGamesAsync(List<BoardGame> boardGames)
    {
      await _boardGameRepo.InsertBoardGamesAsync(boardGames);
    }

    public async Task<BoardGame> UpdateBoardGameAsync(
      int boardGameId,
      UpdateBoardGameDTO model)
    {
      var boardgame = await _boardGameRepo.GetBoardGameAsync(boardGameId);

      if (boardgame == null)
      {
        return null;
      }

      if (model.UsersRated != default)
      {
        boardgame.UsersRated = model.UsersRated;
      }
      if (model.RatingAverage != default)
      {
        boardgame.RatingAverage = model.RatingAverage.Value;
      }

      boardgame.LastModifiedDate = DateTime.UtcNow;

      return await _boardGameRepo.UpdateBoardGameAsync(boardgame);
    }

    public async Task<BoardGame> DeleteBoardGameAsync(int boardGameId)
    {
      var boardgame = await _boardGameRepo.GetBoardGameAsync(boardGameId);
      if (boardgame == null)
      {
        _logger.LogInformation("Boardgame with Id={BoardGameId} does not exist.",
          boardGameId);
        return boardgame;
      }

      _logger.LogInformation("BoardGame {BoardGame} will be deleted.", boardgame);
      await _boardGameRepo.DeleteBoardGameAsync(boardgame);

      return boardgame;
    }
  }
}
