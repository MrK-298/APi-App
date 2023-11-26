using System.Net;

namespace WebApplication1.Function
{
    public class JwtToken
    {
        private readonly RequestDelegate _next;

        public JwtToken(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Kiểm tra xem token có tồn tại trong danh sách "đen" không
            if (Blacklist.Contains(token))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            await _next(context);
        }
    }
}
