using System;
using System.Collections.Generic;

namespace AspNetCore.Rendertron
{
    public class RendertronOptions
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
            "vkShare"
        };

        public string RendertronUrl { get; set; }
        public string AppProxyUrl { get; set; }
        public List<string> UserAgents { get; set; } = new List<string>(BotUserAgents);
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);
        public bool InjectShadyDom { get; set; }
        public TimeSpan HttpCacheMaxAge { get; set; } = TimeSpan.Zero;
        public bool AcceptCompression { get; set; }
    }
}
