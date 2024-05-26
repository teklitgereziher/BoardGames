namespace BoardGames.RestApi.DTOs
{
  public class BoardGameDTO
  {
    public int BoardGameId { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public int MinPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public int PlayTime { get; set; }
    public int MinAge { get; set; }
    public int UsersRated { get; set; }
    public decimal RatingAverage { get; set; }
    public int BGGRank { get; set; }
    public decimal ComplexityAverage { get; set; }
    public int OwnedUsers { get; set; }
  }
}
