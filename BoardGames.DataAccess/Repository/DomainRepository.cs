using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BoardGames.DataAccess.Repository
{
  public class DomainRepository : IDomainRepository
  {
    private readonly IRepository _repository;

    public DomainRepository(IRepository repository)
    {
      _repository = repository;
    }

    public async Task<Domain> GetDomainAsync(int domainId)
    {
      return await _repository
        .Query<Domain>()
        .FirstOrDefaultAsync(b => b.DomainId == domainId);
    }

    public async Task<(List<Domain>, int)> GetDomainsAsync(
      string filterQuery,
      int pageIndex,
      int pageSize,
      string sortColumn,
      string sortOrder)
    {
      var domains = _repository
        .Query<Domain>()
        .OrderBy($"{sortColumn} {sortOrder}")
        .AsQueryable();

      if (!string.IsNullOrEmpty(filterQuery))
      {
        domains = domains.Where(b => b.Name.Contains(filterQuery));
      }

      var domainsCount = domains.Count();
      var domainsList = await domains
        .Skip(pageIndex * pageSize)
        .Take(pageSize)
        .ToListAsync();

      return (domainsList, domainsCount);
    }

    public async Task<Dictionary<string, Domain>> GetDomainsDictAsync()
    {
      return await _repository
        .Query<Domain>()
        .ToDictionaryAsync(b => b.Name);
    }

    public async Task<Domain> UpdateDomainAsync(Domain domain)
    {
      return await _repository.UpdateAsync(domain);
    }

    public async Task DeleteDomainAsync(Domain domain)
    {
      await _repository.DeleteAsync(domain);
    }
  }
}
