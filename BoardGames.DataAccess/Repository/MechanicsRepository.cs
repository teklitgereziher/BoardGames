using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardGames.DataAccess.Repository
{
  public class MechanicsRepository : IMechanicsRepository
  {
    private readonly IRepository _repository;

    public MechanicsRepository(IRepository repository)
    {
      _repository = repository;
    }

    public async Task<List<Mechanic>> GetMechanicsListAsync()
    {
      return await _repository
        .Query<Mechanic>()
        .ToListAsync();
    }

    public async Task<Dictionary<string, Mechanic>> GetMechanicsDictAsync()
    {
      return await _repository
        .Query<Mechanic>()
        .ToDictionaryAsync(b => b.Name);
    }

    public async Task InsertMechanicsAsync(List<Mechanic> mechanics)
    {
      await _repository.InsertAsync(mechanics);
    }
  }
}
