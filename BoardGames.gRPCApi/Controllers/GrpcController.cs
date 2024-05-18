using BoardGames.gRPCApi.gRPC;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

namespace BoardGames.gRPCApi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class GrpcController : ControllerBase
  {
    [HttpGet("{id}")]
    public async Task<BoardGameResponse> GetBoardGame(int id)
    {
      // Set up the gRPC channel
      using var channel = GrpcChannel
          .ForAddress("https://localhost:40443");

      // Instantiate the gRPC client
      var client = new gRPC.Grpc.GrpcClient(channel);

      // Perform the client-to-server call
      var response = await client.GetBoardGameAsync(new BoardGameRequest { Id = id });

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

      using var channel = GrpcChannel
          .ForAddress("https://localhost:40443");
      var client = new gRPC.Grpc.GrpcClient(channel);
      var response = await client.UpdateBoardGameAsync(
                          new UpdateBoardGameRequest
                          {
                            Id = id,
                            Name = name
                          },
                          headers);
      return response;
    }
  }
}
