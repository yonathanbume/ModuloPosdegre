using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using AKDEMIC.DEGREE.Helpers;
using AKDEMIC.DEGREE.ViewModels.HomeViewModels;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Options;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IO;
using AKDEMIC.CORE.Services;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;

namespace AKDEMIC.DEGREE.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," + ConstantHelpers.ROLES.REPORT_QUERIES)]
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
            var modal = new CountViewModel();
            var listGuid = new List<Guid>();
            var Today = DateTime.UtcNow;
            var Today3MonthAgo = DateTime.UtcNow.Date.AddMonths(-3);
            modal.RegistryPatternCount = await _registryPatternService.RegistryPattern3MonthAgoCount();
            modal.DiplomaCount = await _registryPatternService.Diploma3MonthAgoCount();
            modal.BachelorCount = await _registryPatternService.CurrentCountByGradeType(ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR);
            modal.TitleCount = await _registryPatternService.CurrentCountByGradeType(ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE);
            return View(modal);
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
            if (HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest") return new ObjectResult(ConvertHelpers.ErrorCodeToText(code));

            return View();
        }

        /// <summary>
        /// Vista para el seguimiento del diploma
        /// </summary>
        /// <returns>Retorna la vista</returns>
        [AllowAnonymous]
        [Route("seguimiento-diploma")]
        public IActionResult DiplomaTracking()
        {
            return View();
        }

        /// <summary>
        /// VIsta para el seguimiento del diploma
        /// </summary>
        /// <param name="userName">Usuario</param>
        /// <returns>Retorna un Ok o BadRequest</returns>
        [AllowAnonymous]
        [HttpGet("seguimiento-diploma-2/{userName}")]
        public async Task<IActionResult> DiplomaTrack(string userName)
        {
            var message = "";
            var registryPattern = await _context.RegistryPatterns.Include(x => x.Student.User).Where(x => x.Student.User.UserName.Trim().ToLower() == userName.Trim().ToLower()).FirstOrDefaultAsync();
            if (registryPattern == null)
            {
                message = "No se encontró un trámite asociado";
                return Ok(message);
            }
            if (registryPattern.Status == ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED)
            {
                message = "Diploma en proceso";

                var userProcedure = new UserProcedure();
                if (registryPattern.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
                {
                    userProcedure = await _userProcedureService.GetUserProcedureByStaticType(registryPattern.Student.UserId, ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR_DEGREE_APPLICATION);
                }
                else
                {
                    userProcedure = await _userProcedureService.GetUserProcedureByStaticType(registryPattern.Student.UserId, ConstantHelpers.PROCEDURES.STATIC_TYPE.TITLE_DEGREE_APPLICATION);
                }
                switch (registryPattern.DiplomaStatus)
                {
                    case ConstantHelpers.REGISTRY_PATTERN.DIPLOMA_DELIVERY.PENDING:

                        message = "Diploma en proceso";
                        break;
                    case ConstantHelpers.REGISTRY_PATTERN.DIPLOMA_DELIVERY.SIGNED:
                        message = "Diploma firmada para entregar";
                        break;
                    case ConstantHelpers.REGISTRY_PATTERN.DIPLOMA_DELIVERY.DELIVERED:
                        message = "Diploma en entregada";
                        break;
                }
                return Ok(message);
            }
            else
            {
                message = "No se encontró un trámite asociado";
                return Ok(message);
            }

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

        [HttpGet("actualizar-foto-reniec-postulantes-pendientes")]
        public async Task<IActionResult> UpdatePendingReniecPicture()
        {
            var postulants = await _context.Postulants.ToListAsync();

            postulants = postulants.Where(x => CalculateAge(x.BirthDate) >= 18 && string.IsNullOrEmpty(x.ReniecPicture)).ToList();

            var storage = new CloudStorageService(_storageCredentials);

            foreach (var item in postulants)
            {
                var reniecUser = await _userService.GetReniecUserByDni(item.Document);

                if (reniecUser != null)
                {
                    var reniecPictureBase64 = reniecUser.Picture;
                    var reniecPicture = Convert.FromBase64String(reniecPictureBase64);

                    using (var stream = new MemoryStream(reniecPicture))
                    {
                        item.ReniecPicture = await storage.UploadFile(stream, "pideadmision",
                            System.IO.Path.GetExtension(".png"), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.ADMISSION);
                    }

                    await _context.SaveChangesAsync();
                }
            }

            return Ok();
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }

                [HttpGet("testpide")]
        public async Task<IActionResult> TstPIDE()
        {
            try
            {
                var url = "https://ws5.pide.gob.pe/Rest/Reniec/Consultar?nuDniConsulta=75141180&nuDniUsuario=40205066&nuRucUsuario=20148421014&password=40205066";

                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(url))
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        return Ok(responseString);
                    }

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
