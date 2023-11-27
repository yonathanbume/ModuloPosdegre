using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Teacher.Models.GradeViewModels;
using AKDEMIC.INTRANET.Areas.Teacher.Models.GradeViewModels.PdfViewModel;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Model;
using AKDEMIC.INTRANET.Services.EvaluationReportGenerator;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using ClosedXML.Excel;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ConvertHelpers = AKDEMIC.CORE.Helpers.ConvertHelpers;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS + "," + ConstantHelpers.ROLES.SUPERADMIN)]
    [Area("Teacher")]
    [Route("profesor/notas")]
    public class GradeController : BaseController
    {
        private readonly AppCustomSettings _appConfig;
        private IWebHostEnvironment _hostingEnvironment;
        public IConverter _dinkConverter { get; }

        private readonly IEvaluationReportGeneratorService _evaluationReportGeneratorService;
        private readonly IViewRenderService _viewRenderService;
        private readonly ISectionService _sectionService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly ICourseTermService _courseTermService;
        private readonly IEvaluationService _evaluationService;
        private readonly ICourseService _courseService;
        private readonly IEvaluationReportService _evaluationReportService;
        private readonly ICurriculumService _curriculumService;
        private readonly IScoreInputScheduleService _scoreInputScheduleService;
        private readonly IAcademicYearCourseService _academicYearCourseService;
        private readonly ITeacherSectionService _teacherSectionService;
        private readonly IGradeService _gradeService;
        private readonly ITemporalGradeService _temporalGradeService;
        private readonly IGradeRegistrationService _gradeRegistrationService;
        private readonly IClassScheduleService _classScheduleService;
        private readonly IClassService _classService;
        private readonly IClassStudentService _classStudentService;
        private readonly ICourseUnitService _courseUnitService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly ISubstituteExamService _substituteExamService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _accessor;
        private readonly IConfigurationService _configurationService;
        private readonly IStudentService _studentService;
        protected ReportSettings _reportSettings;

        public GradeController(
            IOptions<AppCustomSettings> optionsAccessor,
            AkdemicContext context,
            IUserService userService,
            IEvaluationReportGeneratorService evaluationReportGeneratorService,
            ITermService termService,
            IConverter dinkConverter,
            IViewRenderService viewRenderService,
            IWebHostEnvironment environment,
            ISectionService sectionService,
            IClassService classService,
            IClassScheduleService classScheduleService,
            IClassStudentService classStudentService,
            IStudentSectionService studentSectionService,
            ICourseTermService courseTermService,
            IEvaluationService evaluationService,
            ICourseService courseService,
            IEvaluationReportService evaluationReportService,
            ICurriculumService curriculumService,
            IScoreInputScheduleService scoreInputScheduleService,
            ITeacherSectionService teacherSectionService,
            IGradeService gradeService,
            ITemporalGradeService temporalGradeService,
            ICourseUnitService courseUnitService,
            IAcademicHistoryService academicHistoryService,
            IAcademicSummariesService academicSummariesService,
            IGradeRegistrationService gradeRegistrationService,
            ISubstituteExamService substituteExamService,
            IHttpContextAccessor httpContextAccessor,
            IActionContextAccessor accessor,
            IConfigurationService configurationService,
            IStudentService studentService,
            IAcademicYearCourseService academicYearCourseService,
            IOptionsSnapshot<ReportSettings> reportSettings
        ) : base(context, userService, termService)
        {
            _temporalGradeService = temporalGradeService;
            _evaluationReportGeneratorService = evaluationReportGeneratorService;
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
            _teacherSectionService = teacherSectionService;
            _sectionService = sectionService;
            _studentSectionService = studentSectionService;
            _courseTermService = courseTermService;
            _classService = classService;
            _classScheduleService = classScheduleService;
            _classStudentService = classStudentService;
            _evaluationService = evaluationService;
            _courseService = courseService;
            _evaluationReportService = evaluationReportService;
            _curriculumService = curriculumService;
            _scoreInputScheduleService = scoreInputScheduleService;
            _gradeRegistrationService = gradeRegistrationService;
            _gradeService = gradeService;
            _academicYearCourseService = academicYearCourseService;
            _courseUnitService = courseUnitService;
            _academicHistoryService = academicHistoryService;
            _academicSummariesService = academicSummariesService;
            _substituteExamService = substituteExamService;
            _httpContextAccessor = httpContextAccessor;
            _accessor = accessor;
            _configurationService = configurationService;
            _studentService = studentService;
            if (optionsAccessor == null) throw new ArgumentNullException(nameof(optionsAccessor));
            _appConfig = optionsAccessor.Value;
            _reportSettings = reportSettings.Value;
        }

        /// <summary>
        /// Vista principal donde se listan las secciones asignadas al docente logueado.
        /// Secciones habilitadas en el periodo activo.
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            var confiAuxiliaryEvaluation = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.ENABLED_AUXILIARY_EVALUATION_REPORT);
            ViewBag.AuxiliaryEvaluationReport = bool.Parse(confiAuxiliaryEvaluation);
            return View();
        }

        /// <summary>
        /// Obtiene el listado de secciones asignadas al docente logueado.
        /// </summary>
        /// <param name="term">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene el listado de secciones</returns>
        [Route("secciones/get")]
        public async Task<IActionResult> GetSections(Guid term)
        {
            var userId = GetUserId();
            var sections = await _sectionService.GetAll(userId, null, term, null, null, true);

            var result = sections.Select(x => new
            {
                career = x.CourseTerm.Course.CareerId.HasValue ? x.CourseTerm.Course.Career.Name : "---",
                credits = x.CourseTerm.Course.Credits,
                course = x.CourseTerm.Course.Name,
                section = x.Code,
                term = x.CourseTerm.Term.Name,
                courseId = x.CourseTerm.Course.Id,
                sectionId = x.Id
            }).ToList();

            return Json(result);
        }

        [HttpGet("secciones/{sectionId}/registro-auxiliar-evaluacion")]
        public async Task<IActionResult> GetAuxiliaryEvaluationReportPDF(Guid sectionId)
        {
            var section = await _context.Sections.Where(x => x.Id == sectionId)
                .Select(x => new
                {
                    career = x.CourseTerm.Course.Career.Name,
                    term = x.CourseTerm.Term.Name,
                    curriculum = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Code).FirstOrDefault(),
                    course = x.CourseTerm.Course.Name,
                    code = x.CourseTerm.Course.Code,
                    credits = x.CourseTerm.Course.Credits,
                    x.IsDirectedCourse
                })
                .FirstOrDefaultAsync();

            var teacherSections = await _context.TeacherSections.Where(x => x.SectionId == sectionId)
                .Select(x => new
                {
                    x.Teacher.User.FullName,
                    academicDepartment = x.Teacher.AcademicDepartment.Name
                })
                .ToListAsync();

            var students = await _context.StudentSections.Where(x => x.SectionId == sectionId)
                .Select(x => new Row
                {
                    Names = x.Student.User.FullName,
                    Code = x.Student.User.UserName,
                    Email = x.Student.User.Email,
                    PhoneNumber = x.Student.User.PhoneNumber
                })
                .ToListAsync();

            var model = new ActaScoresViewModel
            {
                Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                BasicInformation = new BasicInformation
                {
                    Career = section.career,
                    Term = section.term,
                    Curriculum = section.curriculum,
                    Course = section.course,
                    Type = section.IsDirectedCourse ? "DIRIGIDO" : "REGULAR",
                    CourseCode = section.code,
                    Credits = section.credits.ToString(),
                    Teacher = teacherSections.Count() > 1 ? "CARGA COMPARTIDA" : teacherSections.Select(x => x.FullName).FirstOrDefault(),
                    TeacherAcademicDepartment = teacherSections.Count() > 1 ? "CARGA COMPARTIDA" : teacherSections.Select(x => x.academicDepartment).FirstOrDefault()
                },
                Rows = students
            };

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 }
            };
            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Views/Grade/AuxiliaryEvaluationReportPDF.cshtml", model);

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = {
                        FontSize = 8,
                        Right = $"{DateTime.UtcNow.ToLocalDateTimeFormat()}",
                        Left = sectionId.ToString(),
                        Center = "Pagina [page] de [toPage]"
                    }
            };
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var fileByte = _dinkConverter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileByte, "application/pdf", "Registro Auxiliar de Evaluaciones.pdf");
        }

        [HttpGet("secciones/{sectionId}/registro-auxiliar-evaluacion-excel")]
        public async Task<IActionResult> GetAuxiliaryEvaluationReportEXCEL(Guid sectionId)
        {
            var section = await _context.Sections.Where(x => x.Id == sectionId)
                .Select(x => new
                {
                    career = x.CourseTerm.Course.Career.Name,
                    term = x.CourseTerm.Term.Name,
                    curriculum = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Code).FirstOrDefault(),
                    course = x.CourseTerm.Course.Name,
                    code = x.CourseTerm.Course.Code,
                    credits = x.CourseTerm.Course.Credits,
                    sectionCode = x.Code,
                    x.IsDirectedCourse
                })
                .FirstOrDefaultAsync();

            var teacherSections = await _context.TeacherSections.Where(x => x.SectionId == sectionId)
                .Select(x => new
                {
                    x.Teacher.User.FullName,
                    academicDepartment = x.Teacher.AcademicDepartment.Name
                })
                .ToListAsync();

            var students = await _context.StudentSections.Where(x => x.SectionId == sectionId)
                .Select(x => new Row
                {
                    Names = x.Student.User.FullName,
                    Code = x.Student.User.UserName,
                    Email = x.Student.User.Email,
                    PhoneNumber = x.Student.User.PhoneNumber
                })
                .ToListAsync();

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Nº");
            dt.Columns.Add("Código");
            dt.Columns.Add("Apellidos y Nombres");
            dt.Columns.Add("Correo Electrónico");
            dt.Columns.Add("Nota");

            for (int i = 0; i < students.Count(); i++)
                dt.Rows.Add((i + 1), students[i].Code, students[i].Names, students[i].Email, string.Empty);

            dt.AcceptChanges();

            var fileName = $"REGISTRO DE EVALUACIÓN PERMANENTE.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);

                ws.Row(1).InsertRowsAbove(10);

                var mergeRangeColumn = 'E';

                ws.Cell(2, 2).Value = GeneralHelpers.GetInstitutionName().ToUpper();
                ws.Cell(2, 2).Style.Font.FontSize = 12;
                ws.Cell(2, 2).Style.Font.Bold = true;
                ws.Cell(2, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(2, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range($"B2:{mergeRangeColumn}2").Merge();

                ws.Cell(3, 2).Value = "REGISTRO DE EVALUACIÓN PERMANENTE";
                ws.Cell(3, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(3, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range($"B3:{mergeRangeColumn}3").Merge();

                ws.Cell(5, 1).Value = "ESCUELA PROFESIONAL";
                ws.Cell(5, 1).Style.Font.Bold = true;
                ws.Cell(5, 2).Value = $"{section.career}";

                ws.Cell(5, 4).Value = "SEMESTRE";
                ws.Cell(5, 4).Style.Font.Bold = true;
                ws.Cell(5, 5).Value = $"'{section.term}";

                ws.Cell(6, 1).Value = "TIPO";
                ws.Cell(6, 1).Style.Font.Bold = true;
                ws.Cell(6, 2).Value = section.IsDirectedCourse ? "DIRIGIDO" : "REGULAR";

                ws.Cell(6, 4).Value = "PLAN DE ESTUDIO";
                ws.Cell(6, 4).Style.Font.Bold = true;
                ws.Cell(6, 5).Value = $"'{section.curriculum}";

                ws.Cell(7, 1).Value = "CURSO";
                ws.Cell(7, 1).Style.Font.Bold = true;
                ws.Cell(7, 2).Value = $"{section.course}";

                ws.Cell(7, 4).Value = "SIGLA";
                ws.Cell(7, 4).Style.Font.Bold = true;
                ws.Cell(7, 5).Value = $"'{section.code}";


                ws.Cell(8, 1).Value = "DPTO. ACADÉMICO";
                ws.Cell(8, 1).Style.Font.Bold = true;
                ws.Cell(8, 2).Value = teacherSections.Count() > 1 ? "CARGA COMPARTIDA" : teacherSections.Select(x => x.academicDepartment).FirstOrDefault();

                ws.Cell(8, 4).Value = "CRÉDITOS";
                ws.Cell(8, 4).Style.Font.Bold = true;
                ws.Cell(8, 5).Value = $"'{section.credits}";

                ws.Rows().AdjustToContents();
                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        /// <summary>
        /// Obtiene el listado de evaluaciones
        /// </summary>
        /// <param name="id">Identificador de la sección</param>
        /// <returns>Objeto que contiene el listado de evaluaciones</returns>
        [Route("getevaluaciones/{id}")]
        public async Task<IActionResult> GetEvaluations(Guid id)
        {
            var section = await _sectionService.Get(id);
            var evaluations = await _evaluationService.GetAllByCourseTerm(section.CourseTermId);
            var result = evaluations.Select(x => new
            {
                evaluation = x.Name,
                percentage = x.Percentage,
                evaluationId = x.Id,
                sectionId = id
            }).ToList();
            return Json(result);
        }

        /// <summary>
        /// Método que genera la pre-acta de una sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Archivo en formato PDF</returns>
        //Pre-Acta 
        [Route("pre-acta/{sectionId}")]
        public async Task<IActionResult> PreEvaluationReport(Guid sectionId)
        {
            try
            {
                var userId = GetUserId();
                var section = await _sectionService.GetWithTeacherSections(sectionId);
                if (section == null)
                    return BadRequest();

                var courseTerm = await _courseTermService.GetCourseTermWithCourse(section.CourseTermId);
                // if (section.TeacherSections.Any(ts => ts.TeacherId == userId)) return Unauthorized();

                var currentCurriculum = await _curriculumService.GetCareerLastCurriculum((Guid)courseTerm.Course.CareerId);

                var evaluations = await _evaluationService.GetAllByCourseAndTermWithTaken(courseTerm.CourseId, courseTerm.TermId, sectionId);
                var students = await _studentSectionService.GetAllWithGradesAndEvaluations(null, null, sectionId);

                var model = new PreEvaluationReportViewModel
                {
                    CourseCode = section.CourseTerm.Course.Code,
                    CourseName = section.CourseTerm.Course.Name,
                    Credits = section.CourseTerm.Course.Credits,
                    Teacher = section.TeacherSections.Any() ? string.Join(", ", section.TeacherSections.Select(ts => ts.Teacher.User.RawFullName)) : "No Asignado",
                    Term = section.CourseTerm.Term.Name,
                    Section = section.Code,
                    Carrer = section.CourseTerm.Course?.Career?.Name ?? "---",
                    Faculty = section.CourseTerm.Course.Career.Faculty.Name,
                    AcademicYear = (byte)(await _academicYearCourseService.GetLevelByCourseAndCurriculum(courseTerm.CourseId, currentCurriculum.Id) ?? 0),
                    Year = Convert.ToByte(Math.Round((Convert.ToDecimal(await _academicYearCourseService.GetLevelByCourseAndCurriculum(courseTerm.CourseId, currentCurriculum.Id) ?? 0)
                        / Convert.ToDecimal(2)), MidpointRounding.AwayFromZero))
                };

                model.ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{@GeneralHelpers.GetTheme()}/logo-report.png");
                model.Students = students
                    .Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                    .Select(x => new PreEvaluationReportViewModel.StudentPreGradeViewModel
                    {
                        Code = x.Student.User.UserName,
                        FullName = x.Student.User.FullName,
                        Grades = x.Section.CourseTerm.Evaluations.Select(ev => x.Grades.Any(g => g.EvaluationId == ev.Id)
                            ? x.Grades.Where(g => g.EvaluationId == ev.Id).Select(g => g.Value).FirstOrDefault() : -1).ToList()
                    }).ToList();

                model.Evaluations = evaluations.Select(x => new PreEvaluationReportViewModel.EvaluationViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Percentage = x.Percentage,
                    Taken = x.Taken
                }).OrderBy(x => x.Id).ToList();
                var globalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 0, Left = 10, Right = 10 }
                };
                var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Views/Grade/PreEvaluationReport.cshtml", model);
                var cssPtah = Path.Combine(_hostingEnvironment.WebRootPath, @"css/pages/pdf/evaluationreport.css");

                var objectSettings = new DinkToPdf.ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = viewToString,
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPtah },
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
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };
                var fileByte = _dinkConverter.Convert(pdf);

                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

                return File(fileByte, "application/pdf", "PreActa.pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para generar el acta de una sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Archivo en formato PDF</returns>
        //Acta
        //[Route("acta-final/{sectionId}")]
        [HttpGet("acta-final/{sectionId}/old")]
        public async Task<IActionResult> OldEvaluationReport(Guid sectionId)
        {
            var section = await _context.Sections.Include(x => x.CourseTerm)
                .FirstOrDefaultAsync(x => x.Id == sectionId);

            if (section == null) return BadRequest();

            var evaluationReport = await _context.EvaluationReports.FirstOrDefaultAsync(x => x.SectionId == sectionId);

            var careerId = await _context.Sections.Where(x => x.Id == sectionId)
                .Select(x => x.CourseTerm.Course.CareerId)
                .FirstOrDefaultAsync();

            var academicYear = (byte)0;
            var year = (byte)0;
            if (careerId != null)
            {
                academicYear = (await _context.AcademicYearCourses.Where(x =>
                    x.Curriculum.CareerId == careerId && x.Curriculum.IsActive &&
                    x.CourseId == section.CourseTerm.CourseId).FirstOrDefaultAsync()).AcademicYear;

                year = (byte)Math.Round(academicYear * 1.00M / 2, 0, MidpointRounding.AwayFromZero);
            }

            var asyncModel = _context.Sections
                .Include(x => x.CourseTerm)
                .Include(x => x.CourseTerm.Course)
                .Include(x => x.CourseTerm.Course.Career)
                .Include(x => x.CourseTerm.Course.Career.Faculty)
                .Include(x => x.CourseTerm.Course.Career.Curriculums)
                .Where(x => x.Id == sectionId)
                .Select(x => new EvaluationReportViewModel
                {
                    CourseCode = x.CourseTerm.Course.Code,
                    CourseName = x.CourseTerm.Course.Name,
                    Credits = x.CourseTerm.Course.Credits,
                    Teacher = x.TeacherSections.Any()
                        ? string.Join(", ", x.TeacherSections.Select(ts => ts.Teacher.User.RawFullName))
                        : "No Asignado",
                    Term = x.CourseTerm.Term.Name,
                    Section = x.Code,
                    Carrer = x.CourseTerm.Course.Career.Name,
                    Faculty = x.CourseTerm.Course.Career.Faculty.Name,
                    AcademicYear = academicYear,
                    Year = year
                }).FirstOrDefaultAsync();

            var asyncStudents = _context.StudentSections
                .Where(x => x.Section.Id == sectionId && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .Select(x => new StudentGradeViewModel
                {
                    Code = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                    Grade = x.FinalGrade,
                    GradeText = ConvertHelpers.NumberToText(x.FinalGrade)
                }).ToListAsync();


            var model = await asyncModel;
            model.ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{@GeneralHelpers.GetTheme()}/logo-report.png");
            model.Students = await asyncStudents;
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 0, Left = 10, Right = 10 }
            };
            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Views/Grade/EvaluationReport.cshtml", model);
            var cssPtah = Path.Combine(_hostingEnvironment.WebRootPath, @"css/pages/pdf/evaluationreport.css");

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPtah },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };
            var pdf = new DinkToPdf.HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var fileByte = _dinkConverter.Convert(pdf);

            if (evaluationReport == null)
            {
                evaluationReport = new EvaluationReport
                {
                    LastReportGeneratedDate = DateTime.UtcNow,
                    SectionId = sectionId,
                    Status = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.GENERATED
                };

                await _context.EvaluationReports.AddAsync(evaluationReport);
            }
            else
            {
                if (evaluationReport.Status != CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED)
                {
                    evaluationReport.LastReportGeneratedDate = DateTime.UtcNow;
                }
            }

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf", "Acta.pdf");
        }

        /// <summary>
        /// Método para generar el acta de una sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Archivo en formato PDF</returns>
        [AllowAnonymous]
        [HttpGet("acta-final/{sectionId}")]
        public async Task<IActionResult> EvaluationReport(Guid sectionId)
        {
            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAH ||
                ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAB ||
                ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNIFSLB ||
                ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSCH ||
                ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNF ||
                ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAJMA ||
                ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNCP ||
                ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.Akdemic ||
                ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG
                )
            {
                var report = await _evaluationReportGeneratorService.GetRegisterEvaluationReport(sectionId);
                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                return File(report.Pdf, "application/pdf", "Acta de Notas.pdf");
            }

            var confiHeader = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER);
            var confiSubheader = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER);

            var model = new ActaScoresViewModel()
            {
                BasicInformation = new BasicInformation(),
                Rows = new List<Row>(),
                Total = new Total(),
                Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                HeaderText = confiHeader,
                SubHeaderText = confiSubheader
            };

            var section = await _sectionService.GetSectionWithTermAndCareer(sectionId);

            if (section == null)
                section = await _teacherSectionService.GetTeacherSectionsWithTermAndCareer(sectionId);
            var userId = GetUserId();
            var user = await _userService.Get(userId);


            var curriculum = await _curriculumService.GetFirstByCourse(section.CourseTerm.CourseId);
            var academicYearCourse = await _academicYearCourseService.GetLevelByCourseAndCurriculum(section.CourseTerm.CourseId, curriculum?.Id) ?? 0;
            var Class = await _classScheduleService.GetFirstForSection(sectionId);

            model.BasicInformation.Faculty = section.CourseTerm?.Course?.Career?.Faculty?.Name;
            model.BasicInformation.Teacher = user == null ? "--" : user.FullName;
            model.BasicInformation.Course = $"{section.CourseTerm?.Course?.Code} - (P:{curriculum?.Code}) - {section.CourseTerm?.Course?.Name}";
            model.BasicInformation.Career = section.CourseTerm?.Course?.Career?.Name;
            model.BasicInformation.Credits = section.CourseTerm?.Course?.Credits.ToString();
            model.BasicInformation.Sede = Class == null ? "--" : Class.Classroom == null ? "--" : Class.Classroom.Building == null ? "--" : Class.Classroom.Building.Name.Substring(0, 1);
            model.BasicInformation.Term = section.CourseTerm?.Term.Name;
            model.BasicInformation.Cycle = academicYearCourse;
            model.BasicInformation.Section = section.Code;
            model.BasicInformation.User = user.FullName;

            var sectionStudents = (await _studentSectionService.GetAllSectionStudentsWithUserBySectionId(sectionId))
                .Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .OrderBy(x => x.Student.User.FullName)
                .ToList();

            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();

            var completionPercentage = 0m;

            var confiEvaluationsByUnits = await _configurationService.GetByKey(ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT);

            if (confiEvaluationsByUnits is null)
            {
                confiEvaluationsByUnits = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT,
                    Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT]
                };
            }

            var evaluationsByUnits = Convert.ToBoolean(confiEvaluationsByUnits.Value);

            if (evaluationsByUnits)
            {
                var courseUnits = await _context.CourseUnits.Where(x => x.CourseSyllabus.CourseId == courseTerm.CourseId && x.CourseSyllabus.TermId == courseTerm.TermId)
                    .Include(x => x.Evaluations).ToListAsync();

                foreach (var item in courseUnits)
                {
                    var unitValue = 0M;

                    if (courseUnits.All(y => y.AcademicProgressPercentage == 0))
                    {
                        unitValue = 100M / courseUnits.Count();
                    }
                    else
                    {
                        unitValue = item.AcademicProgressPercentage;
                    }

                    var totalEvaluation = await _context.Evaluations.Where(x => x.CourseUnitId == item.Id).SumAsync(x => x.Percentage);
                    var evaluations = await _context.Evaluations.Where(x => x.CourseUnitId == item.Id && x.Grades.Any(y => y.StudentSection.SectionId == sectionId)).SumAsync(x => x.Percentage);

                    if (totalEvaluation == 0)
                        totalEvaluation = 1;

                    if (unitValue == 0)
                        unitValue = 1;

                    var progressByUnit = ((decimal)evaluations / (decimal)totalEvaluation) * 100M;

                    completionPercentage += (progressByUnit * (decimal)unitValue) / 100M;
                }

            }
            else
            {
                var totalPercentage = await _context.Evaluations.Where(x => x.CourseTermId == courseTerm.Id).SumAsync(x => x.Percentage);
                var totalPercetageComplete = await _context.Evaluations.Where(x => x.CourseTermId == courseTerm.Id && x.Grades.Any(y => y.StudentSection.SectionId == sectionId)).SumAsync(x => x.Percentage);

                completionPercentage = (totalPercetageComplete / (totalPercentage * 1M)) * 100M;
            }


            var classes = await _classStudentService.GetAll(sectionId);
            var totalClasses = await _classService.Count(null, null, null, null, null, null, null, sectionId);
            model.Approbed = section.CourseTerm.Term.MinGrade;
            model.BasicInformation.CompletionPercentage = Math.Round(completionPercentage, 2, MidpointRounding.AwayFromZero);
            for (int i = 0; i < sectionStudents.Count; i++)
            {
                var classStudent = classes.Where(x => x.StudentId == sectionStudents[i].StudentId).Count();
                var sustEvaluation = await _substituteExamService.GetSubstituteExamByStudentAndSectionId(sectionStudents[i].StudentId, section.Id);
                var row = new Row()
                {
                    HasGrades = await _context.Grades.Where(x => x.StudentSectionId == sectionStudents[i].Id).AnyAsync(),
                    Withdrawn = sectionStudents[i].Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN,
                    Order = i + 1,
                    Code = sectionStudents[i].Student.User.UserName,
                    Surnames = $"{sectionStudents[i].Student.User.PaternalSurname} {sectionStudents[i].Student.User.MaternalSurname}",
                    Names = sectionStudents[i].Student.User.Name,
                    AssistancePercent = totalClasses == 0 ? "100" : classStudent == 0 ? "0" : (classStudent * 100 / totalClasses).ToString(),

                    RegularEvaluation = sectionStudents[i].FinalGrade.ToString(),
                    RegularEvaluationNumber = sectionStudents[i].FinalGrade,
                    RegularEvaluationText = sectionStudents[i].Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ? "RETIRADO"
                    : NUMBERS.VALUES[sectionStudents[i].FinalGrade],

                    FinalEvaluation = sectionStudents[i].FinalGrade.ToString(),
                    FinalEvaluationNumber = sectionStudents[i].FinalGrade,
                    FinalEvaluationText = NUMBERS.VALUES[sectionStudents[i].FinalGrade],

                    FinalEvaluationApprobed = sectionStudents[i].Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ? "RETIRADO"
                    : sectionStudents[i].FinalGrade >= model.Approbed ? "APROBADO" : "DESAPROBADO"
                };

                if (sustEvaluation != null && sustEvaluation.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED)
                {
                    row.SustEvaluation = sustEvaluation.TeacherExamScore.HasValue ? sustEvaluation.TeacherExamScore.ToString() : sustEvaluation.ExamScore.ToString();
                    row.SustEvaluationText = sustEvaluation.TeacherExamScore.HasValue ? NUMBERS.VALUES[(int)sustEvaluation.TeacherExamScore.Value] : NUMBERS.VALUES[(int)sustEvaluation.ExamScore.Value];
                    row.HasSusti = true;

                    if (sustEvaluation.TeacherExamScore >= row.RegularEvaluationNumber)
                    {
                        row.RegularEvaluation = sustEvaluation.PrevFinalScore.ToString();
                        row.RegularEvaluationNumber = sustEvaluation.PrevFinalScore.Value;
                        row.RegularEvaluationText = sectionStudents[i].Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ? "RETIRADO"
                        : NUMBERS.VALUES[sustEvaluation.PrevFinalScore.Value];
                    }
                }

                if (completionPercentage < 100)
                {
                    row.FinalEvaluationApprobed = "EN PROCESO";
                }

                model.Rows.Add(row);
            }
            model.Total.Sust = 0;
            model.Total.Enrollment = sectionStudents.Count;
            model.Total.Approved = sectionStudents.Count - model.Rows.Count(x => x.FinalEvaluationApprobed == "DESAPROBADO");
            model.Total.NotApproved = model.Rows.Count(x => x.FinalEvaluationApprobed == "DESAPROBADO");

            DinkToPdf.GlobalSettings globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = "Acta de notas"
            };
            var viewToString = "";

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG)
            {
                viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Views/Grade/ReportUNJBG.cshtml", model);
            }
            else
           if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD)
                viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Views/Grade/ReportUNAMAD.cshtml", model);
            else
                viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Views/Grade/EvaluationReport.cshtml", model);


            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8" },
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
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var fileByte = _dinkConverter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf", "Acta de Notas.pdf");
        }

        /// <summary>
        /// Método para generar el acta detallada de una sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("acta-final-detallada/{sectionId}")]
        public async Task<IActionResult> EvaluationReport2(Guid sectionId)
        {
            var model = new ActaScoresViewModel()
            {
                BasicInformation = new BasicInformation(),
                Rows = new List<Row>(),
                Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
            };

            var section = await _sectionService.GetSectionWithTermAndCareer(sectionId);
            var teacherSection = await _teacherSectionService.GetTeacherSectionBySection(sectionId);
            var academicYearCourse = await _academicYearCourseService.GetAcademicYearCourseByCourseId(section.CourseTerm.CourseId);
            var Class = await _classScheduleService.GetClassSchedulesBySectionId(sectionId);
            var courseUnits = await _courseUnitService.GetCourseUnits(section.CourseTerm.Course.Id, section.CourseTerm.Term.Id);
            var user = await _userService.GetUserByClaim(User);

            model.BasicInformation.MinGrade = section.CourseTerm.Term.MinGrade;
            model.BasicInformation.Teacher = teacherSection == null ? "--" : teacherSection.Teacher.User.FullName;
            model.BasicInformation.Course = $"[{section.CourseTerm?.Course?.Code}] {section.CourseTerm?.Course?.Name}";
            model.BasicInformation.Career = section.CourseTerm?.Course?.Career?.Name;
            model.BasicInformation.Faculty = section.CourseTerm?.Course?.Career?.Faculty?.Name;
            model.BasicInformation.AcademicProgram = section.CourseTerm?.Course?.AcademicProgram?.Name;
            model.BasicInformation.Credits = section.CourseTerm?.Course?.Credits.ToString();
            model.BasicInformation.Sede = Class == null ? "--" : Class.Classroom == null ? "--" : Class.Classroom.Building == null ? "--" : Class.Classroom.Building.Name.Substring(0, 1);
            model.BasicInformation.Term = section.CourseTerm?.Term.Name;
            model.BasicInformation.Section = section.Code;
            model.BasicInformation.CourseUnitsList = courseUnits;
            model.BasicInformation.CourseUnits = courseUnits.Count();
            model.BasicInformation.User = user.UserName;

            var sectionStudents = await _studentSectionService.GetAllSectionStudentsWithUserBySectionId(sectionId);
            sectionStudents = sectionStudents.Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).ToList();
            model.Approbed = section.CourseTerm.Term.MinGrade;

            for (int i = 0; i < sectionStudents.Count; i++)
            {
                var courseUnitGrades = await _courseUnitService.GetCourseUnitGradesByStudentIdAndSectionId(sectionStudents[i].StudentId, sectionId);
                var susti = await _substituteExamService.GetExamScoreByCourseAndTermAndStudent(section.CourseTerm.Course.Id, section.CourseTerm.Term.Id, sectionStudents[i].StudentId);
                var averagesByUnits = courseUnitGrades is null ? null : courseUnitGrades.Select(x => x.Average).ToList();
                int finalAverage = sectionStudents[i].FinalGrade;
                if (averagesByUnits is null)
                {
                    averagesByUnits = new List<int?>();
                    for (int a = 0; a < courseUnits.Count(); a++)
                    {
                        averagesByUnits.Add(null);
                    }
                }

                if (susti != null)
                    finalAverage = (int)susti;

                var row = new Row()
                {
                    Order = i + 1,
                    Code = sectionStudents[i].Student.User.UserName,
                    Surnames = $"{sectionStudents[i].Student.User.PaternalSurname} {sectionStudents[i].Student.User.MaternalSurname}",
                    Names = sectionStudents[i].Student.User.Name,
                    PartialAverages = averagesByUnits.ToArray(),
                    FinalEvaluation = $"{finalAverage}",
                    FinalEvaluationNumber = finalAverage,
                    FinalEvaluationText = NUMBERS.VALUES[finalAverage],
                    HasSusti = susti.HasValue,
                    StudentSectionId = sectionStudents[i].Id,
                    StudentStatus = sectionStudents[i].Status
                };
                model.Rows.Add(row);
            }

            DinkToPdf.GlobalSettings globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Landscape,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = "Acta de notas"
            };

            //return View("/Areas/Admin/Views/EvaluationReport/Report.cshtml", model);
            string mainViewToString = string.Empty;

            mainViewToString = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Views/Grade/DetailedReportUNAMAD.cshtml", model);

            DinkToPdf.ObjectSettings mainObjectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = mainViewToString,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings =
                {
                    FontName = "Arial",
                    FontSize = 6,
                    Line = false,
                    Right = "Página [page]/[toPage]"
                },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 6,
                    Line = false,
                    Left = "DIRECCION UNIVERSITARIA DE ASUNTOS ACADEMICOS",
                    Center = "Impreso por: " + model.BasicInformation.User,
                    Right = DateTime.UtcNow.ToDefaultTimeZone().ToString("dd/MM/yyyy hh:mm:ss tt")
                }
            };

            DinkToPdf.HtmlToPdfDocument pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { mainObjectSettings }
            };

            byte[] fileByte = _dinkConverter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf", "Acta de Notas.pdf");
        }

        /// <summary>
        /// Vista donde se listan las notas por estudiante
        /// </summary>
        /// <param name="sectionid">Identificador de la sección</param>
        /// <returns>Vista detalle de la sección</returns>
        [Route("detalle/{sectionid}")]
        public async Task<IActionResult> Details(Guid sectionid)
        {
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(_appConfig.GradeDetails, BindingFlags.NonPublic | BindingFlags.Instance);

            var userId = GetUserId();

            if (!(await _teacherSectionService.AnyBySectionAndTeacher(sectionid, userId)))
            {
                ErrorToastMessage("No tiene acceso a la información de esta sección.");
                return RedirectToAction("Index");
            }

            var confiEvaluationsBySection = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_SECTION));

            if (confiEvaluationsBySection)
            {
                var evaluations = await _context.Evaluations.Where(x => x.CourseTerm.Sections.Any(y => y.Id == sectionid)).ToListAsync();
                var sectionEvaluations = await _context.SectionEvaluations.Where(x => x.SectionId == sectionid).ToListAsync();
                var evaluationsConfigurated = true;

                foreach (var eva in evaluations)
                {
                    if (!sectionEvaluations.Any(y => y.EvaluationId == eva.Id))
                    {
                        evaluationsConfigurated = false;
                        break;
                    }
                }

                if (!evaluationsConfigurated)
                {
                    ErrorToastMessage("Las evaluaciones no se encuentran configuradas por el docente.");
                    return RedirectToAction("Index");
                }
            }

            var modal = await (Task<DetailViewModel>)theMethod.Invoke(this, new object[] { sectionid, userId });

            return View(modal);
        }

        /// <summary>
        /// Reporte de los estudiantes matriculados en la sección detallando sus notas
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("detalle/{sectionId}/exportar-excel")]
        public async Task<IActionResult> GetDetailsExcel(Guid sectionId)
        {
            var section = await _sectionService.Get(sectionId);
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
            var course = await _courseService.GetAsync(courseTerm.CourseId);
            var evaluations = await _evaluationService.GetAllByCourseAndTermWithTaken(courseTerm.CourseId, courseTerm.TermId, sectionId);
            var students = await _studentSectionService.GetAllWithGradesAndEvaluations(null, null, sectionId);

            var completionPercentage = 0m;

            var confiEvaluationsByUnits = await _configurationService.GetByKey(ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT);

            if (confiEvaluationsByUnits is null)
            {
                confiEvaluationsByUnits = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT,
                    Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT]
                };
            }

            var evaluationsByUnits = Convert.ToBoolean(confiEvaluationsByUnits.Value);

            if (evaluationsByUnits)
            {
                var courseUnits = await _context.CourseUnits.Where(x => x.CourseSyllabus.CourseId == courseTerm.CourseId && x.CourseSyllabus.TermId == courseTerm.TermId)
                    .Include(x => x.Evaluations).ToListAsync();

                foreach (var item in courseUnits)
                {
                    var unitValue = 0M;

                    if (courseUnits.All(y => y.AcademicProgressPercentage == 0))
                    {
                        unitValue = 100M / courseUnits.Count();
                    }
                    else
                    {
                        unitValue = item.AcademicProgressPercentage;
                    }

                    var totalEvaluation = await _context.Evaluations.Where(x => x.CourseUnitId == item.Id).SumAsync(x => x.Percentage);
                    var sumEvaluations = await _context.Evaluations.Where(x => x.CourseUnitId == item.Id && x.Grades.Any(y => y.StudentSection.SectionId == sectionId)).SumAsync(x => x.Percentage);

                    if (totalEvaluation == 0)
                        totalEvaluation = 1;

                    if (unitValue == 0)
                        unitValue = 1;

                    var progressByUnit = ((decimal)sumEvaluations / (decimal)totalEvaluation) * 100M;

                    completionPercentage += (progressByUnit * (decimal)unitValue) / 100M;
                }

            }
            else
            {
                var totalPercentage = await _context.Evaluations.Where(x => x.CourseTermId == courseTerm.Id).SumAsync(x => x.Percentage);
                var totalPercetageComplete = await _context.Evaluations.Where(x => x.CourseTermId == courseTerm.Id && x.Grades.Any(y => y.StudentSection.SectionId == sectionId)).SumAsync(x => x.Percentage);

                completionPercentage = (totalPercetageComplete / (totalPercentage * 1M)) * 100M;
            }

            var evaluationsModel = evaluations
                .OrderBy(x => x.CourseUnit?.Number)
                .ThenBy(x => x.Week)
                .Select(x => new EvaluationViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Percentage = x.Percentage,
                    Taken = x.Taken,
                    Description = x.Description,
                    CourseUnitNumber = x.CourseUnit?.Number
                })
           .ToList();

            var studentsModel = students
                .Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .Select(ss => new
                {
                    Code = ss.Student.User.UserName,
                    Name = ss.Student.User.FullName,
                    FinalGrade = ss.FinalGrade,
                    Withdrawn = ss.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN,
                    Grades = evaluationsModel.Select(ev =>
                    ss.Grades.Where(g => g.EvaluationId == ev.Id)
                    .Select(g => new
                    {
                        g.Value,
                        evaluationId = ev.Id
                    })
                    .FirstOrDefault())
                    .ToList()
                }).OrderBy(ss => ss.Name).ToList();

            var dt = new DataTable
            {
                TableName = "Notas Ingresadas"
            };

            dt.Columns.Add("CÓDIGO");
            dt.Columns.Add("ESTUDIANTE");
            foreach (var evaluation in evaluationsModel)
            {
                if (evaluationsByUnits)
                {
                    dt.Columns.Add($"U{evaluation.CourseUnitNumber}-{evaluation.Name}({evaluation.Percentage}%)");
                }
                else
                {
                    dt.Columns.Add($"{evaluation.Name}({evaluation.Percentage}%)");
                }
            }

            dt.Columns.Add($"NOTA FINAL (AVANCE AL {completionPercentage}%)");

            foreach (var item in studentsModel)
            {
                var list = new List<string>
                {
                    item.Code,
                    item.Name
                };

                foreach (var evaluation in evaluationsModel)
                {
                    var grade = item.Grades.Where(x => x != null && x.evaluationId == evaluation.Id).FirstOrDefault();

                    if (item.Withdrawn)
                    {
                        list.Add("RET");
                    }
                    else if (grade != null)
                    {
                        if (grade.Value == -1)
                        {
                            list.Add("-");
                        }
                        else
                        {
                            list.Add($"{grade.Value}");
                        }
                    }
                    else
                    {
                        list.Add("-");
                    }
                }

                list.Add($"{item.FinalGrade}");
                dt.Rows.Add(list.ToArray());
            }

            dt.AcceptChanges();

            var fileName = $"NOTAS - SECCIÓN {section.Code}.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet($"{course.Name.Trim()} / {section.Code.Trim()}", null);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }

        /// <summary>
        /// Método para obtener los detalles de la evaluación de una sección
        /// </summary>
        /// <param name="sectionid">Identificador de la sección</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Objeto con los detalles de la evaluación</returns>
        private async Task<DetailViewModel> StandardDetail(Guid sectionid, string userId)
        {
            var confiEvaluationsByUnits = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT);
            var confiEnableBulkSaveGrades = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.TeacherManagement.ENABLE_BULK_SAVE_GRADES);
            var confiGradesByPrincipalTeacher = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.GRADES_CAN_ONLY_PUBLISHED_BY_PRINCIPAL_TEACHER);
            var confiEvaluationsBySection = Boolean.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_SECTION));

            var section = await _sectionService.Get(sectionid);
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
            var course = await _courseService.GetAsync(courseTerm.CourseId);
            var term = await _termService.Get(courseTerm.TermId);
            var teacherSections = await _context.TeacherSections.Where(x => x.SectionId == section.Id).ToListAsync();

            var maxAbsencesPercentage = term.AbsencePercentage;

            bool.TryParse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.ENABLED_SPECIAL_ABSENCE_PERCENTAGE), out var enabledSpecialAbsencePercentage);

            if (enabledSpecialAbsencePercentage)
            {
                float.TryParse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE), out var specialAbsencePercentage);
                var absencePercentageDescription = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE_DESCRIPTION);
                var courseName = course.Name;

                if (!string.IsNullOrEmpty(absencePercentageDescription) && courseName.ToLower().Trim().Contains(absencePercentageDescription.Trim().ToLower()))
                    maxAbsencesPercentage = specialAbsencePercentage;
            }

            var students = await _context.StudentSections.Where(x => x.SectionId == sectionid)
                .Select(x => new
                {
                    x.Id,
                    x.Status,
                    x.FinalGrade,
                    x.Student.User.FullName,
                    x.StudentId,
                    x.Student.User.UserName,
                    x.SectionGroupId,
                    Grades = x.Grades.ToList()
                })
                .ToListAsync();

            var availableToPrint = false;

            var courseUnitsModel = await _context.CourseUnits
                .Where(x => x.CourseSyllabus.CourseId == courseTerm.CourseId && x.CourseSyllabus.TermId == courseTerm.TermId)
                .OrderBy(x => x.Number)
                .ThenBy(x => x.WeekNumberStart)
                .Select(x => new CourseUnitModel
                {
                    Id = x.Id,
                    WeekEnd = x.WeekNumberEnd,
                    WeekStart = x.WeekNumberStart,
                    Number = x.Number,
                    Name = x.Name
                })
                .ToListAsync();

            var classStudents = await _context.ClassStudents.Where(x => x.Class.SectionId == sectionid)
                .Select(x => new
                {
                    x.StudentId,
                    x.IsAbsent,
                    x.Class.ClassSchedule.SectionGroupId
                })
                .ToListAsync();

            var classesBySubGroup = await _context.Classes.Where(x => x.ClassSchedule.SectionId == sectionid)
                .GroupBy(x => x.ClassSchedule.SectionGroupId)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .ToListAsync();


            var evaluationsModel = await _context.Evaluations
                .Where(x => x.CourseTermId == courseTerm.Id)
                .OrderBy(x => x.CourseUnit.Number)
                .ThenBy(x => x.Week)
                .Select(x => new EvaluationViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Percentage = x.Percentage,
                    Week = x.Week,
                    //Taken = x.Taken,
                    Description = x.Description,
                    CourseUnitNumber = x.CourseUnitId.HasValue ? x.CourseUnit.Number : (byte?)null
                })
           .ToListAsync();

            if (confiEvaluationsBySection)
            {
                var sectionEvaluations = await _context.SectionEvaluations.Where(x => x.SectionId == sectionid).ToListAsync();

                foreach (var eva in evaluationsModel)
                {
                    eva.Percentage = sectionEvaluations.Any(x => x.EvaluationId == eva.Id) ? sectionEvaluations.Where(x => x.EvaluationId == eva.Id).Select(x => x.Percentage).FirstOrDefault() : null;
                }
            }

            var evaluationReport = await _evaluationReportService.GetEvaluationReportByFilters(sectionid, null, null, CORE.Helpers.ConstantHelpers.Intranet.EvaluationReportType.REGULAR);

            //Orden de merito
            var studentsId = students.Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).Select(y => y.StudentId).ToList();
            var hasMeritOrder = await _context.AcademicSummaries.Where(x => studentsId.Contains(x.StudentId) && x.TermId == courseTerm.TermId).AnyAsync(y => y.MeritOrder > 0);

            //Historial academico
            var academicHistoriesCount = await _context.AcademicHistories.CountAsync(x => x.SectionId == section.Id && studentsId.Contains(x.StudentId));
            var hasAcademicHistories = academicHistoriesCount >= studentsId.Count();

            var studentsModel = students
                .Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .Select(ss => new StudentViewModel
                {
                    Code = ss.UserName,
                    Name = ss.FullName,
                    Absents = classStudents.Where(x => x.StudentId == ss.StudentId && x.IsAbsent).Count(),
                    Withdrawn = ss.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN,
                    DPI = classesBySubGroup.Where(x => !x.Key.HasValue || x.Key == ss.SectionGroupId).Sum(x => x.count) == 0 ?
                        false :
                        ((decimal)classStudents.Where(x => x.StudentId == ss.StudentId && (!x.SectionGroupId.HasValue || x.SectionGroupId == ss.SectionGroupId) && x.IsAbsent).Count() / (decimal)classesBySubGroup.Where(x => !x.Key.HasValue || x.Key == ss.SectionGroupId).Sum(x => x.count)) * 100M > (decimal)maxAbsencesPercentage,
                    Grades = evaluationsModel
                    .Select(ev =>
                    ss.Grades.Any(g => g.EvaluationId == ev.Id)
                        ? ss.Grades.Where(g => g.EvaluationId == ev.Id).Select(g => g.Value).FirstOrDefault()
                        : -1)
                    .ToList(),
                    EvaluationGrades = evaluationsModel
                    .Select(ev => new EvaluationGradeViewModel
                    {
                        EvaluationId = ev.Id,
                        Grade = ss.Grades.Any(g => g.EvaluationId == ev.Id) ? ss.Grades.Where(g => g.EvaluationId == ev.Id).Select(g => g.Value).FirstOrDefault() : (decimal?)null,
                    })
                    .ToList()
                }).OrderBy(ss => ss.Name).ToList();

            //Asiganar si la evaluación ha sido calificada para todos los estudiantes que no esten DPI.
            foreach (var item in evaluationsModel)
                item.Taken = studentsModel.Where(y => !y.DPI).All(y => y.EvaluationGrades.Any(z => z.EvaluationId == item.Id && z.Grade.HasValue));

            var enabledPartialEvaluationReportRegister = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.ENABLED_PARTIAL_EVALUATION_REPORT_REGISTER));
            var allEvaluationsWithGrades = enabledPartialEvaluationReportRegister || evaluationsModel.All(x => x.Taken);

            var jsondata = new List<JsonDatatbleColumn>
            {
                new JsonDatatbleColumn { field = "code",
                    title = "Código",
                    width = "70",
                    textAlign = "center" },
                new JsonDatatbleColumn { field = "student",
                    title = "Estudiantes inscritos",
                    width = "auto",
                    textAlign = "left" }
            };

            var i = 0;

            foreach (var item in evaluationsModel)
            {
                jsondata.Add(new JsonDatatbleColumn
                {
                    field = "grades[" + i + "]",
                    title = $"{item.Name} ({item.Percentage}%)",
                    width = "70",
                    textAlign = "center"
                });
                i++;
            }

            return new DetailViewModel()
            {
                AllEvaluationsWithGrades = allEvaluationsWithGrades,
                Section = section.Code,
                EvaluationsByUnits = Convert.ToBoolean(confiEvaluationsByUnits),
                EnableBulkSaveGrades = Convert.ToBoolean(confiEnableBulkSaveGrades),
                Course = course.FullName,
                CourseUnits = courseUnitsModel,
                SectionId = sectionid,
                Evaluations = evaluationsModel,
                Students = studentsModel,
                EvaluationReportGenerated = evaluationReport != null ? true : false,
                JsonColumns = JsonConvert.SerializeObject(jsondata),
                AvailableToPrint = availableToPrint,
                HasAcademicHistories = hasAcademicHistories,
                HasMeritOrder = hasMeritOrder,
                EvaluationReportCode = evaluationReport != null ? evaluationReport.Code : string.Empty,
                GradesCanOnlyPublishedByPrincipalTeacher = Convert.ToBoolean(confiGradesByPrincipalTeacher),
                IsPrincipalTeacher = teacherSections.Any(y => y.TeacherId == userId && y.IsPrincipal)
            };
        }

        /// <summary>
        /// Método para obtener los detalles de la evaluación de una sección
        /// </summary>
        /// <param name="sectionid">Identificador de la sección</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Objeto con los detalles de la evaluación</returns>
        private async Task<DetailViewModel> DetailFormat1(Guid sectionid, string userId)
        {
            var section = await _sectionService.Get(sectionid);
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
            var course = await _courseService.GetAsync(courseTerm.CourseId);

            var evaluations = await _evaluationService.GetAllByCourseAndTermWithTaken(courseTerm.CourseId, courseTerm.TermId, sectionid);
            var students = await _studentSectionService.GetAllWithGradesAndEvaluations(null, null, sectionid, userId);
            var availableToPrint = false;
            var allEvaluationsWithGrades = await _context.GradeRegistrations.Where(x => x.SectionId == sectionid).CountAsync() == evaluations.Count();

            if (await _evaluationReportService.GetEvalutionReportByTeacherIdAndCourseId(courseTerm.CourseId, userId) != null)
            {
                availableToPrint = true;
            }

            if (evaluations.Any(x => x.Taken))
            {
                foreach (var item in students)
                {
                    if (item.Grades.Count != evaluations.Count(x => x.Taken))
                    {
                        var pending = evaluations.Where(x => !item.Grades.Any(y => y.EvaluationId == x.Id) && x.Taken).ToList();

                        foreach (var evaluation in pending)
                        {
                            var _grade = new Grade
                            {
                                EvaluationId = evaluation.Id,
                                StudentSectionId = item.Id,
                                Attended = false,
                                Value = 0
                            };

                            await _gradeService.Insert(_grade);
                        }
                    }
                }
            }

            var evaluationsModel = evaluations
                .OrderBy(x => x.Week)
                //.ThenBy(x => x.Name)
                .Select(x => new EvaluationViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Percentage = x.Percentage,
                    Taken = x.Taken
                })
                //.OrderBy(x => x.Name)
                .ToList();

            var studentsModel = students
                .Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .Select(ss => new StudentViewModel
                {
                    Code = ss.Student.User.UserName,
                    Name = ss.Student.User.FullName,
                    Withdrawn = ss.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN,
                    Grades = evaluationsModel
                    .Select(ev =>
                    ss.Grades.Any(g => g.EvaluationId == ev.Id)
                        ? ss.Grades.Where(g => g.EvaluationId == ev.Id).Select(g => g.Value).FirstOrDefault()
                        : -1)
                    .ToList()
                }).OrderBy(ss => ss.Name).ToList();

            var jsondata = new List<JsonDatatbleColumn>
            {
                new JsonDatatbleColumn { field = "code",
                    title = "Código",
                    width = "70",
                    textAlign = "center" },
                new JsonDatatbleColumn { field = "student",
                    title = "Estudiantes inscritos",
                    width = "auto",
                    textAlign = "left" }
            };

            var i = 0;

            foreach (var item in evaluationsModel)
            {
                jsondata.Add(new JsonDatatbleColumn
                {
                    field = "grades[" + i + "]",
                    title = $"{item.Name} ({item.Percentage}%)",
                    width = "70",
                    textAlign = "center"
                });
                i++;
            }

            return new DetailViewModel()
            {
                AllEvaluationsWithGrades = allEvaluationsWithGrades,
                Section = section.Code,
                Course = course.FullName,
                SectionId = sectionid,
                Evaluations = evaluationsModel,
                Students = studentsModel,
                JsonColumns = JsonConvert.SerializeObject(jsondata),
                AvailableToPrint = availableToPrint
            };
        }

        /// <summary>
        /// Vista donde se registran las notas de los estudiantes
        /// </summary>
        /// <param name="section">Identificador de la sección</param>
        /// <param name="evaluation">Identificador de la evaluación</param>
        /// <returns>Vista</returns>
        [HttpGet("registrar/{section}/{evaluation}")]
        public async Task<IActionResult> Register(Guid section, Guid evaluation)
        {
            var userId = GetUserId();

            var teacherSections = await _context.TeacherSections.Where(x => x.SectionId == section).ToListAsync();

            if (!teacherSections.Any(y => y.TeacherId == userId))
            {
                ErrorToastMessage("No tiene acceso a la información de esta sección.");
                return RedirectToAction("Index");
            }

            var confiGradesByPrincipalTeacher = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.GRADES_CAN_ONLY_PUBLISHED_BY_PRINCIPAL_TEACHER);

            if (Convert.ToBoolean(confiGradesByPrincipalTeacher))
            {
                if (!teacherSections.Any(y => y.TeacherId == userId && y.IsPrincipal))
                {
                    InfoToastMessage("Solo los docentes principales pueden subir notas.", "Información");
                    return Redirect($"/profesor/notas/detalle/{section}");
                }
            }

            var sectionobj = await _sectionService.Get(section);
            var courseTerm = await _courseTermService.GetAsync(sectionobj.CourseTermId);
            var course = await _courseService.GetAsync(courseTerm.CourseId);
            var evaluationobj = await _evaluationService.Get(evaluation);

            if (sectionobj != null && evaluationobj != null)
            {
                ViewBag.Section = section;
                ViewBag.SectionName = sectionobj.Code;
                ViewBag.Evaluation = evaluation;
                ViewBag.Course = course.FullName;

                var confiEvaluationsBySection = Boolean.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_SECTION));

                var evaluationName = $"{evaluationobj.Name} - ";

                if (confiEvaluationsBySection)
                {
                    var sectionEvaluation = await _context.SectionEvaluations.Where(x => x.EvaluationId == evaluationobj.Id && x.SectionId == sectionobj.Id).FirstOrDefaultAsync();

                    if (sectionEvaluation == null)
                    {
                        InfoToastMessage("La evaluación no ha sido configurada por el docente.", "Información");
                        return Redirect($"/profesor/notas/detalle/{section}");
                    }

                    evaluationName += $"{sectionEvaluation.Percentage}%";
                }
                else
                {
                    evaluationName += $"{evaluationobj.Percentage}%";
                }

                ViewBag.EvaluationName = evaluationName;
                return View();
            }

            return RedirectToAction("Index");
        }


        [HttpGet("registrar/{sectionId}/unidad/{courseUnitId}")]
        public async Task<IActionResult> RegisterUnit(Guid sectionId, Guid courseUnitId)
        {
            var userId = GetUserId();
            if (!(await _teacherSectionService.AnyBySectionAndTeacher(sectionId, userId)))
            {
                ErrorToastMessage("No tiene acceso a la información de esta sección.");
                return RedirectToAction("Index");
            }

            var sectionobj = await _sectionService.Get(sectionId);
            var courseTerm = await _courseTermService.GetAsync(sectionobj.CourseTermId);
            var course = await _courseService.GetAsync(courseTerm.CourseId);
            var courseUnit = await _courseUnitService.GetAsync(courseUnitId);

            if (sectionobj != null && courseUnit != null)
            {
                ViewBag.SectionId = sectionId;
                ViewBag.CourseUnitId = courseUnit.Id;
                ViewBag.SectionName = sectionobj.Code;
                ViewBag.Course = course.FullName;
                ViewBag.CourseUnitName = $"UNIDAD {courseUnit.Number} - ({courseUnit.Name})";
                return View();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Método que obtiene el listado de estudiantes con su nota
        /// </summary>
        /// <param name="section">Identificador de la sección</param>
        /// <param name="evaluation">Identificador de la evaluación</param>
        /// <returns>Objeto que contiene los datos de los estudiantes</returns>
        [HttpGet("registrar/getalumnos/{section}/{evaluation}")]
        public async Task<IActionResult> GetStudentsSection(Guid section, Guid evaluation)
        {
            var paginationParameter = GetDataTablePaginationParameter();

            var userId = GetUserId();
            var studentSections = await _studentSectionService.GetStudentSectionsRegisterGradeTemplate(section);
            var currentTemporalGrades = await _temporalGradeService.GetAllByFilters(section, evaluation);
            var grades = await _gradeService.GetGradesBySectionId(section);
            var currentGrades = grades.Where(x => x.EvaluationId == evaluation).ToList();
            var sectionEntity = await _sectionService.Get(section);
            var courseTerm = await _courseTermService.GetAsync(sectionEntity.CourseTermId);
            var term = await _termService.Get(courseTerm.TermId);

            var maxAbsencesPercentage = term.AbsencePercentage;

            bool.TryParse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.ENABLED_SPECIAL_ABSENCE_PERCENTAGE), out var enabledSpecialAbsencePercentage);

            if (enabledSpecialAbsencePercentage)
            {
                float.TryParse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE), out var specialAbsencePercentage);
                var absencePercentageDescription = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE_DESCRIPTION);
                var courseName = await _context.Sections.Where(x => x.Id == section).Select(x => x.CourseTerm.Course.Name).FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(absencePercentageDescription) && courseName.ToLower().Trim().Contains(absencePercentageDescription.Trim().ToLower()))
                    maxAbsencesPercentage = specialAbsencePercentage;
            }

            var classStudents = await _context.ClassStudents.Where(x => x.Class.SectionId == section)
                .Select(x => new
                {
                    x.StudentId,
                    x.IsAbsent,
                    x.Class.ClassSchedule.SectionGroupId
                })
                .ToListAsync();

            var classesBySubGroup = await _context.Classes.Where(x => x.ClassSchedule.SectionId == section)
                .GroupBy(x => x.ClassSchedule.SectionGroupId)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .ToListAsync();

            var query = studentSections
                .Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .Select(x => new
                {
                    x.SectionGroupId,
                    id = x.Id,
                    studentId = x.UserId,
                    code = x.StudentUserName,
                    student = x.StudentFullName,
                    status = x.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN,
                    attended = currentGrades.Any(y => y.StudentSectionId == x.Id) ? currentGrades.FirstOrDefault(y => y.StudentSectionId == x.Id).Attended
                        : currentTemporalGrades.Any(y => y.StudentSectionId == x.Id) ? currentTemporalGrades.FirstOrDefault(y => y.StudentSectionId == x.Id).Attended
                        : true,
                    pending = !currentGrades.Any(y => y.StudentSectionId == x.Id),
                    grade = currentGrades.Any(y => y.StudentSectionId == x.Id) ? currentGrades.FirstOrDefault(y => y.StudentSectionId == x.Id).Value
                        : currentTemporalGrades.Any(y => y.StudentSectionId == x.Id) ? currentTemporalGrades.FirstOrDefault(y => y.StudentSectionId == x.Id).Value
                        : 0,
                    absencesPercentage = classesBySubGroup.Where(y => !y.Key.HasValue || y.Key == x.SectionGroupId).Sum(y => y.count) == 0 ?
                        0 :
                        (decimal)classStudents.Where(y => y.StudentId == x.StudentId && (!y.SectionGroupId.HasValue || y.SectionGroupId == x.SectionGroupId) && y.IsAbsent).Count() / (decimal)classesBySubGroup.Where(y => !y.Key.HasValue || y.Key == x.SectionGroupId).Sum(y => y.count) * 100M
                })
                .OrderBy(x => x.student)
                .ToList();

            if (maxAbsencesPercentage != 0)
                query = query.Where(x => x.absencesPercentage <= (decimal)maxAbsencesPercentage).ToList();

            var filterRecords = query.Count;

            var result = GetDataTablePaginationObject(filterRecords, query);

            return Ok(result);
        }

        [HttpGet("registrar/getalumnos-unidad/{sectionId}/{courseUnitId}")]
        public async Task<IActionResult> StudentSectionUnitPartialView(Guid sectionId, Guid courseUnitId)
        {
            var userId = GetUserId();
            var studentSections = await _studentSectionService.GetStudentSectionsRegisterGradeTemplate(sectionId);
            var evaluations = await _context.Evaluations.Where(x => x.CourseUnitId == courseUnitId).ToListAsync();
            var section = await _context.Sections.Where(x => x.Id == sectionId).FirstOrDefaultAsync();
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
            var term = await _termService.Get(courseTerm.TermId);
            var currentTemporalGrades = await _context.TemporalGrades.Where(x => x.StudentSection.SectionId == sectionId && x.Evaluation.CourseUnitId == courseUnitId).ToListAsync();
            var grades = await _context.Grades.Where(x => x.StudentSection.SectionId == sectionId && x.Evaluation.CourseUnitId == courseUnitId).ToListAsync();

            var classStudents = await _context.ClassStudents.Where(x => x.Class.SectionId == sectionId)
                .Select(x => new
                {
                    x.StudentId,
                    x.IsAbsent,
                    x.Class.ClassSchedule.SectionGroupId
                })
                .ToListAsync();

            var classesBySubGroup = await _context.Classes.Where(x => x.ClassSchedule.SectionId == sectionId)
                .GroupBy(x => x.ClassSchedule.SectionGroupId)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .ToListAsync();

            var model = new RegisterGradeByUnitViewModel
            {
                CourseUnitId = courseUnitId,
                Evaluations = evaluations.OrderBy(y => y.Week).Select(y => new EvaluationViewModel
                {
                    Id = y.Id,
                    Name = y.Name,
                    Description = y.Description
                })
                .ToList(),
                StudentSections = studentSections
                .OrderBy(x => x.StudentFullName)
                .Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .Select(x => new StudentSectionViewModel
                {
                    Id = x.Id,
                    StudentId = x.StudentId,
                    AbsencesPercentage = classesBySubGroup.Where(y => !y.Key.HasValue || y.Key == x.SectionGroupId).Sum(y => y.count) == 0 ?
                        0 :
                        (decimal)classStudents.Where(y => y.StudentId == x.StudentId && (!y.SectionGroupId.HasValue || y.SectionGroupId == x.SectionGroupId) && y.IsAbsent).Count() / (decimal)classesBySubGroup.Where(y => !y.Key.HasValue || y.Key == x.SectionGroupId).Sum(y => y.count) * 100M,
                    Code = x.StudentUserName,
                    FullName = x.StudentFullName,
                    Status = x.Status,
                    Evaluations = evaluations
                    .OrderBy(y => y.Week)
                    .Select(y => new EvaluationViewModel
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Description = y.Description,
                        Attended = grades.Any(z => z.EvaluationId == y.Id && z.StudentSectionId == x.Id) ? grades.FirstOrDefault(z => z.EvaluationId == y.Id && z.StudentSectionId == x.Id).Attended
                                    : currentTemporalGrades.Any(z => z.EvaluationId == y.Id && z.StudentSectionId == x.Id) ? currentTemporalGrades.FirstOrDefault(z => z.EvaluationId == y.Id && z.StudentSectionId == x.Id).Attended
                                    : (bool?)null,
                        Grade = grades.Any(z => z.EvaluationId == y.Id && z.StudentSectionId == x.Id) ? grades.FirstOrDefault(z => z.EvaluationId == y.Id && z.StudentSectionId == x.Id).Value.ToString()
                                    : currentTemporalGrades.Any(z => z.EvaluationId == y.Id && z.StudentSectionId == x.Id) ? currentTemporalGrades.FirstOrDefault(z => z.EvaluationId == y.Id && z.StudentSectionId == x.Id).Value.ToString()
                                    : null,
                        Published = grades.Any(z => z.EvaluationId == y.Id && z.StudentSectionId == x.Id)
                    })
                    .ToList()
                })
                .ToList()
            };


            if (term.AbsencePercentage != 0)
                model.StudentSections = model.StudentSections.Where(x => x.AbsencesPercentage <= (decimal)term.AbsencePercentage).ToList();

            return PartialView(model);
        }

        /// <summary>
        /// Método para guardar las notas temporales
        /// </summary>
        /// <param name="section">Identificador de la sección</param>
        /// <param name="evaluation">Identificador de la evaluación</param>
        /// <param name="grades">Objeto que contiene los datos de las notas</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("registrar/post")]
        public async Task<IActionResult> RegisterPost(Guid section, Guid evaluation, GradePostViewModel[] grades)
        {
            var userId = GetUserId();

            var sec = await _sectionService.GetWithEvaluations(section);

            if (!(await _teacherSectionService.AnyBySectionAndTeacher(section, userId)))
                return BadRequest("No está permitido para registrar notas en esta sección");

            var _evaluation = await _evaluationService.Get(evaluation);
            var iPAddress = GetRequestIP();

            var currentTemporalGrades = await _temporalGradeService.GetAllByFilters(section, evaluation);
            var newTemporalGrades = new List<TemporalGrade>();

            var sectionGrades = await _gradeService.GetGradesBySectionId(section);

            foreach (var item in grades)
            {
                if (sectionGrades.Any(x => x.EvaluationId == evaluation && x.StudentSectionId == item.Id))
                    continue;

                var grade = currentTemporalGrades.FirstOrDefault(x => x.StudentSectionId == item.Id);

                if (grade != null)
                {
                    grade.Attended = !item.NotTaken;
                    grade.Value = !item.NotTaken ? item.Grade : 0;
                    grade.CreatorIP = iPAddress;
                }
                else
                {
                    grade = new TemporalGrade
                    {
                        Evaluation = _evaluation,
                        StudentSectionId = item.Id,
                        Attended = !item.NotTaken,
                        Value = !item.NotTaken ? item.Grade : 0,
                        CreatorIP = iPAddress
                    };

                    newTemporalGrades.Add(grade);
                }
            }

            await _temporalGradeService.InsertRange(newTemporalGrades);

            var gradeRegistration = await _gradeRegistrationService.GetByFilters(section, evaluation, null);
            if (gradeRegistration == null)
            {
                gradeRegistration = new GradeRegistration
                {
                    Date = DateTime.UtcNow,
                    EvaluationId = evaluation,
                    SectionId = section,
                    TeacherId = userId
                };
                await _gradeRegistrationService.Insert(gradeRegistration);
            }
            else
            {
                gradeRegistration.Date = DateTime.UtcNow;
                gradeRegistration.TeacherId = userId;
                await _gradeRegistrationService.Update(gradeRegistration);
            }

            return Ok("Registro satisfactorio");
            //return RedirectToAction("Details", new { sectionid = section });
        }

        /// <summary>
        /// Método para publicar las notas oficiales
        /// </summary>
        /// <param name="section">Identificador de la sección</param>
        /// <param name="evaluation">Identificador de la evaluación</param>
        /// <param name="grades">Objeto que contiene los datos de las notas</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("registrar/publicar")]
        public async Task<IActionResult> PublishGrades(Guid section, Guid evaluation, GradePostViewModel[] grades)
        {
            var userId = GetUserId();

            if (!(await _teacherSectionService.AnyBySectionAndTeacher(section, userId)))
                return BadRequest("No tiene acceso a la información de esta sección.");

            var sectionGrades = await _gradeService.GetGradesBySectionId(section);

            var sec = await _sectionService.GetWithEvaluations(section);
            var term = await _termService.Get(sec.CourseTerm.TermId);
            var evaluations = await _evaluationService.GetAllByCourseAndTerm(sec.CourseTerm.CourseId, sec.CourseTerm.TermId);

            if (!(await _teacherSectionService.AnyBySectionAndTeacher(section, userId))) return BadRequest("No está permitido para registrar notas en esta sección.");

            var iPAddress = GetRequestIP();

            //Orden de merito
            var studentsId = await _context.StudentSections.Where(x => x.SectionId == section && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).Select(x => x.StudentId).ToListAsync();
            var hasMeritOrder = await _context.AcademicSummaries.Where(x => studentsId.Contains(x.StudentId) && x.TermId == sec.CourseTerm.TermId).AnyAsync(y => y.MeritOrder > 0);

            if (hasMeritOrder)
                return BadRequest("Se encontraron alumnos con cuadro de mérito asignado.");

            //Historial academico
            var academicHistoriesCount = await _context.AcademicHistories.CountAsync(x => x.SectionId == section && studentsId.Contains(x.StudentId));
            var hasAcademicHistories = academicHistoriesCount >= studentsId.Count();

            if (hasAcademicHistories)
                return BadRequest("Se encontraron alumnos con historial académico creado.");

            var evaluationReport = await _context.EvaluationReports.Where(x => x.SectionId == section).FirstOrDefaultAsync();

            foreach (var item in grades)
            {
                if (sectionGrades.Any(x => x.EvaluationId == evaluation && x.StudentSectionId == item.Id))
                    continue;

                var grade = new Grade
                {
                    EvaluationId = evaluation,
                    StudentSectionId = item.Id,
                    Attended = !item.NotTaken,
                    Value = (!item.NotTaken ? item.Grade : 0),
                    CreatorIP = iPAddress
                };

                await _gradeService.Insert(grade);
                await _studentSectionService.RecalculateFinalGrade(item.Id);

                if (term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                {
                    var studentSection = await _studentSectionService.Get(item.Id);
                    var academicHistory = await _academicHistoryService.GetAcademicHistoryBystudentAndCourseId(studentSection.StudentId, sec.CourseTerm.CourseId, sec.CourseTerm.TermId);

                    if (academicHistory != null)
                    {
                        academicHistory.Grade = studentSection.FinalGrade;
                        academicHistory.Approved = studentSection.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI && studentSection.FinalGrade >= term.MinGrade;

                        await _academicHistoryService.Update(academicHistory);
                    }
                    else
                    {
                        var gradesCount = await _context.Grades.Where(x => x.StudentSectionId == item.Id).CountAsync();
                        if (evaluations.Count() == gradesCount)
                        {
                            academicHistory = new AcademicHistory
                            {
                                CourseId = sec.CourseTerm.CourseId,
                                SectionId = studentSection.SectionId,
                                StudentId = studentSection.StudentId,
                                TermId = term.Id,
                                Approved = studentSection.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI && studentSection.FinalGrade >= term.MinGrade,
                                Grade = studentSection.FinalGrade,
                                Try = studentSection.Try,
                                CurriculumId = studentSection.Student.CurriculumId
                            };

                            if (evaluationReport != null)
                                academicHistory.EvaluationReportId = evaluationReport.Id;

                            await _academicHistoryService.Insert(academicHistory);
                        }
                    }

                    if (studentSection.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI)
                        studentSection.Status = studentSection.FinalGrade >= term.MinGrade ? ConstantHelpers.STUDENT_SECTION_STATES.APPROVED : ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED;

                    await _studentSectionService.Update(studentSection);

                    await _academicSummariesService.ReCreateStudentAcademicSummaries(studentSection.StudentId);
                }
            }

            var wasLate = false;
            var _evaluation = await _evaluationService.Get(evaluation);

            if (_evaluation.CourseUnitId.HasValue)
            {
                var scoreInputSchedule = await _scoreInputScheduleService.GetByTermAndCourseComponent(term.Id, sec.CourseTerm.Course.CourseComponentId);

                if (scoreInputSchedule != null)
                {
                    var courseUnit = await _courseUnitService.GetAsync(_evaluation.CourseUnitId.Value);
                    if (courseUnit != null)
                    {
                        var date = scoreInputSchedule.Details.Where(x => x.NumberOfUnit == courseUnit.Number).FirstOrDefault();

                        if (date != null)
                            wasLate = date.InputDate.Date < DateTime.UtcNow.ToDefaultTimeZone().Date;
                    }
                }
            }
            else
            {
                var maxWeeks = Math.Ceiling((term.ClassEndDate - term.ClassStartDate).TotalDays / 7);
                var tempClassDay = term.ClassStartDate.Date;
                var rangeByWeeks = new List<WeekDetailViewModel>();

                var week = 1;
                while (tempClassDay < term.ClassEndDate.Date)
                {
                    var detail = new WeekDetailViewModel();
                    detail.Week = week;
                    detail.StartDate = tempClassDay;
                    tempClassDay = tempClassDay.AddDays(7);

                    if (tempClassDay >= term.ClassEndDate.Date)
                    {
                        detail.EndDate = term.ClassEndDate.Date;
                    }
                    else
                    {
                        detail.EndDate = tempClassDay.AddDays(-1);
                    }

                    rangeByWeeks.Add(detail);

                    week++;
                }

                var evaluationRange = rangeByWeeks.Where(x => x.Week == _evaluation.Week).FirstOrDefault();
                if (evaluationRange != null)
                    wasLate = evaluationRange.EndDate.Date < DateTime.UtcNow.ToDefaultTimeZone().Date;
            }

            var gradeRegistration = await _gradeRegistrationService.GetByFilters(section, evaluation, null);

            if (gradeRegistration == null)
            {
                gradeRegistration = new GradeRegistration
                {
                    Date = DateTime.UtcNow,
                    EvaluationId = evaluation,
                    SectionId = section,
                    TeacherId = userId,
                    WasLate = wasLate,
                    WasPublished = true
                };
                await _gradeRegistrationService.Insert(gradeRegistration);
            }
            else
            {
                gradeRegistration.Date = DateTime.UtcNow;
                gradeRegistration.WasPublished = true;
                gradeRegistration.WasLate = wasLate;
                gradeRegistration.TeacherId = userId;
                await _gradeRegistrationService.Update(gradeRegistration);
            }

            var currentTemporalGrades = await _temporalGradeService.GetAllByFilters(section, evaluation);
            await _temporalGradeService.DeleteRange(currentTemporalGrades);

            return Ok("Registro satisfactorio");
        }

        /// <summary>
        /// Método para guardar en bloque las notas temporales
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de las notas</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("registrar-notas-unidades/post")]
        public async Task<IActionResult> RegisterEvaluationUnitsPost(RegisterGradeByUnitViewModel model)
        {
            var userId = GetUserId();

            var sec = await _sectionService.GetWithEvaluations(model.SectionId);

            if (!(await _teacherSectionService.AnyBySectionAndTeacher(model.SectionId, userId)))
                return BadRequest("No está permitido para registrar notas en esta sección");

            var iPAddress = GetRequestIP();

            var temporalGrades = await _context.TemporalGrades.Where(x => x.StudentSection.SectionId == model.SectionId).ToListAsync();
            var newTemporalGrades = new List<TemporalGrade>();

            var sectionGrades = await _context.Grades.Where(x => x.StudentSection.SectionId == model.SectionId).ToListAsync();

            foreach (var studentSection in model.StudentSections)
            {
                var temporalGradesByStudent = temporalGrades.Where(x => x.StudentSectionId == studentSection.Id).ToList();

                foreach (var evaluation in studentSection.Evaluations.Where(x => !string.IsNullOrEmpty(x.Grade)))
                {
                    var temporalGrade = temporalGradesByStudent.Where(x => x.EvaluationId == evaluation.Id).FirstOrDefault();
                    var tryParseGrade = decimal.TryParse(evaluation.Grade, out var grade);
                    var notTaken = evaluation.Grade.Trim().ToLower() == "nr";

                    if (temporalGrade is null)
                    {
                        temporalGrade = new TemporalGrade
                        {
                            Attended = !notTaken,
                            EvaluationId = evaluation.Id,
                            StudentSectionId = studentSection.Id,
                            Value = !notTaken ? grade : 0,
                            CreatorIP = iPAddress
                        };

                        newTemporalGrades.Add(temporalGrade);
                    }
                    else
                    {
                        temporalGrade.Attended = !notTaken;
                        temporalGrade.Value = !notTaken ? grade : 0;
                        temporalGrade.CreatorIP = iPAddress;
                    }

                }
            }

            await _temporalGradeService.InsertRange(newTemporalGrades);
            return Ok("Registro satisfactorio");
        }

        /// <summary>
        /// Método para publicar en bloque las notas temporales
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de las notas</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("registrar-notas-unidades/publicar")]
        public async Task<IActionResult> PublushEvaluationUnits(RegisterGradeByUnitViewModel model)
        {
            var userId = GetUserId();

            var sec = await _sectionService.GetWithEvaluations(model.SectionId);
            var term = await _termService.Get(sec.CourseTerm.TermId);
            var evaluations = await _evaluationService.GetAllByCourseAndTerm(sec.CourseTerm.CourseId, sec.CourseTerm.TermId);

            if (!(await _teacherSectionService.AnyBySectionAndTeacher(model.SectionId, userId)))
                return BadRequest("No está permitido para registrar notas en esta sección");

            var iPAddress = GetRequestIP();

            //Orden de merito
            var studentsId = await _context.StudentSections.Where(x => x.SectionId == model.SectionId && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).Select(x => x.StudentId).ToListAsync();
            var hasMeritOrder = await _context.AcademicSummaries.Where(x => studentsId.Contains(x.StudentId) && x.TermId == sec.CourseTerm.TermId).AnyAsync(y => y.MeritOrder > 0);

            if (hasMeritOrder)
                return BadRequest("Se encontraron alumnos con cuadro de mérito asignado.");

            //Historial academico
            var academicHistoriesCount = await _context.AcademicHistories.CountAsync(x => x.SectionId == model.SectionId && studentsId.Contains(x.StudentId));
            var hasAcademicHistories = academicHistoriesCount >= studentsId.Count();

            if (hasAcademicHistories)
                return BadRequest("Se encontraron alumnos con historial académico creado.");

            var grades = await _context.Grades.Where(x => x.StudentSection.SectionId == model.SectionId).ToListAsync();
            var evaluationReport = await _context.EvaluationReports.Where(x => x.SectionId == model.SectionId).FirstOrDefaultAsync();

            foreach (var studentSection in model.StudentSections)
            {
                var gradesByStudent = grades.Where(x => x.StudentSectionId == studentSection.Id).ToList();

                foreach (var evaluation in studentSection.Evaluations)
                {
                    var grade = gradesByStudent.Where(x => x.EvaluationId == evaluation.Id).FirstOrDefault();

                    if (grade != null)
                        continue;

                    var tryParseGrade = decimal.TryParse(evaluation.Grade, out var gradeValue);
                    var notTaken = evaluation.Grade.Trim().ToLower() == "nr";

                    grade = new Grade
                    {
                        Attended = !notTaken,
                        Value = !notTaken ? gradeValue : 0,
                        EvaluationId = evaluation.Id,
                        StudentSectionId = studentSection.Id,
                        CreatorIP = iPAddress
                    };

                    await _gradeService.Insert(grade);
                }

                await _studentSectionService.RecalculateFinalGrade(studentSection.Id);

                if (term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                {
                    var studentSectionEntity = await _studentSectionService.Get(studentSection.Id);
                    var academicHistory = await _academicHistoryService.GetAcademicHistoryBystudentAndCourseId(studentSectionEntity.StudentId, sec.CourseTerm.CourseId, sec.CourseTerm.TermId);

                    if (academicHistory != null)
                    {
                        academicHistory.Grade = studentSectionEntity.FinalGrade;
                        academicHistory.Approved = studentSectionEntity.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI && studentSectionEntity.FinalGrade >= term.MinGrade;

                        await _academicHistoryService.Update(academicHistory);
                    }
                    else
                    {
                        var gradesCount = await _context.Grades.Where(x => x.StudentSectionId == studentSection.Id).CountAsync();
                        if (evaluations.Count() == gradesCount)
                        {
                            academicHistory = new AcademicHistory
                            {
                                CourseId = sec.CourseTerm.CourseId,
                                SectionId = studentSectionEntity.SectionId,
                                StudentId = studentSectionEntity.StudentId,
                                TermId = term.Id,
                                Approved = studentSectionEntity.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI && studentSectionEntity.FinalGrade >= term.MinGrade,
                                Grade = studentSectionEntity.FinalGrade,
                                Try = studentSectionEntity.Try,
                                CurriculumId = studentSectionEntity.Student.CurriculumId
                            };

                            if (evaluationReport != null)
                                academicHistory.EvaluationReportId = evaluationReport.Id;

                            await _academicHistoryService.Insert(academicHistory);
                        }
                    }

                    if (studentSectionEntity.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI)
                        studentSectionEntity.Status = studentSectionEntity.FinalGrade >= term.MinGrade ? ConstantHelpers.STUDENT_SECTION_STATES.APPROVED : ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED;

                    await _studentSectionService.Update(studentSectionEntity);

                    await _academicSummariesService.ReCreateStudentAcademicSummaries(studentSectionEntity.StudentId);
                }
            }

            if (model.StudentSections.Any(y => y.Evaluations.Any(y => !string.IsNullOrEmpty(y.Grade))))
            {
                var evaluationsByCourseUnits = evaluations.Where(x => x.CourseUnitId == model.CourseUnitId).ToList();

                foreach (var item in evaluationsByCourseUnits)
                {
                    var wasLate = false;
                    var _evaluation = await _evaluationService.Get(item.Id);

                    if (_evaluation.CourseUnitId.HasValue)
                    {
                        var scoreInputSchedule = await _scoreInputScheduleService.GetByTermAndCourseComponent(term.Id, sec.CourseTerm.Course.CourseComponentId);

                        if (scoreInputSchedule != null)
                        {
                            var courseUnit = await _courseUnitService.GetAsync(_evaluation.CourseUnitId.Value);
                            if (courseUnit != null)
                            {
                                var date = scoreInputSchedule.Details.Where(x => x.NumberOfUnit == courseUnit.Number).FirstOrDefault();

                                if (date != null)
                                    wasLate = date.InputDate.Date < DateTime.UtcNow.ToDefaultTimeZone().Date;
                            }
                        }
                    }
                    else
                    {
                        var maxWeeks = Math.Ceiling((term.ClassEndDate - term.ClassStartDate).TotalDays / 7);
                        var tempClassDay = term.ClassStartDate.Date;
                        var rangeByWeeks = new List<WeekDetailViewModel>();

                        var week = 1;
                        while (tempClassDay < term.ClassEndDate.Date)
                        {
                            var detail = new WeekDetailViewModel();
                            detail.Week = week;
                            detail.StartDate = tempClassDay;
                            tempClassDay = tempClassDay.AddDays(7);

                            if (tempClassDay >= term.ClassEndDate.Date)
                            {
                                detail.EndDate = term.ClassEndDate.Date;
                            }
                            else
                            {
                                detail.EndDate = tempClassDay.AddDays(-1);
                            }

                            rangeByWeeks.Add(detail);

                            week++;
                        }

                        var evaluationRange = rangeByWeeks.Where(x => x.Week == _evaluation.Week).FirstOrDefault();
                        if (evaluationRange != null)
                            wasLate = evaluationRange.EndDate.Date < DateTime.UtcNow.ToDefaultTimeZone().Date;
                    }


                    var gradeRegistration = await _gradeRegistrationService.GetByFilters(model.SectionId, item.Id, null);

                    if (gradeRegistration == null)
                    {
                        gradeRegistration = new GradeRegistration
                        {
                            Date = DateTime.UtcNow,
                            EvaluationId = item.Id,
                            SectionId = model.SectionId,
                            TeacherId = userId,
                            WasLate = wasLate,
                            WasPublished = true
                        };
                        await _gradeRegistrationService.Insert(gradeRegistration);
                    }
                    else
                    {
                        gradeRegistration.Date = DateTime.UtcNow;
                        gradeRegistration.WasPublished = true;
                        gradeRegistration.WasLate = wasLate;
                        gradeRegistration.TeacherId = userId;

                        await _gradeRegistrationService.Update(gradeRegistration);
                    }

                }
            }

            var currentTemporalGrades = await _context.TemporalGrades.Where(x => x.StudentSection.SectionId == model.SectionId && x.Evaluation.CourseUnitId == model.CourseUnitId).ToListAsync();
            await _temporalGradeService.DeleteRange(currentTemporalGrades);
            return Ok("Registro satisfactorio");
        }

        private static class NUMBERS
        {
            public static List<string> VALUES = new List<string>()
            {
                "CERO",
                "UNO",
                "DOS",
                "TRES",
                "CUATRO",
                "CINCO",
                "SEIS",
                "SIETE",
                "OCHO",
                "NUEVE",
                "DIEZ",
                "ONCE",
                "DOCE",
                "TRECE",
                "CATORCE",
                "QUINCE",
                "DIECISEIS",
                "DIECISIETE",
                "DIECIOCHO",
                "DIECINUEVE",
                "VEINTE"
            };
        }

        /// <summary>
        /// Obtiene la IP pública del docente logueaado
        /// </summary>
        /// <param name="tryUseXForwardHeader"></param>
        /// <returns>IP Pública</returns>
        private string GetRequestIP(bool tryUseXForwardHeader = true)
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

        private T GetHeaderValueAs<T>(string headerName)
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

        private static List<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
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

    }
}
