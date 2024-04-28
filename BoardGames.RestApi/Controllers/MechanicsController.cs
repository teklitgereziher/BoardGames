using BoardGames.DataContract.Models;
using BoardGames.RestApi.DTOs;
using BoardGames.RestApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MyBGList.DTOs;

namespace BoardGames.RestApi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class MechanicsController : ControllerBase
  {
    private readonly IMechanicService _mechanicService;
    private readonly ILogger<MechanicsController> _logger;

    public MechanicsController(
      IMechanicService mechanicService,
      ILogger<MechanicsController> logger)
    {
      _mechanicService = mechanicService;
      _logger = logger;
    }

    [HttpGet]
    [Route("mechanics")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public async Task<IActionResult> GetMechanicsAsync(
      [FromQuery] RequestDTO<MechanicDTO> input)
    {
      try
      {
        var (mechanics, recordCount) = await _mechanicService.GetMechanicsAsync(
            input.FilterQuery,
            input.PageIndex,
            input.PageSize,
            input.SortColumn,
            input.SortOrder);

        var result = new RestDTO<List<Mechanic>>()
        {
          Data = mechanics,
          PageIndex = input.PageIndex,
          PageSize = input.PageSize,
          RecordCount = recordCount,
          Links = new List<LinkDTO> {
          new LinkDTO(
            Url.Action(
              null,
              "Mechanics",
              new { input.PageIndex, input.PageSize },
              Request.Scheme)!,
            "self",
            "GET"),
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

    [HttpPost]
    [Route("{mechanicId}")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> UpdateMechanicAsync(int mechanicId, MechanicDTO model)
    {
      try
      {
        var mechanic = await _mechanicService.UpdateMechanicAsync(mechanicId, model);
        if (mechanic == null)
        {
          return NotFound("Mechanic to update not found.");
        };

        var result = new RestDTO<Mechanic>()
        {
          Data = mechanic,
          Links = new List<LinkDTO>
        {
          new LinkDTO(
            Url.Action(
              null,
              "Mechanics",
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
    [Route("{mechanicId}")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> DeleteMechanicAsync(int id)
    {
      try
      {
        var mechanic = await _mechanicService.DeleteMechanicAsync(id);
        if (mechanic != null)
        {
          return NotFound("Mechanic to delete not found.");
        }

        var result = new RestDTO<Mechanic>()
        {
          Data = mechanic,
          Links = new List<LinkDTO>
        {
          new LinkDTO(
            Url.Action(
              null,
              "Mechanics",
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
  }
}
