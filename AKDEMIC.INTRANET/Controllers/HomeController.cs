using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore.Identity;
using AKDEMIC.ENTITIES.Models.Generals;
using Microsoft.AspNetCore.SignalR;
using AKDEMIC.INTRANET.Hubs;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.INTRANET.Areas.Admin.Models.AnnouncementViewModels;
using Microsoft.AspNetCore.Diagnostics;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.INTRANET.ViewModels.HomeViewModels;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.CORE.Services;
using AutoMapper;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Geo.Interfaces;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System.IO;
using AKDEMIC.CORE.Options;
using Microsoft.Extensions.Options;
using AKDEMIC.INTRANET.ViewModels.ProfileViewModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using AKDEMIC.INTRANET.ViewModels;
using IdentityServer4.Services;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using AKDEMIC.ENTITIES.Models.Tutoring;
using Microsoft.AspNetCore.Hosting;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private new readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfigurationService _configurationService;
        private readonly IInternalProcedureService _internalProcedureService;
        private readonly IRolAnnouncementService _rolAnnouncementService;
        private readonly IRecordHistoryService _recordHistoryService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ISurveyUserService _surveyUserService;
        private readonly IAnnouncementService _announcementService;
        private readonly IEventRoleService _eventRoleService;
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;
        private readonly ITopicService _topicService;
        private readonly IPaymentService _paymentService;
        private readonly IConnectionService _connectionService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IBeginningAnnouncementService _beginningAnnouncementService;
        private readonly IUserAnnouncementService _userAnnouncementService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITutoringAnnouncementService _tutoringAnnouncementService;
        private readonly ICareerService _careerService;
        private readonly IActionContextAccessor _accessor;

        public HomeController(AkdemicContext context, IOptions<CloudStorageCredentials> storageCredentials,
                              UserManager<ApplicationUser> userManager,
                              ICareerService careerService,
                              ISurveyUserService surveyUserService,
                              IConfigurationService configurationService,
                              IHubContext<AkdemicHub> hubContext, RoleManager<ApplicationRole> roleManager,
                              ITutoringAnnouncementService tutoringAnnouncementService,
                              SignInManager<ApplicationUser> signInManager,
                              IInternalProcedureService internalProcedureService,
                              IRolAnnouncementService rolAnnouncementService,
                              ITermService termService,
                              IRecordHistoryService recordHistoryService,
                              IMapper mapper,
                              IUserService userService,
                              IWebHostEnvironment hostingEnvironment,
                              IAnnouncementService announcementService,
                              IEventRoleService eventRoleService,
                              IStudentService studentService,
                              IClassService classService,
                              ITopicService topicService,
                              IPaymentService paymentService,
                              IConnectionService connectionService,
                              IBeginningAnnouncementService beginningAnnouncementService,
                              IUserAnnouncementService userAnnouncementService,
                              IUserNotificationService userNotificationService,
                              IActionContextAccessor accessor) : base(context, userManager, hubContext, termService, userService)
        {
            _roleManager = roleManager;
            _surveyUserService = surveyUserService;
            _configurationService = configurationService;
            _internalProcedureService = internalProcedureService;
            _rolAnnouncementService = rolAnnouncementService;
            _recordHistoryService = recordHistoryService;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _announcementService = announcementService;
            _eventRoleService = eventRoleService;
            _studentService = studentService;
            _classService = classService;
            _topicService = topicService;
            _paymentService = paymentService;
            _connectionService = connectionService;
            _userNotificationService = userNotificationService;
            _storageCredentials = storageCredentials;
            _signInManager = signInManager;
            _beginningAnnouncementService = beginningAnnouncementService;
            _userAnnouncementService = userAnnouncementService;
            _accessor = accessor;
            _tutoringAnnouncementService = tutoringAnnouncementService;
            _careerService = careerService;
        }
        /// <summary>
        /// Obtiene la vista inicial de intranet
        /// </summary>
        /// <returns>Retorna una vista</returns>
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            //await SendNotificationAsync(new string[] { "bienestar" }, "Notificación de prueba elaborada para las pruebas que se quieren hacer en esta prueba LOREM", Url.GenerateLink(nameof(ClassScheduleController.Index), "ClassSchedule", Request.Scheme, new { area = "Student" }), "Urgente", ConstantHelpers.NOTIFICATIONS.COLORS.BRAND);

            var userId = _userManager.GetUserId(User);
            var user = await _userManager.GetUserAsync(User);

            var roles = await _userService.GetRoles(user);

            var requiredEnabled = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.SURVEY_ENFORCE_REQUIRED));
            if (requiredEnabled)
            {
                //Primera encuesta en el periodo de tiempo
                var surveyuser = await _surveyUserService.GetFirstUserSurvey(true, userId);
                if (surveyuser != null) return RedirectToAction("AnswerEnforcedSurvey", "SurveyStudentEnforced");
            }


            if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS))
            {
                var student = await _studentService.GetStudentByUser(userId);
                //var config1 = bool.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.InstitutionalWelfareManagement.STUDENT_INFORMATION_VISIBILITY));
                //var config2 = bool.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.InstitutionalWelfareManagement.INSTITUTIONAL_WELFARE_SURVEY_VISIBILITY));
                var configMedicalRecord = bool.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.InstitutionalWelfareManagement.MEDICAL_RECORD_VISIBILITY));

                if (configMedicalRecord)
                {
                    if (student.Status == ConstantHelpers.Student.States.ENTRANT && !student.MedicalRecordId.HasValue)
                    {
                        return RedirectToAction("Index", "MedicalRecord");
                    }
                }
            }

            var dateNow = DateTime.UtcNow.Date;

            var careerList = new List<Guid>();

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = await _rolAnnouncementService.GetAnnouncementsHome(dateNow, userRoles);
            var result = _mapper.Map<List<AnnouncementViewModel>>(model);

            var profileViewModel = new ProfileViewModel(user.FirstTime, user.Email, user.PhoneNumber);

            var beginnings = await _beginningAnnouncementService.GetBeginningAnnouncements(ConstantHelpers.ANNOUNCEMENT.SYSTEM.INTRANET, User);
            var userannouncement = await _userAnnouncementService.GetActive(user.Id);

            var activationPassword = await _userService.GetPersonalizedEmailPassword(user.UserName);
            var homemodel = new HomeViewModel()
            {
                EmailInstitutional = user.Email == null ? "--" : user.Email,
                ActivationPassword = activationPassword == null ? "--" : activationPassword,
                Announcements = result,
                Profile = profileViewModel,
                BeginningAnnouncements = beginnings,
                UserAnnouncement = userannouncement,

            };

            if (roles.Any(r => r == ConstantHelpers.ROLES.STUDENTS))
            {
                var student = await _studentService.GetStudentByUser(user.Id);
                careerList.Add(student.CareerId);

            }
            if (roles.Any(r => r == ConstantHelpers.ROLES.CAREER_DIRECTOR || r == ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
            {
                var arrayEnumerable = await _careerService.GetCareerByUserCoordinatorId(userId);
                careerList = arrayEnumerable.OfType<Guid>().ToList();
            }

            var manageAnnouncements = Enumerable.Empty<TutoringAnnouncement>();

            manageAnnouncements = await _tutoringAnnouncementService.GetAllByRolesAndCareer(roles.ToArray(), ConstantHelpers.SYSTEMS.INTRANET, careerList);

            var ManageAnnouncements = manageAnnouncements.Select(x => new AnnouncementManagementViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Message = x.Message,
                AllRoles = x.AllRoles,
                AllCareers = x.AllCareers,
                DisplayTime = x.DisplayTime,
                Roles = x.TutoringAnnouncementRoles.Select(tr => tr.Role.Name).ToList(),
                Careers = x.TutoringAnnouncementCareers.Select(tc => tc.Career.Name).ToList(),
                HasFile = !String.IsNullOrEmpty(x.File)
            }).ToList();
            if (manageAnnouncements.Count() > 0)
            {
                homemodel.ManageAnnouncements = ManageAnnouncements;
            }

            return View(homemodel);
        }

        /// <summary>
        /// Obtiene una vista inicial de intranet
        /// </summary>
        /// <returns>Retorna una vista</returns>
        [Route("/inicio")]
        public async Task<IActionResult> Index2()
        {
            //await SendNotificationAsync(new string[] { "bienestar" }, "Notificación de prueba elaborada para las pruebas que se quieren hacer en esta prueba LOREM", Url.GenerateLink(nameof(ClassScheduleController.Index), "ClassSchedule", Request.Scheme, new { area = "Student" }), "Urgente", ConstantHelpers.NOTIFICATIONS.COLORS.BRAND);

            var userId = _userManager.GetUserId(User);
            var user = await _userManager.GetUserAsync(User);

            var roles = await _userService.GetRoles(user);

            var requiredEnabled = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.SURVEY_ENFORCE_REQUIRED));
            if (requiredEnabled)
            {
                //Primera encuesta en el periodo de tiempo
                var surveyuser = await _surveyUserService.GetFirstUserSurvey(true, userId);
                if (surveyuser != null) return RedirectToAction("AnswerEnforcedSurvey", "SurveyStudentEnforced");
            }


            if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS))
            {
                var student = await _studentService.GetStudentByUser(userId);
                var config1 = bool.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.InstitutionalWelfareManagement.STUDENT_INFORMATION_VISIBILITY));
                var config2 = bool.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.InstitutionalWelfareManagement.INSTITUTIONAL_WELFARE_SURVEY_VISIBILITY));
                var configMedicalRecord = bool.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.InstitutionalWelfareManagement.MEDICAL_RECORD_VISIBILITY));

                if (config1 || config2)
                {
                    if (!(ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.PMESUT || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAB))
                    {
                        var code = int.Parse(user.UserName); 

                        if (code >= 200000 && !student.StudentInformationId.HasValue)
                        {
                            return RedirectToAction("Index", "StudentInformation");
                        }
                    }
                    else
                    {
                        if (!student.StudentInformationId.HasValue)
                        {
                            return RedirectToAction("Index", "StudentInformation");
                        }
                    }

                }
                if (configMedicalRecord)
                {
                    if (student.Status == ConstantHelpers.Student.States.ENTRANT && !student.MedicalRecordId.HasValue)
                    {
                        return RedirectToAction("Index", "MedicalRecord");
                    }
                }
            }

            var dateNow = DateTime.UtcNow.Date;

            var careerList = new List<Guid>();

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = await _rolAnnouncementService.GetAnnouncementsHome(dateNow, userRoles);
            var result = _mapper.Map<List<AnnouncementViewModel>>(model);

            var profileViewModel = new ProfileViewModel(user.FirstTime, user.Email, user.PhoneNumber);

            var beginnings = await _beginningAnnouncementService.GetBeginningAnnouncements(ConstantHelpers.ANNOUNCEMENT.SYSTEM.INTRANET, User);
            var userannouncement = await _userAnnouncementService.GetActive(user.Id);

            var activationPassword = await _userService.GetPersonalizedEmailPassword(user.UserName);
            var homemodel = new HomeViewModel()
            {
                EmailInstitutional = user.PersonalEmail == null ? "--" : user.PersonalEmail,
                ActivationPassword = activationPassword == null ? "--" : activationPassword,
                Announcements = result,
                Profile = profileViewModel,
                BeginningAnnouncements = beginnings,
                UserAnnouncement = userannouncement,

            };

            if (roles.Any(r => r == ConstantHelpers.ROLES.STUDENTS))
            {
                var student = await _studentService.GetStudentByUser(user.Id);
                careerList.Add(student.CareerId);

            }
            if (roles.Any(r => r == ConstantHelpers.ROLES.CAREER_DIRECTOR || r == ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
            {
                var arrayEnumerable = await _careerService.GetCareerByUserCoordinatorId(userId);
                careerList = arrayEnumerable.OfType<Guid>().ToList();
            }

            var manageAnnouncements = Enumerable.Empty<TutoringAnnouncement>();

            manageAnnouncements = await _tutoringAnnouncementService.GetAllByRolesAndCareer(roles.ToArray(), ConstantHelpers.SYSTEMS.INTRANET, careerList);

            var ManageAnnouncements = manageAnnouncements.Select(x => new AnnouncementManagementViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Message = x.Message,
                AllRoles = x.AllRoles,
                AllCareers = x.AllCareers,
                DisplayTime = x.DisplayTime,
                Roles = x.TutoringAnnouncementRoles.Select(tr => tr.Role.Name).ToList(),
                Careers = x.TutoringAnnouncementCareers.Select(tc => tc.Career.Name).ToList(),
                HasFile = !String.IsNullOrEmpty(x.File)
            }).ToList();
            if (manageAnnouncements.Count() > 0)
            {
                homemodel.ManageAnnouncements = ManageAnnouncements;
            }

            return View("Index", homemodel);
        }

        //[AllowAnonymous]
        //[Route("/maintenance")]
        //public async Task<IActionResult> Maintenance()
        //{
        //    return View();
        //}

        /// <summary>
        /// Obtiene información del usuario logeado para la vista de inicio
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("home/get")]
        public async Task<IActionResult> GetHome()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userService.Get(userId);
            var roles = await _userManager.GetRolesAsync(user);

            var announcements = await _announcementService.GetHomeAnnouncement();

            var events = await _eventRoleService.GetEventHome(roles);

            var term = await GetActiveTerm();
            if (term == null)
                term = new ENTITIES.Models.Enrollment.Term();
            //var student = await _studentService.GetStudentByUser(userId);
            var student = await _studentService.GetStudentHome(userId);

            var today = (int)(DateTime.UtcNow.DayOfWeek);

            var start = DateTime.UtcNow.Date.AddDays(-today);
            var end = DateTime.UtcNow.Date.AddDays(7 - today);
            var sheduleWeekViewModel = new ScheduleWeekViewModel()
            {
                Start = start.ToString("dd/MM"),
                End = end.ToString("dd/MM")
            };

            var schedules = await _classService.GetSchedulesHome(student.Id, term.Id, start, end);

            var topics = await _topicService.GetTopicsHome();

            var payments = await _paymentService.GetPaymentsHome(userId);


            return Json(new { announcements, events, topics, payments, schedules, sheduleWeekViewModel });
        }
        /*public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/

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

        [HttpGet("connect/{connectionId}")]
        public async Task<IActionResult> SetConnection(String connectionId)
        {
            try
            {
                if (!string.IsNullOrEmpty(connectionId))
                {
                    var user = await GetCurrentUserAsync();

                    await _connectionService.InsertConnection(new Connection { Code = connectionId, UserId = user.Id });

                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("redirect/{id}")]
        public async Task<IActionResult> RedirectUrl(Guid id)
        {
            var userNotification = await _userNotificationService.GetNotificationHomeByUser(id);


            if (userNotification == null) return RedirectToAction(nameof(Index));

            userNotification.IsRead = true;
            userNotification.ReadDate = DateTime.UtcNow;

            await _userNotificationService.Update(userNotification);

            return Redirect(userNotification.Notification.Url);
        }

        //ACADEMIC COUNTER
        /// <summary>
        /// Obtiene las solicitudes internas del usuario logeado
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("solicitudes/por-usuario")]
        public async Task<IActionResult> GetRequestByUserAndMonth()
        {
            var month = DateTime.UtcNow.Month;
            var user = await _userService.GetUserByClaim(User);
            var result = await _internalProcedureService.GetInternalProcedureCountByMonthAndUserId(month, user.Id);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene las solicitudes internas por meses para el usuario logeado como reporte para un grafico
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("solicitudes/por-usuario/chart")]
        public async Task<IActionResult> GetRequestByUserAndMonthChart()
        {
            var month = DateTime.UtcNow.Month;
            var user = await _userService.GetUserByClaim(User);
            var result = await _recordHistoryService.GetReportByMonthAndUserIdChart(month, user.Id);
            return Ok(result);
        }

        /// <summary>
        /// Descarga imagenes del path donde se encuentren
        /// </summary>
        /// <param name="path">Path donde esta guardado la imagen</param>
        /// <returns>Retorna una imagen</returns>
        [AllowAnonymous]
        [HttpGet("imagenes/{*path}")]
        public async Task DownloadImage(string path)
        {
            using (var mem = new MemoryStream())
            {
                var storage = new CloudStorageService(_storageCredentials);

                await storage.TryDownload(mem, "", path);

                // Download file
                var fileName = Path.GetFileName(path);
                var text = $"inline;filename=\"{fileName.Normalize().Replace(' ', '_')}\"";
                HttpContext.Response.Headers["Content-Disposition"] = text;

                if (Path.GetExtension(fileName) == ".svg")
                    HttpContext.Response.ContentType = "image/svg+xml";

                mem.Position = 0;
                await mem.CopyToAsync(HttpContext.Response.Body);
            }
        }

        /// <summary>
        /// Descarga archivos del path donde se encuentren
        /// </summary>
        /// <param name="path">Path donde esta guardado el archivo</param>
        /// <returns>Retorna un archivo</returns>
        [HttpGet("file/{*path}")]
        public async Task DownloadFile(string path)
        {
            using (var mem = new MemoryStream())
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDownload(mem, "", path);

                // Download file
                string fileName = Path.GetFileName(path);
                string text = $"inline;filename=\"{fileName.Normalize().Replace(' ', '_')}\"";
                HttpContext.Response.Headers["Content-Disposition"] = text;
                mem.Position = 0;
                await mem.CopyToAsync(HttpContext.Response.Body);
            }
        }

        /// <summary>
        /// Guarda la información personal del usuario
        /// </summary>
        /// <param name="model">Modelo que contiene la información personal del usuario</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("datos/post")]
        public async Task<IActionResult> PersonalInformation(UpdateinformationViewModel model)
        {
            var user = await GetCurrentUserAsync();
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            user.NormalizedEmail = model.Email.Normalize();
            user.FirstTime = false;

            var passwordValidator = new PasswordValidator<ApplicationUser>();

            var currentPasswordIsCorrect = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);

            var newPasswordIsValid = passwordValidator.ValidateAsync(_userManager, user, model.NewPassword).Result.Succeeded;

            if (!currentPasswordIsCorrect || !newPasswordIsValid)
                return BadRequest("Revise la información ingresada");

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);

            await _userService.Update(user);

            await _signInManager.SignOutAsync();
            return Ok();
        }

        public IActionResult MyIpAddress()
        {
            var RemoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var ContextAccesor = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

            var model = new IpAddressViewModel
            {
                RemoteIpAddress = RemoteIpAddress,
                ContextAccesorIpAddress = ContextAccesor
            };

            return View(model);
        }

        public string GetRequestIP(bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
            {
                var csvList = GetHeaderValueAs<string>("X-Forwarded-For");
                ip = SplitCsv(csvList).FirstOrDefault();
            }

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(ip) && _accessor.ActionContext.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (string.IsNullOrWhiteSpace(ip))
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        public T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values;

            if (_accessor.ActionContext.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        public static List<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }

        /// <summary>
        /// Descarga el comunicado
        /// </summary>
        /// <param name="id">Identificador del comunicado</param>
        /// <returns>Retorna un archivo</returns>
        [HttpGet("descargar-comunicado/pdf/{id}")]
        public async Task Download(Guid id)
        {
            var announcement = await _tutoringAnnouncementService.Get(id);
            await AKDEMIC.CORE.Helpers.GeneralHelpers.GetFileForDownload(HttpContext, _storageCredentials, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.TUTORING_ANNOUNCEMENTS, announcement.File);
        }

        /// <summary>
        /// Descarga un archivo del path donde se encuentre
        /// </summary>
        /// <param name="path">Path donde se encuentre el archivo</param>
        /// <returns>Retorna un archivo</returns>
        [AllowAnonymous]
        [HttpGet("pdf/{*path}")]
        public async Task DownloadPdfService(string path)
        {
            //var fileName = Path.GetFileName(path).Normalize().Replace(' ', '_');
            //await GeneralHelpers.GetFileForDownload(HttpContext, _storageCredentials, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.SISCO, fileName);

            using (var mem = new MemoryStream())
            {
                var storage = new CloudStorageService(_storageCredentials);

                await storage.TryDownload(mem, "", path);

                // Download file
                var fileName = Path.GetFileName(path);
                var text = $"inline;filename=\"{fileName.Normalize().Replace(' ', '_')}\"";
                HttpContext.Response.Headers["Content-Disposition"] = text;
                mem.Position = 0;
                await mem.CopyToAsync(HttpContext.Response.Body);
            }
        }

        [AllowAnonymous]
        [HttpGet("img-login-sistema")]
        public async Task LoginBackgroundImage()
        {
            var path = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.INT_LOGIN_BACKGROUND_IMAGE);

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
