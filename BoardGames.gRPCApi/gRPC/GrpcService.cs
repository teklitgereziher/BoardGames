using BoardGames.DataAccess.Interfaces;
using BoardGames.gRPCApi.Constants;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace BoardGames.gRPCApi.gRPC
{
  // Extends the codegen service base class
  public class GrpcService : Grpc.GrpcBase
  {
    private readonly IBoardGameRepository _gameRepo;
    private readonly IMechanicRepository _mechanicRepo;
    private readonly IDomainRepository _domainRepo;

    public GrpcService(
      IBoardGameRepository gameRepository,
      IMechanicRepository mechanicRepo,
      IDomainRepository domainRepo)
    {
      _gameRepo = gameRepository;
      _mechanicRepo = mechanicRepo;
      _domainRepo = domainRepo;
    }

    public override async Task<BoardGameResponse> GetBoardGame(
        BoardGameRequest request,
        ServerCallContext scc)
    {
      var bg = await _gameRepo.GetBoardGameAsync(request.Id);
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
      var bg = await _gameRepo.GetBoardGameAsync(request.Id);
      var response = new BoardGameResponse();
      if (bg != null)
      {
        bg.Name = request.Name;
        await _gameRepo.UpdateBoardGameAsync(bg);
        response.Id = bg.BoardGameId;
        response.Name = bg.Name;
        response.Year = bg.Year;
      }

      return response;
    }

    public override async Task<DomainResponse> GetDomain(
            DomainRequest request,
            ServerCallContext scc)
    {
      var domain = await _domainRepo.GetDomainAsync(request.Id);
      var response = new DomainResponse();
      if (domain != null)
      {
        response.Id = domain.DomainId;
        response.Name = domain.Name;
      }

      return response;
    }

    [Authorize(Roles = RoleNames.Moderator)]
    public override async Task<DomainResponse> UpdateDomain(
        UpdateDomainRequest request,
        ServerCallContext scc)
    {
      var domain = await _domainRepo.GetDomainAsync(request.Id);
      var response = new DomainResponse();
      if (domain != null)
      {
        domain.Name = request.Name;
        await _domainRepo.UpdateDomainAsync(domain);
        response.Id = domain.DomainId;
        response.Name = domain.Name;
      }
      return response;
    }

    public override async Task<MechanicResponse> GetMechanic(
        MechanicRequest request,
        ServerCallContext scc)
    {
      var mechanic = await _mechanicRepo.GetMechanicAsync(request.Id);
      var response = new MechanicResponse();
      if (mechanic != null)
      {
        response.Id = mechanic.MechanicId;
        response.Name = mechanic.Name;
      }
      return response;
    }

    [Authorize(Roles = RoleNames.Moderator)]
    public override async Task<MechanicResponse> UpdateMechanic(
        UpdateMechanicRequest request,
        ServerCallContext scc)
    {
      var mechanic = await _mechanicRepo.GetMechanicAsync(request.Id);
      var response = new MechanicResponse();
      if (mechanic != null)
      {
        mechanic.Name = request.Name;
        await _mechanicRepo.UpdateMechanicAsync(mechanic);
        response.Id = mechanic.MechanicId;
        response.Name = mechanic.Name;
      }
      return response;
    }
  }
}
