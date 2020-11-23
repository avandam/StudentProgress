﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using StudentProgress.Web.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace StudentProgress.Web
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
            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/");
            });

            services.AddDbContext<ProgressContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("ProgressContext")));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                // Store the session to cookies
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // OpenId authentication
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect(options =>
                {
                    // URL of the Keycloak server
                    options.Authority = Configuration.GetValue<string>("Authentication:Authority");
                    // Client configured in the Keycloak
                    options.ClientId = Configuration.GetValue<string>("Authentication:ClientId");

                    // For testing we disable https (should be true for production)
                    options.RequireHttpsMetadata = false;
                    options.SaveTokens = true;

                    // Client secret shared with Keycloak
                    options.ClientSecret = Configuration.GetValue<string>("Authentication:ClientSecret");
                    options.GetClaimsFromUserInfoEndpoint = true;

                    // OpenID flow to use
                    options.ResponseType = OpenIdConnectResponseType.IdToken;
                    options.CallbackPath = "/signin-oidc";

                    options.Events.OnRedirectToIdentityProvider = async n =>
                    {
                        n.ProtocolMessage.RedirectUri = n.ProtocolMessage.RedirectUri.Replace("http", "https");
                        n.ProtocolMessage.BuildRedirectUrl();
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePathBase("/student");
            app.Use((context, next) =>
            {
                context.Request.PathBase = "/student";
                return next();
            });

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
