using BoardGames.DataContract.Models;

namespace BoardGames.DataAccess.Interfaces
{
  public interface IDomainsRepository
  {
    Task<List<Domain>> GetDomainsListAsync();
    Task<Dictionary<string, Domain>> GetDomainsDictAsync();
    Task InsertDomainsAsync(List<Domain> domains);
  }
}
