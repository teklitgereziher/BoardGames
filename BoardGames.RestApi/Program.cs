using BoardGames.DataAccess.Interfaces;
using BoardGames.DataAccess.Repository;
using BoardGames.DataContract.DatabContext;
using BoardGames.RestApi.Services;
using BoardGames.RestApi.Services.Interfaces;
using BoardGames.RestApi.Swagger;
using BoardGames.Shared.Interfaces;
using BoardGames.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MyBGList
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      builder.Host.UseSerilog((context, services, configuration) =>
      configuration.ReadFrom.Configuration(context.Configuration)
      );

      // Log to Azure Application Insights
      //This configuration will activate the
      //Application Insights logging provider
      //builder.Logging
      //  .AddApplicationInsights(
      //  telemetry => telemetry.ConnectionString =
      //  builder.Configuration["Azure:ApplicationInsights:ConnectionString"],
      //  loggerOptions => { }
      //  );

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

      builder.Services.AddApplicationInsightsTelemetry();

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
      // Code replaced by the [ManualValidationFilter] attribute
      // to apply only for a specific action method
      // If the automatic model validation status check is disabled in the pipeline,
      // we can manually check the model validation status.
      // builder.Services.Configure<ApiBehaviorOptions>(options =>
      //    options.SuppressModelStateInvalidFilter = true
      //    );

      var app = builder.Build();

      //Serilog middleware To automatically log HTTP requests
      app.UseSerilogRequestLogging();

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
