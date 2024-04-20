using Microsoft.AspNetCore.Mvc;

namespace BoardGames.RestApi.Services.Interfaces
{
  public interface ISeedDataService
  {
    Task<JsonResult> SeedDataAsync();
  }
}
