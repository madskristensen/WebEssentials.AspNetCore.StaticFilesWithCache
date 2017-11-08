using System;
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
            return app.UseStaticFilesWithCache(TimeSpan.FromDays(365));
        }

        /// <summary>
        /// Enables static files serving with the specified expiration.
        /// </summary>
        public static IApplicationBuilder UseStaticFilesWithCache(this IApplicationBuilder app, TimeSpan cacheExpiration)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
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
