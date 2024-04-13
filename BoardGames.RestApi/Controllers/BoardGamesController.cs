using Microsoft.AspNetCore.Mvc;

namespace MyBGList.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class BoardGamesController : ControllerBase
  {
    private readonly ILogger<BoardGamesController> _logger;

    public BoardGamesController(ILogger<BoardGamesController> logger)
    {
      _logger = logger;
    }
  }
}
