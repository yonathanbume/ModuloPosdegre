using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Filters;
using AKDEMIC.CORE.Handlers;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Overrides;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.CORE.Validators;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Model;
using AKDEMIC.INTRANET.Profiles;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Factories;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace AKDEMIC.INTRANET
{
    public class Startup
    {
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Database

            services.AddDbContext<AkdemicContext>(options =>
            {
                /*if (CurrentEnvironment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                }*/

                switch (ConstantHelpers.GENERAL.DATABASES.DATABASE)
                {
                    case ConstantHelpers.DATABASES.MYSQL:
                        options.UseMySql(Configuration.GetConnectionString("MySqlDefaultConnection"), new MySqlServerVersion(ConstantHelpers.DATABASES.VERSIONS.MYSQL.VALUES[ConstantHelpers.DATABASES.VERSIONS.MYSQL.V8021]), mySqlOptions => mySqlOptions.EnableRetryOnFailure());
                        //options.UseSqlServer(Configuration.GetConnectionString("MySqlDefaultConnection"), sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());

                        break;
                    case ConstantHelpers.DATABASES.SQL:
                        //options.UseSqlServer(Configuration.GetConnectionString("MySqlDefaultConnection"), sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
                        options.UseSqlServer(Configuration.GetConnectionString("SqlDefaultConnection"), sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
                        break;
                }
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(setup =>
            {
                setup.Password.RequireDigit = false;
                setup.Password.RequiredLength = 5;
                setup.Password.RequireLowercase = false;
                setup.Password.RequireNonAlphanumeric = false;
                setup.Password.RequireUppercase = false;
            })
                .AddPasswordValidator<CustomPasswordValidator<ApplicationUser>>()
                .AddEntityFrameworkStores<AkdemicContext>()
                .AddDefaultTokenProviders();

            #endregion

            #region Logging

            //if (CurrentEnvironment.IsDevelopment())
            //{
            IdentityModelEventSource.ShowPII = true;
            //}

            #endregion

            //services.AddCertificateForwarding(options =>
            //{
            //    // header name might be different, based on your nginx config
            //    options.CertificateHeader = "X-SSL-CERT";s

            //    options.HeaderConverter = (headerValue) =>
            //    {
            //        X509Certificate2 clientCertificate = null;

            //        if (!string.IsNullOrWhiteSpace(headerValue))
            //        {
            //            var bytes = Encoding.UTF8.GetBytes(Uri.UnescapeDataString(headerValue));
            //            clientCertificate = new X509Certificate2(bytes);
            //        }

            //        return clientCertificate;
            //    };
            //});

            var auth = services.AddAuthentication(options =>
            {
                if (ConstantHelpers.GENERAL.Authentication.SSO_ENABLED)
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = /*OpenIdConnectDefaults.AuthenticationScheme*/"oidc";
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                }
            });

            #region OpenIdConnect

            if (ConstantHelpers.GENERAL.Authentication.SSO_ENABLED)
            {
                services.AddHttpClient<CookieEventHandler>();
                //services.AddSingleton<LogoutSessionManager>();
                //services.AddSingleton<IdentityModel.Client.IDiscoveryCache>();
                services.AddSingleton<IDiscoveryCache>(r =>
                {
                    var factory = r.GetRequiredService<IHttpClientFactory>();
                    return new DiscoveryCache(GeneralHelpers.GetAuthority(CurrentEnvironment.IsDevelopment()), () => factory.CreateClient());
                });

                auth.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/acceso-denegado";
                })
                .AddOpenIdConnect(/*OpenIdConnectDefaults.AuthenticationScheme*/"oidc", options =>
                {
                    options.Authority = GeneralHelpers.GetAuthority(CurrentEnvironment.IsDevelopment());

                    options.RequireHttpsMetadata = false;
                    options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;

                    options.ClientId = "intranet";
                    options.ClientSecret = "secret";
                    options.ResponseType = OpenIdConnectResponseType.Code;

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("roles");

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.Name,
                        RoleClaimType = ClaimTypes.Role,
                    };

                    options.RemoteAuthenticationTimeout = TimeSpan.FromHours(1);
                    options.Events.OnRemoteFailure = RemoteAuthFail;
                });

                services.AddAccessTokenManagement();


            }

            #endregion

            if (ConstantHelpers.GENERAL.ExternalAuthentication.GOOGLE)
            {
                var auths = Configuration.GetSection("GoogleCredentials").GetSection("AuthConfigs").Get<List<GoogleCredential>>();
                var credential = auths.Where(x => x.Institution == GeneralHelpers.GetInstitutionAbbreviation().ToLower()).FirstOrDefault();

                if (credential != null)
                {
                    auth
                        .AddGoogle(options =>
                        {
                            options.ClientId = credential.ClientId;
                            options.ClientSecret = credential.ClientSecret;
                        });
                }

            }

            if (ConstantHelpers.GENERAL.ExternalAuthentication.MICROSOFT)
            {
                auth
                    .AddMicrosoftAccount(microsoftOptions =>
                    {
                        IConfigurationSection AuthSection =
                             Configuration.GetSection("Authentication:Microsoft");

                        microsoftOptions.ClientId = AuthSection["ClientId"];
                        microsoftOptions.ClientSecret = AuthSection["ClientSecret"];
                    });

            }

            #region Other

            services.Configure<CloudStorageCredentials>(Configuration.GetSection("AzureStorageCredentials"));
            services.Configure<FormOptions>(x => x.ValueCountLimit = 20480);

            #endregion

            #region Mapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region Repositories / Services          
            services.AddRepository();
            services.AddTransientServices();
            services.AddMemoryCache();

            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            #endregion

            #region Application Services

            // Add application services.
            services.RemoveCollection<IHtmlGenerator, DefaultHtmlGenerator>();
            services.AddTransient<ICloudStorageService, CloudStorageService>();
            services.AddTransient<ITextSharpService, TextSharpService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IHtmlGenerator, CustomHtmlGenerator>();
            services.AddTransient<IDataTablesService, DataTablesService>();
            services.AddTransient<ISelect2Service, Select2Service>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ClaimsPrincipalFactory>();
            services.AddScoped<IViewRenderService, ViewRenderService>();

            if (!ConstantHelpers.GENERAL.Authentication.SSO_ENABLED)
            {
                services.ConfigureApplicationCookie(options =>
                {
                    options.AccessDeniedPath = "/acceso-denegado";
                    options.LoginPath = "/login";
                    options.Cookie.Name = "INTXAUTH";
                });
            }

            services.ConfigureNonBreakingSameSiteCookies();

            Action<MvcOptions> setupAction = (options) => { };

            if (!CurrentEnvironment.IsDevelopment())
            {
                setupAction = (options) =>
                {
                    options.Filters.Add(new ErrorHandlerAttribute());
                };
            }

            services.AddResponseCaching();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSignalR();
            services.AddRazorPages().AddMvcOptions(setupAction);

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.Configure<AppCustomSettings>(Configuration.GetSection(ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]));
            services.Configure<ReportSettings>(Configuration.GetSection(nameof(ReportSettings)));

            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession(opts =>
            {
                opts.Cookie.IsEssential = true; // make the session cookie Essential
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/error/{0}");
                app.UseExceptionHandler("/error/500");
            }

            // Install the dependencies packages for HTML to PDF conversion in Linux
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(CORE.Helpers.ConstantHelpers.AzureEnvironment.AZURE_ENVIRONMENT_VARIABLE)))
                CORE.Helpers.GeneralHelpers.ExecuteBashShellScript(Path.Combine(env.ContentRootPath, CORE.Helpers.ConstantHelpers.AzureEnvironment.AZURE_SHELL_SCRIPT_PATH));

            app.UseCookiePolicy();
            app.UseStaticFiles();
            app.UseRouting();


            //app.UseCors("OidCorsPolicy");
            if (ConstantHelpers.GENERAL.Authentication.SSO_ENABLED)
            {
                var forwardOptions = new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.All,
                    //RequireHeaderSymmetry = fale
                };
                forwardOptions.KnownNetworks.Clear();
                forwardOptions.KnownProxies.Clear();
                app.UseForwardedHeaders(forwardOptions);
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "areaRoute",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapHub<AkdemicHub>(ConstantHelpers.HUBS.AKDEMIC.CLIENT_PROXY.URL);
            });
        }

        private Task RemoteAuthFail(RemoteFailureContext context) { context.Response.Redirect("/"); context.HandleResponse(); return Task.CompletedTask; }
    }
}
