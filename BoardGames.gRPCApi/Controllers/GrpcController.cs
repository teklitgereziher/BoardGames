using BoardGames.gRPCApi.gRPC;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

namespace BoardGames.gRPCApi.Controllers
{
  [Route("[controller]/[action]")]
  [ApiController]
  public class GrpcController : ControllerBase
  {
    [HttpGet("{boardGameId}")]
    public async Task<BoardGameResponse> GetBoardGame(int boardGameId)
    {
      // Set up the gRPC channel
      using var channel = GrpcChannel.ForAddress("https://localhost:40443");

      // Instantiate the gRPC client
      var client = new gRPC.Grpc.GrpcClient(channel);

      // Perform the client-to-server call
      var response = await client.GetBoardGameAsync(
        new BoardGameRequest { Id = boardGameId }
        );

      return response;
    }

    [HttpPost]
    public async Task<BoardGameResponse> UpdateBoardGame(
        string token,
        int id,
        string name)
    {
      var headers = new Metadata
      {
        { "Authorization", $"Bearer {token}" }
      };

      using var channel = GrpcChannel.ForAddress("https://localhost:40443");
      var client = new gRPC.Grpc.GrpcClient(channel);
      var response = await client.UpdateBoardGameAsync(
                          new UpdateBoardGameRequest
                          {
                            Id = id,
                            Name = name
                          },
                          headers
                          );

      return response;
    }

    [HttpGet("{domainId}")]
    public async Task<DomainResponse> GetDomain(int domainId)
    {
      using var channel = GrpcChannel.ForAddress("https://localhost:40443");
      var client = new gRPC.Grpc.GrpcClient(channel);
      var response = await client.GetDomainAsync(
        new DomainRequest { Id = domainId }
        );

      return response;
    }

    [HttpPost]
    public async Task<DomainResponse> UpdateDomain(
        string token,
        int id,
        string name)
    {
      var headers = new Metadata
      {
        { "Authorization", $"Bearer {token}" }
      };

      using var channel = GrpcChannel.ForAddress("https://localhost:40443");
      var client = new gRPC.Grpc.GrpcClient(channel);
      var response = await client.UpdateDomainAsync(
                          new UpdateDomainRequest
                          {
                            Id = id,
                            Name = name
                          },
                          headers);

      return response;
    }

    [HttpGet("{mechanicId}")]
    public async Task<MechanicResponse> GetMechanic(int mechanicId)
    {
      using var channel = GrpcChannel.ForAddress("https://localhost:40443");
      var client = new gRPC.Grpc.GrpcClient(channel);
      var response = await client.GetMechanicAsync(
        new MechanicRequest { Id = mechanicId });

      return response;
    }

    [HttpPost]
    public async Task<MechanicResponse> UpdateMechanic(
        string token,
        int id,
        string name)
    {
      var headers = new Metadata
      {
        { "Authorization", $"Bearer {token}" }
      };

      using var channel = GrpcChannel.ForAddress("https://localhost:40443");
      var client = new gRPC.Grpc.GrpcClient(channel);
      var response = await client.UpdateMechanicAsync(
                          new UpdateMechanicRequest
                          {
                            Id = id,
                            Name = name
                          },
                          headers);

      return response;
    }
  }
}
