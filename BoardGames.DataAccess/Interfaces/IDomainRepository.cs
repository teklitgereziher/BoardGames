using BoardGames.DataContract.Models;

namespace BoardGames.DataAccess.Interfaces
{
  public interface IDomainRepository
  {
    Task<Domain> GetDomainAsync(int domainId);
    Task<(List<Domain>, int)> GetDomainsAsync(
      string filterQuery, int pageIndex, int pageSize,
      string sortColumn, string sortOrder);
    Task<Dictionary<string, Domain>> GetDomainsDictAsync();
    Task<Domain> UpdateDomainAsync(Domain domain);
    Task DeleteDomainAsync(Domain domain);
  }
}
