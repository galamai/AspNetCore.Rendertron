using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly string _proxyUrl;

        public RendertronMiddleware(RequestDelegate next, RendertronMiddlewareOptions options)
        {
            _next = next;
            _options = options;
            _proxyUrl = options.ProxyUrl.EndsWith("/") ? options.ProxyUrl : options.ProxyUrl + "/";
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            var userAgent = request.Headers["User-agent"].ToString().ToLowerInvariant();

            if (!(userAgent == string.Empty && _options.UseForEmptyUserAgents) && !_options.UserAgents.Any(x => userAgent.Contains(x.ToLowerInvariant())))
            {
                await _next(context);
                return;
            }

            var incomingUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            var renderUrl = $"{_proxyUrl}{Uri.EscapeUriString(incomingUrl)}";
            if (_options.InjectShadyDom)
            {
                renderUrl += $"{(string.IsNullOrEmpty(request.QueryString.ToString()) ? "?" : "&")}wc-inject-shadydom=true";
            }

            using (var httpClient = new HttpClient())
            {
                using (var tokenSource = new CancellationTokenSource(_options.Timeout))
                {
                    try
                    {
                        var response = await httpClient.GetAsync(renderUrl, tokenSource.Token);
                        var result = await response.Content.ReadAsStringAsync();
                        await context.Response.WriteAsync(result);
                    }
                    catch (OperationCanceledException)
                    {
                        context.Response.StatusCode = 408;
                    }
                }
            }
        }
    }
}
