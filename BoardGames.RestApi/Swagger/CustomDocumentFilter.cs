using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BoardGames.RestApi.Swagger
{
  internal class CustomDocumentFilter : IDocumentFilter
  {
    public void Apply(
        OpenApiDocument swaggerDoc,
        DocumentFilterContext context)
    {
      swaggerDoc.Info.Title = "Board Game Web API";
    }
  }
}
