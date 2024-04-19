using System.ComponentModel.DataAnnotations;

namespace BoardGames.RestApi.DTOs
{
  public class UpdateBoardGameDTO
  {
    [Required]
    public int Id { get; set; }
    public int UsersRated { get; set; }
    public decimal? RatingAverage { get; set; }
  }
}
