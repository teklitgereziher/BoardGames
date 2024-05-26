using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BoardGames.RestApi.Swagger
{
  /// <summary>
  /// Applies the padlock icon only to the actions with an [Authorize] attribute.
  /// </summary>
  internal class AuthRequirementFilter : IOperationFilter
  {
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      if (!context.ApiDescription
          .ActionDescriptor
          .EndpointMetadata
          .OfType<AuthorizeAttribute>()
          .Any())
      {
        return;
      }

      operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            };
    }
  }
}
