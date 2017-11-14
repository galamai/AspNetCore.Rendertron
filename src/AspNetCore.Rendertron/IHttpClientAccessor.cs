using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AspNetCore.Rendertron
{
    public interface IHttpClientAccessor
    {
        HttpClient HttpClient { get; }
    }
}
