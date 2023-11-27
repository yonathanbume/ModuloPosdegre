using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.EvaluationReportViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Services.EvaluationReportGenerator;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using ClosedXML.Excel;
using DinkToPdf.Contracts;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = 
        ConstantHelpers.ROLES.SUPERADMIN + "," + 
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," + 
        ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + 
        ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF + "," + 
        ConstantHelpers.ROLES.CERTIFICATION
        )]
    [Route("admin/generacion-actas")]
    public class EvaluationReportController : BaseController
    {
        private readonly AppCustomSettings _appConfig;
        private IWebHostEnvironment _hostingEnvironment;
        public IConverter _dinkConverter { get; }
        private readonly IViewRenderService _viewRenderService;
        private readonly ISectionService _sectionService;
        private readonly ITeacherSectionService _teacherSectionService;
        private readonly IAcademicYearCourseService _academicYearCourseService;
        private readonly IClassScheduleService _classScheduleService;
        private readonly ICurriculumService _curriculumService;
        private readonly IEvaluationReportGeneratorService _evaluationReportGeneratorService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IClassStudentService _classStudentService;
        private readonly IClassService _classService;
        private readonly ITextSharpService _textSharpService;
        private readonly IDirectedCourseService _directedCourseService;
        private readonly ISubstituteExamService _substituteExamService;
        private readonly ICareerService _careerService;
        private readonly IAcademicProgramService _academicProgramService;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly IOptions<CloudStorageCredentials> _options;
        private readonly IUserService _userService;
        private readonly ICourseUnitService _courseUnitService;
        private readonly ICorrectionExamService _correctionExamService;
        private readonly IDigitizedSignatureService _digitizedSignatureService;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IExtraordinaryEvaluationService _extraordinaryEvaluationService;
        private readonly IEvaluationReportService _evaluationReportService;
        private readonly IGradeService _gradeService;
        private readonly IDeferredExamService _deferredExamService;
        private readonly IEvaluationService _evaluationService;
        private readonly IFacultyService _facultyService;
        private readonly IConfigurationService _configurationService;
        private readonly ICourseTermService _courseTermService;
        private readonly IExtraordinaryEvaluationStudentService _extraordinaryEvaluationStudentService;
        private readonly IExtraordinaryEvaluationCommitteeService _extraordinaryEvaluationCommitteeService;
        private readonly IAcademicDepartmentService _academicDepartmentService;
        private readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EvaluationReportController(IOptions<AppCustomSettings> optionsAccessor,
            IConverter dinkConverter,
            IViewRenderService viewRenderService,
            IWebHostEnvironment environment,
            ISectionService sectionService,
            IDataTablesService dataTablesService,
            ITeacherSectionService teacherSectionService,
            ITermService termService,
            IAcademicYearCourseService academicYearCourseService,
            IClassScheduleService classScheduleService,
            ICurriculumService curriculumService,
            IEvaluationReportGeneratorService evaluationReportGeneratorService,
            IStudentSectionService studentSectionService,
            IClassStudentService classStudentService,
            IClassService classService,
            ITextSharpService textSharpService,
            IDirectedCourseService directedCourseService,
            ISubstituteExamService substituteExamService,
            ICareerService careerService,
            IAcademicProgramService academicProgramService,
            IStudentService studentService,
            ICourseService courseService,
            IOptions<CloudStorageCredentials> options,
            IUserService userService,
            ICourseUnitService courseUnitService,
            ICorrectionExamService correctionExamService,
            IDigitizedSignatureService digitizedSignatureService,
            ICloudStorageService cloudStorageService,
            IExtraordinaryEvaluationService extraordinaryEvaluationService,
            IGradeService gradeService,
            IDeferredExamService deferredExamService,
            IEvaluationService evaluationService,
            IFacultyService facultyService,
            IConfigurationService configurationService,
            ICourseTermService courseTermService,
            IExtraordinaryEvaluationStudentService extraordinaryEvaluationStudentService,
            IExtraordinaryEvaluationCommitteeService extraordinaryEvaluationCommitteeService,
            IAcademicDepartmentService academicDepartmentService,
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            IEvaluationReportService evaluationReportService) : base(termService, dataTablesService)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
            _sectionService = sectionService;
            _teacherSectionService = teacherSectionService;
            _academicYearCourseService = academicYearCourseService;
            _classScheduleService = classScheduleService;
            _curriculumService = curriculumService;
            _evaluationReportGeneratorService = evaluationReportGeneratorService;
            _studentSectionService = studentSectionService;
            _classStudentService = classStudentService;
            _classService = classService;
            _textSharpService = textSharpService;
            _directedCourseService = directedCourseService;
            _substituteExamService = substituteExamService;
            _careerService = careerService;
            _academicProgramService = academicProgramService;
            _studentService = studentService;
            _courseService = courseService;
            _options = options;
            _userService = userService;
            _courseUnitService = courseUnitService;
            _correctionExamService = correctionExamService;
            _digitizedSignatureService = digitizedSignatureService;
            _cloudStorageService = cloudStorageService;
            _extraordinaryEvaluationService = extraordinaryEvaluationService;
            _evaluationReportService = evaluationReportService;
            _gradeService = gradeService;
            _deferredExamService = deferredExamService;
            _evaluationService = evaluationService;
            _facultyService = facultyService;
            _configurationService = configurationService;
            _courseTermService = courseTermService;
            _extraordinaryEvaluationStudentService = extraordinaryEvaluationStudentService;
            _extraordinaryEvaluationCommitteeService = extraordinaryEvaluationCommitteeService;
            _context = context;
            _userManager = userManager;
            ////////
            if (optionsAccessor == null) throw new ArgumentNullException(nameof(optionsAccessor));
            _appConfig = optionsAccessor.Value;
        }

        public IActionResult EvaluationReport()
            => Index();

        /// <summary>
        /// Vista donde se generan las actas finales
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        [HttpGet("regulares")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado actas (pendientes y generadas)
        /// </summary>
        /// <param name="typeSearch">Tipo de búsqueda</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="academicDepartmentId">Identificador del departamento académico</param>
        /// <param name="teacherId">Identificador del docente</param>
        /// <param name="teacherCode">Nombre de usuario del docente</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="courseSearch">Nombre del curso</param>
        /// <param name="academicYear">Ciclo del curso</param>
        /// <returns>Objeto que contiene el listado de actas</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(byte typeSearch, Guid? termId, Guid? academicDepartmentId, string teacherId, string teacherCode, Guid? careerId, Guid? curriculumId, string courseSearch, int? academicYear)
        {
            if (!termId.HasValue || termId == Guid.Empty)
            {
                var term = await GetActiveTerm();
                if (term != null) termId = term.Id;
            }

            var sentParameters = _dataTablesService.GetSentParameters();

            if (typeSearch == 1)
            {
                var result = await _sectionService.GetEvaluationReportDatatableV2(sentParameters, termId, academicDepartmentId, teacherId, teacherCode, null, null, null, User, null);
                return Ok(result);
            }
            else
            {
                var result = await _sectionService.GetEvaluationReportDatatableV2(sentParameters, termId, null, null, null, careerId, curriculumId, courseSearch, User, academicYear);
                return Ok(result);
            }
        }

        /// <summary>
        /// Vista donde se generan las actas para cursos dirigidos
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("cursos-dirigidos")]
        public IActionResult DirectedCourse()
        {
            return View();
        }

        [HttpGet("examenes-subsanacion")]
        public IActionResult CorrectionExam()
            => View();

        [HttpGet("get-examenes-subsanacion-datatable")]
        public async Task<IActionResult> GetCorrectionExamDatatable(Guid termId, string search, Guid? careerId, Guid? curriculumId)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _correctionExamService.GetDatatable(parameters, termId, careerId, curriculumId, null, search, User);
            return Ok(result);
        }

        [HttpGet("examenes-subsanacion/imprimir-acta")]
        public async Task<IActionResult> PrintEvaluationReportCorrectionExam(Guid id)
        {
            var result = await _evaluationReportGeneratorService.GetActEvaluationReportCorrectionExam(id);
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(result.Pdf, "application/pdf", $"{result.PdfName}.pdf");
        }

        [HttpGet("examenes-subsanacion/vista-previa")]
        public async Task<IActionResult> CorrectionExamPreview(Guid correctionExamId)
        {
            var correctionExam = await _context.CorrectionExams.Where(x => x.Id == correctionExamId).FirstOrDefaultAsync();
            var evaluationReport = await _context.EvaluationReports.Where(x => x.EntityId == correctionExam.Id).FirstOrDefaultAsync();
            var students = await _context.CorrectionExamStudents.Where(x => x.CorrectionExamId == correctionExam.Id).Include(x => x.Student).ThenInclude(x => x.User).ToListAsync();
            var section = await _context.Sections.Where(x => x.Id == correctionExam.SectionId).FirstOrDefaultAsync();
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
            var course = await _courseService.GetAsync(courseTerm.CourseId);
            var term = await _termService.Get(courseTerm.TermId);

            var academicYearCourse = await _academicYearCourseService.GetAcademicYearCourseByCourseId(course.Id);
            var curriculum = await _curriculumService.Get(academicYearCourse.CurriculumId);

            var model = new ActaScoresViewModel()
            {
                BasicInformation = new BasicInformation(),
                Rows = new List<Row>(),
            };

            if (evaluationReport != null)
            {
                model.Number = evaluationReport.Code;
                model.Date = evaluationReport.CreatedAt.HasValue ? evaluationReport.CreatedAt.Value : DateTime.Now;
                model.BasicInformation.ReceptionDate = evaluationReport.Status == CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED ? evaluationReport.ReceptionDate.ToLocalDateFormat() : string.Empty;
                model.Type = evaluationReport.Type;
                model.LastGradeRegistration = evaluationReport.CreatedAt;
                model.BasicInformation.Resolution = "";
            }

            model.BasicInformation.Course = $"[{curriculum?.Code}-{course?.Code}] {course?.Name}";
            model.BasicInformation.Career = "-";
            model.BasicInformation.Faculty = "-";
            model.BasicInformation.AcademicProgram = "-";
            if (course.CareerId.HasValue)
            {
                var career = await _careerService.Get(course.CareerId.Value);
                model.BasicInformation.Career = career?.Name;

                var faculty = await _facultyService.Get(career.FacultyId);
                model.BasicInformation.Faculty = faculty?.Name;

            }

            if (course.AcademicProgramId.HasValue)
            {
                var academicProgram = await _academicProgramService.Get(course.AcademicProgramId.Value);
                model.BasicInformation.AcademicProgram = academicProgram?.Name;
            }

            model.BasicInformation.TheoreticalHours = course?.TheoreticalHours;
            model.BasicInformation.PracticalHours = course?.PracticalHours;
            model.BasicInformation.EffectiveHours = course?.EffectiveHours;
            model.BasicInformation.Credits = course?.Credits.ToString();
            model.BasicInformation.Sede = "-";
            model.BasicInformation.Term = term.Name;
            model.BasicInformation.IsSummer = term.IsSummer;
            model.BasicInformation.Cycle = academicYearCourse?.AcademicYear.ToString();
            if (string.IsNullOrEmpty(model.BasicInformation.Cycle)) model.BasicInformation.Cycle = "0";
            model.BasicInformation.CourseUnits = 0;
            model.Approbed = term.MinGrade;
            model.BasicInformation.EvaluationByUnits = true;

            var i = 1;
            foreach (var extr in students)
            {
                var row = new Row()
                {
                    Order = i,
                    Code = extr.Student.User.UserName,
                    Surnames = $"{extr.Student.User.PaternalSurname} {extr.Student.User.MaternalSurname}",
                    Names = extr.Student.User.Name,
                    FinalEvaluation = extr.Grade,
                    FinalEvaluationNumber = extr.Grade,
                    FinalEvaluationText = extr.Grade.HasValue ? NUMBERS.VALUES[extr.Grade.Value] : "-",
                    StudentStatus = extr.Grade >= term.MinGrade ? ConstantHelpers.STUDENT_SECTION_STATES.APPROVED : ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED
                };

                model.Rows.Add(row);
                i++;
            }

            return PartialView("_PreviewCorrectionExam", model);

        }


        [HttpGet("examenes-aplazados")]
        public IActionResult DeferredExam()
            => View();

        [HttpGet("get-examenes-aplazados-datatable")]
        public async Task<IActionResult> GetDeferredExamDatatable(Guid termId, string search, Guid? careerId, Guid? curriculumId)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _deferredExamService.GetDeferredExamDatatable(parameters, termId, careerId, curriculumId, null, search, User);
            return Ok(result);
        }

        [HttpGet("examanes-aplazados/imprimir-acta")]
        public async Task<IActionResult> PrintEvaluationReportDeferredExam(Guid id)
        {
            var result = await _evaluationReportGeneratorService.GetActEvaluationReportDeferredExam(id);
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(result.Pdf, "application/pdf", $"{result.PdfName}.pdf");
        }

        [HttpGet("examen-aplazado/vista-previa")]
        public async Task<IActionResult> DeferredExamPreview(Guid deferredExamId)
        {
            var deferredExam = await _context.DeferredExams.Where(x => x.Id == deferredExamId).FirstOrDefaultAsync();
            var evaluationReport = await _context.EvaluationReports.Where(x => x.EntityId == deferredExam.Id).FirstOrDefaultAsync();
            var students = await _context.DeferredExamStudents.Where(x => x.DeferredExamId == deferredExam.Id).Include(x => x.Student).ThenInclude(x => x.User).ToListAsync();
            var section = await _context.Sections.Where(x => x.Id == deferredExam.SectionId).FirstOrDefaultAsync();
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
            var course = await _courseService.GetAsync(courseTerm.CourseId);
            var term = await _termService.Get(courseTerm.TermId);

            var academicYearCourse = await _academicYearCourseService.GetAcademicYearCourseByCourseId(course.Id);
            var curriculum = await _curriculumService.Get(academicYearCourse.CurriculumId);

            var model = new ActaScoresViewModel()
            {
                BasicInformation = new BasicInformation(),
                Rows = new List<Row>(),
            };

            if (evaluationReport != null)
            {
                model.Number = evaluationReport.Code;
                model.Date = evaluationReport.CreatedAt.HasValue ? evaluationReport.CreatedAt.Value : DateTime.Now;
                model.BasicInformation.ReceptionDate = evaluationReport.Status == CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED ? evaluationReport.ReceptionDate.ToLocalDateFormat() : string.Empty;
                model.Type = evaluationReport.Type;
                model.LastGradeRegistration = evaluationReport.CreatedAt;
                model.BasicInformation.Resolution = "";
            }

            model.BasicInformation.Course = $"[{curriculum?.Code}-{course?.Code}] {course?.Name}";
            model.BasicInformation.Career = "-";
            model.BasicInformation.Faculty = "-";
            model.BasicInformation.AcademicProgram = "-";
            if (course.CareerId.HasValue)
            {
                var career = await _careerService.Get(course.CareerId.Value);
                model.BasicInformation.Career = career?.Name;

                var faculty = await _facultyService.Get(career.FacultyId);
                model.BasicInformation.Faculty = faculty?.Name;

            }

            if (course.AcademicProgramId.HasValue)
            {
                var academicProgram = await _academicProgramService.Get(course.AcademicProgramId.Value);
                model.BasicInformation.AcademicProgram = academicProgram?.Name;
            }

            model.BasicInformation.TheoreticalHours = course?.TheoreticalHours;
            model.BasicInformation.PracticalHours = course?.PracticalHours;
            model.BasicInformation.EffectiveHours = course?.EffectiveHours;
            model.BasicInformation.Credits = course?.Credits.ToString();
            model.BasicInformation.Sede = "-";
            model.BasicInformation.Term = term.Name;
            model.BasicInformation.IsSummer = term.IsSummer;
            model.BasicInformation.Cycle = academicYearCourse?.AcademicYear.ToString();
            if (string.IsNullOrEmpty(model.BasicInformation.Cycle)) model.BasicInformation.Cycle = "0";
            model.BasicInformation.CourseUnits = 0;
            model.Approbed = term.MinGrade;
            model.BasicInformation.EvaluationByUnits = true;

            var i = 1;
            foreach (var extr in students)
            {
                var row = new Row()
                {
                    Order = i,
                    Code = extr.Student.User.UserName,
                    Surnames = $"{extr.Student.User.PaternalSurname} {extr.Student.User.MaternalSurname}",
                    Names = extr.Student.User.Name,
                    FinalEvaluation = extr.Grade,
                    FinalEvaluationNumber = extr.Grade,
                    FinalEvaluationText = extr.Grade.HasValue ? NUMBERS.VALUES[extr.Grade.Value] : "-",
                    StudentStatus = extr.Grade >= term.MinGrade ? ConstantHelpers.STUDENT_SECTION_STATES.APPROVED : ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED
                };

                model.Rows.Add(row);
                i++;
            }

            return PartialView("_PreviewDeferredExam", model);

        }

        /// <summary>
        /// Obtiene el listado de actas para cursos dirigidos
        /// </summary>
        /// <param name="teacherId">Identificador del docente</param>
        /// <returns>Objeto que contiene el listado de actas</returns>
        [HttpGet("get-dirigidos")]
        public async Task<IActionResult> DirectedCourseGet(string teacherId)
        {
            var term = await GetActiveTerm();
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _directedCourseService.GetEvaluationReportDatatable(sentParameters, term.Id, teacherId, User);
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se generan las actas para evaluaciones extraordinarias
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("evaluacion-extraordinaria")]
        public IActionResult ExtraordinaryEvaluation()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de actas para evaluaciones extraordinarias
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="teacherId">Identificador del docente</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de actas</returns>
        [HttpGet("evaluacion-extraordinaria/get")]
        public async Task<IActionResult> GetExtraordinaryEvaluationReports(Guid? careerId, string teacherId, Guid? termId, byte? type, string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _extraordinaryEvaluationService.GetExtraordinaryEvaluationsDatatable(sentParameters, searchValue, careerId, teacherId, termId, User, true, type);
            return Ok(result);
        }

        /// <summary>
        /// Genera el acta de una evaluación extraordinaria
        /// </summary>
        /// <param name="evaluationId">Identificador de la evaluación</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("evaluacion-extraordinaria/imprimir/acta")]
        public async Task<IActionResult> ExtraordinaryEvaluationReportPdf(Guid evaluationId)
        {

            var result = await _evaluationReportGeneratorService.GetActEvaluationReportExtraordinaryEvaluation(evaluationId);
            return File(result.Pdf, "application/pdf", $"{result.PdfName}.pdf");

        }

        [HttpGet("evaluacion-extraordinaria/vista-previa")]
        public async Task<IActionResult> ExtraordinaryEvaluationReportPreview(Guid evaluationId)
        {
            var evaluation = await _extraordinaryEvaluationService.Get(evaluationId);
            var evaluationReport = await _evaluationReportService.GetEvaluationReportByFilters(null, evaluation.CourseId, evaluation.TermId, ConstantHelpers.Intranet.EvaluationReportType.EXTRAORDINARY_EVALUATION);
            var students = await _extraordinaryEvaluationStudentService.GetByExtraordinaryEvaluationIdWithData(evaluationId);
            var course = await _courseService.GetAsync(evaluation.CourseId);
            var term = await _termService.Get(evaluation.TermId);
            var academicYearCourse = await _academicYearCourseService.GetAcademicYearCourseByCourseId(course.Id);
            var curriculum = await _curriculumService.Get(academicYearCourse.CurriculumId);

            var model = new ActaScoresViewModel()
            {
                BasicInformation = new BasicInformation(),
                Rows = new List<Row>(),
            };

            if (evaluationReport != null)
            {
                model.Number = evaluationReport.Code;
                model.Date = evaluationReport.CreatedAt.HasValue ? evaluationReport.CreatedAt.Value : DateTime.Now;
                model.BasicInformation.ReceptionDate = evaluationReport.Status == CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED ? evaluationReport.ReceptionDate.ToLocalDateFormat() : string.Empty;
                model.Type = evaluationReport.Type;
                model.LastGradeRegistration = evaluationReport.CreatedAt;
                model.BasicInformation.Resolution = evaluation.Resolution;
            }

            model.BasicInformation.Course = $"[{curriculum?.Code}-{course?.Code}] {course?.Name}";
            model.BasicInformation.Career = "-";
            model.BasicInformation.Faculty = "-";
            model.BasicInformation.AcademicProgram = "-";
            if (course.CareerId.HasValue)
            {
                var career = await _careerService.Get(course.CareerId.Value);
                model.BasicInformation.Career = career?.Name;

                var faculty = await _facultyService.Get(career.FacultyId);
                model.BasicInformation.Faculty = faculty?.Name;

            }

            if (course.AcademicProgramId.HasValue)
            {
                var academicProgram = await _academicProgramService.Get(course.AcademicProgramId.Value);
                model.BasicInformation.AcademicProgram = academicProgram?.Name;
            }

            model.BasicInformation.TheoreticalHours = course?.TheoreticalHours;
            model.BasicInformation.PracticalHours = course?.PracticalHours;
            model.BasicInformation.EffectiveHours = course?.EffectiveHours;
            model.BasicInformation.Credits = course?.Credits.ToString();
            model.BasicInformation.Sede = "-";
            model.BasicInformation.Term = term.Name;
            model.BasicInformation.IsSummer = term.IsSummer;
            model.BasicInformation.Cycle = academicYearCourse?.AcademicYear.ToString();
            if (string.IsNullOrEmpty(model.BasicInformation.Cycle)) model.BasicInformation.Cycle = "0";
            model.BasicInformation.CourseUnits = 2;
            model.Approbed = term.MinGrade;
            model.BasicInformation.EvaluationByUnits = true;

            var i = 1;
            foreach (var extr in students)
            {
                var finalGrade = (int)Math.Round(extr.Grade, 0, MidpointRounding.AwayFromZero);
                var row = new Row()
                {
                    Order = i,
                    Code = extr.Student.User.UserName,
                    Surnames = $"{extr.Student.User.PaternalSurname} {extr.Student.User.MaternalSurname}",
                    Names = extr.Student.User.Name,
                    PartialAverages = new int?[2]
                    {
                        finalGrade,
                        finalGrade
                    },
                    FinalEvaluation = finalGrade,
                    FinalEvaluationNumber = finalGrade,
                    FinalEvaluationText = NUMBERS.VALUES[finalGrade],
                    StudentStatus = finalGrade >= term.MinGrade ? ConstantHelpers.STUDENT_SECTION_STATES.APPROVED : ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED
                };

                model.Rows.Add(row);
                i++;
            }

            return PartialView("_PreviewExtraordinaryEvaluationReport", model);

        }

        /// <summary>
        /// Genera el acta registro de una evaluación extraordinaria
        /// </summary>
        /// <param name="evaluationId">Identificador de la evaluación</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("evaluacion-extraordinaria/imprimir/acta-registro")]
        public async Task<IActionResult> ExtraordinaryEvaluationReportV2Pdf(Guid evaluationId)
        {
            var evaluation = await _extraordinaryEvaluationService.Get(evaluationId);
            var evaluationReport = await _evaluationReportService.GetEvaluationReportByFilters(null, evaluation.CourseId, evaluation.TermId, ConstantHelpers.Intranet.EvaluationReportType.EXTRAORDINARY_EVALUATION);
            if (evaluationReport == null)
            {
                var evaluationReportNumber = await _evaluationReportService.GetMaxNumber(evaluation.TermId);

                evaluationReport = new EvaluationReport
                {
                    PrintQuantity = 1,
                    LastReportGeneratedDate = DateTime.UtcNow,
                    CourseId = evaluation.CourseId,
                    TermId = evaluation.TermId,
                    Type = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReportType.EXTRAORDINARY_EVALUATION,
                    Status = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.GENERATED,
                    Number = evaluationReportNumber + 1,
                    Code = $"{(evaluationReportNumber + 1):000000}",
                    EntityId = evaluation.Id
                };

                await _evaluationReportService.InsertEvaluationReport(evaluationReport);
            }
            else
            {
                evaluationReport.PrintQuantity = evaluationReport.PrintQuantity is null ? 1 : evaluationReport.PrintQuantity + 1;
                if (evaluationReport.Status != CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED)
                {
                    evaluationReport.LastReportGeneratedDate = DateTime.UtcNow;
                }
                await _evaluationReportService.UpdateEvaluationReport(evaluationReport);

            }

            var course = await _courseService.GetAsync(evaluation.CourseId);
            var term = await _termService.Get(evaluation.TermId);
            var teacher = await _userService.Get(evaluation.TeacherId);
            var students = await _extraordinaryEvaluationStudentService.GetByExtraordinaryEvaluationIdWithData(evaluationId);
            var user = await _userService.GetUserByClaim(User);
            var committee = await _extraordinaryEvaluationCommitteeService.GetCommittee(evaluation.Id);
            var academicYearCourse = await _academicYearCourseService.GetAcademicYearCourseByCourseId(course.Id);
            var curriculum = await _curriculumService.Get(academicYearCourse.CurriculumId);
            var signature = await _digitizedSignatureService.GetFirst();

            var model = new ActaScoresViewModel()
            {
                BasicInformation = new BasicInformation(),
                Rows = new List<Row>(),
                Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                Number = evaluationReport.Code,
                Date = evaluationReport.CreatedAt.HasValue ? evaluationReport.CreatedAt.Value : DateTime.Now,
            };

            model.BasicInformation.Teacher = teacher?.FullName;
            model.BasicInformation.Course = $"[{curriculum?.Code}-{course?.Code}] {course?.Name}";
            model.BasicInformation.Career = "-";
            model.BasicInformation.Faculty = "-";
            model.BasicInformation.AcademicProgram = "-";
            if (course.CareerId.HasValue)
            {
                var career = await _careerService.Get(course.CareerId.Value);
                model.BasicInformation.Career = career?.Name;

                var faculty = await _facultyService.Get(career.FacultyId);
                model.BasicInformation.Faculty = faculty?.Name;

            }

            if (course.AcademicProgramId.HasValue)
            {
                var academicProgram = await _academicProgramService.Get(course.AcademicProgramId.Value);
                model.BasicInformation.AcademicProgram = academicProgram?.Name;
            }

            model.BasicInformation.TheoreticalHours = course?.TheoreticalHours;
            model.BasicInformation.PracticalHours = course?.PracticalHours;
            model.BasicInformation.EffectiveHours = course?.EffectiveHours;
            model.BasicInformation.Credits = course?.Credits.ToString();
            model.BasicInformation.Sede = "-";
            model.BasicInformation.Term = term.Name;
            model.BasicInformation.IsSummer = term.IsSummer;
            model.BasicInformation.Cycle = academicYearCourse?.AcademicYear.ToString();
            if (string.IsNullOrEmpty(model.BasicInformation.Cycle)) model.BasicInformation.Cycle = "0";
            //model.BasicInformation.Section = "-";
            model.BasicInformation.ReceptionDate = evaluationReport.Status == CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED ? evaluationReport.ReceptionDate.ToLocalDateFormat() : string.Empty;
            model.BasicInformation.User = user.Name;
            model.BasicInformation.Signature = signature is null ? null : await GeneralHelpers.GetImageForStringPartialView(_options, signature.UrlSignature);
            model.Type = evaluationReport.Type;
            model.FinalQR = GetBarCode(evaluationReport.Code);
            model.BasicInformation.CourseUnits = 2;
            model.Approbed = term.MinGrade;
            model.BasicInformation.Committee = committee.Select(x => x.Teacher.User.FullName).ToList();
            model.BasicInformation.EvaluationByUnits = true;
            model.LastGradeRegistration = evaluationReport.CreatedAt;

            var i = 1;

            foreach (var extr in students)
            {
                var finalGrade = (int)Math.Round(extr.Grade, 0, MidpointRounding.AwayFromZero);
                var row = new Row()
                {
                    Order = i,
                    Code = extr.Student.User.UserName,
                    Surnames = $"{extr.Student.User.PaternalSurname} {extr.Student.User.MaternalSurname}",
                    Names = extr.Student.User.Name,
                    PartialAverages = new int?[2]
                    {
                        finalGrade,
                        finalGrade
                    },
                    FinalEvaluation = finalGrade,
                    FinalEvaluationNumber = finalGrade,
                    FinalEvaluationText = NUMBERS.VALUES[finalGrade],
                    StudentStatus = finalGrade >= term.MinGrade ? ConstantHelpers.STUDENT_SECTION_STATES.APPROVED : ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED
                };

                model.Rows.Add(row);

                i++;
            }

            string viewBackground = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/EvaluationReport/Pdf/ReportBackgroundV2.cshtml", model);
            string viewBody = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/EvaluationReport/ReportV2.cshtml", model);

            var documentTitle = $"R-{course?.Career?.Code}-{curriculum.Code}-{course?.Code}-{evaluationReport.Code}";
            var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css/areas/admin/evaluationreport/format2.css");
            var cssPathBackground = Path.Combine(_hostingEnvironment.WebRootPath, @"css/areas/admin/evaluationreport/format2_background.css");

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNICA)
            {
                cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @$"css/areas/admin/evaluationreport/{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value].ToLower()}/format2.css");
                cssPathBackground = Path.Combine(_hostingEnvironment.WebRootPath, @$"css/areas/admin/evaluationreport/{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value].ToLower()}/format2_background.css");
            }

            DinkToPdf.HtmlToPdfDocument pdfBackground = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 0, Bottom = 0, Left = 5, Right = 5 },
                    DocumentTitle = documentTitle
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = viewBackground,
                        WebSettings = { DefaultEncoding = "utf-8" ,UserStyleSheet = cssPathBackground}
                    }
                }
            };

            var pdfBody = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 65, Bottom = 85, Left = 5, Right = 5 },
                    DocumentTitle = documentTitle
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = viewBody,
                        WebSettings = { DefaultEncoding = "utf-8" , UserStyleSheet = cssPath}
                    }
                }
            };

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var bytesBackground = _dinkConverter.Convert(pdfBackground);
            var bytesBody = _dinkConverter.Convert(pdfBody);

            var bytes = _textSharpService.AddHeaderToAllPages(bytesBody, bytesBackground);
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(bytes, "application/pdf", $"{documentTitle}.pdf");

        }

        /// <summary>
        /// Método para generar las actas en bloque según los filtros del usuario
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="teacherId">Identificador del docente</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("imprimir-bloque-acta-final/v2")]
        public async Task<IActionResult> PrintBlockEvaluationReportV2(Guid termId, string teacherId)
        {
            var teacher = await _userService.Get(teacherId);
            var sections = await _context.Sections.Where(x => x.CourseTerm.TermId == termId && x.TeacherSections.Any(y => y.IsPrincipal && y.TeacherId == teacherId) && (x.EvaluationReports.Any() || (x.AcademicHistories.Any() && x.CourseTerm.Evaluations.Any()))).ToListAsync();
            byte[] fileBytesOutPut = null;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


            var outPdf = new PdfDocument();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var section in sections)
                    {
                        var format = await _evaluationReportGeneratorService.GetActEvaluationReport(section.Id);
                        var pdf = PdfReader.Open(new MemoryStream(format.Pdf), PdfDocumentOpenMode.Import);

                        CopyPages(pdf, outPdf);

                        System.IO.Compression.ZipArchiveEntry zipItem = zip.CreateEntry($"{format.PdfName}.pdf");

                        using (System.IO.MemoryStream originalFileMemoryStream = new System.IO.MemoryStream(format.Pdf))
                        {
                            using (System.IO.Stream entryStream = zipItem.Open())
                            {
                                originalFileMemoryStream.CopyTo(entryStream);
                            }
                        }
                    }
                }

                fileBytesOutPut = memoryStream.ToArray();
            }

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileBytesOutPut, "application/pdf", $"{teacher.FullName}-ACTANOTAS.zip");
        }

        /// <summary>
        /// Genera el acta de un curso dirigido
        /// </summary>
        /// <param name="courseId">Identificador del curso</param>
        /// <param name="teacherId">Identificador del docecnte</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("acta-final-curso-dirigido/{courseId}/{teacherId}")]
        public async Task<IActionResult> DirectedCourseEvalutionReport(Guid courseId, string teacherId)
        {
            var model = new ActaScoresViewModel()
            {
                BasicInformation = new BasicInformation(),
                Rows = new List<Row>(),
                Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png")
            };

            try
            {
                var entity = await _directedCourseService.GetAllByTeacherIdAndCourseId(courseId, teacherId);
                if (entity.Count() < 0)
                    return BadRequest("Error al encontrar el curso dirigido");

                var directedCourse = entity.ToArray();

                var teacher = await _userService.Get(teacherId);
                var course = await _courseService.GetAsync(courseId);
                var career = await _careerService.Get(directedCourse[0].CareerId);
                var term = await _termService.Get(directedCourse[0].TermId);

                model.BasicInformation.Teacher = teacher.FullName;
                model.BasicInformation.Course = course.FullName;
                model.BasicInformation.Career = career.Name;
                model.BasicInformation.Credits = $"{course.Credits}";
                model.BasicInformation.Term = term.Name;
                model.Approbed = term.MinGrade;

                for (int i = 0; i < directedCourse.Count(); i++)
                {
                    foreach (var item in directedCourse[i].Students)
                    {
                        var student = await _studentService.Get(item.StudentId);
                        var user = await _userService.Get(student.UserId);

                        var grade = (int)Math.Round(item.Grade);

                        var row = new Row()
                        {
                            Order = i + 1,
                            Code = user.UserName,
                            Surnames = $"{user.PaternalSurname} {user.MaternalSurname}",
                            Names = user.Name,
                            FinalEvaluationNumber = grade,
                            FinalEvaluation = grade,
                            FinalEvaluationText = NUMBERS.VALUES[grade],
                            FinalEvaluationApprobed = grade >= model.Approbed ? "APROBADO" : "DESAPROBADO"
                        };

                        model.Rows.Add(row);
                    }
                }

                var evaluationReport = await _evaluationReportService.GetEvalutionReportByTeacherIdAndCourseId(courseId, teacherId);

                if (evaluationReport is null)
                {
                    var newEvaluationReport = new EvaluationReport
                    {
                        TeacherId = teacherId,
                        CourseId = courseId,
                        Type = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReportType.DIRECTED_COURSE,
                        Status = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.GENERATED,
                        LastReportGeneratedDate = DateTime.UtcNow,
                        PrintQuantity = 1
                    };

                    await _evaluationReportService.InsertEvaluationReport(newEvaluationReport);
                }
                else
                {
                    evaluationReport.LastReportGeneratedDate = DateTime.UtcNow;
                    evaluationReport.PrintQuantity = evaluationReport.PrintQuantity is null ? 1 : evaluationReport.PrintQuantity++;
                    await _evaluationReportService.UpdateEvaluationReport(evaluationReport);
                }

                var globalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 0, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas de Curso Dirigido"
                };

                var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/EvaluationReport/DirectedCourseReport.cshtml", model);

                var objectSettings = new DinkToPdf.ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = viewToString,
                    WebSettings = { DefaultEncoding = "utf-8" }
                };

                var pdf = new DinkToPdf.HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                var fileByte = _dinkConverter.Convert(pdf);
                _textSharpService.AddWatermarkToAllPages(ref fileByte, ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value], 130);

                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                return File(fileByte, "application/pdf", "Acta de Notas de Curso Dirigido.pdf");

            }
            catch (Exception)
            {
                return BadRequest("Error al generar el reporte.");
            }
        }

        /// <summary>
        /// Genera el acta final de una sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <param name="code">Código del acta</param>
        /// <param name="issueDate">Fecha del acta</param>
        /// <param name="receptionDate">Fecha de recepción del acta</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("acta-final/{sectionId}/{code}")]
        [HttpGet("acta-final/{sectionId}")]
        public async Task<IActionResult> EvaluationReport(Guid sectionId, int? code = null, string issueDate = null, string receptionDate = null)
        {
            var evaluationReport = await _context.EvaluationReports.Where(x => x.SectionId == sectionId).FirstOrDefaultAsync();

            if (evaluationReport is null)
            {
                var anyGradeCorrectionsPending = await _context.GradeCorrections.Where(x => x.Grade.StudentSection.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE && x.Grade.StudentSection.SectionId == sectionId && x.State == ConstantHelpers.GRADECORRECTION_STATES.PENDING).AnyAsync();

                if (anyGradeCorrectionsPending)
                    return BadRequest("Se encontraron correcciones de notas pendientes.");
            }

            var result = await _evaluationReportGeneratorService.GetActEvaluationReport(sectionId, code, issueDate, receptionDate);
            var mainPdf = PdfReader.Open(new MemoryStream(result.Pdf), PdfDocumentOpenMode.Import);

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var outPdf = new PdfDocument())
            {
                CopyPages(mainPdf, outPdf);

                MemoryStream stream = new MemoryStream();
                outPdf.Save(stream, false);
                var bytes = stream.ToArray();

                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                return File(bytes, "application/pdf", $"{result.PdfName}.pdf");
            }
        }

        /// <summary>
        /// Genera el registro de acta de una sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("acta-final-registro/{sectionId}")]
        public async Task<IActionResult> EvaluationReportFormat2(Guid sectionId)
        {
            var result = await _evaluationReportGeneratorService.GetRegisterEvaluationReport(sectionId);
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(result.Pdf, "application/pdf", $"{result.PdfName}.pdf");
        }

        /// <summary>
        /// Genera el registro del acta final
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="teacherId">Identificador del docente</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("acta-final-registro/bloque/{termId}/{teacherId}")]
        public async Task<IActionResult> EvaluationReportFormat2Block(Guid termId, string teacherId)
        {
            var teacher = await _userService.Get(teacherId);
            var sections = await _context.Sections.Where(x => x.CourseTerm.TermId == termId && x.TeacherSections.Any(y => y.IsPrincipal && y.TeacherId == teacherId) && x.EvaluationReports.Any()).ToListAsync();
            byte[] fileBytesOutPut = null;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var outPdf = new PdfDocument();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var section in sections)
                    {
                        var format = await _evaluationReportGeneratorService.GetRegisterEvaluationReport(section.Id);
                        var pdf = PdfReader.Open(new MemoryStream(format.Pdf), PdfDocumentOpenMode.Import);
                        CopyPages(pdf, outPdf);

                        System.IO.Compression.ZipArchiveEntry zipItem = zip.CreateEntry($"{format.PdfName}.pdf");

                        using (System.IO.MemoryStream originalFileMemoryStream = new System.IO.MemoryStream(format.Pdf))
                        {
                            using (System.IO.Stream entryStream = zipItem.Open())
                            {
                                originalFileMemoryStream.CopyTo(entryStream);
                            }
                        }
                    }

                    var stream = new MemoryStream();
                    outPdf.Save(stream, false);
                    var fileBytes = stream.ToArray();

                    System.IO.Compression.ZipArchiveEntry zipItemGeneral = zip.CreateEntry("Registro de Actas.pdf");

                    using (System.IO.MemoryStream originalFileMemoryStream = new System.IO.MemoryStream(fileBytes))
                    {
                        using (System.IO.Stream entryStream = zipItemGeneral.Open())
                        {
                            originalFileMemoryStream.CopyTo(entryStream);
                        }
                    }
                }

                fileBytesOutPut = memoryStream.ToArray();
            }

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileBytesOutPut, "application/pdf", $"{teacher.FullName}-REGISTRO-ACTANOTAS.zip");
        }

        /// <summary>
        /// Genera el acta final detallada de una sección
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
                Number = 0.ToString().PadLeft(6, '0')
            };

            try
            {
                var section = await _sectionService.GetSectionWithTermAndCareer(sectionId);

                if (section == null)
                    section = await _teacherSectionService.GetTeacherSectionsWithTermAndCareer(sectionId);

                var teacherSection = await _teacherSectionService.GetTeacherSectionBySection(sectionId);
                var academicYearCourse = await _academicYearCourseService.GetAcademicYearCourseByCourseId(section.CourseTerm.CourseId);
                var Class = await _classScheduleService.GetClassSchedulesBySectionId(sectionId);
                var courseUnits = await _courseUnitService.GetCourseUnits(section.CourseTerm.Course.Id, section.CourseTerm.Term.Id);
                var user = await _userService.GetUserByClaim(User);
                var signature = await _digitizedSignatureService.GetFirst();

                model.BasicInformation.MinGrade = section.CourseTerm.Term.MinGrade;
                model.BasicInformation.Teacher = teacherSection == null ? "--" : teacherSection.Teacher.User.FullName;
                model.BasicInformation.Course = $"[{section.CourseTerm?.Course?.Code}] {section.CourseTerm?.Course?.Name}";
                model.BasicInformation.Career = section.CourseTerm?.Course?.Career?.Name;
                model.BasicInformation.Faculty = section.CourseTerm?.Course?.Career?.Faculty?.Name;
                model.BasicInformation.AcademicProgram = section.CourseTerm?.Course?.AcademicProgram?.Name;
                model.BasicInformation.TheoreticalHours = section.CourseTerm?.Course?.TheoreticalHours;
                model.BasicInformation.PracticalHours = section.CourseTerm?.Course?.PracticalHours;
                model.BasicInformation.EffectiveHours = section.CourseTerm?.Course?.EffectiveHours;
                model.BasicInformation.Credits = section.CourseTerm?.Course?.Credits.ToString();
                model.BasicInformation.Sede = Class == null ? "--" : Class.Classroom == null ? "--" : Class.Classroom.Building == null ? "--" : Class.Classroom.Building.Name.Substring(0, 1);
                model.BasicInformation.Term = section.CourseTerm?.Term.Name;
                model.BasicInformation.IsSummer = section.CourseTerm?.Term.IsSummer;
                model.BasicInformation.Cycle = academicYearCourse?.AcademicYear.ToString();
                if (string.IsNullOrEmpty(model.BasicInformation.Cycle)) model.BasicInformation.Cycle = "0";
                model.BasicInformation.Section = section.Code;
                model.BasicInformation.CourseUnitsList = courseUnits;
                model.BasicInformation.CourseUnits = courseUnits.Count();
                //model.BasicInformation.ReceptionDate = evaluationReport.Status == CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED ? evaluationReport.ReceptionDate.ToLocalDateFormat() : string.Empty;
                model.BasicInformation.User = user.Name;
                model.BasicInformation.Signature = signature is null ? null : await GeneralHelpers.GetImageForStringPartialView(_options, signature.UrlSignature);

                var sectionStudents = await _studentSectionService.GetAllSectionStudentsWithUserBySectionId(sectionId);
                sectionStudents = sectionStudents.Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).ToList();

                model.Approbed = section.CourseTerm.Term.MinGrade;
                model.Evaluations = (await _evaluationService.GetAllByCourseAndTerm(section.CourseTerm.Course.Id, section.CourseTerm.Term.Id)).ToList();

                for (int i = 0; i < sectionStudents.Count; i++)
                {
                    //var grades = await _gradeService.GetGradesByStudentSectionId(sectionStudents[i].Id);
                    var courseUnitGrades = await _courseUnitService.GetCourseUnitGradesByStudentIdAndSectionId(sectionStudents[i].StudentId, sectionId);
                    var SubstituteExamsStudient = await _substituteExamService.GetSubstituteExamByStudentId(sectionStudents[i].StudentId);
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
                        Try = sectionStudents[i].Try == 0 ? 1 : sectionStudents[i].Try,
                        PartialAverages = averagesByUnits.ToArray(),
                        FinalEvaluation = finalAverage,
                        FinalEvaluationNumber = finalAverage,
                        FinalEvaluationText = NUMBERS.VALUES[finalAverage],
                        HasSusti = susti.HasValue,
                        StudentSectionId = sectionStudents[i].Id,
                        StudentStatus = sectionStudents[i].Status
                    };
                    model.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                model.BasicInformation.Teacher = "";
                model.BasicInformation.Course = "";
                model.BasicInformation.Career = "";
                model.BasicInformation.Credits = "";
                model.BasicInformation.Sede = "";
                model.BasicInformation.Term = "";
                model.BasicInformation.Cycle = "";
                model.BasicInformation.Section = "";
            }
            DinkToPdf.GlobalSettings globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Landscape,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 0, Left = 5, Right = 5 },
                DocumentTitle = "Acta de notas"
            };

            //return View("/Areas/Admin/Views/EvaluationReport/Report.cshtml", model);
            var mainViewToString = string.Empty;

            mainViewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/EvaluationReport/DetailedReportUNAMAD.cshtml", model);

            var mainObjectSettings = new DinkToPdf.ObjectSettings
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
            _textSharpService.AddWatermarkToAllPages(ref fileByte, ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value], 130);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf", "Acta de Notas.pdf");
        }

        /// <summary>
        /// Vista buscador de actas
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("buscador")]
        public IActionResult Search()
        {
            return View();
        }

        /// <summary>
        /// Vista detalle del acta donde se litan los alumnos
        /// </summary>
        /// <param name="id">Identificador del acta</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("acta/{id}/detalle")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var evaluationReport = await _evaluationReportService.Get(id);
            var model = new EvaluationReportDetailViewModel
            {
                Id = evaluationReport.Id
            };

            if (evaluationReport.Type == ConstantHelpers.Intranet.EvaluationReportType.REGULAR)
            {
                var section = await _sectionService.Get(evaluationReport.SectionId.Value);
                var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
                var course = await _courseService.GetAsync(courseTerm.CourseId);

                model.Section = section.Code;
                model.Course = course.FullName;
                model.EvaluationReportCode = evaluationReport.Code;
            }

            ViewBag.EvaluationReportId = id;
            return View(model);
        }

        /// <summary>
        /// Obtiene el listado de alumnos matriculados
        /// </summary>
        /// <param name="evaluationReportId">Identificador del acta</param>
        /// <param name="serachValue">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de alumnos</returns>
        [HttpGet("acta/{evaluationReportId}/datatable")]
        public async Task<IActionResult> DetailDatatable(Guid evaluationReportId, string serachValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentSectionService.GetStudentByEvaluationReportDatatable(sentParameters, evaluationReportId, serachValue);
            return Ok(result);
        }

        /// <summary>
        /// Genera el acta final
        /// </summary>
        /// <param name="evaluationReportId">Identificador del acta</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("acta/{evaluationReportId}/pdf")]
        public async Task<IActionResult> EvaluationReportPdf(Guid evaluationReportId)
        {
            var evaluationReport = await _evaluationReportService.GetEvaluationReportById(evaluationReportId);

            if (evaluationReport.Type == ConstantHelpers.Intranet.EvaluationReportType.EXTRAORDINARY_EVALUATION)
            {
                var extraordinaryEvaluation = await _context.ExtraordinaryEvaluations.Where(x => x.Id == evaluationReport.EntityId).FirstOrDefaultAsync();
                var resultEvaluation = await _evaluationReportGeneratorService.GetActEvaluationReportExtraordinaryEvaluation(extraordinaryEvaluation.Id);
                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                return File(resultEvaluation.Pdf, "application/pdf", $"{resultEvaluation.PdfName}.pdf");
            }

            if (evaluationReport.Type == ConstantHelpers.Intranet.EvaluationReportType.DEFERRED)
            {
                var deferredExam = await _context.DeferredExams.Where(x => x.Id == evaluationReport.EntityId).FirstOrDefaultAsync();
                var resultEvaluation = await _evaluationReportGeneratorService.GetActEvaluationReportDeferredExam(deferredExam.Id);
                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                return File(resultEvaluation.Pdf, "application/pdf", $"{resultEvaluation.PdfName}.pdf");
            }

            if (evaluationReport == null || !evaluationReport.SectionId.HasValue) return BadRequest("No se encontró el acta seleccionada");
            var result = await _evaluationReportGeneratorService.GetActEvaluationReport(evaluationReport.SectionId.Value);
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(result.Pdf, "application/pdf", $"{result.PdfName}.pdf");
        }

        [HttpGet("acta/{evaluationReportId}/excel")]
        public async Task<IActionResult> EvaluationReportExcel(Guid evaluationReportId)
        {
            var evaluationReport = await _evaluationReportService.GetEvaluationReportById(evaluationReportId);
            if (evaluationReport == null || !evaluationReport.SectionId.HasValue) return BadRequest("No se encontró el acta seleccionada");
            var result = await _evaluationReportService.GetEvaluationReportInformation(evaluationReport.SectionId.Value);

            result.Course.Section.Students = result.Course.Section.Students.Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).ToList();

            var dt = new DataTable
            {
                TableName = $"Estudiantes"
            };

            dt.Columns.Add("CÓDIGO");
            dt.Columns.Add("APELLIDOS Y NOMBRES");
            dt.Columns.Add("NOTA");
            dt.Columns.Add("OBSERVACIONES");

            foreach (var item in result.Course.Section.Students)
            {
                dt.Rows.Add(
                    item.UserName,
                    item.FullName,
                    item.FinalGrade,
                    item.FinalGradeText
                    );
            }

            dt.AcceptChanges();

            string fileName = $"{result.BasicInformation.Code}.xlsx";
            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var worksheet = wb.Worksheet(dt.TableName);
                worksheet.Row(1).InsertRowsAbove(9);

                var mergeRangeColumn = 'E';

                worksheet.Cell(2, 1).Value = GeneralHelpers.GetInstitutionName().ToUpper();
                worksheet.Cell(2, 1).Style.Font.FontSize = 12;
                worksheet.Cell(2, 1).Style.Font.Bold = true;
                worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A2:{mergeRangeColumn}2").Merge();

                worksheet.Cell(3, 1).Value = $"Acta de Evaluación Final";
                worksheet.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(3, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A3:{mergeRangeColumn}3").Merge();

                worksheet.Cell(4, 1).Value = "FACULTAD";
                worksheet.Cell(4, 1).Style.Font.Bold = true;
                worksheet.Cell(4, 1).Style.Font.FontSize = 11;
                worksheet.Cell(4, 2).Value = $"'{result.Course.Career.Faculty}";

                worksheet.Cell(5, 1).Value = "ESCUELA PROFESIONAL";
                worksheet.Cell(5, 1).Style.Font.Bold = true;
                worksheet.Cell(5, 1).Style.Font.FontSize = 11;
                worksheet.Cell(5, 2).Value = $"'{result.Course.Career.Name}";

                worksheet.Cell(6, 1).Value = "ASIGNATURA";
                worksheet.Cell(6, 1).Style.Font.Bold = true;
                worksheet.Cell(6, 1).Style.Font.FontSize = 11;
                worksheet.Cell(6, 2).Value = $"{result.Course.Name}";

                worksheet.Cell(4, 3).Value = "SEMESTRE";
                worksheet.Cell(4, 3).Style.Font.Bold = true;
                worksheet.Cell(4, 3).Style.Font.FontSize = 11;
                worksheet.Cell(4, 4).Value = $"'{result.Term.Name}";

                worksheet.Cell(5, 3).Value = "NUMERACIÓN";
                worksheet.Cell(5, 3).Style.Font.Bold = true;
                worksheet.Cell(5, 3).Style.Font.FontSize = 11;
                worksheet.Cell(5, 4).Value = $"'{result.BasicInformation.Code}";

                worksheet.Cell(6, 3).Value = "PLAN DE ESTUDIO";
                worksheet.Cell(6, 3).Style.Font.Bold = true;
                worksheet.Cell(6, 3).Style.Font.FontSize = 11;
                worksheet.Cell(6, 4).Value = $"{result.Course.Curriculum}";

                worksheet.Cell(7, 1).Value = "FECHA";
                worksheet.Cell(7, 1).Style.Font.Bold = true;
                worksheet.Cell(7, 1).Style.Font.FontSize = 11;
                worksheet.Cell(7, 2).Value = result.BasicInformation.LastGradeRegistration.HasValue ? $"{result.BasicInformation.LastGradeRegistration.Value.Day} de {ConstantHelpers.MONTHS.VALUES[result.BasicInformation.LastGradeRegistration.Value.Month]} del {result.BasicInformation.LastGradeRegistration.Value.Year}, {result.BasicInformation.LastGradeRegistration.Value.ToString("HH:mm:ss")}" : "";

                worksheet.Rows().AdjustToContents();
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        /// <summary>
        /// Obtiene el listado de actas generadas según los filtros del usuario
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="courseId">Identificador del curso</param>
        /// <param name="code">Código del acta</param>
        /// <param name="courseSearch">Nombr del curso</param>
        /// <returns>Objeto que contiene el listado de actas</returns>
        [HttpGet("buscador/datatable")]
        public async Task<IActionResult> EvaluationReportDatatable(Guid? termId, Guid? careerId, Guid? curriculumId, Guid? courseId, string code, string courseSearch, bool? onlyReceived)
        {

            var sentParameters = _dataTablesService.GetSentParameters();
            var result = new DataTablesStructs.ReturnedData<object>();
            if (string.IsNullOrEmpty(code))
            {
                result = await _evaluationReportService.GetSearchEvaluationReportDatatable(sentParameters, null, termId, careerId, curriculumId, null, null, courseSearch, onlyReceived);
            }
            else
            {
                result = await _evaluationReportService.GetSearchEvaluationReportDatatable(sentParameters, null, termId, null, null, null, code, null, onlyReceived);
            }

            return Ok(result);

        }

        /// <summary>
        /// Vista donde se listan las actas emititdas
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("buscador/actas-emitidas")]
        public IActionResult EvaluationReportIssued()
            => View();

        /// <summary>
        /// Vista donde se listan las actas emitidas sin notas registradas
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("buscador/actas-emitidas-sin-notas")]
        public IActionResult EvaluationReportIssuedWithoutGrades()
            => View();

        /// <summary>
        /// Vista donde se listan las actas no emitidas
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("buscador/actas-no-emitidas")]
        public IActionResult EvaluationReportNotIssued()
            => View();

        [HttpGet("buscador/actas-recepcionadas")]
        public IActionResult EvaluationReportReceived()
            => View();

        /// <summary>
        /// Obtiene el listado de actas no emitidas
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <returns>Objeto que contiene el listado de actas</returns>
        [HttpGet("buscador/datatable/no-emitidas")]
        public async Task<IActionResult> GetEvaluationReportNotIssued(Guid? termId, Guid? careerId, Guid? curriculumId)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _sectionService.GetSectionsWithOutEvaluationReport(parameters, termId, careerId, curriculumId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene la firma adjuntada
        /// </summary>
        /// <returns>Datos de la firma</returns>
        [HttpGet("get-firma")]
        public async Task<IActionResult> GetSignature()
        {
            var signature = await _digitizedSignatureService.GetFirst();
            return Ok(signature);
        }

        /// <summary>
        /// Método para subir la firma
        /// </summary>
        /// <param name="file">Imagen de la firma</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("subir-firma")]
        public async Task<IActionResult> UploadSignature(IFormFile file)
        {
            var entity = await _digitizedSignatureService.GetFirst();

            if (file is null)
                return BadRequest("Es necesario seleccionar un archivo");

            int MaxContentLength = 1024 * 1024 * 10; //Max 10 MB file
            if (file.Length > MaxContentLength)
                return BadRequest("El archivo excede los 10 MB");

            var extension = Path.GetExtension(file.FileName).ToLower();
            var url = await _cloudStorageService.UploadFile(file.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.DIGITIZED_SIGNATURES,
                extension, ConstantHelpers.FileStorage.SystemFolder.INTRANET);

            if (entity is null)
            {
                entity = new DigitizedSignature
                {
                    UrlSignature = url
                };

                await _digitizedSignatureService.Insert(entity);
                return Ok();
            }
            else
            {
                entity.UrlSignature = url;
                await _digitizedSignatureService.Update(entity);
                return Ok();
            }
        }

        /// <summary>
        /// Obtiene la vista parcial del acta final
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("obtener-vista-previa/{sectionId}")]
        public async Task<IActionResult> _PreviewEvaluationReport(Guid sectionId)
        {
            var sectionStudents = await _context.StudentSections
                .Include(x => x.Student.User)
                .Include(x => x.Section)
                .Where(x => x.SectionId == sectionId && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .OrderBy(x => x.Student.User.FullName)
                .AsNoTracking()
                .ToListAsync();
            var section = await _sectionService.GetSectionWithTermAndCareer(sectionId);

            if (section == null)
                section = await _teacherSectionService.GetTeacherSectionsWithTermAndCareer(sectionId);


            var model = new ActaScoresViewModel()
            {
                BasicInformation = new BasicInformation(),
                Rows = new List<Row>(),
                Img = ""
            };

            var evaluationReport = await _evaluationReportService.GetEvaluationReportBySectionId(sectionId);
            if (evaluationReport != null)
            {
                model.Number = evaluationReport.Code;
                model.Date = evaluationReport.LastReportGeneratedDate ?? DateTime.UtcNow;
            }

            var curriculumId = section.CourseTerm.Course.AcademicYearCourses.Select(x => x.CurriculumId).FirstOrDefault();
            var teacherSections = await _teacherSectionService.GetAllBySection(sectionId);
            var curriculum = await _curriculumService.Get(curriculumId);

            model.BasicInformation.Teacher = teacherSections.Any() ? teacherSections.Where(y => y.IsPrincipal).Count() > 1 ? "CARGA COMPARTIDA" : teacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).FirstOrDefault() : "--";
            model.BasicInformation.Course = $"[{curriculum?.Code}-{section.CourseTerm?.Course?.Code}] {section.CourseTerm?.Course?.Name}";
            model.BasicInformation.Career = section.CourseTerm?.Course?.Career?.Name;
            model.BasicInformation.Faculty = section.CourseTerm?.Course?.Career?.Faculty?.Name;
            model.Type = section.IsDirectedCourse ? ConstantHelpers.Intranet.EvaluationReportType.DIRECTED_COURSE : ConstantHelpers.Intranet.EvaluationReportType.REGULAR;
            model.BasicInformation.Term = section.CourseTerm?.Term.Name;
            model.BasicInformation.Credits = section.CourseTerm?.Course?.Credits.ToString();
            model.BasicInformation.Section = section.Code;
            model.BasicInformation.IsSummer = section.CourseTerm?.Term?.IsSummer;

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

            model.BasicInformation.EvaluationByUnits = evaluationsByUnits;

            if (evaluationsByUnits)
            {
                var courseUnits = await _courseUnitService.GetQuantityCourseUnits(section.CourseTerm.Course.Id, section.CourseTerm.Term.Id);
                model.BasicInformation.CourseUnits = courseUnits;

                for (int i = 0; i < sectionStudents.Count; i++)
                {
                    var courseUnitGrades = await _courseUnitService.GetCourseUnitGradesByStudentIdAndSectionId(sectionStudents[i].StudentId, sectionId);
                    var susti = await _context.SubstituteExams.Where(x => x.SectionId == sectionStudents[i].SectionId && x.StudentId == sectionStudents[i].StudentId && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED).Select(x => x.ExamScore).FirstOrDefaultAsync();
                    var averagesByUnits = courseUnitGrades is null ? null : courseUnitGrades.Select(x => x.Average).ToList();

                    int finalAverage = sectionStudents[i].FinalGrade;

                    if (section.CourseTerm.Term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                    {
                        finalAverage = await _context.AcademicHistories.Where(x => x.SectionId == section.Id && x.StudentId == sectionStudents[i].StudentId).Select(x => x.Grade).FirstOrDefaultAsync();
                    }

                    if (averagesByUnits is null)
                    {
                        averagesByUnits = new List<int?>();
                        for (int a = 0; a < courseUnits; a++)
                        {
                            averagesByUnits.Add(null);
                        }
                    }

                    var row = new Row()
                    {
                        Order = i + 1,
                        Code = sectionStudents[i].Student.User.UserName,
                        Surnames = $"{sectionStudents[i].Student.User.PaternalSurname} {sectionStudents[i].Student.User.MaternalSurname}",
                        Names = sectionStudents[i].Student.User.Name,
                        Try = sectionStudents[i].Try == 0 ? 1 : sectionStudents[i].Try,
                        PartialAverages = averagesByUnits.ToArray(),
                        FinalEvaluation = finalAverage,
                        FinalEvaluationNumber = finalAverage,
                        FinalEvaluationText = NUMBERS.VALUES[finalAverage],
                        HasSusti = susti.HasValue && susti.Value >= section.CourseTerm.Term.MinGrade && ((susti <= 14 && finalAverage == susti.Value) || (susti > 14 && finalAverage == 14)),
                        StudentStatus = sectionStudents[i].Status
                    };

                    model.Rows.Add(row);
                }

            }
            else
            {
                var evaluations = await _context.Evaluations.Where(x => x.CourseTermId == section.CourseTermId).OrderBy(x => x.Week).ThenBy(x => x.Percentage).ToListAsync();
                model.BasicInformation.Evaluations = evaluations.Count();
                model.BasicInformation.EvaluationsList = evaluations;

                for (int i = 0; i < sectionStudents.Count; i++)
                {
                    var grades = await _context.Grades.Where(x => x.StudentSectionId == sectionStudents[i].Id && x.EvaluationId.HasValue)
                        .OrderBy(x => x.Evaluation.Week).ThenBy(x => x.Evaluation.Percentage)
                        .Select(x => new
                        {
                            x.EvaluationId,
                            x.Value
                        })
                        .ToListAsync();

                    var averagesByEvaluations = new List<decimal>();

                    foreach (var evaluation in evaluations)
                    {
                        if (grades.Any(y => y.EvaluationId == evaluation.Id))
                        {
                            averagesByEvaluations.Add(grades.Where(x => x.EvaluationId == evaluation.Id).Select(y => y.Value).FirstOrDefault());
                        }
                        else
                        {
                            averagesByEvaluations.Add(0);
                        }
                    }

                    var susti = await _context.SubstituteExams.Where(x => x.SectionId == sectionStudents[i].SectionId && x.StudentId == sectionStudents[i].StudentId && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED).Select(x => x.ExamScore).FirstOrDefaultAsync();

                    int finalAverage = sectionStudents[i].FinalGrade;

                    if (section.CourseTerm.Term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                    {
                        finalAverage = await _context.AcademicHistories.Where(x => x.SectionId == section.Id && x.StudentId == sectionStudents[i].StudentId).Select(x => x.Grade).FirstOrDefaultAsync();
                    }

                    if (sectionStudents[i].Student.User == null)
                    {
                        sectionStudents[i].Student.User = await _context.Users.Where(x => x.Id == sectionStudents[i].Student.UserId).FirstOrDefaultAsync();
                    }

                    var row = new Row()
                    {
                        Order = i + 1,
                        Code = sectionStudents[i].Student.User.UserName,
                        Surnames = $"{sectionStudents[i].Student.User.PaternalSurname} {sectionStudents[i].Student.User.MaternalSurname}",
                        Names = sectionStudents[i].Student.User.Name,
                        Try = sectionStudents[i].Try == 0 ? 1 : sectionStudents[i].Try,
                        PartialEvaluationAverages = averagesByEvaluations.ToArray(),
                        FinalEvaluation = finalAverage,
                        FinalEvaluationNumber = finalAverage,
                        FinalEvaluationText = NUMBERS.VALUES[finalAverage],
                        HasSusti = susti.HasValue && susti.Value >= section.CourseTerm.Term.MinGrade && ((susti <= 14 && finalAverage == susti.Value) || (susti > 14 && finalAverage == 14)),
                        StudentStatus = sectionStudents[i].Status
                    };

                    model.Rows.Add(row);
                }
            }

            return PartialView(model);
        }

        [HttpGet("cargar-actas-historicas")]
        public IActionResult LoadEvaluationReport()
        {
            return View();
        }

        [HttpPost("cargar-actas-historicas/cargar-excel")]
        public async Task<IActionResult> LoadEvaluationReportFile(IFormFile file)
        {
            var result = new List<LoadEvaluationReportViewModel>();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(file.OpenReadStream(), false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                foreach (DocumentFormat.OpenXml.Spreadsheet.Sheet s in workbookPart.Workbook.Descendants<DocumentFormat.OpenXml.Spreadsheet.Sheet>())
                {
                    if (!(workbookPart.GetPartById(s.Id) is WorksheetPart wsPart)) continue;
                    foreach (DocumentFormat.OpenXml.Spreadsheet.Row row in wsPart.Worksheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Row>().Skip(1))
                    {
                        var model = new LoadEvaluationReportViewModel();

                        try
                        {
                            var cells = row.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>();
                            var termName = OpenXmlHelpers.Excel.GetCellValue(cells.ElementAt(0));
                            var code = OpenXmlHelpers.Excel.GetCellValue(cells.ElementAt(1));
                            var date = OpenXmlHelpers.Excel.GetCellValue(cells.ElementAt(2));

                            model.Term = termName;
                            model.Code = code;
                            model.ReceptionDate = date;

                            if (string.IsNullOrEmpty(termName) && string.IsNullOrEmpty(code) && string.IsNullOrEmpty(date))
                                continue;

                            var tryConvertNumber = int.TryParse(code, out var number);

                            if (termName is null || termName == string.Empty)
                            {
                                model.Observations = "El campo 'PERIODO' es obligatorio.";
                                result.Add(model);
                                continue;
                            }

                            if (code is null || code == string.Empty)
                            {
                                model.Observations = "El campo 'CÓDIGO' es obligatorio.";
                                result.Add(model);
                                continue;
                            }

                            if (date is null || date == string.Empty)
                            {
                                model.Observations = "El campo 'FEC. RECEPCIÓN' es obligatorio.";
                                result.Add(model);
                                continue;
                            }

                            if (!tryConvertNumber)
                            {
                                model.Observations = $"El campo código '{code}' no cumple con el formato esperado.";
                                result.Add(model);
                                continue;
                            }

                            DateTime receptionDate;

                            var dateIsNumber = int.TryParse(date, out int n);
                            if (dateIsNumber)
                            {
                                receptionDate = DateTime.FromOADate(int.Parse(date));
                            }
                            else
                            {
                                var tryParseDate = DateTime.TryParseExact(date, "dd/MM/yyyy", provider: CultureInfo.InvariantCulture, style: DateTimeStyles.None, out receptionDate);

                                if (!tryParseDate)
                                {
                                    model.Observations = $"El campo 'FEC. RECEPCIÓN' no cumple con el formato esperado.";
                                    result.Add(model);
                                    continue;
                                }

                                //receptionDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }

                            var term = await _context.Terms.Where(x => x.Name == termName.Trim()).FirstOrDefaultAsync();

                            if (term is null)
                            {
                                model.Observations = $"El periodo '{termName}' no existe.";
                                result.Add(model);
                                continue;
                            }

                            var evaluationReportEntity = await _context.EvaluationReports.Where(y => y.Number == number && y.Term.Year == term.Year).FirstOrDefaultAsync();

                            if (evaluationReportEntity != null)
                                model.EvaluationReportId = evaluationReportEntity.Id;

                            model.Valid = true;
                            model.Term = term.Name;
                            model.TermId = term.Id;
                            model.Code = number.ToString();
                            model.ReceptionDate = receptionDate.ToDateFormat();
                            model.New = evaluationReportEntity == null;

                            result.Add(model);
                        }
                        catch (Exception ex)
                        {
                            model.Observations = "Error al cargar los datos de la fila.";
                            result.Add(model);
                            continue;
                        }
                    }
                }
            }

            return Ok(result);
        }

        [HttpGet("cargar-actas-historicas/descargar-formato")]
        public IActionResult GetFormatLoadEvaluationReport()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Carga de actas");

                worksheet.Column(1).Width = 20;
                worksheet.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 1).Value = "PERIODO ACADÉMICO";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.Gray;

                worksheet.Column(2).Width = 20;
                worksheet.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 2).Value = "CÓDIGO";
                worksheet.Cell(1, 2).Style.Font.Bold = true;
                worksheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.Gray;

                worksheet.Column(3).Width = 20;
                worksheet.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 3).Value = "FEC. RECEPCIÓN";
                worksheet.Cell(1, 3).Style.Font.Bold = true;
                worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.Gray;

                worksheet.RangeUsed();

                Response.Cookies.Append("fileDownload", "true", new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(1) });

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Listado de Docentes" + DateTime.Now.ToString("dd/MM") + ".xlsx");
                }
            }
        }

        [HttpPost("cargar-actas-historicas")]
        public async Task<IActionResult> LoadEvaluationReport(LoadEvaluationReportHeaderViewModel model)
        {
            var evaluationReportsToAdd = new List<EvaluationReport>();
            var updatedEvaluationReport = 0;

            foreach (var item in model.EvaluationReports.Where(x => x.Valid))
            {
                var date = ConvertHelpers.DatepickerToDatetime(item.ReceptionDate);

                if (item.New)
                {
                    var number = Convert.ToInt32(item.Code);

                    evaluationReportsToAdd.Add(new ENTITIES.Models.Intranet.EvaluationReport
                    {
                        ReceptionDate = date.ToUtcDateTime(),
                        Number = number,
                        Code = $"{number:000000}",
                        RelationId = "CHIZ",
                        TermId = item.TermId
                    });
                }
                else
                {
                    var evaluationReport = await _context.EvaluationReports.Where(x => x.Id == item.EvaluationReportId).FirstOrDefaultAsync();
                    evaluationReport.TermId = item.TermId;
                    evaluationReport.ReceptionDate = date;

                    updatedEvaluationReport++;
                }
            }

            await _context.EvaluationReports.AddRangeAsync(evaluationReportsToAdd);
            await _context.SaveChangesAsync();
            return Ok($"Actas agregadas : {evaluationReportsToAdd.Count()} - Actas Actualizadas : {updatedEvaluationReport}");
        }

        [HttpGet("validar-codigo-acta-seccion")]
        public async Task<IActionResult> ValidateEvaluationReportSection(string code, Guid sectionId)
        {
            var section = await _context.Sections.Where(x => x.Id == sectionId).FirstOrDefaultAsync();
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Id == courseTerm.TermId).FirstOrDefaultAsync();

            var evaluationReport = await _context.EvaluationReports.Where(x => !x.SectionId.HasValue && x.TermId == courseTerm.TermId && x.Code == code.Trim()).FirstOrDefaultAsync();

            if (evaluationReport is null)
            {
                return BadRequest($"No se encontró el acta con código {code} en el periodo {term.Name}");
            }

            var data = new
            {
                evaluationReport.Id,
                receptionDate = evaluationReport.ReceptionDate.ToLocalDateFormat()
            };

            return Ok(data);
        }

        [HttpPost("asignar-acta-codigo-seccion")]
        public async Task<IActionResult> AssignEvaluationReportByCode(Guid evaluationReportId, Guid sectionId, string receptionDate)
        {
            var evaluationReport = await _context.EvaluationReports.Where(x => x.Id == evaluationReportId).FirstOrDefaultAsync();

            if (evaluationReport is null)
                return BadRequest("Error al obtener los datos del acta.");

            evaluationReport.SectionId = sectionId;
            evaluationReport.ReceptionDate = ConvertHelpers.DatepickerToDatetime(receptionDate).ToUtcDateTime();
            await _context.SaveChangesAsync();
            return Ok();
        }

        #region Private Functions 
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
        /// Método para copiar paginas de un pdf a otro
        /// </summary>
        /// <param name="from">PDF inicio</param>
        /// <param name="to">PDF fin</param>
        private void CopyPages(PdfDocument from, PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }

        /// <summary>
        /// Método para obtener el acta final
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <param name="code">Código del acta</param>
        /// <param name="issueDate">Fecha de generación</param>
        /// <param name="receptionDate">Fecha de recepción</param>
        /// <returns>Nombre del archivo y archivo en un arreglo de bytes</returns>
        private async Task<Tuple<string, byte[]>> StandardFormat(Guid sectionId, int? code, string issueDate = null, string receptionDate = null)
        {

            var evaluationReport = await _evaluationReportService.GetEvaluationReportBySectionId(sectionId);

            var modelTest = await _evaluationReportService.GetEvaluationReportInformation(sectionId);

            if (evaluationReport == null)
            {
                var sectionReport = await _sectionService.Get(sectionId);
                var courseTerm = await _courseTermService.GetAsync(sectionReport.CourseTermId);
                evaluationReport = new EvaluationReport
                {
                    PrintQuantity = 1,
                    LastReportGeneratedDate = DateTime.UtcNow,
                    SectionId = sectionId,
                    Type = sectionReport.IsDirectedCourse ? ConstantHelpers.Intranet.EvaluationReportType.DIRECTED_COURSE : ConstantHelpers.Intranet.EvaluationReportType.REGULAR,
                    Status = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.GENERATED,
                    TermId = courseTerm.TermId
                };

                if (code.HasValue && code != 0)
                {
                    evaluationReport.Number = code.Value;
                    evaluationReport.Code = $"{code.Value:000000}";
                }
                else
                {
                    var evaluationReportNumber = await _evaluationReportService.GetMaxNumber(courseTerm.TermId);

                    evaluationReport.Number = evaluationReportNumber + 1;
                    evaluationReport.Code = $"{(evaluationReportNumber + 1):000000}";
                }

                if (!string.IsNullOrEmpty(receptionDate))
                    evaluationReport.ReceptionDate = ConvertHelpers.DatepickerToUtcDateTime(receptionDate);

                await _evaluationReportService.InsertEvaluationReport(evaluationReport);

                if (!string.IsNullOrEmpty(issueDate))
                {
                    evaluationReport.CreatedAt = ConvertHelpers.DatepickerToUtcDateTime(issueDate);
                    await _evaluationReportService.UpdateEvaluationReport(evaluationReport);
                }
            }
            else
            {
                evaluationReport.PrintQuantity = evaluationReport.PrintQuantity is null ? 1 : evaluationReport.PrintQuantity + 1;
                if (evaluationReport.Status != CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED)
                {
                    evaluationReport.LastReportGeneratedDate = DateTime.UtcNow;
                }
                await _evaluationReportService.UpdateEvaluationReport(evaluationReport);
            }

            var model = new ActaScoresViewModel()
            {
                BasicInformation = new BasicInformation(),
                Rows = new List<Row>(),
                Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                Number = evaluationReport.Code,
                Date = evaluationReport.CreatedAt.HasValue ? evaluationReport.CreatedAt.Value : DateTime.Now,
            };

            var section = await _sectionService.GetSectionWithTermAndCareer(sectionId);

            if (section == null)
                section = await _teacherSectionService.GetTeacherSectionsWithTermAndCareer(sectionId);

            var curriculumId = section.CourseTerm.Course.AcademicYearCourses.Select(x => x.CurriculumId).FirstOrDefault();
            var curriculum = await _curriculumService.Get(curriculumId);

            var teacherSections = await _teacherSectionService.GetAllBySection(sectionId);
            var academicYearCourse = await _academicYearCourseService.GetAcademicYearCourseByCourseId(section.CourseTerm.CourseId);
            var Class = await _classScheduleService.GetClassSchedulesBySectionId(sectionId);
            var user = await _userService.GetUserByClaim(User);
            var signature = await _digitizedSignatureService.GetFirst();

            model.BasicInformation.Teacher = teacherSections.Any() ? teacherSections.Where(y => y.IsPrincipal).Count() > 1 ? "CARGA COMPARTIDA" : teacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).FirstOrDefault() : "--";
            model.BasicInformation.Course = $"[{curriculum?.Code}-{section.CourseTerm?.Course?.Code}] {section.CourseTerm?.Course?.Name}";
            model.BasicInformation.Career = section.CourseTerm?.Course?.Career?.Name;
            model.BasicInformation.Faculty = section.CourseTerm?.Course?.Career?.Faculty?.Name;
            model.BasicInformation.AcademicProgram = section.CourseTerm?.Course?.AcademicProgram?.Name;
            model.BasicInformation.TheoreticalHours = section.CourseTerm?.Course?.TheoreticalHours;
            model.BasicInformation.PracticalHours = section.CourseTerm?.Course?.PracticalHours;
            model.BasicInformation.EffectiveHours = section.CourseTerm?.Course?.EffectiveHours;
            model.BasicInformation.Credits = section.CourseTerm?.Course?.Credits.ToString();
            model.BasicInformation.Sede = Class == null ? "--" : Class.Classroom == null ? "--" : Class.Classroom.Building == null ? "--" : Class.Classroom.Building.Name.Substring(0, 1);
            model.BasicInformation.Term = section.CourseTerm?.Term.Name;
            model.BasicInformation.IsSummer = section.CourseTerm?.Term.IsSummer;
            model.BasicInformation.Cycle = academicYearCourse?.AcademicYear.ToString();
            if (string.IsNullOrEmpty(model.BasicInformation.Cycle)) model.BasicInformation.Cycle = "0";
            model.BasicInformation.Section = section.Code;
            model.BasicInformation.ReceptionDate = evaluationReport.Status == CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED ? evaluationReport.ReceptionDate.ToLocalDateFormat() : string.Empty;
            model.BasicInformation.User = user.Name;
            model.BasicInformation.Signature = signature is null ? null : await GeneralHelpers.GetImageForStringPartialView(_options, signature.UrlSignature);
            model.Type = evaluationReport.Type;

            model.FinalQR = GetBarCode(evaluationReport.Code);

            if (section.IsDirectedCourse)
            {
                var resolution = await _context.Resolutions.Where(x => x.TableName == "Section" && x.KeyValue == section.Id).FirstOrDefaultAsync();
                if (resolution != null)
                {
                    model.BasicInformation.Resolution = resolution.Number;
                }
            }


            var sectionStudents = await _studentSectionService.GetAllSectionStudentsWithUserBySectionId(sectionId);
            sectionStudents = sectionStudents.Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).ToList();
            model.Approbed = section.CourseTerm.Term.MinGrade;

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
            model.BasicInformation.EvaluationByUnits = evaluationsByUnits;

            if (evaluationsByUnits)
            {

                var courseUnits = await _courseUnitService.GetQuantityCourseUnits(section.CourseTerm.Course.Id, section.CourseTerm.Term.Id);
                model.BasicInformation.CourseUnits = courseUnits;

                for (int i = 0; i < sectionStudents.Count; i++)
                {
                    var courseUnitGrades = await _courseUnitService.GetCourseUnitGradesByStudentIdAndSectionId(sectionStudents[i].StudentId, sectionId);
                    var susti = await _context.SubstituteExams.Where(x => x.SectionId == sectionStudents[i].SectionId && x.StudentId == sectionStudents[i].StudentId && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED).Select(x => x.ExamScore).FirstOrDefaultAsync();
                    var averagesByUnits = courseUnitGrades is null ? null : courseUnitGrades.Select(x => x.Average).ToList();

                    int finalAverage = sectionStudents[i].FinalGrade;

                    if (section.CourseTerm.Term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                    {
                        finalAverage = await _context.AcademicHistories.Where(x => x.SectionId == section.Id && x.StudentId == sectionStudents[i].StudentId).Select(x => x.Grade).FirstOrDefaultAsync();
                    }

                    if (averagesByUnits is null)
                    {
                        averagesByUnits = new List<int?>();
                        for (int a = 0; a < courseUnits; a++)
                        {
                            averagesByUnits.Add(null);
                        }
                    }

                    var row = new Row()
                    {
                        Order = i + 1,
                        Code = sectionStudents[i].Student.User.UserName,
                        Surnames = $"{sectionStudents[i].Student.User.PaternalSurname} {sectionStudents[i].Student.User.MaternalSurname}",
                        Names = sectionStudents[i].Student.User.Name,
                        Try = sectionStudents[i].Try == 0 ? 1 : sectionStudents[i].Try,
                        PartialAverages = averagesByUnits.ToArray(),
                        FinalEvaluation = finalAverage,
                        FinalEvaluationNumber = finalAverage,
                        FinalEvaluationText = NUMBERS.VALUES[finalAverage],
                        HasSusti = susti.HasValue && susti.Value >= section.CourseTerm.Term.MinGrade && ((susti <= 14 && finalAverage == susti.Value) || (susti > 14 && finalAverage == 14)),
                        StudentStatus = sectionStudents[i].Status
                    };

                    model.Rows.Add(row);
                }

            }
            else
            {
                var evaluations = await _context.Evaluations.Where(x => x.CourseTermId == section.CourseTermId).OrderBy(x => x.Week).ThenBy(x => x.Percentage).ToListAsync();
                model.BasicInformation.Evaluations = evaluations.Count();
                model.BasicInformation.EvaluationsList = evaluations;

                for (int i = 0; i < sectionStudents.Count; i++)
                {
                    var grades = await _context.Grades.Where(x => x.StudentSectionId == sectionStudents[i].Id && x.EvaluationId.HasValue)
                        .OrderBy(x => x.Evaluation.Week).ThenBy(x => x.Evaluation.Percentage)
                        .Select(x => new
                        {
                            x.EvaluationId,
                            x.Value
                        })
                        .ToListAsync();

                    var averagesByEvaluations = new List<decimal>();

                    foreach (var evaluation in evaluations)
                    {
                        if (grades.Any(y => y.EvaluationId == evaluation.Id))
                        {
                            averagesByEvaluations.Add(grades.Where(x => x.EvaluationId == evaluation.Id).Select(y => y.Value).FirstOrDefault());
                        }
                        else
                        {
                            averagesByEvaluations.Add(0);
                        }
                    }

                    var susti = await _context.SubstituteExams.Where(x => x.SectionId == sectionStudents[i].SectionId && x.StudentId == sectionStudents[i].StudentId && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED).Select(x => x.ExamScore).FirstOrDefaultAsync();

                    int finalAverage = sectionStudents[i].FinalGrade;

                    if (section.CourseTerm.Term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                    {
                        finalAverage = await _context.AcademicHistories.Where(x => x.SectionId == section.Id && x.StudentId == sectionStudents[i].StudentId).Select(x => x.Grade).FirstOrDefaultAsync();
                    }

                    var row = new Row()
                    {
                        Order = i + 1,
                        Code = sectionStudents[i].Student.User.UserName,
                        Surnames = $"{sectionStudents[i].Student.User.PaternalSurname} {sectionStudents[i].Student.User.MaternalSurname}",
                        Names = sectionStudents[i].Student.User.Name,
                        Try = sectionStudents[i].Try == 0 ? 1 : sectionStudents[i].Try,
                        PartialEvaluationAverages = averagesByEvaluations.ToArray(),
                        FinalEvaluation = finalAverage,
                        FinalEvaluationNumber = finalAverage,
                        FinalEvaluationText = NUMBERS.VALUES[finalAverage],
                        HasSusti = susti.HasValue && susti.Value >= section.CourseTerm.Term.MinGrade && ((susti <= 14 && finalAverage == susti.Value) || (susti > 14 && finalAverage == 14)),
                        StudentStatus = sectionStudents[i].Status
                    };

                    model.Rows.Add(row);
                }
            }

            var lastGradeRegistration = await _context.Grades.Where(x => x.StudentSection.SectionId == section.Id).OrderByDescending(x => x.CreatedAt).Select(y => y.CreatedAt).FirstOrDefaultAsync();
            var lastSustiRegistration = await _context.SubstituteExams.Where(x => x.SectionId == section.Id && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED).OrderByDescending(x => x.CreatedAt).Select(x => x.CreatedAt).FirstOrDefaultAsync();

            if (lastGradeRegistration.HasValue && lastSustiRegistration.HasValue)
            {
                model.LastGradeRegistration = lastGradeRegistration > lastSustiRegistration ? lastGradeRegistration : lastSustiRegistration;
            }
            else
            {
                model.LastGradeRegistration = lastSustiRegistration.HasValue ? lastSustiRegistration : lastGradeRegistration;
            }


            var academicHsitories = await _context.AcademicHistories.Where(x => x.SectionId == section.Id).ToListAsync();
            academicHsitories.ForEach(x => x.EvaluationReportId = evaluationReport.Id);

            await _context.SaveChangesAsync();

            var documentTitle = $"A-{section.CourseTerm?.Course?.Career?.Code}-{curriculum?.Code}-{section.CourseTerm?.Course?.Code}-{evaluationReport.Code}";

            DinkToPdf.GlobalSettings globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 60, Bottom = 82, Left = 5, Right = 5 },
                DocumentTitle = documentTitle,
                DPI = 300
            };
            DinkToPdf.GlobalSettings globalSettingsHeader = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 0, Bottom = 0, Left = 5, Right = 5 },
                DocumentTitle = documentTitle,
                DPI = 300
            };

            var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css/areas/admin/evaluationreport/format1.css");
            var cssPathBackground = Path.Combine(_hostingEnvironment.WebRootPath, @"css/areas/admin/evaluationreport/format1_background.css");

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNICA)
            {
                cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @$"css/areas/admin/evaluationreport/{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value].ToLower()}/format1.css");
                cssPathBackground = Path.Combine(_hostingEnvironment.WebRootPath, @$"css/areas/admin/evaluationreport/{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value].ToLower()}/format1_background.css");
            }

            var mainViewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/EvaluationReport/Report.cshtml", model);

            var mainObjectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = mainViewToString,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPath }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { mainObjectSettings }
            };

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var fileByte = _dinkConverter.Convert(pdf);
            var headerViewtoString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/EvaluationReport/Pdf/ReportBackground.cshtml", model);

            var headerObjectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = headerViewtoString,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPathBackground }
            };

            var pdfHeader = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettingsHeader,
                Objects = { headerObjectSettings }
            };

            var headerFileByte = _dinkConverter.Convert(pdfHeader);

            fileByte = _textSharpService.AddHeaderToAllPages(fileByte, headerFileByte);
            fileByte = _textSharpService.AddPagination(fileByte);

            //primeras paginas
            var mainPdf = PdfReader.Open(new MemoryStream(fileByte), PdfDocumentOpenMode.Import);

            //pagina final
            //var partialPdf = PdfReader.Open(new MemoryStream(fileByte2), PdfDocumentOpenMode.Import);

            var configuration = await _configurationService.GetByKey(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_WITH_REGISTER);

            if (configuration is null)
            {
                configuration = new ENTITIES.Models.Configuration
                {
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_WITH_REGISTER],
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_WITH_REGISTER
                };
            }

            using (PdfDocument outPdf = new PdfDocument())
            {
                CopyPages(mainPdf, outPdf);
                CopyPages(mainPdf, outPdf);
                CopyPages(mainPdf, outPdf);

                if (bool.Parse(configuration.Value))
                {
                    //CopyPages(partialPdf, outPdf);
                }

                MemoryStream stream = new MemoryStream();
                outPdf.Save(stream, false);
                var bytes = stream.ToArray();
                //_textSharpService.AddWatermarkToAllPages(ref bytes, ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value], 130);
                return new Tuple<string, byte[]>(documentTitle, bytes);
            }
        }

        /// <summary>
        /// Método para obtener el acta final
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <param name="code">Código del acta</param>
        /// <param name="issueDate">Fecha de generación</param>
        /// <param name="receptionDate">Fecha de recepción</param>
        /// <returns>Nombre del archivo y archivo en un arreglo de bytes</returns>
        private async Task<Tuple<string, byte[]>> ReportFormat1(Guid sectionId, int? code, string issueDate = null, string receptionDate = null)
        {
            var evaluationReport = await _evaluationReportService.GetEvaluationReportBySectionId(sectionId);

            var section = await _context.Sections
               .Where(x => x.Id == sectionId)
               .Select(x => new
               {
                   x.Code,
                   x.CourseTermId,
                   x.IsDirectedCourse,
                   CourseCode = x.CourseTerm.Course.Code,
                   CourseName = x.CourseTerm.Course.Name,
                   x.CourseTerm.CourseId,
                   x.CourseTerm.TermId,
                   CurriculumId = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.CurriculumId).FirstOrDefault(),

                   x.CourseTerm.Course.CareerId,
                   CareerName = x.CourseTerm.Course.CareerId.HasValue ? x.CourseTerm.Course.Career.Name : "",

                   FacultyId = x.CourseTerm.Course.CareerId.HasValue ? x.CourseTerm.Course.Career.FacultyId : Guid.Empty,
                   FacultyName = x.CourseTerm.Course.CareerId.HasValue ? x.CourseTerm.Course.Career.Faculty.Name : "",

                   DirectorId = x.CourseTerm.Course.CareerId.HasValue ? x.CourseTerm.Course.Career.CareerDirectorId : null,
                   AcademicProgram = x.CourseTerm.Course.AcademicProgramId.HasValue ? x.CourseTerm.Course.AcademicProgram.Name : "",

                   x.CourseTerm.Course.TheoreticalHours,
                   x.CourseTerm.Course.PracticalHours,
                   x.CourseTerm.Course.EffectiveHours,
                   x.CourseTerm.Course.Credits,

                   TermName = x.CourseTerm.Term.Name,
                   x.CourseTerm.Term.IsSummer,
                   x.CourseTerm.Term.MinGrade
               }).FirstOrDefaultAsync();

            if (evaluationReport == null)
            {
                var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
                evaluationReport = new EvaluationReport
                {
                    PrintQuantity = 1,
                    LastReportGeneratedDate = DateTime.UtcNow,
                    SectionId = sectionId,
                    Type = section.IsDirectedCourse ? ConstantHelpers.Intranet.EvaluationReportType.DIRECTED_COURSE : ConstantHelpers.Intranet.EvaluationReportType.REGULAR,
                    Status = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.GENERATED,
                    TermId = courseTerm.TermId
                };

                if (code.HasValue && code != 0)
                {
                    evaluationReport.Number = code.Value;
                    evaluationReport.Code = $"{code.Value:000000}";
                }
                else
                {
                    var evaluationReportNumber = await _evaluationReportService.GetMaxNumber(courseTerm.TermId);

                    evaluationReport.Number = evaluationReportNumber + 1;
                    evaluationReport.Code = $"{(evaluationReportNumber + 1):000000}";
                }

                await _evaluationReportService.InsertEvaluationReport(evaluationReport);
            }
            else
            {
                evaluationReport.PrintQuantity = evaluationReport.PrintQuantity is null ? 1 : evaluationReport.PrintQuantity + 1;
                if (evaluationReport.Status != CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED)
                {
                    evaluationReport.LastReportGeneratedDate = DateTime.UtcNow;
                }
                await _evaluationReportService.UpdateEvaluationReport(evaluationReport);
            }

            var model = new ActaScoresViewModel()
            {
                BasicInformation = new BasicInformation(),
                Rows = new List<Row>(),
                Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                Number = evaluationReport.Code,
                CareerDirector = "-",
                Sender = "-",
                Date = evaluationReport.CreatedAt.HasValue ? evaluationReport.CreatedAt.Value : DateTime.Now
            };

            model.Sender = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_SENDER);

            //QR GENERATOR
            //QRCodeGenerator qrGenerator = new QRCodeGenerator();
            //var URLAbsolute = Url.GenerateLink(nameof(DocumentVerifierController.EvaluationReportVerifier), "DocumentVerifier", Request.Scheme, new { id = evaluationReport.Id });
            //QRCodeData qrCodeData = qrGenerator.CreateQrCode(URLAbsolute,
            //QRCodeGenerator.ECCLevel.Q);
            //QRCode qrCode = new QRCode(qrCodeData);
            //Bitmap qrCodeImage = qrCode.GetGraphic(3);
            //var bitMap = ConvertHelpers.BitmapToBytes(qrCodeImage);
            //var finalQR = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(bitMap));
            //model.FinalQR = finalQR;

            var curriculum = await _curriculumService.Get(section.CurriculumId);
            var teacherSection = await _teacherSectionService.GetTeacherSectionBySection(sectionId);
            var academicYearCourse = await _academicYearCourseService.GetAcademicYearCourseByCourseId(section.CourseId);
            var classSchedule = await _classScheduleService.GetClassSchedulesBySectionId(sectionId);
            var courseUnits = await _courseUnitService.GetQuantityCourseUnits(section.CourseId, section.TermId);

            var user = await _userService.GetUserByClaim(User);
            var signature = await _digitizedSignatureService.GetFirst();

            if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAMAD)
            {
                if (!string.IsNullOrEmpty(section.DirectorId))
                {
                    var careerDirector = await _userService.Get(section.DirectorId);
                    model.CareerDirector = careerDirector.FullName;
                }
            }
            else
            {
                model.CareerDirector = "";
                if (section.CareerId.HasValue)
                {
                    var academicDepartment = await _context.AcademicDepartments.FirstOrDefaultAsync(x => x.CareerId == section.CareerId);
                    if (academicDepartment == null)
                        academicDepartment = await _context.AcademicDepartments.FirstOrDefaultAsync(x => x.CareerId == section.FacultyId);

                    if (academicDepartment != null && !string.IsNullOrEmpty(academicDepartment.AcademicDepartmentDirectorId))
                    {
                        var departmentDirector = await _userService.Get(academicDepartment.AcademicDepartmentDirectorId);
                        model.CareerDirector = departmentDirector.FullName;
                    }
                }
            }

            model.BasicInformation.Teacher = teacherSection == null ? "--" : teacherSection.Teacher.User.FullName;
            model.BasicInformation.Course = $"[{curriculum?.Code}-{section.CourseCode}] {section.CourseName}";
            model.BasicInformation.Career = section.CareerName;
            model.BasicInformation.Faculty = section.FacultyName;
            model.BasicInformation.AcademicProgram = section.AcademicProgram;
            model.BasicInformation.TheoreticalHours = section.TheoreticalHours;
            model.BasicInformation.PracticalHours = section.PracticalHours;
            model.BasicInformation.EffectiveHours = section.EffectiveHours;
            model.BasicInformation.Credits = section.Credits.ToString();
            model.BasicInformation.Sede = classSchedule == null ? "--" : classSchedule.Classroom == null ? "--" : classSchedule.Classroom.Building == null ? "--" : classSchedule.Classroom.Building.Name.Substring(0, 1);
            model.BasicInformation.Term = section.TermName;
            model.BasicInformation.IsSummer = section.IsSummer;
            model.BasicInformation.Cycle = academicYearCourse?.AcademicYear.ToString();
            if (string.IsNullOrEmpty(model.BasicInformation.Cycle)) model.BasicInformation.Cycle = "0";
            model.BasicInformation.Curriculum = academicYearCourse?.Curriculum?.Name;
            model.BasicInformation.Section = section.Code;
            model.BasicInformation.CourseUnits = courseUnits;
            model.BasicInformation.ReceptionDate = evaluationReport.Status == CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED ? evaluationReport.ReceptionDate.ToLocalDateFormat() : string.Empty;
            model.BasicInformation.User = user.Name;
            model.BasicInformation.Signature = signature is null ? null : await GeneralHelpers.GetImageForStringPartialView(_options, signature.UrlSignature);
            model.Type = evaluationReport.Type;

            var studentSections = await _context.StudentSections
                .Where(x => x.SectionId == sectionId && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .OrderBy(x => x.Student.User.FullName)
                .Select(x => new
                {
                    x.StudentId,
                    x.Student.User.UserName,
                    x.Student.User.PaternalSurname,
                    x.Student.User.MaternalSurname,
                    x.Student.User.Name,
                    x.Try,
                    x.Status,
                    x.FinalGrade
                }).ToListAsync();

            model.Approbed = section.MinGrade;

            var substituteExams = await _context.SubstituteExams
                .Where(x => x.Section.CourseTerm.CourseId == section.CourseId
                && x.Section.CourseTerm.TermId == section.TermId)
                //.Select(x => x.ExamScore)
                .AsNoTracking()
                .ToListAsync();

            var count = 1;
            foreach (var studentSection in studentSections)
            {
                var courseUnitGrades = await _courseUnitService.GetCourseUnitGradesByStudentIdAndSectionId(studentSection.StudentId, sectionId);
                var substituteExam = substituteExams.FirstOrDefault(x => x.StudentId == studentSection.StudentId);

                var averagesByUnits = courseUnitGrades is null ? null : courseUnitGrades.Select(x => x.Average).ToList();
                if (averagesByUnits == null)
                {
                    averagesByUnits = new List<int?>();
                    for (var a = 0; a < courseUnits; a++)
                        averagesByUnits.Add(null);
                }

                var finalAverage = studentSection.FinalGrade;
                if (substituteExam != null) finalAverage = (int)substituteExam.ExamScore;

                var row = new Row
                {
                    Order = count,
                    Code = studentSection.UserName,
                    Surnames = $"{studentSection.PaternalSurname} {studentSection.MaternalSurname}",
                    Names = studentSection.Name,
                    Try = studentSection.Try == 0 ? 1 : studentSection.Try,
                    PartialAverages = averagesByUnits.ToArray(),
                    FinalEvaluation = finalAverage,
                    FinalEvaluationNumber = finalAverage,
                    FinalEvaluationText = NUMBERS.VALUES[finalAverage],
                    HasSusti = substituteExam != null,
                    StudentStatus = studentSection.Status
                    //FinalEvaluation = sectionStudents[i].FinalGrade.ToString(),
                    //FinalEvaluationNumber = sectionStudents[i].FinalGrade,
                    //FinalEvaluationText = NUMBERS.VALUES[sectionStudents[i].FinalGrade],

                };
                model.Rows.Add(row);
                count++;
            }

            var academicHistories = await _context.AcademicHistories.Where(x => x.SectionId == sectionId).ToListAsync();
            academicHistories.ForEach(x => x.EvaluationReportId = evaluationReport.Id);
            await _context.SaveChangesAsync();

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = "Acta de notas"
            };

            var mainViewToString = string.Empty;
            var leftfooter = "";
            var rightfooter = "";
            var centerfooter = "";

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD)
            {
                leftfooter = "DIRECCION UNIVERSITARIA DE ASUNTOS ACADEMICOS";
                rightfooter = $"Impreso por: {model.BasicInformation.User} {DateTime.UtcNow.ToLocalDateTimeFormat()}";
                centerfooter = "Página [page]/[toPage]";
                mainViewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/EvaluationReport/ReportUNAMAD.cshtml", model);
            }
            else
            {
                leftfooter = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}";
                mainViewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/EvaluationReport/ReportUNJBG.cshtml", model);
                rightfooter = "Página [page] de [toPage]";
            }

            var mainObjectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = mainViewToString,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 6,
                    Line = false,
                    Left = leftfooter,
                    Right =rightfooter,
                    Center =centerfooter
                }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { mainObjectSettings }
            };
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var fileByte = _dinkConverter.Convert(pdf);
            _textSharpService.AddWatermarkToAllPages(ref fileByte, ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value], 130);

            var mainPdf = PdfReader.Open(new MemoryStream(fileByte), PdfDocumentOpenMode.Import);
            using var outPdf = new PdfDocument();
            CopyPages(mainPdf, outPdf);

            var stream = new MemoryStream();
            outPdf.Save(stream, false);
            return new Tuple<string, byte[]>("Acta de Notas", stream.ToArray());
        }

        /// <summary>
        /// Método para obtener el acta final
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="teacherId">Identificador del docente</param>
        /// <returns>Nombre del archivo y archivo en un arreglo de bytes</returns>
        private async Task<byte[]> StandardBlockReport(Guid termId, string teacherId)
        {
            var sections = await _context.Sections.Where(x => x.CourseTerm.TermId == termId && x.TeacherSections.Any(y => y.IsPrincipal && y.TeacherId == teacherId) && (x.EvaluationReports.Any() || (x.AcademicHistories.Any() && x.CourseTerm.Evaluations.Any()))).ToListAsync();
            byte[] fileBytesOutPut = null;

            var outPdf = new PdfDocument();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var section in sections)
                    {
                        var format = await StandardFormat(section.Id, null);
                        var pdf = PdfReader.Open(new MemoryStream(format.Item2), PdfDocumentOpenMode.Import);
                        CopyPages(pdf, outPdf);

                        System.IO.Compression.ZipArchiveEntry zipItem = zip.CreateEntry($"{format.Item1}.pdf");

                        using (System.IO.MemoryStream originalFileMemoryStream = new System.IO.MemoryStream(format.Item2))
                        {
                            using (System.IO.Stream entryStream = zipItem.Open())
                            {
                                originalFileMemoryStream.CopyTo(entryStream);
                            }
                        }
                    }

                    var stream = new MemoryStream();
                    outPdf.Save(stream, false);
                    var fileBytes = stream.ToArray();

                    System.IO.Compression.ZipArchiveEntry zipItemGeneral = zip.CreateEntry("Registro de Actas.pdf");

                    using (System.IO.MemoryStream originalFileMemoryStream = new System.IO.MemoryStream(fileBytes))
                    {
                        using (System.IO.Stream entryStream = zipItemGeneral.Open())
                        {
                            originalFileMemoryStream.CopyTo(entryStream);
                        }
                    }
                }

                fileBytesOutPut = memoryStream.ToArray();
            }

            return fileBytesOutPut;
        }

        /// <summary>
        /// Método para obtener las actas finales en bloque según lo filtros del usuario
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="departmentId">Identificador del departamento académico</param>
        /// <param name="teacherId">identificador del docente</param>
        /// <returns>Arreglo de bytes que contiene el archivo PDF</returns>
        private async Task<byte[]> BlockReportFormat1(Guid? termId, Guid? careerId, Guid? departmentId, string teacherId)
        {
            var block = new List<EvaluationReportBlockViewModel>();

            Term term;
            if (termId.HasValue) term = await _termService.Get(termId.Value);
            else term = await _termService.GetActiveTerm();

            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == term.Id)
                .AsNoTracking();

            var queryGrades = _context.Grades
                .Where(x => x.StudentSection.Section.CourseTerm.TermId == term.Id)
                .AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty)
            {
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);
                queryGrades = queryGrades.Where(x => x.StudentSection.Section.CourseTerm.Course.CareerId == careerId);
            }

            if (departmentId.HasValue && departmentId != Guid.Empty)
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.Teacher.AcademicDepartmentId == departmentId));
                queryGrades = queryGrades.Where(x => x.StudentSection.Section.TeacherSections.Any(y => y.Teacher.AcademicDepartmentId == departmentId));
            }

            if (!string.IsNullOrEmpty(teacherId))
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.TeacherId == teacherId));
                queryGrades = queryGrades.Where(x => x.StudentSection.Section.TeacherSections.Any(y => y.TeacherId == teacherId));
            }

            var sections = await query
                .Where(x => x.StudentSections.Any())
                .Select(x => new
                {
                    x.Id,
                    x.CourseTermId,
                    x.IsDirectedCourse,
                    x.TeacherSections,
                    x.Code,

                    x.CourseTerm.CourseId,
                    CourseCode = x.CourseTerm.Course.Code,
                    CourseName = x.CourseTerm.Course.Name,
                    x.CourseTerm.Course.PracticalHours,
                    x.CourseTerm.Course.TheoreticalHours,
                    x.CourseTerm.Course.Credits,
                    x.CourseTerm.Course.EffectiveHours,
                    x.CourseTerm.Course.CareerId,
                    FacultyId = x.CourseTerm.Course.CareerId.HasValue ? x.CourseTerm.Course.Career.FacultyId : Guid.Empty,
                    CareerName = x.CourseTerm.Course.CareerId.HasValue ? x.CourseTerm.Course.Career.Name : "",
                    FacultyName = x.CourseTerm.Course.CareerId.HasValue ? x.CourseTerm.Course.Career.Faculty.Name : "",
                    CareerDirectorId = x.CourseTerm.Course.CareerId.HasValue ? x.CourseTerm.Course.Career.CareerDirectorId : string.Empty
                }).ToListAsync();

            //if (sections.Count == 0)
            //    return BadRequest("No se encontraron actas que cumplan con los requisitos indicados previamente.");

            var outPdf = new PdfDocument();
            var evaluationReports = await _context.EvaluationReports.Where(x => x.Section.CourseTerm.TermId == term.Id).ToListAsync();
            var newEvaluationReports = new List<EvaluationReport>();

            var user = await _userManager.GetUserAsync(User);
            var signature = await _digitizedSignatureService.GetFirst();

            var allGrades = await queryGrades
                .Include(x => x.Evaluation)
                .ToListAsync();

            var courseTermHashset = sections.Select(x => x.CourseTermId).ToHashSet();
            var allEvaluations = await _context.Evaluations
                .Where(x => courseTermHashset.Contains(x.CourseTermId))
                .AsNoTracking()
                .ToListAsync();

            foreach (var section in sections)
            {
                var evaluationReport = evaluationReports.FirstOrDefault(x => x.SectionId == section.Id);
                var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
                if (evaluationReport == null)
                {
                    var evaluationReportNumber = await _evaluationReportService.GetMaxNumber(courseTerm.TermId);
                    evaluationReport = new EvaluationReport
                    {
                        PrintQuantity = 1,
                        LastReportGeneratedDate = DateTime.UtcNow,
                        SectionId = section.Id,
                        Type = section.IsDirectedCourse ? ConstantHelpers.Intranet.EvaluationReportType.DIRECTED_COURSE : ConstantHelpers.Intranet.EvaluationReportType.REGULAR,
                        Status = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.GENERATED,
                        Number = evaluationReportNumber + 1,
                        TermId = courseTerm.TermId,
                        Code = $"{(evaluationReportNumber + 1):000000}"
                    };
                    //newEvaluationReports.Add(evaluationReport);
                    await _context.EvaluationReports.AddAsync(evaluationReport);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    evaluationReport.PrintQuantity = evaluationReport.PrintQuantity is null ? 1 : evaluationReport.PrintQuantity + 1;
                    if (evaluationReport.Status != ConstantHelpers.Intranet.EvaluationReport.RECEIVED)
                    {
                        evaluationReport.LastReportGeneratedDate = DateTime.UtcNow;
                    }
                    //await _evaluationReportService.UpdateEvaluationReport(evaluationReport);
                }

                var model = new ActaScoresViewModel()
                {
                    BasicInformation = new BasicInformation(),
                    Rows = new List<Row>(),
                    Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                    Number = evaluationReport.Code,
                    Date = evaluationReport.CreatedAt.HasValue ? evaluationReport.CreatedAt.Value : DateTime.Now
                };

                //QR GENERATOR
                //QRCodeGenerator qrGenerator = new QRCodeGenerator();
                //var URLAbsolute = Url.GenerateLink(nameof(DocumentVerifierController.EvaluationReportVerifier), "DocumentVerifier", Request.Scheme, new { id = evaluationReport.Id });
                //QRCodeData qrCodeData = qrGenerator.CreateQrCode(URLAbsolute,
                //QRCodeGenerator.ECCLevel.Q);
                //QRCode qrCode = new QRCode(qrCodeData);
                //Bitmap qrCodeImage = qrCode.GetGraphic(3);

                //var bitMap = ConvertHelpers.BitmapToBytes(qrCodeImage);

                //var finalQR = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(bitMap));

                //model.FinalQR = finalQR;

                try
                {
                    var teacherName = "--";
                    if (section.TeacherSections.Count > 0)
                    {
                        var teacherSectionId = section.TeacherSections.FirstOrDefault().TeacherId;
                        var teacher = await _userManager.FindByIdAsync(teacherSectionId);
                        teacherName = teacher.FullName;
                    }

                    var courseUnits = await _context.Courses
                        .Where(x => x.Id == section.CourseId)
                        .Select(x => x.CourseComponent.QuantityOfUnits)
                        .FirstOrDefaultAsync();

                    var campusCode = await _context.ClassSchedules
                        .Where(x => x.SectionId == section.Id)
                        .Select(x => x.Classroom.Building.Campus.Code).FirstOrDefaultAsync();

                    if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAMAD)
                    {
                        var careerDirector = await _userService.Get(section.CareerDirectorId);
                        if (careerDirector != null) model.CareerDirector = careerDirector.FullName;
                    }
                    else
                    {
                        model.CareerDirector = "";
                        if (section.CareerId.HasValue)
                        {
                            var academicDepartment = await _context.AcademicDepartments.FirstOrDefaultAsync(x => x.CareerId == section.CareerId);

                            if (academicDepartment == null)
                                academicDepartment = await _context.AcademicDepartments.FirstOrDefaultAsync(x => x.CareerId == section.FacultyId);

                            if (academicDepartment != null && !string.IsNullOrEmpty(academicDepartment.AcademicDepartmentDirectorId))
                            {
                                var departmentDirector = await _userService.Get(academicDepartment.AcademicDepartmentDirectorId);
                                model.CareerDirector = departmentDirector.FullName;
                            }
                        }
                    }

                    var academicYearCourse = await _context.AcademicYearCourses.FirstOrDefaultAsync(x => x.CourseId == section.CourseId);

                    model.BasicInformation.Teacher = teacherName;
                    model.BasicInformation.Course = $"[{section.CourseCode}] {section.CourseName}";
                    model.BasicInformation.Career = section.CareerName;
                    model.BasicInformation.Faculty = section.FacultyName;
                    model.BasicInformation.TheoreticalHours = section.TheoreticalHours;
                    model.BasicInformation.PracticalHours = section.PracticalHours;
                    model.BasicInformation.EffectiveHours = section.EffectiveHours;
                    model.BasicInformation.Credits = section.Credits.ToString();
                    model.BasicInformation.Sede = campusCode == null ? "--" : campusCode;
                    model.BasicInformation.Term = term.Name;
                    model.BasicInformation.IsSummer = term.IsSummer;
                    model.BasicInformation.Cycle = academicYearCourse != null ? academicYearCourse.AcademicYear.ToString() : "0";
                    model.BasicInformation.Section = section.Code;
                    model.BasicInformation.User = user.Name;
                    model.BasicInformation.CourseUnits = courseUnits;
                    model.BasicInformation.ReceptionDate = evaluationReport.Status == ConstantHelpers.Intranet.EvaluationReport.RECEIVED ? evaluationReport.ReceptionDate.ToLocalDateFormat() : string.Empty;
                    model.BasicInformation.Signature = signature is null ? null : await GeneralHelpers.GetImageForStringPartialView(_options, signature.UrlSignature);
                    model.Approbed = term.MinGrade;
                    model.Type = evaluationReport.Type;

                    var students = await _context.StudentSections
                        .Where(x => x.SectionId == section.Id && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                        .Include(x => x.Student)
                        .ThenInclude(x => x.User)
                        .ToListAsync();

                    var studentSectionHashset = students.Select(x => x.Id).ToHashSet();
                    var grades = allGrades.Where(x => studentSectionHashset.Contains(x.StudentSectionId)).ToList();

                    var evaluations = allEvaluations
                        .Where(x => x.CourseTermId == section.CourseTermId)
                        .ToList();

                    var substituteExams = await _context.SubstituteExams
                        .Where(x => x.Section.CourseTerm.CourseId == section.CourseId && x.Section.CourseTerm.TermId == term.Id)
                        //.Select(x => x.ExamScore)
                        .ToListAsync();

                    var count = 1;

                    var rows = new List<Row>();

                    students = students.OrderBy(x => x.Student.User.FullName).ToList();

                    foreach (var item in students)
                    {
                        var row = new Row()
                        {
                            Code = item.Student.User.UserName,
                            Surnames = $"{item.Student.User.PaternalSurname} {item.Student.User.MaternalSurname}",
                            Names = item.Student.User.Name,
                            Try = item.Try == 0 ? 1 : item.Try,
                            StudentStatus = item.Status
                        };

                        row.Order = count;

                        var averagesByUnits = evaluations
                            .GroupBy(x => x.CourseUnitId)
                            .Select(x => (int?)Math.Round(grades.Where(y => y.StudentSectionId == item.Id && x.Any(z => y.EvaluationId == z.Id))
                                .Sum(y => (y.Value * y.Evaluation.Percentage) / x.Sum(z => z.Percentage)))
                            ).ToList();

                        while (averagesByUnits.Count() < model.BasicInformation.CourseUnits)
                        {
                            averagesByUnits.Add(0);
                        }

                        row.PartialAverages = averagesByUnits.ToArray();

                        var substiGrade = substituteExams.Where(x => x.StudentId == item.StudentId).Select(x => x.ExamScore).FirstOrDefault();

                        var finalGrade = item.FinalGrade;
                        if (substiGrade.HasValue) finalGrade = substiGrade.Value;

                        row.FinalEvaluation = finalGrade;
                        row.FinalEvaluationNumber = finalGrade;
                        row.FinalEvaluationText = NUMBERS.VALUES[finalGrade];
                        row.HasSusti = substiGrade.HasValue;

                        rows.Add(row);

                        count++;
                    }

                    model.Rows = rows;
                }
                catch (Exception)
                {
                    model.BasicInformation.Teacher = "";
                    model.BasicInformation.Course = "";
                    model.BasicInformation.Career = "";
                    model.BasicInformation.Credits = "";
                    model.BasicInformation.Sede = "";
                    model.BasicInformation.Term = "";
                    model.BasicInformation.Cycle = "0";
                    model.BasicInformation.Section = "";
                }

                DinkToPdf.GlobalSettings globalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                };

                var mainViewToString = string.Empty;
                var leftfooter = "";
                var rightfooter = "";
                var centerfooter = "";

                if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD)
                {
                    leftfooter = "DIRECCION UNIVERSITARIA DE ASUNTOS ACADEMICOS";
                    rightfooter = $"Impreso por: {model.BasicInformation.User} {DateTime.UtcNow.ToLocalDateTimeFormat()}";
                    centerfooter = "Página [page]/[toPage]";
                    mainViewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/EvaluationReport/ReportUNAMAD.cshtml", model);
                }
                else
                {
                    leftfooter = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}";
                    mainViewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/EvaluationReport/ReportUNJBG.cshtml", model);
                    rightfooter = "Página [page] de [toPage]";
                }

                var mainObjectSettings = new DinkToPdf.ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = mainViewToString,
                    WebSettings = { DefaultEncoding = "utf-8" },
                    HeaderSettings =
                    {

                    },
                    FooterSettings = {
                        FontName = "Arial",
                        FontSize = 6,
                        Line = false,
                        Left = leftfooter,
                        Right =rightfooter,
                        Center =centerfooter
                    }
                };

                DinkToPdf.HtmlToPdfDocument pdf = new DinkToPdf.HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { mainObjectSettings }
                };

                byte[] fileByte = _dinkConverter.Convert(pdf);

                block.Add(new EvaluationReportBlockViewModel
                {
                    Teacher = model.BasicInformation.Teacher,
                    Document = fileByte
                });
            }

            block = block.OrderBy(x => x.Teacher).ToList();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            foreach (var item in block)
            {
                var mainPdf = PdfReader.Open(new MemoryStream(item.Document), PdfDocumentOpenMode.Import);
                CopyPages(mainPdf, outPdf);
            }

            MemoryStream stream = new MemoryStream();
            outPdf.Save(stream, false);
            var fileBytes = stream.ToArray();
            _textSharpService.AddWatermarkToAllPages(ref fileBytes, ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value], 130);
            return fileBytes;
        }

        /// <summary>
        /// Método para obtener las actas finales en bloque según lo filtros del usuario
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="departmentId">Identificador del departamento académico</param>
        /// <param name="teacherId">identificador del docente</param>
        /// <returns>Arreglo de bytes que contiene el archivo PDF</returns>
        private async Task<byte[]> BlockReportFormat2(Guid? termId, Guid? careerId, Guid? departmentId, string teacherId)
        {
            var block = new List<EvaluationReportBlockViewModel>();

            var term = await _termService.GetActiveTerm();

            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == term.Id)
                .AsNoTracking();

            var queryGrades = _context.Grades
                .Where(x => x.StudentSection.Section.CourseTerm.TermId == term.Id)
                .AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty)
            {
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);
                queryGrades = queryGrades.Where(x => x.StudentSection.Section.CourseTerm.Course.CareerId == careerId);
            }

            if (departmentId.HasValue && departmentId != Guid.Empty)
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.Teacher.AcademicDepartmentId == departmentId));
                queryGrades = queryGrades.Where(x => x.StudentSection.Section.TeacherSections.Any(y => y.Teacher.AcademicDepartmentId == departmentId));
            }

            if (!string.IsNullOrEmpty(teacherId))
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.TeacherId == teacherId));
                queryGrades = queryGrades.Where(x => x.StudentSection.Section.TeacherSections.Any(y => y.TeacherId == teacherId));
            }

            var sections = await query
                .Where(x => x.StudentSections.Any())
                .Include(x => x.TeacherSections)
                .Include(x => x.CourseTerm)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Career)
                .ThenInclude(x => x.Faculty)
                .AsNoTracking()
                .ToListAsync();

            //if (sections.Count == 0)
            //    return BadRequest("No se encontraron actas que cumplan con los requisitos indicados previamente.");

            var outPdf = new PdfDocument();
            var evaluationReports = await _context.EvaluationReports.Where(x => x.Section.CourseTerm.TermId == term.Id).ToListAsync();
            var newEvaluationReports = new List<EvaluationReport>();

            var user = await _userManager.GetUserAsync(User);
            var signature = await _digitizedSignatureService.GetFirst();

            var allGrades = await queryGrades
                .Include(x => x.Evaluation)
                .ToListAsync();

            var courseTermHashset = sections.Select(x => x.CourseTermId).ToHashSet();
            var allEvaluations = await _context.Evaluations
                .Where(x => courseTermHashset.Contains(x.CourseTermId))
                .AsNoTracking()
                .ToListAsync();

            foreach (var section in sections)
            {
                var evaluationReport = evaluationReports.FirstOrDefault(x => x.SectionId == section.Id);

                if (evaluationReport == null)
                {
                    var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
                    var evaluationReportNumber = await _evaluationReportService.GetMaxNumber(courseTerm.TermId);
                    evaluationReport = new EvaluationReport
                    {
                        PrintQuantity = 1,
                        LastReportGeneratedDate = DateTime.UtcNow,
                        SectionId = section.Id,
                        Type = section.IsDirectedCourse ? ConstantHelpers.Intranet.EvaluationReportType.DIRECTED_COURSE : ConstantHelpers.Intranet.EvaluationReportType.REGULAR,
                        Status = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.GENERATED,
                        Number = evaluationReportNumber + 1,
                        Code = $"{(evaluationReportNumber + 1):000000}",
                        TermId = courseTerm.TermId
                    };
                    //newEvaluationReports.Add(evaluationReport);
                    await _context.EvaluationReports.AddAsync(evaluationReport);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    evaluationReport.PrintQuantity = evaluationReport.PrintQuantity is null ? 1 : evaluationReport.PrintQuantity + 1;
                    if (evaluationReport.Status != ConstantHelpers.Intranet.EvaluationReport.RECEIVED)
                    {
                        evaluationReport.LastReportGeneratedDate = DateTime.UtcNow;
                    }
                    //await _evaluationReportService.UpdateEvaluationReport(evaluationReport);
                }

                var model = new ActaScoresViewModel()
                {
                    BasicInformation = new BasicInformation(),
                    Rows = new List<Row>(),
                    Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                    Number = evaluationReport.Code,
                    Date = evaluationReport.CreatedAt.HasValue ? evaluationReport.CreatedAt.Value : DateTime.Now
                };

                //model.FinalQR = "";

                try
                {
                    var teacherName = "--";

                    if (section.TeacherSections.Count > 0)
                    {
                        var teacherSectionId = section.TeacherSections.FirstOrDefault().TeacherId;
                        var teacher = await _userManager.FindByIdAsync(teacherSectionId);
                        teacherName = teacher.FullName;
                    }

                    var courseUnits = await _context.Courses
                        .Where(x => x.Id == section.CourseTerm.CourseId)
                        .Select(x => x.CourseComponent.QuantityOfUnits)
                        .FirstOrDefaultAsync();

                    var campusCode = await _context.ClassSchedules
                        .Where(x => x.SectionId == section.Id)
                        .Select(x => x.Classroom.Building.Campus.Code).FirstOrDefaultAsync();

                    var academicYearCourse = await _context.AcademicYearCourses.FirstOrDefaultAsync(x => x.CourseId == section.CourseTerm.CourseId);

                    model.BasicInformation.Teacher = teacherName;
                    model.BasicInformation.Course = $"[{section.CourseTerm.Course.Code}] {section.CourseTerm.Course.Name}";
                    model.BasicInformation.Career = section.CourseTerm.Course.Career.Name;
                    model.BasicInformation.Faculty = section.CourseTerm.Course.Career.Faculty.Name;
                    model.BasicInformation.TheoreticalHours = section.CourseTerm.Course.TheoreticalHours;
                    model.BasicInformation.PracticalHours = section.CourseTerm.Course.PracticalHours;
                    model.BasicInformation.EffectiveHours = section.CourseTerm.Course.EffectiveHours;
                    model.BasicInformation.Credits = section.CourseTerm.Course.Credits.ToString();
                    model.BasicInformation.Sede = campusCode == null ? "--" : campusCode;
                    model.BasicInformation.Term = term.Name;
                    model.BasicInformation.IsSummer = term.IsSummer;
                    model.BasicInformation.Cycle = academicYearCourse != null ? academicYearCourse.AcademicYear.ToString() : "0";
                    model.BasicInformation.Section = section.Code;
                    model.BasicInformation.User = user.Name;
                    model.BasicInformation.CourseUnits = courseUnits;
                    model.BasicInformation.ReceptionDate = evaluationReport.Status == ConstantHelpers.Intranet.EvaluationReport.RECEIVED ? evaluationReport.ReceptionDate.ToLocalDateFormat() : string.Empty;
                    model.BasicInformation.Signature = signature is null ? null : await GeneralHelpers.GetImageForStringPartialView(_options, signature.UrlSignature);
                    model.Approbed = term.MinGrade;
                    model.Type = evaluationReport.Type;

                    var students = await _context.StudentSections
                        .Where(x => x.SectionId == section.Id && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                        .Include(x => x.Student)
                        .ThenInclude(x => x.User)
                        .ToListAsync();

                    var studentSectionHashset = students.Select(x => x.Id).ToHashSet();
                    var grades = allGrades.Where(x => studentSectionHashset.Contains(x.StudentSectionId)).ToList();

                    var evaluations = allEvaluations
                        .Where(x => x.CourseTermId == section.CourseTermId)
                        .ToList();

                    var substituteExams = await _context.SubstituteExams
                        .Where(x => x.Section.CourseTerm.CourseId == section.CourseTerm.CourseId && x.Section.CourseTerm.TermId == term.Id)
                        //.Select(x => x.ExamScore)
                        .ToListAsync();

                    var count = 1;

                    var rows = new List<Row>();

                    students = students.OrderBy(x => x.Student.User.FullName).ToList();

                    foreach (var item in students)
                    {
                        var row = new Row()
                        {
                            Code = item.Student.User.UserName,
                            Surnames = $"{item.Student.User.PaternalSurname} {item.Student.User.MaternalSurname}",
                            Names = item.Student.User.Name,
                            Try = item.Try == 0 ? 1 : item.Try
                        };

                        row.Order = count;

                        var averagesByUnits = evaluations
                            .GroupBy(x => x.CourseUnitId)
                            .Select(x => (int?)Math.Round(grades.Where(y => y.StudentSectionId == item.Id && x.Any(z => y.EvaluationId == z.Id))
                                .Sum(y => (y.Value * y.Evaluation.Percentage) / x.Sum(z => z.Percentage)))
                            ).ToList();

                        while (averagesByUnits.Count() < model.BasicInformation.CourseUnits)
                        {
                            averagesByUnits.Add(0);
                        }

                        row.PartialAverages = averagesByUnits.ToArray();

                        var substiGrade = substituteExams.Where(x => x.StudentId == item.StudentId).Select(x => x.ExamScore).FirstOrDefault();

                        var finalGrade = item.FinalGrade;
                        if (substiGrade.HasValue) finalGrade = substiGrade.Value;

                        row.FinalEvaluation = finalGrade;
                        row.FinalEvaluationNumber = finalGrade;
                        row.FinalEvaluationText = NUMBERS.VALUES[finalGrade];
                        row.HasSusti = substiGrade.HasValue;

                        rows.Add(row);

                        count++;
                    }

                    model.Rows = rows;
                }
                catch (Exception)
                {
                    model.BasicInformation.Teacher = "";
                    model.BasicInformation.Course = "";
                    model.BasicInformation.Career = "";
                    model.BasicInformation.Credits = "";
                    model.BasicInformation.Sede = "";
                    model.BasicInformation.Term = "";
                    model.BasicInformation.Cycle = "0";
                    model.BasicInformation.Section = "";
                }

                var globalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                };

                var mainViewToString = string.Empty;

                mainViewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/EvaluationReport/ReportUNJBG.cshtml", model);

                var mainObjectSettings = new DinkToPdf.ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = mainViewToString,
                    WebSettings = { DefaultEncoding = "utf-8" }
                };

                var pdf = new DinkToPdf.HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { mainObjectSettings }
                };

                var fileByte = _dinkConverter.Convert(pdf);

                block.Add(new EvaluationReportBlockViewModel
                {
                    Teacher = model.BasicInformation.Teacher,
                    Document = fileByte
                });
            }

            block = block.OrderBy(x => x.Teacher).ToList();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            foreach (var item in block)
            {
                var mainPdf = PdfReader.Open(new MemoryStream(item.Document), PdfDocumentOpenMode.Import);
                CopyPages(mainPdf, outPdf);
            }

            MemoryStream stream = new MemoryStream();
            outPdf.Save(stream, false);

            var fileBytes = stream.ToArray();
            //_textSharpService.AddWatermarkToAllPages(ref fileBytes, ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value], 130);
            return fileBytes;
        }

        /// <summary>
        /// Método para obtener el registro de acta final
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Nombre del archivo y archivo en un arreglo de bytes</returns>
        private async Task<Tuple<string, byte[]>> GetEvaluationReportFormat2(Guid sectionId)
        {
            var evaluationReport = await _evaluationReportService.GetEvaluationReportBySectionId(sectionId);
            var model = new ActaScoresViewModel()
            {
                BasicInformation = new BasicInformation(),
                Rows = new List<Row>(),
                Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                Number = evaluationReport.Code,
                Date = evaluationReport.CreatedAt.HasValue ? evaluationReport.CreatedAt.Value : DateTime.Now
            };

            var section = await _sectionService.GetSectionWithTermAndCareer(sectionId);

            if (section == null)
                section = await _teacherSectionService.GetTeacherSectionsWithTermAndCareer(sectionId);

            var curriculumId = section.CourseTerm.Course.AcademicYearCourses.Select(x => x.CurriculumId).FirstOrDefault();
            var curriculum = await _curriculumService.Get(curriculumId);

            var teacherSections = await _teacherSectionService.GetAllBySection(sectionId);
            var academicYearCourse = await _academicYearCourseService.GetAcademicYearCourseByCourseId(section.CourseTerm.CourseId);
            var Class = await _classScheduleService.GetClassSchedulesBySectionId(sectionId);
            var user = await _userService.GetUserByClaim(User);
            var signature = await _digitizedSignatureService.GetFirst();

            var lastGradeRegistration = await _context.Grades.Where(x => x.StudentSection.SectionId == section.Id).OrderByDescending(x => x.CreatedAt).Select(y => y.CreatedAt).FirstOrDefaultAsync();
            var lastSustiRegistration = await _context.SubstituteExams.Where(x => x.SectionId == section.Id && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED).OrderByDescending(x => x.CreatedAt).Select(x => x.CreatedAt).FirstOrDefaultAsync();
            if (lastGradeRegistration.HasValue && lastSustiRegistration.HasValue)
            {
                model.LastGradeRegistration = lastGradeRegistration > lastSustiRegistration ? lastGradeRegistration : lastSustiRegistration;
            }
            else
            {
                model.LastGradeRegistration = lastSustiRegistration.HasValue ? lastSustiRegistration : lastGradeRegistration;
            }

            if (model.LastGradeRegistration == DateTime.MinValue)
                model.LastGradeRegistration = null;

            model.BasicInformation.Teacher = teacherSections.Any() ? teacherSections.Where(y => y.IsPrincipal).Count() > 1 ? "CARGA COMPARTIDA" : teacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).FirstOrDefault() : "--";
            model.BasicInformation.Course = $"[{curriculum?.Code}-{section.CourseTerm?.Course?.Code}] {section.CourseTerm?.Course?.Name}";
            model.BasicInformation.Career = section.CourseTerm?.Course?.Career?.Name;
            model.BasicInformation.Faculty = section.CourseTerm?.Course?.Career?.Faculty?.Name;
            model.BasicInformation.AcademicProgram = section.CourseTerm?.Course?.AcademicProgram?.Name;
            model.BasicInformation.TheoreticalHours = section.CourseTerm?.Course?.TheoreticalHours;
            model.BasicInformation.PracticalHours = section.CourseTerm?.Course?.PracticalHours;
            model.BasicInformation.EffectiveHours = section.CourseTerm?.Course?.EffectiveHours;
            model.BasicInformation.Credits = section.CourseTerm?.Course?.Credits.ToString();
            model.BasicInformation.Sede = Class == null ? "--" : Class.Classroom == null ? "--" : Class.Classroom.Building == null ? "--" : Class.Classroom.Building.Name.Substring(0, 1);
            model.BasicInformation.Term = section.CourseTerm?.Term.Name;
            model.BasicInformation.IsSummer = section.CourseTerm?.Term.IsSummer;
            model.BasicInformation.Cycle = academicYearCourse?.AcademicYear.ToString();
            if (string.IsNullOrEmpty(model.BasicInformation.Cycle)) model.BasicInformation.Cycle = "0";
            model.BasicInformation.Section = section.Code;
            model.BasicInformation.ReceptionDate = evaluationReport.Status == CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED ? evaluationReport.ReceptionDate.ToLocalDateFormat() : string.Empty;
            model.BasicInformation.User = user.Name;
            model.BasicInformation.Signature = signature is null ? null : await GeneralHelpers.GetImageForStringPartialView(_options, signature.UrlSignature);
            model.Type = evaluationReport.Type;

            model.FinalQR = GetBarCode(evaluationReport.Code);

            var sectionStudents = await _studentSectionService.GetAllSectionStudentsWithUserBySectionId(sectionId);
            sectionStudents = sectionStudents.Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).ToList();
            model.Approbed = section.CourseTerm.Term.MinGrade;

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
            model.BasicInformation.EvaluationByUnits = evaluationsByUnits;

            if (evaluationsByUnits)
            {
                var courseUnits = await _courseUnitService.GetQuantityCourseUnits(section.CourseTerm.Course.Id, section.CourseTerm.Term.Id);
                model.BasicInformation.CourseUnits = courseUnits;

                for (int i = 0; i < sectionStudents.Count; i++)
                {
                    var courseUnitGrades = await _courseUnitService.GetCourseUnitGradesByStudentIdAndSectionId(sectionStudents[i].StudentId, sectionId);
                    var susti = await _context.SubstituteExams.Where(x => x.SectionId == sectionStudents[i].SectionId && x.StudentId == sectionStudents[i].StudentId && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED).Select(x => x.ExamScore).FirstOrDefaultAsync();
                    var averagesByUnits = courseUnitGrades is null ? null : courseUnitGrades.Select(x => x.Average).ToList();
                    int finalAverage = sectionStudents[i].FinalGrade;

                    if (section.CourseTerm.Term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                    {
                        finalAverage = await _context.AcademicHistories.Where(x => x.SectionId == section.Id && x.StudentId == sectionStudents[i].StudentId).Select(x => x.Grade).FirstOrDefaultAsync();
                    }

                    if (averagesByUnits is null)
                    {
                        averagesByUnits = new List<int?>();
                        for (int a = 0; a < courseUnits; a++)
                        {
                            averagesByUnits.Add(null);
                        }
                    }

                    var row = new Row()
                    {
                        Order = i + 1,
                        Code = sectionStudents[i].Student.User.UserName,
                        Surnames = $"{sectionStudents[i].Student.User.PaternalSurname} {sectionStudents[i].Student.User.MaternalSurname}",
                        Names = sectionStudents[i].Student.User.Name,
                        Try = sectionStudents[i].Try == 0 ? 1 : sectionStudents[i].Try,
                        PartialAverages = averagesByUnits.ToArray(),
                        FinalEvaluation = finalAverage,
                        FinalEvaluationNumber = finalAverage,
                        FinalEvaluationText = NUMBERS.VALUES[finalAverage],
                        HasSusti = susti.HasValue && susti.Value >= section.CourseTerm.Term.MinGrade && ((susti <= 14 && finalAverage == susti.Value) || (susti > 14 && finalAverage == 14)),
                        StudentStatus = sectionStudents[i].Status
                        //FinalEvaluation = sectionStudents[i].FinalGrade.ToString(),
                        //FinalEvaluationNumber = sectionStudents[i].FinalGrade,
                        //FinalEvaluationText = NUMBERS.VALUES[sectionStudents[i].FinalGrade],

                    };


                    model.Rows.Add(row);
                }

            }
            else
            {
                var evaluations = await _context.Evaluations.Where(x => x.CourseTermId == section.CourseTermId).OrderBy(x => x.Week).ThenBy(x => x.Percentage).ToListAsync();
                model.BasicInformation.Evaluations = evaluations.Count();
                model.BasicInformation.EvaluationsList = evaluations;

                for (int i = 0; i < sectionStudents.Count; i++)
                {
                    var grades = await _context.Grades.Where(x => x.StudentSectionId == sectionStudents[i].Id && x.EvaluationId.HasValue)
                        .OrderBy(x => x.Evaluation.Week).ThenBy(x => x.Evaluation.Percentage)
                        .Select(x => new
                        {
                            x.EvaluationId,
                            x.Value
                        })
                        .ToListAsync();

                    var averagesByEvaluations = new List<decimal>();

                    foreach (var evaluation in evaluations)
                    {
                        if (grades.Any(y => y.EvaluationId == evaluation.Id))
                        {
                            averagesByEvaluations.Add(grades.Where(x => x.EvaluationId == evaluation.Id).Select(y => y.Value).FirstOrDefault());
                        }
                        else
                        {
                            averagesByEvaluations.Add(0);
                        }
                    }

                    var susti = await _context.SubstituteExams.Where(x => x.SectionId == sectionStudents[i].SectionId && x.StudentId == sectionStudents[i].StudentId && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED).Select(x => x.ExamScore).FirstOrDefaultAsync();

                    int finalAverage = sectionStudents[i].FinalGrade;

                    if (section.CourseTerm.Term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                    {
                        finalAverage = await _context.AcademicHistories.Where(x => x.SectionId == section.Id && x.StudentId == sectionStudents[i].StudentId).Select(x => x.Grade).FirstOrDefaultAsync();
                    }

                    var row = new Row()
                    {
                        Order = i + 1,
                        Code = sectionStudents[i].Student.User.UserName,
                        Surnames = $"{sectionStudents[i].Student.User.PaternalSurname} {sectionStudents[i].Student.User.MaternalSurname}",
                        Names = sectionStudents[i].Student.User.Name,
                        Try = sectionStudents[i].Try == 0 ? 1 : sectionStudents[i].Try,
                        PartialEvaluationAverages = averagesByEvaluations.ToArray(),
                        FinalEvaluation = finalAverage,
                        FinalEvaluationNumber = finalAverage,
                        FinalEvaluationText = NUMBERS.VALUES[finalAverage],
                        HasSusti = susti.HasValue && susti.Value >= section.CourseTerm.Term.MinGrade && ((susti <= 14 && finalAverage == susti.Value) || (susti > 14 && finalAverage == 14)),
                        StudentStatus = sectionStudents[i].Status
                    };

                    model.Rows.Add(row);
                }
            }

            var academicHsitories = await _context.AcademicHistories.Where(x => x.SectionId == section.Id).ToListAsync();
            academicHsitories.ForEach(x => x.EvaluationReportId = evaluationReport.Id);

            await _context.SaveChangesAsync();


            string viewBackground = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/EvaluationReport/Pdf/ReportBackgroundV2.cshtml", model);
            string viewBody = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/EvaluationReport/ReportV2.cshtml", model);

            var documentTitle = $"R-{section.CourseTerm?.Course?.Career?.Code}-{curriculum.Code}-{section.CourseTerm?.Course?.Code}-{evaluationReport.Code}";


            var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css/areas/admin/evaluationreport/format2.css");
            var cssPathBackground = Path.Combine(_hostingEnvironment.WebRootPath, @"css/areas/admin/evaluationreport/format2_background.css");

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNICA)
            {
                cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @$"css/areas/admin/evaluationreport/{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value].ToLower()}/format2.css");
                cssPathBackground = Path.Combine(_hostingEnvironment.WebRootPath, @$"css/areas/admin/evaluationreport/{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value].ToLower()}/format2_background.css");
            }

            var pdfBackground = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 0, Bottom = 0, Left = 5, Right = 5 },
                    DocumentTitle = documentTitle
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = viewBackground,
                        WebSettings = { DefaultEncoding = "utf-8" , UserStyleSheet = cssPathBackground}
                    }
                }
            };

            var pdfBody = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 65, Bottom = 70, Left = 5, Right = 5 },
                    DocumentTitle = documentTitle
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = viewBody,
                        WebSettings = { DefaultEncoding = "utf-8" , UserStyleSheet = cssPath}
                    }
                }
            };

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var bytesBackground = _dinkConverter.Convert(pdfBackground);
            var bytesBody = _dinkConverter.Convert(pdfBody);

            var bytes = _textSharpService.AddHeaderToAllPages(bytesBody, bytesBackground);

            return new Tuple<string, byte[]>(documentTitle, bytes);
        }

        /// <summary>
        /// Obtiene el código de barras en base 64
        /// </summary>
        /// <param name="str">Texto</param>
        /// <returns>Código de barras en base 64</returns>
        private string GetBarCode(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            using (var barCode = new BarcodeLib.Barcode())
            {
                using (var barcodeImage = barCode.Encode(BarcodeLib.TYPE.CODE128, str, 200, 50))
                {
                    using (var ms = new MemoryStream())
                    {
                        barcodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = ms.ToArray();
                        barcodeImage.Dispose();

                        return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(imageBytes));
                    }
                }
            }

            //var barcode = new NetBarcode.Barcode(str, false, 200, 50);
            //var array = barcode.GetByteArray();
            //return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(array));
        }

        #endregion
    }
}
