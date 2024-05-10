using BoardGames.DataContract.Models;
using BoardGames.RestApi.Constants;
using BoardGames.RestApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BoardGames.RestApi.Controllers
{
  //[Authorize]
  [Route("[controller]")]
  [ApiController]
  public class SeedController : ControllerBase
  {
    private readonly ISeedDataService _seedService;
    private readonly ILogger<SeedController> _logger;
    private readonly UserManager<BoardGameUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedController(
      ISeedDataService seedService,
      ILogger<SeedController> logger,
      UserManager<BoardGameUser> userManager,
      RoleManager<IdentityRole> roleManager)
    {
      _logger = logger;
      _seedService = seedService;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    [HttpPut(Name = "Seed")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> SeedBoardGames()
    {
      return await _seedService.SeedDataAsync();
    }

    [HttpPost]
    [Route("roles")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> AddRoles()
    {
      int rolesCreated = 0;
      int usersAddedToRoles = 0;

      if (!await _roleManager.RoleExistsAsync(RoleNames.Moderator))
      {
        await _roleManager.CreateAsync(
            new IdentityRole(RoleNames.Moderator));
        rolesCreated++;
      }
      if (!await _roleManager.RoleExistsAsync(RoleNames.Administrator))
      {
        await _roleManager.CreateAsync(
            new IdentityRole(RoleNames.Administrator));
        rolesCreated++;
      }

      var testModerator = await _userManager
          .FindByNameAsync("moderatoruser");
      if (testModerator != null
          && !await _userManager.IsInRoleAsync(
              testModerator, RoleNames.Moderator))
      {
        await _userManager.AddToRoleAsync(testModerator, RoleNames.Moderator);
        usersAddedToRoles++;
      }

      var testAdministrator = await _userManager
          .FindByNameAsync("administratoruser");
      if (testAdministrator != null
          && !await _userManager.IsInRoleAsync(
              testAdministrator, RoleNames.Administrator))
      {
        await _userManager.AddToRoleAsync(
            testAdministrator, RoleNames.Moderator);
        await _userManager.AddToRoleAsync(
            testAdministrator, RoleNames.Administrator);
        usersAddedToRoles++;
      }

      return new JsonResult(new
      {
        RolesCreated = rolesCreated,
        UsersAddedToRoles = usersAddedToRoles
      });
    }
  }
}
