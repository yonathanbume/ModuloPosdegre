using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.ViewModels.AcademicSummaryViewModels;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.INTRANET.Areas.Admission.Controllers
{
    //   [Authorize(Roles = ConstantHelpers.ROLES.ADMISSION + "," + ConstantHelpers.ROLES.DEGREES_AND_TITLES)]

    [Authorize(Roles = ConstantHelpers.ROLES.ADMISSION)]
    [Area("Admission")]
    [Route("admision/situacion-academica")]
    public class AcademicSituationController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IAcademicYearCourseService _academicYearCourseService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IAcademicSummariesService _academicSummariesService;

        public IConverter _dinkConverter { get; }
        private readonly IViewRenderService _viewRenderService;
        private IWebHostEnvironment _hostingEnvironment;

        public AcademicSituationController(
            IUserService userService,
            ITermService termService,
            IStudentService studentService,
            IStudentSectionService studentSectionService,
            IAcademicYearCourseService academicYearCourseService,
            IAcademicSummariesService academicSummariesService,
            IAcademicHistoryService academicHistoryService,
            IConverter dinkConverter,
            IDataTablesService dataTablesService,
            IViewRenderService viewRenderService,
            IWebHostEnvironment environment) : base(termService, userService, dataTablesService)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
            _studentService = studentService;
            _academicHistoryService = academicHistoryService;
            _studentSectionService = studentSectionService;
            _academicYearCourseService = academicYearCourseService;
            _academicSummariesService = academicSummariesService;
        }

        /// <summary>
        /// Vista principal del controlador
        /// </summary>
        /// <returns>Vista</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Vista del progreso curricular del estudiante
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("{id}/situacion")]
        public IActionResult CurriculumProgress(Guid id) => View(id);

        /// <summary>
        /// Vista del historial académico del estudiante
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("{id}/historial")]
        public IActionResult AcademicSummary(Guid id) => View(id);

        /// <summary>
        /// Obtiene el listado de estudiantes para ser usado en tablas
        /// </summary>
        /// <param name="fid">Identificador de la facultad</param>
        /// <param name="cid">identificador de la escuela profesionaal</param>
        /// <param name="academicOrder">Ciclo académico</param>
        /// <returns>Listado de estudiantes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStudents(Guid fid, Guid cid, int? academicOrder)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetStudentDatatableAcademicSituationWithOutComent(sentParameters, fid, cid, academicOrder);

            return Ok(result);

        }

        /// <summary>
        /// Genera el reporte del historial académico del estudiante
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("{id}/historial/reporte")]
        public async Task<IActionResult> RecordReport(Guid id)
        {
            var student = await _studentService.GetStudentWithCareerAndUser(id);
            if (id == Guid.Empty)
            {
                var userId = GetUserId();
                student = await _studentService.GetStudentByUser(userId);
            }

            var terms = (await _termService.GetTermsByStudentSections(student.Id)).OrderBy(x => x.Name).ToList();
            var currentTerm = await GetActiveTerm();
            var cumulative = 0.00M;
            var credits = 0;

            var studentTerms = await Task.WhenAll(terms.Select(async t =>
            {
                var ac = await _academicSummariesService.GetByStudentAndTerm(student.Id, t.Id); // Si es activo será NULL

                if (ac != null)
                    cumulative = cumulative == 0 ? ac.WeightedAverageGrade
                        : (credits + ac.TotalCredits != 0
                            ? ((cumulative * credits) + (ac.WeightedAverageGrade * ac.TotalCredits)) / (credits + ac.TotalCredits) : cumulative);

                var studentTerm = new StudentTermReportViewModel
                {
                    CareerName = ac?.Career.Name ?? student.Career.Name,
                    TermName = ac?.Term.Name ?? t.Name,
                    StudentSections = (await Task.WhenAll(t.Status == ConstantHelpers.TERM_STATES.ACTIVE
                                        ? (await _studentSectionService.GetAll(student.Id, t.Id))
                                        .Select(async x => new StudentSectionsViewModel
                                        {
                                            CourseName = x.Section.CourseTerm.Course.FullName,
                                            Credits = x.Section.CourseTerm.Course.Credits,
                                            Try = x.Try,
                                            NotDisapproved = x.Status != CORE.Helpers.ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED,
                                            FinalGrade = x.Status == CORE.Helpers.ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS ? "-" : (x.Status == CORE.Helpers.ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ? "Retirado" : x.FinalGrade.ToString()),
                                            AcademicYear = await _academicYearCourseService.GetLevelByCourseAndCurriculum(x.Section.CourseTerm.CourseId, student.CurriculumId)
                                                ?? await _academicYearCourseService.GetLevelByCourseAndCurriculum(x.Section.CourseTerm.CourseId) ?? 0
                                        }).ToList()
                                        : (await _academicHistoryService.GetAcademicHistoriesByStudent(student.Id, t.Id, false))
                                        .Select(async x => new StudentSectionsViewModel
                                        {
                                            CourseName = x.Course.FullName,
                                            Credits = x.Course.Credits,
                                            Try = x.Try,
                                            NotDisapproved = x.Approved,
                                            FinalGrade = x.Withdraw ? "Retirado" : x.Grade.ToString(),
                                            AcademicYear = await _academicYearCourseService.GetLevelByCourseAndCurriculum(x.CourseId, student.CurriculumId)
                                                ?? await _academicYearCourseService.GetLevelByCourseAndCurriculum(x.CourseId) ?? 0
                                        }).ToList())).ToList(),
                    AcademicSummary = new SummaryViewModel()
                    {
                        TotalCredits = (ac?.TotalCredits
                            ?? await _studentSectionService.GetTotalCreditsByStudentAndTerm(student.Id, t.Id)).ToString("0.0"),
                        ApprovedCredits = ac?.ApprovedCredits ?? 0,
                        WeightedFinalGrade = !ac?.TermHasFinished ?? true ? "-" : Math.Round(ac.WeightedAverageGrade, 2).ToString(),
                        CumulativeWeightedFinalGrade = !ac?.TermHasFinished ?? true ? "-" : Math.Round(cumulative, 2).ToString(),
                        Order = !ac?.TermHasFinished ?? true ? "-" : ac.MeritOrder.ToString(),
                        MeritOrder = !ac?.TermHasFinished ?? true ? "-" : ac.MeritOrder >= 0 ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.VALUES[ac.MeritType] : "",
                        Observations = !ac?.TermHasFinished ?? true ? "---" : string.IsNullOrEmpty(ac.Observations) ? "---" : ac.Observations,
                        StudentAcademicYear = ac?.StudentAcademicYear ?? student.CurrentAcademicYear
                    }
                };

                return studentTerm;
            }));

            var model = new RecordReportViewModel()
            {
                FacultyName = student.Career.Faculty.Name,
                CareerName = student.Career.Name,
                StudentCode = student.User.UserName,
                StudentFullName = student.User.FullName,
                ImagePath = Path.Combine(_hostingEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-alt.png"),
                CurrentTerm = currentTerm?.Name,
                Terms = studentTerms.ToList()
            };

            var viewToString = await _viewRenderService.RenderToStringAsync("/Views/Shared/Components/AcademicSummaryReport/Default.cshtml", model);
            var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css/pages/pdf/evaluationreport.css");

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 0, Left = 10, Right = 10 },
                },
                Objects =
                {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = viewToString,
                        WebSettings =
                        {
                            DefaultEncoding = "utf-8",
                            UserStyleSheet = cssPath
                        },
                        FooterSettings = {
                            FontName = "Arial",
                            FontSize = 9,
                            Line = false,
                            Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                            Center = "",
                            Right = "Pág: [page]/[toPage]"
                        }
                    }
                }
            };

            var fileByte = _dinkConverter.Convert(pdf);
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf", "Reporte de Notas.pdf");
        }
    }
}
