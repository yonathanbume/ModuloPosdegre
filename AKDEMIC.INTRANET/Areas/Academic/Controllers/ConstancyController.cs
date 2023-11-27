using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Academic.ViewModels.ConstancyViewModels;
using AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Model;
using AKDEMIC.PDF.Services.ReportCardGenerator;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AKDEMIC.INTRANET.Areas.Academic.Controllers
{
    [Area("Academic")]
    [Route("academico/constancias")]
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_RECORD + "," +
        CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF + "," +
        CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        CORE.Helpers.ConstantHelpers.ROLES.CERTIFICATION + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE)]
    public class ConstancyController : BaseController
    {
        private IWebHostEnvironment _hostingEnvironment;
        public IConverter _dinkConverter { get; }
        private readonly IViewRenderService _viewRenderService;
        private readonly IStudentService _studentService;
        private readonly IInternalProcedureService _internalProcedureService;
        private readonly IMapper _mapper;
        private readonly IPostulantService _postulantService;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IRegistryPatternService _registryPatternService;
        private readonly IRecordHistoryService _recordHistoryService;
        private readonly ITextSharpService _pdfSharpService;
        private readonly IDependencyService _dependencyService;
        private readonly IReportCardGeneratorService _reportCardGeneratorService;
        private readonly IDocumentTypeService _documentTypeService;
        private readonly IConfigurationService _configurationService;
        protected ReportSettings _reportSettings;

        public ConstancyController(AkdemicContext context, UserManager<ApplicationUser> userManager,
            IConverter dinkConverter, IViewRenderService viewRenderService, IWebHostEnvironment environment,
            IStudentService studentService,
            IInternalProcedureService internalProcedureService,
            IDataTablesService dataTablesService,
            IMapper mapper,
            IUserService userService,
            ITermService termService,
            IPostulantService postulantService,
            IAcademicSummariesService academicSummariesService,
            IAcademicHistoryService academicHistoryService,
            IRegistryPatternService registryPatternService,
            IRecordHistoryService recordHistoryService,
            ITextSharpService pdfSharpService,
            IDependencyService dependencyService,
            IReportCardGeneratorService reportCardGeneratorService,
            IDocumentTypeService documentTypeService,
            IConfigurationService configurationService,
            IOptionsSnapshot<ReportSettings> reportSettings
        ) : base(context, userManager, userService, termService, dataTablesService)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
            _studentService = studentService;
            _internalProcedureService = internalProcedureService;
            _mapper = mapper;
            _postulantService = postulantService;
            _academicSummariesService = academicSummariesService;
            _academicHistoryService = academicHistoryService;
            _registryPatternService = registryPatternService;
            _recordHistoryService = recordHistoryService;
            _pdfSharpService = pdfSharpService;
            _dependencyService = dependencyService;
            _reportCardGeneratorService = reportCardGeneratorService;
            _documentTypeService = documentTypeService;
            _configurationService = configurationService;
            _reportSettings = reportSettings.Value;
        }

        /// <summary>
        /// Vista inicial de la generación de constancias
        /// </summary>
        /// <returns>Vista inicial</returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Método para crear un nuevo registro asociado a las constancias
        /// </summary>
        /// <param name="model">Modelo con los datos del registro</param>
        /// <returns>Id del nuevo registro</returns>
        [HttpPost("generar")]
        public async Task<IActionResult> CreateRecord(CreateRecordViewModel model)
        {
            var record = new RecordHistory
            {
                Type = model.RecordType,
                Date = DateTime.UtcNow,
                //Number = await _recordHistoryService.GetLatestRecordNumberByType(model.RecordType, term.Year) + 1,
                StudentId = model.StudentId,
                DerivedUserId = model.AcademicRecordStaffId,
                //InternalProcedure = new ENTITIES.Models.DocumentaryProcedure.InternalProcedure
                //{
                //    Number = number + 1,
                //    DependencyId = dependecy.Id,
                //    DocumentTypeId = documentType.Id,
                //    UserId = user.Id,
                //    Subject = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[model.RecordType],
                //    UserInternalProcedures = userInternalProcedure
                //}
            };

            await _recordHistoryService.Insert(record);
            return Ok(record.Id);
        }
        /// <summary>
        /// Método general para generar la constancia relacionada al registro
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("imprimir/{id}")]
        public async Task<IActionResult> PrintConstancy(Guid id)
        {
            var record = await _recordHistoryService.Get(id);
            var number = $"{record.Number.ToString().PadLeft(5, '0')}-{record.Date.Year}";

            switch (record.Type)
            {
                case CORE.Helpers.ConstantHelpers.RECORDS.STUDYRECORD:
                    return await StudyRecord(record.StudentId.Value, number, record.Id);
                case CORE.Helpers.ConstantHelpers.RECORDS.PROOFONINCOME:
                    return await GetProofOfIncome(record.StudentId.Value, number, record.Id);
                case CORE.Helpers.ConstantHelpers.RECORDS.ENROLLMENT:
                    return await GetRecordEnrollment(record.StudentId.Value, number, record.Id);
                case CORE.Helpers.ConstantHelpers.RECORDS.REGULARSTUDIES:
                    return await GetRecordOfRegularStudies(record.StudentId.Value, number, record.Id);
                case CORE.Helpers.ConstantHelpers.RECORDS.EGRESS:
                    return await GetRecordOfEgress(record.StudentId.Value, number, record.Id);
                case CORE.Helpers.ConstantHelpers.RECORDS.MERITCHART:
                    return await MeritChart(record.StudentId.Value, number, record.Id);
                case CORE.Helpers.ConstantHelpers.RECORDS.UPPERFIFTH:
                    return await UpperFifth(record.StudentId.Value, number, record.Id);
                case CORE.Helpers.ConstantHelpers.RECORDS.UPPERTHIRD:
                    return await UpperThird(record.StudentId.Value, number, record.Id);
                case CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICRECORD:
                    return await AcademicRecord(record.StudentId.Value, number, record.Id);
                case CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICPERFORMANCESUMMARY:
                    return await AcademicPerformanceSummary(record.StudentId.Value, number, record.Id);
                default:
                    break;
            }

            return Ok();
        }
        /// <summary>
        /// Retorna la lista de tipos de constancias del sistema en formato para elementos Select
        /// </summary>
        /// <returns>Objeto con la lista de tipos de constancias</returns>
        [HttpGet("tipos/get")]
        public IActionResult GetRecordTypes()
        {
            var selectlist = new List<SelectListItem>()
            {
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.STUDYRECORD.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.STUDYRECORD]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.PROOFONINCOME.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.PROOFONINCOME]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.ENROLLMENT.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.ENROLLMENT]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.REGULARSTUDIES.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.REGULARSTUDIES]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.EGRESS.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.EGRESS]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.MERITCHART.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.MERITCHART]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.UPPERFIFTH.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.UPPERFIFTH]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.UPPERTHIRD.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.UPPERTHIRD]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICRECORD.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICRECORD]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICPERFORMANCESUMMARY.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICPERFORMANCESUMMARY]
                },
                 new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.JOBTITLE.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.JOBTITLE]
                },
                  new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.BACHELOR.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.BACHELOR]
                }
            };

            var result = selectlist
                .Select(x => new
                {
                    id = x.Value,
                    text = x.Text
                })
                .OrderBy(x => x.text)
                .ToList();

            return Ok(new { items = result });
        }
        /// <summary>
        /// Retorna el historial de constancias generadas del estudiante
        /// </summary>
        /// <param name="studentId">Id del estudiante</param>
        /// <param name="type">Tipo de constancia</param>
        /// <returns>Objeto con la lista de constancias</returns>
        [HttpGet("historial/get")]
        public async Task<IActionResult> GetRecordHistory(Guid studentId, int type)
        {
            var data = await _context.RecordHistories
                .Where(x => x.StudentId == studentId && x.Type == type)
                .OrderByDescending(x => x.Date)
                .Select(x => new
                {
                    id = x.Id,
                    date = x.Date.ToLocalDateFormat(),
                    observations = /*x.Observations,*/ string.Empty,
                    number = $"{x.Number.ToString().PadLeft(5, '0')}-{x.Date.Year}"
                })
                .ToListAsync();

            var result = new
            {
                data = data
            };

            return Ok(result);
        }

        /// <summary>
        /// Genera el reporte de notas del estudiante en el periodo académico en formato PDF
        /// </summary>
        /// <param name="studentid">Id del estudiante</param>
        /// <param name="termid">Id del periodo académico</param>
        /// <returns>Documento PDF</returns>
        [Route("notas/{studentid}/{termid}")]
        public async Task<IActionResult> ReportGrades(Guid studentid, Guid termid)
        {
            var pdf = await _reportCardGeneratorService.GetReportCardPDF(studentid, termid);
            return File(pdf.Pdf, "application/pdf");


            //var datenow = DateTime.Now;
            //var term = await _context.Terms.FirstOrDefaultAsync(x => x.Id == termid);

            //var student = await _context.Students
            //              .Include(x => x.User)
            //              .Include(x => x.Career)
            //              .Include(x => x.Campus)
            //              .Include(x => x.Career.Faculty)
            //              .FirstOrDefaultAsync(x => x.Id == studentid);



            //var curriculum = await _context.Curriculums.FirstOrDefaultAsync(x => x.Id == student.CurriculumId);

            //var academicYearCourses = await _context.AcademicYearCourses
            //               .Where(x => x.CurriculumId == curriculum.Id)
            //               .Select(x => new
            //               {
            //                   x.CourseId,
            //                   x.AcademicYear
            //               })
            //               .ToListAsync();


            //var grades = new List<GradeVm>();
            //var totalCredits = 0.0M;
            //var totalGradeCredits = 0.0M;

            //if (term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
            //{
            //    var studentsections = await _context.StudentSections
            //         .Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == termid)
            //         .Select(x => new
            //         {
            //             x.Section.CourseTerm.Course.Credits,
            //             x.Status,
            //             x.FinalGrade,
            //             x.Section.CourseTerm.CourseId,
            //             x.Section.CourseTerm.Course.Name,
            //             x.Section.CourseTerm.Course.Code,
            //         })
            //         .ToListAsync();

            //    foreach (var studentsection in studentsections)
            //    {
            //        var grade = new GradeVm();
            //        var academicyearCourse = academicYearCourses.FirstOrDefault(x => x.CourseId == studentsection.CourseId);

            //        grade.Ciclo = academicyearCourse?.AcademicYear.ToString("00");

            //        grade.Grades = "-";
            //        grade.Credits = studentsection.Credits.ToString("0.0");
            //        grade.Observation = ConstantHelpers.STUDENT_SECTION_STATES.VALUES[studentsection.Status];

            //        grade.Course = studentsection.Name;
            //        grade.Code = studentsection.Code;

            //        totalCredits += studentsection.Credits;
            //        totalGradeCredits += (studentsection.FinalGrade * studentsection.Credits);

            //        grades.Add(grade);
            //    }
            //}
            //else
            //{
            //    var academicHistories = await _context.AcademicHistories
            //                            .Where(x => x.TermId == term.Id && x.StudentId == student.Id && x.SectionId.HasValue
            //                            && x.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED && x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION)
            //                            .Select(x => new
            //                            {
            //                                x.CourseId,
            //                                x.Grade,
            //                                x.Course.Credits,
            //                                x.Approved,
            //                                x.Course.Name,
            //                                x.Course.Code,
            //                                x.Withdraw
            //                            })
            //                            .ToListAsync();

            //    foreach (var item in academicHistories)
            //    {
            //        var grade = new GradeVm();
            //        var academicyearCourse = academicYearCourses.FirstOrDefault(x => x.CourseId == item.CourseId);

            //        grade.Ciclo = academicyearCourse?.AcademicYear.ToString("00");

            //        grade.Grades = item.Withdraw ? "-" : item.Grade.ToString();
            //        grade.Credits = item.Credits.ToString("0.0");
            //        grade.Observation = item.Withdraw ? "Retirado" : item.Approved ? "Aprobado" : "Desaprobado";
            //        grade.Course = item.Name;
            //        grade.Code = item.Code;

            //        if (!item.Withdraw)
            //        {
            //            totalCredits += item.Credits;
            //            totalGradeCredits += (item.Grade * item.Credits);
            //        }

            //        grades.Add(grade);
            //    }
            //}

            //grades = grades.OrderBy(x => x.Ciclo).ThenBy(x => x.Code).ThenBy(x => x.Course).ToList();

            //var model = new ReportGradesViewModel
            //{
            //    Department = GeneralHelpers.GetInstitutionLocation(),
            //    FullName = $"{student.User.Name} {student.User.PaternalSurname} {student.User.MaternalSurname}",
            //    CodeStudent = student.User.UserName,
            //    Career = student.Career.Name,
            //    Campus = student.Campus?.Name,
            //    UniversityName = GeneralHelpers.GetInstitutionName(),
            //    Grades = grades,
            //    ActiveTerm = term.Name,
            //    HeaderText = _reportSettings.ReportGradesHeaderText
            //};
            //if (term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
            //{
            //    if (totalGradeCredits != 0 && totalCredits != 0)
            //        model.Average = Math.Round(Convert.ToDecimal(totalGradeCredits) / Convert.ToDecimal(totalCredits), 2).ToString("0.0");
            //    else
            //        model.Average = "0.00";
            //}
            //else model.Average = "-";

            //model.ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{GeneralHelpers.GetTheme()}/logo-report.png");
            //var globalSettings = new GlobalSettings
            //{
            //    ColorMode = ColorMode.Color,
            //    Orientation = Orientation.Portrait,
            //    PaperSize = PaperKind.A4,
            //    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
            //    DocumentTitle = "Boleta de Notas"
            //};
            //var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/Constancy/ReportGrades.cshtml", model);
            //var objectSettings = new ObjectSettings
            //{
            //    PagesCount = true,
            //    HtmlContent = viewToString,
            //    WebSettings = { DefaultEncoding = "utf-8" },
            //    FooterSettings = {
            //        FontName = "Arial",
            //        FontSize = 9,
            //        Line = false,
            //        Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
            //        Center = "",
            //        Right = "Pág: [page]/[toPage]"
            //    }
            //};
            //var pdf = new HtmlToPdfDocument()
            //{
            //    GlobalSettings = globalSettings,
            //    Objects = { objectSettings }
            //};
            //var fileByte = _dinkConverter.Convert(pdf);
            //if (!User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF) && !User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD))
            //{
            //    if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAB && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAH)
            //        _pdfSharpService.AddWatermarkToAllPages(ref fileByte, "Solo Lectura", 130);
            //}
            //return File(fileByte, "application/pdf");
        }

        #region - CONSTANCIAS - 
        /// <summary>
        /// Método privado para generar el documento "Certificado de Estudios" en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="number">Código de página</param>
        /// <param name="recordId">Id del registro</param>
        /// <returns>Documento PDF</returns>
        private async Task<IActionResult> StudyRecord(Guid id, string number, Guid recordId)
        {
            var term = await GetActiveTerm();
            var result = await _studentService.GetStudyRecord(id);
            var model = _mapper.Map<StudyRecordViewModel>(result);
            model.PathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png");
            model.Term = term != null ? term.Name : "...";
            model.Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES"));
            model.Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation();
            model.University = CORE.Helpers.GeneralHelpers.GetInstitutionName();

            model.PageCode = number;
            model.ImageQR = GetImageQR(recordId);
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 10,
                },
            };
            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/StudyRecord.cshtml", model);
            var objectSettings = new ObjectSettings
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
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileByte = _dinkConverter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileByte, "application/pdf", $"{model.Code} - Certificado de Estudios.pdf");
        }
        /// <summary>
        /// Método privado para generar el documento "Constancia de Ingresos" en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="number">Código de página</param>
        /// <param name="recordId">Id del registro</param>
        /// <returns>Documento PDF</returns>
        private async Task<IActionResult> GetProofOfIncome(Guid id, string number, Guid recordId)
        {
            var student = await _studentService.GetStudentProofOfInCome(id);

            var postulant = await _postulantService.GetPostulantByDniAndTerm(student.User.Dni, student.AdmissionTermId);
            var studentsCount = await _postulantService.GetStudentCounttByTermAndCareer(student.AdmissionTermId, student.CareerId);

            var proof = new ProofOfIncomeViewModel
            {
                Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                Career = student.Career.Name,
                Faculty = student.Career.Faculty.Name,
                StudentSex = student.User.Sex,
                StudentName = student.User.RawFullName,
                EnrollmentNumber = student.User.UserName,
                Modality = student.AdmissionType.Name,
                IncomeYear = student.AdmissionTerm.Year,
                AmountOfStudents = studentsCount,
                Place = (postulant == null) ? 0 : postulant.OrderMerit,
                Score = (postulant == null) ? 0 : postulant.FinalScore,
                PageCode = number,
                ImageQR = GetImageQR(recordId)
            };

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 10,
                },
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/ProofOfIncome.cshtml", proof);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/recordofenrollment.css");
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlContent,
                PagesCount = true,
                WebSettings = { DefaultEncoding = "UTF-8", UserStyleSheet = userStyleSheet },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Constancia-Ingreso.pdf");
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileContents, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }
        /// <summary>
        /// Método privado para generar el documento "Constancia de Matrícula" en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="number">Código de página</param>
        /// <param name="recordId">Id del registro</param>
        /// <returns>Documento PDF</returns>
        private async Task<IActionResult> GetRecordEnrollment(Guid id, string number, Guid recordId)
        {
            var term = await GetActiveTerm();

            if (term == null)
                return BadRequest("No existe periodo activo");

            var student = await _studentService.GetStudentRecordEnrollment(id);

            var record = new RecordOfEnrollmentViewModel
            {
                Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                Career = student.Career.Name,
                Faculty = student.Career.Faculty.Name,
                StudentSex = student.User.Sex,
                StudentName = student.User.RawFullName,
                EnrollmentNumber = student.User.UserName,
                IncomeYear = term.Name,
                Semester = term.Number,
                StartDate = term.ClassStartDate.ToString("dd/MM/yyyy"),
                PageCode = number,
                ImageQR = GetImageQR(recordId)
            };

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 10,
                },
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/RecordOfEnrollment.cshtml", record);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/recordofenrollment.css");
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlContent,
                PagesCount = true,
                WebSettings = { DefaultEncoding = "UTF-8", UserStyleSheet = userStyleSheet },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Constancia-Matricula.pdf");
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileContents, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }
        /// <summary>
        /// Método privado para generar el documento "Constancia de Estudios Regulares" en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="number">Código de página</param>
        /// <param name="recordId">Id del registro</param>
        /// <returns>Documento PDF</returns>
        private async Task<IActionResult> GetRecordOfRegularStudies(Guid id, string number, Guid recordId)
        {
            var student = await _studentService.GetStudentRecordEnrollment(id);

            var record = new RecordOfRegularStudiesViewModel
            {
                Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                Career = student.Career.Name,
                Faculty = student.Career.Faculty.Name,
                StudentSex = student.User.Sex,
                StudentName = student.User.RawFullName,
                EnrollmentNumber = student.User.UserName,
                IncomeYear = student.AdmissionTerm.Name,
                Semester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.TEXT[student.CurrentAcademicYear] : student.CurrentAcademicYear + "°",
                //.ToString("D2"),
                StartDate = student.AdmissionTerm.StartDate.ToString("dd/MM/yyyy"),
                StudentCondition = CORE.Helpers.ConstantHelpers.Student.States.VALUES[student.Status],
                PageCode = number,
                ImageQR = GetImageQR(recordId)
            };

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 10,
                },
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/RecordOfRegularStudies.cshtml", record);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/recordofenrollment.css");
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlContent,
                PagesCount = true,
                WebSettings = { DefaultEncoding = "UTF-8", UserStyleSheet = userStyleSheet },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Constancia-Estudios-Regulares.pdf");
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileContents, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }
        /// <summary>
        /// Método privado para generar el documento "Constancia de Egreso" en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="number">Código de página</param>
        /// <param name="recordId">Id del registro</param>
        /// <returns>Documento PDF</returns>
        private async Task<IActionResult> GetRecordOfEgress(Guid id, string number, Guid recordId)
        {
            var student = await _studentService.GetStudentRecordEnrollment(id);

            var record = new RecordOfEgressViewModel
            {
                Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                Career = student.Career.Name,
                Faculty = student.Career.Faculty.Name,
                StudentSex = student.User.Sex,
                StudentName = student.User.RawFullName,
                EnrollmentNumber = student.User.UserName,
                GraduatedYear = student.GraduationTerm?.Name,
                EndDate = student.GraduationTerm?.EndDate.ToString("dd/MM/yyyy"),
                PageCode = number,
                ImageQR = GetImageQR(recordId)
            };

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 10,
                },
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/RecordOfEgress.cshtml", record);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/recordofenrollment.css");
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlContent,
                PagesCount = true,
                WebSettings = { DefaultEncoding = "UTF-8", UserStyleSheet = userStyleSheet },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Constancia-Egreso.pdf");
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            //return File(fileContents,)
            return File(fileContents, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }
        /// <summary>
        /// Método privado para generar el documento "Reporte de Cuadros de Mérito" en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="number">Código de página</param>
        /// <param name="recordId">Id del registro</param>
        /// <returns>Documento PDF</returns>
        private async Task<IActionResult> MeritChart(Guid id, string number, Guid recordId)
        {
            var student = await _studentService.GetStudentWithCareerAcademicUser(id);

            var result = await _academicSummariesService.GetDetailMeritChart(id);
            var details = _mapper.Map<List<MeritChartDetailViewModel>>(result);
            var average = await _academicSummariesService.GetAverage(student.GraduationTermId);

            var model = new MeritChartViewModel
            {
                Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                StudentName = student.User.FullName.ToUpper(),
                StudentCode = student.User.Dni,
                Career = student.Career.Name.ToUpper(),
                AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper(),
                WeightedAverage = Convert.ToDouble(average),
                Details = details,
                IsGraduated = student.Status == CORE.Helpers.ConstantHelpers.Student.States.GRADUATED,
                StudentGender = student.User.Sex,
                CurrentSemester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.TEXT[student.CurrentAcademicYear] : student.CurrentAcademicYear + "°",
                PageCode = number,
                ImageQR = GetImageQR(recordId)
            };

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 10,
                },
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/MeritChart.cshtml", model);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/meritchart.css");
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlContent,
                PagesCount = true,
                WebSettings = { DefaultEncoding = "UTF-8", UserStyleSheet = userStyleSheet },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Reporte-de-cuadro-meritos.pdf");
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileContents, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }
        /// <summary>
        /// Método privado para generar el documento "Reporte de Quinto Superior" en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="number">Código de página</param>
        /// <param name="recordId">Id del registro</param>
        /// <returns>Documento PDF</returns>
        private async Task<IActionResult> UpperFifth(Guid id, string number, Guid recordId)
        {
            var student = await _studentService.GetStudentWithCareerAcademicUser(id);
            var current = await _academicSummariesService.GetCurrent(id, student.GraduationTermId);

            var result = await _academicSummariesService.GetDetailUpperFith(id);

            var details = _mapper.Map<List<UpperFifthDetailsViewModel>>(result);

            var model = new UpperFifthViewModel
            {
                Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper(),
                Career = student.Career.Name,
                StudentCode = student.User.Dni,
                StudentName = student.User.FullName,
                WeightedAverage = current,
                Details = details,
                StudentSex = student.User.Sex,
                IsGraduated = student.Status == CORE.Helpers.ConstantHelpers.Student.States.GRADUATED,
                CurrentSemester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.TEXT[student.CurrentAcademicYear] : student.CurrentAcademicYear + "°",
                PageCode = number,
                ImageQR = GetImageQR(recordId)
            };

            var rangeOfterms = "";
            for (int i = 0; i < details.Count; i++)
            {
                rangeOfterms += details[i].Term;
                if (i != details.Count)
                    rangeOfterms += "-";
            }

            model.RangeOfTerms = rangeOfterms;

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 10,
                },
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/UpperFifth.cshtml", model);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/upperfifth.css");
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlContent,
                PagesCount = true,
                WebSettings = { DefaultEncoding = "UTF-8", UserStyleSheet = userStyleSheet },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Reporte-de-quinto-superior.pdf");
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileContents, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }
        /// <summary>
        /// Método privado para generar el documento "Reporte de Tercio Superior" en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="number">Código de página</param>
        /// <param name="recordId">Id del registro</param>
        /// <returns>Documento PDF</returns>
        private async Task<IActionResult> UpperThird(Guid id, string number, Guid recordId)
        {
            var student = await _studentService.GetStudentWithCareerAcademicUser(id);
            var current = await _academicSummariesService.GetCurrent(id, student.GraduationTermId);
            var result = await _academicSummariesService.GetDetailUpperThird(id);
            var details = _mapper.Map<List<UpperFifthDetailsViewModel>>(result);

            var model = new UpperFifthViewModel
            {
                Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper(),
                Career = student.Career.Name,
                StudentCode = student.User.Dni,
                StudentName = student.User.FullName,
                WeightedAverage = current,
                Details = details,
                StudentSex = student.User.Sex,
                IsGraduated = student.Status == CORE.Helpers.ConstantHelpers.Student.States.GRADUATED,
                CurrentSemester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.TEXT[student.CurrentAcademicYear] : student.CurrentAcademicYear + "°",
                PageCode = number,
                ImageQR = GetImageQR(recordId)
            };

            var rangeOfterms = "";
            for (int i = 0; i < details.Count; i++)
            {
                rangeOfterms += details[i].Term;
                if (i != details.Count)
                    rangeOfterms += "-";
            }

            model.RangeOfTerms = rangeOfterms;

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 10,
                },
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/UpperThird.cshtml", model);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/upperfifth.css");
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlContent,
                PagesCount = true,
                WebSettings = { DefaultEncoding = "UTF-8", UserStyleSheet = userStyleSheet },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Reporte-de-tercio-superior.pdf");
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileContents, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }
        /// <summary>
        /// Método privado para generar el documento "Registro Académico" en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="number">Código de página</param>
        /// <param name="recordId">Id del registro</param>
        /// <returns>Documento PDF</returns>
        private async Task<IActionResult> AcademicRecord(Guid id, string number, Guid recordId)
        {
            var student = await _studentService.GetStudentWithCareerAdmissionAcademicUser(id);
            var average = await _academicSummariesService.GetAverageBachelorsDegree(id, student.GraduationTermId);

            var coursesDisapproved = await _academicHistoryService.GetCoursesDisapprovedByStudentId(id);
            var coursesRecovered = await _academicHistoryService.GetCoursesRecoveredByStudentId(id);

            var model = new AcademicRecordViewModel
            {
                Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                Student = student.User.FullName,
                EnrollmentCode = student.User.UserName,
                StartYear = $"{student.AdmissionTerm.Year}",
                Speciality = student.AcademicProgram?.Name,
                StartDateSemester = student.AdmissionTerm.Name,
                GraduatedDateSemester = student.GraduationTerm is null ? "-" : student.GraduationTerm.Name,
                RegularStudies = true ? "Si" : "No",
                DisapprovedCourses = coursesDisapproved,
                RecoveredCourses = coursesRecovered,
                Average = average,
                Observations = "-",
                PageCode = number,
                ImageQR = GetImageQR(recordId)
            };

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 10,
                },
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/AcademicRecord.cshtml", model);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/academicRecord.css");
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlContent,
                PagesCount = true,
                WebSettings = { DefaultEncoding = "UTF-8", UserStyleSheet = userStyleSheet },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Record-academic-titulacion.pdf");
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            //return View(@"/Views/Test/Pdf/AcademicRecord.cshtml");
            return File(fileContents, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }
        /// <summary>
        /// Método privado para generar el documento "Resumen de Rendimiento Acacdémico" en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="number">Código de página</param>
        /// <param name="recordId">Id del registro</param>
        /// <returns>Documento PDF</returns>
        private async Task<IActionResult> AcademicPerformanceSummary(Guid id, string number, Guid recordId)
        {
            var student = await _studentService.GetStudentWithCareerAcademicUser(id);

            var result = await _academicSummariesService.GetAcademicPerformanceSummary(id);

            var details = result.Select(x => new AcademicPerformanceSummaryDetailViewModel
            {
                ApprovedCredits = x.ApprovedCredits,
                Average = x.Average,
                DisapprovedCredits = x.DisapprovedCredits,
                Term = x.Term,
                TotalCredits = x.TotalCredits
            }).ToList();

            var model = new AcademicPerformanceSummaryViewModel
            {
                Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                StudentName = student.User.FullName.ToUpper(),
                StudentCode = student.User.Dni,
                Career = student.Career.Name.ToUpper(),
                AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper(),
                Details = details,
                StudentGender = student.User.Sex,
                CurrentSemester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.TEXT[student.CurrentAcademicYear] : student.CurrentAcademicYear + "°",
                PageCode = number,
                ImageQR = GetImageQR(recordId)
            };


            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 10,
                    Right = 10,
                    Top = 10,
                },
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/AcademicPerformanceSummary.cshtml", model);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/meritchart.css");
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlContent,
                PagesCount = true,
                WebSettings = { DefaultEncoding = "UTF-8", UserStyleSheet = userStyleSheet },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Resumen-rendimiento-academico.pdf");
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileContents, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }

        /// <summary>
        /// Método privado para generar el código QR que redireccionará a la vista de verificación de la constancia
        /// </summary>
        /// <param name="recordId">Id del registro</param>
        /// <returns></returns>
        private string GetImageQR(Guid recordId)
        {
            //QR GENERATOR
            var qrGenerator = new QRCodeGenerator();
            var URLAbsolute = Url.GenerateLink(nameof(DocumentVerifierController.ConstancyVerifier), "DocumentVerifier", Request.Scheme, new { id = recordId });
            var qrCodeData = qrGenerator.CreateQrCode(URLAbsolute, QRCodeGenerator.ECCLevel.Q);

            var qrCode = new QRCoder.PngByteQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(3);
            var finalQR = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(qrCodeImage));
            return finalQR;
        }

        #endregion
    }
}
