using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using AKDEMIC.SERVICE.Services.Enrollment.Implementations;

namespace AKDEMIC.INTRANET.Controllers
{
    [AllowAnonymous]
    public class JsonController : BaseController
    {
        private readonly ICourseService _courseService;
        private readonly ICourseTermService _courseTermService;
        private readonly ICurriculumService _curriculumService;
        private readonly IEvaluationTypeService _evaluationTypeService;
        private readonly AkdemicContext _context;
        private readonly ISelect2Service _select2Service;
        private readonly IFacultyService _facultyService;
        private readonly ICareerService _careerService;
        private readonly IAcademicProgramService _academicProgramService;
        private readonly IAdmissionTypeService _admissionTypeService;
        private readonly ISubstituteExamService _substituteExamService;
        private readonly IPsychologicalDiagnosticService _psychologicalDiagnosticService;
        private readonly ITeacherService _teacherService;
        private readonly IUserService _userService;
        private readonly IAreaService _areaService;
        private readonly ICampusService _campusService;
        private readonly IBuildingService _buildingService;
        private readonly IClassroomService _classroomService;
        private readonly IRoleService _roleService;
        private readonly IDependencyService _dependencyService;
        private readonly IStudentService _studentService;
        private readonly ISectionService _sectionService;
        private readonly ITutorialStudentService _tutorialStudentService;
        private readonly IConfigurationService _configurationService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IClassStudentService _classStudentService;
        private readonly IAcademicYearCourseService _academicYearCourseService;
        private readonly IDepartmentService _departmentService;
        private readonly IProvinceService _provinceService;
        private readonly IDistrictService _districtService;
        private readonly ICourseUnitService _courseUnitService;
        private readonly IWorkingDayService _workingDayService;
        private readonly IDeanService _deanService;
        private readonly IExtracurricularActivityService _extracurricularActivityService;
        private readonly IExtracurricularCourseService _extracurricularCourseService;
        private readonly IExtracurricularAreaService _extracurricularAreaService;
        private readonly IAcademicDepartmentService _academicDepartmentService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly ITeacherSectionService _teacherSectionService;
        private readonly IConceptService _conceptService;
        private readonly IUserProcedureDerivationFileService _userProcedureDerivationFileService;
        private readonly IUserProcedureDerivationService _userProcedureDerivationService;
        private readonly IUserProcedureService _userProcedureService;
        private readonly IProcedureDependencyService _procedureDependencyService;
        private readonly IDataTablesService _dataTablesService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IAcademicRecordDepartmentService _academicRecordDepartmentService;
        private readonly IGradeService _gradeService;
        private readonly ISectionGroupService _sectionGroupService;
        private readonly ICourseCertificateService _courseCertificateService;
        private readonly IStudentScaleService _studentScaleService;
        private readonly IEnrollmentFeeService _enrollmentFeeService;

        public JsonController(UserManager<ApplicationUser> userManager,
            IConceptService conceptService,
            ITermService termService,
            ICurriculumService curriculumService,
            IEvaluationTypeService evaluationTypeService,
            AkdemicContext context
            , ICourseService courseService,
            ISelect2Service select2Service,
            IFacultyService facultyService,
            ICareerService careerService,
            IAcademicProgramService academicProgramService,
            IAdmissionTypeService admissionTypeService,
            ISubstituteExamService substituteExamService,
            IPsychologicalDiagnosticService psychologicalDiagnosticService,
            ITeacherService teacherService,
            IUserService userService,
            IAreaService areaService,
            ICampusService campusService,
            IBuildingService buildingService,
            IClassroomService classroomService,
            IRoleService roleService,
            IDependencyService dependencyService,
            IStudentService studentService,
            ISectionService sectionService,
            ITutorialStudentService tutorialStudentService,
            IConfigurationService configurationService,
            IStudentSectionService studentSectionService,
            IClassStudentService classStudentService,
            IAcademicYearCourseService academicYearCourseService,
            IDepartmentService departmentService,
            IProvinceService provinceService,
            IDistrictService districtService,
            ICourseUnitService courseUnitService,
            IWorkingDayService workingDayService,
            IDeanService deanService,
            IExtracurricularActivityService extracurricularActivityService,
            IExtracurricularCourseService extracurricularCourseService,
            IExtracurricularAreaService extracurricularAreaService,
            ICourseTermService courseTermService,
            IOptions<CloudStorageCredentials> storageCredentials,
            IAcademicDepartmentService academicDepartmentService,
            IUserProcedureDerivationFileService userProcedureDerivationFileService,
            IUserProcedureDerivationService userProcedureDerivationService,
            IUserProcedureService userProcedureService,
            IProcedureDependencyService procedureDependencyService,
            IDataTablesService dataTablesService,
            ITeacherSectionService teacherSectionService,
            IAcademicHistoryService academicHistoryService,
            IAcademicRecordDepartmentService academicRecordDepartmentService,
            IGradeService gradeService,
            ICourseCertificateService courseCertificateService,
            IStudentScaleService studentScaleService,
            ISectionGroupService sectionGroupService,
            IEnrollmentFeeService enrollmentFeeService
        ) : base(userManager, termService)
        {
            _courseService = courseService;
            _courseTermService = courseTermService;
            _select2Service = select2Service;
            _facultyService = facultyService;
            _careerService = careerService;
            _academicProgramService = academicProgramService;
            _admissionTypeService = admissionTypeService;
            _substituteExamService = substituteExamService;
            _psychologicalDiagnosticService = psychologicalDiagnosticService;
            _teacherService = teacherService;
            _userService = userService;
            _areaService = areaService;
            _campusService = campusService;
            _buildingService = buildingService;
            _classroomService = classroomService;
            _roleService = roleService;
            _dependencyService = dependencyService;
            _studentService = studentService;
            _sectionService = sectionService;
            _tutorialStudentService = tutorialStudentService;
            _configurationService = configurationService;
            _studentSectionService = studentSectionService;
            _classStudentService = classStudentService;
            _academicYearCourseService = academicYearCourseService;
            _departmentService = departmentService;
            _provinceService = provinceService;
            _districtService = districtService;
            _courseUnitService = courseUnitService;
            _workingDayService = workingDayService;
            _deanService = deanService;
            _curriculumService = curriculumService;
            _evaluationTypeService = evaluationTypeService;
            _context = context;
            _extracurricularActivityService = extracurricularActivityService;
            _extracurricularCourseService = extracurricularCourseService;
            _extracurricularAreaService = extracurricularAreaService;
            _storageCredentials = storageCredentials;
            _teacherSectionService = teacherSectionService;
            _academicDepartmentService = academicDepartmentService;
            _conceptService = conceptService;
            _userProcedureDerivationFileService = userProcedureDerivationFileService;
            _userProcedureDerivationService = userProcedureDerivationService;
            _userProcedureService = userProcedureService;
            _procedureDependencyService = procedureDependencyService;
            _dataTablesService = dataTablesService;
            _academicHistoryService = academicHistoryService;
            _academicRecordDepartmentService = academicRecordDepartmentService;
            _gradeService = gradeService;
            _sectionGroupService = sectionGroupService;
            _enrollmentFeeService = enrollmentFeeService;
        }

        /// <summary>
        /// Obtiene el listado de anuncios en formato para elementos select2
        /// </summary>
        /// <returns>Objeto con la lista de anuncios</returns>
        [HttpGet("roles-anuncios")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _roleService.GetAll();
            var result = data
                .Where(x => x.Name == ConstantHelpers.ROLES.STUDENTS ||
                            x.Name == ConstantHelpers.ROLES.TEACHERS ||
                            x.Name == ConstantHelpers.ROLES.CAREER_DIRECTOR ||
                            x.Name == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR ||
                            x.Name == ConstantHelpers.ROLES.DEAN ||
                            x.Name == ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToList();
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de conceptos en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene la lista de conceptos</returns>
        [HttpGet("conceptos/get")]
        public async Task<IActionResult> GetConcepts()
        {
            var result = await _conceptService.GetConceptsJson(null);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de tipos de preguntas para las encuestas en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de tipos de preguntas</returns>
        [HttpGet("tipospregunta/get")]
        public IActionResult GetQuestionTypes()
        {
            var result = ConstantHelpers.SURVEY
                                .TYPE_QUESTION.Select(x => new
                                {
                                    id = x.Key,
                                    text = x.Value
                                }).ToList();


            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de periodos académicos en formato para elementos select2
        /// </summary>
        /// <returns>objeto que contiene el listado de los periodos académicos</returns>
        [HttpGet("periodos/get")]
        public async Task<IActionResult> GetTerms()
        {
            IEnumerable<Select2Structs.Result> terms = await _termService.GetTermsSelect2ClientSide();
            var activeTerm = await GetActiveTerm();
            return Ok(new { items = terms, selected = activeTerm?.Id });
        }

        [HttpGet("periodos/get/v2")]
        public async Task<IActionResult> GetTermsV2()
        {
            object result = await _termService.GetTermsWithStatus(ConstantHelpers.TERM_STATES.ACTIVE);
            var activeTerm = await _termService.GetActiveTerm();
            return Ok(new { items = result, selected = activeTerm?.Id });
        }

        [HttpGet("periodos/pendientes/get")]
        public async Task<IActionResult> GetPendingTerms()
        {
            var result1 = await _termService.GetTermsStatus(ConstantHelpers.TERM_STATES.ACTIVE);
            var result2 = await _termService.GetTermsStatus(ConstantHelpers.TERM_STATES.INACTIVE);
            result1.AddRange(result2);

            var result = result1
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name,
                })
                .OrderByDescending(x => x.text)
                .ToList();

            var activeTerm = await _termService.GetActiveTerm();
            return Ok(new { items = result, selected = activeTerm?.Id });
        }

        /// <summary>
        /// Obtiene el listado de periodos académicos finalizados en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de periodos académicos</returns>
        [HttpGet("periodos-pasados/get")]
        public async Task<IActionResult> GetTermsOlds()
        {
            var data = await _termService.GetAll();
            var activeTerm = await GetActiveTerm();
            var terms = data
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED)
                .OrderByDescending(x => x.StartDate)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToList();
            return Ok(new { items = terms, selected = terms.FirstOrDefault().id });
        }

        /// <summary>
        /// Obtiene el listado de los ultimos periodos académicos en formato para elementos select2
        /// </summary>
        /// <param name="yearDifference">Años de diferencia</param>
        /// <returns>Objeto que contiene el listado de periodos académicos</returns>
        [HttpGet("ultimos-periodos/get")]
        public async Task<IActionResult> GetLastTerms(int? yearDifference)
        {
            IEnumerable<Select2Structs.Result> terms = await _termService.GetLastTermsSelect2ClientSide(yearDifference);
            return Ok(new { items = terms });
        }

        /// <summary>
        /// Obtiene el listado de periodos matriculados por estudiante en formato para elementos select2
        /// </summary>
        /// <param name="studentId">identificador del estudiante</param>
        /// <returns>Objeto que contiene el listado de periodos académicos</returns>
        [HttpGet("periodos-por-estudiante/get")]
        public async Task<IActionResult> GetTerms(Guid studentId)
        {
            object result = await _termService.GetTermsByStudentIdSelect2ClientSide(studentId);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de periodos matriculados por estudiante para el usuario logeado en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de periodos académicos</returns>
        [HttpGet("periodos-por-estudiante/get/v2")]
        public async Task<IActionResult> GetTermsByUser()
        {
            if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS))
            {
                var user = await _userService.GetUserByClaim(User);
                var student = await _studentService.GetStudentByUser(user.Id);
                object result = await _termService.GetTermsByStudentIdSelect2ClientSide(student.Id);
                return Ok(new { items = result });
            }

            return Ok();
        }

