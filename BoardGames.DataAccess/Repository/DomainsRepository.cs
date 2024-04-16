using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardGames.DataAccess.Repository
{
  public class DomainsRepository : IDomainsRepository
  {
    private readonly IRepository _repository;

    public DomainsRepository(IRepository repository)
    {
      _repository = repository;
    }

    public async Task<List<Domain>> GetDomainsListAsync()
    {
      return await _repository
        .Query<Domain>()
        .ToListAsync();
    }

    public async Task<Dictionary<string, Domain>> GetDomainsDictAsync()
    {
      return await _repository
        .Query<Domain>()
        .ToDictionaryAsync(b => b.Name);
    }

    public async Task InsertDomainsAsync(List<Domain> domains)
    {
      await _repository.InsertAsync(domains);
    }
  }
}
