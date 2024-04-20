using BoardGames.DataAccess.Interfaces;
using BoardGames.DataAccess.Repository;
using BoardGames.DataContract.DatabContext;
using BoardGames.RestApi.Services;
using BoardGames.RestApi.Services.Interfaces;
using BoardGames.RestApi.Swagger;
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
      builder.Services.AddScoped<IRepository, BaseRepository>();
      builder.Services.AddScoped<ICsvReader, CsvReader>();
      builder.Services.AddScoped<IBoardGameRepository, BoardGameRepository>();
      builder.Services.AddScoped<IDomainRepository, DomainRepository>();
      builder.Services.AddScoped<IMechanicRepository, MechanicRepository>();
      builder.Services.AddScoped<IBoardGameService, BoardGameService>();
      builder.Services.AddScoped<ISeedDataService, SeedDataService>();
      builder.Services.AddScoped<IDomainService, DomainService>();
      builder.Services.AddScoped<IMechanicService, MechanicService>();

      builder.Services.AddDbContext<BoardGamesDbContext>(options =>
      {
        options.UseNpgsql(
          builder.Configuration.GetConnectionString("DbConnection")
          );
      }, ServiceLifetime.Scoped);
      builder.Services.AddSwaggerGen(options =>
      {
        options.ParameterFilter<SortColumnFilter>();
        options.ParameterFilter<SortOrderFilter>();
      });

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