        [HttpGet("periodos-por-estudiante/get/v3")]
        public async Task<IActionResult> GetTermsV3ByUser(Guid? studentId = null)
        {
            if (studentId == null)
            {
                var user = await _userService.GetUserByClaim(User);
                var student = await _studentService.GetStudentByUser(user.Id);
                studentId = student.Id;
            }

            var result = await _context.Terms
            .OrderByDescending(x => x.Year).ThenByDescending(x => x.Number)
            .Where(x =>
            x.CourseTerms.Any(y => y.Sections.Any(z => z.StudentSections.Any(g => g.StudentId == studentId))) ||
            x.AcademicHistories.Any(y => y.StudentId == studentId)
            )
            .Select(x => new
            {
                x.Id,
                Text = x.Name,
                Selected = x.Status == ConstantHelpers.TERM_STATES.ACTIVE
            })
            .ToListAsync();

            return Ok(new { items = result });

        }

        /// <summary>
        /// Obtiene el listado de docentes con carga lectiva activa en un periodo académico en formato para elementos select2
        /// </summary>
        /// <param name="tid">Identificador del periodo académico</param>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de docentes</returns>
        [HttpGet("periodo/{tid}/profesor/get")]
        public async Task<IActionResult> GetTTeacherByTerm(Guid tid, string q)
        {
            var parameters = _select2Service.GetRequestParameters();
            var result = await _teacherSectionService.GetTeachersSelect2(parameters, q, tid);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de notas por alumno en formato para elementos select2
        /// </summary>
        /// <param name="cid">Identificador del curso</param>
        /// <param name="tid">identificador del periodo</param>
        /// <param name="q">texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de notsa</returns>
        [HttpGet("cursos/{cid}/periodo/{tid}/historial-academico/get")]
        public async Task<IActionResult> GetTAcademicHistoryByCourseAndTerm(Guid cid, Guid tid, string q)
        {
            var parameters = _select2Service.GetRequestParameters();
            var result = await _academicHistoryService.GetTAcademicHistoryByCourseAndTerm(parameters, q, cid, tid);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de notas por alumno en formato para elementos select2
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <param name="eid">Identificador de la evaluación</param>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de notas</returns>
        [HttpGet("alumnos/seccion/{sid}/evaluacion/{eid}/get")]
        public async Task<IActionResult> GetStudentsByCourseTermAndEvaluation(Guid sid, Guid? eid, string q)
        {
            var parameters = _select2Service.GetRequestParameters();
            var result = await _gradeService.GetStudentsBySectionAndEvaluation(parameters, sid, eid, q);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de periodos académicos donde el curso se ha aperturado en formato para elementos select2
        /// </summary>
        /// <param name="cid">Identificador del curso</param>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de periodos académicos</returns>
        [HttpGet("cursos/{cid}/periodos/get")]
        public async Task<JsonResult> GetCourseTerms(Guid cid, string q)
        {
            Select2Structs.RequestParameters parameters = _select2Service.GetRequestParameters();
            Select2Structs.ResponseParameters result = await _termService.GetTermsByCourseIdSelect2(parameters, cid, q);
            return Json(result);
        }

        /// <summary>
        /// Obtiene los datos del periodo académico
        /// </summary>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene los datos del periodo académico</returns>
        [HttpGet("periodos/{pid}/get")]
        public async Task<JsonResult> GetTerm(Guid pid)
        {
            ENTITIES.Models.Enrollment.Term term = await _termService.Get(pid);
            var result = new
            {
                term.Id,
                Text = term.Name
            };

            return Json(result);
        }

        /// <summary>
        /// Obtiene el listado de facultades en formato para elementos select2
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de facultades</returns>
        [HttpGet("facultades/get")]
        public async Task<IActionResult> GetFaculties(string q)
        {
            object result = await _facultyService.GetFacultiesSelect2ClientSide(false, User);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de facultades en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de facultades</returns>
        [HttpGet("facultades/v2/get")]
        public async Task<IActionResult> GetFacultiesAll()
        {
            object faculties = await _facultyService.GetFacultiesSelect2ClientSide(true, User);

            var result = new
            {
                items = faculties
            };
            return Ok(result);
        }

        [HttpGet("tipodeescuela/get")]
        public IActionResult StudentInformationTypeSchool()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.PERSONAL_INFORMATION.PERSONAL_INFORMATION_TYPE_SCHOOL.TYPE_SCHOOL
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("preparacion-universitaria/get")]
        public IActionResult StudentInformationUniversityPreparation()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.PERSONAL_INFORMATION.PERSONAL_INFORMATION_UNIVERSITY_PREPARATION.UNIVERSITY_PREPARATION
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("sostiene-hogar/get")]
        public IActionResult StudentInformationEconomyPrincipalPerson()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_PRINCIPAL_PERSON.PRINCIPAL_PERSON
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("metodo-economico/get")]
        public IActionResult StudentInformationEconomyEconomicMethod()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_ECONOMIC_METHOD.ECONOMIC_METHOD
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("d-sector-economico/get")]
        public IActionResult StudentInformationEconomyDSector()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_D_SECTOR.D_SECTOR
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("d-condicion-laboral/get")]
        public IActionResult StudentInformationEconomyDWorkCondition()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_D_WORK_CONDITION.D_WORK_CONDITION
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("desocupado/get")]
        public IActionResult StudentInformationEconomyBusy()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_BUSY.BUSY
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("i-sector-economico/get")]
        public IActionResult StudentInformationEconomyISector()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_I_SECTOR.I_SECTOR
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("i-condicion-laboral/get")]
        public IActionResult StudentInformationEconomyIWorkCondition()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_I_WORK_CONDITION.I_WORK_CONDITION
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("dependencia-economica/get")]
        public IActionResult StudentInformationEconomyStudentDependency()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_STUDENT_DEPENDENCY.STUDENT_DEPENDENCY
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("vivienda/get")]
        public IActionResult StudentInformationEconomyStudentCoexistence()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_STUDENT_COEXISTENCE.STUDENT_COEXISTENCE
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("riesgo-familiar/get")]
        public IActionResult StudentInformationEconomyStudentRisk()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_STUDENT_RISK.STUDENT_RISK
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("estudiante-dedicacion-laboral/get")]
        public IActionResult StudentInformationEconomyWorkDedication()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_WORK_DEDICATION.VALUES
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("estudiante-condicion-laboral/get")]
        public IActionResult StudentInformationEconomyWorkCondition()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_WORK_CONDITION.VALUES
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("salud-familiar-enfermo/get")]
        public IActionResult StudentInformationHealthParentSick()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.HEALTH.HEALTH_PARENT_SICK.PARENT_SICK
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("salud-tiene-seguro/get")]
        public IActionResult StudentInformationHealthHasInsurance()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.HEALTH.HEALTH_INSURANCE.INSURANCE
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("salud-tipo-de-seguro/get")]
        public IActionResult StudentInformationHealthTypeInsurance()
        {
            var result = ConstantHelpers.STUDENT_INFORMATION.HEALTH.TYPE_INSURANCE.VALUES
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                })
                .ToList();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("provincias/{provinceId}/distritos/get")]
        public async Task<IActionResult> GetDistrictsV2(Guid provinceId, string q)
        {
            var result = await _districtService.GetDistrictsSelect2ClientSide(provinceId);

            if (!string.IsNullOrEmpty(q))
            {
                result = result.Where(d => d.Text.Contains(q));
            }

            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales asignados al personal de registros académicos logueado en formato para elementos select2
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <returns>Objeto que contiene el listado de escuelas profesionales</returns>
        [HttpGet("carreras/registroacademico/get")]
        public async Task<IActionResult> GetCareersByAcademicRegister(Guid? facultyId)
            => Ok(await _academicRecordDepartmentService.GetCareersSelect2ClientSide(User, facultyId));

        /// <summary>
        /// Obtiene el listado de facultades asignadas al personal de registros académicos logueado en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de facultades</returns>
        [HttpGet("facultades/registroacademico/get")]
        public async Task<IActionResult> GetFacultiesByAcademicRegister()
            => Ok(await _academicRecordDepartmentService.GetFacultiesSelect2ClientSide(User));

        /// <summary>
        /// Obtiene el listado de escuelas profesionales en formato para elementos select2
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <returns>Objeto que contiene el listado de escuelas profesionales</returns>
        [HttpGet("carrerasporfacultad/{facultyId}/get")]
        public async Task<IActionResult> GetCareersByFaculty(Guid? facultyId = null)
        {
            object careers = await _careerService.GetCareerSelect2ClientSide(null, facultyId, false);
            return Ok(new { items = careers });
        }

        /// <summary>
        /// Obtiene el listado de programas académicos en formato para elementos select2
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Objeto que contiene el listado de programas académicos</returns>
        [HttpGet("especialidadporcarrera/{careerId}/get")]
        public async Task<IActionResult> GetSpecialityByCareer(Guid? careerId)
        {
            if (careerId == Guid.Empty)
            {
                careerId = null;
            }

            IEnumerable<Select2Structs.Result> result = await _academicProgramService.GetAcademicProgramByCareerSelect2ClientSide(careerId);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales en formato para elementos select2
        /// </summary>
        /// <param name="id">Identificador de la facultad</param>
        /// <returns>Objeto que contiene el listado de escuelas profesionales</returns>
        [HttpGet("facultades/{id}/carreras/get")]
        public async Task<IActionResult> GetFacultyCareers(Guid id)
        {
            object careers = await _careerService.GetCareerSelect2ClientSide(null, id);
            var result = new { items = careers };
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales en formato para elementos select2
        /// </summary>
        /// <param name="id">Identificador de la facultad</param>
        /// <param name="hasAll">¿Encabezado de todos?</param>
        /// <returns>Objeto que contiene el listado de escuelas profesionales</returns>
        [HttpGet("facultades/{id}/carreras/v2/get")]
        public async Task<IActionResult> GetFacultyCareersAll(Guid id, bool hasAll = true)
        {
            object careers = await _careerService.GetCareerSelect2ClientSide(null, id, hasAll, null, User);
            var result = new { items = careers };
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de tipos de admisión en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de tipos de admisión</returns>
        [HttpGet("admissionTypes/get")]
        public async Task<IActionResult> GetAdmissionTypes()
        {
            IEnumerable<Select2Structs.Result> result = await _admissionTypeService.GetAdmissionTypesSelect2ClientSide();
            return Json(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de diagnóstico psicológicos en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de diagnóstico psicológicos</returns>
        [HttpGet("diagnostics-psicologicos/get")]
        public async Task<IActionResult> GetPsychologicalDiagnostics()
        {
            IEnumerable<Select2Structs.Result> result = await _psychologicalDiagnosticService.GetPychologicalDiagnosticsSelect2ClientSide();
            return Json(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de docentes en formato para elementos select2
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de docentes</returns>
        [HttpGet("profesores/get")]
        public async Task<IActionResult> GetTeachers(string q)
        {
            Select2Structs.RequestParameters parameters = _select2Service.GetRequestParameters();
            Select2Structs.ResponseParameters result = await _teacherService.GetTeachersSelect2(parameters, q);
            return Json(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de usuarios con el rol de personal de registro académico
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de usuarios</returns>
        [HttpGet("registros_academicos/get")]
        public async Task<IActionResult> GetAcademicRecords(string q)
        {
            List<string> roles = new List<string>
            {
                ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF
            };

            IEnumerable<Select2Structs.Result> result = await _userService.GetUsersByRolesSelect2ClientSide(roles);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de docentes en formato para elementos select2
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="academicDepartmentId">Identificador del departamento académico</param>
        /// <returns>Objeto que contiene el listado de docentes</returns>
        [HttpGet("profesores/get/v2")]
        public async Task<IActionResult> GetTeachersV2(string q, Guid? careerId, Guid? academicDepartmentId)
        {
            Select2Structs.RequestParameters parameters = _select2Service.GetRequestParameters();
            Select2Structs.ResponseParameters result = await _teacherService.GetTeachersSelect2(parameters, q, careerId, academicDepartmentId, User);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de docentes en formato para elementos select2
        /// </summary>
        /// <param name="fid">Identificador de la facultad</param>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <returns>Objeto que contiene el listado de docentes</returns>
        [HttpGet("profesores/get/{fid}/{cid}")]
        public async Task<IActionResult> GetTeachersAndCarrer(Guid fid, Guid cid)
        {
            IEnumerable<Select2Structs.Result> result = await _teacherService.GetTeachersSelect2ClientSide(fid, cid);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de docentes en formato para elementos select2
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="academicDepartmentId">Identificador del departamento académico</param>
        /// <returns>Objeto que contiene el listado de docentes</returns>
        [HttpGet("profesores/departamento-academico/get/{facultyId}/{academicDepartmentId}")]
        public async Task<IActionResult> GetTeachersAndCarrer(Guid? facultyId = null, Guid? academicDepartmentId = null)
        {
            var result = await _teacherService.GetTeacherSelectByAcademicDepartment(facultyId, academicDepartmentId);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el detalle del docente
        /// </summary>
        /// <param name="tid">Identificador del docente</param>
        /// <returns>Objeto que contiene la información del docente</returns>
        [HttpGet("profesores/{tid}/get")]
        public async Task<IActionResult> GetTeacher(string tid)
        {
            Teacher teacher = await _teacherService.GetAsync(tid);
            ApplicationUser user = await _userService.Get(teacher.UserId);

            var result = new
            {
                Id = teacher.UserId,
                Text = user.FullName
            };

            return Json(result);
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de escuelas profesionales</returns>
        [HttpGet("areascarreras/get")]
        public async Task<IActionResult> GetAreasAndCareers()
        {
            object careers = await _careerService.GetCareerSelect2ClientSide();
            IEnumerable<Select2Structs.Result> areas = await _areaService.GetAreasSelect2ClientSide();
            List<object> list = new List<object>();
            list.Add(new
            {
                Text = "Áreas",
                children = areas
            });

            list.Add(new
            {
                Text = "Carreras",
                Children = careers
            });

            return Ok(new { items = list });
        }

        /// <summary>
        /// Obtiene el detalle de la escuela profesional 
        /// </summary>
        /// <param name="id">Identificador de la escuela profesional</param>
        /// <returns>Objeto que contiene los datos de la escuela profesional</returns>
        [HttpGet("areascarreras/{id}/get")]
        public async Task<IActionResult> GetAreaOrCareer(Guid id)
        {
            ENTITIES.Models.Enrollment.Area area = await _areaService.Get(id);
            var resultArea = new
            {
                area.Id,
                Text = area.Name
            };

            if (area == null)
            {
                Career career = await _careerService.Get(id);
                var resultCareer = new
                {
                    career.Id,
                    Text = career.Name
                };

                return Ok(resultCareer);
            }

            return Ok(area);
        }

        /// <summary>
        /// Obtiene el listado de sedes en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de sedes</returns>
        [HttpGet("sedes/get")]
        public async Task<IActionResult> GetCampuses()
        {
            object result = await _campusService.GetAllAsSelect2ClientSide();
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de programas académicos en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de programas académicos</returns>
        [HttpGet("especialidades/get")]
        public async Task<IActionResult> GetSpecialties()
        {
            var result = await _context.Roles
                     .Where(x => x.Name == ConstantHelpers.ROLES.PSYCHOLOGY || x.Name == ConstantHelpers.ROLES.NUTRITION || x.Name == ConstantHelpers.ROLES.INFIRMARY)
                     .Select(x => new
                     {
                         x.Id,
                         text = x.Name
                     })
                     .ToListAsync();
            //IEnumerable<Select2Structs.Result> result = await _doctorSpecialtyService.GetSpecialitiesSelect2ClientSide();
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de pabellones en formato para elementos select2
        /// </summary>
        /// <param name="id">Identificador del campus</param>
        /// <returns>Objeto que contiene el listado de pabellones</returns>
        [HttpGet("sedes/{id}/pabellones/get")]
        public async Task<IActionResult> GetBuildings(Guid id)
        {
            IEnumerable<Select2Structs.Result> result = await _buildingService.GetBuildingsSelect2ClientSide(id);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de aulas asociadas al pabellón en formato para elementos select2
        /// </summary>
        /// <param name="bid">Identificador del pabellón</param>
        /// <returns>Objeto que contiene el listado de aulas</returns>
        [HttpGet("sedes/{cid}/pabellones/{bid}/aulas/get")]
        public async Task<IActionResult> GetBuildingClassrooms(Guid bid)
        {
            IEnumerable<Select2Structs.Result> result = await _classroomService.GetClassroomsSelect2ClientSide(bid);
            return Json(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de aulas en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de aulas</returns>
        [HttpGet("aulas/get")]
        public async Task<IActionResult> GetClassrooms()
        {
            IEnumerable<Select2Structs.Result> result = await _classroomService.GetClassroomsSelect2ClientSide();
            return Json(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de aulas en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de aulas</returns>
        [HttpGet("aular/get/v2")]
        public async Task<IActionResult> GetClassroomsSelect2ServerSide()
        {
            var paramteres = _select2Service.GetRequestParameters();
            var result = await _classroomService.GetClassroomSelect2(paramteres, paramteres.SearchTerm);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de aulas en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de aulas</returns>
        [HttpGet("aulas/{id}/get/")]
        public async Task<IActionResult> GetClassroom(Guid id)
        {
            ENTITIES.Models.Enrollment.Classroom classroom = await _classroomService.Get(id);
            var result = new
            {
                classroom.Id,
                Text = classroom.Description
            };

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de roles (alumnos y estudiantes) en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de roles</returns>
        [HttpGet("roles_selected/get")]
        public async Task<IActionResult> GetSelectedRoles()
        {
            List<string> roles = new List<string>
            {
                ConstantHelpers.ROLES.STUDENTS,
                ConstantHelpers.ROLES.TEACHERS,
                ConstantHelpers.ROLES.CAREER_DIRECTOR,
                ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR,
                ConstantHelpers.ROLES.DEAN
            };

            IEnumerable<ApplicationRole> rolesResult = await _roleService.GetAllByName(roles);
            var result = rolesResult.Select(x => new
            {
                x.Id,
                Text = x.Name
            }).OrderBy(x => x.Text);

            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de dependencias en formato para elementos select2 
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de dependencias</returns>
        [HttpGet("dependencias/get")]
        public async Task<IActionResult> GetDependencies(string q)
        {
            Select2Structs.RequestParameters parameters = _select2Service.GetRequestParameters();
            Select2Structs.ResponseParameters result = await _dependencyService.GetDependenciesSelect2(parameters, q);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de dependencias en formato para elementos select2
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de dependencias</returns>
        [HttpGet("dependencias/todas")]
        public async Task<IActionResult> GetAllDependencies(string q)
        {
            object result = await _dependencyService.GetDependenciesJson();
            return Ok(result);
            //var result = await _dependencyService.GetDependenciesSelect2(parameters, q);
            //return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene la informción de la dependencia
        /// </summary>
        /// <param name="id">Identificador de la dependencia</param>
        /// <returns>Objeto que contiene los datos de la dependencia</returns>
        [HttpGet("dependencias/{id}/get")]
        public async Task<JsonResult> GetDependency(Guid id)
        {
            ENTITIES.Models.DocumentaryProcedure.Dependency dependecy = await _dependencyService.Get(id);
            return Json(new { id = dependecy.Id, text = dependecy.Name });
        }

        /// <summary>
        /// Obtiene el listado de roles a excepción de docentes y estudiantes
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Listado de roles</returns>
        [HttpGet("roles/get")]
        public async Task<JsonResult> GetRoles(string q)
        {
            object result = await _roleService.GetAllAsSelect2ClientSide(new List<string> {
                ConstantHelpers.ROLES.STUDENTS,
                ConstantHelpers.ROLES.TEACHERS
            });

            return Json(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de roles en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de roles</returns>
        [HttpGet("roles_todos/get")]
        public async Task<JsonResult> GetAllRoles()
        {
            object result = await _roleService.GetAllAsSelect2ClientSide();

            return Json(new { items = result });
        }

        /// <summary>
        /// Obtiene la información del rol
        /// </summary>
        /// <param name="id">Identificador del rol</param>
        /// <returns>Objeto que contiene la información del rol</returns>
        [HttpGet("roles/{id}/get")]
        public async Task<JsonResult> GetRole(string id)
        {
            ApplicationRole result = await _roleService.Get(id);
            return Json(new { id = result.Id, text = result.Name });
        }

        /// <summary>
        /// Obtiene el listado de cursos en formato para elementos select2
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <returns>Objeto que contiene el listado de cursos</returns>
        [HttpGet("cursos/get")]
        public async Task<IActionResult> GetCourses(string q, [FromQuery] Guid? cid)
        {
            IEnumerable<Select2Structs.Result> courses = await _courseService.GetCoursesSelect2ClientSide(cid, q);
            return Json(new { items = courses });
        }

        /// <summary>
        /// Obtiene el listado de cursos en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de cursos</returns>
        [HttpGet("cursos/get/v2")]
        public async Task<IActionResult> GetCourseV2()
        {
            var parameters = _select2Service.GetRequestParameters();
            var result = await _courseService.GetCoursesSelect2(parameters, User, parameters.SearchTerm);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales en formato para elementos select2 
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <param name="fid">Identificador de la facultad</param>
        /// <returns>Objeto que contiene el listado de escuelas profesionales</returns>
        [HttpGet("carreras/get")]
        public async Task<IActionResult> GetCareers(string q, [FromQuery] Guid? fid)
        {
            object careers = await _careerService.GetCareerSelect2ClientSide(null, fid, false, null, User);
            return Json(new { items = careers });
        }


        /// <summary>
        /// Obtiene una lista de los planes curriculares activos del ultimo años segun los filtros
        /// </summary>
        /// <param name="careerId">Identificador de la Escuela Profesional</param>
        /// <returns>Objeto que contiene el listado de planes curriculares</returns>
        [HttpGet("planes-activos/ultimo-ano/carrera/{careerId}/get")]
        public async Task<IActionResult> GetCurriculums(Guid careerId)
        {
            object curriculums = await _curriculumService.GetCurriculumsLastYearActiveByCareerSelect2(careerId);
            return Json(new { items = curriculums });
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales en formato para elementos select2  
        /// </summary>
        /// <returns>Objeto que contiene el listado de escuelas profesionales</returns>
        [HttpGet("carreras/get/v4")]
        public async Task<IActionResult> GetAllCareersV4()
        {
            var parameter = _select2Service.GetRequestParameters();
            var result = await _careerService.GetCareerSelect2(parameter, parameter.SearchTerm, null, User);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales en formato para elementos select2  
        /// </summary>
        /// <returns>Objeto que contiene el listado de escuelas profesionales</returns>
        [HttpGet("carreras/v2/get")]
        public async Task<IActionResult> GetAllCareers()
        {
            var result = await _careerService.GetCareerSelect2ClientSide(null, null, false, null, User);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales en formato para elementos select2  
        /// </summary>
        /// <returns>Objeto que contiene el listado de escuelas profesionales</returns>
        [HttpGet("carreras/v3/get")]
        public async Task<IActionResult> GetCareersSelect2()
        {

            var data = _context.Careers.AsQueryable();
            if (User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
            {
                var userId = _userManager.GetUserId(User);
                var careers = await _context.Careers.Where(x => x.AcademicCoordinatorId == userId).Select(x => x.Id).ToListAsync();

                if (careers.Count > 0)
                {
                    data = data.Where(x => careers.Contains(x.Id));
                }
            }

            var result = data.Select(x => new
            {
                id = x.Id,
                text = x.Name
            }).ToList();

            return Ok(new { items = result.OrderBy(x => x.text) });
        }

        /// <summary>
        /// Obtiene los datos de la escuela profesional 
        /// </summary>
        /// <param name="id">Identificador de la escuela profesional</param>
        /// <returns>Objeto que contiene el listado de escuelas profesionales</returns>
        [HttpGet("carreras/{id}/get")]
        public async Task<IActionResult> GetCareer(Guid id)
        {
            Career career = await _careerService.Get(id);
            return Ok(new { id = career.Id, text = career.Name });
        }

        /// <summary>
        /// Obtiene el listado de alumnos matriculados en una sección en formato para elementos select2  
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <returns>Objeto que contiene el listado de alumnos</returns>
        [HttpGet("alumnos/get")]
        public async Task<IActionResult> GetStudents(Guid sid)
        {
            IEnumerable<Student> students = await _studentService.GetStudentsBySeciontId(sid);
            var result = students.Select(s => new
            {
                id = s.Id,
                text = s.User.FullName
            }).ToList();
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de alumnos en formato para elementos select2  
        /// </summary>
        /// <param name="selectedId">Identificador del estudiante</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de alumnos</returns>
        [HttpGet("alumnosv2/get")]
        public async Task<IActionResult> GetStudentsv2(Guid? selectedId, string searchValue)
        {
            Select2Structs.RequestParameters requestParameters = _select2Service.GetRequestParameters();
            Select2Structs.ResponseParameters result = await _studentService.GetStudentSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = $"{x.User.UserName} - {x.User.FullName}",
                Selected = x.Id == selectedId
            }, (x) => new[] { x.User.FullName }, searchValue);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de secciones asignadas al docente en formato para elementos select2  
        /// </summary>
        /// <returns>Objeto que contiene el listado de secciones</returns>
        [HttpGet("profesor/seccionescurso/get")]
        public async Task<IActionResult> GetTeacherSectionCourse()
        {
            var teacherId = _userManager.GetUserId(User);
            var term = await _termService.GetActiveTerm();
            var sections = await _sectionService.GetSectionsByTermIdAndTeacherIdSelect2ClientSide(term.Id, teacherId);
            return Ok(new { items = sections });
        }

        /// <summary>
        /// Obtiene el listado de estudiantes asignados a la tutoría
        /// </summary>
        /// <param name="tid">Identificador de la tutoría</param>
        /// <returns>Objeto que contiene el listado de estudiantes</returns>
        [HttpGet("tutorias/{tid}/alumnos/get")]
        public async Task<IActionResult> GetTutorialStudents(Guid tid)
        {
            IEnumerable<Select2Structs.Result> result = await _tutorialStudentService.GetStudentsByTutorialIdSelect2ClientSide(tid);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de secciones asignadas al estudiante logueado en formato para elementos select2  
        /// </summary>
        /// <returns>Objeto que contiene el listado de secciones</returns>
        [HttpGet("alumno/seccionescurso/get")]
        public async Task<IActionResult> GetStudentSectionCourse()
        {
            ENTITIES.Models.Enrollment.Term term = await _termService.GetActiveTerm();
            string userId = _userManager.GetUserId(User);
            Student student = await _studentService.GetStudentByUser(userId);
            IEnumerable<Select2Structs.Result> result = await _sectionService.GetSectionsByStudentIdAndTermIdSelect2ClientSide(student.Id, term.Id);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de inasistencias del alumno loguedo en una seccióm
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Objeto que contiene el listado de inasistencias</returns>
        [HttpGet("alumno/secciones/{sectionId}/inasistencias/get")]
        public async Task<IActionResult> GetStudentAbsences(Guid sectionId)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _classStudentService.GetClassStudentSelect2ClientSide(userId, sectionId, true);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de periodos académicos finalizados en formato para elementos select2  
        /// </summary>
        /// <returns>Objeto que contiene el listado de periodos académicos finalizados</returns>
        [HttpGet("periodos-finalizados/get")]
        public async Task<IActionResult> GetFinishedTerms()
        {
            object result = await _termService.GetTermsFinishedSelect2ClientSide();
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de doctores en formato para elementos select2 
        /// </summary>
        /// <returns>Objeto que contiene el listado de doctores</returns>
        [HttpGet("doctores/get")]
        public async Task<IActionResult> GetDoctors()
        {
            var result = await _context.Users.Where(x => x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.PSYCHOLOGY))
                .Select(y => new
                {
                    y.Id,
                    text = y.FullName
                })
                .ToListAsync();

            //var speciality = await _doctorSpecialtyService.GetSpecialtyByDescription("Psicología");
            //IEnumerable<Select2Structs.Result> result = await _doctorService.GetDoctorsSelect2ClientSide(speciality.Id);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de usuarios administrativos en formato para elementos select2 
        /// </summary>
        /// <returns>Objeto que contiene el listado de usuarios</returns>
        [HttpGet("administrativos")]
        public async Task<IActionResult> GetOnlyAdministratives()
        {
            object result = await _userService.GetOnlyAdministrative(_select2Service.GetRequestParameters(), _select2Service.GetSearchTerm());
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de cursos que pertenezcan a un plan de estudio activo en formato para elementos select2 
        /// </summary>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <returns>Objeto que contiene el listado de cursos</returns>
        [HttpGet("carreras/{cid}/cursos/get")]
        public async Task<IActionResult> GetCoursesFromCareer(Guid cid)
        {
            object result = await _academicYearCourseService.GetCoursesByCareerIdAndCurriculumActive(cid);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de cursos aperturados en el perido activo en formato para elementos select2 
        /// </summary>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <returns>Objeto que contiene el listado de cursos</returns>
        [HttpGet("carreras/{cid}/cursos/activeterm/get")]
        public async Task<IActionResult> GetCoursesFromCareerAndActiveTerm(Guid cid)
        {
            ENTITIES.Models.Enrollment.Term term = await _termService.GetActiveTerm();
            IEnumerable<ENTITIES.Models.Enrollment.CourseTerm> courses = await _courseTermService.GetAllByTermAndCareer(cid, term.Id);
            var result = courses.Select(x => new
            {
                id = x.CourseId,
                text = $" {x.Course.Code} - {x.Course.Name}"
            }).ToList();

            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de cursos aperturados en formato para elementos select2 
        /// </summary>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns></returns>
        [HttpGet("carreras/{cid}/cursos/activos/get")]
        public async Task<IActionResult> GetCoursesFromCareerAndActiveTerm(Guid cid, Guid termId)
        {
            IEnumerable<ENTITIES.Models.Enrollment.CourseTerm> courses = await _courseTermService.GetAllByTermAndCareer(cid, termId);
            var result = courses.Select(x => new
            {
                id = x.CourseId,
                text = $" {x.Course.Code} - {x.Course.Name}"
            }).ToList();

            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de programas académicos en formato para elementos select2 
        /// </summary>
        /// <param name="cid">identificador de la escuela profesional</param>
        /// <returns>Objeto que contiene el listado de programas académicos</returns>
        [HttpGet("carreras/{cid}/programas/get")]
        public async Task<IActionResult> GetAcademicProgramFromCareer(Guid cid)
        {
            IEnumerable<Select2Structs.Result> result = await _academicProgramService.GetAcademicProgramByCareerSelect2ClientSide(cid);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de programas académicos en formato para elementos select2 
        /// </summary>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <returns>Objeto que contiene el listado de programas académicos</returns>
        [HttpGet("programas-academico/get")]
        public async Task<IActionResult> GetAcademicPrograms(Guid curriculumId)
        {
            var result = await _academicYearCourseService.GetCareerAcademicProgramsByCurriculumSelect2(curriculumId);
            return Ok(new { items = result });
        }

        [HttpGet("programas-academico-por-usuario/get")]
        public async Task<IActionResult> GetAcademicProgramsByUser()
        {
            if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS))
            {
                var user = await _userService.GetUserByClaim(User);
                var student = await _studentService.GetStudentByUser(user.Id);
                var result = await _academicYearCourseService.GetCareerAcademicProgramsByCurriculumSelect2(student.CurriculumId);
                return Ok(new { items = result });
            }

            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de programas académicos asociados a un plan de estudio
        /// </summary>
        /// <param name="planId">Identificador del plan de estudio</param>
        /// <returns>Objeto que contiene el listado de programas académicos</returns>
        [HttpGet("programas-por-plan/{planId}")]
        public async Task<IActionResult> GetProgramsByPlan(Guid planId) => Ok(await _academicProgramService.GetCareerAcademicProgramsByPlan(planId));

        /// <summary>
        /// Obtiene el listado de departamentos en formato para elementos select2
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <param name="countryCode">Código de país</param>
        /// <returns>Objeto que contiene el listado de departamentos</returns>
        [AllowAnonymous]
        [HttpGet("departamentos/get")]
        public async Task<IActionResult> GetDepartments(string q, string countryCode)
        {
            var result = await _departmentService.GetDepartmenstJson(q, countryCode);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de provincias en formato para elementos select2
        /// </summary>
        /// <param name="did">Identificador del departmento</param>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de provincias</returns>
        [AllowAnonymous]
        [HttpGet("departamentos/{did}/provincias/get")]
        public async Task<IActionResult> GetProvinces(Guid did, string q)
        {
            IEnumerable<Select2Structs.Result> result = await _provinceService.GetProvincesSelect2ClientSide(did);

            if (!string.IsNullOrEmpty(q))
            {
                result = result.Where(p => p.Text.Contains(q));
            }

            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de distritos en formato para elementos select2
        /// </summary>
        /// <param name="pid">Identificador de la provincia</param>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de distritos</returns>
        [AllowAnonymous]
        [HttpGet("departamentos/{did}/provincias/{pid}/distritos/get")]
        public async Task<IActionResult> GetDistricts(Guid pid, string q)
        {
            IEnumerable<Select2Structs.Result> result = await _districtService.GetDistrictsSelect2ClientSide(pid);

            if (!string.IsNullOrEmpty(q))
            {
                result = result.Where(d => d.Text.Contains(q));
            }

            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de unidades asociads al curso en formato para elementos select2
        /// </summary>
        /// <param name="id">identificador del curso</param>
        /// <returns>Objeto que contiene el listado de unidades</returns>
        [HttpGet("cursos/{id}/temario/get")]
        public async Task<IActionResult> GetCourseSyllabus(Guid id)
        {
            ENTITIES.Models.Enrollment.Term term = await _termService.GetActiveTerm();
            object result = await _courseUnitService.GetCourseUnitsSelect2ClientSide(id, term.Id);
            return Ok(new { items = result });
        }

        [HttpGet("get-maximas-unidades/{termId}")]
        public async Task<IActionResult> GetMaxUnitsCount(Guid termId)
        {
            var result = await _context.CourseUnits.OrderByDescending(x=>x.Number)
                .Select(x=>x.Number)
                .FirstOrDefaultAsync();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de secciones asociadas al docente
        /// </summary>
        /// <param name="teacherId">Identificador del docente</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="type">Tipo</param>
        /// <returns>Objeto que contiene el listadao de secciones</returns>
        [HttpGet("cursos/docente/{teacherId}/periodo/{termId}/tipo/{type}/get")]
        public async Task<IActionResult> GetTeacherCoursesSelectV2(string teacherId, Guid? termId, int? type)
        {
            if (!termId.HasValue)
            {
                var activeTerm = await _termService.GetActiveTerm();
                termId = activeTerm.Id;
            }

            var query = _context.TeacherSections.Where(x => x.TeacherId == teacherId).AsNoTracking();

            var susti = _context.SubstituteExams.AsNoTracking();

            if (termId.HasValue)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (type.HasValue)
                if (type == ConstantHelpers.GRADERECTIFICATION.TYPE.SUBSTITUTORY)
                    query = query.Where(x => susti.Any(y => y.SectionId == x.SectionId));
            var result = await query
                .Select(x => new
                {
                    Id = x.SectionId,
                    Text = $"{x.Section.CourseTerm.Course.Code} - {x.Section.CourseTerm.Course.Name} - {x.Section.Code}"
                })
                .Distinct()
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de secciones asignadas al docente en formato para elementos select2
        /// </summary>
        /// <param name="teacherId">Identificador del docente</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene el listado de secciones</returns>
        [HttpGet("cursos/docente/{teacherId}")]
        public async Task<IActionResult> GetTeacherCoursesSelect(string teacherId, Guid? termId)
        {
            if (!termId.HasValue)
            {
                var activeTerm = await _termService.GetActiveTerm();
                termId = activeTerm.Id;
            }

            var result = await _context.TeacherSections
                .Where(x => x.TeacherId == teacherId && x.Section.CourseTerm.TermId == termId)
                .Select(x => new
                {
                    Id = x.Section.CourseTerm.CourseId,
                    Text = $"{x.Section.CourseTerm.Course.Code}-{x.Section.CourseTerm.Course.Name}"
                })
                .Distinct()
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de evaluciones asignadas a una sección en formato para elementos select2
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <param name="termId">identificador del periodo académico</param>
        /// <returns>Objeto que contiene el listado de evaluaciones</returns>
        [HttpGet("cursos/{sectionId}/periodo/{termId}/evaluacion/get")]
        public async Task<IActionResult> GetEvaluationByCourse(Guid sectionId, Guid termId)
        {
            var result = await _context.Evaluations
                .Where(x => x.CourseTerm.Sections.Any(y => y.Id == sectionId) && x.CourseTerm.TermId == termId)
                .Select(x => new
                {
                    Id = x.Id,
                    Text = $"U{x.CourseUnit.Number}-{x.Name}"
                })
                .Distinct()
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de secciones asignadas al docente en formato para elementos select2
        /// </summary>
        /// <param name="teacherId">Identificador del docente</param>
        /// <param name="courseId">Identificador del curso</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene el listado de secciones</returns>
        [HttpGet("secciones-por-curso/{courseId}/docente/{teacherId}")]
        public async Task<IActionResult> GetTeacherCoursesSelect(string teacherId, Guid courseId, Guid? termId)
        {
            if (!termId.HasValue)
            {
                var activeTerm = await _termService.GetActiveTerm();
                termId = activeTerm.Id;
            }

            var query = _context.TeacherSections
                .Where(x => x.TeacherId == teacherId && x.Section.CourseTerm.TermId == termId && x.Section.CourseTerm.CourseId == courseId).AsNoTracking();

            var result = await query
                .Select(x => new
                {
                    Id = x.SectionId,
                    Text = x.Section.Code
                })
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de inasistencias del personal
        /// </summary>
        /// <returns>Objeto que contiene el listado de inasistencias del personal</returns>
        [HttpGet("personal/inasistencias/get")]
        public async Task<IActionResult> GetUserAbsences()
        {
            string userId = _userManager.GetUserId(User);
            ENTITIES.Models.Enrollment.Term term = await _termService.GetActiveTerm();
            if (term == null)
                term = await _termService.GetLastTerm();
            if (term == null)
                term = new ENTITIES.Models.Enrollment.Term();

            IEnumerable<Select2Structs.Result> result = await _workingDayService.GetWorkingDaySelect2ClientSide(term.Id, userId, true);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales asociadas al decano logueado
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de escuelas profesionales</returns>
        [HttpGet("decano/carreras/get")]
        [Authorize(Roles = ConstantHelpers.ROLES.DEAN)]
        public async Task<IActionResult> GetDeanCareers(string q)
        {
            string userId = _userManager.GetUserId(User);
            Dean dean = await _deanService.Get(userId);
            object careers = await _careerService.GetCareerSelect2ClientSide(null, dean.FacultyId);
            return Ok(new { items = careers });
        }

        /// <summary>
        /// Obtiene el listado de notas por curso
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <param name="aid">Año académico</param>
        /// <returns>Objeto que contiene el listado de notas por curso</returns>
        [HttpGet("alumnos/{id}/situacion/nivel/{aid}/get")]
        public async Task<IActionResult> GetAcademicYearDetail(Guid id, byte? aid)
        {
            if (id == Guid.Empty)
            {
                return BadRequest($"No se pudo encontrar un usuario con el id {id}.");
            }

            if (!aid.HasValue)
            {
                return BadRequest($"Debe seleccionar un año académico.");
            }

            object result = await _academicYearCourseService.GetAcademicYearDetailByStudentIdAndAcademicYearId(id, aid.Value, User);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de planes de estudio en formato para elementos select2
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="onlyActive">¿Solo activos?</param>
        /// <returns>Objeto que contiene el listado de planes de estudio</returns>
        [HttpGet("planes2")]
        public async Task<IActionResult> GetPlans2(Guid careerId, bool onlyActive = false)
        {
            object planes = await _curriculumService.GetAllNumberPlusCodeByCareerId(careerId, onlyActive);
            return Ok(planes);
        }

        /// <summary>
        /// Obtiene los ciclos académicos de un plan de estudio en formato para elementos select2
        /// </summary>
        /// <param name="curriculumid">Identificador del plan de estudio</param>
        /// <returns>Objeto que contiene el listado de ciclos académicos</returns>
        [HttpGet("planes-estudio/{curriculumid}/niveles/get")]
        public async Task<IActionResult> GetCurriculumAcademicYears(Guid curriculumid)
        {
            object result = await _academicYearCourseService.GetCurriculumAcademicYearsJson(curriculumid);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de cursos asociados a un plan de estudio
        /// </summary>
        /// <param name="curriculumid">Identificador del plan de estudio</param>
        /// <param name="academicyear">Ciclo académico</param>
        /// <returns>Objeto que contiene el listado de cursos</returns>
        [HttpGet("planes-estudio/{curriculumid}/niveles/{academicyear}/get")]
        public async Task<IActionResult> GetAcademicYearCourses(Guid curriculumid, byte academicyear)
        {
            var result = await _academicYearCourseService.GetCoursesByCurriculum(curriculumid, academicyear);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de planes de estudio en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de planes de estudio</returns>
        [HttpGet("planes-estudio/get")]
        public async Task<IActionResult> GeCurriculumns()
        {
            var query = _context.Curriculums.Where(x => x.IsActive).OrderBy(x => x.Year).ThenBy(x => x.Code).AsQueryable();

            if (User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
            {
                var userId = _userManager.GetUserId(User);
                var careers = await _context.Careers.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId).Select(x => x.Id).ToListAsync();
                query = query.Where(z => careers.Contains(z.CareerId));
            }
            var result = await query.Select(x => new
            {
                id = x.Id,
                text = x.Name
            }).ToListAsync();

            result.Insert(0, new { id = new Guid(), text = "Todas" });

            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de planes de estudio asociados a una escuela profesional en formato para elementos select2
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Objeto que contiene el listado de planes de estudio</returns>
        [HttpGet("planes-estudio/{careerId}/get")]
        public async Task<IActionResult> GeCurriculumns(Guid careerId, Guid? academicProgramId)
        {
            var query = _context.Curriculums.Where(x => x.IsActive).OrderBy(x => x.Year).ThenBy(x => x.Code).AsNoTracking();

            if (academicProgramId.HasValue)
                query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (careerId != Guid.Empty)
            {
                query = query.Where(z => z.CareerId == careerId);
            }

            var result = await query.Select(x => new
            {
                id = x.Id,
                text = x.Name
            }).ToListAsync();


            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de planes de estudio activos en formato para elementos select2
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene el listado de planes de estudio</returns>
        [HttpGet("planes-estudio/{careerId}/activos/{termId}/get")]
        public async Task<IActionResult> GeCurriculumns(Guid careerId, Guid termId)
        {
            var result = await _context.Curriculums
                .Where(x => x.CareerId == careerId && x.IsActive && x.AcademicYearCourses.Any(y => y.Course.CourseTerms.Any(z => z.TermId == termId && z.Sections.Any())))
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Code)
                .Select(x => new
                {
                    Id = x.Id,
                    Text = $"{x.Year}-{x.Code}"
                })
                .ToListAsync();

            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de planes de estudio
        /// </summary>
        /// <param name="id">Identificador del programa académico</param>
        /// <returns>Objeto que contiene el listado de planes de estudio</returns>
        [HttpGet("planes-estudio/{id}/programas-academicos/get")]
        public async Task<IActionResult> GetCurriculumAcademicPrograms(Guid id)
        {
            return Ok(await _curriculumService.GetAcademicProgramsCurriculumJson(id));
        }

        [HttpGet("ciclos/numeros-romanos/get")]
        public async Task<IActionResult> GetAcademicYearRomanNumerals()
        {
            var result = ConstantHelpers.ACADEMIC_YEAR
                .ROMAN_NUMERALS.Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                }).ToList();

            if (User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR_GENERAL))
            {
                var maxAcademicYear = Convert.ToInt16(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.TeacherManagement.CAREER_DIRECTOR_GENERAL_ACADEMIC_YEAR));
                result = result.Where(x => x.id <= maxAcademicYear).ToList();
            }

            return Ok(result);
        }

        /// <summary>
        /// Obtiene los ciclos académicos de una escuela profesional en formato para elementos select2
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Objeto que contiene los ciclos académicos</returns>
        [HttpGet("carrera/{careerId}/niveles/get")]
        public async Task<IActionResult> GetAcademicYears(Guid careerId)
        {
            object result = await _academicYearCourseService.GetCareerAcademicYear(careerId);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de secciones aperturadas
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Objeto que contiene el listado de secciones</returns>
        [HttpGet("secciones/{careerId}/get")]
        public async Task<IActionResult> GetSections(Guid careerId)
        {
            object result = await _sectionService.GetSectionsByCareerSelect2ClientSide(careerId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de secciones aperturadas de un curso
        /// </summary>
        /// <param name="courseId">Identificador del curso</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene el listado de secciones</returns>
        [HttpGet("cursos/{courseId}/secciones/get")]
        public async Task<IActionResult> GetSectionsByCourse(Guid courseId, Guid? termId = null)
        {
            var term = new Term();
            if (!termId.HasValue)
            {
                term = await GetActiveTerm();
            }
            else
            {
                term = await _termService.Get(termId.Value);
            }
            var courseTerm = await _courseTermService.GetFirstByCourseAndeTermId(courseId, term.Id);
            object result = await _sectionService.GetSectionByCourseTermIdSelect2(courseTerm != null ? courseTerm.Id : Guid.Empty);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de cursos electivos
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <returns>Objeto que contiene el listado de cursos electivos</returns>
        [HttpGet("alumnos/{id}/situacion/electivos/get")]
        public async Task<IActionResult> GetAcademicSituationElective(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest($"No se pudo encontrar un usuario con el id {id}.");
            }

            var result = await _academicYearCourseService.GetAcademicSituationElectiveByStudentId(id);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle del historial académico
        /// </summary>
        /// <param name="id">identificador del estudiante</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene los datos del historial académico</returns>
        [HttpGet("alumnos/{id}/historial/periodo/{pid}/get")]
        public async Task<IActionResult> GetAcademicSummaryDetail(Guid id, Guid pid)
        {
            if (pid.Equals(Guid.Empty))
            {
                return BadRequest($"No se pudo encontrar un periodo con el id {pid}.");
            }

            object result = await _academicYearCourseService.GetAcademicSummaryDetail(id, pid);
            return Ok(result);
        }

        [HttpGet("areas-extracurriculares/actividades/get")]
        public async Task<IActionResult> GetExtracurricularActivityAreas()
        {
            var data = await _extracurricularAreaService.GetAll();
            var result = data
                .Where(x => x.Type == ConstantHelpers.ExtracurricularArea.Type.ACTIVITY)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToList();

            return Json(new { items = result });
        }

        [HttpGet("areas-extracurriculares/cursos/get")]
        public async Task<IActionResult> GetExtracurricularCourseAreas()
        {
            var data = await _extracurricularAreaService.GetAll();
            var result = data
                .Where(x => x.Type == ConstantHelpers.ExtracurricularArea.Type.COURSE)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToList();

            return Json(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de cursos extracurriculares
        /// </summary>
        /// <returns>Objeto que contiene el listado de cursos extracurriculares</returns>
        [HttpGet("cursosextracurriculares/get")]
        public async Task<IActionResult> GetExtracurricularCourses()
        {
            IEnumerable<Select2Structs.Result> result = await _extracurricularCourseService.GetExtracurricularCoursesSelect2ClientSide();
            return Json(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de actividades extracurriculares en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de actividades extracurriculares</returns>
        [HttpGet("extracurricular-activities/get")]
        public async Task<IActionResult> GetExtracurricularActivities()
        {

            IEnumerable<Select2Structs.Result> result = await _extracurricularActivityService.GetExtracurricularActivitiesSelect2ClientSide();
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de planes de estudio en formato para elementos select2
        /// </summary>
        /// <param name="id">Identificador de la escuela profesional</param>
        /// <returns></returns>
        [HttpGet("carreras/{id}/planestudio/get")]
        public async Task<IActionResult> GetCurriculumByCareer(Guid id)
        {
            object result = await _curriculumService.GetCareerCurriculumJson(id);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de cursos en formato para elementos select2
        /// </summary>
        /// <param name="id">Identificador del plan de estudio</param>
        /// <returns>Objeto que contiene el listado de cursos</returns>
        [HttpGet("curriculum/{id}/cursos/get")]
        public async Task<IActionResult> GetCoursesByCurriculum(Guid id)
        {
            object result = await _academicYearCourseService.GetCoursesByCurriculum(id);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de docentes en formato para elementos select2
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de docentes</returns>
        [HttpGet("facultades/{id}/docentes/get")]
        public async Task<IActionResult> GetFacultyTeachers(Guid? facultyId, string search)
        {
            Select2Structs.RequestParameters parameters = _select2Service.GetRequestParameters();
            Select2Structs.ResponseParameters result = await _teacherService.GetTeachersByFacultySelect2(parameters, search, facultyId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de docentes en formato para elementos select2
        /// </summary>
        /// <param name="id">Identificador de la facultad</param>
        /// <returns>Objeto que contiene el listado de docentes</returns>
        [HttpGet("facultades/{id}/docentes/v2/get")]
        public async Task<IActionResult> GetFacultyTeachersAll(Guid id)
        {
            IEnumerable<Select2Structs.Result> teachers = await _teacherService.GetTeachersSelect2ClientSide(id, null);
            var result = new
            {
                items = teachers
            };
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de departamentos académicos en formato para elementos select2
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <returns>Objeto que contiene el listado de facultades</returns>
        [HttpGet("departamentos-academicos/get")]
        public async Task<IActionResult> GetAcademicDepartments(Guid? facultyId = null)
        {
            var departments = await _academicDepartmentService.GetAll();

            if (facultyId != null) departments = departments.Where(x => x.FacultyId == facultyId);

            if (User.IsInRole(CORE.Helpers.ConstantHelpers.ROLES.DEAN) || User.IsInRole(CORE.Helpers.ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var userId = _userManager.GetUserId(User);
                departments = departments.Where(x => (x.Career != null && (x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId)) || (x.Faculty != null && (x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId))).ToList();
            }
            else
            if (User.IsInRole(CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
            {
                var userId = _userManager.GetUserId(User);
                departments = departments.Where(x => x.AcademicDepartmentDirectorId == userId).ToList();
            }
            else
            if (User.IsInRole((ConstantHelpers.ROLES.CAREER_DIRECTOR)) ||
                User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
            {
                var userId = _userManager.GetUserId(User);
                var careers = await _context.Careers.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId).Select(x => x.Id).ToListAsync();
                departments = departments.Where(x => x.CareerId.HasValue && careers.Contains(x.CareerId.Value)).ToList();
            }
            else if (User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
            {
                var userId = _userManager.GetUserId(User);
                var academicDepartments = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartmentId).ToListAsync();
                departments = departments.Where(x => academicDepartments.Contains(x.Id)).ToList();
            }

            var result = departments
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .OrderBy(x => x.text)
                .ToList();

            return Ok(result);
        }

        #region Holiday

        /// <summary>
        /// Obtiene el listado de tipos de feriados
        /// </summary>
        /// <returns>Objeto que contiene el listado de tipos de feriados</returns>
        [HttpGet("tipos-feriado")]
        public ActionResult GetHolidayTypes()
        {
            var types = ConstantHelpers.Generals.Holiday.Type.VALUES.Select(x => new
            {
                id = x.Key,
                text = x.Value
            }).ToList();

            var result = new
            {
                items = types
            };

            return Ok(result);
        }

        /// <summary>
        /// Método para descargar un archivo
        /// </summary>
        /// <param name="path">Ruta del archivo</param>
        /// <returns>Archivo</returns>
        [AllowAnonymous]
        [HttpGet("documentos/{*path}")]
        public async Task DownloadDocument(string path)
        {
            using (var mem = new MemoryStream())
            {
                var storage = new CloudStorageService(_storageCredentials);

                await storage.TryDownload(mem, "", path);

                // Download file
                var fileName = Path.GetFileName(path);
                var text = $"inline;filename=\"{fileName.Normalize().Replace(' ', '_')}\"";
                HttpContext.Response.Headers["Content-Disposition"] = text;
                mem.Position = 0;
                await mem.CopyToAsync(HttpContext.Response.Body);
            }
        }

        #endregion

        /// <summary>
        /// Obtiene el listado de clases virtuales en formato para elementos select2
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Objeto que contiene el listado de clases virtuales</returns>
        [HttpGet("get-videoconferencias-select")]
        public async Task<IActionResult> GetVirtualClassSelect(Guid sectionId)
        {
            var videoconferences = await _context.VirtualClass
                .Where(x => x.Content.SectionId == sectionId)
                .Select(x => new
                {
                    text = x.Name,
                    id = x.Id
                })
                .ToListAsync();

            return Ok(videoconferences);
        }

        /// <summary>
        /// Obtiene el listado de tipos de constancias
        /// </summary>
        /// <returns>Objeto que contiene el listado de tipos de constancias</returns>
        [HttpGet("tipos-constancias/get")]
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
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.UPPERFIFTH.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.UPPERFIFTH]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.UPPERTHIRD.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.UPPERTHIRD]
                },
                 new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIES.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIES]
                },
                  new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIESPARTIAL.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIESPARTIAL]
                },
                 new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICRECORD.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICRECORD]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.NODEBT.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.NODEBT]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.CURRICULUM_REVIEW.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.CURRICULUM_REVIEW]
                },
                 new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.FIRST_ENROLLMENT.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.FIRST_ENROLLMENT]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.TENTH_HIGHER.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.TENTH_HIGHER]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.UPPER_MIDDLE.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.UPPER_MIDDLE]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.NOT_BE_PENALIZED.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.NOT_BE_PENALIZED]
                },
                 new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.ENROLLMENT_REPORT.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.ENROLLMENT_REPORT]
                },
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
        /// Obtiene el listado de estados de los trámites de usuarios
        /// </summary>
        /// <returns>Objeto que contiene el listado de estados</returns>
        [HttpGet("status/get")]
        public async Task<IActionResult> GetStatusProcedure()
        {

            var result = ConstantHelpers.USER_PROCEDURES.STATUS.VALUES.Select(x => new
            {
                id = x.Key,
                text = x.Value
            }).ToList();
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de competencias asociadas al plan de estudio
        /// </summary>
        /// <param name="curriculumId">identificador del plan de estudio</param>
        /// <returns>Objeto que contiene el listado de competencias</returns>
        [HttpGet("competencias/{curriculumId}/get")]
        public async Task<IActionResult> GetCompetences(Guid curriculumId)
        {
            var result = await _context.CurriculumCompetencies.Where(x => x.CurriculumId == curriculumId).Select(x => new
            {
                id = x.CompetencieId,
                text = x.Competencie.Name
            }).ToListAsync();

            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de tareas del trámite
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de tareas del trámite</returns>
        [HttpGet("tareas/get")]
        public async Task<IActionResult> GetActivity(string q)
        {
            var query = _context.ActivityProcedures.AsQueryable();
            if (!string.IsNullOrEmpty(q))
                query = query.Where(x => x.Description.Contains(q));
            var result = await query.Select(x => new
            {
                id = x.Id,
                text = x.Description
            }).ToListAsync();
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de requisitos del trámites de usuario
        /// </summary>
        /// <param name="id">identificador del trámite de usuario</param>
        /// <returns>Objeto que contiene el listado de requisitos</returns>
        [HttpGet("dependencia/tramites/usuarios/get-req/{id}")]
        public async Task<IActionResult> GetRequerimentProcedure(Guid id)
        {
            var sentParameter = _dataTablesService.GetSentParameters();
            var result = await _userProcedureService.GetRequerimentProcedure(sentParameter, id, CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.STATUS.SENT);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de observaciones del trámite de usuario
        /// </summary>
        /// <param name="id">Identificador del trámite de usuario</param>
        /// <returns>Objeto que contiene el listado de observaciones</returns>
        [HttpGet("dependencia/tramites/usuarios/get-observation-file/{id}")]
        public async Task<IActionResult> GetObservationFile(Guid id)
        {
            var result = await _userProcedureService.GetRequerimentProcedureList(id, CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.STATUS.RESOLVED);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de los archivos de las derivaciones del trámite de usuario
        /// </summary>
        /// <param name="upid">Identificador del trámite de usuario</param>
        /// <returns>Objeto que contiene el listado de los archivos de las derivaciones del trámite de usuario</returns>
        [Route("dependencia/tramites/usuarios/derivaciones/archivos/{upid}/get")]
        public async Task<IActionResult> GetUserProcedureDerivationFilesByUserProcedure(Guid upid)
        {
            var result = await _userProcedureDerivationFileService.GetUserProcedureDerivationFilesByUserProcedure(upid);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de las derivaciones del trámite de usuario
        /// </summary>
        /// <param name="upid">Identificador del trámite de usuario</param>
        /// <returns>Objeto que contiene el listado de las derivaciones</returns>
        [Route("dependencia/tramites/usuarios/derivaciones/{upid}/get")]
        public async Task<IActionResult> GetUserProcedureDerivationsByUserProcedure(Guid upid)
        {
            var result = await _userProcedureDerivationService.GetUserProcedureDerivationsByUserProcedure(upid);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de trámites finalizados
        /// </summary>
        /// <param name="upid">Identificador del trámite de usuario</param>
        /// <returns>Objeto que contiene el listado de trámites finalizados</returns>
        [HttpGet("dependencia/tramites/usuarios/derivaciones/finalizado/{upid}/get")]
        public async Task<IActionResult> GetUserProcedureByUserProcedureFinish(Guid upid)
        {
            var result = await _userProcedureService.GetFinishByUserProcedure(upid);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de derivaciones de un trámite
        /// </summary>
        /// <param name="upid">Identificador del trámite de usuario</param>
        /// <returns>Objeto que contiene el listado de derivaciones de un trámite</returns>
        [HttpGet("dependencia/tramites/usuarios/derivaciones/todos/{upid}/get")]
        public async Task<IActionResult> GetUserProcedureByUserProcedureAll(Guid upid)
        {
            var resultIsFinishedList = await _userProcedureService.GetFinishByUserProcedure(upid);
            var resultAllDerivations = await _userProcedureDerivationService.GetUserProcedureDerivationsByUserProcedure(upid);

            return Ok(new { itsFinished = resultIsFinishedList.Count() > 0, derivationFinishedList = resultIsFinishedList, derivationList = resultAllDerivations });
        }

        /// <summary>
        /// Obtiene el listado de dependencias asociadas a un trámite
        /// </summary>
        /// <param name="pid">Identificador del trámite</param>
        /// <returns>Objeto que contiene el listado de dependencias</returns>
        [Route("dependencia/tramites/dependencias/{pid}/get")]
        public async Task<IActionResult> GetProcedureDependenciesByProcedure(Guid pid)
        {
            var result = await _procedureDependencyService.GetProcedureDependenciesByProcedure(pid);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de certificados por curso en formato para elementos select2
        /// </summary>
        /// <returns>Objeto que contiene el listado de certificados por curso</returns>
        [HttpGet("certificados-cursos")]
        public async Task<IActionResult> GetCourseCertificates()
        {
            var result = (await _courseCertificateService.GetAll())
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToList();

            return Ok(new { items = result });
        }


        /// <summary>
        /// Obtiene el listado de tipos de evaluaciones
        /// </summary>
        /// <returns>Objeto que contiene el listado de tipos de evaluaciones</returns>
        [HttpGet("get-tipos-evaluaciones")]
        public async Task<IActionResult> GetEvaluationsTypesSelect2()
        {
            var result = await _evaluationTypeService.GetEvaluationTypeJson();
            return Ok(result);
        }


        /// <summary>
        /// Obtiene los ciclos concluidos del alumno
        /// </summary>
        /// <returns>Listado de ciclos</returns>
        [HttpGet("get-ciclos-alumno")]
        public async Task<IActionResult> GetAcademicYearsByUser()
        {
            var user = await _userService.GetUserByClaim(User);
            var student = await _studentService.GetStudentByUser(user.Id);
            var curriculumId = await _context.Students.Where(x => x.Id == student.Id).Select(x => x.CurriculumId).FirstOrDefaultAsync();
            var data = await _context.AcademicYearCourses.Where(x => x.CurriculumId == curriculumId).Select(x => x.AcademicYear).ToListAsync();
            var result = data
                .Distinct()
                .Select(x => new
                {
                    id = x,
                    text = ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x]
                })
                .OrderBy(x => x.id)
                .ToList();

            return Ok(result);
        }

        [HttpGet("cursos-matriculados-usuario")]
        public async Task<IActionResult> GetEnrolledCoursesByUser()
        {
            if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS))
            {
                var user = await _userService.GetUserByClaim(User);
                var student = await _studentService.GetStudentByUser(user.Id);
                var term = await _termService.GetActiveTerm();
                if (term == null)
                    term = new Term();
                var result = await _studentSectionService.GetCoursesSelect2ClientSide(student.Id, term.Id, true);
                return Ok(result);
            }

            return Ok();
        }

        [HttpGet("get-cursos-disponibles-recuperacion-nota")]
        public async Task<IActionResult> GetEnrolledCoursesToGradeRecovery(Guid? studentId)
        {
            var data = new List<AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student.EnrolledCourseTemplate>();

            if (studentId.HasValue)
            {
                data = await _studentService.GetEnrolledCoursesToGradeRecovery(studentId.Value);
            }
            else
            {
                if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                {
                    var user = await _userService.GetUserByClaim(User);
                    var student = await _studentService.GetStudentByUser(user.Id);
                    data = await _studentService.GetEnrolledCoursesToGradeRecovery(student.Id);
                }
            }

            var result = data.Select(x => new
            {
                Id = x.StudentSectionId,
                Text = x.CourseName
            }).ToList();

            return Ok(result);
        }

        [HttpGet("get-cursos-disponibles-examen-sustitutorio")]
        public async Task<IActionResult> GetEnrolledCoursesAvailableToSubstitueExam(Guid? studentId)
        {
            var data = new List<AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student.EnrolledCourseTemplate>();

            if (studentId.HasValue)
            {
                data = await _studentService.GetEnrolledCoursesAvailableToSubstitueExam(studentId.Value);
            }
            else
            {
                if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                {
                    var user = await _userService.GetUserByClaim(User);
                    var student = await _studentService.GetStudentByUser(user.Id);
                    data = await _studentService.GetEnrolledCoursesAvailableToSubstitueExam(student.Id);
                }
            }

            var result = data.Select(x => new
            {
                Id = x.StudentSectionId,
                Text = x.CourseName
            }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Retorna la lista de conceptos con sus planes contables 
        /// </summary>
        /// <param name="term">Término a filtrar</param>
        /// <returns>Objeto con la lista de conceptos con planes contables</returns>
        [HttpGet("conceptos/buscar")]
        public async Task<IActionResult> GetConceptsByTerm(string term)
        {
            var result = await _conceptService.GetConceptsWithAccountingPlanJson(term);
            return Ok(new { items = result });
        }


        [HttpGet("get-cursos-disponibles-tramite-exoneracion")]
        public async Task<IActionResult> GetCoursesAvailableForExoneratedCourse(Guid? studentId)
        {
            var data = new List<AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student.EnrolledCourseTemplate>();

            if (studentId.HasValue)
            {
                data = await _studentService.GetCoursesAvailableForExoneratedCourse(studentId.Value);
            }
            else
            {
                if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                {
                    var user = await _userService.GetUserByClaim(User);
                    var student = await _studentService.GetStudentByUser(user.Id);
                    data = await _studentService.GetCoursesAvailableForExoneratedCourse(student.Id);
                }
            }

            var result = data.Select(x => new
            {
                Id = x.CourseId,
                Text = x.CourseName
            }).ToList();

            return Ok(result);
        }

        [HttpGet("get-cursos-disponibles-tramite-evaluacion-extraordinaria")]
        public async Task<IActionResult> GetCoursesAvailableForExtraordinaryEvaluation(Guid? studentId)
        {
            var data = new List<AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student.EnrolledCourseTemplate>();

            if (studentId.HasValue)
            {
                data = await _studentService.GetCoursesAvailableForExtraordinaryEvaluation(studentId.Value);
            }
            else
            {
                if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                {
                    var user = await _userService.GetUserByClaim(User);
                    var student = await _studentService.GetStudentByUser(user.Id);
                    data = await _studentService.GetCoursesAvailableForExtraordinaryEvaluation(student.Id);
                }
            }

            var result = data.Select(x => new
            {
                Id = x.CourseId,
                Text = x.CourseName
            }).ToList();

            return Ok(result);
        }

        [HttpGet("beneficios-estudiantes/get")]
        public async Task<IActionResult> GetStudentBenefits(Guid? studentId)
        {
            var benefits = ConstantHelpers.Student.Benefit.VALUES
                .Select(x => new
                {
                    id = x.Key,
                    text = x.Value
                }).ToList();

            var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == studentId);

            return Ok(new { items = benefits, selected = student?.Benefit });
        }

        [HttpGet("get-escalas-estudiantes")]
        public async Task<IActionResult> GetStudentScalesSelect2()
        {
            var data = await _studentScaleService.GetAll();

            var result = data
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToList();

            return Ok(new { items = result });
        }

        [HttpGet("get-pensiones-matricula")]
        public async Task<IActionResult> GetEnrollmentFeesSelect2()
        {
            var result = await _enrollmentFeeService.GetAllSelect2Data();
            return Ok(new { items = result });
        }

        [HttpGet("get-sub-grupos-seccion")]
        public async Task<IActionResult> GetSubGroupsBySection(Guid sectionId)
        {
            var result = await _sectionGroupService.GetSectionGroupBySectionSelect2lientSide(sectionId);
            return Ok(result);
        }
    }
}
