using System;

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods for the <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtension
    {
        /// <summary>
        /// Enables static files serving with the default cache expiration of 1 year.
        /// </summary>
        public static IApplicationBuilder UseStaticFilesWithCache(this IApplicationBuilder app)
        {
            return app.UseStaticFilesWithCache(TimeSpan.FromDays(365), new FileExtensionContentTypeProvider());
        }

        /// <summary>
        /// Enables static files with custom content type provider, serving with the default cache expiration of 1 year.
        /// </summary>
        public static IApplicationBuilder UseStaticFilesWithCache(this IApplicationBuilder app, FileExtensionContentTypeProvider provider)
        {
            return app.UseStaticFilesWithCache(TimeSpan.FromDays(365), new FileExtensionContentTypeProvider());
        }

        /// <summary>
        /// Enables static files serving with the specified expiration.
        /// </summary>
        public static IApplicationBuilder UseStaticFilesWithCache(this IApplicationBuilder app, TimeSpan cacheExpiration, FileExtensionContentTypeProvider provider)
        {
            provider.Mappings[".svg"] = "image/svg+xml; charset=utf-8";
            provider.Mappings[".vsix"] = "application/octed-stream";
            provider.Mappings[".webmanifest"] = "application/manifest+json; charset=utf-8";

            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = provider,
                OnPrepareResponse = (context) =>
                {
                    context.Context.Response.Headers[HeaderNames.CacheControl] = $"max-age={cacheExpiration.TotalSeconds.ToString()}";
                    context.Context.Response.Headers[HeaderNames.Expires] = DateTime.UtcNow.Add(cacheExpiration).ToString("R");

                    if (context.Context.Request.Query.ContainsKey("v"))
                    {
                        context.Context.Response.Headers[HeaderNames.CacheControl] += ",immutable";
                    }
                }
            });

            return app;
        }
    }
}
