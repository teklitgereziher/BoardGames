using System.ComponentModel.DataAnnotations;

namespace BoardGames.DataContract.Models
{
  public class Category
  {
    public int CategoryId { get; set; }
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
    [Required]
    public DateTime CreatedDate { get; set; }
    [Required]
    public DateTime LastModifiedDate { get; set; }
    public ICollection<BoardGames_Categories> BoardGames_Categories { get; set; }
  }
}
