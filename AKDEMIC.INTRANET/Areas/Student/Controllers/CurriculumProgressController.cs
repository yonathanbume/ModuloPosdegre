using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System.IO;
using AKDEMIC.INTRANET.ViewModels.AcademicSummaryViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using AKDEMIC.CORE.Services;
using System;
using AKDEMIC.CORE.Extensions;
using DinkToPdf.Contracts;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.INTRANET.Areas.Admin.Models.EnrollmentReservationViewModels;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/progreso")]
    public class CurriculumProgressController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IConverter _converter;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IViewRenderService _viewRenderService;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly IAcademicYearCourseService _academicYearCourseService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly ITermService _termService;
        private readonly AkdemicContext _context;
        private readonly IConfigurationService _configurationService;
        private readonly IEnrollmentReservationService _enrollmentReservationService;

        public CurriculumProgressController(IUserService userService,
            IStudentService studentService,
            IConverter converter,
            IWebHostEnvironment hostingEnvironment,
            IViewRenderService viewRenderService,
            IAcademicSummariesService academicSummariesService,
            IAcademicHistoryService academicHistoryService,
            IStudentSectionService studentSectionService,
            ITermService termService,
            AkdemicContext context,
            IConfigurationService configurationService,
            IAcademicYearCourseService academicYearCourseService,
            IEnrollmentReservationService enrollmentReservationService) : base(userService)
        {
            _studentService = studentService;
            _converter = converter;
            _hostingEnvironment = hostingEnvironment;
            _viewRenderService = viewRenderService;
            _academicSummariesService = academicSummariesService;
            _academicYearCourseService = academicYearCourseService;
            _academicHistoryService = academicHistoryService;
            _studentSectionService = studentSectionService;
            _termService = termService;
            _context = context;
            _configurationService = configurationService;
            _enrollmentReservationService = enrollmentReservationService;
        }

        /// <summary>
        /// Vista donde se muestra el progreso del alumno logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene los cursos de la malla curricular del alumno logueado
        /// </summary>
        /// <param name="aid">Año académico</param>
        /// <returns>Listado de cursos</returns>
        [Route("nivel/{aid}/get")]
        public async Task<IActionResult> GetAcademicYearDetail(byte? aid)
        {
            if (!aid.HasValue)
                return BadRequest($"No se pudo encontrar el año académico.");

            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);

            var result = (await _academicYearCourseService.GetAcademicYearDetailByStudentIdAndAcademicYearId(student.Id, aid.Value));

            return Ok(result);
        }

        /// <summary>
        /// Obtiene los cursos electivos del alumno logueado
        /// </summary>
        /// <returns>Listado de cursos electivos</returns>
        [Route("electivos/get")]
        public async Task<IActionResult> GetAcademicSituationElective()
        {
            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);

            var result = await _academicYearCourseService.GetAcademicSituationElectiveByStudentId(student.Id);
            return Ok(result);
        }

        [HttpGet("record-academico")]
        public async Task<IActionResult> AcademicRecord()
        {
            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);

            var model = new RecordReportViewModel();
            model.ImagePath = Path.Combine(_hostingEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png");

            string viewToString = "";
            string cssPath = "";

            var terms = (await _termService.GetTermsByStudentSectionsAndAcademicHistories(student.Id)).OrderBy(x => x.Name).ToList();
            terms = terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED).ToList();
            var currentTerm = await _termService.GetActiveTerm();

            var evaluationReportDateConfi = Convert.ToByte(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE));

            var studentTerms = new List<StudentTermReportViewModel>();

            foreach (var t in terms)
            {
                var ac = await _academicSummariesService.GetByStudentAndTerm(student.Id, t.Id); // Si es activo será NULL

                var studentTerm = new StudentTermReportViewModel
                {
                    CareerName = ac?.Career.Name ?? student.Career.Name,
                    TermName = ac?.Term.Name ?? t.Name,
                    StudentSections = (await Task.WhenAll(t.Status == ConstantHelpers.TERM_STATES.ACTIVE
                                       ? (await _studentSectionService.GetAll(student.Id, t.Id))
                                       .Select(async x => new StudentSectionsViewModel
                                       {
                                           CourseCode = x.Section.CourseTerm.Course.Code,
                                           CourseName = x.Section.CourseTerm.Course.Name,
                                           Credits = x.Section.CourseTerm.Course.Credits,
                                           Try = x.Try,
                                           NotDisapproved = x.Status != CORE.Helpers.ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED,
                                           FinalGrade = x.Status == CORE.Helpers.ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS ? "-" : (x.Status == CORE.Helpers.ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ? "Retirado" : x.FinalGrade.ToString()),
                                           AcademicYear = await _academicYearCourseService.GetLevelByCourseAndCurriculum(x.Section.CourseTerm.CourseId, student.CurriculumId)
                                               ?? await _academicYearCourseService.GetLevelByCourseAndCurriculum(x.Section.CourseTerm.CourseId) ?? 0
                                       }).ToList()
                                       : (await _academicHistoryService.GetAcademicHistoriesByStudent(student.Id, t.Id, null, true))
                                       .Select(async x => new StudentSectionsViewModel
                                       {
                                           CourseCode = x.Course.Code,
                                           CourseName = x.Course.Name,
                                           Credits = x.Course.Credits,
                                           Try = x.Try,
                                           NotDisapproved = x.Approved,
                                           Type = x.Type,
                                           Date = evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.CreatedAt ? x.EvaluationReport?.CreatedAt.ToLocalDateFormat() : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.ReceptionDate ? x.EvaluationReport?.ReceptionDate.ToLocalDateFormat() : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.LastGradePublished ? x.EvaluationReport?.LastGradePublishedDate.ToLocalDateFormat() : string.Empty,
                                           //Date = x.EvaluationReport?.ReceptionDate?.ToLocalDateFormat(),
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
                        CumulativeWeightedFinalGrade = !ac?.TermHasFinished ?? true ? "-" : Math.Round(ac.WeightedAverageCumulative, 2).ToString(),
                        Order = !ac?.TermHasFinished ?? true ? "-" : ac.MeritOrder.ToString(),
                        MeritOrder = !ac?.TermHasFinished ?? true ? "-" : ac.MeritOrder >= 0 ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.VALUES[ac.MeritType] : "",
                        Observations = !ac?.TermHasFinished ?? true ? "---" : string.IsNullOrEmpty(ac.Observations) ? "---" : ac.Observations,
                        StudentAcademicYear = ac?.StudentAcademicYear ?? student.CurrentAcademicYear
                    }
                };

                studentTerms.Add(studentTerm);
            }

            var specialEvaluationsActiveTerms = await _context.AcademicHistories.Where(x => x.StudentId == student.Id && x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE
                    && (x.Type == ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION || x.Type == ConstantHelpers.AcademicHistory.Types.SPECIAL || x.Type == ConstantHelpers.AcademicHistory.Types.REEVALUATION || x.Type == ConstantHelpers.AcademicHistory.Types.CONVALIDATION)
                )
                .Select(x => new StudentSectionsViewModel
                {
                    CourseCode = x.Course.Code,
                    CourseName = x.Course.Name,
                    Credits = x.Course.Credits,
                    Try = x.Try,
                    NotDisapproved = x.Approved,
                    Type = x.Type,
                    Date = evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.CreatedAt ? x.EvaluationReport.CreatedAt.ToLocalDateFormat() : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.ReceptionDate ? x.EvaluationReport.ReceptionDate.ToLocalDateFormat() : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.LastGradePublished ? x.EvaluationReport.LastGradePublishedDate.ToLocalDateFormat() : string.Empty,
                    //Date = x.EvaluationReport?.ReceptionDate?.ToLocalDateFormat(),
                    FinalGrade = x.Withdraw ? "Retirado" : x.Grade.ToString(),
                    AcademicYear = _context.AcademicYearCourses.Where(y=>y.CourseId == x.CourseId && y.CurriculumId == x.Student.CurriculumId).Select(y=>y.AcademicYear).FirstOrDefault()
                })
                    .ToListAsync();

            if (specialEvaluationsActiveTerms.Any())
            {
                var studentTerm = new StudentTermReportViewModel
                {
                    CareerName = student.Career.Name,
                    TermName = currentTerm.Name,
                    StudentSections = specialEvaluationsActiveTerms,
                    AcademicSummary = new SummaryViewModel()
                };

                studentTerms.Add(studentTerm);
            }

            model.StudentCurriculum = student.Curriculum.Code;
            model.FacultyName = student.Career.Faculty.Name;
            model.CareerName = student.Career.Name;
            model.StudentCode = student.User.UserName;
            model.StudentFullName = student.User.FullName;
            model.StudentDni = student.User.Dni;
            model.CurrentTerm = currentTerm?.Name;
            model.Terms = studentTerms.ToList();

            viewToString = await _viewRenderService.RenderToStringAsync("/Views/Shared/Components/AcademicSummaryReport/StudentDefault.cshtml", model);
            cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css/pages/pdf/evaluationreport.css");
            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPath },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Right = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "Página [page] de [toPage]"
                }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 }
                },
                Objects = { objectSettings }
            };

            var fileByte = _converter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf", "Reporte de Notas.pdf");
        }

        [HttpGet("reservas-matricula/get")]
        public async Task<IActionResult> GetEnrollmentReservations()
        {
            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);
            var data = await _enrollmentReservationService.GetEnrollmentReservationsByStudent(student.Id);

            var result = data
                .Select(x => new
                {
                    startDate = x.CreatedAt.ToLocalDateFormat(),
                    endDate = x.ExpirationDate.ToLocalDateFormat(),
                    term = x.Term.Name,
                    file = x.FileURL
                })
                .ToList();

            return Ok(result);
        }

        //[HttpGet("reservas-matricula/descargar/{id}")]
        //public async Task<IActionResult> DownloadEnrollmentReservationPDF(Guid id)
        //{
        //    try
        //    {
        //        var viewModel = new EnrollmentReservationPDF();
        //        var enrollmentReservation = await _enrollmentReservationService.Get(id);

        //        var userId = _userService.GetUserIdByClaim(User);
        //        var student = await _studentService.GetStudentByUser(userId);
        //        //var student = await _studentService.GetWithIncludes(enrollmentReservation.StudentId);
        //        if (student.Id != enrollmentReservation.StudentId)
        //            return BadRequest("No se pudo descargar el archivo");

        //        viewModel.FullName = student.User.FullName;
        //        viewModel.Dni = student.User.Document ?? student.User.Dni;
        //        viewModel.UserName = student.User.UserName;
        //        viewModel.Curriculum = student.Curriculum.Name;
        //        viewModel.AcademicProgram = student.AcademicProgram != null ? student.AcademicProgram.Name : "--";
        //        viewModel.CurrentAcademicYear = $"{student.CurrentAcademicYear}";
        //        viewModel.Observation = enrollmentReservation.Observation;
        //        viewModel.Date = enrollmentReservation.ExpirationDate.ToLocalDateFormat();

        //        var viewToString = "";

        //        viewToString = await _viewRenderService.RenderToStringAsync($"/Areas/Admin/Views/EnrollmentReservation/PDF.cshtml", viewModel);

        //        var objectSettings = new DinkToPdf.ObjectSettings
        //        {
        //            PagesCount = true,
        //            HtmlContent = viewToString,
        //            WebSettings = { DefaultEncoding = "utf-8" },
        //            FooterSettings = {
        //                FontName = "Arial",
        //                FontSize = 9,
        //                Line = false,
        //                Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
        //                Center = "",
        //                Right = "Pág: [page]/[toPage]"
        //            }
        //        };

        //        var globalSettings = new DinkToPdf.GlobalSettings
        //        {
        //            ColorMode = DinkToPdf.ColorMode.Color,
        //            Orientation = DinkToPdf.Orientation.Portrait,
        //            PaperSize = DinkToPdf.PaperKind.A4,
        //            Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 30, Left = 10, Right = 10 }
        //        };

        //        var pdf = new DinkToPdf.HtmlToPdfDocument
        //        {
        //            GlobalSettings = globalSettings,
        //            Objects = { objectSettings }
        //        };

        //        var fileByte = _converter.Convert(pdf);

        //        HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

        //        return File(fileByte, "application/pdf", "archivo.pdf");
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest("No se pudo descargar el archivo");
        //    }
        //}
    }
}
