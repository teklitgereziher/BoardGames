using System.ComponentModel.DataAnnotations;

namespace BoardGames.GraphqlApi.DTOs
{
  public class MechanicDTO
  {
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
  }
}
