using System.ComponentModel.DataAnnotations;

namespace BoardGames.RestApi.DTOs
{
  public class LoginDTO
  {
    [Required]
    [MaxLength(255)]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
  }
}
