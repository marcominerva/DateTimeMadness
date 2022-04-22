using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ClientContext;

internal class ClientContextOperationFilter : IOperationFilter
{
    private readonly ClientContextOptions options;

    public ClientContextOperationFilter(ClientContextOptions options)
    {
        this.options = options;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = options.TimeZoneHeader,
            In = ParameterLocation.Header,
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string",
            }
        });
    }
}
