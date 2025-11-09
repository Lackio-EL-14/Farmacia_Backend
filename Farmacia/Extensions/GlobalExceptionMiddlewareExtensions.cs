using Microsoft.AspNetCore.Builder;
using Farmacia.Api.Middlewares;

namespace Farmacia.Api.Extensions
{
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
