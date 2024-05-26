using BoardGames.DataContract.Models;
using BoardGames.RestApi.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BoardGames.RestApi.Controllers
{
  [Route("[controller]/")]
  [ApiController]
  public class AccountController : ControllerBase
  {
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly UserManager<BoardGameUser> _userManager;

    public AccountController(
      ILogger<AccountController> logger,
      IConfiguration configuration,
      UserManager<BoardGameUser> userManager)
    {
      _logger = logger;
      _configuration = configuration;
      _userManager = userManager;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="input">A DTO containing the user data.</param>
    /// <returns> A 201 - Created status code in case of success.</returns>
    /// <response code="201">User has been registered</response>
    /// <response code="400">Invalid data</response>
    /// <response code="500">An error occurred</response>
    /// [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Register(RegisterDTO input)
    {
      try
      {
        if (ModelState.IsValid)
        {
          var newUser = new BoardGameUser();
          newUser.UserName = input.UserName;
          newUser.Email = input.Email;

          var result = await _userManager.CreateAsync(newUser, input.Password);

          if (result.Succeeded)
          {
            _logger.LogInformation("User {userName} ({email}) has been created.",
            newUser.UserName,
            newUser.Email);

            return StatusCode(201, $"User '{newUser.UserName}' has been created.");
          }
          else
          {
            throw new Exception(string.Format("Error: {0}", string.Join(" ",
              result.Errors.Select(e => e.Description))));
          }
        }
        else
        {
          var details = new ValidationProblemDetails(ModelState);
          details.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
          details.Status = StatusCodes.Status400BadRequest;
          return new BadRequestObjectResult(details);
        }
      }
      catch (Exception e)
      {
        var exceptionDetails = new ProblemDetails()
        {
          Detail = e.Message,
          Status = StatusCodes.Status500InternalServerError,
          Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        return StatusCode(StatusCodes.Status500InternalServerError, exceptionDetails);
      }
    }

    /// <summary>
    /// Performs a user login.
    /// </summary>
    /// <param name="input">A DTO containing the user's credentials.</param>
    /// <returns>The Bearer Token (in JWT format).</returns>
    /// <response code="200">User has been logged in</response>
    /// <response code="400">Login failed (bad request)</response>
    /// <response code="401">Login failed (unauthorized)</response>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Login(LoginDTO input)
    {
      try
      {
        if (ModelState.IsValid)
        {
          var user = await _userManager.FindByNameAsync(input.UserName);
          var isValidPsd = await _userManager.CheckPasswordAsync(user, input.Password);

          if (user == null || !isValidPsd)
          {
            throw new Exception("Invalid login attempt.");
          }
          else
          {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(
                        _configuration["JWT:SigningKey"])),
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
              new Claim( ClaimTypes.Name, user.UserName)
            };

            claims.AddRange(
                (await _userManager.GetRolesAsync(user))
                    .Select(r => new Claim(ClaimTypes.Role, r)));

            var jwtObject = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(300),
                signingCredentials: signingCredentials);

            var jwtString = new JwtSecurityTokenHandler()
                .WriteToken(jwtObject);

            return StatusCode(
                StatusCodes.Status200OK,
                jwtString);
          }
        }
        else
        {
          var details = new ValidationProblemDetails(ModelState)
          {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Status = StatusCodes.Status400BadRequest
          };

          return new BadRequestObjectResult(details);
        }
      }
      catch (Exception e)
      {
        var exceptionDetails = new ProblemDetails
        {
          Detail = e.Message,
          Status = StatusCodes.Status401Unauthorized,
          Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        return StatusCode(
            StatusCodes.Status401Unauthorized,
            exceptionDetails);
      }
    }
  }
}
