using Microsoft.AspNetCore.Mvc;

namespace BoardGames.RestApi.Services
{
  public interface ISeedDataService
  {
    Task<JsonResult> SeedDataAsync();
  }
}
