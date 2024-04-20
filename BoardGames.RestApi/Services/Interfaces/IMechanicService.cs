using BoardGames.DataContract.Models;
using BoardGames.RestApi.DTOs;

namespace BoardGames.RestApi.Services.Interfaces
{
  public interface IMechanicService
  {
    Task<Mechanic> GetMechanicAsync(int domainId);
    Task<(List<Mechanic>, int)> GetMechanicsAsync(
      string filterQuery, int pageIndex,
      int pageSize, string sortColumn,
      string sortOrder);
    Task<Mechanic> UpdateMechanicAsync(MechanicDTO model);
    Task<Mechanic> DeleteMechanicAsync(int domainId);
  }
}
