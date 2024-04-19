using BoardGames.DataContract.Models;
using BoardGames.RestApi.Services;
using Microsoft.AspNetCore.Mvc;
using MyBGList.DTOs;
using System.Net;

namespace MyBGList.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class BoardGamesController : ControllerBase
  {
    private readonly IBoardGameService _boardGameService;
    private readonly ILogger<BoardGamesController> _logger;

    public BoardGamesController(
      IBoardGameService boardGameService,
      ILogger<BoardGamesController> logger)
    {
      _boardGameService = boardGameService;
      _logger = logger;
    }

    [HttpGet]
    [Route("games")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public async Task<IActionResult> GetBoardGames(
      string filterQuery = null,
      int pageIndex = 0,
      int pageSize = 10,
      string sortColumn = "BoardGameId",
      string sortOrder = "ASC")
    {
      try
      {
        var (games, gameCount) = await _boardGameService
          .GetBoardGamesAsync(filterQuery, pageIndex, pageSize, sortColumn, sortOrder);
        var response = new RestDTO<List<BoardGame>>
        {
          Data = games,
          PageIndex = pageIndex,
          PageSize = pageSize,
          RecordCount = gameCount,
          Links = new List<LinkDTO> {
          new LinkDTO(
            Url.Action(
              null,
              "BoardGames",
              new { pageIndex, pageSize },
              Request.Scheme)!,
            "self",
            "GET"),
          }
        };

        return Ok(response);
      }
      catch (Exception ex)
      {
        return StatusCode((int)HttpStatusCode.InternalServerError,
          $"Internal server error, Error= {ex.Message}");
      }
    }
  }
}
