namespace BoardGames.DataContract.Models
{
  public class BoardGames_Categories
  {
    public int BoardGameId { get; set; }
    public int CategoryId { get; set; }
    public BoardGame BoardGame { get; set; }
    public Category Category { get; set; }
  }
}
