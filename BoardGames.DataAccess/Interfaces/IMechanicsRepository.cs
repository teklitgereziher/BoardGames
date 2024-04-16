using BoardGames.DataContract.Models;

namespace BoardGames.DataAccess.Interfaces
{
  public interface IMechanicsRepository
  {
    Task<List<Mechanic>> GetMechanicsListAsync();
    Task<Dictionary<string, Mechanic>> GetMechanicsDictAsync();
    Task InsertMechanicsAsync(List<Mechanic> mechanics);
  }
}
