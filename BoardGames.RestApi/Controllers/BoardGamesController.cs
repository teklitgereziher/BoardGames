using BoardGames.DataContract.Models;
using BoardGames.RestApi.Attributes;
using BoardGames.RestApi.Constants;
using BoardGames.RestApi.DTOs;
using BoardGames.RestApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBGList.DTOs;
using System.ComponentModel.DataAnnotations;

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
    [Route("retrieveGames")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public async Task<IActionResult> RetrieveBoardGamesAsync(
      [FromQuery] RequestDTO<AddBoardGameDTO> input)
    {
      try
      {
        var (games, gameCount) = await _boardGameService
          .GetBoardGamesAsync(
          input.FilterQuery,
          input.PageIndex,
          input.PageSize,
          input.SortColumn,
          input.SortOrder);
        var response = new RestDTO<List<BoardGame>>
        {
          Data = games,
          PageIndex = input.PageIndex,
          PageSize = input.PageSize,
          RecordCount = gameCount,
          Links = new List<LinkDTO> {
          new LinkDTO(
            Url.Action(
              null,
              "BoardGames",
              new { input.PageIndex, input.PageSize },
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

    [Authorize(Roles = RoleNames.Administrator)]
    [HttpGet]
    [Route("games")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public async Task<IActionResult> GetBoardGamesAsync(
      string filterQuery = null,
      int pageIndex = 0,
      [Range(1, 100)] int pageSize = 10,
      [SortColumnValidator(typeof(AddBoardGameDTO))] string sortColumn = "BoardGameId",
      [RegularExpression("ASC|DESC")] string sortOrder = "ASC")
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

    [Authorize]
    [HttpPost]
    [Route("addBoardGame")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> AddBoardGameAsync(AddBoardGameDTO boardGameDTO)
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

    [Authorize]
    [HttpPut]
    [Route("{boardGameId}")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> UpdateBoardGameAsync(
      int boardGameId,
      UpdateBoardGameDTO model)
    {

      try
      {
        var boardgame = await _boardGameService.UpdateBoardGameAsync(boardGameId, model);
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

    [Authorize(Roles = RoleNames.Administrator)]
    [HttpDelete]
    [Route("{boardGameId}")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> DeleteBoardGameAsync(int boardGameId)
    {
      try
      {
        var boardgame = await _boardGameService.DeleteBoardGameAsync(boardGameId);
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
              boardGameId,
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

    [Authorize(Roles = RoleNames.Administrator)]
    [HttpDelete]
    [Route("deleteGames")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> DeleteGamesAsync(string boardGameIds)
    {
      try
      {
        var idArray = boardGameIds.Split(',').Select(x => int.Parse(x));
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
              boardGameIds,
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
