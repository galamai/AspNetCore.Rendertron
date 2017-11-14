using AspNetCore.Rendertron;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RendertronServiceCollectionExtensions
    {
        public static IServiceCollection AddRendertron(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddRendertron(null);
            return serviceCollection;
        }

        public static IServiceCollection AddRendertron(this IServiceCollection serviceCollection, Action<HttpClientAccessorOptions> setupAction)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            var options = new HttpClientAccessorOptions();
            setupAction?.Invoke(options);

            serviceCollection.AddSingleton<IHttpClientAccessor>(new HttpClientAccessor(options));

            return serviceCollection;
        }
    }
}
