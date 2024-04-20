using BoardGames.DataContract.Models;

namespace BoardGames.DataAccess.Interfaces
{
  public interface IMechanicRepository
  {
    Task<Mechanic> GetMechanicAsync(int mechanicId);
    Task<Dictionary<string, Mechanic>> GetMechanicsDictAsync();
    Task<(List<Mechanic>, int)> GetMechanicsAsync(
      string filterQuery, int pageIndex, int pageSize,
      string sortColumn, string sortOrder);
    Task<Mechanic> UpdateMechanicAsync(Mechanic mechanic);
    Task DeleteMechanicAsync(Mechanic mechanic);
  }
}
