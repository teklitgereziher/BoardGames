using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BoardGames.DataAccess.Repository
{
  public class MechanicRepository : IMechanicRepository
  {
    private readonly IRepository _repository;

    public MechanicRepository(IRepository repository)
    {
      _repository = repository;
    }

    public async Task<Mechanic> GetMechanicAsync(int mechanicId)
    {
      return await _repository
        .Query<Mechanic>()
        .FirstOrDefaultAsync(b => b.MechanicId == mechanicId);
    }

    public async Task<Dictionary<string, Mechanic>> GetMechanicsDictAsync()
    {
      return await _repository
        .Query<Mechanic>()
        .ToDictionaryAsync(b => b.Name);
    }

    public async Task<(List<Mechanic>, int)> GetMechanicsAsync(
      string filterQuery,
      int pageIndex,
      int pageSize,
      string sortColumn,
      string sortOrder)
    {
      var mechanics = _repository
        .Query<Mechanic>()
        .OrderBy($"{sortColumn} {sortOrder}")
        .AsQueryable();

      if (!string.IsNullOrEmpty(filterQuery))
      {
        mechanics = mechanics.Where(b => b.Name.Contains(filterQuery));
      }

      var mechanicCount = mechanics.Count();
      var mechanicsList = await mechanics
        .Skip(pageIndex * pageSize)
        .Take(pageSize)
        .ToListAsync();

      return (mechanicsList, mechanicCount);
    }

    public async Task<Mechanic> UpdateMechanicAsync(Mechanic mechanic)
    {
      return await _repository.UpdateAsync(mechanic);
    }

    public async Task DeleteMechanicAsync(Mechanic mechanic)
    {
      await _repository.DeleteAsync(mechanic);
    }
  }
}
