using AspNetCore.Rendertron;

namespace Microsoft.AspNetCore.Builder
{
    public static class RendertronMiddlewareExtensions
    {
        public static IApplicationBuilder UseRendertron(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RendertronMiddleware>();
        }
    }
}
