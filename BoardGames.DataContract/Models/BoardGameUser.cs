using Microsoft.AspNetCore.Identity;

namespace BoardGames.DataContract.Models
{
  public class BoardGameUser : IdentityUser
  {
    public int UserId { get; set; }
  }
}
