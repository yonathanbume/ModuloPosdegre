using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.CareerDirector.Models.ReportViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)]
    [Area("CareerDirector")]
    [Route("director-carrera/reporte_asistencia")]
    public class ReportController : BaseController
    {
        private readonly ISelect2Service _select2Service;
        private readonly IDataTablesService _dataTablesService;
        private readonly IAcademicProgramService _academicProgramService;
        private readonly IFacultyService _facultyService;
        private readonly ICareerService _careerService;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        private readonly ITermService _termService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IClassStudentService _classStudentService;


        public ReportController(UserManager<ApplicationUser> userManager,
            IDataTablesService dataTablesService,
            ISelect2Service select2Service,
            IAcademicProgramService academicProgramService,
            IFacultyService facultyService,
            ICareerService careerService,
            IStudentService studentService,
            IStudentSectionService studentSectionService,
            IUserService userService,
            ITermService termService,
            IClassStudentService classStudentService
            ) : base(userManager)
        {
            _academicProgramService = academicProgramService;
            _facultyService = facultyService;
            _careerService = careerService;
            _studentService = studentService;
            _studentSectionService = studentSectionService;
            _userService = userService;
            _termService = termService;
            _classStudentService = classStudentService;
            _select2Service = select2Service;
            _dataTablesService = dataTablesService;
        }

        /// <summary>
        /// Vista donde se muestra el reporte de asistencia de estudiantes
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de estudiantes filtrados por los siguientes parámetros para ser usado en tablas
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="fid">Identificador de la facultad</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <returns>Listado de estudiantes</returns>
        [HttpGet("carrera/facultad")]
        public async Task<IActionResult> GetStudentFilter(string search, Guid? cid, Guid? fid, Guid? pid)
        {
            if (cid.Equals(Guid.Empty))
                return NotFound($"No se pudo encontrar un Carrera con el id '{cid}'.");
            if (fid.Equals(Guid.Empty))
                return NotFound($"No se pudo encontrar una Facultad con el id '{fid}'.");

            /****/
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetStudentFilterDatatable(parameters, cid, fid, pid, search);
            return Ok(result);
            /****/
        }

        /// <summary>
        /// Obtiene el listado de programas académicos para ser usado en select
        /// </summary>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <returns>Listado de programas académicos</returns>
        [HttpGet("getprograms/{cid}")]
        public async Task<IActionResult> GetPrograms(Guid cid)
        {
            var requestParameters = _select2Service.GetRequestParameters();
            var result = await _academicProgramService.GetAcademicProgramByCareerSelect2(requestParameters, null, cid);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales para ser usado en select
        /// </summary>
        /// <param name="fid">Identificador de la facultad</param>
        /// <returns>Listado de escuelas profesionas</returns>
        [HttpGet("getcareers/{fid}")]
        public async Task<IActionResult> GetCareers(Guid fid)
        {
            var requestParameters = _select2Service.GetRequestParameters();
            var result = await _careerService.GetCareerByUserCoordinatorIdSelect2(requestParameters, _userManager.GetUserId(User), null, null, fid);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de facultades para ser usado en select
        /// </summary>
        /// <returns>Listado de facultades</returns>
        [HttpGet("getfaculties")]
        public async Task<IActionResult> GetFaculties()
        {
            var requestParameters = _select2Service.GetRequestParameters();
            var result = await _facultyService.GetFacultiesByAcademicCoordinatorSelect2(requestParameters, _userManager.GetUserId(User));
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se muestra el detalle del estudiante
        /// </summary>
        /// <param name="sid">Identificador del estudiante</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("{sid}")]
        public async Task<IActionResult> GetReportView(Guid sid)
        {
            var student = await _studentService.GetStudentWithCareerAndUser(sid);

            IndexViewModel model = new IndexViewModel()
            {
                Student = new StudentViewModel()
                {
                    StudentId = student.Id,
                    FullName = student.User.FullName,
                    UserName = student.User.UserName,
                    Career = new CareerViewModel()
                    {
                        Name = student.Career.Name
                    }
                },
                ActiveTerm = (await _termService.GetActiveTerm()).Id,
                AttendanceMinPercentage = ConstantHelpers.COURSES.ATTENDANCE_MIN_PERCENTAGE,
                Terms = (await _termService.GetAll()).Select(x => new TermViewModel()
                {
                    Name = x.Name,
                    Id = x.Id
                }),
            };
            return View(model);
        }

        /// <summary>
        /// Obtiene el listado de cursos matriculados
        /// </summary>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <param name="sid">identificador del estudiante</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de cursos matriculados</returns>
        [HttpGet("periodo/get")]
        public async Task<IActionResult> GetStudentCourses(Guid pid, Guid sid, string search)
        {
            if (pid.Equals(Guid.Empty))
                return BadRequest($"No se pudo encontrar un Periodo con el id {pid}.");
            if (sid.Equals(Guid.Empty))
                return BadRequest($"No se pudo encontrar un Estudiante con el id {sid}.");
            /****/
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _studentSectionService.GetStudentSectionDatatable(parameters, sid, pid, search);
            return Ok(result);
            /****/
        }

        /// <summary>
        /// Obtiene el listado de clases del alumno para ser usado en tablas
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <param name="aid">Identificador del estudiante</param>
        /// <param name="filter">Asistencia o falta</param>
        /// <returns>Listado de clases</returns>
        [HttpGet("seccion/alumno/get")]
        public async Task<IActionResult> GetSectionAbsenceDetail(Guid sid, Guid aid, int filter)
        {
            if (sid.Equals(Guid.Empty))
                return BadRequest($"No se pudo encontrar una Sección con el id {sid}.");
            if (aid.Equals(Guid.Empty))
                return BadRequest($"No se pudo encontrar el estudiante con el id {aid}.");

            /****/
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _classStudentService.GetSectionAbsenceDetailDataTable(parameters, sid, aid, filter);
            return Ok(result);
            /****/
        }

        /// <summary>
        /// Obtiene el listado de periodos académicos para ser usado en select
        /// </summary>
        /// <returns>Listado de periodos académicos</returns>
        [HttpGet("periodos/get")]
        public async Task<IActionResult> GetTerms()
        {
            var requestParameters = _select2Service.GetRequestParameters();
            var terms = await _termService.GetTermsSelect2(requestParameters);

            return Ok(terms);
        }
    }
}
