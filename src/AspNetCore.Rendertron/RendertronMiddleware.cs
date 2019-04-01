using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Rendertron
{
    public class RendertronMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRendertronClient _rendertronClient;
        private readonly IOptionsMonitor<RendertronOptions> _optionsAccessor;

        public RendertronMiddleware(
            RequestDelegate next,
            IRendertronClient rendertronClient,
            IOptionsMonitor<RendertronOptions> optionsAccessor)
        {
            _next = next;
            _rendertronClient = rendertronClient;
            _optionsAccessor = optionsAccessor;
        }

        public Task Invoke(HttpContext context)
        {
            var options = _optionsAccessor.CurrentValue;

            if (IsNeedRender(context, options))
            {
                return InvokeRender(context, options);
            }
            else
            {
                return _next(context);
            }
        }

        private bool IsNeedRender(HttpContext context, RendertronOptions options)
        {
            var userAgent = context.Request.Headers["User-agent"].ToString().ToLowerInvariant();

            return options.UserAgents.Any(x => userAgent.Contains(x.ToLowerInvariant()));
        }

        private async Task InvokeRender(HttpContext context, RendertronOptions options)
        {
            var cancellationToken = context.RequestAborted;

            var response = await _rendertronClient
                .RenderAsync(context.Request.GetDisplayUrl(), cancellationToken)
                .ConfigureAwait(false);

            AddHttpCacheHeaders(context.Response, options);
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsync(response.Result, cancellationToken);
        }

        private void AddHttpCacheHeaders(HttpResponse httpResponse, RendertronOptions options)
        {
            if (options.HttpCacheMaxAge > TimeSpan.Zero)
            {
                httpResponse.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = options.HttpCacheMaxAge
                };
                httpResponse.Headers[HeaderNames.Vary] = new string[] { "Accept-Encoding" };
            }
        }
    }
}
