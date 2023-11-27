using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation;
using AKDEMIC.INTRANET.Areas.AcademicRecord.Models.TitleReport;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Model;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using iTextSharp.text.xml.simpleparser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF)]
    [Area("AcademicRecord")]
    [Route("registrosacademicos/informes-de-titulos")]
    public class TitleReportController : BaseController
    {
        private IWebHostEnvironment _hostingEnvironment;
        public IConverter _dinkConverter { get; }
        private readonly IViewRenderService _viewRenderService;
        private readonly ISelect2Service _select2Service;
        private readonly ICareerService _careerService;
        private readonly IStudentService _studentService;
        private readonly IGradeReportService _gradeReportService;
        private readonly IConfigurationService _configurationService;
        private readonly IRegistryPatternService _registryPatternService;
        private readonly IUniversityAuthorityService _universityAuthorityService;
        private readonly IUserService _userService;
        private readonly IFacultyService _facultyService;
        private readonly IUserInternalProcedureService _userInternalProcedureService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IDegreeRequirementService _degreeRequirementService;
        private readonly IGradeReportRequirementService _gradeReportRequirementService;
        private readonly IRecordHistoryService _recordHistoryService;
        private readonly IProcedureService _procedureService;
        private readonly IUserProcedureService _userProcedureService;
        protected ReportSettings _reportSettings;

        public TitleReportController(
            IDataTablesService dataTablesService,
            IConverter dinkConverter,
            IViewRenderService viewRenderService,
            IWebHostEnvironment environment,
            IRegistryPatternService registryPatternService,
            IUniversityAuthorityService universityAuthorityService,
            IStudentService studentService,
            UserManager<ApplicationUser> userManager,
            IDegreeRequirementService degreeRequirementService,
            IFacultyService facultyService,
            IOptions<CloudStorageCredentials> storageCredentials,
            IUserInternalProcedureService userInternalProcedureService,
            IGradeReportRequirementService gradeReportRequirementService,
            IRecordHistoryService recordHistoryService,
            IProcedureService procedureService,
            IUserProcedureService userProcedureService,
            IUserService userService,
            ISelect2Service select2Service,
            ICareerService careerService,
            IGradeReportService gradeReportService,
            IConfigurationService configurationService,
            IOptionsSnapshot<ReportSettings> reportSettings
        ) : base(userManager, dataTablesService)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
            _userService = userService;
            _gradeReportService = gradeReportService;
            _configurationService = configurationService;
            _careerService = careerService;
            _select2Service = select2Service;
            _registryPatternService = registryPatternService;
            _studentService = studentService;
            _universityAuthorityService = universityAuthorityService;
            _facultyService = facultyService;
            _userInternalProcedureService = userInternalProcedureService;
            _storageCredentials = storageCredentials;
            _degreeRequirementService = degreeRequirementService;
            _gradeReportRequirementService = gradeReportRequirementService;
            _recordHistoryService = recordHistoryService;
            _procedureService = procedureService;
            _userProcedureService = userProcedureService;
            _reportSettings = reportSettings.Value;
        }

        /// <summary>
        /// Vista donde se gestiona los informes de títulos
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de informes de títulos para ser usado en tablas
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Listado de informes de título</returns>
        [HttpGet("obtener-informes-titulos")]
        public async Task<IActionResult> GetGradeReportDatatable(string searchValue, Guid? careerId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _gradeReportService.GetGradeReportDatatable(sentParameters, careerId, ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE, searchValue);
            return Ok(result);
        }

        /// <summary>
        /// Vista de creación del informe de título
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="recordHistoryId">Identificador de la solicitud</param>
        /// <returns>Vista de creación</returns>
        [HttpGet("creacion-informe-titulo/{username}/{recordHistoryId}")]
        public async Task<IActionResult> CreateReportGradeTitle(string username, Guid recordHistoryId)
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
            var degreeRequirements = await _degreeRequirementService.GetDegreeRequirementsByType(ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE);
            var configuration = await _configurationService.GetConfigurationByGENIntegratedSystem();
            var result = new ResultViewModel
            {
                DegreeRequirements = degreeRequirements,
                Value = configuration.Value
            };
            return View(result);
        }

        /// <summary>
        /// Método para obtener los datos del alumno
        /// </summary>
        /// <param name="username">Nombre de usuario del alumno</param>
        /// <returns></returns>
        [HttpPost("busqueda-por-alumno/{username}")]
        public async Task<IActionResult> SearchStudentByUserName(string username)
        {
            var result = await _gradeReportService.GetStudentByUserNameBachelor(username);
            return Ok(result);
        }

        /// <summary>
        /// Método para registrar el informe de título
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del nuevo informe de título</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("registrar-informe-titulo")]
        public async Task<IActionResult> AddGradeReportTitle(TitleReportViewModel model)
        {
            string degreeModality = ConstantHelpers.DEGREE_OBTENTION_MOD.NO_SPECIFY;
            var configurationDegreeProffesionalExperience = new Configuration();
            var configurationDegreeSufficiencyExam = new Configuration();
            var configurationDegreeSupportTesis = new Configuration();
            var recordHistory = await _recordHistoryService.Get(model.RecordHistoryId);

            if (recordHistory is null)
                return BadRequest("No se encontró la solicitud asociada para generar informado de grado.");

            var student = await _studentService.GetWithIncludes(model.StudentId);
            var universityAuthorities = await _universityAuthorityService.GetUniversityAuthoritiesList();
            var deanFaculty = await _facultyService.GetDeanById(student.Career.FacultyId);
            var gradeReport = await _gradeReportService.GetGradeReportBachelor(student.Id);
            if (await _gradeReportService.ExistGradeReport(model.StudentId, ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE))
            {
                return BadRequest("Ya existe un informe de título creado para el estudiante seleccionado");
            }
            if (!await _gradeReportService.ExistGradeReport(model.StudentId, ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR))
            {
                return BadRequest("Se requiere un informe de grado para el estudiante");
            }


            var result = await _configurationService.GetConfigurationByGENIntegratedSystem();
            var entity = new GradeReport
            {
                Date = DateTime.UtcNow.Date,
                StudentId = model.StudentId,
                Number = gradeReport.Number,
                Observation = model.Observation,
                PromotionGrade = model.PromotionGrade,
                SemesterStudied = model.SemesterStudied,
                Year = DateTime.UtcNow.Year,
                YearsStudied = model.YearsStudied,
                GradeType = ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE,
                BachelorOrigin = model.BachelorOrigin,
                Credits = model.Credits,
                OriginDegreeCountry = model.OriginDegreeCountry,
                PedagogicalTitleOrigin = model.PedagogicalTitleOrigin,
                ResearchWork = model.ResearchWork,
                ResearchWorkURL = model.ResearchWorkURL,
                StudyModality = model.StudyModality,
                GraduationDate = gradeReport.GraduationDate
            };
            if (bool.Parse(result.Value))
            {
                entity.ProcedureId = model.ProcedureId.Value;
            }
            else
            {
                entity.ConceptId = model.ConceptId.Value;
            }

            if (bool.Parse(result.Value))
            {

                configurationDegreeProffesionalExperience = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE);
                configurationDegreeSufficiencyExam = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM);
                configurationDegreeSupportTesis = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUPPORT_TESIS);

                var guidTupaTitleDegreeProffesionalExperience = Guid.Empty;
                var guidTupaTitleDegreeSufficiencyExam = Guid.Empty;
                var guidTupaTitleDegreeSupportTesis = Guid.Empty;


                if (!String.IsNullOrEmpty(configurationDegreeProffesionalExperience.Value))
                {
                    guidTupaTitleDegreeProffesionalExperience = new Guid(configurationDegreeProffesionalExperience.Value);
                }

                if (!String.IsNullOrEmpty(configurationDegreeSufficiencyExam.Value))
                {
                    guidTupaTitleDegreeSufficiencyExam = new Guid(configurationDegreeSufficiencyExam.Value);
                }
                if (!String.IsNullOrEmpty(configurationDegreeSupportTesis.Value))
                {
                    guidTupaTitleDegreeSupportTesis = new Guid(configurationDegreeSupportTesis.Value);
                }


                if (model.ProcedureId == guidTupaTitleDegreeProffesionalExperience)
                {
                    degreeModality = CORE.Helpers.ConstantHelpers.DEGREE_OBTENTION_MOD.PROFFESIONAL_EXPERIENCE;
                }
                if (model.ProcedureId == guidTupaTitleDegreeSufficiencyExam)
                {
                    degreeModality = CORE.Helpers.ConstantHelpers.DEGREE_OBTENTION_MOD.SUFFIENCY_EXAM;
                }

                if (model.ProcedureId == guidTupaTitleDegreeSupportTesis)
                {
                    degreeModality = CORE.Helpers.ConstantHelpers.DEGREE_OBTENTION_MOD.SUPPORTING_TESIS;
                }

            }
            else
            {
                configurationDegreeProffesionalExperience = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE);
                configurationDegreeSufficiencyExam = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM);
                configurationDegreeSupportTesis = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUPPORT_TESIS);

                var guidConceptBachelor = Guid.Empty;
                var guidConceptTitleDegreeProffesionalExperience = Guid.Empty;
                var guidConceptTitleDegreeSufficiencyExam = Guid.Empty;
                var guidConceptTitleDegreeSupportTesis = Guid.Empty;



                if (!String.IsNullOrEmpty(configurationDegreeProffesionalExperience.Value))
                {
                    guidConceptTitleDegreeProffesionalExperience = new Guid(configurationDegreeProffesionalExperience.Value);
                }

                if (!String.IsNullOrEmpty(configurationDegreeSufficiencyExam.Value))
                {
                    guidConceptTitleDegreeSufficiencyExam = new Guid(configurationDegreeSufficiencyExam.Value);
                }
                if (!String.IsNullOrEmpty(configurationDegreeSupportTesis.Value))
                {
                    guidConceptTitleDegreeSupportTesis = new Guid(configurationDegreeSupportTesis.Value);
                }


                if (model.ConceptId == guidConceptTitleDegreeProffesionalExperience)
                {
                    degreeModality = CORE.Helpers.ConstantHelpers.DEGREE_OBTENTION_MOD.PROFFESIONAL_EXPERIENCE;
                }
                if (model.ConceptId == guidConceptTitleDegreeSufficiencyExam)
                {
                    degreeModality = CORE.Helpers.ConstantHelpers.DEGREE_OBTENTION_MOD.SUFFIENCY_EXAM;
                }
                if (model.ConceptId == guidConceptTitleDegreeSupportTesis)
                {
                    degreeModality = CORE.Helpers.ConstantHelpers.DEGREE_OBTENTION_MOD.SUPPORTING_TESIS;
                }
            }

            var entityRegistryPattern = new RegistryPattern();

            if (bool.Parse(result.Value))
            {
                entityRegistryPattern.ProcedureId = model.ProcedureId;
            }
            else
            {
                entityRegistryPattern.ConceptId = model.ConceptId;
            }
            var cultureInfo = new CultureInfo("es-Es");
            var universityCode = await _configurationService.GetByKey(CORE.Helpers.ConstantHelpers.Configuration.General.INSTITUTION_GRADE_CODE);
            TextInfo myTI = cultureInfo.TextInfo;
            entityRegistryPattern.GraduationDate = gradeReport.GraduationDate;
            entityRegistryPattern.UniversityCode = universityCode != null ? universityCode.Value : "0000";
            entityRegistryPattern.StudentId = model.StudentId;
            entityRegistryPattern.GradeType = ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE;
            entityRegistryPattern.DocumentType = student.User.DocumentType;
            entityRegistryPattern.BussinesSocialReason = GeneralHelpers.GetInstitutionName();
            entityRegistryPattern.AcademicProgram = student.Career.Name;
            entityRegistryPattern.GradeAbbreviation = "T";
            entityRegistryPattern.ObtainingDegreeModality = degreeModality;
            entityRegistryPattern.BachelorOrigin = model.BachelorOrigin;
            entityRegistryPattern.Credits = model.Credits;
            entityRegistryPattern.OriginDegreeCountry = model.OriginDegreeCountry;
            entityRegistryPattern.PedagogicalTitleOrigin = model.PedagogicalTitleOrigin;
            entityRegistryPattern.ResearchWork = model.ResearchWork;
            entityRegistryPattern.ResearchWorkURL = model.ResearchWorkURL;
            entityRegistryPattern.StudyModality = model.StudyModality;
            //entityRegistryPattern.AcademicDegreeDenomination = "Titulado en " + myTI.ToTitleCase(student.AcademicProgram.Name.ToLower());
            entityRegistryPattern.AcademicDegreeDenomination = student.Curriculum.AcademicDegreeProfessionalTitle;

            if (String.IsNullOrEmpty(student.Curriculum.AcademicDegreeProfessionalTitle))
                return BadRequest("No se ha consigado la denominación de grado.");


            entityRegistryPattern.AcademicResponsible = "DECANO";
            entityRegistryPattern.GeneralSecretary = "SECRETARIO GENERAL";
            entityRegistryPattern.ManagingDirector = "RECTOR";
            entityRegistryPattern.RegistryNumber = $"{(await _registryPatternService.CurrentCountByGradeType(ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE) + 1).ToString():000}";

            if (universityAuthorities.Count == 0)
            {
                entityRegistryPattern.GeneralSecretaryFullName = "";
                entityRegistryPattern.ManagingDirectorFullName = "";
            }
            else
            {
                var generalSecretary = universityAuthorities.Where(x => x.Type == ConstantHelpers.UNIVERSITY_AUTHORITY.TYPE.GENERAL_SECRETARY).FirstOrDefault();
                var rector = universityAuthorities.Where(x => x.Type == ConstantHelpers.UNIVERSITY_AUTHORITY.TYPE.RECTOR).FirstOrDefault();
                entityRegistryPattern.GeneralSecretaryFullName = generalSecretary.User.FullName;
                entityRegistryPattern.ManagingDirectorFullName = rector.User.FullName;
            }
            if (deanFaculty != null)
            {
                entityRegistryPattern.AcademicResponsibleFullName = deanFaculty;
            }

            recordHistory.Status = ConstantHelpers.RECORD_HISTORY_STATUS.FINALIZED;
            await _recordHistoryService.Update(recordHistory);

            //solicitud de titulos 
            var procedureTitle = await _procedureService.GetByStaticType(ConstantHelpers.PROCEDURES.STATIC_TYPE.TITLE_DEGREE_APPLICATION);
            var currentTerm = await GetActiveTerm();
            if (currentTerm == null)
            {
                currentTerm = await _termService.GetLastTerm();
                if (currentTerm == null)
                    return BadRequest("No existen periodos académicos en el sistema");
            }
            var userProcedure = new UserProcedure()
            {
                UserId = student.UserId,
                ProcedureId = procedureTitle.Id,
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
        /// Obtiene el listado de modalidades de título
        /// </summary>
        /// <returns>Listado de modalidades</returns>
        [HttpGet("obtener-modalidades-titulo")]
        public async Task<IActionResult> ModalitySelect()
        {
            var isIntegrated = await _configurationService.GetConfigurationByGENIntegratedSystem();
            var result = await _configurationService.ConfigurationGradeModality(Convert.ToBoolean(isIntegrated.Value), ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE);
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
        /// Vista detalle del informe de título
        /// </summary>
        /// <param name="titlereportId">Identificador del informe de título</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("detalle/{titlereportId}")]
        public async Task<IActionResult> TitleReportDetail(Guid titlereportId)
        {
            var isIntegrated = await _configurationService.GetConfigurationByGENIntegratedSystem();
            var result = await _gradeReportService.GetGradeReportWithIncludes(titlereportId);
            var gradeReportBachelor = await _gradeReportService.GetGradeReportBachelor(result.StudentId);
            var gradeReportRequirements = await _gradeReportRequirementService.GetRequirementsByGradeReport(titlereportId);
            var detail = new TitleReportDetailViewModel
            {
                AdmissionTermId = result.Student.AdmissionTermId,
                CareerName = result.Student.Career.Name,
                CurricularSystem = ConstantHelpers.CURRICULUM.STUDY_REGIME.VALUES.ContainsKey(result.Student.Curriculum.StudyRegime) ? ConstantHelpers.CURRICULUM.STUDY_REGIME.VALUES[result.Student.Curriculum.StudyRegime] : "No especifica",
                Curriculum = result.Student.Curriculum.Name,
                Date = result.Date.ToLocalDateFormat(),
                FacultyName = result.Student.Career.Faculty.Name,
                FullName = result.Student.User.FullName,
                Name = result.Student.User.Name,
                GraduationTermId = result.Student.GraduationTermId.Value,
                IsIntegrated = bool.Parse(isIntegrated.Value),
                MaternalSurname = result.Student.User.MaternalSurname,
                Number = result.Number,
                Observation = result.Observation,
                PaternalSurname = result.Student.User.PaternalSurname,
                PromotionGrade = gradeReportBachelor == null ? 0.0M : gradeReportBachelor.PromotionGrade,
                SemesterStudied = gradeReportBachelor == null ? 0 : gradeReportBachelor.SemesterStudied,
                UserName = result.Student.User.UserName,
                YearsStudied = gradeReportBachelor == null ? 0 : gradeReportBachelor.YearsStudied,
                Year = result.Year,
                BachelorOrigin = result.BachelorOrigin,
                Credits = gradeReportBachelor == null ? 0 : gradeReportBachelor.Credits,
                OriginDegreeCountry = result.OriginDegreeCountry,
                PedagogicalTitleOrigin = result.PedagogicalTitleOrigin,
                ResearchWork = result.ResearchWork,
                ResearchWorkURL = result.ResearchWorkURL,
                StudyModality = result.StudyModality,
                GraduationDate = gradeReportBachelor == null ? DateTime.UtcNow.ToLocalDateFormat() : gradeReportBachelor.GraduationDate.ToLocalDateFormat(),
                GradeReportDegreeRequirements = gradeReportRequirements

            };
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
        /// Genera la constancia de título
        /// </summary>
        /// <param name="titlereportId">Identificador del informe de título</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("generar-constancia-titulo/{titlereportId}")]
        public async Task<IActionResult> JobTitle(Guid titlereportId)
        {

            var gradeReport = await _gradeReportService.GetGradeReportWithIncludes(titlereportId);
            var gradeReportBachelor = await _gradeReportService.GetGradeReportBachelor(gradeReport.StudentId);
            var userName = _userManager.GetUserName(User);

            var model = new JobTitleViewModel();
            {
                model.Number = gradeReport.Number;
                model.DocumentNumberBachelor = gradeReportBachelor == null ? "" : gradeReportBachelor.Number;
                model.DocumentDateBachelor = gradeReportBachelor == null ? "" : gradeReportBachelor.CreatedAt.ToDefaultTimeZone().Value.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES"));
                model.Date = DateTime.UtcNow.ToString("dddd dd 'de' MMMM, yyyy", new CultureInfo("es-ES"));
                model.DateInform = !gradeReport.CreatedAt.HasValue ? "" : gradeReport.CreatedAt.ToDefaultTimeZone().Value.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES"));
                model.YearInform = gradeReport.Date.Year.ToString();
                model.Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation();
                model.Student = gradeReport.Student.User.FullName.ToUpper();
                model.School = gradeReport.Student.Career.Name.ToUpper();
                model.Faculty = gradeReport.Student.Career.Faculty.Name.ToUpper();
                model.Specialty = gradeReport.Student.AcademicProgram is null ? "ESPECIALIDAD" : gradeReport.Student.AcademicProgram.Name;
                model.StartYear = gradeReport.Student.AdmissionTerm.Name;
                model.EndYear = gradeReport.Student.GraduationTerm is null ? "-" : gradeReport.Student.GraduationTerm.Name;
                model.RegistrationNumber = gradeReport.Student.User.UserName;
                model.Average = gradeReport.PromotionGrade;
                model.Note = Math.Round(gradeReport.PromotionGrade);
                model.StudentGender = gradeReport.Student.User.Sex;
                model.ResearchWork = string.IsNullOrEmpty(gradeReport.ResearchWork) ? "" : gradeReport.ResearchWork.ToUpper();
                model.Observations = gradeReport.Observation;
                model.HeaderText = _reportSettings.JobTitleHeaderText;
            };

            var integrated = await _configurationService.GetConfigurationByGENIntegratedSystem();
            var registryPattern = await _registryPatternService.GetRegistryPatternBasedGradeReport(gradeReport.StudentId, ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE);

            model.Modality = registryPattern.ObtainingDegreeModality;

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

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/JobTitle.cshtml", model);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/jobtitle.css");
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlContent,
                PagesCount = true,
                WebSettings = { DefaultEncoding = "UTF-8", UserStyleSheet = userStyleSheet },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 7,
                    Left="Elaborado por: " + userName + " - " + DateTime.UtcNow.ToLocalDateFormat()
                 },
            };

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Informe-de-titulo-profesional.pdf");
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileContents, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
        }

        /// <summary>
        /// Método para descargar un documento
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
