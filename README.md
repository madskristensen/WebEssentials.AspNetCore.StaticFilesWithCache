# ASP.NET Core Static Files with caching

[![Build status](https://ci.appveyor.com/api/projects/status/rqp3tneiy0bi1697?svg=true)](https://ci.appveyor.com/project/madskristensen/webessentials-aspnetcore-outputcaching)
[![NuGet](https://img.shields.io/nuget/v/WebEssentials.AspNetCore.StaticFilesWithCache.svg)](https://nuget.org/packages/WebEssentials.AspNetCore.StaticFilesWithCache/)

Middleware for ASP.NET Core that enables static files and adds client-side caching headers.

## Features
Here's what the middleware does:

- Customize the expiration time span
- Adds both `cache-control` and `expires` headers
- Adds `cache-control: immutable` for fingerprinted files

## Register the middleware
Where you normally registers to use the `app.UseStaticFiles()` middleware, instead replace it with this:

```c#
public void Configure(IApplicationBuilder app)
{
    app.UseStaticFilesWithCache();
}
```

To specify the cache expiration time, pass the method a `TimeSpan` like so:

```c#
public void Configure(IApplicationBuilder app)
{
    app.UseStaticFilesWithCache(TimeSpan.FromDays(30));
}
```

This will tell the browsers to cache the static file for 30 days.