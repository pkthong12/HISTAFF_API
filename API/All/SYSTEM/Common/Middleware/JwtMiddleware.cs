using Microsoft.Extensions.Options;

namespace API.All.SYSTEM.Common.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
        {

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var user = jwtUtils.GetUserFromAccessToken(token ?? "");
            if (user != null)
            {
                context.Items["sid"] = user.ID;
            }

            await _next(context);
        }
    }
}
