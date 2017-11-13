# AspNetCore.Rendertron
ASP.net core middleware for GoogleChrome Rendertron [https://github.com/GoogleChrome/rendertron](https://github.com/GoogleChrome/rendertron).

## NuGet
`Install-Package Galamai.AspNetCore.Rendertron`

## Use
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

## Configuration

`RendertronMiddlewareOptions`

| Property | Default | Description |
| -------- | ------- | ----------- |
| `ProxyUrl` | *Required* | URL of your running Rendertron proxy service. |
| `UserAgents` | A set of known bots that benefit from pre-rendering. ("W3C_Validator", "baiduspider", "bingbot", "embedly", "facebookexternalhit", "linkedinbo", "outbrain", "pinterest", "quora link preview", "rogerbo", "showyoubot", "slackbot", "twitterbot", "vkShare") | Part of requests by User-Agent header. |
| `UseForEmptyUserAgents` | `false` | Pre-rendering for empty User-Agent |
| `InjectShadyDom` | `false` | Force the web components polyfills to be loaded. [Read more.](https://github.com/GoogleChrome/rendertron#web-components) |
| `Timeout` | `TimeSpan.FromSeconds(11)` | Millisecond timeout for the proxy request to Rendertron. If exceeded, the standard response is served (i.e. `next()` is called). See also the [Rendertron timeout.](https://github.com/GoogleChrome/rendertron#rendering-budget-timeout) |