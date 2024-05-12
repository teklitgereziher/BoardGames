using BoardGames.DataContract.DatabContext;
using BoardGames.DataContract.Models;
using BoardGames.RestApi.Constants;
using BoardGames.RestApi.DTOs;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BoardGames.RestApi.GraphQL
{
  public class Mutation
  {
    [Serial]
    [Authorize(Roles = [RoleNames.Moderator])]
    public async Task<BoardGame> UpdateBoardGame(
        [Service] BoardGamesDbContext context, AddBoardGameDTO model)
    {
      var boardgame = await context.BoardGames
          .Where(b => b.BoardGameId == model.BoardGameId)
          .FirstOrDefaultAsync();

      if (boardgame != null)
      {
        if (!string.IsNullOrEmpty(model.Name))
        {
          boardgame.Name = model.Name;
        }
        if (model.Year > 0)
        {
          boardgame.Year = model.Year;
        }
        boardgame.LastModifiedDate = DateTime.UtcNow;
        context.BoardGames.Update(boardgame);
        await context.SaveChangesAsync();
      }

      return boardgame;
    }

    [Serial]
    [Authorize(Roles = [RoleNames.Administrator])]
    public async Task DeleteBoardGame(
        [Service] BoardGamesDbContext context, int id)
    {
      var boardgame = await context.BoardGames
          .Where(b => b.BoardGameId == id)
          .FirstOrDefaultAsync();

      if (boardgame != null)
      {
        context.BoardGames.Remove(boardgame);
        await context.SaveChangesAsync();
      }
    }

    [Serial]
    [Authorize(Roles = [RoleNames.Moderator])]
    public async Task<Domain> UpdateDomain(
        [Service] BoardGamesDbContext context, DomainDTO model)
    {
      Domain domain = await context.Domains
          .Where(d => d.DomainId == model.Id)
          .FirstOrDefaultAsync();

      if (domain != null)
      {
        if (!string.IsNullOrEmpty(model.Name))
        {
          domain.Name = model.Name;
        }
        domain.LastModifiedDate = DateTime.UtcNow;
        context.Domains.Update(domain);
        await context.SaveChangesAsync();
      }

      return domain;
    }

    [Serial]
    [Authorize(Roles = [RoleNames.Administrator])]
    public async Task DeleteDomain(
        [Service] BoardGamesDbContext context, int id)
    {
      var domain = await context.Domains
          .Where(d => d.DomainId == id)
          .FirstOrDefaultAsync();

      if (domain != null)
      {
        context.Domains.Remove(domain);
        await context.SaveChangesAsync();
      }
    }

    [Serial]
    [Authorize(Roles = [RoleNames.Moderator])]
    public async Task<Mechanic> UpdateMechanic(
        [Service] BoardGamesDbContext context, MechanicDTO model)
    {
      Mechanic mechanic = await context.Mechanics
          .Where(m => m.MechanicId == model.Id)
          .FirstOrDefaultAsync();

      if (mechanic != null)
      {
        if (!string.IsNullOrEmpty(model.Name))
        {
          mechanic.Name = model.Name;
        }
        mechanic.LastModifiedDate = DateTime.UtcNow;

        context.Mechanics.Update(mechanic);
        await context.SaveChangesAsync();
      }

      return mechanic;
    }

    [Serial]
    [Authorize(Roles = [RoleNames.Administrator])]
    public async Task DeleteMechanic(
        [Service] BoardGamesDbContext context, int id)
    {
      Mechanic mechanic = await context.Mechanics
          .Where(m => m.MechanicId == id)
          .FirstOrDefaultAsync();

      if (mechanic != null)
      {
        context.Mechanics.Remove(mechanic);
        await context.SaveChangesAsync();
      }
    }
  }
}
