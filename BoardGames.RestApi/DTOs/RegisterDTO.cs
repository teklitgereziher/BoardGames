using System.ComponentModel.DataAnnotations;

namespace BoardGames.RestApi.DTOs
{
  public class RegisterDTO
  {
    [Required]
    [MaxLength(255)]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
  }
}
