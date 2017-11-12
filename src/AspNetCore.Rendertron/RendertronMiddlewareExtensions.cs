using AspNetCore.Rendertron;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class RendertronMiddlewareExtensions
    {
        public static IApplicationBuilder UseRendertron(this IApplicationBuilder builder, string proxyUrl)
        {
            return builder.UseRendertron(new RendertronMiddlewareOptions() { ProxyUrl = proxyUrl });
        }

        public static IApplicationBuilder UseRendertron(this IApplicationBuilder builder, RendertronMiddlewareOptions options)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            return builder.UseMiddleware<RendertronMiddleware>(options);
        }
    }
}
