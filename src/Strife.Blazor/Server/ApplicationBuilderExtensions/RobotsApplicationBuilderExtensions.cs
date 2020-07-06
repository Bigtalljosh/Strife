using Microsoft.AspNetCore.Builder;
using System;

namespace Strife.Blazor.Server.ApplicationBuilderExtensions
{
    internal static class RobotsApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddRobots(this IApplicationBuilder app)
        {
            app.UseRobotsTxt(builder =>
            builder
                .AddSection(section =>
                    section
                        .AddComment("Allow Googlebot")
                        .AddUserAgent("Googlebot")
                        .Allow("/")
                    )
                .AddSection(section =>
                    section
                        .AddComment("Disallow the rest")
                        .AddUserAgent("*")
                        .AddCrawlDelay(TimeSpan.FromSeconds(10))
                        .Disallow("/")
                    )
                .AddSitemap("https://example.com/sitemap.xml") //TODO Replace me
            );

            return app;
        }
    }
}
