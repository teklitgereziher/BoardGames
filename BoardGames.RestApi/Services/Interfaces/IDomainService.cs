using BoardGames.DataContract.Models;
using BoardGames.RestApi.DTOs;

namespace BoardGames.RestApi.Services.Interfaces
{
  public interface IDomainService
  {
    Task<Domain> GetDomainAsync(int domainId);
    Task<(List<Domain>, int)> GetDomainsAsync(
      string filterQuery, int pageIndex,
      int pageSize, string sortColumn,
      string sortOrder);
    Task<Domain> UpdateDomainAsync(int domainId, DomainDTO model);
    Task<Domain> DeleteDomainAsync(int domainId);
  }
}
