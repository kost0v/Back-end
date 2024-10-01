namespace Lab15.Middleware
{
    public class RoleCheckMiddleware
    {
        private readonly RequestDelegate _next;
        public RoleCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                if (!context.User.IsInRole("Admin") &&
    context.Request.Path.StartsWithSegments("/Admin"))
                {
                    context.Response.Redirect("/Account/AccessDenied");
                    return;
                }
            }
            await _next(context);
        }
    }
    public static class RoleCheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseRoleCheck(this IApplicationBuilder
       builder)
        {
            return builder.UseMiddleware<RoleCheckMiddleware>();
        }
    }
}