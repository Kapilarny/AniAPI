using AspNetCore.Proxy;
using Commons;
using Commons.Collections;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Middlewares;

namespace WebAPI
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
            // Add Cors
            services.AddCors(o =>
            {
                o.AddPolicy("CorsEveryone", builder =>
                {
                    builder.AllowAnyOrigin();
                });
                o.AddPolicy("CorsInternal", builder =>
                {
                    builder.WithOrigins(
                        "http://aniapi.com",
                        "https://aniapi.com")
                    .SetIsOriginAllowedToAllowWildcardSubdomains();
                });
            });

            services.AddMvc(o =>
            {
                o.UseGeneralRoutePrefix("api/v{version:apiVersion}");
            }).AddJsonOptions(options => { 
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(3);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication("Bearer")
                .AddScheme<AuthenticationSchemeOptions, JWTAuthenticationHandler>("Bearer", null);

            IConfiguration config = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).
                AddInMemoryCollection(new AppSettingsCollection().GetConfiguration()).
                Build();

            services.AddSingleton<IConfiguration>(config);

            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
                c.DocumentFilter<EnumDocumentFilter>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "WebAPI.xml");
                c.IncludeXmlComments(filePath);
            });

            services.AddHttpContextAccessor();

            services.AddSingleton<IRateLimitDependency, RateLimitDependency>();

            services.AddProxies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            app.UseLoggingMiddleware();
            app.UseRateLimitMiddleware();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            loggerFactory.AddFile("Logs/log-{Date}.txt");
        }
    }
}
