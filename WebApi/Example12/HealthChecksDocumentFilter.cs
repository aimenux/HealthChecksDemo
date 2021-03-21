using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Example12
{
    public class HealthChecksDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var openApiResponses = new OpenApiResponses
            {
                ["200"] = new()
                {
                    Description = "Success",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["text/plain"] = new(),
                        ["application/json"] = new(),
                        ["text/json"] = new()
                    }
                }
            };
            var openApiOperation = new OpenApiOperation
            {
                Responses = openApiResponses,
                Tags = new List<OpenApiTag>
                {
                    new()
                    {
                        Name = "HealthChecks"
                    }
                }
            };
            var openApiPathItem = new OpenApiPathItem();
            openApiPathItem.AddOperation(OperationType.Get, openApiOperation);
            swaggerDoc.Paths.Add(HealthChecksSettings.HealthCheckLiveEndpoint, openApiPathItem);
            swaggerDoc.Paths.Add(HealthChecksSettings.HealthCheckReadyEndpoint, openApiPathItem);
        }
    }
}
