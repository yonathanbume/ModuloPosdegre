using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Web;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation;
using AKDEMIC.INTRANET.Areas.AcademicRecord.Models.GradeReport;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Model;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF)]
    [Area("AcademicRecord")]
    [Route("registrosacademicos/informes-de-grados")]
    public class GradeReportController : BaseController
    {
        private IWebHostEnvironment _hostingEnvironment;
        public IConverter _dinkConverter { get; }
        private readonly IViewRenderService _viewRenderService;
        private readonly ISelect2Service _select2Service;
        private readonly ICareerService _careerService;
        private readonly IFacultyService _facultyService;
        private readonly IStudentService _studentService;
        private readonly IGradeReportService _gradeReportService;
        private readonly IConfigurationService _configurationService;
        private readonly IRegistryPatternService _registryPatternService;
        private readonly IUniversityAuthorityService _universityAuthorityService;
        private readonly ITermService _termService;
        private readonly IUserService _userService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IGradeReportRequirementService _gradeReportRequirementService;
        private readonly IDeanFacultyService _deanFacultyService;
        private readonly IRecordHistoryService _recordHistoryService;
        private readonly IUserInternalProcedureService _userInternalProcedureService;
        private readonly IProcedureService _procedureService;
        private readonly IUserProcedureService _userProcedureService;
        private readonly IDegreeRequirementService _degreeRequirementService;
        protected ReportSettings _reportSettings;

        public GradeReportController(
            IDataTablesService dataTablesService,
            IUniversityAuthorityService universityAuthorityService,
            IRegistryPatternService registryPatternService,
            IStudentService studentService,
            ISelect2Service select2Service,
            ICareerService careerService,
            IFacultyService facultyService,
            IConverter dinkConverter, IViewRenderService viewRenderService,
            IWebHostEnvironment environment,
            IUserInternalProcedureService userInternalProcedureService,
            IProcedureService procedureService,
            IUserProcedureService userProcedureService,
            IDegreeRequirementService degreeRequirementService,
            UserManager<ApplicationUser> userManager,
            IDeanFacultyService deanFacultyService,
            IRecordHistoryService recordHistoryService,
            IOptions<CloudStorageCredentials> storageCredentials,
            IGradeReportRequirementService gradeReportRequirementService,
            IGradeReportService gradeReportService,
            IConfigurationService configurationService,
            IUserService userService,
            ITermService termService,
            IOptionsSnapshot<ReportSettings> reportSettings
        ) : base(userManager, dataTablesService)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
            _gradeReportService = gradeReportService;
            _configurationService = configurationService;
            _careerService = careerService;
            _facultyService = facultyService;
            _select2Service = select2Service;
            _registryPatternService = registryPatternService;
            _studentService = studentService;
            _universityAuthorityService = universityAuthorityService;
            _userService = userService;
            _termService = termService;
            _deanFacultyService = deanFacultyService;
            _recordHistoryService = recordHistoryService;
            _userInternalProcedureService = userInternalProcedureService;
            _procedureService = procedureService;
            _userProcedureService = userProcedureService;
            _degreeRequirementService = degreeRequirementService;
            _storageCredentials = storageCredentials;
            _gradeReportRequirementService = gradeReportRequirementService;
            _reportSettings = reportSettings.Value;
        }

        /// <summary>
        /// Vista donde se gestionan los informaes de grados
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de periodos académicos para ser usado en select
        /// </summary>
        /// <returns>Listado de periodos académicos</returns>
        [HttpGet("periodos/get")]
        public async Task<IActionResult> GetTerms()
        {
            var result = await _termService.GetSelect2Terms();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de informes de grados registrados para ser usado en tablas
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Listado de informes de grados</returns>
        [HttpGet("obtener-informes-grados")]
        public async Task<IActionResult> GetGradeReportDatatable(string searchValue, Guid? careerId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _gradeReportService.GetGradeReportDatatable(sentParameters, careerId, ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR, searchValue);
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se crean los informes de grados
        /// </summary>
        /// <param name="username">Usuario del alumno</param>
        /// <param name="recordHistoryId">Identificador del historial de registro</param>
        /// <returns>Vista</returns>
        [HttpGet("creacion-informe-grados/{username}/{recordHistoryId}")]
        public async Task<IActionResult> CreateReportGrade(string username, Guid recordHistoryId)
        {
            if (!String.IsNullOrEmpty(username))
            {
                ViewBag.username = username;
            }
            else
            {
                ViewBag.username = "";
            }
            ViewBag.RecordHistoryId = recordHistoryId;
            var degreeRequirements = await _degreeRequirementService.GetDegreeRequirementsByType(ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR);
            var configuration = await _configurationService.GetConfigurationByGENIntegratedSystem();
            var result = new ResultViewModel
            {
                DegreeRequirements = degreeRequirements,
                Value = configuration.Value
            };
            return View(result);
        }

        /// <summary>
        /// Obtiene los datos del alumno
        /// </summary>
        /// <param name="username">Usuario del alumno</param>
        /// <returns>Objeto que contiene los datos del alumno</returns>
        [HttpPost("busqueda-por-alumno/{username}")]
        public async Task<IActionResult> SearchStudentByUserName(string username)
        {
            var result = await _gradeReportService.GetStudentByUserName(username);
            return Ok(result);
        }

        /// <summary>
        /// Método para registrar el informe de grado
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del nuevo informe de grado</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("registrar-informe-grado")]
        public async Task<IActionResult> AddGradeReport(GradeReportViewModel model)
        {
            string degreeModality = "";
            var configurationBachelor = new Configuration();
            var recordHistory = await _recordHistoryService.Get(model.RecordHistoryId);

            if (recordHistory is null)
                return BadRequest("No se encontró la solicitud asociada para generar informado de grado.");

            var student = await _studentService.GetWithIncludes(model.StudentId);
            var dean = await _facultyService.GetDeanById(student.Career.FacultyId);
            var universityAuthorities = await _universityAuthorityService.GetUniversityAuthoritiesList();
            var graduationTerm = new Term();
            if (model.GraduationTermId != null && model.GraduationTermId != Guid.Empty)
            {
                graduationTerm = await _termService.Get(model.GraduationTermId);
                if (graduationTerm.Year < student.AdmissionTerm.Year)
                {
                    return BadRequest("El año de egreso no puede ser menor que la fecha de ingreso");
                }
                student.GraduationTermId = graduationTerm.Id;
            }

            var result = await _configurationService.GetConfigurationByGENIntegratedSystem();

            if (await _gradeReportService.ExistGradeReport(model.StudentId, ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR))
            {
                return BadRequest("Ya existe un informe de grado creado para el estudiante seleccionado");
            }

            var record = student.RecordHistories.Where(x => x.Type == ConstantHelpers.RECORDS.BACHELOR).FirstOrDefault();
            var entity = new GradeReport
            {
                Date = DateTime.UtcNow.Date,
                StudentId = model.StudentId,
                Number = record.Code,
                Observation = model.Observation,
                PromotionGrade = model.PromotionGrade,
                Year = DateTime.UtcNow.Year,
                YearsStudied = model.YearsStudied,
                GradeType = ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR,
                BachelorOrigin = model.BachelorOrigin,
                Credits = model.Credits,
                OriginDegreeCountry = model.OriginDegreeCountry,
                PedagogicalTitleOrigin = model.PedagogicalTitleOrigin,
                ResearchWork = model.ResearchWork,
                ResearchWorkURL = model.ResearchWorkURL,
                StudyModality = model.StudyModality,
                SemesterStudied = model.SemesterStudied,
                GraduationDate = ConvertHelpers.DatepickerToUtcDateTime(model.GraduationDate)
            };

            if (bool.Parse(result.Value))
            {
                entity.ProcedureId = model.ProcedureId.Value;
            }
            else
            {
                entity.ConceptId = model.ConceptId.Value;
            }

            degreeModality = CORE.Helpers.ConstantHelpers.DEGREE_OBTENTION_MOD.AUTOMATIC_BACHELOR;

            if (bool.Parse(result.Value))
            {
                configurationBachelor = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.DegreeManagement.TUPA_BACHELOR);

                var guidTupaBachelor = Guid.Empty;

                if (!String.IsNullOrEmpty(configurationBachelor.Value))
                {
                    guidTupaBachelor = new Guid(configurationBachelor.Value);
                }

            }
            else
            {
                configurationBachelor = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.DegreeManagement.CONCEPT_BACHELOR);

                var guidConceptBachelor = Guid.Empty;

                if (!String.IsNullOrEmpty(configurationBachelor.Value))
                {
                    guidConceptBachelor = new Guid(configurationBachelor.Value);
                }

            }

            var entityRegistryPattern = new RegistryPattern();

            if (bool.Parse(result.Value))
            {
                entityRegistryPattern.ProcedureId = model.ProcedureId.Value;
            }
            else
            {
                entityRegistryPattern.ConceptId = model.ConceptId.Value;
            }

            var cultureInfo = new CultureInfo("es-Es");
            TextInfo myTI = cultureInfo.TextInfo;
            var universityCode = await _configurationService.GetByKey(CORE.Helpers.ConstantHelpers.Configuration.General.INSTITUTION_GRADE_CODE);
            entityRegistryPattern.GraduationDate = ConvertHelpers.DatepickerToUtcDateTime(model.GraduationDate);
            entityRegistryPattern.UniversityCode = universityCode != null ? universityCode.Value : "0000";
            entityRegistryPattern.StudentId = model.StudentId;
            entityRegistryPattern.GradeType = ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR;
            entityRegistryPattern.DocumentType = student.User.DocumentType;
            entityRegistryPattern.BussinesSocialReason = GeneralHelpers.GetInstitutionName();
            entityRegistryPattern.AcademicProgram = student.Career.Name;
            entityRegistryPattern.GradeAbbreviation = "B";
            entityRegistryPattern.ObtainingDegreeModality = ConstantHelpers.DEGREE_OBTENTION_MOD.VALUES[degreeModality];
            entityRegistryPattern.Credits = model.Credits;
            entityRegistryPattern.BachelorOrigin = model.BachelorOrigin;
            entityRegistryPattern.OriginDegreeCountry = model.OriginDegreeCountry;
            entityRegistryPattern.PedagogicalTitleOrigin = model.PedagogicalTitleOrigin;
            entityRegistryPattern.ResearchWork = model.ResearchWork;
            entityRegistryPattern.ResearchWorkURL = model.ResearchWorkURL;
            entityRegistryPattern.StudyModality = model.StudyModality;
            entityRegistryPattern.AcademicResponsible = "DECANO";
            entityRegistryPattern.GeneralSecretary = "SECRETARIO GENERAL";
            entityRegistryPattern.ManagingDirector = "RECTOR";
            if (String.IsNullOrEmpty(student.Curriculum.AcademicDegreeBachelor))
                return BadRequest("No se ha consigado la denominación de grado.");
            entityRegistryPattern.AcademicDegreeDenomination = student.Curriculum.AcademicDegreeBachelor;
            entityRegistryPattern.RegistryNumber = $"{(await _registryPatternService.CurrentCountByGradeType(ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR) + 1).ToString():000}";
            //entityRegistryPattern.AcademicDegreeDenomination = "Bachiller en " + myTI.ToTitleCase(student.AcademicProgram.Name.ToLower());

            if (universityAuthorities.Count == 0)
            {
                entityRegistryPattern.GeneralSecretaryFullName = "";
                entityRegistryPattern.ManagingDirectorFullName = "";
            }
            else
            {
                var generalSecretary = universityAuthorities.Where(x => x.Type == ConstantHelpers.UNIVERSITY_AUTHORITY.TYPE.GENERAL_SECRETARY).FirstOrDefault();
                var rector = universityAuthorities.Where(x => x.Type == ConstantHelpers.UNIVERSITY_AUTHORITY.TYPE.RECTOR).FirstOrDefault();
                if (generalSecretary != null)
                {
                    entityRegistryPattern.GeneralSecretaryFullName = generalSecretary.User.FullName;
                }
                if (rector != null)
                {
                    entityRegistryPattern.ManagingDirectorFullName = rector.User.FullName;
                }

            }
            if (dean != null)
            {
                entityRegistryPattern.AcademicResponsibleFullName = dean;
            }
            recordHistory.Status = ConstantHelpers.RECORD_HISTORY_STATUS.FINALIZED;
            await _recordHistoryService.Update(recordHistory);

            //solicitud de grados 
            var procedureBachelor = await _procedureService.GetByStaticType(ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR_DEGREE_APPLICATION);
            var currentTerm = await _termService.GetActiveTerm();
            if (currentTerm == null)
            {
                currentTerm = await _termService.GetLastTerm();
                if (currentTerm == null)
                    return BadRequest("No existen periodos académicos en el sistema");
            }
            var userProcedure = new UserProcedure()
            {
                UserId = student.UserId,
                ProcedureId = procedureBachelor.Id,
                TermId = currentTerm.Id,
                DNI = student.User.Dni,
                Status = ConstantHelpers.USER_PROCEDURES.STATUS.PENDING

            };
            await _userProcedureService.Insert(userProcedure);
            await _gradeReportService.Insert(entity);
            await _registryPatternService.Insert(entityRegistryPattern);
            if (model.DegreeRequirements != null)
            {
                if (model.DegreeRequirements.Count > 0)
                {
                    foreach (var documentFile in model.DegreeRequirements)
                    {
                        var fileName = documentFile.DocumentFile.FileName;

                        if (documentFile.DocumentFile.Length > ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.APPLICATION.GENERIC)
                        {
                            return BadRequest($"El tamaño del archivo '{fileName}' excede el límite de {ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.APPLICATION.GENERIC / 1024 / 1024}MB");
                        }

                        if (!documentFile.DocumentFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.GENERIC))
                        {
                            return BadRequest($"El contenido del archivo '{fileName}' es inválido");
                        }
                    }
                    var cloudStorageService = new CloudStorageService(_storageCredentials);
                    var gradeReportRequirements = new List<GradeReportRequirement>();
                    foreach (var documentFile in model.DegreeRequirements)
                    {
                        try
                        {
                            var uploadFilePath = await cloudStorageService.UploadFile(documentFile.DocumentFile.OpenReadStream(),
                                ConstantHelpers.CONTAINER_NAMES.GRADE_REPORT_REQUIREMENTS, Path.GetExtension(documentFile.DocumentFile.FileName),
                                ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                            gradeReportRequirements.Add(new GradeReportRequirement
                            {
                                Document = uploadFilePath,
                                DegreeRequirementId = documentFile.DegreeRequirementId,
                                Description = documentFile.Observation,
                                GradeReportId = entity.Id,

                            });
                        }
                        catch (Exception)
                        {
                            return BadRequest($"Hubo un problema al subir el archivo '{documentFile.DocumentFile.FileName}'");
                        }
                    }
                    await _gradeReportRequirementService.InsertRange(gradeReportRequirements);
                    return Ok(entity.Id);
                }
            }

            return Ok(entity.Id);

        }

        /// <summary>
        /// Obtiene el listado de modalidades para ser usado en select
        /// </summary>
        /// <returns>Listado de modalidades</returns>
        [HttpGet("obtener-modalidades-grados")]
        public async Task<IActionResult> ModalitySelect()
        {
            var isIntegrated = await _configurationService.GetConfigurationByGENIntegratedSystem();
            var result = await _configurationService.ConfigurationGradeModality(Convert.ToBoolean(isIntegrated.Value), ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales para ser usado en select
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Listado de escuelas profesionales</returns>
        [HttpGet("obtener-carreras")]
        public async Task<IActionResult> GetCareerSelect(string searchValue)
        {
            var requestParameters = _select2Service.GetRequestParameters();
            var result = await _careerService.GetCareerSelect2(requestParameters, searchValue);
            return Ok(result);
        }

        /// <summary>
        /// Vista detalle del informe de grado
        /// </summary>
        /// <param name="gradereportId">identificador del informe de grado</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("detalle/{gradereportId}")]
        public async Task<IActionResult> GradeReportDetail(Guid gradereportId)
        {
            var isIntegrated = await _configurationService.GetConfigurationByGENIntegratedSystem();
            var result = await _gradeReportService.GetGradeReportWithIncludes(gradereportId);
            var detail = new GradeReportDetailViewModel
            {
                AdmissionTermId = result.Student.AdmissionTermId,
                CareerName = result.Student.Career.Name,
                CurricularSystem = ConstantHelpers.CURRICULUM.STUDY_REGIME.VALUES.ContainsKey(result.Student.Curriculum.StudyRegime) ? ConstantHelpers.CURRICULUM.STUDY_REGIME.VALUES[result.Student.Curriculum.StudyRegime] : "No especifica",
                Curriculum = result.Student.Curriculum.Name,
                Date = result.Date.ToLocalDateFormat(),
                FacultyName = result.Student.Career.Faculty.Name,
                FullName = result.Student.User.FullName,
                Name = result.Student.User.Name,
                IsIntegrated = bool.Parse(isIntegrated.Value),
                MaternalSurname = result.Student.User.MaternalSurname,
                Number = result.Number,
                Observation = result.Observation,
                PaternalSurname = result.Student.User.PaternalSurname,
                PromotionGrade = result.PromotionGrade,
                SemesterStudied = result.SemesterStudied,
                UserName = result.Student.User.UserName,
                YearsStudied = result.YearsStudied,
                Year = result.Year,
                Credits = result.Credits,
                OriginDegreeCountry = result.OriginDegreeCountry,
                PedagogicalTitleOrigin = result.PedagogicalTitleOrigin,
                ResearchWork = result.ResearchWork,
                ResearchWorkURL = result.ResearchWorkURL,
                StudyModality = result.StudyModality,
                BachelorOrigin = result.BachelorOrigin,
                GraduationDate = result.GraduationDate.ToLocalDateFormat()

            };
            var gradeReportRequirements = await _gradeReportRequirementService.GetRequirementsByGradeReport(gradereportId);
            detail.GraduationTermId = result.Student.GraduationTermId.HasValue ? result.Student.GraduationTermId.Value : Guid.Empty;
            detail.GradeReportDegreeRequirements = gradeReportRequirements;


            if (result.Student.AcademicProgram != null)
            {
                detail.AcademicProgram = result.Student.AcademicProgram.Name;
            }
            if (result.ConceptId.HasValue)
            {
                detail.ConceptId = result.ConceptId;
            }
            if (result.ProcedureId.HasValue)
            {
                detail.ProcedureId = result.ProcedureId;
            }

            return View(detail);
        }

        /// <summary>
        /// Método para generar la constancia de grados
        /// </summary>
        /// <param name="gradeReportId">Identificador del informe de grado</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("generar-constancia-grado/{gradeReportId}")]
        public async Task<IActionResult> BachelorsDegree(Guid gradeReportId)
        {
            //var userId = _userService.GetUserIdByClaim(User);
            var userName = _userManager.GetUserName(User);
            var gradeReport = await _gradeReportService.GetGradeReportWithIncludes(gradeReportId);
            //var academicSemesters = await _academicSummariesService.GetacademicSemestersCount(id);

            var model = new BachelorsViewModel
            {
                Number = gradeReport.Number,
                YearInform = gradeReport.Date.Year.ToString(),
                Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                DateInform = gradeReport.CreatedAt.ToDefaultTimeZone().Value.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                Student = gradeReport.Student.User.FullName.ToUpper(),
                School = gradeReport.Student.Career.Name.ToUpper(),
                Faculty = gradeReport.Student.Career.Faculty.Name.ToUpper(),
                Specialty = gradeReport.Student.AcademicProgram is null ? "ESPECIALIDAD" : gradeReport.Student.AcademicProgram.Name,
                StartYear = gradeReport.Student.AdmissionTerm.Name,
                RegistrationNumber = gradeReport.Student.User.UserName,
                Average = gradeReport.PromotionGrade,
                AcademicSemesters = gradeReport.SemesterStudied,
                StudentGender = gradeReport.Student.User.Sex,
                PageCode = gradeReport.Number,
                Curriculum = ConstantHelpers.CURRICULUM.STUDY_REGIME.VALUES.ContainsKey(gradeReport.Student.Curriculum.StudyRegime) ? ConstantHelpers.CURRICULUM.STUDY_REGIME.VALUES[gradeReport.Student.Curriculum.StudyRegime].ToUpper() : "No especifica",
                HeaderText = _reportSettings.BachelorsDegreeHeaderText,
                Observations = gradeReport.Observation,
                EndYear = gradeReport.Student.GraduationTerm is null ? "-" : gradeReport.Student.GraduationTerm.Name
            };

            //if (gradeReport.Student.GraduationYear.HasValue)
            //{
            //    model.EndYear = gradeReport.Student.GraduationTerm
            //    EndYear = gradeReport.Student.GraduationTerm is null ? gradeReport.Student.GraduationYear is null ? gradeReport.Student.GraduationYear.Value : "-" : gradeReport.Student.GraduationTerm.Name,
            //}
            //else
            //{

            //}

            var IsBolean = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM);
            var BachelorConfiguration = new Configuration();
            var BachelorType = 0;

            if (bool.Parse(IsBolean.Value))
            {
                BachelorConfiguration = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.DegreeManagement.TUPA_TYPE_BACHELOR);
                BachelorType = int.Parse(BachelorConfiguration.Value);
                if (BachelorType == ConstantHelpers.Configuration.BachelorTypeConfiguration.TUPA.AUTOMATIC)
                {
                    model.Bachelorship = "AUTOMATICO";
                }
                else
                {
                    model.Bachelorship = "POR SOLICITUD";
                }

            }
            else
            {
                BachelorConfiguration = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.DegreeManagement.CONCEPT_TYPE_BACHELOR);
                BachelorType = int.Parse(BachelorConfiguration.Value);
                if (BachelorType == ConstantHelpers.Configuration.BachelorTypeConfiguration.TUPA.AUTOMATIC)
                {
                    model.Bachelorship = "AUTOMATICO";
                }
                else
                {
                    model.Bachelorship = "POR SOLICITUD";
                }
            }



            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 20,
                    Left = 20,
                    Right = 20,
                    Top = 20,
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
                    FontSize = 7,
                    Left="Elaborado por: "+ userName + " - " + DateTime.UtcNow.ToLocalDateFormat()
                 },
            };

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Informe-de-grado-bachiller.pdf");
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileContents, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }

        /// <summary>
        /// Obtiene la fecha fin del periodo académico
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Fecha fin del periodo académico</returns>
        [HttpGet("obtener-fecha-finalizacion/{termId}")]
        public async Task<IActionResult> GetEndDate(Guid termId)
        {
            var result = await _gradeReportService.GetEndDateByTermId(termId);
            return Ok(result);
        }

        /// <summary>
        /// Método para descargar requerimiento
        /// </summary>
        /// <param name="id">Identificador del requerimiento</param>
        /// <returns>Archivo</returns>
        [HttpGet("descargar-documento/{id}")]
        public async Task DonwloadDocument(Guid id)
        {
            var gradeReportRequirement = await _gradeReportRequirementService.Get(id);
            var fileName = gradeReportRequirement.Document;
            await GeneralHelpers.GetFileForDownload(HttpContext, _storageCredentials, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.GRADE_REPORT_REQUIREMENTS, fileName);

        }

    }
}
