using AspNetCore.Rendertron;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RendertronServiceCollectionExtensions
    {
        public static IServiceCollection AddRendertron(
            this IServiceCollection serviceCollection,
            string rendertronUrl)
        {

            return serviceCollection.AddRendertron(x =>
            {
                x.RendertronUrl = rendertronUrl;
            });
        }

        public static IServiceCollection AddRendertron(
            this IServiceCollection serviceCollection,
            Action<RendertronOptions> configureOptions)
        {
            serviceCollection.Configure(configureOptions);

            serviceCollection
                .AddHttpClient<IRendertronClient, RendertronClient>((provider, client) =>
                {
                    var options = provider.GetRequiredService<IOptionsMonitor<RendertronOptions>>().CurrentValue;
                    client.Timeout = options.Timeout;
                })
                .ConfigurePrimaryHttpMessageHandler(provider =>
                {
                    var options = provider.GetRequiredService<IOptionsMonitor<RendertronOptions>>().CurrentValue;
                    var handler = new HttpClientHandler();

                    if (options.AcceptCompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    }

                    return handler;
                });

            return serviceCollection;
        }
    }
}
