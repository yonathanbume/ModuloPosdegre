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
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System.Collections.Generic;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/historial")]
    public class AcademicSummaryController : BaseController
    {
        private IWebHostEnvironment _hostingEnvironment;
        public IConverter _dinkConverter { get; }
        private readonly IViewRenderService _viewRenderService;
        private readonly IStudentService _studentService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IAcademicYearCourseService _academicYearCourseService;
        private readonly IAcademicSummariesService _academicSummariesService;

        public AcademicSummaryController(IConverter dinkConverter,
            IViewRenderService viewRenderService,
            IWebHostEnvironment environment,
            IUserService userService,
            IStudentService studentService,
            IStudentSectionService studentSectionService,
            IAcademicHistoryService academicHistoryService,
            IAcademicYearCourseService academicYearCourseService,
            IAcademicSummariesService academicSummariesService,
            ITermService termService) : base(userService, termService)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
            _studentService = studentService;
            _studentSectionService = studentSectionService;
            _academicHistoryService = academicHistoryService;
            _academicYearCourseService = academicYearCourseService;
            _academicSummariesService = academicSummariesService;
        }

        /// <summary>
        /// Vista donde se detalla los periodos matriculados junto a su detalle (cursos y notas)
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el historial académico del estudiante logueado
        /// </summary>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene el historial académico</returns>
        [Route("periodo/{pid}/get")]
        public async Task<IActionResult> GetAcademicSummaryDetail(Guid pid)
        {
            if (pid.Equals(Guid.Empty))
                return BadRequest($"No se pudo encontrar un Periodo con el id {pid}.");

            var userId = _userService.GetUserIdByClaim(User);
            var term = await _termService.Get(pid);
            var student = await _studentService.GetStudentByUser(userId);

            var result = await Task.WhenAll(term.Status == ConstantHelpers.TERM_STATES.ACTIVE
                ? (await _studentSectionService.GetAll(student.Id, pid))
                .Select(async x => new
                {
                    course = x.Section.CourseTerm.Course.FullName,
                    credits = x.Section.CourseTerm.Course.Credits.ToString("0.0"),
                    @try = x.Try,
                    finalGrade = (x.Status == ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS) ? "-" : (x.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ? "Retirado" : x.FinalGrade.ToString()),
                    notDisapproved = x.Status != ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED,
                    academicYear = await _academicYearCourseService.GetLevelByCourseAndCurriculum(x.Section.CourseTerm.CourseId, student.CurriculumId)
                        ?? await _academicYearCourseService.GetLevelByCourseAndCurriculum(x.Section.CourseTerm.CourseId) ?? 0
                }).ToList()
                : (await _academicHistoryService.GetAcademicHistoriesByStudent(student.Id, pid, false))
                .Select(async x => new
                {
                    course = x.Course.FullName,
                    credits = x.Course.Credits.ToString("0.0"),
                    @try = x.Try,
                    finalGrade = x.Withdraw ? "Retirado" : x.Grade.ToString(),
                    notDisapproved = x.Approved,
                    academicYear = await _academicYearCourseService.GetLevelByCourseAndCurriculum(x.CourseId, student.CurriculumId)
                        ?? await _academicYearCourseService.GetLevelByCourseAndCurriculum(x.CourseId) ?? 0
                }).ToList());
            return Ok(result);
        }

        /// <summary>
        /// Genera un reporte donde se detalla el historial académico del estudiante logueado
        /// </summary>
        /// <returns>Archivo en formato PDF</returns>
        [Route("reporte")]
        public async Task<IActionResult> RecordReport()
        {
            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);
            var terms = (await _termService.GetTermsByStudentSections(student.Id)).OrderBy(x => x.Name);

            var currentTerm = await GetActiveTerm();
            var cumulative = 0.00M;
            var credits = 0;

            var studentTerms = new List<StudentTermReportViewModel>();
            foreach (var term in terms)
            {
                var ac = await _academicSummariesService.GetByStudentAndTerm(student.Id, term.Id); // Si es activo será NULL

                if (ac != null)
                    cumulative = cumulative == 0 ? ac.WeightedAverageGrade
                        : (credits + ac.TotalCredits != 0
                            ? ((cumulative * credits) + (ac.WeightedAverageGrade * ac.TotalCredits)) / (credits + ac.TotalCredits) : cumulative);

                var studentTerm = new StudentTermReportViewModel
                {
                    CareerName = ac?.Career.Name ?? student.Career.Name,
                    TermName = ac?.Term.Name ?? term.Name,
                };

                if (term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                {
                    studentTerm.StudentSections = (await _studentSectionService.GetAll(student.Id, term.Id))
                                       .Select(x => new StudentSectionsViewModel
                                       {
                                           CourseId = x.Section.CourseTerm.CourseId,
                                           CourseName = x.Section.CourseTerm.Course.FullName,
                                           Credits = x.Section.CourseTerm.Course.Credits,
                                           Try = x.Try,
                                           NotDisapproved = x.Status != CORE.Helpers.ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED,
                                           FinalGrade = x.Status == CORE.Helpers.ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS ? "-" : (x.Status == CORE.Helpers.ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ? "Retirado" : x.FinalGrade.ToString()),
                                       }).ToList();
                }
                else
                {
                    studentTerm.StudentSections = (await _academicHistoryService.GetAcademicHistoriesByStudent(student.Id, term.Id, false))
                                       .Select(x => new StudentSectionsViewModel
                                       {
                                           CourseId = x.CourseId,
                                           CourseName = x.Course.FullName,
                                           Credits = x.Course.Credits,
                                           Try = x.Try,
                                           NotDisapproved = x.Approved,
                                           FinalGrade = x.Withdraw ? "Retirado" : x.Grade.ToString()
                                       }).ToList();
                }

                foreach (var item in studentTerm.StudentSections)
                {
                    item.AcademicYear = await _academicYearCourseService.GetLevelByCourseAndCurriculum(item.CourseId, student.CurriculumId)
                                       ?? await _academicYearCourseService.GetLevelByCourseAndCurriculum(item.CourseId) ?? 0;
                }

                studentTerm.AcademicSummary = new SummaryViewModel()
                {
                    TotalCredits = (ac?.TotalCredits ?? await _studentSectionService.GetTotalCreditsByStudentAndTerm(student.Id, term.Id)).ToString("0.0"),
                    WeightedFinalGrade = !ac?.TermHasFinished ?? true ? "-" : Math.Round(ac.WeightedAverageGrade, 2).ToString(),
                    CumulativeWeightedFinalGrade = !ac?.TermHasFinished ?? true ? "-" : Math.Round(cumulative, 2).ToString(),
                    Order = !ac?.TermHasFinished ?? true ? "-" : ac.MeritOrder.ToString(),
                    MeritOrder = !ac?.TermHasFinished ?? true ? "-" : ac.MeritType >= 0 ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.VALUES[ac.MeritType] : "",
                    Observations = !ac?.TermHasFinished ?? true ? "---" : string.IsNullOrEmpty(ac.Observations) ? "---" : ac.Observations,
                    StudentAcademicYear = ac?.StudentAcademicYear ?? student.CurrentAcademicYear
                };

                studentTerms.Add(studentTerm);
            }

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
                    new DinkToPdf.ObjectSettings()
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
