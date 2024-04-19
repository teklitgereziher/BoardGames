using BoardGames.DataContract.Models;
using BoardGames.RestApi.DTOs;
using BoardGames.RestApi.Services;
using Microsoft.AspNetCore.Mvc;
using MyBGList.DTOs;

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
        return StatusCode(
          StatusCodes.Status500InternalServerError,
          $"Internal server error, Error= {ex.Message}"
          );
      }
    }

    [HttpPost]
    [Route("addBoardGame")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> AddBoardGame(AddBoardGameDTO boardGameDTO)
    {
      try
      {
        var boardGame = await _boardGameService.AddBoardGameAsync(boardGameDTO);
        return Created(string.Empty, boardGame);
      }
      catch (Exception ex)
      {
        return StatusCode(
          StatusCodes.Status500InternalServerError,
          $"Internal server error, Error= {ex.Message}"
          );
      }
    }

    [HttpPut]
    [Route("updateBoardGame")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> UpdateBoardGameAsync(UpdateBoardGameDTO model)
    {

      try
      {
        var boardgame = await _boardGameService.UpdateBoardGameAsync(model);
        if (boardgame == null)
        {
          return NotFound("Board game to update not found.");
        }

        var result = new RestDTO<BoardGame>()
        {
          Data = boardgame,
          Links = new List<LinkDTO>
        {
          new LinkDTO(
            Url.Action(
              null,
              "BoardGames",
              model,
              Request.Scheme)!,
            "self",
            "POST"),
        }
        };

        return Ok(result);
      }
      catch (Exception ex)
      {
        return StatusCode(
          StatusCodes.Status500InternalServerError,
          $"Internal server error, Error= {ex.Message}"
          );
      }
    }

    [HttpDelete]
    [Route("deleteGame")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> DeleteBoardGameAsync(int id)
    {
      try
      {
        var boardgame = await _boardGameService.DeleteBoardGameAsync(id);
        if (boardgame != null)
        {
          return NotFound("Board game to delete not found.");
        }

        var result = new RestDTO<BoardGame>()
        {
          Data = boardgame,
          Links = new List<LinkDTO>
        {
          new LinkDTO(
            Url.Action(
              null,
              "BoardGames",
              id,
              Request.Scheme)!,
            "self",
            "DELETE"),
        }
        };

        return Ok(result);
      }
      catch (Exception ex)
      {
        return StatusCode(
          StatusCodes.Status500InternalServerError,
          $"Internal server error, Error= {ex.Message}"
          );
      }
    }

    [HttpDelete]
    [Route("deleteGames")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> DeleteGames(string ids)
    {
      try
      {
        var idArray = ids.Split(',').Select(x => int.Parse(x));
        var deletedBGList = new List<BoardGame>();

        foreach (int id in idArray)
        {
          var boardgame = await _boardGameService.DeleteBoardGameAsync(id);
        }

        var result = new RestDTO<BoardGame[]>()
        {
          Data = null,
          Links = new List<LinkDTO>
        {
          new LinkDTO(
            Url.Action(
              null,
              "BoardGames",
              ids,
              Request.Scheme)!,
            "self",
            "DELETE"),
        }
        };

        return Ok(result);
      }
      catch (Exception ex)
      {
        return StatusCode(
          StatusCodes.Status500InternalServerError,
          $"Internal server error, Error= {ex.Message}"
          );
      }
    }
  }
}
