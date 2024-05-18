using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.DatabContext;
using BoardGames.gRPCApi.Constants;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace BoardGames.gRPCApi.gRPC
{
  // Extends the codegen service base class
  public class GrpcService : Grpc.GrpcBase
  {
    private readonly BoardGamesDbContext _context;
    private readonly IBoardGameRepository _gameRepository;

    public GrpcService(BoardGamesDbContext context, IBoardGameRepository gameRepository)
    {
      _context = context;
      _gameRepository = gameRepository;
    }

    public override async Task<BoardGameResponse> GetBoardGame(
        BoardGameRequest request,
        ServerCallContext scc)
    {
      var bg = await _gameRepository.GetBoardGameAsync(request.Id);
      var response = new BoardGameResponse();
      if (bg != null)
      {
        response.Id = bg.BoardGameId;
        response.Name = bg.Name;
        response.Year = bg.Year;
      }
      return response;
    }

    [Authorize(Roles = RoleNames.Moderator)]
    public override async Task<BoardGameResponse> UpdateBoardGame(
        UpdateBoardGameRequest request,
        ServerCallContext scc)
    {
      var bg = await _gameRepository.GetBoardGameAsync(request.Id);
      var response = new BoardGameResponse();
      if (bg != null)
      {
        bg.Name = request.Name;
        await _gameRepository.UpdateBoardGameAsync(bg);
        response.Id = bg.BoardGameId;
        response.Name = bg.Name;
        response.Year = bg.Year;
      }

      return response;
    }
  }
}
