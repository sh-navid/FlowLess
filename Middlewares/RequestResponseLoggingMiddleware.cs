using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using System;

namespace NoFlowEngine.Middlewares
{
    /// <summary>
    /// Middleware for logging HTTP requests and responses.
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestResponseLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="next"/> or <paramref name="loggerFactory"/> is null.</exception>
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<RequestResponseLoggingMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Invokes the middleware to log the request and response.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            // Enable buffering to read the request body multiple times
            context.Request.EnableBuffering();

            // Read the request body
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

            // Rewind the request body stream for further processing
            context.Request.Body.Position = 0;

            // Log the request information
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} {context.Request.QueryString} Body: {requestBody}");

            // Store the original response body stream
            var originalBodyStream = context.Response.Body;

            // Create a new memory stream to hold the response body
            using (var responseBody = new MemoryStream())
            {
                // Replace the response body with the memory stream
                context.Response.Body = responseBody;

                // Invoke the next middleware in the pipeline
                await _next(context);

                // Rewind the response body stream to the beginning
                responseBody.Seek(0, SeekOrigin.Begin);

                // Read the response body
                var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();

                // Rewind the response body stream again for copying
                responseBody.Seek(0, SeekOrigin.Begin);

                // Log the response information
                _logger.LogInformation($"Response: {context.Response.StatusCode}: {responseBodyText}");

                // Copy the response body to the original stream
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }

    /// <summary>
    /// Extension methods for <see cref="RequestResponseLoggingMiddleware"/>.
    /// </summary>
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        /// <summary>
        /// Adds the <see cref="RequestResponseLoggingMiddleware"/> to the <see cref="IApplicationBuilder"/> request pipeline.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}