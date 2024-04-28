using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.Models;
using BoardGames.RestApi.DTOs;
using BoardGames.RestApi.Services.Interfaces;

namespace BoardGames.RestApi.Services
{
  public class MechanicService : IMechanicService
  {
    private readonly IMechanicRepository _mechanicsRepo;

    public MechanicService(IMechanicRepository mechanicsRepo)
    {
      _mechanicsRepo = mechanicsRepo;
    }

    public async Task<Mechanic> GetMechanicAsync(int domainId)
    {
      return await _mechanicsRepo.GetMechanicAsync(domainId);
    }

    public async Task<(List<Mechanic>, int)> GetMechanicsAsync(
      string filterQuery,
      int pageIndex,
      int pageSize,
      string sortColumn,
      string sortOrder)
    {
      return await _mechanicsRepo.GetMechanicsAsync(
        filterQuery,
        pageIndex,
        pageSize,
        sortColumn,
        sortOrder);
    }

    public async Task<Mechanic> UpdateMechanicAsync(int mechanicId, MechanicDTO model)
    {
      var mechanic = await _mechanicsRepo.GetMechanicAsync(mechanicId);
      if (mechanic == null)
      {
        return null;
      }

      if (!string.IsNullOrEmpty(model.Name))
      {
        mechanic.Name = model.Name;
      }
      mechanic.LastModifiedDate = DateTime.UtcNow;

      return await _mechanicsRepo.UpdateMechanicAsync(mechanic);
    }

    public async Task<Mechanic> DeleteMechanicAsync(int domainId)
    {
      var mechanic = await _mechanicsRepo.GetMechanicAsync(domainId);
      if (mechanic == null)
      {
        return null;
      }
      await _mechanicsRepo.DeleteMechanicAsync(mechanic);

      return mechanic;
    }
  }
}
