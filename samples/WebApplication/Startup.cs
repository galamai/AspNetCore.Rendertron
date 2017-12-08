using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            app.UseRendertron(proxyUrl: "http://localhost/render/");

            // Redirect all requests to the index.html
            var options = new RewriteOptions();
            options.AddRewrite("(.*)", "/index.html", true);
            app.UseRewriter(options);
            app.UseStaticFiles();
        }
    }
}
