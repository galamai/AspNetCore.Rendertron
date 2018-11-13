using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Rendertron
{
    public class RendertronMiddlewareOptions
    {
        static string[] BotUserAgents = new string[]
        {
            "W3C_Validator",
            "baiduspider",
            "bingbot",
            "embedly",
            "facebookexternalhit",
            "linkedinbo",
            "outbrain",
            "pinterest",
            "quora link preview",
            "rogerbo",
            "showyoubot",
            "slackbot",
            "twitterbot",
            "vkShare",
            "googlebot"
        };

        public string ProxyUrl { get; set; }
        public List<string> UserAgents { get; set; } = new List<string>(BotUserAgents);
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(11);
        public bool InjectShadyDom { get; set; }
        public TimeSpan HttpCacheMaxAge { get; set; } = TimeSpan.Zero;
    }
}
