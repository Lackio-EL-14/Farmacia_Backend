
using System.Text.Json;
using Farmacia.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Farmacia.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException aex)
            {
                _logger.LogError(aex, "ApiException captured");
                context.Response.StatusCode = aex.StatusCode;
                context.Response.ContentType = "application/json";
                var resp = new { message = aex.Message, errors = aex.Errors };
                await context.Response.WriteAsync(JsonSerializer.Serialize(resp));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var resp = new { message = "Error interno del servidor" };
                await context.Response.WriteAsync(JsonSerializer.Serialize(resp));
            }
        }
    }
}
