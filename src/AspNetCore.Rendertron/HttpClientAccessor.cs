using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AspNetCore.Rendertron
{
    public class HttpClientAccessor : IHttpClientAccessor
    {
        private static HttpClient _httpClient;

        public HttpClientAccessor(HttpClientAccessorOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var handler = new HttpClientHandler();

            if (options.AcceptCompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            _httpClient = new HttpClient(handler);
        }

        public HttpClient HttpClient => _httpClient;
    }
}
