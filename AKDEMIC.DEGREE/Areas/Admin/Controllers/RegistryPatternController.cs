using AKDEMIC.CORE.Helpers;
using AKDEMIC.DEGREE.Areas.Admin.Models.RegistryPatternViewModels;
using AKDEMIC.DEGREE.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/padron-de-registro")]
    public class RegistryPatternController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IRegistryPatternService _registryPatternService;
        private readonly IFacultyService _facultyService;
        private readonly IPaymentService _paymentService;
        private readonly IStudentService _studentService;
        private readonly IForeignUniversityOriginService _foreignUniversityOriginService;
        private readonly ITermService _termService;
        private readonly IAcademicProgramService _academicProgramService;
        private readonly IProcedureService _procedureService;
        private readonly IUserProcedureService _userProcedureService;
        public RegistryPatternController(IConfigurationService configurationService,
            IDataTablesService dataTablesService,
            IRegistryPatternService registryPatternService,
            IForeignUniversityOriginService foreignUniversityOriginService,
            IFacultyService facultyService,
            ICareerService careerService,
            IPaymentService paymentService,
            IProcedureService procedureService,
            IUserProcedureService userProcedureService,
            IStudentService studentService,
            IAcademicProgramService academicProgramService,
            ITermService termService) : base(careerService, configurationService)
        {
            _dataTablesService = dataTablesService;
            _registryPatternService = registryPatternService;
            _paymentService = paymentService;
            _facultyService = facultyService;
            _studentService = studentService;
            _termService = termService;
            _foreignUniversityOriginService = foreignUniversityOriginService;
            _academicProgramService = academicProgramService;
            _procedureService = procedureService;
            _userProcedureService = userProcedureService;

        }

        /// <summary>
        /// Vista principal donde se listan los padrones de registro
        /// </summary>
        /// <returns>Retorna la vista principal</returns>
        public async Task<IActionResult> Index()
        {
            var configurationSystemIntegrated = Boolean.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM));
            return View(configurationSystemIntegrated);
        }

        /// <summary>
        /// Obtiene el listado de las escuelas profesionales
        /// </summary>
        /// <returns>Retorna un Ok con la información de las escuelas para ser usado en select</returns>
        [HttpGet("carreras/get")]
        public async Task<IActionResult> GetCareers()
        {
            var result = await _careerService.GetCareersJson(null);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de facultades
        /// </summary>
        /// <returns>Retorna un Ok con la información de las facultades para ser usado en select</returns>
        [HttpGet("facultades/get")]
        public async Task<IActionResult> GetFaculties()
        {
            var result = await _facultyService.GetFacultiesAllJson();
            return Ok(new { items = result });

        }

        /// <summary>
        /// Obtiene el listado de las escuelas profesionales en base a los siguientes parámetros
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <returns>Retorna un Ok con la información de las escuelas profesionales para ser usado en select</returns>
        [HttpGet("carreras/{facultyId}/get")]
        public async Task<IActionResult> GetCareers(Guid facultyId)
        {
            var result = await _careerService.GetCareersByFacultyIdJson(facultyId, true);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de los programas académicos en base a los siguientes parámetros
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Retorna un Ok con la información de los programas académicos para ser usado en select</returns>
        [HttpGet("programaacademicos/{careerId}/get")]
        public async Task<IActionResult> GetAcademicPrograms(Guid careerId)
        {
            var result = await _academicProgramService.GetAcademicProgramsSelect(careerId, true);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de los programas académicos en base a los siguientes parámetros
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Retorna un Ok con la información de los programas académicos para ser usado en select</returns>
        [HttpGet("programaacademicos2/{careerId}/get")]
        public async Task<IActionResult> GetAcademicPrograms2(Guid careerId)
        {
            var result = await _academicProgramService.GetAcademicProgramsSelect(careerId, false);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado periodos 
        /// </summary>
        /// <returns>Retorna un Ok con la información de los periodos académicos para ser usado en select</returns>
        [HttpGet("periodos/get")]
        public async Task<IActionResult> GetTerms()
        {
            var result = await _termService.GetSelect2Terms();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de universidades extranjeras
        /// </summary>
        /// <returns>Retorna un Ok con la información de las universidades extranjeras para ser usado en select</returns>
        [HttpGet("universidades-extranjeras/get")]
        public async Task<IActionResult> GetForeignUniversities()
        {
            var result = await _foreignUniversityOriginService.GetSelect2();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de registros de padrones en base a los siguientes parámetros
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="academicProgramId">Identificador del programa académico</param>
        /// <param name="searchBookNumber">Texto búsqueda número de libro</param>
        /// <param name="dateStartFilter">Fec. Inicio</param>
        /// <param name="dateEndFilter">Fec. Fin</param>
        /// <param name="type">Tipo</param>
        /// <param name="clasification">Clasificación</param>
        /// <returns>Retorna un Ok con la información de los registros de padrones para ser usado en tablas</returns>
        [HttpGet("lista-registro")]
        public async Task<IActionResult> GetUserProcedures(string searchValue, Guid? facultyId, Guid? careerId, Guid? academicProgramId, string searchBookNumber, string dateStartFilter, string dateEndFilter, int type, int clasification)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _registryPatternService.GetRegistryPatternDatatableByConfiguration(sentParameters, facultyId, careerId, academicProgramId, searchBookNumber, dateStartFilter, dateEndFilter, searchValue, null, null, null, type, null, null, null, clasification);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene los detalles del registro de padrón
        /// </summary>
        /// <param name="id">Identificador del registro de padrón</param>
        /// <returns>Retorna la vista</returns>
        [HttpGet("{id}/gestion")]
        public async Task<IActionResult> CreateRegistryPattern(Guid id)
        {
            var template = await _registryPatternService.GetRegistryPatternTemplate(id);
            var configuration = await _configurationService.GetValueByKey(AKDEMIC.CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.METHOD_TYPE_REGISTRY_PATTERN_CREATION);
            var result = new RegistryPatternViewModel()
            {
                MethodTypeRegistration = bool.Parse(configuration),
                Id = template.Id,
                UserName = template.UserName,
                GraduationDate = template.GraduationDate,
                StudentId = template.StudentId,
                GraduationTermId = template.GraduationTermId,
                UniversityCode = template.UniversityCode,
                BussinesSocialReason = template.BussinesSocialReason,
                RegistrationDate = template.RegistrationDate,
                RegistrationEnd = template.RegistrationEnd,
                FacultyName = template.FacultyName,
                CareerName = template.CareerName,
                PostGraduateSchool = template.PostGraduateSchool,
                PaternalSurname = template.PaternalSurname,
                MaternalSurname = template.MaternalSurname,
                StudentName = template.StudentName,
                Sex = template.Sex,
                DocumentType = template.DocumentType,
                DNI = template.DNI,
                Phone = template.Phone,
                Email1 = template.Email1,
                Email2 = template.Email2,
                BachelorOrigin = template.BachelorOrigin,
                AcademicDegree = template.AcademicDegree,
                AcademicDegreeDenomination = template.AcademicDegreeDenomination,
                Specialty = template.Specialty,
                ResearchWork = template.ResearchWork,
                Credits = template.Credits,
                ResearchWorkURL = template.ResearchWorkURL,
                DegreeProgram = template.DegreeProgram,
                PedagogicalTitleOrigin = template.PedagogicalTitleOrigin,
                ObtainingDegreeModality = template.ObtainingDegreeModality,
                StudyModality = template.StudyModality,
                GradeAbbreviation = template.GradeAbbreviation,
                OriginDegreeCountry = template.OriginDegreeCountry,
                OriginDegreeUniversity = template.OriginDegreeUniversity,
                GradDenomination = template.GradDenomination,
                ResolutionNumber = template.ResolutionNumber,
                ResolutionDateByUniversityCouncil = template.ResolutionDateByUniversityCouncil,
                OriginDiplomatDate = template.OriginDiplomatDate,
                DuplicateDiplomatDate = template.DuplicateDiplomatDate,
                DiplomatNumber = template.DiplomatNumber,
                EmissionTypeOfDiplomat = template.EmissionTypeOfDiplomat,
                BookCode = template.BookCode,
                FolioCode = template.FolioCode,
                RegistryNumber = template.RegistryNumber,
                ManagingDirector = template.ManagingDirector,
                ManagingDirectorFullName = template.ManagingDirectorFullName,
                GeneralSecretary = template.GeneralSecretary,
                GeneralSecretaryFullName = template.GeneralSecretaryFullName,
                AcademicResponsible = template.AcademicResponsible,
                AcademicResponsibleFullName = template.AcademicResponsibleFullName,
                OriginPreRequisiteDegreeCountry = template.OriginPreRequisiteDegreeCountry,
                ForeignUniversityOriginId = template.ForeignUniversityOriginId,
                OriginPreRequisiteDegreeDenomination = template.OriginPreRequisiteDegreeDenomination,
                OfficeNumber = template.OfficeNumber,
                DateEnrollmentProgram = template.DateEnrollmentProgram,
                StartDateEnrollmentProgram = template.StartDateEnrollmentProgram,
                EndDateEnrollmentProgram = template.EndDateEnrollmentProgram,
                AcademicProgram = template.AcademicProgram,
                Relation = template.Relation,
                SpecialtyMention = template.SpecialtyMention,
                UniversityCouncilType = template.UniversityCouncilType,
                FacultyCouncilDate = template.FacultyCouncilDate,
                UniversityCouncilDate = template.UniversityCouncilDate,
                Initial = template.Initial,
                Correlative = template.Correlative,
                Year = template.Year,
                BookTitle = template.BookTitle,
                AcademicProgramId = template.AcademicProgramId,
                CareerId = template.CareerId
            };

            if (string.IsNullOrEmpty(template.BookCode) && string.IsNullOrEmpty(template.FolioCode) && string.IsNullOrEmpty(template.RegistryNumber))
            {
                var tem = await _registryPatternService.GetRegistryPatternBook();
                template.BookCode = tem.BookCode;
                template.FolioCode = tem.FolioCode;
                template.RegistryNumber = tem.RegistryNumber;
            }
            return View(result);

        }

        /// <summary>
        /// Método para editar el padrón de registro
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del padrón</param>
        /// <returns>Retorna un OK o BadRequest</returns>
        [HttpPost("editar")]
        public async Task<IActionResult> EditRegistryPattern(RegistryPatternViewModel model)
        {
            var student = await _studentService.GetWithIncludes(model.StudentId);
            var configuration = await _configurationService.GetValueByKey(AKDEMIC.CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.METHOD_TYPE_REGISTRY_PATTERN_CREATION);
            var registryPattern = await _registryPatternService.GetWithIncludes(model.Id);
            var institutionGradeCode = await _configurationService.GetByKey(CORE.Helpers.ConstantHelpers.Configuration.General.INSTITUTION_GRADE_CODE);


            registryPattern.UniversityCode = institutionGradeCode != null ? institutionGradeCode.Value : "0000";
            registryPattern.BussinesSocialReason = model.BussinesSocialReason;
            registryPattern.Student.AcademicProgramId = model.AcademicProgramId;
            registryPattern.BachelorOrigin = model.BachelorOrigin;

            if (bool.Parse(configuration))
            {
                registryPattern.ResearchWork = model.ResearchWork;
                registryPattern.GraduationDate = !string.IsNullOrWhiteSpace(model.GraduationDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.GraduationDate) : (DateTime?)null;

            }

            registryPattern.Specialty = model.Specialty;


            registryPattern.DegreeProgram = ConstantHelpers.PROGRAM_STUDIES.VALUES[model.DegreeProgram];
            registryPattern.PedagogicalTitleOrigin = model.PedagogicalTitleOrigin;
            registryPattern.ObtainingDegreeModality = model.ObtainingDegreeModality;
            registryPattern.StudyModality = model.StudyModality;

            registryPattern.BookTitle = model.BookTitle;

            registryPattern.OriginDegreeCountry = model.OriginDegreeCountry;
            registryPattern.OriginDegreeUniversity = model.OriginDegreeUniversity;


            registryPattern.ResolutionNumber = model.ResolutionNumber;
            registryPattern.ResolutionDateByUniversityCouncil = !string.IsNullOrWhiteSpace(model.ResolutionDateByUniversityCouncil) ? ConvertHelpers.DatepickerToUtcDateTime(model.ResolutionDateByUniversityCouncil) : (DateTime?)null;
            registryPattern.OriginDiplomatDate = !string.IsNullOrWhiteSpace(model.OriginDiplomatDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.OriginDiplomatDate) : (DateTime?)null;
            registryPattern.DuplicateDiplomatDate = !string.IsNullOrWhiteSpace(model.DuplicateDiplomatDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.DuplicateDiplomatDate) : (DateTime?)null;
            registryPattern.DiplomatNumber = model.DiplomatNumber;
            registryPattern.EmissionTypeOfDiplomat = model.EmissionTypeOfDiplomat;

            ///Libro - registro - folio    
            registryPattern.BookCode = model.BookCode;
            registryPattern.FolioCode = model.FolioCode;
            registryPattern.RegistryNumber = model.RegistryNumber;


            registryPattern.OriginPreRequisiteDegreeCountry = model.OriginPreRequisiteDegreeCountry;
            registryPattern.ForeignUniversityOriginId = model.ForeignUniversityOriginId;
            registryPattern.OriginPreRequisiteDegreeDenomination = model.OriginPreRequisiteDegreeDenomination;
            //registryPattern.OfficeNumber = model.OfficeNumber;
            registryPattern.DateEnrollmentProgram = !string.IsNullOrWhiteSpace(model.DateEnrollmentProgram) ? ConvertHelpers.DatepickerToUtcDateTime(model.DateEnrollmentProgram) : (DateTime?)null;
            registryPattern.StartDateEnrollmentProgram = !string.IsNullOrWhiteSpace(model.StartDateEnrollmentProgram) ? ConvertHelpers.DatepickerToUtcDateTime(model.StartDateEnrollmentProgram) : (DateTime?)null;
            registryPattern.EndDateEnrollmentProgram = !string.IsNullOrWhiteSpace(model.EndDateEnrollmentProgram) ? ConvertHelpers.DatepickerToUtcDateTime(model.EndDateEnrollmentProgram) : (DateTime?)null;
            registryPattern.AcademicProgram = model.AcademicProgram;
            registryPattern.Relation = model.Relation;
            registryPattern.SpecialtyMention = model.SpecialtyMention;
            registryPattern.UniversityCouncilType = model.UniversityCouncilType;
            registryPattern.FacultyCouncilDate = !string.IsNullOrWhiteSpace(model.FacultyCouncilDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.FacultyCouncilDate) : (DateTime?)null;
            registryPattern.UniversityCouncilDate = !string.IsNullOrWhiteSpace(model.UniversityCouncilDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.UniversityCouncilDate) : (DateTime?)null;
            registryPattern.Initial = model.Initial;
            registryPattern.Correlative = model.Correlative;
            registryPattern.SineaceAcreditation = model.SineaceAcreditation;
            registryPattern.SineaceAcreditationStartDate = !string.IsNullOrWhiteSpace(model.SineaceAcreditationStartDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.SineaceAcreditationStartDate) : (DateTime?)null;
            registryPattern.SineaceAcreditationEndDate = !string.IsNullOrWhiteSpace(model.SineaceAcreditationEndDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.SineaceAcreditationEndDate) : (DateTime?)null;
            registryPattern.SineaceAcreditationDegreeModalityStartDate = !string.IsNullOrWhiteSpace(model.SineaceAcreditationDegreeModalityStartDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.SineaceAcreditationDegreeModalityStartDate) : (DateTime?)null;
            registryPattern.SineaceAcreditationDegreeModalityEndDate = !string.IsNullOrWhiteSpace(model.SineaceAcreditationDegreeModalityEndDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.SineaceAcreditationDegreeModalityEndDate) : (DateTime?)null;
            registryPattern.ProcessDegreeDate = !string.IsNullOrWhiteSpace(model.ProcessDegreeDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.ProcessDegreeDate) : (DateTime?)null;
            registryPattern.DegreeSustentationDate = !string.IsNullOrWhiteSpace(model.DegreeSustentationDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.DegreeSustentationDate) : (DateTime?)null;
            registryPattern.IsOriginal = model.IsOriginal;
            registryPattern.ComplianceRevalidationCriteria = model.ComplianceRevalidationCriteria;
            registryPattern.SustainabilityMode = model.SustainabilityMode;

            //registryPattern.Year = model.Year;
            if (model.ForeignUniversityOriginId.HasValue)
            {
                registryPattern.ForeignUniversityOriginId = model.ForeignUniversityOriginId;
            }



            var userProcedure = new UserProcedure();
            if (registryPattern.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
            {
                userProcedure = await _userProcedureService.GetUserProcedureByStaticType(student.UserId, ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR_DEGREE_APPLICATION);
            }
            else
            {
                userProcedure = await _userProcedureService.GetUserProcedureByStaticType(student.UserId, ConstantHelpers.PROCEDURES.STATIC_TYPE.TITLE_DEGREE_APPLICATION);
            }

            if (userProcedure != null)
            {
                if (userProcedure.Status == ConstantHelpers.USER_PROCEDURES.STATUS.PENDING)
                {
                    userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS;
                }
            }

            await _registryPatternService.Update(registryPattern);
            return Ok();

        }

        /// <summary>
        /// Método para generar el reporte que contiene los detalles de los registros de padrón en base a los siguietnes parámetros
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="academicProgramId">Identificador del programa académico</param>
        /// <param name="dni">Dni</param>
        /// <param name="searchBookNumber">Texto búsqueda número libro</param>
        /// <param name="dateStartFilter">Fec Inicio</param>
        /// <param name="dateEndFilter">Fec. Fin</param>
        /// <param name="type">Tipo</param>
        /// <param name="clasification">Clasificación</param>
        /// <returns>Retorna un archivo en formato EXCEL</returns>
        [HttpGet("reporte-excel")]
        public async Task<IActionResult> GetExcel(Guid? facultyId, Guid? careerId, Guid? academicProgramId, string dni = null, string searchBookNumber = null, string dateStartFilter = null, string dateEndFilter = null, int? type = null, int? clasification = null)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte");
                try
                {
                    await _registryPatternService.DownloadExcel(worksheet, facultyId, careerId, academicProgramId, dni, searchBookNumber, dateStartFilter, dateEndFilter, type, clasification);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                worksheet.RangeUsed().SetAutoFilter();

                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";


                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Padron de registro.xlsx");
                }
            }
        }

        /// <summary>
        /// Método para generar el reporte para SUNEDU que contiene los detalles de los registros de padrón en base a los siguietnes parámetros
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="academicProgramId">Identificador del programa académico</param>
        /// <param name="dni">Dni</param>
        /// <param name="searchBookNumber">Texto búsqueda número libro</param>
        /// <param name="dateStartFilter">Fec Inicio</param>
        /// <param name="dateEndFilter">Fec. Fin</param>
        /// <param name="type">Tipo</param>
        /// <param name="clasification">Clasificación</param>
        /// <returns>Retorna un archivo en formato EXCEL</returns>
        [HttpGet("reporte-excel-sunedu")]
        public async Task<IActionResult> GetExcelSunedu(Guid? facultyId, Guid? careerId, Guid? academicProgramId, string dni = null, string searchBookNumber = null, string dateStartFilter = null, string dateEndFilter = null, int? type = null, int? clasification = null)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte Sunedu");
                try
                {
                    await _registryPatternService.DownloadExcel(worksheet, facultyId, careerId, academicProgramId, dni, searchBookNumber, dateStartFilter, dateEndFilter, type, clasification, true);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                worksheet.RangeUsed().SetAutoFilter();

                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";


                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Padron de registro sunedu.xlsx");
                }
            }
        }

        /// <summary>
        /// Obtiene el listado de padrones generados
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Retorna un Ok con la información de los padrones generados para ser usado en tablas</returns>
        [HttpGet("padrones-generados/{facultyId?}/{careerId?}")]
        public async Task<IActionResult> ProcedureGenerate(string searchValue, Guid? facultyId, Guid? careerId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _registryPatternService.UserProceduresGenerateByRegistryPattern(sentParameters, facultyId, careerId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de los conceptos asignados para los padrones generados
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Retorna un Ok con la información para ser usado en tablas</returns>
        [HttpGet("padrones-generados-conceptos/{facultyId?}/{careerId?}")]
        public async Task<IActionResult> ProcedureGenerateConcepts(string searchValue, Guid? facultyId, Guid? careerId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _paymentService.ConceptGenerateByRegistryPattern(sentParameters, facultyId, careerId);
            return Ok(result);
        }

        //private string GetConfigurationValue(Dictionary<string, string> list, string key)
        //{
        //    return list.ContainsKey(key) ? list[key] :

        //        CORE.Helpers.ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES.ContainsKey(key) ?
        //        CORE.Helpers.ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES[key] :

        //        CORE.Helpers.ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES.ContainsKey(key) ?
        //        CORE.Helpers.ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[key] :

        //        CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.DEFAULT_VALUES.ContainsKey(key) ?
        //        CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.DEFAULT_VALUES[key] :

        //        CORE.Helpers.ConstantHelpers.Configuration.General.DEFAULT_VALUES[key];
        //}



        //#region JOBS
        //[HttpGet("job-padrones")]
        //public async Task<IActionResult> GenerateRegistryPaternsJob()
        //{
        //    var regitryPatterns = await _context.RegistryPatterns.Include(x => x.Student.User).ToListAsync();
        //    var userProcedures = await _context.UserProcedures.Include(x => x.Procedure.ProcedureSubcategory).Where(x => x.Procedure.ProcedureCategory.StaticType == ConstantHelpers.PROCEDURE_CATEGORIES.STATIC_TYPE.DEGREES_AND_TITLES).ToListAsync();
        //    var counter = 0;
        //    foreach (var registry in regitryPatterns)
        //    {
        //        foreach (var userProcedure in userProcedures)
        //        {
        //            switch (userProcedure.Procedure.ProcedureSubcategory.Name)
        //            {
        //                case "BACHILLER":
        //                    if (userProcedure.UserId == registry.Student.UserId)
        //                    {
        //                        registry.UserProcedureId = userProcedure.Id;
        //                    }
        //                    break;
        //                case "TITULADO":
        //                    if (userProcedure.UserId == registry.Student.UserId)
        //                    {
        //                        registry.UserProcedureId = userProcedure.Id;
        //                    }
        //                    break;
        //            }
        //            if (counter % 25 == 0)
        //            {
        //                await _context.SaveChangesAsync();
        //            }
        //            counter++;
        //        }
        //    }
        //    await _context.SaveChangesAsync();
        //    return Ok("Done");

        //}

        //[HttpGet("job-user-procedures")]
        //public async Task<IActionResult> GenerateUserProcedures()
        //{
        //    var registryPatterns = await _context.RegistryPatterns.Include(x => x.Student.User).ToListAsync();
        //    var procedureBachellor = await _context.Procedures.Where(x => x.ProcedureCategory.StaticType == ConstantHelpers.PROCEDURE_CATEGORIES.STATIC_TYPE.DEGREES_AND_TITLES && x.ProcedureSubcategory.Name == "BACHILLER").FirstOrDefaultAsync();
        //    var procedureTitle = await _context.Procedures.Where(x => x.ProcedureCategory.StaticType == ConstantHelpers.PROCEDURE_CATEGORIES.STATIC_TYPE.DEGREES_AND_TITLES && x.ProcedureSubcategory.Name == "TITULO PROFESIONAL").FirstOrDefaultAsync();
        //    var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
        //    var lstUserProcedures = new List<UserProcedure>();
        //    foreach (var registry in registryPatterns)
        //    {
        //        var student = await _context.Students.Include(x => x.User).Where(x => x.Id == registry.StudentId).FirstOrDefaultAsync();
        //        switch (registry.GradDenomination)
        //        {
        //            case "BACHILLER":
        //                var userProcedureBachellor = new UserProcedure()
        //                {
        //                    DNI = student.User.Dni,
        //                    ProcedureId = procedureBachellor.Id,
        //                    UserId = student.UserId,
        //                    Status = ConstantHelpers.USER_PROCEDURES.STATUS.ACCEPTED,
        //                    TermId = term.Id
        //                };
        //                lstUserProcedures.Add(userProcedureBachellor);

        //                break;
        //            case "TITULO":
        //                var userProcedureTitle = new UserProcedure()
        //                {
        //                    DNI = student.User.Dni,
        //                    ProcedureId = procedureTitle.Id,
        //                    UserId = student.UserId,
        //                    Status = ConstantHelpers.USER_PROCEDURES.STATUS.ACCEPTED,
        //                    TermId = term.Id
        //                };
        //                lstUserProcedures.Add(userProcedureTitle);
        //                break;
        //        }

        //    }
        //    await _context.UserProcedures.AddRangeAsync(lstUserProcedures);
        //    await _context.SaveChangesAsync();
        //    return Ok("Done");

        //}

        //[HttpGet("distinct_patterns")]
        //public async Task<IActionResult> RegistryPatternsChange()
        //{
        //    var registryPatterns = await _context.RegistryPatterns.Include(x => x.Student.User).Include(x => x.UserProcedure).ToListAsync();
        //    foreach (var item in registryPatterns)
        //    {
        //        switch (item.GradDenomination)
        //        {
        //            case "BACHILLER":
        //                var userProcedureBachellor = await _context.UserProcedures
        //                .Where(x => x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.ACCEPTED && x.Procedure.ProcedureCategory.StaticType == 1 && x.Procedure.ProcedureSubcategory.StaticType == 1 && x.User.UserRoles.Any(s => s.Role.Name == "Alumnos") && x.UserId == item.Student.UserId).FirstOrDefaultAsync();
        //                if (item.UserProcedureId != userProcedureBachellor.Id)
        //                {
        //                    item.UserProcedureId = userProcedureBachellor.Id;
        //                    await _context.SaveChangesAsync();
        //                }
        //                else
        //                {
        //                    continue;
        //                }
        //                break;
        //            case "TITULO":
        //                var userProcedureTitle = await _context.UserProcedures
        //                 .Where(x => x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.ACCEPTED && x.Procedure.ProcedureCategory.StaticType == 1 && x.Procedure.ProcedureSubcategory.StaticType == 2 && x.User.UserRoles.Any(s => s.Role.Name == "Alumnos") && x.UserId == item.Student.UserId).FirstOrDefaultAsync();
        //                if (item.UserProcedureId != userProcedureTitle.Id)
        //                {
        //                    item.UserProcedureId = userProcedureTitle.Id;
        //                    await _context.SaveChangesAsync();
        //                }
        //                else
        //                {
        //                    continue;
        //                }
        //                break;

        //        }

        //    }
        //    return Ok("Done");

        //}

        //[HttpGet("crear_payments")]
        //public async Task<IActionResult> CreatePayment()
        //{
        //    var student = await _context.Students.FirstOrDefaultAsync();
        //    //var aaa = new Guid("79429DEF-88D1-491B-99E0-0007F0EA8182");
        //    //var student = await _context.Students.Where(x=>x.Id == aaa).FirstOrDefaultAsync();
        //    var configurationBachelorType = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_TYPE_BACHELOR).FirstOrDefaultAsync();
        //    var configurationBachelor = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_BACHELOR).FirstOrDefaultAsync();
        //    var configurationTitleDegreeProfesionalExperience = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE).FirstOrDefaultAsync();
        //    var configurationTitleDegreeSuffiencyExam = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM).FirstOrDefaultAsync();
        //    var configurationTitleDegreeSupportTesis = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUPPORT_TESIS).FirstOrDefaultAsync();

        //    var guidBachelor = new Guid(configurationBachelor.Value);
        //    var guidConceptTitleDegreeProffesionalExperience = new Guid(configurationTitleDegreeProfesionalExperience.Value);
        //    var guidConceptTitleDegreeSufficiencyExam = new Guid(configurationTitleDegreeSuffiencyExam.Value);
        //    var guidConceptTitleDegreeSupportTesis = new Guid(configurationTitleDegreeSupportTesis.Value);


        //    var conceptBachelor = await _context.Concepts.Where(x => x.Id == guidBachelor).FirstOrDefaultAsync();
        //    var conceptTitleDegreeProffesionalExperience = await _context.Concepts.Where(x => x.Id == guidConceptTitleDegreeProffesionalExperience).FirstOrDefaultAsync();
        //    var conceptTitleDegreeSuffiencyExam = await _context.Concepts.Where(x => x.Id == guidConceptTitleDegreeSufficiencyExam).FirstOrDefaultAsync();
        //    var conceptTitleDegreeSupportTesis = await _context.Concepts.Where(x => x.Id == guidConceptTitleDegreeSupportTesis).FirstOrDefaultAsync();

        //    if (conceptTitleDegreeProffesionalExperience != null)
        //    {
        //        var payment = new Payment
        //        {
        //            Description = conceptTitleDegreeProffesionalExperience.Description,
        //            SubTotal = conceptTitleDegreeProffesionalExperience.Amount * 1.00M,
        //            IgvAmount = conceptTitleDegreeProffesionalExperience.Amount * CORE.Helpers.ConstantHelpers.Treasury.IGV,
        //            Discount = 0.00M,
        //            Total = conceptTitleDegreeProffesionalExperience.Amount + conceptTitleDegreeProffesionalExperience.Amount * CORE.Helpers.ConstantHelpers.Treasury.IGV,
        //            Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.DEGREE,
        //            UserId = student.UserId,
        //            Status = CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PAID,
        //            ConceptId = conceptTitleDegreeProffesionalExperience.Id
        //        };

        //        await _context.Payments.AddAsync(payment);
        //        await _context.SaveChangesAsync();
        //    }
        //    if (conceptTitleDegreeSuffiencyExam != null)
        //    {
        //        var payment = new Payment
        //        {
        //            Description = conceptTitleDegreeSuffiencyExam.Description,
        //            SubTotal = conceptTitleDegreeSuffiencyExam.Amount * 1.00M,
        //            IgvAmount = conceptTitleDegreeSuffiencyExam.Amount * CORE.Helpers.ConstantHelpers.Treasury.IGV,
        //            Discount = 0.00M,
        //            Total = conceptTitleDegreeSuffiencyExam.Amount + conceptTitleDegreeSuffiencyExam.Amount * CORE.Helpers.ConstantHelpers.Treasury.IGV,
        //            Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.DEGREE,
        //            UserId = student.UserId,
        //            Status = CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PAID,
        //            ConceptId = conceptTitleDegreeSuffiencyExam.Id
        //        };

        //        await _context.Payments.AddAsync(payment);
        //        await _context.SaveChangesAsync();
        //    }
        //    if (conceptTitleDegreeSupportTesis != null)
        //    {
        //        var payment = new Payment
        //        {
        //            Description = conceptTitleDegreeSupportTesis.Description,
        //            SubTotal = conceptTitleDegreeSupportTesis.Amount * 1.00M,
        //            IgvAmount = conceptTitleDegreeSupportTesis.Amount * CORE.Helpers.ConstantHelpers.Treasury.IGV,
        //            Discount = 0.00M,
        //            Total = conceptTitleDegreeSupportTesis.Amount + conceptTitleDegreeSupportTesis.Amount * CORE.Helpers.ConstantHelpers.Treasury.IGV,
        //            Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.DEGREE,
        //            UserId = student.UserId,
        //            Status = CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PAID,
        //            ConceptId = conceptTitleDegreeSupportTesis.Id
        //        };

        //        await _context.Payments.AddAsync(payment);
        //        await _context.SaveChangesAsync();
        //    }

        //    if (conceptBachelor != null)
        //    {
        //        var payment = new Payment
        //        {
        //            Description = conceptBachelor.Description,
        //            SubTotal = conceptBachelor.Amount * 1.00M,
        //            IgvAmount = conceptBachelor.Amount * CORE.Helpers.ConstantHelpers.Treasury.IGV,
        //            Discount = 0.00M,
        //            Total = conceptBachelor.Amount + conceptBachelor.Amount * CORE.Helpers.ConstantHelpers.Treasury.IGV,
        //            Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.DEGREE,
        //            UserId = student.UserId,
        //            Status = CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PAID,
        //            ConceptId = conceptBachelor.Id
        //        };

        //        await _context.Payments.AddAsync(payment);
        //        await _context.SaveChangesAsync();
        //    }



        //    return Ok("listo");
        //}
        //#endregion

    }
}
