using System.Net;

namespace AspNetCore.Rendertron
{
    public sealed class RendertronResponse
    {
        public string Result { get; }
        public HttpStatusCode StatusCode { get; }

        public RendertronResponse(string result, HttpStatusCode statusCode)
        {
            Result = result;
            StatusCode = statusCode;
        }
    }
}
