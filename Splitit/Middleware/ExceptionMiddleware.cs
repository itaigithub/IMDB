using System.Net;

namespace Splitit.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "handled in middleware");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = new
                {
                    error = ex.Message,
                };
                await context.Response.WriteAsync(ex.Message);
            }
        }

    }

    public static class ExceptionMiddlewareExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}

