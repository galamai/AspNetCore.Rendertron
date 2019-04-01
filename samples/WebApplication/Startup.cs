using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Rewrite;

namespace WebApplication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add rendertron services
            services.AddRendertron(options =>
            {
                // rendertron service url
                options.RendertronUrl = "http://rendertron:3000/render/";

                // proxy url for application
                options.AppProxyUrl = "http://webapplication";

                // prerender for firefox
                //options.UserAgents.Add("firefox");

                // inject shady dom
                options.InjectShadyDom = true;
                
                // use http compression
                options.AcceptCompression = true;
            });

            // Add response caching
            services.AddResponseCaching();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Response cache middleware
            app.UseResponseCaching();

            app.UseStaticFiles();

            // Use Rendertron middleware
            app.UseRendertron();

            // Redirect all requests to the index.html
            var options = new RewriteOptions();
            options.AddRewrite("(.*)", "/index.html", true);
            app.UseRewriter(options);
            app.UseStaticFiles();
        }
    }
}
