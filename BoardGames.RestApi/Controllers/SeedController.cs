using BoardGames.RestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoardGames.RestApi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class SeedController : ControllerBase
  {
    private readonly IWebHostEnvironment _env;
    private readonly ISeedDataService _seedService;
    private readonly ILogger<SeedController> _logger;

    public SeedController(
      ISeedDataService seedService,
      IWebHostEnvironment env,
      ILogger<SeedController> logger)
    {
      _logger = logger;
      _seedService = seedService;
      _env = env;
    }

    [HttpPut(Name = "Seed")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> SeedBoardGames()
    {
      return await _seedService.SeedDataAsync();
    }
  }
}
