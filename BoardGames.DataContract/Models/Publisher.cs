using System.ComponentModel.DataAnnotations;

namespace BoardGames.DataContract.Models
{
  public class Publisher
  {
    [Key]
    [Required]
    public int PublisherId { get; set; }
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
    [Required]
    public DateTime CreatedDate { get; set; }
    [Required]
    public DateTime LastModifiedDate { get; set; }
    public ICollection<BoardGame> BoardGames { get; set; }
  }
}
