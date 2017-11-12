# AspNetCore.Rendertron
ASP.net core middleware for Google Rendertron.

# NuGet
`Install-Package Galamai.AspNetCore.Rendertron`

# Use
In `Startup.cs`

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        ...
        app.UseRendertron(proxyUrl: "http://rendertron:8080/render/");
        ...
        app.UseMvc();
    }
    
 or
 
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        ...
        app.UseRendertron(new RendertronMiddlewareOptions()
        {
            ProxyUrl = "http://rendertron:8080/render/",
            InjectShadyDom = true
        });
        ...
        app.UseMvc();
    }
