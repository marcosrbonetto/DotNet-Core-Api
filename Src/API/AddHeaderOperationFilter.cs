using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiDebts.Src.API
{
    public class AddHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            var headers = SwaggerHeaders.Instance.Headers;

            var filterDescriptions = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var attributes = filterDescriptions
                    .Where(x => x.Filter is ServiceFilterAttribute)
                    .ToList();

            var b = attributes.Where(x => ((ServiceFilterAttribute)x.Filter).ServiceType.IsSubclassOf(typeof(BaseAttribute))).ToList();
            b.ForEach((item) =>
            {

                var itemFilter = ((ServiceFilterAttribute)item.Filter);
                BaseAttribute instance = (BaseAttribute)Activator.CreateInstance(itemFilter.ServiceType, new object[] { null});
                var headers = instance.GetSwaggerHeaders();
                headers.ForEach((attribute) =>
                {
                    if (!operation.Parameters.Any(x => x.Name == attribute.Name))
                    {
                        operation.Parameters.Add(new OpenApiParameter
                        {
                            Name = attribute.Name,
                            In = ParameterLocation.Header,
                            Description = attribute.Description,
                            Required = attribute.IsRequired,
                            Schema = string.IsNullOrEmpty(attribute.DefaultValue)
                                ? null
                                : new OpenApiSchema
                                {
                                    Type = "String",
                                    Default = new OpenApiString(attribute.DefaultValue)
                                }
                        });
                    }
                });

            });
        }
    }
}