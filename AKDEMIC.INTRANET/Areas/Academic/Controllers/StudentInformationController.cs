using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation;
using AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels;
using AKDEMIC.INTRANET.Areas.Student.Models.PaymentHistoryViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Model;
using AKDEMIC.PDF.Services.CertificateOfStudiesGenerator;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AutoMapper;
using ClosedXML.Excel;
using DinkToPdf;
using DinkToPdf.Contracts;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AKDEMIC.INTRANET.Areas.Academic.Controllers
{
    [Area("Academic")]
    [Route("academico/alumnos")]
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.INSTITUTIONAL_WELFARE + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.DEAN + "," +
        ConstantHelpers.ROLES.DEAN_SECRETARY + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_RECORD + "," +
        CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF + "," +
        CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        CORE.Helpers.ConstantHelpers.ROLES.OTI_SUPPORT + "," +
        CORE.Helpers.ConstantHelpers.ROLES.CERTIFICATION + "," +
        CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR_GENERAL + "," +
        CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_SECRETARY + "," +
        CORE.Helpers.ConstantHelpers.ROLES.SOCIAL_SERVICE + "," +
        CORE.Helpers.ConstantHelpers.ROLES.PASSWORD_EDITOR)]
    public class StudentInformationController : BaseController
    {
        private IWebHostEnvironment _hostingEnvironment;
        public IConverter _dinkConverter { get; }
        private readonly IAcademicHistoryDocumentService _academicHistoryDocumentService;
        private readonly IViewRenderService _viewRenderService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IStudentService _studentService;
        private readonly IEvaluationReportService _evaluationReportService;
        private readonly IMapper _mapper;
        private readonly IExtracurricularActivityStudentService _extracurricularActivityStudentService;
        private readonly IAcademicYearCourseService _academicYearCourseService;
        private readonly ICertificateOfStudiesGeneratorService _certificateOfStudiesGeneratorService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IResolutionService _resolutionService;
        private readonly IClassScheduleService _classScheduleService;
        private readonly IClassStudentService _classStudentService;
        private readonly ICampusService _campusService;
        private readonly IPostulantService _postulantService;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly ICourseService _courseService;
        private readonly IAcademicProgramService _academicProgramService;
        private readonly ICareerService _careerService;
        private readonly IEnrollmentReservationService _enrollmentReservationService;
        private readonly IStudentObservationService _studentObservationService;
        private readonly IRegistryPatternService _registryPatternService;
        private readonly IRecordHistoryService _recordHistoryService;
        private readonly IConfigurationService _configurationService;
        private readonly IDirectedCourseService _directedCourseService;
        private readonly IDirectedCourseStudentService _directedCourseStudentService;
        private readonly ICourseTermService _courseTermService;
        private readonly IInvoiceService _invoiceService;
        private readonly IConceptService _conceptService;
        private readonly IPaymentService _paymentService;
        private readonly ITextSharpService _pdfSharpService;
        private readonly IProcedureDependencyService _procedureDependencyService;
        private readonly IUserProcedureService _userProcedureService;
        private readonly IProcedureRequirementService _procedureRequirementService;
        private readonly IProcedureService _procedureService;
        private readonly IEnrollmentTurnService _enrollmentTurnService;
        private readonly ICurriculumService _curriculumService;
        private readonly IExtraordinaryEvaluationStudentService _extraordinaryEvaluationStudentService;
        private readonly ISectionService _sectionService;
        private readonly IStudentIncomeScoreService _studentIncomeScoreService;
        private readonly IAcademicRecordDepartmentService _academicRecordDepartmentService;
        private readonly IDepartmentService _departmentService;
        private readonly IProvinceService _provinceService;
        private readonly IDistrictService _districtService;
        private readonly IStudentCourseCertificateService _studentCourseCertificateService;
        private readonly ICourseCertificateService _courseCertificateService;
        private readonly IEnrollmentShiftService _enrollmentShiftService;
        private readonly ICareerEnrollmentShiftService _careerEnrollmentShiftService;
        private readonly IEnrollmentConceptService _enrollmentConceptService;
        private readonly IStudentPortfolioService _studentPortfolioService;
        protected ReportSettings _reportSettings;

        public StudentInformationController(AkdemicContext context, UserManager<ApplicationUser> userManager,
            IConverter dinkConverter, IViewRenderService viewRenderService, IWebHostEnvironment environment,
            IOptions<CloudStorageCredentials> storageCredentials, ICurriculumService curriculumService,
            IStudentService studentService,
            IDataTablesService dataTablesService,
            IEvaluationReportService evaluationReportService,
            IAcademicHistoryDocumentService academicHistoryDocumentService,
            IMapper mapper,
            IUserService userService,
            IExtracurricularActivityStudentService extracurricularActivityStudentService,
            ITermService termService,
            IAcademicYearCourseService academicYearCourseService,
            ICertificateOfStudiesGeneratorService certificateOfStudiesGeneratorService,
            IStudentSectionService studentSectionService,
            IResolutionService resolutionService,
            IClassScheduleService classScheduleService,
            IClassStudentService classStudentService,
            ICampusService campusService,
            IPostulantService postulantService,
            IAcademicSummariesService academicSummariesService,
            IAcademicHistoryService academicHistoryService,
            ICourseService courseService,
            IAcademicProgramService academicProgramService,
            ICareerService careerService,
            IEnrollmentReservationService enrollmentReservationService,
            IStudentObservationService studentObservationService,
            IRegistryPatternService registryPatternService,
            IRecordHistoryService recordHistoryService,
            IConfigurationService configurationService,
            IDirectedCourseService directedCourseService,
            IDirectedCourseStudentService directedCourseStudentService,
            ICourseTermService courseTermService,
            IInvoiceService invoiceService,
            IConceptService conceptService,
            IPaymentService paymentService,
            ITextSharpService pdfSharpService,
            IProcedureDependencyService procedureDependencyService,
            IUserProcedureService userProcedureService,
            IProcedureRequirementService procedureRequirementService,
            IExtraordinaryEvaluationStudentService extraordinaryEvaluationStudentService,
            ISectionService sectionService,
            IStudentIncomeScoreService studentIncomeScoreService,
            IAcademicRecordDepartmentService academicRecordDepartmentService,
            IProcedureService procedureService, IEnrollmentTurnService enrollmentTurnService,
            IDepartmentService departmentService, IProvinceService provinceService, IDistrictService districtService,
            IStudentCourseCertificateService studentCourseCertificateService,
            ICourseCertificateService courseCertificateService,
            IEnrollmentShiftService enrollmentShiftService,
            ICareerEnrollmentShiftService careerEnrollmentShiftService,
            IEnrollmentConceptService enrollmentConceptService,
            IStudentPortfolioService studentPortfolioService,
            IOptionsSnapshot<ReportSettings> reportSettings
        ) : base(context, userManager, userService, termService, dataTablesService)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
            _storageCredentials = storageCredentials;
            _studentService = studentService;
            _evaluationReportService = evaluationReportService;
            _mapper = mapper;
            _extracurricularActivityStudentService = extracurricularActivityStudentService;
            _academicYearCourseService = academicYearCourseService;
            _certificateOfStudiesGeneratorService = certificateOfStudiesGeneratorService;
            _studentSectionService = studentSectionService;
            _resolutionService = resolutionService;
            _classScheduleService = classScheduleService;
            _classStudentService = classStudentService;
            _campusService = campusService;
            _postulantService = postulantService;
            _academicSummariesService = academicSummariesService;
            _academicHistoryService = academicHistoryService;
            _courseService = courseService;
            _academicProgramService = academicProgramService;
            _careerService = careerService;
            _academicHistoryDocumentService = academicHistoryDocumentService;
            _enrollmentReservationService = enrollmentReservationService;
            _studentObservationService = studentObservationService;
            _registryPatternService = registryPatternService;
            _recordHistoryService = recordHistoryService;
            _configurationService = configurationService;
            _directedCourseService = directedCourseService;
            _directedCourseStudentService = directedCourseStudentService;
            _courseTermService = courseTermService;
            _invoiceService = invoiceService;
            _conceptService = conceptService;
            _paymentService = paymentService;
            _pdfSharpService = pdfSharpService;
            _procedureDependencyService = procedureDependencyService;
            _userProcedureService = userProcedureService;
            _procedureRequirementService = procedureRequirementService;
            _procedureService = procedureService;
            _enrollmentTurnService = enrollmentTurnService;
            _curriculumService = curriculumService;
            _extraordinaryEvaluationStudentService = extraordinaryEvaluationStudentService;
            _sectionService = sectionService;
            _studentIncomeScoreService = studentIncomeScoreService;
            _academicRecordDepartmentService = academicRecordDepartmentService;
            _departmentService = departmentService;
            _provinceService = provinceService;
            _districtService = districtService;
            _courseCertificateService = courseCertificateService;
            _studentCourseCertificateService = studentCourseCertificateService;
            _enrollmentShiftService = enrollmentShiftService;
            _careerEnrollmentShiftService = careerEnrollmentShiftService;
            _reportSettings = reportSettings.Value;
            _enrollmentConceptService = enrollmentConceptService;
            _studentPortfolioService = studentPortfolioService;
        }
        /// <summary>
        /// Retorna la vista inicial de información de estudiantes
        /// </summary>
        /// <returns>Vista inicial</returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Retorna la lista de estudiantes disponibles según el rol del usuario
        /// </summary>
        /// <param name="faculty">Id de la facultad</param>
        /// <param name="career">Id de la escuela</param>
        /// <param name="academicProgram">Id del programa académicoo</param>
        /// <param name="search">Término a filtrar</param>
        /// <returns>Objeto con la lista de estudiantes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid faculty, Guid career, Guid academicProgram, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            if (User.IsInRole(CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR))
            {
                var userId = _userManager.GetUserId(User);
                var careers = await _careerService.GetCareerByUserCoordinatorId(userId);
                career = careers.Any() ? careers.FirstOrDefault().Id : Guid.Empty;

                var result = await _studentService.GetStudentsByFacultyAndCareerAndAcademicProgramDatatable(sentParameters, null, career, null, search);
                return Ok(result);
            }

            if (User.IsInRole(CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
            {
                var userId = _userManager.GetUserId(User);
                var careers = await _careerService.GetCareerByUserCoordinatorId(userId);
                career = careers.Any() ? careers.FirstOrDefault().Id : Guid.Empty;

                var result = await _studentService.GetStudentsByFacultyAndCareerAndAcademicProgramDatatable(sentParameters, null, career, null, search);
                return Ok(result);
            }
            else
            {
                var result = await _studentService.GetStudentsByFacultyAndCareerAndAcademicProgramDatatable(sentParameters, faculty, career, academicProgram, search, User);
                return Ok(result);
            }
        }

        /// <summary>
        /// Método para buscar a los estudiantes por el término ingresado y devolverlos en formato para elementos Select2
        /// </summary>
        /// <param name="term">Término a filtrar</param>
        /// <returns>Objeto con la lista de estudiantes</returns>
        [HttpGet("buscar")]
        public async Task<IActionResult> Search(string term)
        {
            var students = await _studentService.SearchStudentByTerm(term, null, User);
            return Ok(new { items = students });
        }
        /// <summary>
        /// Retorna la vista de la información del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Vista de información del estudiante</returns>
        [HttpGet("informacion/{id}")]
        public async Task<IActionResult> Information(Guid id)
        {
            var model = await _studentService.GetStudentinformationById(id);
            var result = _mapper.Map<StudentViewModel>(model);
            return View(result);
        }
        /// <summary>
        /// Retorna la vista parcial con las opciones del estudiante disponibles para el usuario
        /// </summary>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/opciones")]
        public IActionResult _Options()
        {
            return PartialView();
        }

        /// <summary>
        /// Retorna la vista parcial de la situación académica del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/{id}/estudiante/situacion")]
        public IActionResult _StudentSituation(Guid id)
        {
            return PartialView(id);
        }
        /// <summary>
        /// Genera el reporte de la situación académica del estudiante en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/estudiante/situacion-get-pdf")]
        public async Task<IActionResult> GetStudentSituationPdf(Guid id)
        {
            var student = await _studentService.GetWithData(id);
            var academicYears = await _curriculumService.GetAllAcademicYears(student.CurriculumId);

            var studentSummaries = await _academicSummariesService.GetAllByStudent(student.Id);
            var lastStudentTerm = studentSummaries.OrderBy(x => x.Term.StartDate).LastOrDefault();

            var electiveApprovedCredits = await _studentService.GetElectiveApprovedCredits(student.Id);
            var requiredApprovedCredits = await _studentService.GetRequiredApprovedCredits(student.Id);

            var curriculum = await _curriculumService.Get(student.CurriculumId);

            var model = new ViewModels.StudentInformation.PdfViewModels.AcademicSitutationViewModel
            {
                Student = student.User.FullName,
                AcademicProgram = student.AcademicProgram?.Name,
                Career = student.Career.Name,
                ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png"),
                StudentUsername = student.User.UserName,
                WeightedFinalGrade = lastStudentTerm?.WeightedAverageGrade ?? 0.00M,
                CumulativeWeightedFinalGrade = (studentSummaries.Sum(t => t.TotalCredits) != 0)
                    ? (studentSummaries.Sum(t => t.TotalCredits * t.WeightedAverageGrade) /
                       studentSummaries.Sum(t => t.TotalCredits))
                    : 0,
                RequiredApprovedCredits = requiredApprovedCredits,
                ElectiveApprovedCredits = electiveApprovedCredits,
                ElectiveCredits = curriculum.ElectiveCredits,
                RequiredCredits = curriculum.RequiredCredits
            };

            var details = new List<ViewModels.StudentInformation.PdfViewModels.AcademicSitutationDetailViewmodel>();

            foreach (var ay in academicYears)
            {
                var result = await _academicYearCourseService.GetAcademicYearDetailByStudentIdAndAcademicYearId(id, ay, User);
                foreach (var item in result)
                {
                    details.Add(new ViewModels.StudentInformation.PdfViewModels.AcademicSitutationDetailViewmodel
                    {
                        AcademicYear = ay,
                        Course = item.CourseName,
                        CourseCode = item.CourseCode,
                        Credits = item.Credits,
                        FinalGrade = item.Grade,
                        Semester = item.Term,
                        Status = item.Status ? "Cumplido" : "Pendiente"
                    });
                }
            }

            model.Details = details;

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/Pdf/AcademicSituationPdf.cshtml", model);
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 5, Right = 5 },
            };
            var objectSettings = new DinkToPdf.ObjectSettings
            {
                HtmlContent = viewToString,
                WebSettings =
                {
                    DefaultEncoding = "utf-8",
                },
                PagesCount = true,
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 7,
                    Right = "Pág: [page]/[toPage]",
                    Left ="Fecha: "+ DateTime.Now.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES"))+" - Hora: "+DateTime.Now.ToString("HH:mm 'hrs'", new CultureInfo("es-ES"))
                },
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileByte = _dinkConverter.Convert(pdf);

            if (!User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF) && !User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD))
            {
                if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAB && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAH)
                    _pdfSharpService.AddWatermarkToAllPages(ref fileByte, "Solo Lectura", 130);
            }

            return File(fileByte, "application/pdf", $"{model.StudentUsername}-SITUACIÓN ACADÉMICA.pdf");
        }
        /// <summary>
        /// Retorna la vista parcial del recordd de notas del estudiante en todos los periodos matriculados
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/{id}/estudiante/historial")]
        public IActionResult _StudentRecord(Guid id)
        {
            return PartialView(id);
        }

        /// <summary>
        /// Retorna la vista parcial sobre la información básica del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/{id}/datos-generales")]
        public async Task<IActionResult> _GeneralData(Guid id)
        {
            var model = await _studentService.GetStudentGeneralDataById(id);
            var result = _mapper.Map<GeneralDataViewModel>(model);

            var enableEnrollmentFees = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.Enrollment.ENABLE_ENROLLMENT_FEES));
            result.EnableFees = enableEnrollmentFees;

            return PartialView(result);
        }
        /// <summary>
        /// Método para actualizar la foto de perfil del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="model">Modelo con la foto del estudiante</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/actualizar-foto")]
        public async Task<IActionResult> UpdatePhoto(Guid id, GeneralDataViewModel model)
        {
            var student = await _studentService.Get(id);
            var user = await _userService.Get(student.UserId);

            var storage = new CloudStorageService(_storageCredentials);

            if (string.IsNullOrEmpty(model.UrlPhotoCropImg))
                return BadRequest("No se encontró la imagen.");

            var imgArray1 = model.UrlPhotoCropImg.Split(";");
            var imgArray2 = imgArray1[1].Split(",");

            var newImage = Convert.FromBase64String(imgArray2[1]);

            var term = await _termService.GetActiveTerm();
            using (var stream = new MemoryStream(newImage))
            {
                var extension = Path.GetExtension(model.Picture.FileName);
                user.Picture = await storage.UploadFile(stream, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.USER_PROFILE_PICTURE, Path.GetExtension(model.Picture.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                var observation = new StudentObservation
                {
                    Observation = $"Actualización de la foto de perfil del estudiante",
                    StudentId = id,
                    Type = ConstantHelpers.OBSERVATION_TYPES.INFO_UPDATE,
                    UserId = _userManager.GetUserId(User),
                    TermId = term != null ? term.Id : (Guid?)null
                };

                await _studentObservationService.Insert(observation);
            }

            await _userService.Update(user);

            return Ok();
        }
        /// <summary>
        /// Método para actualizar la información general del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="model">Modelo con datos del estudiante</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/datos-generales")]
        public async Task<IActionResult> _GeneralData(Guid id, GeneralDataViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Valores ingresados incorrectos. Revise la información.");

            if (ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate) > DateTime.UtcNow)
                return BadRequest("La Fecha de Nacimiento ingresada no es válida.");

            if (await _studentService.ValidateUserEmail(model.Email, model.Id))
                return BadRequest("El correo electrónico especificado ya se encuentra registrado.");

            if (!string.IsNullOrEmpty(model.PersonalEmail) && await _studentService.ValidateUserPersonalEmail(model.PersonalEmail, model.Id))
                return BadRequest("El correo electrónico alternativo especificado ya se encuentra registrado.");

            var student = await _studentService.Get(model.Id);
            var user = await _userService.Get(student.UserId);

            var term = await _termService.GetActiveTerm();

            var observations = new List<StudentObservation>();
            if (user.Email != model.Email)
            {
                observations.Add(new StudentObservation
                {
                    Observation = $"Actualización del correo electrónico del estudiante. Antes: {user.Email} | Ahora: {model.Email}",
                    StudentId = id,
                    Type = ConstantHelpers.OBSERVATION_TYPES.INFO_UPDATE,
                    UserId = _userManager.GetUserId(User),
                    TermId = term != null ? term.Id : (Guid?)null
                });

                user.Email = model.Email;
                user.NormalizedEmail = _userManager.NormalizeEmail(model.Email);
            }

            if (user.PersonalEmail != model.PersonalEmail)
            {
                observations.Add(new StudentObservation
                {
                    Observation = $"Actualización del correo electrónico alternativo del estudiante. Antes: {user.PersonalEmail} | Ahora: {model.PersonalEmail}",
                    StudentId = id,
                    Type = ConstantHelpers.OBSERVATION_TYPES.INFO_UPDATE,
                    UserId = _userManager.GetUserId(User),
                    TermId = term != null ? term.Id : (Guid?)null
                });

                user.PersonalEmail = model.PersonalEmail;
            }

            if (user.PhoneNumber != model.PhoneNumber)
            {
                observations.Add(new StudentObservation
                {
                    Observation = $"Actualización del número telefónico del estudiante. Antes: {user.PhoneNumber} | Ahora: {model.PhoneNumber}",
                    StudentId = id,
                    Type = ConstantHelpers.OBSERVATION_TYPES.INFO_UPDATE,
                    UserId = _userManager.GetUserId(User),
                    TermId = term != null ? term.Id : (Guid?)null
                });
                user.PhoneNumber = model.PhoneNumber;
            }

            user.Name = model.Name;
            user.MaternalSurname = model.MaternalSurname;
            user.PaternalSurname = model.PaternalSurname;
            var fullname = $"{(string.IsNullOrEmpty(model.PaternalSurname) ? "" : $"{model.PaternalSurname} ")}{(string.IsNullOrEmpty(model.MaternalSurname) ? "" : $"{model.MaternalSurname}")}, {(string.IsNullOrEmpty(model.Name) ? "" : $"{model.Name}")}";
            if (fullname != user.FullName)
            {
                observations.Add(new StudentObservation
                {
                    Observation = $"Actualización del nombre de estudiante. Antes: {user.FullName} | Ahora: {fullname}",
                    StudentId = id,
                    Type = ConstantHelpers.OBSERVATION_TYPES.INFO_UPDATE,
                    UserId = _userManager.GetUserId(User),
                    TermId = term != null ? term.Id : (Guid?)null
                });

                user.FullName = fullname;
            }

            if (user.Address != model.Address || user.DepartmentId != model.DepartmentId || user.ProvinceId != model.ProvinceId || user.DistrictId != model.DistrictId)
            {
                var oldDepartment = user.DepartmentId.HasValue ? (await _departmentService.Get(user.DepartmentId.Value)).Name : "";
                var oldProvince = user.ProvinceId.HasValue ? (await _provinceService.Get(user.ProvinceId.Value)).Name : "";
                var oldDistrict = user.DistrictId.HasValue ? (await _districtService.Get(user.DistrictId.Value)).Name : "";

                var newDepartment = model.DepartmentId.HasValue ? (await _departmentService.Get(model.DepartmentId.Value)).Name : "";
                var newProvince = model.ProvinceId.HasValue ? (await _provinceService.Get(model.ProvinceId.Value)).Name : "";
                var newDistrict = model.DistrictId.HasValue ? (await _districtService.Get(model.DistrictId.Value)).Name : "";

                observations.Add(new StudentObservation
                {
                    Observation = $"Actualización de la dirección del estudiante. Antes: {oldDepartment} - {oldProvince} - {oldDistrict} - {user.Address} | Ahora: {newDepartment} - {newProvince} - {newDistrict} - {model.Address}",
                    StudentId = id,
                    Type = ConstantHelpers.OBSERVATION_TYPES.INFO_UPDATE,
                    UserId = _userManager.GetUserId(User),
                    TermId = term != null ? term.Id : (Guid?)null
                });

                user.Address = model.Address;
                user.DepartmentId = model.DepartmentId;
                user.ProvinceId = model.ProvinceId;
                user.DistrictId = model.DistrictId;
            }

            var date = ConvertHelpers.DatepickerToDatetime(model.BirthDate);
            if (user.BirthDate.Date != date.Date)
            {
                observations.Add(new StudentObservation
                {
                    Observation = $"Actualización de la fecha de nacimiento del estudiante. Antes: {user.BirthDate:dd/MM/yyyy} | Ahora: {date:dd/MM/yyyy}",
                    StudentId = id,
                    Type = ConstantHelpers.OBSERVATION_TYPES.INFO_UPDATE,
                    UserId = _userManager.GetUserId(User),
                    TermId = term != null ? term.Id : (Guid?)null
                });
                user.BirthDate = ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate);
            }

            if (user.Sex != model.Sex)
            {
                var prevSex = user.Sex == 1 ? "Masculino" : "Femenino";
                var newSex = model.Sex == 1 ? "Masculino" : "Femenino";
                observations.Add(new StudentObservation
                {
                    Observation = $"Actualización del sexo del estudiante. Antes: {prevSex} | Ahora: {newSex}",
                    StudentId = id,
                    Type = ConstantHelpers.OBSERVATION_TYPES.INFO_UPDATE,
                    UserId = _userManager.GetUserId(User),
                    TermId = term != null ? term.Id : (Guid?)null
                });
                user.Sex = model.Sex;
            }

            if (user.Dni != model.Dni)
            {
                observations.Add(new StudentObservation
                {
                    Observation = $"Actualización del documento de identidad del estudiante. Antes: {user.Dni} | Ahora: {model.Dni}",
                    StudentId = id,
                    Type = ConstantHelpers.OBSERVATION_TYPES.INFO_UPDATE,
                    UserId = _userManager.GetUserId(User),
                    TermId = term != null ? term.Id : (Guid?)null
                });
                user.Dni = model.Dni;
                user.Document = model.Dni;
            }

            // Upload Picture
            if (model.Picture != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (!string.IsNullOrEmpty(user.Picture))
                    await storage.TryDelete(user.Picture, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.USER_PROFILE_PICTURE);

                user.Picture = await storage.UploadFile(model.Picture.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.USER_PROFILE_PICTURE,
                    Path.GetExtension(model.Picture.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                observations.Add(new StudentObservation
                {
                    Observation = $"Actualización de la foto de perfil del estudiante",
                    StudentId = id,
                    Type = ConstantHelpers.OBSERVATION_TYPES.INFO_UPDATE,
                    UserId = _userManager.GetUserId(User),
                    TermId = term != null ? term.Id : (Guid?)null
                });
            }

            await _userService.Update(user);

            student.RacialIdentity = model.RacialIdentity;
            student.EnrollmentFeeId = model.EnrollmentFeeId.HasValue && model.EnrollmentFeeId != Guid.Empty ? model.EnrollmentFeeId : null;
            await _studentService.Update(student);

            await _studentObservationService.InsertRange(observations);

            return Ok();
        }
        /// <summary>
        /// Retorna la vista parcial de cambio de clave del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/{id}/cambiar-clave")]
        public IActionResult _ChangePassword(Guid id)
        {
            return PartialView();
        }
        /// <summary>
        /// Método para actualizar la clave del estudiante con una nueva clave ingresada por el usuario
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="model">Modelo con la nueva clave del usuario</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/cambiar-clave")]
        public async Task<IActionResult> _ChangePassword(Guid id, ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Información inválida");
            }

            var student = await _studentService.Get(id);
            var user = await _userService.Get(student.UserId);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

            if (!result.Succeeded)
                return BadRequest(string.Join("; ", result.Errors.Select(x => x.Description).ToList()));

            return Ok();
        }
        /// <summary>
        /// Retorna la vista parcial de trámites del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/{id}/tramites")]
        public IActionResult _Procedures(Guid id)
        {
            return PartialView();
        }
        /// <summary>
        /// Retorna la lista de trámites realizados por el estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="search">Término a filtrar</param>
        /// <returns>Objeto con la lista de trámites</returns>
        [HttpGet("informacion/{id}/tramites/get")]
        public async Task<IActionResult> GetUserProcedures(Guid id, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetUserProceduresDatatable(sentParameters, id, search);
            return Ok(result);
        }
        /// <summary>
        /// Retorna la vista parcial de matrículas del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/{id}/matricula-actual")]
        public IActionResult _Enrollment(Guid id)
        {
            return PartialView();
        }
        /// <summary>
        /// Retorna la lista de cursos matriculados del estudiante en el periodo indicado
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="termId">Id del periodo académica</param>
        /// <returns>Objeto con la lista de cursos matriculados</returns>
        [HttpGet("informacion/{id}/matriculas/get")]
        public async Task<IActionResult> GetEnrolledCourses(Guid id, Guid termId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetEnrolledCoursesDatatable(sentParameters, id, termId);
            return Ok(result);
        }
        /// <summary>
        /// Retorna la lista de otros cursos matriculados de manera extraordinaria
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="termId">Id del periodo académico</param>
        /// <returns>Objeto con la lista de cursos</returns>
        [HttpGet("informacion/{id}/matriculas/otros-cursos/get")]
        public async Task<IActionResult> GetOtherEnrolledCourses(Guid id, Guid termId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetOthersEnrolledCoursesDatatable(sentParameters, id, termId);
            return Ok(result);
        }
        /// <summary>
        /// Método para generar la ficha de matrícula del estudiante del periodo inidicado
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="termId">Id del periodo académico</param>
        /// <param name="pronabec">Flag si mostrar datos de Pronabec</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/matriculas/reporte")]
        [HttpGet("informacion/{id}/matriculas/reporte/{termId}")]
        public async Task<IActionResult> EnrollmentReport(Guid id, Guid termId, bool pronabec = false)
        {
            var includeExtraordinary = false;
            if (!pronabec)
                includeExtraordinary = true;

            var url = string.Empty;
            if (ConstantHelpers.Solution.Routes.Keys.Contains(ConstantHelpers.GENERAL.Institution.Value))
            {
                var baseUrl = ConstantHelpers.Solution.Routes[ConstantHelpers.GENERAL.Institution.Value][CORE.Helpers.ConstantHelpers.Solution.Enrollment];
                url = $"{baseUrl}admin/matricula/alumno/{id}/detalle-cursos-matriculados";
            }

            var template = await _studentSectionService.GetEnrollmentReportTemplate(id, termId, pronabec: pronabec, qrUrl: url, includeExtraordinaryEvaluations: includeExtraordinary);

            template.Image = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png");

            var storage = new CloudStorageService(_storageCredentials);

            var signatureImageUrl = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IMAGE_CERTIFICATE_SIGNATURE);
            var signatureBase64 = string.Empty;

            if (!string.IsNullOrEmpty(signatureImageUrl))
            {
                using (var mem = new MemoryStream())
                {
                    await storage.TryDownload(mem, ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION, signatureImageUrl);
                    signatureBase64 = $"data:image/png;base64, {Convert.ToBase64String(mem.ToArray())}";
                }
            }

            template.SignatuareImgBase64 = signatureBase64;

            var orientation = DinkToPdf.Orientation.Portrait;
            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG)
                orientation = DinkToPdf.Orientation.Portrait;

            var margin = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 15, Right = 15 };
            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG)
                margin = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 };
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = orientation,
                PaperSize = DinkToPdf.PaperKind.Letter,
                Margins = margin,
                DPI = 290
            };

            var objectSettings = new DinkToPdf.ObjectSettings();

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG)
            {
                template.PrintingDate = DateTime.UtcNow.ToDefaultTimeZone().ToString("dddd, dd MMMM yyyy", new CultureInfo("es-PE"));

                var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/Pdf/EnrollmentReportUNJBG.cshtml", template);

                objectSettings = new DinkToPdf.ObjectSettings
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
            }
            else
            {
                var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/Pdf/EnrollmentReport.cshtml", template);
                var cssPtah = Path.Combine(_hostingEnvironment.WebRootPath, @"css/areas/academic/studentinformation/enrollmentreport.css");

                objectSettings = new DinkToPdf.ObjectSettings
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
            }

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var fileByte = _dinkConverter.Convert(pdf);

            //HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            //return File(fileByte, "application/pdf", "Reporte matricula.pdf");
            return File(fileByte, "application/pdf", $"{template.StudentCode}-Matricula {template.Semester}.pdf");
        }
        /// <summary>
        /// Retorna la lista de secciones disponibles del curso en el periodo
        /// </summary>
        /// <param name="id">Id de la matrícula en la sección</param>
        /// <returns>Objeto con la lista de secciones</returns>
        [HttpGet("informacion/matricula-actual/{id}/secciones-disponibles")]
        public async Task<IActionResult> GetAvailableSections(Guid id)
        {
            var availableSections = await _studentService.GetAvailableSections(id);
            var result = new
            {
                items = availableSections
            };

            return Ok(result);
        }
        /// <summary>
        /// Métoddo para actualizar la sección del curso del estudiante
        /// </summary>
        /// <param name="id">Id del estuddiante</param>
        /// <param name="model">Modelo con datos actualizados de la matrícula</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/matricula-actual/cambiar-seccion")]
        public async Task<IActionResult> ChangeSection(Guid id, ChangeSectionViewModel model)
        {
            var studentSection = await _studentSectionService.Get(model.StudentSectionId);

            studentSection.SectionId = model.NewSectionId;

            var storage = new CloudStorageService(_storageCredentials);

            var fileURL = await storage.UploadFile(model.File.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.RESOLUTIONS,
                Path.GetExtension(model.File.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);

            var resolution = new Resolution
            {
                TableName = _context.Model.FindEntityType(typeof(StudentSection)).GetTableName(),
                KeyValue = studentSection.Id,
                Description = $"Cambio de sección del estudiante",
                Number = model.Resolution,
                IssueDate = DateTime.UtcNow,
                FilePath = fileURL
            };

            await _resolutionService.Insert(resolution);

            return Ok();
        }
        /// <summary>
        /// Retorna la vista parcial del horario de clases del periodo del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/{id}/horario")]
        public IActionResult _Schedule(Guid id)
        {
            return PartialView();
        }
        /// <summary>
        /// Retorna el horario de clases del estudiante en el formato para elementos FullCalendar
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Objeto con el horario</returns>
        [HttpGet("informacion/{id}/horario/get")]
        public async Task<IActionResult> GetSchedule(Guid id)
        {
            var termId = Guid.Empty;
            var term = await GetActiveTerm();
            if (term == null)
                term = await _termService.GetLastTerm();
            if (term != null)
                termId = term.Id;

            var result = await _classScheduleService.GetSchedule(id, termId);

            return Ok(result);
        }

        [HttpGet("informacion/{id}/horario/reporte-pdf")]
        public async Task<IActionResult> GetSchedulePdf(Guid id)
        {
            var termId = Guid.Empty;
            var term = await GetActiveTerm();
            if (term == null)
                term = await _termService.GetLastTerm();
            if (term != null)
                termId = term.Id;

            var model = await _classScheduleService.GetScheduleReport(id, termId);

            //Schedule Parameters
            model.ScheduleParameters = new List<(TimeSpan, TimeSpan, string)>();
            int start_time = 7;
            int end_time = 23;

            for (int i = start_time; i < end_time; i++)
            {
                model.ScheduleParameters.Add((new TimeSpan(0, i, 0, 0, 0), new TimeSpan(0, i + 1, 0, 0), $"{i:00}.00 - {i + 1:00}.00"));
            }

            model.Image = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png");

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/Pdf/StudentSchedule.cshtml", model);
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
                        Right = "Pag: [page]/[toPage]"
                }
            };

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileByte = _dinkConverter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileByte, "application/pdf", "horario_estudiante.pdf");
        }


        /// <summary>
        /// Retorna la vista parcial del historial de notas completo del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/{id}/historial")]
        public async Task<IActionResult> _AcademicHistory(Guid id)
        {
            var model = await _studentSectionService.GetTermSelectListByStudent(id);
            //Actualmente usamos la Version 2 de esta vista parcial
            var version = 2;
            var partialView = "";
            switch (version)
            {
                case 1:
                    partialView = "_AcademicHistory";
                    return PartialView(partialView, model);
                case 2:
                    partialView = "_AcademicHistoryVer2";
                    return PartialView(partialView, id);
                default:
                    partialView = "_AcademicHistoryVer2";
                    return PartialView(partialView, id);
            }
        }
        /// <summary>
        /// Retorna la lista completa de cursos matriculados y sus notas obtenidas
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Objjeto con la lista de notas</returns>
        [HttpGet("informacion/{id}/historial/cursos")]
        public async Task<IActionResult> GetAcademicHistoryDatatable(Guid id)
        {
            //aqui
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _academicYearCourseService.GetAcademicHistoryV2CoursesDatatable(sentParameters, id);

            return Ok(result);
        }
        /// <summary>
        /// Método para generar el reporte PDF del historial de notas del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/historial-notas/reporte-pdf")]
        public async Task<IActionResult> AcademicHistoryPdf(Guid id)
        {
            var result = await _academicYearCourseService.GetAcademicHistoryV2Courses(id);
            var student = await _studentService.GetStudentWithCareerAndUser(id);
            var model = new ViewModels.StudentInformation.PdfViewModels.AcademicHistoryCoursePdfViewModel
            {
                ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png"),
                JsPath = Path.Combine(_hostingEnvironment.WebRootPath, @"js/lib/jsbarcode/JsBarcode.all.min.js"),
                Today = DateTime.UtcNow.ToDefaultTimeZone(),
                StudentInfo = new ViewModels.StudentInformation.PdfViewModels.StudentInfoViewModel
                {
                    Position = student.Position.ToString(),
                    Score = student.Postulants != null ? student.Postulants.Select(x => x.FinalScore.ToString("0.##")).FirstOrDefault() : "0.00",
                    Modality = student.AdmissionType.Name,
                    RegisterNumber = "",//student.use
                    IncomeYear = student.AdmissionTerm.Year.ToString(),
                    StudentName = student.User.FullName,
                    StudentCode = student.User.UserName,
                    StudentDni = student.User.Dni,
                    CareerName = student.Career.Name,
                    Faculty = student.Career.Faculty.Name,
                    CampusName = GeneralHelpers.GetInstitutionLocation(),
                    CurriculumCode = student.Curriculum.Code,
                    StudentRegime = student.AcademicProgram == null ? "" : student.AcademicProgram.Name,
                    /*****************************/
                    Image = string.IsNullOrEmpty(student.User.Picture) ? Path.Combine(_hostingEnvironment.WebRootPath, $@"images/demo/user.png") : await GeneralHelpers.GetImageForStringPartialView(_storageCredentials, student.User.Picture)
                    /****************************/
                },
                Details = result
                .Select(x => new ViewModels.StudentInformation.PdfViewModels.HistoryCourseViewModel
                {
                    Year = x.Year,
                    Code = x.Code,
                    Term = x.Term,
                    Status = x.Status,
                    Grade = x.Grade,
                    Approved = x.Approved,
                    Credits = x.Credits.ToString("0.0"),
                    Course = x.Course,
                    Observations = x.Observations,
                    Validated = x.Validated,
                    Type = x.Type,
                    Withdraw = x.Withdraw
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Code)
                .ThenBy(x => x.Term)
                .ToArray()
            };
            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/Pdf/AcademicHistoryCoursePdf.cshtml", model);
            //var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css\pages\pdf\academichistorypdf.css");
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 5, Right = 5 },
            };
            var objectSettings = new DinkToPdf.ObjectSettings
            {
                HtmlContent = viewToString,
                WebSettings =
                {
                    DefaultEncoding = "utf-8",
                },
                PagesCount = true,
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 8,
                    Right = "Pág: [page]/[toPage]",
                    Left ="Fecha: "+ DateTime.Now.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES"))+" - Hora: "+DateTime.Now.ToString("HH:mm 'hrs'", new CultureInfo("es-ES")),
                    Line = true
                },
                //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var fileByte = _dinkConverter.Convert(pdf);

            if (!User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF) && !User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD))
            {
                if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAB && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAH)
                    _pdfSharpService.AddWatermarkToAllPages(ref fileByte, "Solo Lectura", 130);
            }

            //HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf");
        }


        [HttpGet("informacion/{id}/historial-notas/record-academico-pdf")]
        public async Task<IActionResult> AcademicRecordPdf(Guid id)
        {
            var student = await _studentService.GetStudentWithCareerAndUser(id);
            var result = await _academicYearCourseService.GetAcademicHistoryV2Courses(id);
            var academicYears = result.GroupBy(x => x.AcademicYear).ToList();

            var model = new ViewModels.StudentInformation.PdfViewModels.AcademicRecordV2PdfViewModel
            {
                ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png"),
                Career = student.Career.Name,
                UserName = student.User.UserName,
                FullName = student.User.FullName,
                Curriculum = student.Curriculum.Code,
                Faculty = student.Career.Faculty.Name,
                AcademicYears = new List<ViewModels.StudentInformation.PdfViewModels.AcademicrecordV2AcademicYearPdfViewModel>(),
                ExtracurricularAreas = new List<ViewModels.StudentInformation.PdfViewModels.AcademicRecordV2ExtracurricularAreaViewModel>(),
            };

            if (student.CampusId.HasValue)
            {
                var studentCampus = await _campusService.Get(student.CampusId.Value);
                model.Campus = studentCampus.Name;
            }

            var acumulativeGradexCredit = 0m;
            var acumulativeCredits = 0M;
            var sumApprovedCredits = 0M;
            foreach (var item in academicYears)
            {
                var credits = item.Sum(x => x.Credits);
                var gradeXCredit = item.Sum(x => x.Credits * x.Grade);

                var approvedCredits = item.Where(x => x.Approved).Sum(x => x.Credits);

                acumulativeCredits += credits;
                acumulativeGradexCredit += gradeXCredit;
                sumApprovedCredits += approvedCredits;

                var entity = new ViewModels.StudentInformation.PdfViewModels.AcademicrecordV2AcademicYearPdfViewModel
                {
                    AcademicYear = item.Key,
                    AccumulatedAverageFormula = $"{acumulativeGradexCredit}/{acumulativeCredits}",
                    AccumulatedAverage = Math.Round(acumulativeGradexCredit / acumulativeCredits, 3, MidpointRounding.AwayFromZero),

                    SemesterAverage = Math.Round(gradeXCredit / credits, 3, MidpointRounding.AwayFromZero),
                    SemesterAverageFormula = $"{gradeXCredit}/{credits}",

                    AcademicHistories = item.Select(ayc => new ViewModels.StudentInformation.PdfViewModels.AcademicRecordV2HistoryPdfViewModel
                    {
                        CourseCode = ayc.Code,
                        CourseName = ayc.Course,
                        Credits = ayc.Credits,
                        Term = ayc.Term,
                        Grade = ayc.Grade,
                        Observation = ayc.Type != ConstantHelpers.AcademicHistory.Types.REGULAR && !string.IsNullOrEmpty(ayc.AcademicObservations) ? ayc.AcademicObservations : ConstantHelpers.AcademicHistory.Types.VALUES[ayc.Type],
                        Date = ayc.EvaluationReportDate.ToLocalDateFormat(),
                        Try = ayc.Try,
                        AcademicHistoryType = ayc.Type
                    })
                    .ToList()
                };
                model.AcademicYears.Add(entity);
            }

            var extracurricularAreas = await _extracurricularActivityStudentService.GetAllByStudent(student.Id);

            var areas = extracurricularAreas.GroupBy(x => x.ExtracurricularActivity.ExtracurricularAreaId).ToList();

            foreach (var item in areas)
            {
                model.ExtracurricularAreas.Add(new ViewModels.StudentInformation.PdfViewModels.AcademicRecordV2ExtracurricularAreaViewModel
                {
                    Name = item.Select(x => x.ExtracurricularActivity.ExtracurricularArea.Name).FirstOrDefault(),
                    Activities = item.Select(x => new ViewModels.StudentInformation.PdfViewModels.AcademicRecordv2ExtracurricularActivityViewModel
                    {
                        Code = x.ExtracurricularActivity.Code,
                        Name = x.ExtracurricularActivity.Name,
                        Credits = x.ExtracurricularActivity.Credits,
                        Term = x.ExtracurricularActivity.Term.Name,
                        Try = 1,
                        Grade = x.Grade,
                        Date = x.EvaluationReportDate.ToLocalDateFormat(),
                        Type = "Reconocimiento"
                    }).ToList()
                });

                sumApprovedCredits += item.Where(x => x.Grade.HasValue && x.Grade >= x.ExtracurricularActivity.Term.MinGrade).Sum(x => x.ExtracurricularActivity.Credits);
            }

            model.TotalApprovedCredits = sumApprovedCredits;

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/Pdf/AcademicRecordV2Pdf.cshtml", model);
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
            };
            var objectSettings = new DinkToPdf.ObjectSettings
            {
                HtmlContent = viewToString,
                WebSettings =
                {
                    DefaultEncoding = "utf-8",
                },
                PagesCount = true,
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 8,
                    Right = "Pág. [page]",
                    Left =$"Fecha de Impresión: {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Line = true
                },
                //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var fileByte = _dinkConverter.Convert(pdf);

            //HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf");
        }


        /// <summary>
        /// Retorna el historial de notas del estudiante del periodo indicado
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="termid">Id del periodo académico</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/{id}/historial/{termid}/notas")]
        public async Task<IActionResult> GetGrades(Guid id, Guid termid)
        {

            var model = await _studentSectionService.GetGradesByStudentAnTerm(id, termid);
            //var result = _mapper.Map<List<CourseViewModel>>(model);
            var result = model.Select(x => new CourseViewModel
            {
                Try = x.Try,
                Course = x.Course,
                FinalGrade = x.FinalGrade,
                Observations = x.Observations,
                Status = x.Status,
                Formula = x.Formula,
                Section = x.Section,
                Credits = x.Credits.ToString("0.0"),
                PercentageProgress = x.PercentageProgress,
                Evaluations = x.Evaluations.Select(g => new CourseEvaluationViewModel
                {
                    Name = g.Name,
                    Percentage = g.Percentage,
                    Grade = g.Grade,
                    Approved = g.Approved,
                    Attended = g.Attended,
                    Taked = g.Taked
                }).ToList()
            }).ToList();
            return PartialView("_AcademicHistoryCourses", result);
        }

        /// <summary>
        /// Retorna la vista parcial del control de asistencia del alumno
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns></returns>
        [HttpGet("informacion/{id}/asistencia")]
        public async Task<IActionResult> _AssistanceHistory(Guid id)
        {
            var model = await _studentSectionService.GetTermSelectListByStudent(id);

            return PartialView(model);
        }
        /// <summary>
        /// Retorna la lista de cursos matriculados en un periodo con la información sobre su asistencia
        /// </summary>
        /// <param name="id">Id del periodo</param>
        /// <param name="termid">Término a filtrar</param>
        /// <returns>Objeto con la lista de cursos</returns>
        [HttpGet("informacion/{id}/asistencia/{termid}/cursos")]
        public async Task<IActionResult> GetCoursesAssistance(Guid id, Guid termid)
        {

            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentSectionService.GetCoursesAssistanceByStudentAndTermDatatable(sentParameters, id, termid);
            return Ok(result);
        }
        /// <summary>
        /// Retorna el detalle de asistencias del alumno en un curso específico
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="sectionid">Id de la sección</param>
        /// <returns>Objeto con la lista de asistencias</returns>
        [HttpGet("informacion/{id}/asistencia/{sectionid}/detalle")]
        public async Task<IActionResult> GetCoursesAssistanceDetail(Guid id, Guid sectionid)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _classStudentService.GetCoursesAssistanceDetailByStudentAndSectionDatatable(sentParameters, id, sectionid);
            return Ok(result);
        }

        /// <summary>
        /// Método para la carga del documento de historial académico del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="model">Modelo con los datos del documento</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/situacion-academica/adjuntar-archivo")]
        public async Task<IActionResult> CurriculumDigitalUpload(Guid id, AcademicHistoryDocumentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Debe completar todos los campos");
            }
            var storage = new CloudStorageService(_storageCredentials);

            var student = await _studentService.GetWithIncludes(id);
            var oldDocument = await _academicHistoryDocumentService.GetByStudentId(id);
            if (oldDocument != null)
            {
                await storage.TryDelete(oldDocument.DocumentUrl, ConstantHelpers.CONTAINER_NAMES.ACADEMICHISTORYDOCUMENT);
                await _academicHistoryDocumentService.Delete(oldDocument);
            }

            var correlative = (await _academicHistoryDocumentService.GetLastCorrelative()) + 1;

            var academicHistoryDocument = new AcademicHistoryDocument
            {
                StudentId = student.Id,
                PhysicalLocation = correlative.ToString().PadLeft(6, '0')
            };

            if (model.CPDocument != null)
            {
                academicHistoryDocument.DocumentUrl = await storage.UploadFile(model.CPDocument.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.ACADEMICHISTORYDOCUMENT, $"{correlative.ToString().PadLeft(6, '0')}-HA-{student.User.UserName}",
                    Path.GetExtension(model.CPDocument.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            await _academicHistoryDocumentService.Insert(academicHistoryDocument);

            return Ok();
        }
        /// <summary>
        /// Retorna la información del archivo adjunto del historial académico
        /// </summary>
        /// <param name="studentId">Id del estudiante</param>
        /// <returns>Objeto con información del archivo</returns>
        [HttpGet("informacion/{studentId}/situacion-academica/archivo-alumno")]
        public async Task<IActionResult> CurriculumDigitalByStudent(Guid studentId)
        {
            var result = await _academicHistoryDocumentService.GetByStudentId(studentId);
            return Ok(result);
        }
        /// <summary>
        /// Método para descargar el archivo adjunto del historial académico del alumno
        /// </summary>
        /// <param name="studentId">Id del estudiante</param>
        /// <returns>Documento adjunto</returns>
        [HttpGet("informacion/{studentId}/situacion-academica/archivo-alumno/descargar")]
        public async Task DownloadCurriculumDigitalByStudent(Guid studentId)
        {
            var entity = await _academicHistoryDocumentService.GetByStudentId(studentId);
            if (entity is null)
                return;

            var storage = new CloudStorageService(_storageCredentials);
            using (var mem = new MemoryStream())
            {
                await storage.TryDownload(mem, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.ACADEMICHISTORYDOCUMENT, entity.DocumentUrl);

                var fileName = Path.GetFileName(entity.DocumentUrl);
                HttpContext.Response.Headers["Content-Disposition"] = $"attachment;filename=\"{fileName}\"";
                mem.Position = 0;
                await mem.CopyToAsync(HttpContext.Response.Body);
            }

        }
        /// <summary>
        /// Método para actualizar la información de ingreso del estudiante
        /// </summary>
        /// <param name="model">Modelo con información del estudiante</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{studentId}/situacion-academica/data-admision")]
        public async Task<IActionResult> AcademicHistoryAdmissionData(AcademicHistoryAdmissionData model)
        {
            var data = await _studentIncomeScoreService.GetByStudent(model.StudentId);

            if (data is null)
            {
                var entity = new StudentIncomeScore
                {
                    TotalStudents = model.TotalStudents,
                    Order = model.Order,
                    PossibleScore = model.PossibleScore,
                    Score = model.Score,
                    StudentId = model.StudentId,
                    RegistrationNumber = model.RegistrationNumber
                };

                await _studentIncomeScoreService.Insert(entity);
            }
            else
            {
                data.PossibleScore = model.PossibleScore;
                data.Order = model.Order;
                data.Score = model.Score;
                data.TotalStudents = model.TotalStudents;
                data.RegistrationNumber = model.RegistrationNumber;

                await _studentIncomeScoreService.Update(data);
            }

            return Ok();
        }

        /// <summary>
        /// Retorna la información de ingreso del estudiante
        /// </summary>
        /// <param name="studentId">Id del estudiante</param>
        /// <returns>Objeto con información del ingreso</returns>
        [HttpGet("informacion/{studentId}/situacion-academica/data-admision/get")]
        public async Task<IActionResult> AcademicHistoryAdmissionData(Guid studentId)
        {
            var data = await _studentIncomeScoreService.GetByStudent(studentId);
            return Ok(data);
        }
        /// <summary>
        /// Retorna la vista parcial del historial académico del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/{id}/situacion-academica")]
        public async Task<IActionResult> _CurriculumProgress(Guid id)
        {
            var file = await _academicHistoryDocumentService.GetByStudentId(id);
            var credits = await _academicSummariesService.GetTotalCreditsApproved(id);
            var approvedCourses = await _studentService.GetNumberOfApprovedCourses(id);

            var model = new CurriculumProgressViewModel
            {
                Id = id,
                TotalAppovedCredits = credits,
                TotalApprovedCourses = approvedCourses,
                HasFile = file != null
            };

            return PartialView(model);
        }
        /// <summary>
        /// Método para validar la información de ingreso del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpGet("informacion/{id}/situacion-academica/validar-reporte-pdf")]
        public async Task<IActionResult> ValidateCurriculumProgressPdf(Guid id)
        {
            return Ok();
        }
        /// <summary>
        /// Método para generar el reporte de historial académico del estudiante y su progreso según el formato PDF del sistema
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/situacion-academica/reporte-pdf")]
        public async Task<IActionResult> CurriculumProgressPdf(Guid id)
        {
            await _academicSummariesService.ReCreateStudentAcademicSummaries(id);

            var student = await _studentService.GetStudentWithCareerAndUser(id);
            //var academicYears = await _academicYearService.GetAllWithAcademicYearCoursesAndHistoriesByCurriculumAndStudent(student.CurriculumId, student.Id);

            var studentIncomeData = await _studentIncomeScoreService.GetByStudent(student.Id);
            var academicYearCourses = await _academicYearCourseService.GetWithHistoriesByCurriculumAndStudent(student.CurriculumId, student.Id);
            var academicYears = academicYearCourses
                .GroupBy(x => x.AcademicYear)
                .ToList();

            var evaluationReportDateConfi = Convert.ToByte(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE));

            var result = await _academicSummariesService.GetAcademicPerformanceSummary(id);
            var details = result.Select(x => new ViewModels.StudentInformation.PdfViewModels.AcademicPerformanceSummaryDetailViewModel
            {
                ApprovedCredits = x.ApprovedCredits.ToString("0.0"),
                Average = x.Average,
                DisapprovedCredits = x.DisapprovedCredits.ToString("0.0"),
                Term = x.Term,
                TotalCredits = x.TotalCredits.ToString("0.0"),
                Year = x.Year,
                Number = x.Number,
                CumulativeScore = x.CumulativeScore,
                CumulativeWeightedAverage = x.CumulativeWeightedAverage,
                WeightedAverageGrade = x.WeightedAverageGrade,
                TermScore = x.TermScore,
                AdditionalCredits = x.ExtraCredits
            }).ToList();

            var model = new ViewModels.StudentInformation.PdfViewModels.AcademicHistoryPdfViewModel
            {
                Details = details,
                ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png"),
                JsPath = Path.Combine(_hostingEnvironment.WebRootPath, @"js/lib/jsbarcode/JsBarcode.all.min.js"),
                Today = DateTime.UtcNow.ToDefaultTimeZone(),
                StudentInfo = new ViewModels.StudentInformation.PdfViewModels.StudentInfoViewModel
                {
                    Position = $"{studentIncomeData?.Order}/{studentIncomeData?.TotalStudents}",
                    Score = $"{studentIncomeData?.Score}/{studentIncomeData?.PossibleScore}",
                    Modality = student.AdmissionType?.Name,
                    RegisterNumber = studentIncomeData?.RegistrationNumber,
                    IncomeYear = student.AdmissionDate.HasValue ? student.AdmissionDate.Value.Year.ToString() : "",
                    StudentName = student.User.FullName,
                    StudentCode = student.User.UserName,
                    StudentDni = student.User.Dni,
                    CareerName = student.Career.Name,
                    Faculty = student.Career.Faculty.Name,
                    CampusName = GeneralHelpers.GetInstitutionLocation(),
                    CurriculumCode = student.Curriculum.Code,
                    StudentRegime = student.AcademicProgramId.HasValue ? student.AcademicProgram.Name : "",
                    /*****************************/
                    Image = string.IsNullOrEmpty(student.User.Picture) ? Path.Combine(_hostingEnvironment.WebRootPath, $@"images/demo/user.png") : await GeneralHelpers.GetImageForStringPartialView(_storageCredentials, student.User.Picture)
                    /****************************/
                },
                AcademicYears = academicYears.Select(ay => new ViewModels.StudentInformation.PdfViewModels.AcademicYearViewModel
                {
                    AcademicYearNumber = ay.Key,
                    Courses = ay.Select(ayc => new ViewModels.StudentInformation.PdfViewModels.CourseViewModel
                    {
                        Name = ayc.Course.Name,
                        Code = ayc.Course.Code,
                        Credits = ayc.Course.Credits.ToString("0.0"),
                        IsElective = ayc.IsElective,
                        AcademicHistories = ayc.Course.AcademicHistories?
                        .Where(ayc => !ayc.Withdraw)
                        .OrderBy(x => x.Term.Name)
                        .ThenBy(x => x.Grade)
                        .Select(ah => new ViewModels.StudentInformation.PdfViewModels.AcademicHistoryViewModel
                        {
                            Observation = ah.Observations,
                            Grade = ah.Grade,
                            Term = ah.Term.Name,
                            IsValidated = ah.Validated,
                            Withdrawn = ah.Withdraw,
                            Approved = ah.Approved
                        }).ToList(),
                        EvaluationReports = ayc.Course.AcademicHistories.Where(ah => !ah.Withdraw).Any(ah => ah.EvaluationReportId.HasValue) ?
                        ayc.Course.AcademicHistories.Where(ah => !ah.Withdraw).Any(ah => ah.Approved) ?
                          ayc.Course.AcademicHistories
                        .Where(ah => !ah.Withdraw && ah.Approved)
                        .Where(ah => ah.EvaluationReportId.HasValue)
                        .OrderByDescending(ah => ah.Grade)
                        .Select(ah => new ViewModels.StudentInformation.PdfViewModels.EvaluationReportViewModel
                        {
                            GeneratedId = ah.EvaluationReport.Code,
                            CreatedAt = evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.CreatedAt ? ah.EvaluationReport.CreatedAt : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.ReceptionDate ? ah.EvaluationReport.ReceptionDate : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.LastGradePublished ? ah.EvaluationReport.LastGradePublishedDate : null
                            //CreatedAt = ah.EvaluationReport.ReceptionDate
                        }).FirstOrDefault()
                        :
                        ayc.Course.AcademicHistories
                        .Where(ah => !ah.Withdraw)
                        .Where(ah => ah.EvaluationReportId.HasValue)
                        .OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade)
                        .Select(ah => new ViewModels.StudentInformation.PdfViewModels.EvaluationReportViewModel
                        {
                            GeneratedId = ah.EvaluationReport.Code,
                            CreatedAt = evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.CreatedAt ? ah.EvaluationReport.CreatedAt : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.ReceptionDate ? ah.EvaluationReport.ReceptionDate : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.LastGradePublished ? ah.EvaluationReport.LastGradePublishedDate : null
                            //CreatedAt = ah.EvaluationReport.ReceptionDate
                        }).FirstOrDefault()
                        : null
                    }).OrderBy(ayc => ayc.Code)
                    .ToList()
                }).ToList()
            };
            //return View("/Areas/Academic/Views/StudentInformation/Pdf/AcademicHistory.cshtml", model);
            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/Pdf/AcademicHistory.cshtml", model);
            //var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css\pages\pdf\academichistorypdf.css");
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 8, Right = 8 },
            };
            var objectSettings = new DinkToPdf.ObjectSettings
            {
                HtmlContent = viewToString,
                WebSettings =
                {
                    DefaultEncoding = "utf-8",
                },
                PagesCount = true,
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Right = "Pág: [page]/[toPage]",
                    Left ="Fecha: "+ DateTime.UtcNow.ToDefaultTimeZone().ToString("dd 'de' MMMM, yyyy") + " - Hora: " + DateTime.UtcNow.ToDefaultTimeZone().ToString("HH:mm 'hrs'"),
                    Line = true
                },
                //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileByte = _dinkConverter.Convert(pdf);

            if (!User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF) && !User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD))
            {
                if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAB && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAH)
                    _pdfSharpService.AddWatermarkToAllPages(ref fileByte, "Solo Lectura", 130);
            }

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf");
        }

        /// <summary>
        /// Método para generar el reporte de historial académico del estudiante en formato Excel
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento Excel</returns>
        [HttpGet("informacion/{id}/situacion-academica/reporte-excel")]
        public async Task<IActionResult> CurriculumProgressExcel(Guid id)
        {
            var student = await _studentService.GetStudentWithCareerAndUser(id);

            var studentIncomeData = await _studentIncomeScoreService.GetByStudent(student.Id);

            var academicYearCourses = await _academicYearCourseService.GetWithHistoriesByCurriculumAndStudent(student.CurriculumId, student.Id);
            var academicYears = academicYearCourses
                .GroupBy(x => x.AcademicYear)
                .ToList();

            var result = await _academicSummariesService.GetAcademicPerformanceSummary(id);
            var model = new ViewModels.StudentInformation.PdfViewModels.AcademicHistoryPdfViewModel
            {
                StudentInfo = new ViewModels.StudentInformation.PdfViewModels.StudentInfoViewModel
                {
                    StudentName = student.User.FullName,
                    StudentCode = student.User.UserName,
                },
                AcademicYears = academicYears.Select(ay => new ViewModels.StudentInformation.PdfViewModels.AcademicYearViewModel
                {
                    AcademicYearNumber = ay.Key,
                    Courses = ay.Select(ayc => new ViewModels.StudentInformation.PdfViewModels.CourseViewModel
                    {
                        Name = ayc.Course.Name,
                        Code = ayc.Course.Code,
                        Credits = ayc.Course.Credits.ToString("0.0"),
                        IsElective = ayc.IsElective,
                        AcademicHistories = ayc.Course.AcademicHistories?
                        .Where(ayc => !ayc.Withdraw)
                        .OrderBy(x => x.Term.Name)
                        .Select(ah => new ViewModels.StudentInformation.PdfViewModels.AcademicHistoryViewModel
                        {
                            Observation = ah.Observations,
                            Grade = ah.Grade,
                            Term = ah.Term.Name,
                            IsValidated = ah.Validated,
                            Withdrawn = ah.Withdraw,
                            Approved = ah.Approved
                        }).ToList(),
                        EvaluationReports = ayc.Course.AcademicHistories.Where(ah => !ah.Withdraw).Any(ah => ah.EvaluationReportId.HasValue) ?
                        ayc.Course.AcademicHistories.Where(ah => !ah.Withdraw).Any(ah => ah.Approved) ?
                          ayc.Course.AcademicHistories
                        .Where(ah => !ah.Withdraw)
                        .Where(ah => ah.EvaluationReportId.HasValue)
                        .OrderByDescending(ah => ah.Grade)
                        .Select(ah => new ViewModels.StudentInformation.PdfViewModels.EvaluationReportViewModel
                        {
                            GeneratedId = ah.EvaluationReport.Code,
                            CreatedAt = ah.EvaluationReport.ReceptionDate
                        }).FirstOrDefault()
                        :
                        ayc.Course.AcademicHistories
                        .Where(ah => !ah.Withdraw)
                        .Where(ah => ah.EvaluationReportId.HasValue)
                        .OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade)
                        .Select(ah => new ViewModels.StudentInformation.PdfViewModels.EvaluationReportViewModel
                        {
                            GeneratedId = ah.EvaluationReport.Code,
                            CreatedAt = ah.EvaluationReport.ReceptionDate
                        }).FirstOrDefault()
                        : null
                    }).OrderBy(ayc => ayc.Code)
                    .ToList()
                }).ToList()
            };

            string fileName = $"Historial Académico.xlsx";

            var maxTerms = model.AcademicYears.Max(ay => ay.Courses.Count == 0 ? 0 : ay.Courses.Max(c => c.AcademicHistories.Count()));
            var maxColumns = (maxTerms * 2) + 5;

            using (var wb = new XLWorkbook())
            {
                var worksheet = wb.Worksheets.Add("Historial");
                worksheet.Cell(1, 1).Value = GeneralHelpers.GetInstitutionName().ToUpper();
                worksheet.Cell(1, 1).Style.Font.FontSize = 13;
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range(1, 1, 1, maxColumns).Merge();

                worksheet.Cell(2, 1).Value = "HISTORIAL ACADÉMICO";
                worksheet.Cell(2, 1).Style.Font.FontSize = 12;
                worksheet.Cell(2, 1).Style.Font.Bold = true;
                worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range(2, 1, 2, maxColumns).Merge();

                worksheet.Cell(3, 1).Value = $"Cod. Estudiante : {model.StudentInfo.StudentCode}";
                worksheet.Cell(3, 1).Style.Font.FontSize = 11;
                worksheet.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell(3, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range(3, 1, 3, maxColumns).Merge();

                worksheet.Cell(4, 1).Value = $"Estudiante : {model.StudentInfo.StudentName}";
                worksheet.Cell(4, 1).Style.Font.FontSize = 11;
                worksheet.Cell(4, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell(4, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range(4, 1, 4, maxColumns).Merge();

                //TABLA 

                worksheet.Cell(6, 1).Value = $"COD";
                worksheet.Cell(6, 1).Style.Font.FontSize = 11;
                worksheet.Cell(6, 1).Style.Font.Bold = true;
                worksheet.Cell(6, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(6, 1).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(6, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(6, 1).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(6, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

                worksheet.Cell(6, 2).Value = $"ASIGNATURA";
                worksheet.Cell(6, 2).Style.Font.FontSize = 11;
                worksheet.Cell(6, 2).Style.Font.Bold = true;
                worksheet.Cell(6, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(6, 2).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(6, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(6, 2).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(6, 2).Style.Fill.BackgroundColor = XLColor.LightGray;

                worksheet.Cell(6, 3).Value = $"CRED";
                worksheet.Cell(6, 3).Style.Font.FontSize = 11;
                worksheet.Cell(6, 3).Style.Font.Bold = true;
                worksheet.Cell(6, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(6, 3).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(6, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(6, 3).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(6, 3).Style.Fill.BackgroundColor = XLColor.LightGray;

                var currentColumn = 4;

                for (int i = 0; i < maxTerms; i++)
                {
                    worksheet.Cell(6, currentColumn).Value = $"NOTA";
                    worksheet.Cell(6, currentColumn).Style.Font.FontSize = 11;
                    worksheet.Cell(6, currentColumn).Style.Font.Bold = true;
                    worksheet.Cell(6, currentColumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(6, currentColumn).Style.Border.OutsideBorderColor = XLColor.Black;
                    worksheet.Cell(6, currentColumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(6, currentColumn).Style.Border.OutsideBorderColor = XLColor.Black;
                    worksheet.Cell(6, currentColumn).Style.Fill.BackgroundColor = XLColor.LightGray;

                    currentColumn++;

                    worksheet.Cell(6, currentColumn).Value = $"AÑO-PER";
                    worksheet.Cell(6, currentColumn).Style.Font.FontSize = 11;
                    worksheet.Cell(6, currentColumn).Style.Font.Bold = true;
                    worksheet.Cell(6, currentColumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(6, currentColumn).Style.Border.OutsideBorderColor = XLColor.Black;
                    worksheet.Cell(6, currentColumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(6, currentColumn).Style.Border.OutsideBorderColor = XLColor.Black;
                    worksheet.Cell(6, currentColumn).Style.Fill.BackgroundColor = XLColor.LightGray;

                    currentColumn++;
                }

                worksheet.Cell(6, currentColumn).Value = $"ACTA";
                worksheet.Cell(6, currentColumn).Style.Font.FontSize = 11;
                worksheet.Cell(6, currentColumn).Style.Font.Bold = true;
                worksheet.Cell(6, currentColumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(6, currentColumn).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(6, currentColumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(6, currentColumn).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(6, currentColumn).Style.Fill.BackgroundColor = XLColor.LightGray;

                currentColumn++;

                worksheet.Cell(6, currentColumn).Value = $"FECHA";
                worksheet.Cell(6, currentColumn).Style.Font.FontSize = 11;
                worksheet.Cell(6, currentColumn).Style.Font.Bold = true;
                worksheet.Cell(6, currentColumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(6, currentColumn).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(6, currentColumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(6, currentColumn).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(6, currentColumn).Style.Fill.BackgroundColor = XLColor.LightGray;

                var currentRow = 7;

                foreach (var item in model.AcademicYears)
                {
                    var columnsDetail = 1;

                    worksheet.Cell(currentRow, columnsDetail).Value = $"SEMESTRE : {ConstantHelpers.ACADEMIC_YEAR.TEXT[item.AcademicYearNumber].ToUpper()}";
                    worksheet.Cell(currentRow, columnsDetail).Style.Font.FontSize = 11;
                    worksheet.Cell(currentRow, columnsDetail).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                    worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                    worksheet.Range(currentRow, columnsDetail, currentRow, maxColumns).Merge();
                    worksheet.Range(currentRow, columnsDetail, currentRow, maxColumns).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Range(currentRow, columnsDetail, currentRow, maxColumns).Style.Border.OutsideBorderColor = XLColor.Black;

                    currentRow++;

                    foreach (var detail in item.Courses)
                    {
                        worksheet.Cell(currentRow, columnsDetail).Value = $"'{detail.Code}";
                        worksheet.Cell(currentRow, columnsDetail).Style.Font.FontSize = 10;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                        columnsDetail++;

                        worksheet.Cell(currentRow, columnsDetail).Value = $"'{detail.Name}";
                        worksheet.Cell(currentRow, columnsDetail).Style.Font.FontSize = 10;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;

                        columnsDetail++;

                        worksheet.Cell(currentRow, columnsDetail).Value = $"'{detail.Credits}";
                        worksheet.Cell(currentRow, columnsDetail).Style.Font.FontSize = 10;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                        columnsDetail++;

                        foreach (var terms in detail.AcademicHistories)
                        {
                            worksheet.Cell(currentRow, columnsDetail).Value = terms.IsValidated && terms.Grade == -1 ? "CV" : terms.Withdrawn ? "RET" : $"'{terms.Grade}";
                            worksheet.Cell(currentRow, columnsDetail).Style.Font.FontSize = 10;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;

                            columnsDetail++;

                            worksheet.Cell(currentRow, columnsDetail).Value = $"'{terms.Term}";
                            worksheet.Cell(currentRow, columnsDetail).Style.Font.FontSize = 10;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;

                            columnsDetail++;
                        }

                        for (int i = 0; i < maxTerms - detail.AcademicHistories.Count(); i++)
                        {
                            worksheet.Cell(currentRow, columnsDetail).Value = $"-";
                            worksheet.Cell(currentRow, columnsDetail).Style.Font.FontSize = 10;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;

                            columnsDetail++;

                            worksheet.Cell(currentRow, columnsDetail).Value = $"-";
                            worksheet.Cell(currentRow, columnsDetail).Style.Font.FontSize = 10;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;

                            columnsDetail++;
                        }

                        worksheet.Cell(currentRow, columnsDetail).Value = $"'{detail.EvaluationReports?.GeneratedId.ToString().PadLeft(6, '0')}";
                        worksheet.Cell(currentRow, columnsDetail).Style.Font.FontSize = 10;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                        columnsDetail++;

                        worksheet.Cell(currentRow, columnsDetail).Value = $"'{detail.EvaluationReports?.CreatedAt.ToLocalDateFormat()}";
                        worksheet.Cell(currentRow, columnsDetail).Style.Font.FontSize = 10;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(currentRow, columnsDetail).Style.Border.OutsideBorderColor = XLColor.Black;
                        columnsDetail++;

                        columnsDetail = 1;

                        currentRow++;
                    }
                }

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        /// <summary>
        /// Retorna la información del acta relacionada a la nota del estudiante
        /// </summary>
        /// <param name="academicHistoryId">Id del historial académico</param>
        /// <returns>Objeto con información del acta</returns>
        [HttpGet("informacion/{id}/situacion-academica/get-detalles-acta")]
        public async Task<IActionResult> GetEvaluationReport(Guid academicHistoryId)
        {
            var academicHistory = await _academicHistoryService.Get(academicHistoryId);
            var evaluationReportCode = string.Empty;
            var receptionDate = string.Empty;
            var createdAt = string.Empty;
            var lastGradePublished = string.Empty;

            if (academicHistory.EvaluationReportId.HasValue)
            {
                var evaluationReport = await _evaluationReportService.Get(academicHistory.EvaluationReportId.Value);
                evaluationReportCode = evaluationReport.Code;
                receptionDate = evaluationReport.ReceptionDate.ToLocalDateFormat();
                createdAt = evaluationReport.CreatedAt.ToLocalDateFormat();
                lastGradePublished = evaluationReport.LastGradePublishedDate.ToLocalDateTimeFormat();
            }

            var data = new
            {
                academicHistory.EvaluationReportId,
                EvaluationReportCode = evaluationReportCode,
                observation = academicHistory.Observations,
                termId = academicHistory.TermId,
                receptionDate,
                createdAt,
                lastGradePublished
            };

            return Ok(data);
        }
        /// <summary>
        /// Método para validar si existe un acta con el código ingresado registrada en el periodo académico
        /// </summary>
        /// <param name="termId">Id del periodo académico</param>
        /// <param name="code">Código del acta</param>
        /// <returns>Objeto con la información del acta</returns>
        [HttpGet("informacion/{id}/situacion-academica/validar-acta")]
        public async Task<IActionResult> ValidateEvaluationReport(Guid termId, string code)
        {
            var evaluationReports = await _evaluationReportService.GetEvaluationReportsByCode(code, termId);
            var term = await _termService.Get(termId);

            if (!evaluationReports.Any())
                return BadRequest($"No se encontraron actas con el código '{code}' que pertenezcan al periodo '{term.Name}'");

            var data = evaluationReports
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    receptionDate = x.ReceptionDate == DateTime.MinValue ? null : x.ReceptionDate.ToLocalDateFormat(),
                    createdAt = x.CreatedAt.ToLocalDateFormat(),
                    lastGradePublishedDate = x.LastGradePublishedDate.ToLocalDateTimeFormat()
                })
                .FirstOrDefault();

            return Ok(data);
        }

        /// <summary>
        /// Método para actualizar el acta relacionada a la nota del estudiante
        /// </summary>
        /// <param name="model">Modelo con la información del acta</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/situacion-academica/actualizar-acta")]
        public async Task<IActionResult> UpdateEvaluationReportOnAcademicHistory(UpdateEvaluationReportViewModel model)
        {
            var user = await _userService.GetUserByClaim(User);
            var academicHistory = await _academicHistoryService.Get(model.AcademicHistoryId);
            var term = await _termService.Get(model.TermId);

            academicHistory.EvaluationReportId = null;

            if (!string.IsNullOrEmpty(model.EvaluationReportCode))
            {
                var evaluationReports = await _evaluationReportService.GetEvaluationReportsByCode(model.EvaluationReportCode, model.TermId);

                if (!evaluationReports.Any())
                    return BadRequest($"No se encontraron actas con el código '{model.EvaluationReportCode}' que pertenezcan al periodo '{term.Name}'");

                var evaluationReport = await _evaluationReportService.Get(evaluationReports.Select(x => x.Id).FirstOrDefault());

                if (!string.IsNullOrEmpty(model.EvaluationReportReceptionDate))
                    evaluationReport.ReceptionDate = ConvertHelpers.DatepickerToUtcDateTime(model.EvaluationReportReceptionDate);

                if (!string.IsNullOrEmpty(model.EvaluationReportCreatedAt))
                    evaluationReport.CreatedAt = ConvertHelpers.DatepickerToUtcDateTime(model.EvaluationReportCreatedAt);

                if (!string.IsNullOrEmpty(model.EvaluationReportLastGradePublished))
                    evaluationReport.LastGradePublishedDate = ConvertHelpers.DatetimepickerToUtcDateTime(model.EvaluationReportLastGradePublished);

                academicHistory.EvaluationReportId = evaluationReport.Id;

                if (academicHistory.SectionId.HasValue)
                {
                    var academicHistoriesBySectionId = await _context.AcademicHistories.Where(x => x.SectionId == academicHistory.SectionId).ToListAsync();
                    academicHistoriesBySectionId.ForEach(x => x.EvaluationReportId = evaluationReport.Id);
                }

                var course = await _courseService.GetAsync(academicHistory.CourseId);
                var termAcademicHistory = await _termService.Get(academicHistory.TermId);

                var activeTerm = await _termService.GetActiveTerm();

                var observation = new StudentObservation
                {
                    Observation = $"Se asigno el acta {model.EvaluationReportCode} al curso {course.Name} del periodo {termAcademicHistory.Name}",
                    StudentId = academicHistory.StudentId,
                    UserId = user.Id,
                    Type = ConstantHelpers.OBSERVATION_TYPES.INFO_UPDATE,
                    TermId = activeTerm != null ? activeTerm.Id : (Guid?)null
                };

                if (model.File != null)
                {
                    var storage = new CloudStorageService(_storageCredentials);
                    var filePath = await storage.UploadFile(model.File.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.INTRANET_FILES,
                        Path.GetExtension(model.File.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                    observation.File = filePath;
                }

                await _studentObservationService.Insert(observation);
            }

            academicHistory.Observations = model.Observation;
            academicHistory.TermId = model.TermId;

            await _academicHistoryService.Update(academicHistory);
            return Ok();
        }

        /// <summary>
        /// Retorna la lista de cursos del plan de estudios del estudiante con las notas obtenidas
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Objeto con la lista de cursos</returns>
        [HttpGet("informacion/{id}/situacion-academica/cursos")]
        public async Task<IActionResult> GetCurriculumCourses(Guid id)
        {

            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _academicYearCourseService.GetCurriculumCoursesDatatable(sentParameters, id);

            return Ok(result);
        }
        /// <summary>
        /// Retorna la lista de cursos electivos disponibles en el plan de estudios del estudiante y las notas obtenidas
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Objeto con la lista de cursos electivos</returns>
        [HttpGet("informacion/{id}/situacion-academica/electivos")]
        public async Task<IActionResult> GetCurriculumElectives(Guid id)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _academicYearCourseService.GetCurriculumElectivesDatatable(sentParameters, id);

            return Ok(result);
        }
        /// <summary>
        /// Retorna la información de cursos convalidados del plan anterior relacionados al curso seleccionado
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="course">Id del curso</param>
        /// <returns>Texto con los nombres de los cursos convalidados</returns>
        [HttpGet("informacion/{id}/situacion-academica/equivalencia")]
        public async Task<IActionResult> GetCurriculumCourseEquivalences(Guid id, Guid course)
        {
            var result = await _academicYearCourseService.GetCourseEquivalences(id, course);
            return Ok(result);
        }

        #region - CONSTANCIAS - 
        /// <summary>
        /// Método para generar la constancia de estudios del estudiante en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/certificado-estudios")]
        public async Task<IActionResult> StudyRecord(Guid id)
        {
            var term = await GetActiveTerm();
            var result = await _studentService.GetStudyRecord(id);
            var model = _mapper.Map<StudyRecordViewModel>(result);
            model.PathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png");
            model.Term = term != null ? term.Name : "...";
            model.Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES"));
            model.Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation();
            model.University = GeneralHelpers.GetInstitutionName();

            model.PageCode = await GetPageCode(ConstantHelpers.RECORDS.STUDYRECORD);

            await SaveRecordHistory(ConstantHelpers.RECORDS.STUDYRECORD, model.Student, "", id);

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 10,
                },
            };

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/StudyRecord.cshtml", model);

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

            if (!User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF) && !User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD))
            {
                if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAB && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAH)
                    _pdfSharpService.AddWatermarkToAllPages(ref fileByte, "Solo Lectura", 130);
            }

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileByte, "application/pdf", $"{model.Code} - Certificado de Estudios.pdf");
        }

        /// <summary>
        /// Método para generar la constancia de estudios del estudiante v2 en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/v2/certificado-estudios")]
        public async Task<IActionResult> StudyRecordV2(Guid id)
        {
            var result = await _certificateOfStudiesGeneratorService.GetCertificateOfStudiesPDF(id, null);
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(result.Pdf, "application/pdf", $"{result.PdfName}.pdf");


            var universityName = GeneralHelpers.GetInstitutionName();
            var model = await _studentService.GetStudntCertificate(id, universityName);
            var student = _mapper.Map<HeadBoardCertificateViewModel>(model);

            var viewToString = "";
            var studentCode = "";
            var objectsSettings = new List<DinkToPdf.ObjectSettings>();



            var modelLstCertificate = await _academicHistoryService.GetListCertificateByStudentAndCurriculum(student.IdStudent, student.CurriculumId);
            var lstCertificate = modelLstCertificate
                .Select(x => new CertificateViewModel
                {
                    TermName = x.TermName,
                    TermMinGrade = x.TermMinGrade,
                    Grade = x.Grade,
                    Credits = x.Credits,
                    AcademicNumber = Convert.ToInt32(x.AcademicNumber),
                    AcademicYear = x.AcademicYear,
                    CourseCode = x.CourseCode,
                    Observations = x.Observations,
                    CourseName = x.CourseName
                }).ToList();

            var values = await _configurationService.GetDataDictionary();

            var universityinfo = new UniversityInformation
            {
                Address = GetConfigurationValue(values, ConstantHelpers.Configuration.General.INSTITUTION_ADDRESS),
                PhoneNumber = GetConfigurationValue(values, ConstantHelpers.Configuration.General.INSTITUTION_PHONENUMBER),
                Campus = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_MAIN_CAMPUS),
                HeaderOffice = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_MAIN_OFFICE),
                Office = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_OFFICE),
                Sender = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_SENDER_COORDINATOR),
                WebSite = GetConfigurationValue(values, ConstantHelpers.Configuration.General.INSTITUTION_WEBSITE)
            };

            var user = await _userService.GetUserByClaim(User);

            CertificateCompleteViewModel certificateComplete = new CertificateCompleteViewModel
            {
                HeaderBoard = student,
                Certificate = lstCertificate,
                ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{@GeneralHelpers.GetTheme()}/logo-report.png"),
                Today = DateTime.UtcNow.ToShortDateString(),
                YearOfStudies = Math.Truncate((decimal)lstCertificate.GroupBy(x => new { x.TermName }).Count() / 2),
                University = universityinfo,
                User = user.UserName
            };

            viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/Pdf/StudentCertificate.cshtml", certificateComplete);
            //return View("/Areas/Academic/Views/StudentInformation/Pdf/StudentCertificate.cshtml", certificateComplete);
            studentCode = student.UserName;
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
            objectsSettings.Add(objectSettings);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,

                Margins = new MarginSettings { Top = 10, Bottom = 18, Left = 10, Right = 10 }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings
            };
            pdf.Objects.AddRange(objectsSettings);

            var fileByte = _dinkConverter.Convert(pdf);

            if (!User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF) && !User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD))
            {
                if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAB && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAH)
                    _pdfSharpService.AddWatermarkToAllPages(ref fileByte, "Solo Lectura", 130);
            }

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileByte, "application/pdf", $"certificado_de_estudios_{student.UserName}.pdf");
        }
        /// <summary>
        /// Método para generar el formato PDF del plan de estudios del estudiante y sus notas
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/plan-estudios")]
        public async Task<IActionResult> StudyCurriculum(Guid id)
        {
            var universityName = GeneralHelpers.GetInstitutionName();
            var model = await _studentService.GetStudntCertificate(id, universityName);
            var student = _mapper.Map<HeadBoardCertificateViewModel>(model);

            var viewToString = "";
            var studentCode = "";
            var objectsSettings = new List<DinkToPdf.ObjectSettings>();

            var modelLstCertificate = await _academicHistoryService.GetListCertificateByStudentAndCurriculum(student.IdStudent, student.CurriculumId);
            var lstCertificate = _mapper.Map<List<CertificateViewModel>>(modelLstCertificate);

            var certificateComplete = new CertificateCompleteViewModel
            {
                HeaderBoard = student,
                Certificate = lstCertificate,
                ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{@GeneralHelpers.GetTheme()}/logo-report.png"),
                Today = DateTime.UtcNow.ToShortDateString(),
                YearOfStudies = Math.Truncate((decimal)lstCertificate.GroupBy(x => new { x.TermName }).Count() / 2)
            };

            viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/Pdf/StudentCurriculum.cshtml", certificateComplete);
            studentCode = student.UserName;
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
            objectsSettings.Add(objectSettings);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,

                Margins = new MarginSettings { Top = 10, Bottom = 18, Left = 10, Right = 10 }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings
            };
            pdf.Objects.AddRange(objectsSettings);

            var fileByte = _dinkConverter.Convert(pdf);

            if (!User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF) && !User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD))
            {
                if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAB && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAH)
                    _pdfSharpService.AddWatermarkToAllPages(ref fileByte, "Solo Lectura", 130);
            }

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileByte, "application/pdf", $"plan_de_estudios_{student.UserName}.pdf");
        }

        /// <summary>
        /// Método para generar la constancia de ingreso del estudiante en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/constancia-ingreso")]
        public async Task<IActionResult> GetProofOfIncome(Guid id)
        {
            var student = await _studentService.GetStudentProofOfInCome(id);

            var postulant = await _postulantService.GetPostulantByDniAndTerm(student.User.Dni, student.AdmissionTermId);
            var studentsCount = await _postulantService.GetStudentCounttByTermAndCareer(student.AdmissionTermId, student.CareerId);

            var activeterm = await GetPageCode(ConstantHelpers.RECORDS.PROOFONINCOME);

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
                PageCode = activeterm
            };

            await SaveRecordHistory(ConstantHelpers.RECORDS.PROOFONINCOME, student.User.FullName, student.User.Dni, id);
            //dni term
            /***/
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 40,
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

            //return View(@"/Areas/Academic/Views/StudentInformation/Pdf/ProofOfIncome.cshtml", proof);
            return File(fileContents, ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }
        /// <summary>
        /// Método para generar la constancia de matrícula del estudiante en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/constancia-matricula")]
        public async Task<IActionResult> GetRecordEnrollment(Guid id)
        {

            var term = await GetActiveTerm();

            if (term == null)
            {
                return BadRequest("No existe periodo activo");
            }
            var student = await _studentService.GetStudentRecordEnrollment(id);

            var activeterm = await GetPageCode(ConstantHelpers.RECORDS.ENROLLMENT);
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
                StartDate = student.AdmissionTerm.StartDate.ToString("dd/MM/yyyy"),
                PageCode = activeterm
            };
            await SaveRecordHistory(ConstantHelpers.RECORDS.ENROLLMENT, student.User.FullName, student.User.Dni, id);
            /**********/
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 40,
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

            //return View(@"/Views/Test/Pdf/RecordOfEnrollment.cshtml", record);
            return File(fileContents, ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }
        /// <summary>
        /// Método para generar la constancia de estudios regulares del estudiante en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/constancia-estudios-regulares")]
        public async Task<IActionResult> GetRecordOfRegularStudies(Guid id)
        {
            var student = await _studentService.GetStudentRecordEnrollment(id);

            var activeterm = await GetPageCode(ConstantHelpers.RECORDS.REGULARSTUDIES);
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
                Semester = student.CurrentAcademicYear.ToString("D2"),
                StartDate = student.AdmissionTerm.StartDate.ToString("dd/MM/yyyy"),
                StudentCondition = ConstantHelpers.Student.States.VALUES[student.Status],
                PageCode = activeterm
            };
            await SaveRecordHistory(ConstantHelpers.RECORDS.REGULARSTUDIES, student.User.FullName, student.User.Dni, id);
            /************************/
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 40,
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

            //return View(@"/Views/Test/Pdf/RecordOfRegularStudies.cshtml", record);
            return File(fileContents, ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }
        /// <summary>
        /// Método para generar la constancia de egreso del estudiante en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/constancia-egreso")]
        public async Task<IActionResult> GetRecordOfEgress(Guid id)
        {
            var student = await _studentService.GetStudentRecordEnrollment(id);

            var activeterm = await GetPageCode(ConstantHelpers.RECORDS.EGRESS);

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
                PageCode = activeterm
            };
            await SaveRecordHistory(ConstantHelpers.RECORDS.EGRESS, student.User.FullName, student.User.Dni, id);
            /**************/
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 40,
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

            //return View(@"/Views/Test/Pdf/RecordOfEgress.cshtml", record);
            return File(fileContents, ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }

        /// <summary>
        /// Método para generar el reporte de cuadros de mérito del estudiante en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/cuadro-meritos")]
        public async Task<IActionResult> MeritChart(Guid id)
        {
            var student = await _studentService.GetStudentWithCareerAcademicUser(id);

            var result = await _academicSummariesService.GetDetailMeritChart(id);
            var details = _mapper.Map<List<MeritChartDetailViewModel>>(result);
            var average = await _academicSummariesService.GetAverage(student.GraduationTermId);

            var activeterm = await GetPageCode(ConstantHelpers.RECORDS.MERITCHART);
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
                IsGraduated = student.Status == ConstantHelpers.Student.States.GRADUATED,
                StudentGender = student.User.Sex,
                CurrentSemester = (student.CurrentAcademicYear <= 10) ? ConstantHelpers.ACADEMIC_YEAR.TEXT[student.CurrentAcademicYear] : student.CurrentAcademicYear + "°",
                PageCode = activeterm
            };
            await SaveRecordHistory(ConstantHelpers.RECORDS.MERITCHART, student.User.FullName, student.User.Dni, id);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 40,
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

            //return View(@"/Views/Test/Pdf/MeritChart.cshtml",model);
            return File(fileContents, ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }

        /// <summary>
        /// Método para generar la constancia de quinto superior del estudiante en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/quinto-superior")]
        public async Task<IActionResult> UpperFifth(Guid id)
        {
            var student = await _studentService.GetStudentWithCareerAcademicUser(id);
            //if (student.Status != ConstantHelpers.Student.States.BACHELOR || student.DegreeDate is null || student.GraduationTermId is null)
            //    return BadRequest("El alumno seleccionado aún no se ha graduado.");

            var current = await _academicSummariesService.GetCurrent(id, student.GraduationTermId);

            var result = await _academicSummariesService.GetDetailUpperFith(id);

            var details = _mapper.Map<List<UpperFifthDetailsViewModel>>(result);

            var activeterm = await GetPageCode(ConstantHelpers.RECORDS.UPPERFIFTH);
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
                IsGraduated = student.Status == ConstantHelpers.Student.States.GRADUATED,
                CurrentSemester = (student.CurrentAcademicYear <= 10) ? ConstantHelpers.ACADEMIC_YEAR.TEXT[student.CurrentAcademicYear] : student.CurrentAcademicYear + "°",
                PageCode = activeterm
            };
            await SaveRecordHistory(ConstantHelpers.RECORDS.UPPERFIFTH, student.User.FullName, student.User.Dni, id);

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
                    Top = 40,
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

            //return View(@"/Views/Test/Pdf/MeritChart.cshtml",model);
            return File(fileContents, ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }

        /// <summary>
        /// Método para generar la constancia de tercio superior del estudiante en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/tercio-superior")]
        public async Task<IActionResult> UpperThird(Guid id)
        {
            var student = await _studentService.GetStudentWithCareerAcademicUser(id);
            //if (student.Status != ConstantHelpers.Student.States.BACHELOR || student.DegreeDate is null || student.GraduationTermId is null)
            //    return BadRequest("El alumno seleccionado aún no se ha graduado.");

            var current = await _academicSummariesService.GetCurrent(id, student.GraduationTermId);

            var result = await _academicSummariesService.GetDetailUpperThird(id);

            var details = _mapper.Map<List<UpperFifthDetailsViewModel>>(result);

            var activeterm = await GetPageCode(ConstantHelpers.RECORDS.UPPERTHIRD);
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
                IsGraduated = student.Status == ConstantHelpers.Student.States.GRADUATED,
                CurrentSemester = (student.CurrentAcademicYear <= 10) ? ConstantHelpers.ACADEMIC_YEAR.TEXT[student.CurrentAcademicYear] : student.CurrentAcademicYear + "°",
                PageCode = activeterm
            };

            await SaveRecordHistory(ConstantHelpers.RECORDS.UPPERTHIRD, student.User.FullName, student.User.Dni, id);

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
                    Top = 40,
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

            //return View(@"/Views/Test/Pdf/MeritChart.cshtml",model);
            return File(fileContents, ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }

        /// <summary>
        /// Método para generar reporte de grado de bachiller del estudiante en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/grado-bachiller")]
        public async Task<IActionResult> BachelorsDegreeInfo(Guid id)
        {
            var student = await _studentService.GetStudentWithCareerAdmissionAcademicUser(id);

            //if (student.Status != ConstantHelpers.Student.States.BACHELOR || student.DegreeDate is null || student.GraduationTermId is null)
            //    return BadRequest("El alumno seleccionado aún no se ha graduado.")

            var average = await _academicSummariesService.GetAverageBachelorsDegree(id, student.GraduationTermId);

            var bachelor = await _configurationService.GetConfigurationByGRADTupaTypeBachelor();


            var academicSemesters = await _academicSummariesService.GetacademicSemestersCount(id);

            var activeterm = await GetPageCode(ConstantHelpers.RECORDS.BACHELOR);

            var model = new BachelorsViewModel
            {
                Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                Student = student.User.FullName.ToUpper(),
                School = student.Career.Faculty.Name.ToUpper(),
                Faculty = student.Career.Name.ToUpper(),
                Specialty = student.AcademicProgram is null ? "ESPECIALIDAD" : student.AcademicProgram.Name,
                StartYear = student.AdmissionTerm.Name,
                EndYear = student.GraduationTerm is null ? "-" : student.GraduationTerm.Name,
                RegistrationNumber = student.User.UserName,
                Average = average,
                AcademicSemesters = academicSemesters,
                Bachelorship = Convert.ToByte(bachelor) == ConstantHelpers.Configuration.BachelorTypeConfiguration.TUPA.AUTOMATIC ? "AUTOMATICO" : "POR SOLICITUD",
                StudentGender = student.User.Sex,
                PageCode = activeterm,
                HeaderText = _reportSettings.BachelorsDegreeInfoHeaderText
            };
            await SaveRecordHistory(ConstantHelpers.RECORDS.BACHELOR, student.User.FullName, student.User.Dni, id);

            //var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM).FirstOrDefaultAsync();

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                    Top = 40,
                },
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/BachelorsDegree.cshtml", model);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/bachelorsDegree.css");
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
            var fileDownloadName = HttpUtility.UrlEncode($"Reporte-de-grado-bachiller.pdf");
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            //return View(@"/Views/Test/Pdf/BachelorsDegree.cshtml");
            return File(fileContents, ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }


        /// <summary>
        /// Método para generar el reporte de notas para titulación del estudiante en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/record-academico-titulacion")]
        public async Task<IActionResult> AcademicRecord(Guid id)
        {
            var student = await _studentService.GetStudentWithCareerAdmissionAcademicUser(id);
            //if (student.Status != ConstantHelpers.Student.States.BACHELOR || student.DegreeDate is null || student.GraduationTermId is null)
            //    return BadRequest("El alumno seleccionado aún no se ha graduado.");

            var average = await _academicSummariesService.GetAverageBachelorsDegree(id, student.GraduationTermId);

            var coursesDisapproved = await _academicHistoryService.GetCoursesDisapprovedByStudentId(id);
            var coursesRecovered = await _academicHistoryService.GetCoursesRecoveredByStudentId(id);
            var activeterm = await GetPageCode(ConstantHelpers.RECORDS.ACADEMICRECORD);

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
                PageCode = activeterm
            };

            await SaveRecordHistory(ConstantHelpers.RECORDS.ACADEMICRECORD, student.User.FullName, student.User.Dni, id);

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
            return File(fileContents, ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }

        /// <summary>
        /// Método para generar la constancia de rendimiento académico del estudiante en formato PDF
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("informacion/{id}/resumen-rendimeinto")]
        public async Task<IActionResult> AcademicPerformanceSummary(Guid id)
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

            //var average = await _academicSummariesService.GetAverage(student.GraduationTermId);
            var activeterm = await GetPageCode(ConstantHelpers.RECORDS.ACADEMICPERFORMANCESUMMARY);

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
                CurrentSemester = (student.CurrentAcademicYear <= 10) ? ConstantHelpers.ACADEMIC_YEAR.TEXT[student.CurrentAcademicYear] : student.CurrentAcademicYear + "°",
                PageCode = activeterm
            };
            await SaveRecordHistory(ConstantHelpers.RECORDS.ACADEMICPERFORMANCESUMMARY, student.User.FullName, student.User.Dni, id);

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

            //return View(@"/Areas/Academic/Views/StudentInformation/Pdf/AcademicPerformanceSummary.cshtml", model);
            return File(fileContents, ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }
        /// <summary>
        /// Método privado que retorna el último número generado del tipo de acta a imprimir
        /// </summary>
        /// <param name="type">Tipo de acta</param>
        /// <returns>Texto con el número y año</returns>
        private async Task<string> GetPageCode(int type)
        {
            var term = await GetActiveTerm();
            var currentNumber = await GetRecordNumber(type, term.Year);
            return $"{currentNumber.ToString().PadLeft(5, '0')}-{term.Year}";
        }
        /// <summary>
        /// Método privado que retorna el siguiente número de acta a usar en la generación de actas
        /// </summary>
        /// <param name="type">Tipo de acta</param>
        /// <param name="year">Año</param>
        /// <returns>Número de acta</returns>
        private async Task<int> GetRecordNumber(int type, int year)
        {
            return (await _recordHistoryService.GetLatestRecordNumberByType(type, year)) + 1;
        }
        /// <summary>
        /// Método privado para registrar un nuevo número de acta generada
        /// </summary>
        /// <param name="recordType">Tipo de acta</param>
        /// <param name="fullName">Nombre completo</param>
        /// <param name="dni">Documento de identidad</param>
        /// <param name="studentId">Id del estudiante</param>
        private async Task SaveRecordHistory(int recordType, string fullName, string dni, Guid studentId)
        {
            try
            {
                var term = await GetActiveTerm();
                await _recordHistoryService.Insert(new RecordHistory
                {
                    Type = recordType,
                    Date = DateTime.UtcNow,
                    Number = await GetRecordNumber(recordType, term.Year),
                    StudentId = studentId
                });
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion

        /// <summary>
        /// Retorna la vista parcial de notas por periodo académico
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/{id}/boleta-notas")]
        public async Task<IActionResult> _ReportCard(Guid id)
        {
            var model = await _studentSectionService.GetTermSelectListByStudent(id);
            return PartialView(model);
        }
        /// <summary>
        /// Retorna la información general del periodo indicado a mostrar en la vista de notas por periodo
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="termid">Id del periodo académico</param>
        /// <returns>Objeto con información del periodo matriculado</returns>
        [HttpGet("informacion/{id}/boleta-notas/{termid}/cabecera")]
        public async Task<IActionResult> GetReportCardHeader(Guid id, Guid termid)
        {
            var result = await _studentSectionService.GetReportCardHeader(id, termid);
            return Ok(result);
        }
        /// <summary>
        /// Retorna la lista de cursos matriculados en el periodo y sus notas
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="termid">Id del periodo académico</param>
        /// <returns>Objeto con la lista de cursos</returns>
        [HttpGet("informacion/{id}/boleta-notas/{termid}/cursos")]
        public async Task<IActionResult> GetCoursesReportCard(Guid id, Guid termid)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentSectionService.GetCoursesReportCardDatatable(sentParameters, id, termid);
            return Ok(result);
        }

        [HttpGet("informacion/{id}/boleta-notas/detalle-notas/{studentSectionId}")]
        public async Task<IActionResult> GetGradesByStudentSectionReportCard(Guid studentSectionId)
        {
            //var data = await _studentSectionService.GetGrades
            var data = await _studentSectionService.GetStudentGradesTemplate(studentSectionId);
            return Ok(data);
        }

        /// <summary>
        /// Vista parcial para la solicitud de trámites del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/{id}/solicitud-tramites")]
        public IActionResult _ProcedureRequest(Guid id)
        {
            return PartialView(id);
        }
        /// <summary>
        /// Retorna la lista de cursos pendientes del plan de estudios del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Objeto con la lista de cursos</returns>
        [HttpGet("informacion/{id}/cursos-pendientes")]
        public async Task<IActionResult> _PendingCourses(Guid id)
        {
            var student = await _studentService.Get(id);

            var courses = await _academicYearCourseService.GetPendingCoursesStudent(id, student.CurriculumId);

            var result = new
            {
                items = courses
            };

            return Ok(result);
        }
        /// <summary>
        /// Método para registrar una nueva reserva de matrícula del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="model">Modelo con los datos de la reserva</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/solicitud-tramites/reserva")]
        public async Task<IActionResult> ReservationRequest(Guid id, ReservationRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Debe completar todos los campos");
            }

            var fileUrl = string.Empty;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                fileUrl = await storage.UploadFile(model.File.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.ENROLLMENT_RESERVATION,
                    Path.GetExtension(model.File.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            var result = await _studentService.StudentReservationRequest(User, id, model.Receipt, fileUrl, model.Observation);

            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok();
        }
        /// <summary>
        /// Método para habilitar una matrícula extemporánea al estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="model">Modelo con información del pago de la matrícula</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/solicitud-tramites/matricula-extemporanea")]
        public async Task<IActionResult> ExtemporaneousEnrollment(Guid id, ExtemporaneousViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Debe completar todos los campos");
            }

            var student = await _studentService.Get(id);
            bool exist = await _paymentService.ExistsAnotherExtemporaneousPaymentForUser(student.UserId);
            if (exist)
            {
                return BadRequest("Ya existe un pago de matrícula extemporánea en este periodo para este estudiante.");
            }

            /*****/
            var conceptId = Guid.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.EXTEMPORANEOUS_ENROLLMENT_SURCHARGE_PROCEDURE));

            var enrollmentConcepts = await _enrollmentConceptService.GetByTypeIncludeConcept(ConstantHelpers.EnrollmentConcept.Type.EXTEMPORANEOUS_ENROLLMENT_CONCEPT);
            var enrollmentConcept = enrollmentConcepts
                .OrderByDescending(x => x.CareerId)
                .ThenByDescending(x => x.AdmissionTypeId)
                .FirstOrDefault(x => (!x.CareerId.HasValue || x.CareerId == student.CareerId)
                  && (!x.AdmissionTypeId.HasValue || x.AdmissionTypeId == student.AdmissionTypeId));
            if (enrollmentConcept != null) conceptId = enrollmentConcept.ConceptId;

            var concept = await _conceptService.GetConcept(conceptId);
            var total = concept.Amount;
            var subtotal = total;
            var igvAmount = 0.00M;

            if (concept.IsTaxed)
            {
                subtotal = total / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                igvAmount = total - subtotal;
            }

            var payment = new Payment
            {
                Description = concept.Description,
                EntityId = concept.Id,
                Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT,
                UserId = student.UserId,
                SubTotal = subtotal,
                IgvAmount = igvAmount,
                Discount = 0.00M,
                Total = total,
                ConceptId = concept.Id,
                Status = CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PAID,
                PaymentDate = DateTime.UtcNow,
                Receipt = model.Receipt,
            };

            await _paymentService.Insert(payment);

            return Ok();
        }
        /// <summary>
        /// Retorna la lista de pagos del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Objeto con la lista de pagos</returns>
        [HttpGet("informacion/{id}/solicitud-tramites/getpaymenthistory")]
        public async Task<IActionResult> GetPaymentHistoryByTerm(Guid id)
        {
            var user = await _userService.GetUserByStudent(id);
            var parameters = _dataTablesService.GetSentParameters();
            var payments = await _paymentService.GetPaymentByUserDatatable(parameters, user.Id);
            return Ok(payments);
        }
        /// <summary>
        /// Método para matricular al estudiante en un curso dirigido en el periodo actual
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="model">Modelo con información de la matrícula</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/solicitud-tramites/curso-dirigido")]
        public async Task<IActionResult> DirectedCourseRequest(Guid id, DirectedCourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Debe completar todos los campos");
            }

            var term = await _termService.GetActiveTerm();

            var courseTerm = await _courseTermService.GetFirstByCourseAndeTermId(model.CourseId, term.Id);
            if (courseTerm == null)
            {
                courseTerm = new CourseTerm
                {
                    CourseId = model.CourseId,
                    TermId = term.Id,
                    //CoordinatorId = model.TeacherId
                };
                await _courseTermService.InsertAsync(courseTerm);
            }

            //verificar si este estudiante esta en el curso dirigido
            var activeRequest = await _studentSectionService.GetByStudentAndCourseTerm(model.StudentId, courseTerm.Id);
            if (activeRequest != null) return BadRequest("Cuenta con una solicitud pendiente del curso dirigido");

            //verificar si ha solicitado mas de 2 vecs
            var attempts = await _studentSectionService.CountDirectedCourseAttempts(model.StudentId, model.CourseId);
            if (attempts >= 2) return BadRequest("Ya solicito el curso dirigido 2 veces o más");

            var userId = _userManager.GetUserId(User);
            var student = await _studentService.Get(id);

            var enrollmentTurn = await _enrollmentTurnService.GetByStudentIdAndTerm(student.Id, term.Id);
            if (enrollmentTurn != null) enrollmentTurn.IsConfirmed = true;

            var observation = new StudentObservation
            {
                Observation = $"Matricula en curso dirigido con resolución {model.Resolution}",
                StudentId = id,
                Type = ConstantHelpers.OBSERVATION_TYPES.OBSERVATION,
                UserId = userId,
                TermId = term != null ? term.Id : (Guid?)null
            };
            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                observation.File = await storage.UploadFile(model.File.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.INTRANET_FILES,
                    Path.GetExtension(model.File.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            await _studentObservationService.Insert(observation);

            var sections = await _sectionService.GetAllByCourseAndTerm(model.CourseId, term.Id);

            var directedCourseSection = sections.FirstOrDefault(x => x.IsDirectedCourse);
            if (directedCourseSection == null)
            {
                directedCourseSection = new Section
                {
                    CourseTermId = courseTerm.Id,
                    Code = "DIRIGIDO",
                    IsDirectedCourse = true,
                    Vacancies = 99
                };

                await _sectionService.InsertAsync(directedCourseSection);
            }

            var studentSection = new StudentSection
            {
                StudentId = student.Id,
                SectionId = directedCourseSection.Id
            };

            await _studentSectionService.Insert(studentSection);
            await _paymentService.InsertDirectedCourseStudentPayment(studentSection.Id, student.UserId);

            return Ok();
        }
        /// <summary>
        /// Método para registrar un certificado de estudios del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="model">Modelo con información del certificado</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/solicitud-tramites/certificado-curso")]
        public async Task<IActionResult> CourseCertificateRequest(Guid id, StudentCourseCertificateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Debe completar todos los campos");
            }

            var studentCertificate = new StudentCourseCertificate
            {
                StudentId = id,
                CourseCertificateId = model.CertificateId
            };

            try
            {
                await _studentCourseCertificateService.Insert(studentCertificate);
            }
            catch
            {
                return BadRequest("Ya posee el certificado seleccionado");
            }

            var certificate = await _courseCertificateService.Get(model.CertificateId);
            var userId = _userManager.GetUserId(User);
            var term = await _termService.GetActiveTerm();

            var observation = new StudentObservation
            {
                Observation = $"Validación de Certificado del Curso {certificate.Name}",
                StudentId = id,
                Type = ConstantHelpers.OBSERVATION_TYPES.OBSERVATION,
                UserId = userId,
                TermId = term != null ? term.Id : (Guid?)null
            };

            //if (model.File != null)
            //{
            //    var storage = new CloudStorageService(_storageCredentials);

            //    observation.File = await storage.UploadFile(model.File.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.INTRANET_FILES,
            //        Path.GetExtension(model.File.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            //}

            await _studentObservationService.Insert(observation);

            return Ok();
        }
        /// <summary>
        /// Método para descargar el archivo adjunto al curso dirigido
        /// </summary>
        /// <param name="id">Id del curso dirigido</param>
        /// <returns>Archivo adjunto</returns>
        [HttpGet("informacion/curso-dirigido/descarga/{id}")]
        public async Task TestDowload2(Guid id)
        {
            var course = await _directedCourseService.Get(id);
            var storage = new CloudStorageService(_storageCredentials);

            using (var mem = new MemoryStream())
            {
                await storage.TryDownload(mem, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.PUBLIC_INFORMATION, course.File);

                var fileName = "curso-dirigido" + Path.GetExtension(course.File);
                HttpContext.Response.Headers["Content-Disposition"] = $"attachment;filename=\"{fileName}\"";
                mem.Position = 0;
                await mem.CopyToAsync(HttpContext.Response.Body);
            }
        }
        /// <summary>
        /// Método para eliminar el archivo adjunto al curso dirigido
        /// </summary>
        /// <param name="id">Id del curso dirigido</param>
        [HttpGet("informacion/curso-dirigido/eliminar/{id}")]
        public async Task TestDelete(Guid id)
        {
            var course = await _directedCourseService.Get(id);
            var storage = new CloudStorageService(_storageCredentials);
            await storage.TryDelete(course.File, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.PUBLIC_INFORMATION);
        }
        /// <summary>
        /// Método para actualizar la escuela profesional del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="model">Modelo con la nueva escuela</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/solicitud-tramites/transferencia")]
        public async Task<IActionResult> TransferRequest(Guid id, TransferRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Debe completar todos los campos");
            }

            var result = await _studentService.StudentCareerTransferRequest(User, id, model.CareerId, model.CurriculumId);

            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok();
        }

        [HttpPost("informacion/{id}/solicitud-tramites/cambio-plan")]
        public async Task<IActionResult> ChangeCurriculum(Guid id, ChangeCurriculumViewModel model)
        {
            var student = await _studentService.Get(id);
            var term = await _termService.GetActiveTerm();
            var curriculum = await _curriculumService.Get(model.CurriculumId);
            var academicPrograms = await _academicProgramService.GetAcademicProgramsByCurriculumId(curriculum.Id);
            var userId = _userManager.GetUserId(User);

            if (academicPrograms.Count() > 2 && !model.AcademicProgramId.HasValue)
                return BadRequest("Es necesario seleccionar el programa académico.");

            if (academicPrograms.Count() == 1)
                model.AcademicProgramId = academicPrograms.Select(y => y.Id).FirstOrDefault();

            student.CurriculumId = model.CurriculumId;
            student.AcademicProgramId = model.AcademicProgramId;

            var observation = new StudentObservation
            {
                Observation = model.Reason,
                StudentId = student.Id,
                Type = ConstantHelpers.OBSERVATION_TYPES.CHANGE_CURRICULUM,
                UserId = userId,
                TermId = term != null ? term.Id : (Guid?)null
            };

            await _studentObservationService.Insert(observation);
            await _studentService.Update(student);
            return Ok();
        }

        [HttpPost("informacion/{id}/solicitud-tramites/cambio-sede")]
        public async Task<IActionResult> ChangeCampus(Guid id, ChangeCampusViewModel model)
        {
            var student = await _studentService.Get(id);

            if (student.CampusId == model.CampusId)
                return BadRequest("El estudiante ya se encuentra asignado a la sede seleccionada.");

            var term = await _termService.GetActiveTerm();
            var newCampus = await _campusService.Get(model.CampusId);
            var userId = _userManager.GetUserId(User);

            var observationText = $"Sede asignada - {newCampus.Name.ToUpper()}";

            if (student.CampusId.HasValue)
            {
                var oldCampus = await _campusService.Get(student.CampusId.Value);
                observationText = $"Cambio de sede - de {oldCampus.Name.ToUpper()} a {newCampus.Name.ToUpper()}";
            }

            var observation = new StudentObservation
            {
                Observation = observationText,
                StudentId = student.Id,
                Type = ConstantHelpers.OBSERVATION_TYPES.CHANGE_CAMPUS,
                UserId = userId,
                TermId = term != null ? term.Id : (Guid?)null
            };

            student.CampusId = model.CampusId;

            await _studentObservationService.Insert(observation);
            return Ok();
        }

        /// <summary>
        /// Retorna la lista de cursos matriculados en el periodo actual en formato para elementos Select2
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Objeto con la lista de cursos matriculados</returns>
        [HttpGet("informacion/{id}/cursos-matriculados")]
        public async Task<IActionResult> GetStudentCourses(Guid id)
        {
            var term = await _termService.GetActiveTerm();
            if (term == null)
                term = new Term();
            var result = await _studentSectionService.GetCoursesSelect2ClientSide(id, term.Id, true);
            return Ok(result);
        }
        /// <summary>
        /// Método para realizar el retiro de un cursos del periodo actual
        /// </summary>
        /// <param name="CourseId">Id del curso a desmatricular</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/solicitud-tramites/retiro-curso")]
        public async Task<IActionResult> CourseWithdrawal(Guid CourseId)
        {
            var result = await _studentService.StudentCourseWithdrawalRequest(User, CourseId);

            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok();
        }
        /// <summary>
        /// Método para retirar al estudiante del periodo académico actual. Se marcarán todo los cursos como retirados
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/solicitud-tramites/retiro-ciclo")]
        public async Task<IActionResult> AcademicYearWithdrawal(Guid id, IFormFile file)
        {
            var result = await _studentService.StudentAcademicYearWithdrawalRequest(User, id);

            if (!result.Succeeded)
                return BadRequest(result.Message);

            var storage = new CloudStorageService(_storageCredentials);
            var term = await _termService.GetActiveTerm();

            var entity = new StudentObservation
            {
                StudentId = id,
                Observation = $"Retiro de ciclo - Periodo {term.Name}",
                UserId = _userManager.GetUserId(User),
                Type = ConstantHelpers.OBSERVATION_TYPES.WITHDRAWN,
                TermId = term != null ? term.Id : (Guid?)null
            };

            if (file != null)
            {
                entity.File = await storage.UploadFile(file.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.RESOLUTIONS,
                    Path.GetExtension(file.FileName), ConstantHelpers.FileStorage.SystemFolder.ENROLLMENT);
            }

            await _studentObservationService.Insert(entity);

            return Ok();
        }
        /// <summary>
        /// Método para realizar la expulsión del estudiante permanentemente
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="model">Modelo con la información del retiro</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/solicitud-tramites/expulsar-alumno")]
        public async Task<IActionResult> ExpelStudent(Guid id, ReasonViewModel model)
        {
            var term = await _termService.GetActiveTerm();
            if (term != null)
            {
                var studentSections = await _studentSectionService.GetAll(id, term.Id);
                foreach (var item in studentSections)
                {
                    item.Status = ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN;
                }
                await _studentSectionService.UpdateRange(studentSections);
            }

            var student = await _studentService.Get(id);
            student.Status = ConstantHelpers.Student.States.EXPELLED;
            await _studentService.Update(student);

            var userId = _userManager.GetUserId(User);
            var observation = new StudentObservation
            {
                Observation = model.Reason,
                StudentId = id,
                Type = ConstantHelpers.OBSERVATION_TYPES.EXPULSION,
                UserId = userId,
                TermId = term != null ? term.Id : (Guid?)null
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                observation.File = await storage.UploadFile(model.File.File.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.INTRANET_FILES,
                    Path.GetExtension(model.File.File.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            await _studentObservationService.Insert(observation);

            return Ok();
        }
        /// <summary>
        /// Método para realizar el cambio de programa o especialidad del estudiante dentro del mismo plan de estudios
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="academicProgramId">Id del nuevo programa académico</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/solicitud-tramites/cambio-programa")]
        public async Task<IActionResult> ChangeAcademicProgramRequest(Guid id, Guid academicProgramId)
        {
            if (academicProgramId == Guid.Empty)
            {
                return BadRequest("Debe completar todos los campos");
            }

            var result = await _studentService.StudentChangeAcademicProgramRequest(User, id, academicProgramId);

            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok();
        }

        /// <summary>
        /// Método para realizar la amnistía del estudiante moviendolo al último plan de estudios y comenzando desde el primer ciclo
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="model">Modelo con información de la amnistía</param>
        /// <returns>Objeto con información del último plan</returns>
        [HttpPost("informacion/{id}/solicitud-tramites/amnistia")]
        public async Task<IActionResult> AmnestyRequest(Guid id, AmnestyRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Debe completar todos los campos");
            }

            var student = await _studentService.Get(id);
            //var oldCurriculum = await _curriculumService.Get(student.CurriculumId);
            var lastCurriculum = await _curriculumService.GetCareerLastCurriculum(student.CareerId);

            if (student.CurriculumId == lastCurriculum.Id)
            {
                var academicYearCourses = await _academicYearCourseService.GetAll(student.CurriculumId);
                var academicHistories = await _academicHistoryService.GetByStudentId(student.Id);

                var toRemove = academicHistories.Where(x => academicYearCourses.Any(y => y.CourseId == x.CourseId)).ToList();
                _academicHistoryService.RemoveRange(toRemove);
            }

            student.CurrentAcademicYear = 1;
            student.CurriculumId = lastCurriculum.Id;

            var storage = new CloudStorageService(_storageCredentials);
            var term = await _termService.GetActiveTerm();

            var entity = new StudentObservation
            {
                StudentId = id,
                //Observation = $"Cambio de plan de estudios por AMNISTÍA. Plan anterior: {oldCurriculum.Code} Plan nuevo: {lastCurriculum.Code}",
                Observation = $"AMNISTÍA: {model.Observation}",
                UserId = _userManager.GetUserId(User),
                Type = ConstantHelpers.OBSERVATION_TYPES.AMNESTY,
                TermId = term != null ? term.Id : (Guid?)null
            };

            if (model.File != null)
            {
                entity.File = await storage.UploadFile(model.File.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.RESOLUTIONS,
                    Path.GetExtension(model.File.FileName), ConstantHelpers.FileStorage.SystemFolder.ENROLLMENT);
            }

            await _studentObservationService.Insert(entity);

            var result = new
            {
                id = lastCurriculum.Id,
                code = lastCurriculum.Code
            };

            return Ok(result);
        }

        /// <summary>
        /// Método para realizar el reingreso del estudiante a la universidad manteniendo el estado previo a su retiro
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="file">Documento o resolución adjunta</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/solicitud-tramites/reingreso")]
        public async Task<IActionResult> ReentryStudent(Guid id, IFormFile file)
        {
            var term = await _termService.GetActiveTerm();
            var fileUrl = string.Empty;
            var student = await _studentService.Get(id);

            var inactiveStatus = new List<int> {
                ConstantHelpers.Student.States.RESERVED,
                ConstantHelpers.Student.States.SANCTIONED,
                ConstantHelpers.Student.States.DESERTION,
                ConstantHelpers.Student.States.RETIRED,
                ConstantHelpers.Student.States.NOENROLLMENT,
                ConstantHelpers.Student.States.EXPELLED,
                ConstantHelpers.Student.States.RESIGNATION,
                ConstantHelpers.Student.States.CANCELLATION
            };

            if (!inactiveStatus.Contains(student.Status))
                return BadRequest("El alumno ya se encuentra activo");

            if (file != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                fileUrl = await storage.UploadFile(file.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.INTRANET_FILES,
                    Path.GetExtension(file.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            var result = await _studentService.ReentryStudentRequest(User, id, fileUrl);

            if (!result.Succeeded)
                return BadRequest(result.Message);

            await _paymentService.GenerateStudentReentryPayments(student.Id);

            if (term != null)
            {
                var careerShift = await _careerEnrollmentShiftService.Get(term.Id, student.CareerId);
                if (careerShift != null && careerShift.WasExecuted)
                    await _enrollmentTurnService.GenerateStudentTurn(term.Id, student.Id);
            }

            return Ok();
        }

        /// <summary>
        /// Método para registrar la renuncia del estudiante a la universidad. Se retirará automáticamente de los cursos matriculados y cambiará su estado.
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="model">Modelo con la información de la renuncia</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/solicitud-tramites/renuncia")]
        public async Task<IActionResult> ResignStudent(Guid id, ReasonViewModel model)
        {
            var fileUrl = string.Empty;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                fileUrl = await storage.UploadFile(model.File.File.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.INTRANET_FILES,
                    Path.GetExtension(model.File.File.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            var result = await _studentService.ResignStudentRequest(User, id, model.Reason, fileUrl);

            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok();
        }

        /// <summary>
        /// Método para registrar la anulación de ingreso del estudiante a la universidad. Se anulará su ingreso solo si no tiene historial académico registrado
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="model">Modelo con la información de la anulación</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/solicitud-tramites/anular-ingreso")]
        public async Task<IActionResult> CancelStudentEntry(Guid id, ReasonViewModel model)
        {
            var academicHistories = await _academicHistoryService.GetByStudentId(id);
            if (academicHistories.Any())
                return BadRequest("El alumno posee notas registradas. Debe usar la opción de renuncia.");

            var term = await _termService.GetActiveTerm();

            if (term != null)
            {
                var studentSections = await _studentSectionService.GetAll(id, term.Id);
                foreach (var item in studentSections)
                {
                    item.Status = ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN;
                }
                await _studentSectionService.UpdateRange(studentSections);
            }

            var student = await _studentService.Get(id);
            student.Status = ConstantHelpers.Student.States.CANCELLATION;
            await _studentService.Update(student);

            var userId = _userManager.GetUserId(User);
            var observation = new StudentObservation
            {
                Observation = model.Reason,
                StudentId = id,
                Type = ConstantHelpers.OBSERVATION_TYPES.CANCELLATION,
                UserId = userId,
                TermId = term != null ? term.Id : (Guid?)null
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                observation.File = await storage.UploadFile(model.File.File.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.INTRANET_FILES,
                    Path.GetExtension(model.File.File.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            await _studentObservationService.Insert(observation);

            return Ok();
        }

        [HttpPost("informacion/{id}/solicitud-tramites/cambiar-beneficio")]
        public async Task<IActionResult> CancelStudentEntry(Guid id, byte benefit)
        {
            var student = await _studentService.Get(id);

            student.Benefit = benefit;

            await _studentService.Update(student);

            return Ok();
        }
        /// <summary>
        /// Retorna la vista parcial de observaciones del estudiante
        /// </summary>
        /// <returns>Vista parcial</returns>
        [HttpGet("informacion/{id}/observaciones")]
        public IActionResult _Observations()
        {
            return PartialView();
        }
        /// <summary>
        /// Retorna la lista de observaciones relacionadas al estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Objeto con la lista de observaciones</returns>
        [HttpGet("informacion/{id}/observaciones/get")]
        public async Task<IActionResult> GetStudentObservations(Guid id)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentObservationService.GetObservationsDatatable(sentParameters, id);
            return Ok(result);
        }

        [HttpGet("informacion/observacion/{id}/get")]
        public async Task<IActionResult> GetObservation(Guid id)
        {
            var observation = await _studentObservationService.Get(id);
            var result = new
            {
                observationFile = observation.File,
                observation = observation.Observation,
                observationType = observation.Type,
                observationTypeName = ConstantHelpers.OBSERVATION_TYPES.VALUES.ContainsKey(observation.Type) ? ConstantHelpers.OBSERVATION_TYPES.VALUES[observation.Type] : "-",
                term = observation.TermId.HasValue ? (await _termService.Get(observation.TermId.Value))?.Name : "-"
            };
            return Ok(result);
        }
        /// <summary>
        /// Método para registrar una nueva observación del estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <param name="Observation">Texto de la observación a crear</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("informacion/{id}/observaciones/crear")]
        public async Task<IActionResult> CreateObservation(Guid id, string observation, IFormFile observationFile, byte observationType, Guid? termId)
        {
            var userId = _userManager.GetUserId(User);
            var term = await _termService.GetActiveTerm();

            var studentObservation = new StudentObservation
            {
                UserId = userId,
                StudentId = id,
                Observation = observation,
                //Type = ConstantHelpers.OBSERVATION_TYPES.OBSERVATION,
                Type = observationType,
                TermId = term != null ? term.Id : (Guid?)null
            };

            if (termId.HasValue)
                studentObservation.TermId = termId;

            if (observationFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                studentObservation.File = await storage.UploadFile(observationFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.STUDENT_OBSERVATION,
                Path.GetExtension(observationFile.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            await _studentObservationService.Insert(studentObservation);

            return Ok();
        }

        [HttpGet("informacion/{id}/estadisticas")]
        public async Task<IActionResult> _StudentStatistics(Guid id)
        {
            var student = await _studentService.Get(id);
            var user = await _userService.Get(student.UserId);
            var model = new StudentStatisticsViewModel
            {
                FullName = user.FullName,
                StudentId = id,
                Picture = user.Picture,
                UserName = user.UserName
            };

            return View(model);
        }

        [HttpGet("informacion/{id}/estadisticas/promedio-periodo-grafico")]
        public async Task<IActionResult> StudentAveragePerTerm(Guid id)
        {
            var result = await _academicSummariesService.GetAcademicSummariesReportByStudent(id);
            return Ok(result);
        }

        [HttpGet("informacion/{id}/estadisticas/competencias-detallado")]
        public async Task<IActionResult> StudentStatisticsCompetenciesDetailed(Guid id)
        {
            var student = await _studentService.Get(id);
            var user = await _userService.Get(student.UserId);

            var competencies = await _context.CurriculumCompetencies.Where(x => x.CurriculumId == student.CurriculumId)
                .OrderBy(x => x.Competencie.Name)
                .Include(x => x.Competencie)
                .ToListAsync();

            var academicYearCourses = await _academicYearCourseService.GetWithHistoriesByCurriculumAndStudent(student.CurriculumId, student.Id);

            var modelCompetencies = competencies
                .Select(x => new StudentStatisticsCompetenciesViewModel
                {
                    Competencie = x.Competencie.Name,
                    Type = x.Competencie.Type,
                    Description = x.Description,
                    Details = academicYearCourses.Where(y => y.CompetencieId == x.CompetencieId)
                    .Select(y => new StudentStatisticsCompetenciesDetailViewModel
                    {
                        Course = y.Course.Name,
                        Code = y.Course.Code,
                        Credits = y.Course.Credits,
                        FinalGrade =
                        y.Course.AcademicHistories.Any() ?
                        y.Course.AcademicHistories?
                        .Where(ayc => !ayc.Withdraw)
                        .OrderByDescending(z => z.Term.Year)
                        .ThenByDescending(z => z.Term.Number)
                        .ThenByDescending(z => z.Grade)
                        .Select(ah => ah.Grade)
                        .FirstOrDefault()
                        :
                        null
                        ,
                        Approved =
                        y.Course.AcademicHistories.Any() ?
                        y.Course.AcademicHistories?
                        .Where(ayc => !ayc.Withdraw)
                        .OrderByDescending(z => z.Term.Year)
                        .ThenByDescending(z => z.Term.Number)
                        .ThenByDescending(z => z.Grade)
                        .Select(ah => ah.Approved)
                        .FirstOrDefault() :
                        null
                    })
                    .ToList()
                })
                .ToList();

            var model = new StudentStatisticsViewModel
            {
                FullName = user.FullName,
                StudentId = id,
                Picture = user.Picture,
                UserName = user.UserName,
                Competencies = modelCompetencies
            };

            return PartialView("_StudentCompetenciesDetailed", model);
        }

        [HttpGet("informacion/{id}/estadisticas/competencias-graficos")]
        public async Task<IActionResult> StudentStatisticsCompetenciesChart(Guid id)
        {
            var student = await _studentService.Get(id);

            var competencies = await _context.CurriculumCompetencies.Where(x => x.CurriculumId == student.CurriculumId)
                .OrderBy(x => x.Competencie.Name)
                .Include(x => x.Competencie)
                .ToListAsync();

            var academicYearCourses = await _academicYearCourseService.GetWithHistoriesByCurriculumAndStudent(student.CurriculumId, student.Id);

            var model = competencies
                .Select(x => new StudentStatisticsCompetenciesViewModel
                {
                    CompetencieId = x.CompetencieId,
                    Competencie = x.Competencie.Name,
                    Type = x.Competencie.Type,
                    Description = x.Description,
                    Details = academicYearCourses.Where(y => y.CompetencieId == x.CompetencieId)
                    .Select(y => new StudentStatisticsCompetenciesDetailViewModel
                    {
                        Course = y.Course.Name,
                        Code = y.Course.Code,
                        Credits = y.Course.Credits,
                        FinalGrade =
                        y.Course.AcademicHistories.Any() ?
                        y.Course.AcademicHistories?
                        .Where(ayc => !ayc.Withdraw)
                        .OrderByDescending(z => z.Term.Year)
                        .ThenByDescending(z => z.Term.Number)
                        .ThenByDescending(z => z.Grade)
                        .Select(ah => ah.Grade)
                        .FirstOrDefault()
                        :
                        null
                        ,
                        Approved =
                        y.Course.AcademicHistories.Any() ?
                        y.Course.AcademicHistories?
                        .Where(ayc => !ayc.Withdraw)
                        .OrderByDescending(z => z.Term.Year)
                        .ThenByDescending(z => z.Term.Number)
                        .ThenByDescending(z => z.Grade)
                        .Select(ah => ah.Approved)
                        .FirstOrDefault() :
                        null
                    })
                    .ToList()
                })
                .ToList();

            foreach (var item in model)
            {
                if (!string.IsNullOrEmpty(item.Average))
                {
                    var average = Math.Round(Convert.ToDecimal(item.Average), 0, MidpointRounding.AwayFromZero);
                    if (average <= 10)
                    {
                        item.Level = "Deficiente";
                    }
                    else if (average >= 11 && average <= 13)
                    {
                        item.Level = "Regular";
                    }
                    else if (average >= 14 && average <= 16)
                    {
                        item.Level = "Bueno";
                    }
                    else if (average >= 17 && average <= 20)
                    {
                        item.Level = "Excelente";
                    }
                }
            }

            return PartialView("_StudentCompetenciesChart", model);
        }

        /// <summary>
        /// Retorna la lista de escuelas profesionales relacionadas a la facultad y según el rol del usuario en formato para elementos Select2
        /// </summary>
        /// <param name="id">Id de la facultad</param>
        /// <returns>Objetoc con la lista de escuelas</returns>
        [HttpGet("facultades/{id}/carreras/v2/get")]
        public async Task<IActionResult> GetFacultyCareersAll(Guid id)
        {
            if (User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
            {
                var careers = await _academicRecordDepartmentService.GetCareersSelect2ClientSide(User, id, true);
                var result = new
                {
                    items = careers
                };

                return Ok(result);
            }
            else
            {
                var careers = await _careerService.GetSelect2ByFaculty(id);
                var result = new
                {
                    items = careers
                };

                return Ok(result);
            }
        }

        [HttpGet("especialidades/get/{cid}")]
        public async Task<IActionResult> GetAcademicPrograms(Guid cid)
        {
            var result = await _academicProgramService.GetAcademicProgramsSelect(cid);

            return Json(new { items = result });
        }
        /// <summary>
        /// Método privado para obtener el valor de la variable de configuración
        /// </summary>
        /// <param name="list">Lista de variables de configuración existentes</param>
        /// <param name="key">Identificador de la variable</param>
        /// <returns>Texto con el valor de la variable</returns>
        private string GetConfigurationValue(Dictionary<string, string> list, string key)
        {
            return list.ContainsKey(key) ? list[key] :

                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES.ContainsKey(key) ?
                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[key] : "";
        }

        [HttpGet("informacion/{id}/portafolio")]
        public IActionResult _Portfolio()
        {
            return PartialView();
        }
        /// <summary>
        /// Retorna la lista de observaciones relacionadas al estudiante
        /// </summary>
        /// <param name="id">Id del estudiante</param>
        /// <returns>Objeto con la lista de observaciones</returns>
        [HttpGet("informacion/{id}/portafolio/get")]
        public async Task<IActionResult> GetStudentPortfolio(Guid id)
        {
            var result = await _studentPortfolioService.GetStudentPortfolioDatatable(id);
            return Ok(result);
        }

        [HttpPost("informacion/{id}/portafolio/crear")]
        public async Task<IActionResult> CreatePortfolio(Guid id, IFormFile file, Guid type)
        {
            var portfolio = new StudentPortfolio
            {
                StudentId = id,
                StudentPortfolioTypeId = type,
            };

            if (file != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                portfolio.File = await storage.UploadFile(file.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.STUDENT_PORTFOLIO,
                Path.GetExtension(file.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            await _studentPortfolioService.Insert(portfolio);
            return Ok();
        }

        [HttpPost("informacion/{id}/portafolio/validar")]
        public async Task<IActionResult> CreatePortfolio(Guid id, Guid type)
        {
            var portfolio = await _studentPortfolioService.Get(id, type);

            if (portfolio == null)
                return BadRequest("No existe el portafolio del estudiante");

            if (string.IsNullOrEmpty(portfolio.File))
                return BadRequest("Debe registrar el documento primero");

            if (portfolio.IsValidated)
            {
                portfolio.ValidationDate = null;
                portfolio.IsValidated = false;
            }
            else
            {
                portfolio.ValidationDate = DateTime.UtcNow;
                portfolio.IsValidated = true;
            }

            await _studentPortfolioService.Update(portfolio);
            return Ok();
        }

    }
}
