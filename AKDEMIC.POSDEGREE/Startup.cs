
using AKDEMIC.CORE.Options;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Data;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Overrides;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using AKDEMIC.REPOSITORY.Factories;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.Degree.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Degree.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Degree.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Implementations;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Implementations;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Generals.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Implementations;
using AKDEMIC.SERVICE.Services.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Implementations;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Extensions;
using System.Runtime.InteropServices;
using System.IO;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Filters;
using AKDEMIC.POSDEGREE.Helpers;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Implementatios;
using AKDEMIC.SERVICE.Services.PosDegree.Implementations;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.SERVICE.Services.PosDegree.Interfaces;

namespace AKDEMIC.POSDEGREE
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

            var lockoutOptions = new LockoutOptions()
            {
                AllowedForNewUsers = true,
                DefaultLockoutTimeSpan = TimeSpan.FromMinutes(CORE.Helpers.ConstantHelpers.Lockout.Time),
                MaxFailedAccessAttempts = CORE.Helpers.ConstantHelpers.Lockout.MaxFailedAccessAttempts
            };

            services.AddDbContext<AkdemicContext>(options =>
            {
                if (CurrentEnvironment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                }

                switch (ConstantHelpers.GENERAL.DATABASES.DATABASE)
                {
                    case ConstantHelpers.DATABASES.MYSQL:
                        options.UseMySql(Configuration.GetConnectionString("MySqlDefaultConnection"),
                            new MySqlServerVersion(ConstantHelpers.DATABASES.VERSIONS.MYSQL.VALUES[ConstantHelpers.DATABASES.VERSIONS.MYSQL.V8021]),
                            mySqlOptions => mySqlOptions.EnableRetryOnFailure());

                        break;
                    case ConstantHelpers.DATABASES.SQL:
                        options.UseSqlServer(Configuration.GetConnectionString("SqlDefaultConnection"), sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());

                        break;
                }
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(setup =>
            {
                setup.Lockout = lockoutOptions;
                setup.Password.RequireDigit = false;
                setup.Password.RequiredLength = 5;
                setup.Password.RequireLowercase = false;
                setup.Password.RequireNonAlphanumeric = false;
                setup.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AkdemicContext>()
                .AddDefaultTokenProviders();

            #endregion

            #region Logging

            if (CurrentEnvironment.IsDevelopment())
            {
                //IdentityModelEventSource.ShowPII = true;
            }

            #endregion


            #region OpenIdConnect

            if (CORE.Helpers.ConstantHelpers.GENERAL.Authentication.SSO_ENABLED)
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "oidc";
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                    {
                        options.SlidingExpiration = true;
                        options.AccessDeniedPath = "/acceso-denegado";
                    })
                    .AddOpenIdConnect("oidc", options =>
                    {
                        options.Authority = CORE.Helpers.GeneralHelpers.GetAuthority(CurrentEnvironment.IsDevelopment());

                        options.RequireHttpsMetadata = false;
                        options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;

                        options.ClientId = "degree";
                        options.ClientSecret = "secret";
                        options.ResponseType = OpenIdConnectResponseType.Code;

                        options.Scope.Clear();
                        options.Scope.Add("openid");
                        options.Scope.Add("profile");
                        options.Scope.Add("roles");

                        // keeps id_token smaller
                        options.GetClaimsFromUserInfoEndpoint = true;
                        options.SaveTokens = true;

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = ClaimTypes.Name,
                            RoleClaimType = ClaimTypes.Role,
                        };

                        options.RemoteAuthenticationTimeout = TimeSpan.FromHours(1);
                        options.Events.OnRemoteFailure = RemoteAuthFail;
                    });

                // add automatic token management
                services.AddAccessTokenManagement();
            }
            #endregion OpenIdConnect

            #region Other

            services.Configure<CloudStorageCredentials>(Configuration.GetSection("AzureStorageCredentials"));
            services.Configure<FormOptions>(x => x.ValueCountLimit = 20480);

            #endregion

            #region Repositories / Services

            // General
            services.AddScoped(typeof(IAcademicProgramRepository), typeof(AcademicProgramRepository));
            services.AddTransient<IAcademicProgramService, AcademicProgramService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IStudentRepository), typeof(StudentRepository));
            services.AddTransient<IStudentService, StudentService>();
            services.AddScoped(typeof(ITermRepository), typeof(TermRepository));
            services.AddTransient<ITermService, TermService>();
            services.AddScoped(typeof(IUserNotificationRepository), typeof(UserNotificationRepository));
            services.AddTransient<IUserNotificationService, UserNotificationService>();
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddTransient<IUserService, UserService>();
            //prueba
            services.AddScoped(typeof(IDocumentTypeRepository), typeof(DocumentTypeRepository));
            services.AddTransient<IDocumentTypeService,DocumentTypeService>();

            // Intranet
            services.AddScoped(typeof(IAcademicCalendarDateRepository), typeof(AcademicCalendarDateRepository));
            services.AddTransient<IAcademicCalendarDateService, AcademicCalendarDateService>();
            services.AddScoped(typeof(IWorkingDayRepository), typeof(WorkingDayRepository));
            services.AddTransient<IWorkingDayService, WorkingDayService>();

            services.AddScoped(typeof(IGradeReportRepository), typeof(GradeReportRepository));
            services.AddTransient<IGradeReportService, GradeReportService>();

            // Enrollment
            services.AddScoped(typeof(REPOSITORY.Repositories.Enrollment.Interfaces.ICourseRepository), typeof(REPOSITORY.Repositories.Enrollment.Implementations.CourseRepository));
            services.AddTransient<ICourseService, CourseService>();
            services.AddScoped(typeof(IEvaluationRepository), typeof(EvaluationRepository));
            services.AddTransient<IEvaluationService, EvaluationService>();
            services.AddScoped(typeof(ISectionRepository), typeof(SectionRepository));
            services.AddTransient<ISectionService, SectionService>();

            services.AddScoped(typeof(IRegistryPatternRepository), typeof(RegistryPatternRepository));
            services.AddTransient<IRegistryPatternService, RegistryPatternService>();

            services.AddScoped(typeof(IHistoricalRegistryPatternRepository), typeof(HistoricalRegistryPatternRepository));
            services.AddTransient<IHistoricalRegistryPatternService, HistoricalRegistryPatternService>();

            services.AddScoped(typeof(ICareerRepository), typeof(CareerRepository));
            services.AddTransient<ICareerService, CareerService>();

            services.AddScoped(typeof(IFacultyRepository), typeof(FacultyRepository));
            services.AddTransient<IFacultyService, FacultyService>();


            services.AddScoped(typeof(IConfigurationRepository), typeof(ConfigurationRepository));
            services.AddTransient<IConfigurationService, ConfigurationService>();

            services.AddScoped(typeof(IPaymentRepository), typeof(PaymentRepository));
            services.AddTransient<IPaymentService, PaymentService>();

            services.AddScoped(typeof(IDegreeRequirementRepository), typeof(DegreeRequirementRepository));
            services.AddTransient<IDegreeRequirementService, DegreeRequirementService>();

            services.AddScoped(typeof(IGradeReportRepository), typeof(GradeReportRepository));
            services.AddTransient<IGradeReportService, GradeReportService>();

            services.AddScoped(typeof(IGradeReportRequirementRepository), typeof(GradeReportRequirementRepository));
            services.AddTransient<IGradeReportRequirementService, GradeReportRequirementService>();

            services.AddScoped(typeof(IForeignUniversityOriginRepository), typeof(ForeignUniversityOriginRepository));
            services.AddTransient<IForeignUniversityOriginService, ForeignUniversityOriginService>();

            services.AddScoped(typeof(IDeanFacultyRepository), typeof(DeanFacultyRepository));
            services.AddTransient<IDeanFacultyService, DeanFacultyService>();

            services.AddScoped(typeof(IUniversityAuthorityRepository), typeof(UniversityAuthorityRepository));
            services.AddTransient<IUniversityAuthorityService, UniversityAuthorityService>();

            services.AddScoped(typeof(IUserProcedureRepository), typeof(UserProcedureRepository));
            services.AddTransient<IUserProcedureService, UserProcedureService>();

            services.AddScoped(typeof(IProcedureRepository), typeof(ProcedureRepository));
            services.AddTransient<IProcedureService, ProcedureService>();

            services.AddScoped(typeof(ICareerAccreditationRepository), typeof(CareerAccreditationRepository));
            services.AddTransient<ICareerAccreditationService, CareerAccreditationService>();




            services.AddTransient<IDataTablesService, DataTablesService>();
            //Posdegree
            services.AddScoped(typeof(IMasterRepository), typeof(MasterRepository));
            services.AddTransient<IMasterService, MasterService>();

            services.AddScoped(typeof(IPosdegreeStudentRepository), typeof(PosdegreeStudentRepository));
            services.AddTransient<IPosdegreeStudentService, PosdegreeStudentService>();

            services.AddScoped(typeof(ITeacherPRepository), typeof(AKDEMIC.REPOSITORY.Repositories.PosDegree.Implementatios.TeacherRepository));
            services.AddTransient<ITeacherPService, AKDEMIC.SERVICE.Services.PosDegree.Implementations.TeacherService>();
           
            services.AddScoped(typeof(IAsignaturaRepository), typeof(AsignaturaRepository));
            services.AddTransient<IAsignaturaService, AsignaturaService>();

            services.AddScoped(typeof(ITypeEnrollmentRepository), typeof(TypeEnrollmentRepository));
            services.AddTransient<ITypeEnrollmentService, TypeEnrollmentService>();
            services.AddScoped(typeof(ISemestreRepository), typeof(SemestreRepository));
            services.AddTransient<ISemestreService, SemestreService>();
            #endregion

            #region Application Services

            // Add application services.
            //services.RemoveCollection<IHtmlGenerator, DefaultHtmlGenerator>();
            services.AddTransient<ICloudStorageService, CloudStorageService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IHtmlGenerator, CustomHtmlGenerator>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ClaimsPrincipalFactory>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddHttpContextAccessor();

            if (!ConstantHelpers.GENERAL.Authentication.SSO_ENABLED)
            {
                services.ConfigureApplicationCookie(options =>
                {
                    options.AccessDeniedPath = "/acceso-denegado";
                    options.LoginPath = "/login";
                    options.Cookie.Name = "DEGXAUTH";
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

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            if (CurrentEnvironment.IsDevelopment())
            {
                services.AddControllersWithViews().AddRazorRuntimeCompilation();
            }
            else
            {
                services.AddControllersWithViews();
            }
            services.AddRazorPages().AddMvcOptions(setupAction);
            services.AddSignalR();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddSession(opts =>
            {
                opts.Cookie.IsEssential = true; // make the session cookie Essential
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    //app.UseBrowserLink();
            //   // app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    //app.UseStatusCodePagesWithReExecute("/error/{0}");
            //    //app.UseExceptionHandler("/error/500");
            //}
             
            app.UseDeveloperExceptionPage();

            // Install the dependencies packages for HTML to PDF conversion in Linux
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(CORE.Helpers.ConstantHelpers.AzureEnvironment.AZURE_ENVIRONMENT_VARIABLE)))
                CORE.Helpers.GeneralHelpers.ExecuteBashShellScript(Path.Combine(env.ContentRootPath, CORE.Helpers.ConstantHelpers.AzureEnvironment.AZURE_SHELL_SCRIPT_PATH));

            app.UseCookiePolicy();

            app.UseStaticFiles();
            app.UseRouting();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "areaRoute",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        private Task RemoteAuthFail(RemoteFailureContext context) { context.Response.Redirect("/"); context.HandleResponse(); return Task.CompletedTask; }
    }
}
