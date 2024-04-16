using BoardGames.Shared.Models.Csv;

namespace BoardGames.Shared.Interfaces
{
  public interface ICsvReader
  {
    IEnumerable<BggRecord> Read();
  }
}
