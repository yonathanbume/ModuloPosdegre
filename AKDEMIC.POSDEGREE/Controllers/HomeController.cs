using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;

using AKDEMIC.POSDEGREE.Helpers;
using AKDEMIC.POSDEGREE.ViewModels.HomeViewModels;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System.Text.RegularExpressions;

namespace AKDEMIC.POSDEGREE.Controllers {

    /*private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }*/
   
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.ADMIN_POSDEGREE)]

    public class HomeController : BaseController
        {
            private readonly IOptions<CloudStorageCredentials> _storageCredentials;
            private readonly IRegistryPatternService _registryPatternService;
            private readonly AkdemicContext _context;
            private readonly IPaymentService _paymentService;
            private readonly IWebHostEnvironment _hostingEnvironment;
            private readonly IUserProcedureService _userProcedureService;
            private readonly IUserService _userService;

            public HomeController(IOptions<CloudStorageCredentials> storageCredentials,
                IPaymentService paymentService,
                IWebHostEnvironment hostingEnvironment,
                IRegistryPatternService registryPatternService,
                IUserProcedureService userProcedureService,
                IUserService userService,
                AkdemicContext context,
                IConfigurationService configurationService) : base(configurationService)
            {
                _storageCredentials = storageCredentials;
                _registryPatternService = registryPatternService;
                _context = context;
                _paymentService = paymentService;
                _hostingEnvironment = hostingEnvironment;
                _userProcedureService = userProcedureService;
                _userService = userService;
            }

            /// <summary>
            /// Vista de Incio del sistema
            /// </summary>
            /// <returns>Retorna la vista</returns>
            public async Task<IActionResult> Index()
            {
            /*
            var modal = new CountViewModel();
             var listGuid = new List<Guid>();
             var Today = DateTime.UtcNow;
             var Today3MonthAgo = DateTime.UtcNow.Date.AddMonths(-3);
             modal.RegistryPatternCount = await _registryPatternService.RegistryPattern3MonthAgoCount();
             modal.DiplomaCount = await _registryPatternService.Diploma3MonthAgoCount();
             modal.BachelorCount = await _registryPatternService.CurrentCountByGradeType(ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR);
             modal.TitleCount = await _registryPatternService.CurrentCountByGradeType(ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE);
             return View(modal);*/
            //return View("Views/Home/Index.cshtml");
            return View("Views/Shared/Templates/Default/_Layout2.cshtml");

        }

        /// <summary>
        /// Vista de Error
        /// </summary>
        /// <param name="code">Código de error</param>
        /// <returns>Retorna la vista</returns>
        [AllowAnonymous]
            [Route("error/{code}")]
            public IActionResult Error(int code)
            {
                // Get the details of the exception that occurred
                var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

                if (exceptionFeature != null)
                {
                    // Get which route the exception occurred at
                    var routeWhereExceptionOccurred = exceptionFeature.Path;

                    // Get the exception that occurred
                    var exceptionThatOccurred = exceptionFeature.Error;

                    // TODO: Do something with the exception
                    // Log it with Serilog?
                    // Send an e-mail, text, fax, or carrier pidgeon?  Maybe all of the above?
                    // Whatever you do, be careful to catch any exceptions, otherwise you'll end up with a blank page and throwing a 500
                }

                // Valida si el request fue Ajax
               // if (HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest") return new ObjectResult(ConvertHelpers.ErrorCodeToText(code));

                return View();
            }
            [AllowAnonymous]
            [HttpGet("img-login-sistema")]
            public async Task LoginBackgroundImage()
            {
                var path = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.DegreeManagement.GRAD_LOGIN_BACKGROUND_IMAGE);

                using (var mem = new MemoryStream())
                {
                    if (string.IsNullOrEmpty(path))
                    {
                        path = Path.Combine(_hostingEnvironment.WebRootPath, @"images/login/login.jpg");

                        using (var file = new FileStream(path, FileMode.Open))
                        {
                            await file.CopyToAsync(mem);
                        }
                    }
                    else
                    {
                        var storage = new CloudStorageService(_storageCredentials);
                        await storage.TryDownload(mem, "", path);
                    }

                    // Download file
                    var fileName = Path.GetFileName(path);
                    var text = $"inline;filename=\"{fileName.Normalize().Replace(' ', '_')}\"";
                    HttpContext.Response.Headers["Content-Disposition"] = text;
                    mem.Position = 0;
                    await mem.CopyToAsync(HttpContext.Response.Body);
                }
            }  
        }
    }

