using System.Web.Http;
using WebActivatorEx;
using webApiTask;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace webApiTask
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "webApiTask");

                        c.IncludeXmlComments(string.Format(@"{0}\bin\webApiTask.XML",
                        System.AppDomain.CurrentDomain.BaseDirectory));
                    })
                .EnableSwaggerUi();
        }
    }
}
