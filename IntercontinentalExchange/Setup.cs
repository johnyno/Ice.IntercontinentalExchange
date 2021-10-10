using System;
using System.Diagnostics;
using System.Net;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace IntercontinentalExchange.Host
{
    public abstract class Setup
    {
        internal static Action<IApplicationBuilder> ExceptionHandlerSetupAction => appError => appError.Run(GlobalExceptionHandler);
        private static RequestDelegate GlobalExceptionHandler =>
            async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    //logger.LogError($"Something went wrong: {contextFeature.Error}");
                    await context.Response.WriteAsync(new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error.",
                        Discription = contextFeature.Error.Message
                    }.ToString());
                }
            };

        internal static Action<SwaggerUIOptions> SwaggerUiSetupAction =>
            c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Intercontinental Exchange Ex");
            };

        internal static Action<JsonOptions> ControllersJsonSetupAction =>
            options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Default;
            };

        internal static Action<IEndpointRouteBuilder> EndPointSetupAction =>
            endpoints =>
            {
                endpoints.MapControllers();
            };


        internal static IApplicationBuilder Init(IApplicationBuilder app)
        {
            //any init you need

           

            return app;
        }
    }
}
