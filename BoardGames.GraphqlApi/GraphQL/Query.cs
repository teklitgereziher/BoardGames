using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.Models;

namespace BoardGames.GraphqlApi.GraphQL
{
  public class Query
  {
    [Serial]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<BoardGame> GetBoardGames(
        [Service] IRepository repository)
        => repository.Query<BoardGame>();

    [Serial]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain> GetDomains(
        [Service] IRepository repository)
        => repository.Query<Domain>();

    [Serial]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Mechanic> GetMechanics(
        [Service] IRepository repository)
        => repository.Query<Mechanic>();
  }
}
