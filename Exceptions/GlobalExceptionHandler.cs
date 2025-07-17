using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace NoFlowEngine.Exceptions
{
    /// <summary>
    /// Provides extension methods to configure global exception handling for an ASP.NET Core application.
    /// </summary>
    public static class GlobalExceptionHandler
    {
        /// <summary>
        /// Configures the exception handler middleware to catch and handle unhandled exceptions.
        /// It logs the error and returns a standardized error response to the client.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="logger">The <see cref="ILogger"/> instance used for logging errors.</param>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");

                        await context.Response.WriteAsync(new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error. Please contact support."
                        }.ToString());
                    }
                });
            });
        }
    }
}