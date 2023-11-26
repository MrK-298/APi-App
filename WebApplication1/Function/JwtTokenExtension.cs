namespace WebApplication1.Function
{
    public static class JwtTokenExtension
    {
        public static IApplicationBuilder UseJwtTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtToken>();
        }
    }
}
