using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Rendertron
{
    public interface IRendertronClient
    {
        Task<RendertronResponse> RenderAsync(string url, CancellationToken cancellationToken);
    }
}
