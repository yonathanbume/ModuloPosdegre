using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Areas.Teacher.Models.ExtraordinaryEvaluationViewModels;
using Microsoft.AspNetCore.Authorization;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.CORE.Helpers;
using DinkToPdf.Contracts;
using AKDEMIC.CORE.Extensions;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.TEACHERS)]
    [Route("profesor/evaluacion-extraordinaria")]
    public class ExtraordinaryEvaluationController : BaseController
    {
        private readonly IExtraordinaryEvaluationStudentService _extraordinaryEvaluationStudentService;
        private readonly IExtraordinaryEvaluationService _extraordinaryEvaluationService;
        private readonly ICourseService _courseService;
        private readonly IConfigurationService _configurationService;
        private readonly IUserService _userService;
        private readonly IEvaluationReportService _evaluationReportService;
        private readonly IExtraordinaryEvaluationCommitteeService _extraordinaryEvaluationCommitteeService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly ITermService _termService;
        private readonly IStudentService _studentService;
        private readonly IViewRenderService _viewRenderService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IConverter _dinkConverter { get; }

        public ExtraordinaryEvaluationController(IDataTablesService dataTablesService, UserManager<ApplicationUser> userManager,
            IExtraordinaryEvaluationStudentService extraordinaryEvaluationStudentService,
            IConverter dinkConverter,
            IWebHostEnvironment webHostEnvironment,
            IViewRenderService viewRenderService,
            IExtraordinaryEvaluationService extraordinaryEvaluationService,
            ICourseService courseService,
            IConfigurationService configurationService,
            IUserService userService,
            IEvaluationReportService evaluationReportService,
            IExtraordinaryEvaluationCommitteeService extraordinaryEvaluationCommitteeService,
            IAcademicHistoryService academicHistoryService, ITermService termService,
            IStudentService studentService) : base(userManager, dataTablesService)
        {
            _extraordinaryEvaluationStudentService = extraordinaryEvaluationStudentService;
            _extraordinaryEvaluationService = extraordinaryEvaluationService;
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _courseService = courseService;
            _configurationService = configurationService;
            _userService = userService;
            _evaluationReportService = evaluationReportService;
            _extraordinaryEvaluationCommitteeService = extraordinaryEvaluationCommitteeService;
            _academicHistoryService = academicHistoryService;
            _termService = termService;
            _webHostEnvironment = webHostEnvironment;
            _studentService = studentService;
        }

        /// <summary>
        /// Obtiene el listado de evaluaciones extraordinarias asignadas al docente logueado.
        /// Deben pertenecer al periodo activo.
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de evaluaciones extraordinarias</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var user = await _userService.GetUserByClaim(User);
            var result = await _extraordinaryEvaluationService.GetExtraordinaryEvaluationsToTeacherDatatable(parameters, search, user.Id);
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se gestionan las evaluaciones extraordinarias
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            var enabled_extraordinary_evaluation = bool.Parse(await _configurationService.GetValueByKey(AKDEMIC.CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ENABLED_EXTRAORDINARY_EVALUATION));

            if (!enabled_extraordinary_evaluation)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            return View();
        }

        /// <summary>
        /// Vista detalle de la evaluación extraordinaria
        /// </summary>
        /// <param name="id">Identificador de la evaluación extraordinaria</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("detalles/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var enabled_extraordinary_evaluation = bool.Parse(await _configurationService.GetValueByKey(AKDEMIC.CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ENABLED_EXTRAORDINARY_EVALUATION));

            if (!enabled_extraordinary_evaluation)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            var entity = await _extraordinaryEvaluationService.Get(id);
            var commitee = await _extraordinaryEvaluationCommitteeService.GetCommittee(entity.Id);
            var teacher = await _userService.Get(entity.TeacherId);
            var course = await _courseService.GetAsync(entity.CourseId);
            var term = await _termService.Get(entity.TermId);

            var model = new ExtraordinaryEvaluationViewModel
            {
                Id = entity.Id,
                Committee = commitee.Select(x => x.Teacher.User.FullName).ToList(),
                ResolutionFileUrl = entity.ResolutionFile,
                Teacher = teacher.FullName,
                Course = $"{course.Code} - {course.Name}",
                Term = term.Name
            };

            return View(model);
        }

        [HttpGet("{extraordinaryEvaluationId}/calificacion-pendiente/get")]
        public async Task<IActionResult> HasPendingCalification(Guid extraordinaryEvaluationId)
        {
            var hasPendingCalification = await _extraordinaryEvaluationStudentService.IsPendingFromQualify(extraordinaryEvaluationId);

            return Ok(hasPendingCalification);
        }

        [HttpGet("{extraordinaryEvaluationId}/reporte-pdf")]
        public async Task<IActionResult> ReportPDF(Guid extraordinaryEvaluationId)
        {
            var extraordinaryEvaluation = await _extraordinaryEvaluationService.Get(extraordinaryEvaluationId);

            if (extraordinaryEvaluation == null)
                return BadRequest("Sucedio un error");

            var model = await _extraordinaryEvaluationStudentService.GetEvaluationReportInformation(extraordinaryEvaluation.Id);

            var userLoggedIn = await _userService.GetUserByClaim(User);
            model.UserLoggedIn = userLoggedIn.Name;
            model.Img = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png");

            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format7/ExtraordinaryEvaluationReport.cshtml", model);
            var pdfContent = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de Evaluación Final"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        },
                        FooterSettings = {
                            FontName = "Arial",
                            FontSize = 6,
                            Line = false,
                            Left = $"{model.UserLoggedIn}",
                            Right = $"{DateTime.UtcNow.ToDefaultTimeZone().ToString("dd-MM-yyyy HH:mm:ss")}",
                            Center = "Página [page] de [toPage]",
                        }
                    }
                }
            };

            var fileContent = _dinkConverter.Convert(pdfContent);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileContent, "application/pdf", "ReporteEvaExtraordinaria.pdf");
        }

        /// <summary>
        /// Método para obtener el listado de estudiantes asignados a la evaluación extraordinaria
        /// </summary>
        /// <param name="extraordinaryEvalutionId">Identificador de la evaluación extraordinaria</param>
        /// <returns>Objeto que contiene el listado de estudiantes asignados</returns>
        [HttpGet("get-estudiantes")]
        public async Task<IActionResult> GetStudents(Guid extraordinaryEvalutionId)
        {
            var students = await _extraordinaryEvaluationStudentService.GetStudentsDatatableClientSide(extraordinaryEvalutionId);
            return Ok(students);
        }

        /// <summary>
        /// Método para obtener los detalles de la evaluación extraordinaria de un estudiante
        /// </summary>
        /// <param name="extraordinaryEvaluationStudentId">Identificador de la evaluación extraordinaria de un estudiante</param>
        /// <returns>Objeto que contiene los datos de la evaluación</returns>
        [HttpGet("get-evaluacion-estudiante")]
        public async Task<IActionResult> GetEvaluationStudent(Guid extraordinaryEvaluationStudentId)
        {
            var entity = await _extraordinaryEvaluationStudentService.Get(extraordinaryEvaluationStudentId);
            var result = new
            {
                entity.Id,
                entity.Observations,
                entity.Grade
            };

            return Ok(result);
        }

        /// <summary>
        /// Método para calificar la evaluación extraordinaria de un estudiante
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la calificación</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("calificar")]
        public async Task<IActionResult> ExtraordinaryEvaluationGrade(GradeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Verifique la información ingresada");
            }
            var evaluationStudent = await _extraordinaryEvaluationStudentService.Get(model.Id);
            var evaluation = await _extraordinaryEvaluationService.Get(evaluationStudent.ExtraordinaryEvaluationId);
            var term = await _termService.Get(evaluation.TermId);

            //if (evaluation.Type == ConstantHelpers.Intranet.ExtraordinaryEvaluation.EXTRAORDINARY)
            //{
            //    var evalutionReport = await _evaluationReportService.GetEvaluationReportByFilters(null, evaluation.CourseId, evaluation.TermId, ConstantHelpers.Intranet.EvaluationReportType.EXTRAORDINARY_EVALUATION);
            //    if (evalutionReport != null)  
            //        return BadRequest("Ya se ha generado el acta de la evaluación.");
            //}

            evaluationStudent.GradePublicationDate = DateTime.UtcNow;
            evaluationStudent.Grade = model.Grade;
            evaluationStudent.Status = model.Grade >= term.MinGrade
                ? CORE.Helpers.ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.APPROVED
                : CORE.Helpers.ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.DISAPPROVED;
            evaluationStudent.Observations = model.Observations;

            var finalGrade = (int)Math.Round(model.Grade, 0, MidpointRounding.AwayFromZero);
            var lastTry = await _academicHistoryService.GetLastByStudentAndCourse(evaluationStudent.StudentId, evaluation.CourseId);

            var student = await _studentService.Get(evaluationStudent.StudentId);

            var academicHistory = new AcademicHistory
            {
                Approved = finalGrade >= term.MinGrade,
                CourseId = evaluation.CourseId,
                Grade = finalGrade,
                Observations = "EE",
                StudentId = evaluationStudent.StudentId,
                TermId = evaluation.TermId,
                Try = lastTry != null ? lastTry.Try + 1 : 1,
                Validated = false,
                Type = CORE.Helpers.ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION,
                CurriculumId = student.CurriculumId
            };

            await _academicHistoryService.Insert(academicHistory);

            evaluationStudent.AcademicHistoryId = academicHistory.Id;
            await _extraordinaryEvaluationStudentService.Update(evaluationStudent);

            return Ok();
        }

        ///// <summary>
        ///// Método para editar la calificación de un estudiante
        ///// </summary>
        ///// <param name="model">Objeto que contiene los datos actualizados de la calificación</param>
        ///// <returns>Código de estado HTTP</returns>
        //[HttpPost("editar-calificacion")]
        //public async Task<IActionResult> EditExtraordianryEvaluationGrade(GradeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest("Verifique la información ingresada");

        //    var evaluationStudent = await _extraordinaryEvaluationStudentService.Get(model.Id);
        //    var evalution = await _extraordinaryEvaluationService.Get(evaluationStudent.ExtraordinaryEvaluationId);
        //    var term = await _termService.Get(evalution.TermId);

        //    var evalutionReport = await _evaluationReportService.GetEvaluationReportByFilters(null, evalution.CourseId, evalution.TermId, ConstantHelpers.Intranet.EvaluationReportType.EXTRAORDINARY_EVALUATION);
        //    if (evalutionReport != null)
        //        return BadRequest("Ya se ha generado el acta de la evaluación.");

        //    evaluationStudent.Status = model.Grade >= term.MinGrade
        //        ? CORE.Helpers.ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.APPROVED
        //        : CORE.Helpers.ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.DISAPPROVED;

        //    evaluationStudent.Grade = model.Grade;
        //    evaluationStudent.Observations = model.Observations;

        //    if (evaluationStudent.AcademicHistoryId.HasValue)
        //    {
        //        var finalGrade = (int)Math.Round(model.Grade, 0, MidpointRounding.AwayFromZero);

        //        var academicHistory = await _academicHistoryService.Get(evaluationStudent.AcademicHistoryId.Value);
        //        if (academicHistory is null)
        //            return BadRequest("No se encontró el historial de notas del estudiantes.");

        //        academicHistory.Approved = finalGrade >= term.MinGrade;
        //        academicHistory.Grade = finalGrade;
        //    }

        //    await _extraordinaryEvaluationStudentService.Update(evaluationStudent);

        //    return Ok();
        //}
    }
}
