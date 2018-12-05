using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using s = SPOT.Admin.Code.Constants.StringConstants;
using SPOT.Admin.Code;
using SPOT.Admin.Code.Extensions;

namespace SPOT.Admin
{
    public class Startup
    {
        private readonly List<Exception> _startupExceptions = new List<Exception>();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                // Adding Modules
                services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonFormatters();
                services.AddDistributedMemoryCache();
                services.AddMemoryCache();
                services.AddOptions();

                // Session
                var envConfigOptions = Configuration.GetSection(s.EnvironmentConfigSection).Get<EnvConfig>();
                services.AddSession(options =>
                {
                    options.IdleTimeout = TimeSpan.FromMinutes(
                            null != envConfigOptions?.SessionIdleTimeout && envConfigOptions.SessionIdleTimeout > 0
                                ? envConfigOptions.SessionIdleTimeout : 20);
                });

                // IoC
                services.AddAppWeb(Configuration);

                // CORS
                services.AddCors(options =>
                {
                    options.AddPolicy(s.AllowAnyOrigin,
                        builder => builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials());
                });

                // Optimization
                services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
                services.AddResponseCompression(options =>
                {
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
                        "image/gif",
                        "application/vnd.ms-fontobject",
                        "application/font-sfnt",
                        "application/font-woff",
                        "font/woff2"
                    };
                });
            }
            catch (Exception ex)
            {
                _startupExceptions.Add(ex);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            try
            {
                app.UseCors(s.AllowAnyOrigin);
                app.UseHttpsRedirection();
                app.UseMvc();
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = s.ClientApp;
                    if (env.IsDevelopment())
                    {
                        spa.UseAngularCliServer(s.NpmStartCommand);
                    }
                });
            }
            catch (Exception ex)
            {
                _startupExceptions.Add(ex);
            }
        }

        private void RenderStartupErrors(IApplicationBuilder app)
        {
            app.Run(
                async context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/plain";

                    foreach (var ex in _startupExceptions)
                    {
                        await context.Response.WriteAsync($"Error on startup {ex.Message}").ConfigureAwait(false);
                    }
                });
        }
    }
}
