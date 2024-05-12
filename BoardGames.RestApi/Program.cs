using BoardGames.DataAccess.Interfaces;
using BoardGames.DataAccess.Repository;
using BoardGames.DataContract.DatabContext;
using BoardGames.DataContract.Models;
using BoardGames.RestApi.GraphQL;
using BoardGames.RestApi.Services;
using BoardGames.RestApi.Services.Interfaces;
using BoardGames.RestApi.Swagger;
using BoardGames.Shared.Interfaces;
using BoardGames.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

      builder.Services.AddGraphQLServer()
        .AddAuthorization()
        .AddQueryType<Query>()
        .AddMutationType<Mutation>()
        .AddProjections()
        .AddFiltering()
        .AddSorting();

      // Identity service — to perform the registration and login processes
      // 1. Adds the Identity service
      // 2. Configures password strength requirements
      builder.Services.AddIdentity<BoardGameUser, IdentityRole>(options =>
      {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 12;
      }).AddEntityFrameworkStores<BoardGamesDbContext>();

      // authentication service — to define the rules for issuing and reading JWTs
      // 1. Adds the Authentication service
      // 2. Sets the default authorization - related schemes
      // 3. Adds the JWT Bearer authentication scheme
      // 4. Configures JWT options and settings
      builder.Services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme =
        options.DefaultChallengeScheme =
        options.DefaultForbidScheme =
        options.DefaultScheme =
        options.DefaultSignInScheme =
        options.DefaultSignOutScheme =
            JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateIssuerSigningKey = true,
          RequireExpirationTime = true,
          ValidIssuer = builder.Configuration["JWT:Issuer"],
          ValidAudience = builder.Configuration["JWT:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(
              System.Text.Encoding.UTF8.GetBytes(
                  builder.Configuration["JWT:SigningKey"])
          )
        };
      });

      builder.Services.AddSwaggerGen(options =>
      {
        options.ParameterFilter<SortColumnFilter>();
        options.ParameterFilter<SortOrderFilter>();

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          In = ParameterLocation.Header,
          Description = "Please enter token",
          Name = "Authorization",
          Type = SecuritySchemeType.Http,
          BearerFormat = "JWT",
          Scheme = "bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
          }
        });
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

      app.MapGraphQL("/graphql");

      app.MapControllers();

      app.Run();
    }
  }
}
