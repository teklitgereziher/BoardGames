using BoardGames.DataAccess.Interfaces;
using BoardGames.DataAccess.Repository;
using BoardGames.DataContract.DatabContext;
using BoardGames.RestApi.Services;
using BoardGames.Shared.Interfaces;
using BoardGames.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace MyBGList
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddControllers();
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();
      builder.Services.AddScoped<IRepository, BaseRepository>();
      builder.Services.AddScoped<ICsvReader, CsvReader>();
      builder.Services.AddScoped<IBoardGameRepository, BoardGameRepository>();
      builder.Services.AddScoped<IDomainsRepository, DomainsRepository>();
      builder.Services.AddScoped<IMechanicsRepository, MechanicsRepository>();
      builder.Services.AddScoped<IBoardGameService, BoardGameService>();
      builder.Services.AddScoped<ISeedDataService, SeedDataService>();

      builder.Services.AddDbContext<BoardGamesDbContext>(options =>
      {
        options.UseNpgsql(
          builder.Configuration.GetConnectionString("DbConnection")
          );
      }, ServiceLifetime.Scoped);

      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();

      app.UseAuthorization();

      app.MapControllers();

      app.Run();
    }
  }
}
