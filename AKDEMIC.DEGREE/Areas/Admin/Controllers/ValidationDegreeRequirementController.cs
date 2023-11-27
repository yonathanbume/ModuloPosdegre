using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DEGREE.Areas.Admin.Models.ValidationDegreeRequirementViewModels;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/solicitud-grado-por-requisitos")]
    public class ValidationDegreeRequirementController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IDegreeRequirementService _degreeRequirementService;
        private readonly IGradeReportRequirementService _gradeReportRequirementService;
        private readonly IConfigurationService _configurationService;
        private readonly IFacultyService _facultyService;
        private readonly IUniversityAuthorityService _universityAuthorityService;
        private readonly ITermService _termService;
        private readonly IGradeReportService _gradeReportService;
        private readonly IRegistryPatternService _registryPatternService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IProcedureService _procedureService;
        private readonly IUserProcedureService _userProcedureService;

        public ValidationDegreeRequirementController(IStudentService studentService,
             IDegreeRequirementService degreeRequirementService, IGradeReportRequirementService gradeReportRequirementService,
             IConfigurationService configurationService,
             IFacultyService facultyService,
             IUniversityAuthorityService universityAuthorityService,
             ITermService termService,
             IProcedureService procedureService,
             IUserProcedureService userProcedureService,
             IRegistryPatternService registryPatternService,
             IOptions<CloudStorageCredentials> storageCredentials,
             IGradeReportService gradeReportService)
        {
            _studentService = studentService;
            _degreeRequirementService = degreeRequirementService;
            _gradeReportRequirementService = gradeReportRequirementService;
            _configurationService = configurationService;
            _universityAuthorityService = universityAuthorityService;
            _termService = termService;
            _facultyService = facultyService;
            _gradeReportService = gradeReportService;
            _registryPatternService = registryPatternService;
            _storageCredentials = storageCredentials;
            _procedureService = procedureService;
            _userProcedureService = userProcedureService;
        }

        /// <summary>
        /// Vista principal para la solicitud de grado por requisitos
        /// </summary>
        /// <returns>Retorna la vista</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Retorna la vista del informe de grado
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <param name="gradeType">Tipo de Gado</param>
        /// <returns>Retorna la Vista</returns>
        [HttpGet("informe/{gradeType}/{studentId}")]
        public async Task<IActionResult> Inform(Guid studentId, int gradeType)
        {
            var degreeRequirements = await _degreeRequirementService.GetDegreeRequirementsByType(gradeType);
            var configuration = await _configurationService.GetValueByKey(AKDEMIC.CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.METHOD_TYPE_REGISTRY_PATTERN_CREATION);
            var studentInfo = await _gradeReportService.GetStudentByStudentId(studentId);
            var credits = await _studentService.GetApprovedCreditsByStudentId(studentId);

            var result = new ResultViewModel
            {
                DegreeRequirements = degreeRequirements,
                Value = configuration,
                StudentId = studentId,
                GradeType = gradeType,
                Approvedcredits = credits.ToString(),
                FullName = studentInfo.FullName,
                ProfessionalSchool = studentInfo.Career
            };
            return View(result);
        }

        /// <summary>
        /// Método para buscar a un alumno por nombre de usuario o nombre completo
        /// </summary>
        /// <param name="term">Texto de búsqueda</param>
        /// <returns>Retorna un Ok con los datos del estudiante para ser usado en select</returns>
        [HttpGet("buscar")]
        public async Task<IActionResult> Search(string term, int degreeType = -1)
        {
            //var students = await _studentService.SearchStudentByTerm(term, null, User);
            int? degreeTypeValue = null;
            if (degreeType != -1)
                degreeTypeValue = degreeType;
            var students = await _studentService.SearchStudentForDegreeByTerm(term, degreeTypeValue, null, User);
            return Ok(new { items = students });
        }


        /// <summary>
        /// Mëotodo para registrar un informe de grado
        /// </summary>
        /// <param name="model">Objeto que contiene los datos para el nuevo informe de grado</param>
        /// <returns>Retorna un OK o BadRequest</returns>
        [HttpPost("registrar-informe-grado")]
        public async Task<IActionResult> AddGradeReport(GradeReportViewModel model)
        {
            string degreeModality = "";
            var configurationBachelor = new Configuration();
            var student = await _studentService.GetWithIncludes(model.StudentId);
            var deanFaculty = await _facultyService.GetDeanById(student.Career.FacultyId);
            var universityAuthorities = await _universityAuthorityService.GetUniversityAuthoritiesList();
            var credits = await _studentService.GetApprovedCreditsByStudentId(student.Id);

            var graduationTerm = new Term();

            var result = await _configurationService.GetConfigurationByGENIntegratedSystem();

            if (model.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
            {
                if (await _gradeReportService.ExistGradeReport(model.StudentId, ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR))
                {
                    return BadRequest("Ya existe un informe de grado bachiller creado para el estudiante seleccionado");
                }
            }
            else
            {
                if (await _gradeReportService.ExistGradeReport(model.StudentId, ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE))
                {
                    return BadRequest("Ya existe un informe de grado Titulo Profesional creado para el estudiante seleccionado");
                }
            }

            //var record = student.RecordHistories.Where(x => x.Type == model.GradeType).FirstOrDefault();
            var entity = new GradeReport
            {
                Date = DateTime.UtcNow.Date,
                StudentId = model.StudentId,
                Year = DateTime.UtcNow.Year,
                GradeType = model.GradeType
            };

            degreeModality = CORE.Helpers.ConstantHelpers.DEGREE_OBTENTION_MOD.AUTOMATIC_BACHELOR;

            var entityRegistryPattern = new RegistryPattern();

            var cultureInfo = new CultureInfo("es-Es");
            var myTI = cultureInfo.TextInfo;
            //entityRegistryPattern.GraduationDate = ConvertHelpers.DatepickerToUtcDateTime(model.GraduationDate);
            entityRegistryPattern.StudentId = model.StudentId;

            if (model.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
                entityRegistryPattern.GradeType = ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR;
            else
                entityRegistryPattern.GradeType = ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE;

            var institutionGradeCode = await _configurationService.GetByKey(CORE.Helpers.ConstantHelpers.Configuration.General.INSTITUTION_GRADE_CODE);
            entityRegistryPattern.DocumentType = student.User.DocumentType;
            entityRegistryPattern.BussinesSocialReason = GeneralHelpers.GetInstitutionName();
            entityRegistryPattern.UniversityCode = institutionGradeCode != null ? institutionGradeCode.Value : "00000";
            entityRegistryPattern.AcademicProgram = student.Career.Name;
            entityRegistryPattern.GradeAbbreviation = ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR == model.GradeType ? "B" : "T";
            entityRegistryPattern.ObtainingDegreeModality = ConstantHelpers.DEGREE_OBTENTION_MOD.VALUES[degreeModality];
            if (model.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
            {
                if (string.IsNullOrEmpty(student.Curriculum.AcademicDegreeBachelor))
                    return BadRequest("No se ha definido el grado académico en el plan de estudios del estudiante seleccionado");

                entityRegistryPattern.AcademicDegreeDenomination = student.Curriculum.AcademicDegreeBachelor;
            }
            else
            {
                if (string.IsNullOrEmpty(student.Curriculum.AcademicDegreeProfessionalTitle))
                    return BadRequest("No se ha definido el grado académico en el plan de estudios del estudiante seleccionado");

                entityRegistryPattern.AcademicDegreeDenomination = student.Curriculum.AcademicDegreeProfessionalTitle;
            }
            entityRegistryPattern.Credits = credits;
            //entityRegistryPattern.BachelorOrigin = model.BachelorOrigin;
            //entityRegistryPattern.OriginDegreeCountry = model.OriginDegreeCountry;
            //entityRegistryPattern.PedagogicalTitleOrigin = model.PedagogicalTitleOrigin;
            //entityRegistryPattern.ResearchWork = model.ResearchWork;
            //entityRegistryPattern.ResearchWorkURL = model.ResearchWorkURL;
            //entityRegistryPattern.StudyModality = model.StudyModality;
            entityRegistryPattern.AcademicResponsible = "DECANO";
            entityRegistryPattern.GeneralSecretary = "SECRETARIO GENERAL";
            entityRegistryPattern.ManagingDirector = "RECTOR";


            if (model.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
            {
                entityRegistryPattern.AcademicDegreeDenomination = student.Curriculum.AcademicDegreeBachelor;
                entityRegistryPattern.RegistryNumber = $"{(await _registryPatternService.CurrentCountByGradeType(ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR) + 1).ToString():000}";
            }
            else
            {
                entityRegistryPattern.AcademicDegreeDenomination = student.Curriculum.AcademicDegreeProfessionalTitle;
                entityRegistryPattern.RegistryNumber = $"{(await _registryPatternService.CurrentCountByGradeType(ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE) + 1).ToString():000}";
            }

            //var positionGrade = ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR == model.GradeType ? "Bachiller" : "Titulado";
            //entityRegistryPattern.AcademicDegreeDenomination = $"{positionGrade} en " + myTI.ToTitleCase(student.AcademicProgram.Name.ToLower());

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
                    entityRegistryPattern.GeneralSecretaryFullName = generalSecretary.User.Name;
                }
                if (rector != null)
                {
                    entityRegistryPattern.ManagingDirectorFullName = rector.User.Name;
                }

            }
            if (deanFaculty != null)
            {
                entityRegistryPattern.AcademicResponsibleFullName = deanFaculty;
            }
            //userInternalProcedure.Status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.GENERATED;
            //await _userInternalProcedureService.Update(userInternalProcedure);

            //solicitud de grados bachilleres o titulados
            var currentTerm = await _termService.GetActiveTerm();

            if (model.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
            {
                var procedureBachelor = await _procedureService.GetByStaticType(ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR_DEGREE_APPLICATION);

                var userProcedure = new UserProcedure()
                {
                    UserId = student.UserId,
                    ProcedureId = procedureBachelor.Id,
                    TermId = currentTerm.Id,
                    DNI = student.User.Dni,
                    Status = ConstantHelpers.USER_PROCEDURES.STATUS.PENDING

                };
                await _userProcedureService.Insert(userProcedure);

            }
            else
            {
                var procedureTitle = await _procedureService.GetByStaticType(ConstantHelpers.PROCEDURES.STATIC_TYPE.TITLE_DEGREE_APPLICATION);

                var userProcedure = new UserProcedure()
                {
                    UserId = student.UserId,
                    ProcedureId = procedureTitle.Id,
                    TermId = currentTerm.Id,
                    DNI = student.User.Dni,
                    Status = ConstantHelpers.USER_PROCEDURES.STATUS.PENDING

                };
                await _userProcedureService.Insert(userProcedure);

            }


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
                                Description = documentFile.Observation

                            });
                        }
                        catch (Exception)
                        {
                            return BadRequest($"Hubo un problema al subir el archivo '{documentFile.DocumentFile.FileName}'");
                        }
                    }
                    entity.GradeReportRequirements = gradeReportRequirements;
                }
            }

            await _gradeReportService.Insert(entity);
            await _registryPatternService.Insert(entityRegistryPattern);

            return Ok();

        }

        //[HttpGet("informacion-alumno")]
        //public async Task<IActionResult> GetStudentInformation(Guid studentId)
        //{
        //    var student = await _studentService.GetWithData(studentId);
        //    var result = new
        //    {
        //        Career = student.Career.Name,
        //        Email = student.User.Email,
        //        Dni = student.User.Dni,
        //        FullName = student.User.FullName
        //    };
        //    return Ok(result);
        //}
    }
}
