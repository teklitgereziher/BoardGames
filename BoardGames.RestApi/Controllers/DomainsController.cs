using BoardGames.DataContract.Models;
using BoardGames.RestApi.Attributes;
using BoardGames.RestApi.DTOs;
using BoardGames.RestApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MyBGList.DTOs;
using System.Diagnostics;

namespace BoardGames.RestApi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class DomainsController : ControllerBase
  {
    private readonly IDomainService _domainService;
    private readonly ILogger<DomainsController> _logger;

    public DomainsController(
      IDomainService domainService,
      ILogger<DomainsController> logger)
    {
      _domainService = domainService;
      _logger = logger;
    }

    [HttpGet]
    [Route("getDomains")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    [ManualValidationFilter]
    public async Task<IActionResult> GetDomainsAsync(
            [FromQuery] RequestDTO<DomainDTO> input)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          var details = new ValidationProblemDetails(ModelState);
          details.Extensions["traceId"] =
              Activity.Current?.Id ?? HttpContext.TraceIdentifier;
          if (ModelState.Keys.Any(k => k == "PageSize"))
          {
            details.Type =
                "https://tools.ietf.org/html/rfc7231#section-6.6.2";
            details.Status = StatusCodes.Status501NotImplemented;
            return new ObjectResult(details)
            {
              StatusCode = StatusCodes.Status501NotImplemented
            };
          }
          else
          {
            details.Type =
                "https://tools.ietf.org/html/rfc7231#section-6.5.1";
            details.Status = StatusCodes.Status400BadRequest;
            return new BadRequestObjectResult(details);
          }
        }

        var (domains, recordCount) = await _domainService
            .GetDomainsAsync(
            input.FilterQuery,
            input.PageIndex,
            input.PageSize,
            input.SortColumn,
            input.SortOrder);

        var result = new RestDTO<List<Domain>>()
        {
          Data = domains,
          PageIndex = input.PageIndex,
          PageSize = input.PageSize,
          RecordCount = recordCount,
          Links = new List<LinkDTO> {
          new LinkDTO(
            Url.Action(
              null,
              "Domains",
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
    [Route("updateDomain")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> UpdateDomainAsync(DomainDTO model)
    {
      try
      {
        var domain = await _domainService.UpdateDomainAsync(model);
        if (domain == null)
        {
          return NotFound("Domain to update not found.");
        }

        var result = new RestDTO<Domain>()
        {
          Data = domain,
          Links = new List<LinkDTO>
        {
          new LinkDTO(
            Url.Action(
              null,
              "Domains",
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
    [Route("deleteDomain")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> DeleteDomainAsync(int id)
    {
      try
      {
        var domain = await _domainService.DeleteDomainAsync(id);
        if (domain == null)
        {
          return NotFound("Domain to delete not found.");
        }

        var result = new RestDTO<Domain?>()
        {
          Data = domain,
          Links = new List<LinkDTO>
        {
          new LinkDTO(
            Url.Action(
              null,
              "Domains",
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
