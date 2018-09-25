using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using PushnotificationsDemo.Models;
using PushnotificationsDemo.Services;
using System;
using System.Diagnostics;
using System.IO.Compression;

namespace PushnotificationsDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IPushService, PushService>();

            services.AddDbContextPool<DemoDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Database")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Add gzip compression
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                //options.EnableForHttps = true;
                options.MimeTypes = new[]
                {
                    // Default
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
 
                    // Custom
                    "image/svg+xml",
                    "application/font-woff2"
                };
            });

            services.Configure<HstsOptions>(options =>
            {
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DemoDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"An error occurred seeding the DB: {e}");
            }

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", new[] { "SAMEORIGIN" });
                context.Response.Headers.Add("Expect-CT", new[] { "expect-ct: max-age=604800, report-uri=https://example.com" });
                context.Response.Headers.Add("X-XSS-Protection", new[] { "1; mode=block; report=https://example.com" });
                context.Response.Headers.Add("X-Content-Type-Options", new[] { "nosniff" });
                context.Response.Headers.Add("Referrer-Policy", new[] { "strict-origin-when-cross-origin" });
                context.Response.Headers.Add("Feature-Policy", new[] { "accelerometer 'none'; camera 'none'; geolocation 'self'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'" });
                context.Response.Headers.Add("Content-Security-Policy", new[] { "default-src 'self'; script-src 'self'; style-src 'self' *.msecnd.net; img-src 'self' data:; connect-src https: wss: 'self'; font-src 'self' c.s-microsoft.com; frame-src 'self'; form-action 'self'; upgrade-insecure-requests; report-uri https://example.com" });
                context.Response.Headers.Remove(HeaderNames.Server);
                context.Response.Headers.Remove("X-Powered-By");
                await next();
            });

            app.UseResponseCompression();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int cacheExpirationInSeconds = 60 * 60 * 24 * 30; //one month
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + cacheExpirationInSeconds;
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
