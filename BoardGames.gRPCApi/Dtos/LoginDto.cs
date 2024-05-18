using System.ComponentModel.DataAnnotations;

namespace BoardGames.gRPCApi.Dtos
{
  public class LoginDto
  {
    [Required]
    [MaxLength(255)]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
  }
}
