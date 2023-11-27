using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.INTRANET.Areas.Admin.Models.AcademicSituationViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Model;
using AKDEMIC.INTRANET.ViewModels.AcademicSummaryViewModels;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using ClosedXML.Excel;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + 
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + 
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," + 
        ConstantHelpers.ROLES.DEGREES_AND_TITLES + "," + 
        ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + 
        ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + 
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF)]
    [Area("Admin")]
    [Route("admin/situacion-academica")]
    public class AcademicSituationController : BaseController
    {
        private readonly AppCustomSettings _appConfig;
        private readonly ITextSharpService _pdfSharpService;
        private readonly IStudentService _studentService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IAcademicYearCourseService _academicYearCourseService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly ICurriculumService _curriculumService;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly ICareerService _careerService;
        private readonly IAcademicYearCreditService _academicYearCreditService;
        private readonly IFacultyService _facultyService;
        private readonly IConfigurationService _configurationService;
        public IConverter _dinkConverter { get; }
        private readonly IViewRenderService _viewRenderService;
        private readonly AkdemicContext _context;
        private IWebHostEnvironment _hostingEnvironment;
        protected ReportSettings _reportSettings;

        public AcademicSituationController(
            IOptions<AppCustomSettings> optionsAccessor,
            IUserService userService,
            IConfigurationService configurationService,
            ITermService termService,
            ITextSharpService pdfSharpService,
            IStudentService studentService,
            IStudentSectionService studentSectionService,
            IAcademicYearCourseService academicYearCourseService,
            IAcademicSummariesService academicSummariesService,
            IAcademicHistoryService academicHistoryService,
            ICurriculumService curriculumService,
            IConverter dinkConverter,
            IViewRenderService viewRenderService,
            AkdemicContext context,
            IWebHostEnvironment environment,
            IDataTablesService dataTablesService,
            ICareerService careerService,
            IAcademicYearCreditService academicYearCreditService,
            IFacultyService facultyService,
            IOptionsSnapshot<ReportSettings> reportSettings
        ) : base(termService, userService, dataTablesService)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _context = context;
            _hostingEnvironment = environment;
            _pdfSharpService = pdfSharpService;
            _studentService = studentService;
            _academicHistoryService = academicHistoryService;
            _curriculumService = curriculumService;
            _studentSectionService = studentSectionService;
            _academicYearCourseService = academicYearCourseService;
            _academicSummariesService = academicSummariesService;
            _configurationService = configurationService;
            if (optionsAccessor == null) throw new ArgumentNullException(nameof(optionsAccessor));
            _appConfig = optionsAccessor.Value;
            _reportSettings = reportSettings.Value;
            _careerService = careerService;
            _academicYearCreditService = academicYearCreditService;
            _facultyService = facultyService;
        }

        /// <summary>
        /// Vista donde se muestra el listado de estudiantes según cuadro de mérito
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Vista donde se muestra el historial académico del estudiante
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("{id}/historial")]
        public IActionResult AcademicSummary(Guid id)
        {
            return View(id);
        }

        /// <summary>
        /// Vista donde se muestra el progreso curricular del estudiante
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("{id}/situacion")]
        public IActionResult CurriculumProgress(Guid id)
        {
            return View(id);
        }

        /// <summary>
        /// Obtiene el listado de estudiantes según los filtros del usuario
        /// </summary>
        /// <param name="tid">Identificador del periodo académico</param>
        /// <param name="fid">Identificador de la facultad</param>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="year">Ciclo de estudio</param>
        /// <param name="academicOrder">Orden de mérito</param>
        /// <returns>Listado de estudiantes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStudents(Guid tid, Guid fid, Guid cid, string searchValue, Guid curriculumId, int? year = null, int? academicOrder = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetStudentDatatableAcademicSituation(sentParameters, tid, fid, cid, searchValue, academicOrder, User, curriculumId, year);
            return Ok(result);
        }

        /// <summary>
        /// Genera el reporte de situación académica
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <returns>Archivo en formato PDF</returns>
        [Route("{id}/historial/reporte")]
        public async Task<IActionResult> RecordReport(Guid id)
        {
            var bytess = await this.GetFunction<AcademicSituationController, byte[]>(_appConfig.AcademicSituationRecordReport, id);
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(bytess, "application/pdf", "Reporte de Notas.pdf");
        }

        /// <summary>
        /// Genera el cuadro de mérito
        /// </summary>
        /// <param name="tid">Identificador del periodo académico</param>
        /// <param name="fid">Identificador de la facultad</param>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="year">Año</param>
        /// <param name="academicOrder">Orden de mérito</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("reporte-pdf")]
        public async Task<IActionResult> DownloadMeritChartPdf(Guid tid, Guid fid, Guid cid, Guid curriculumId, byte year, int? academicOrder = null)
        {
            var term = await _termService.Get(tid);
            var career = await _careerService.Get(cid);
            var faculty = await _facultyService.Get(fid);
            var curriculum = await _curriculumService.Get(curriculumId);
            var academicYearCredits = await _academicYearCreditService.Get(curriculumId, year);

            var summaries = await _academicSummariesService.GetMeritChartAcademicSummaries(tid, fid, cid, curriculumId, year, academicOrder, User);

            var model = new MeritChartPdfViewModel
            {
                Image1 = Path.Combine(_hostingEnvironment.WebRootPath, @"images/escudo.png"),
                Image2 = Path.Combine(_hostingEnvironment.WebRootPath, @"images/themes/" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png"),
                AcademicYear = ConstantHelpers.ACADEMIC_YEAR.TEXT[year],
                Career = career.Name,
                Faculty = faculty.Name,
                Term = term.Name,
                Students = summaries
                    .Select(x => new MeritChartPdfStudentViewModel
                    {
                        Number = x.TotalOrder,
                        MeritOrder = x.MeritOrder,
                        Code = x.Student.User.UserName,
                        Name = x.Student.User.FullName,
                        AcademicProgram = x.Student.AcademicProgram.Code,
                        AverageGrade = x.WeightedAverageGrade,
                        Curriculum = curriculum.Code,
                        CurriculumCredits = academicYearCredits.Credits,
                        EnrolledCredits = x.TotalCredits,
                        ApprovedCredits = x.ApprovedCredits,
                        DisapprovedCredits = x.TotalCredits - x.ApprovedCredits,
                        Modality = x.Student.AcademicHistories.Where(y => y.TermId == tid).OrderByDescending(y => y.Try).FirstOrDefault().Try <= 2
                        ? ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES[1]
                        : ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES[x.Student.AcademicHistories.Where(y => y.TermId == tid).OrderByDescending(y => y.Try).FirstOrDefault().Try],
                        Condition = x.TotalCredits == x.ApprovedCredits ? "INVICTO" : $"{x.TotalCredits - x.ApprovedCredits} CRD. DESAP."
                    }).ToList()
            };

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 8, Bottom = 8, Left = 8, Right = 8 },
                DocumentTitle = $"MERITO {term.Name} {career.Code} {curriculum.Code} {year}",
                DPI = 380
            };

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/AcademicSituation/MeritChartPdf.cshtml", model);
            //var cssPath = @"file:///" + System.IO.Path.Combine(_hostingEnvironment.WebRootPath, @"css/pages/pdf/arial-family.css");

            var printTime = DateTime.UtcNow.ToDefaultTimeZone();

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8"/*, UserStyleSheet = cssPath*/ },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 8,
                    Line = true,
                    Left = $"Fecha : {printTime.Day} de {ConstantHelpers.MONTHS.VALUES[printTime.Month]} del {printTime.Year} - Hora: {printTime:H:mm:ss} hrs.",
                    Center = "",
                    Right = "Pag: [page]/[toPage]"
                }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileByte = _dinkConverter.Convert(pdf);
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf", globalSettings.DocumentTitle + ".pdf");
        }

        /// <summary>
        /// Genera el reporte de situación académica
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="academicOrder">Orden de mérito</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [Route("reporte-excel")]
        public async Task<IActionResult> DownloadExcelReport(Guid? termId, Guid? facultyId, Guid? careerId, int? academicOrder = null)
        {
            var data = await _studentService.GetStudentAcademicSituationExcelTemplate(termId, facultyId, careerId, academicOrder, User);
            var dt = new DataTable
            {
                TableName = "Reporte de Situación Academica"
            };

            dt.Columns.Add("Estudiante");
            dt.Columns.Add("Dni");
            dt.Columns.Add("Carrera");
            dt.Columns.Add("Facultad");
            dt.Columns.Add("Orden de Mérito");
            dt.Columns.Add("Promedio Ponderado");
            dt.Columns.Add("Clasificación");
            dt.Columns.Add("Creditos Aprobados");

            foreach (var student in data)
                dt.Rows.Add(student.Name, student.Dni,
                    student.Career,
                    student.Faculty,
                    student.LastOrder,
                    student.AprrovedCredits,
                    student.LastOrder,
                    student.LastMeritOrder);

            dt.AcceptChanges();

            var img = Path.Combine(_hostingEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png");
            var fileName = $"Reporte de Situación Academica.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Listado de Estudiantes", img);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }

        [HttpGet("acumulado")]
        public IActionResult Acumulative()
        {
            return View();
        }

        /// <summary>
        /// Vista donde se muestra el listado de estudiantes con su promedio ponderado acumulado
        /// </summary>
        /// <param name="tid">Identificador del periodo académico</param>
        /// <param name="fid">Identificador de la facultad</param>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="academicOrder">Orden de mérito</param>
        /// <returns>Vista</returns>
        [HttpGet("acumulado/get")]
        public async Task<IActionResult> GetOrderStudentsCumulative(Guid tid, Guid fid, Guid cid, string searchValue, int? academicOrder = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetStudentDatatableAcademicSituationCumulative(sentParameters, tid, fid, cid, searchValue, academicOrder, User);
            return Ok(result);
        }
        #region PRIVATES
        /// <summary>
        /// Formato 1 de situación académica
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <returns>Arreglo de bytes</returns>
        private async Task<byte[]> StandardFormat(Guid id)
        {
            var student = await _studentService.GetStudentWithCareerAndUser(id);
            if (id == Guid.Empty)
            {
                var userId = GetUserId();
                student = await _studentService.GetStudentByUser(userId);
            }

            var model = new RecordReportViewModel();
            model.ImagePath = Path.Combine(_hostingEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png");

            string viewToString = "";
            string cssPath = "";

            var terms = (await _termService.GetTermsByStudentSections(student.Id)).OrderBy(x => x.Name).ToList();
            var currentTerm = await GetActiveTerm();

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
                        CumulativeWeightedFinalGrade = !ac?.TermHasFinished ?? true ? "-" : Math.Round(ac.WeightedAverageCumulative, 2).ToString(),
                        Order = !ac?.TermHasFinished ?? true ? "-" : ac.MeritOrder.ToString(),
                        MeritOrder = !ac?.TermHasFinished ?? true ? "-" : ac.MeritOrder >= 0 ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.VALUES[ac.MeritType] : "",
                        Observations = !ac?.TermHasFinished ?? true ? "---" : string.IsNullOrEmpty(ac.Observations) ? "---" : ac.Observations,
                        StudentAcademicYear = ac?.StudentAcademicYear ?? student.CurrentAcademicYear
                    }
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

            viewToString = await _viewRenderService.RenderToStringAsync("/Views/Shared/Components/AcademicSummaryReport/Default.cshtml", model);
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
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
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

            var fileByte = _dinkConverter.Convert(pdf);

            if (!User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF) && !User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD))
            {
                if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAB && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAH)
                    _pdfSharpService.AddWatermarkToAllPages(ref fileByte, "Solo Lectura", 130);
            }
            //return View("~/Views/Shared/Components/AcademicSummaryReport/ReportUNJBG.cshtml",model);
            return fileByte;
        }

        /// <summary>
        /// Formato 2 de situación académica
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <returns>Arreglo de bytes</returns>
        private async Task<byte[]> ReportFormat1(Guid id)
        {
            var student = await _studentService.GetStudentWithCareerAndUser(id);
            if (id == Guid.Empty)
            {
                var userId = GetUserId();
                student = await _studentService.GetStudentByUser(userId);
            }

            var model = new RecordReportViewModel();
            model.HeaderText = _reportSettings.AcademicSituationReportFormat1HeaderText;
            model.ImagePath = Path.Combine(_hostingEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png");

            string viewToString = "";
            string cssPath = "";
            string footer = "";

            var academicYearCourses = await _academicYearCourseService.GetWithHistoriesByCurriculumAndStudent(student.CurriculumId, student.Id);
            var academicYears = academicYearCourses
                .GroupBy(x => x.AcademicYear)
                .ToList();
            var curriculum = await _curriculumService.Get(student.CurriculumId);

            model.FacultyName = student.Career.Faculty.Name;
            model.CareerName = student.Career.Name;
            model.StudentCode = student.User.UserName;
            model.StudentFullName = student.User.FullName;
            model.StudentDni = student.User.Dni;
            model.Regime = ConstantHelpers.CURRENT_COMPUTERS_TYPES.VALUES.ContainsKey(curriculum.RegimeType)
                ? ConstantHelpers.CURRICULUM.REGIME_TYPE.VALUES[curriculum.RegimeType] : "-";
            model.CurriculumCode = curriculum != null ? curriculum.Code : "-";
            model.Modality = "PRESENCIAL";
            model.AcademicYears = academicYears.Select(x => new StudentAcademicYearViewModel
            {
                AcademicYearName = ConstantHelpers.ACADEMIC_YEAR.TEXT[x.Key].ToUpper(),
                Courses = x.Select(c => new StudentCourseViewModel
                {
                    CourseName = c.Course.Name,
                    CourseCode = c.Course.Code,
                    Credits = c.Course.Credits,
                    ActType = "FINAL",
                    AcademicHistories = c.Course.AcademicHistories?
                    .OrderBy(y => y.Term.Name)
                    .Select(ah => new AcademicHistoryViewModel
                    {
                        FinalGrade = ah.Grade,
                        TermName = ah.Term.Name,
                        IsValidated = ah.Validated,
                        Approbed = ah.Approved,
                        Withdraw = ah.Withdraw,
                        FinalGradeDateTime = c.Course.EvaluationReports.Select(er => er.CreatedAt).FirstOrDefault()
                    }).ToList()
                }).ToList()
            }).ToList();

            viewToString = await _viewRenderService.RenderToStringAsync("/Views/Shared/Components/AcademicSummaryReport/ReportUNJBG.cshtml", model);
            cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css/pages/pdf/unjbg/academicsummaryreportpdf.css");


            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPath },
                FooterSettings = new DinkToPdf.FooterSettings
                {
                    FontName = "Arial",
                    FontSize = 8,
                    Line = true,
                    Left = $"Fecha de Impresion : {DateTime.UtcNow.ToLocalDateFormat()} a las {DateTime.UtcNow.ToLocalTimeFormat()}",
                    Right = "Pág [page]",
                    Spacing = -5
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

            var fileByte = _dinkConverter.Convert(pdf);
            if (!User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF) && !User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD))
            {
                if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAB && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAH)
                    _pdfSharpService.AddWatermarkToAllPages(ref fileByte, "Solo Lectura", 130);
            }

            //return View("~/Views/Shared/Components/AcademicSummaryReport/ReportUNJBG.cshtml",model);
            return fileByte;
        }
        #endregion
    }
}
