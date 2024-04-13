using System.ComponentModel.DataAnnotations;

namespace BoardGames.DataContract.Models
{
  public class Mechanic
  {
    [Key]
    [Required]
    public int MechanicId { get; set; }
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;
    [Required]
    public DateTime CreatedDate { get; set; }
    [Required]
    public DateTime LastModifiedDate { get; set; }
    public ICollection<BoardGames_Mechanics> BoardGames_Mechanics { get; set; }
  }
}
