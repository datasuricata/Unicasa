using System.Web.Http;
using WebActivatorEx;
using Unicasa.API;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Web.Http.Description;
using System.Collections.Generic;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Unicasa.API
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            System.Reflection.Assembly thisAssembly = typeof(SwaggerConfig).Assembly;

            config.EnableSwagger(c =>
            {
                c.BasicAuth("basic").Description("Bearer Token Authentication");
                c.OperationFilter<AddRequiredHeaderParameter>();

                c.SingleApiVersion("v1", "Unicasa");
            })
            .EnableSwaggerUi(c =>
            {

            });

        }
    }

    public class AddRequiredHeaderParameter : IOperationFilter
    {

        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }

            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                type = "string",
                //required = true
            });
        }
    }
}
