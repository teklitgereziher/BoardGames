using BoardGames.DataContract.DatabContext;
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
        [Service] BoardGamesDbContext context)
        => context.BoardGames;

    [Serial]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain> GetDomains(
        [Service] BoardGamesDbContext context)
        => context.Domains;

    [Serial]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Mechanic> GetMechanics(
        [Service] BoardGamesDbContext context)
        => context.Mechanics;
  }
}
