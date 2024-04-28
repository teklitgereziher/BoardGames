using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.Models;
using BoardGames.RestApi.DTOs;
using BoardGames.RestApi.Services.Interfaces;

namespace BoardGames.RestApi.Services
{
  public class DomainService : IDomainService
  {
    private readonly IDomainRepository _domainRepo;

    public DomainService(IDomainRepository domainRepo)
    {
      _domainRepo = domainRepo;
    }

    public async Task<Domain> GetDomainAsync(int domainId)
    {
      return await _domainRepo.GetDomainAsync(domainId);
    }

    public async Task<(List<Domain>, int)> GetDomainsAsync(
      string filterQuery,
      int pageIndex,
      int pageSize,
      string sortColumn,
      string sortOrder)
    {
      return await _domainRepo.GetDomainsAsync(
        filterQuery,
        pageIndex,
        pageSize,
        sortColumn,
        sortOrder);
    }

    public async Task<Domain> UpdateDomainAsync(int domainId, DomainDTO model)
    {

      var domain = await _domainRepo.GetDomainAsync(domainId);
      if (domain == null)
      {
        return null;
      }

      if (!string.IsNullOrEmpty(model.Name))
      {
        domain.Name = model.Name;
      }
      domain.LastModifiedDate = DateTime.UtcNow;

      return await _domainRepo.UpdateDomainAsync(domain);
    }

    public async Task<Domain> DeleteDomainAsync(int domainId)
    {
      var domain = await _domainRepo.GetDomainAsync(domainId);
      if (domain == null)
      {
        return null;
      }
      await _domainRepo.DeleteDomainAsync(domain);

      return domain;
    }
  }
}
