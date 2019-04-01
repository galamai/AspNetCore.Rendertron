using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Rendertron
{
    public class RendertronClient : IRendertronClient
    {
        private readonly HttpClient _httpClient;
        private readonly IOptionsMonitor<RendertronOptions> _optionsMonitor;

        public RendertronClient(HttpClient httpClient, IOptionsMonitor<RendertronOptions> optionsMonitor)
        {
            _httpClient = httpClient;
            _optionsMonitor = optionsMonitor;
        }

        public async Task<RendertronResponse> RenderAsync(string url, CancellationToken cancellationToken)
        {
            var options = _optionsMonitor.CurrentValue;
            var fromAppProxy = !string.IsNullOrEmpty(options.AppProxyUrl);

            if (fromAppProxy)
            {
                var realUri = new Uri(url);
                var uriBuilder = new UriBuilder(options.AppProxyUrl);
                uriBuilder.Path = realUri.AbsolutePath;
                uriBuilder.Query = realUri.Query;
                url = uriBuilder.Uri.ToString();
            }

            var rendertronUrl = options.RendertronUrl.EndsWith("/") ? options.RendertronUrl : options.RendertronUrl + "/";
            var renderUrl = $"{rendertronUrl}{Uri.EscapeUriString(url)}";

            if (options.InjectShadyDom)
            {
                renderUrl += $"{(url.Contains("?") ? "&" : "?")}wc-inject-shadydom=true";
            }

            using (var response = await _httpClient.GetAsync(renderUrl, cancellationToken).ConfigureAwait(false))
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.IsSuccessStatusCode && fromAppProxy)
                {
                    content = content.Replace("<base href=", ">", "<base href=\"/\">");
                }

                if (string.IsNullOrEmpty(content))
                {
                    content = response.ReasonPhrase;
                }

                return new RendertronResponse(content, response.StatusCode);
            }
        }
    }
}
