using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiDebts.Src.API
{
    public abstract class BaseOperationFilter : IOperationFilter
    {
        public abstract void Apply(OpenApiOperation operation, OperationFilterContext context);
    }
}