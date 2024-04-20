using BoardGames.DataAccess.Interfaces;
using BoardGames.DataContract.DatabContext;
using BoardGames.DataContract.Models;
using BoardGames.RestApi.Services.Interfaces;
using BoardGames.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoardGames.RestApi.Services
{
  public class SeedDataService : ISeedDataService
  {
    private readonly BoardGamesDbContext _context;
    private IBoardGameRepository _boardGameRepo;
    private IDomainRepository _domainRepo;
    private IMechanicRepository _mechanicsRepo;
    private readonly ICsvReader _csvDataReader;

    public SeedDataService(
      BoardGamesDbContext context,
      IBoardGameRepository repository,
      IDomainRepository domainRepo,
      IMechanicRepository mechanicsRepo,
      ICsvReader csvDataReader)
    {
      _context = context;
      _boardGameRepo = repository;
      _domainRepo = domainRepo;
      _mechanicsRepo = mechanicsRepo;
      _csvDataReader = csvDataReader;
    }

    public async Task<JsonResult> SeedDataAsync()
    {
      var existingBoardGames = await _boardGameRepo.GetBoardGamesDictAsync();
      var existingDomains = await _domainRepo.GetDomainsDictAsync();
      var existingMechanics = await _mechanicsRepo.GetMechanicsDictAsync();
      var now = DateTime.UtcNow;

      var records = _csvDataReader.Read();

      var skippedRows = 0;
      foreach (var record in records)
      {
        if (!record.ID.HasValue
        || string.IsNullOrEmpty(record.Name)
        || existingBoardGames.ContainsKey(record.ID.Value))
        {
          skippedRows++;
          continue;
        }
        var boardgame = new BoardGame()
        {
          BoardGameId = record.ID.Value,
          Name = record.Name,
          BGGRank = record.BGGRank ?? 0,
          ComplexityAverage = record.ComplexityAverage ?? 0,
          MaxPlayers = record.MaxPlayers ?? 0,
          MinAge = record.MinAge ?? 0,
          MinPlayers = record.MinPlayers ?? 0,
          OwnedUsers = record.OwnedUsers ?? 0,
          PlayTime = record.PlayTime ?? 0,
          RatingAverage = record.RatingAverage ?? 0,
          UsersRated = record.UsersRated ?? 0,
          Year = record.YearPublished ?? 0,
          CreatedDate = now,
          LastModifiedDate = now,
        };
        _context.BoardGames.Add(boardgame);

        if (!string.IsNullOrEmpty(record.Domains))
          foreach (var domainName in record.Domains
          .Split(',', StringSplitOptions.TrimEntries)
          .Distinct(StringComparer.InvariantCultureIgnoreCase))
          {
            var domain = existingDomains.GetValueOrDefault(domainName);
            if (domain == null)
            {
              domain = new Domain()
              {
                Name = domainName,
                CreatedDate = now,
                LastModifiedDate = now
              };
              _context.Domains.Add(domain);
              existingDomains.Add(domainName, domain);
            }
            _context.BoardGames_Domains.Add(new BoardGames_Domains()
            {
              BoardGame = boardgame,
              Domain = domain,
              CreatedDate = now
            });
          }
        if (!string.IsNullOrEmpty(record.Mechanics))
          foreach (var mechanicName in record.Mechanics
          .Split(',', StringSplitOptions.TrimEntries)
          .Distinct(StringComparer.InvariantCultureIgnoreCase))
          {
            var mechanic = existingMechanics.GetValueOrDefault(mechanicName);
            if (mechanic == null)
            {
              mechanic = new Mechanic()
              {
                Name = mechanicName,
                CreatedDate = now,
                LastModifiedDate = now
              };
              _context.Mechanics.Add(mechanic);
              existingMechanics.Add(mechanicName, mechanic);
            }
            _context.BoardGames_Mechanics.Add(new BoardGames_Mechanics()
            {
              BoardGame = boardgame,
              Mechanic = mechanic,
              CreatedDate = now
            });
          }
      }

      // SAVE
      using var transaction = _context.Database.BeginTransaction();
      //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT BoardGames ON");
      await _context.SaveChangesAsync();
      //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT BoardGames OFF");
      transaction.Commit();

      return new JsonResult(new
      {
        BoardGames = _context.BoardGames.Count(),
        Domains = _context.Domains.Count(),
        Mechanics = _context.Mechanics.Count(),
        SkippedRows = skippedRows
      });
    }
  }
}
