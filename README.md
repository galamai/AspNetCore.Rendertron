# AspNetCore.Rendertron
ASP.net core middleware for GoogleChrome Rendertron [https://github.com/GoogleChrome/rendertron](https://github.com/GoogleChrome/rendertron).

## NuGet
`Install-Package Galamai.AspNetCore.Rendertron`

## Use
In `Startup.cs`

Configure rendertron services

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
    }
	
or

	public void ConfigureServices(IServiceCollection services)
    {
        // Add rendertron services
		services.AddRendertron(rendertronUrl: "http://rendertron:3000/render/");
    }

    
Configure middleware

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        ...
        // Use Rendertron middleware
        app.UseRendertron();
        ...
        app.UseMvc();
    }

## Services configuration

| Property | Default | Description |
| -------- | ------- | ----------- |
| `AcceptCompression` | `false` | Add http compression suppor for Rendertron proxy client. |
| `RendertronUrl` | *Required* | URL of your running Rendertron service. |
| `UserAgents` | A set of known bots that benefit from pre-rendering. ("W3C_Validator", "baiduspider", "bingbot", "embedly", "facebookexternalhit", "linkedinbo", "outbrain", "pinterest", "quora link preview", "rogerbo", "showyoubot", "slackbot", "twitterbot", "vkShare") | Part of requests by User-Agent header. |
| `InjectShadyDom` | `false` | Force the web components polyfills to be loaded. [Read more.](https://github.com/GoogleChrome/rendertron#web-components) |
| `Timeout` | `TimeSpan.FromSeconds(10)` | Millisecond timeout for the proxy request to Rendertron. If exceeded, the standard response is served (i.e. `next()` is called). See also the [Rendertron timeout.](https://github.com/GoogleChrome/rendertron#rendering-budget-timeout) |
| `AppProxyUrl` | `null` | Use proxy before application. |
| `HttpCacheMaxAge` | `TimeSpan.Zero` | Set responce cache max age. |
