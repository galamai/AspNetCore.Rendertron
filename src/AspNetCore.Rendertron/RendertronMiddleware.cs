using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Rendertron
{
    public class RendertronMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RendertronMiddlewareOptions _options;
        private readonly IHttpClientAccessor _httpClientAccessor;
        private readonly string _proxyUrl;
        private readonly CacheControlHeaderValue _cacheControlHeaderValue;
        private readonly string[] _varyHeaders;

        public RendertronMiddleware(
            RequestDelegate next,
            RendertronMiddlewareOptions options,
            IHttpClientAccessor httpClientAccessor)
        {
            _next = next;
            _options = options;
            _httpClientAccessor = httpClientAccessor;

            _proxyUrl = options.ProxyUrl.EndsWith("/") ? options.ProxyUrl : options.ProxyUrl + "/";
            _cacheControlHeaderValue = new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = _options.HttpCacheMaxAge
            };
            _varyHeaders = new string[] { "Accept-Encoding" };
        }

        public Task Invoke(HttpContext context)
        {
            var cancellationToken = context.RequestAborted;
            cancellationToken.ThrowIfCancellationRequested();

            var userAgent = context.Request.Headers["User-agent"].ToString().ToLowerInvariant();

            if (IsNeedRender(userAgent, cancellationToken))
            {
                return InvokeRender(context, cancellationToken);
            }
            else
            {
                return _next(context);
            }
        }

        private bool IsNeedRender(string userAgent, CancellationToken cancellationToken)
        {
            return _options.UserAgents.Any(x =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return userAgent.Contains(x.ToLowerInvariant());
            });
        }

        private async Task InvokeRender(HttpContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var incomingUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";

            var (html, statusCode) = await RenderAsync(incomingUrl, cancellationToken);
            AddHttpCacheHeaders(context.Response);
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(html, cancellationToken);
        }

        private async Task<(string html, int statusCode)> RenderAsync(string incomingUrl, CancellationToken cancellationToken)
        {
            var renderUrl = $"{_proxyUrl}{Uri.EscapeUriString(incomingUrl)}";
            if (_options.InjectShadyDom)
            {
                renderUrl += $"{(incomingUrl.Contains('?') ? "&" : "?")}wc-inject-shadydom=true";
            }

            using (var tokenSource = new CancellationTokenSource(_options.Timeout))
            {
                cancellationToken.Register(() => tokenSource.Cancel());

                var response = await _httpClientAccessor.HttpClient.GetAsync(renderUrl, tokenSource.Token);

                if (response.IsSuccessStatusCode)
                {
                    var html = await response.Content.ReadAsStringAsync();
                    return (html, 200);
                }
                else
                {
                    return (response.ReasonPhrase, (int)response.StatusCode);
                }
            }
        }

        private void AddHttpCacheHeaders(HttpResponse httpResponse)
        {
            if (_options.HttpCacheMaxAge > TimeSpan.Zero)
            {
                httpResponse.GetTypedHeaders().CacheControl = _cacheControlHeaderValue;
                httpResponse.Headers[HeaderNames.Vary] = _varyHeaders;
            }
        }
    }
}
