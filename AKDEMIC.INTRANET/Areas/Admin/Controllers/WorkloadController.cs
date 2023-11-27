using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN
        + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE
        + "," + ConstantHelpers.ROLES.INTRANET_ADMIN
        + "+" + ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR
        + "," + ConstantHelpers.ROLES.CAREER_DIRECTOR
        + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR
        + "," + ConstantHelpers.ROLES.REPORT_QUERIES)]
    [Area("Admin")]
    [Route("admin/carga-lectiva")]
    public class WorkloadController : BaseController
    {
        private readonly ICourseService _courseService;
        private readonly ISectionService _sectionService;
        private readonly IClassService _classService;
        private readonly IStudentService _studentService;
        private readonly IEvaluationService _evaluationService;

        public WorkloadController(ICourseService courseService, ISectionService sectionService,
            IClassService classService,
            IStudentService studentService, IEvaluationService evaluationService, ITermService termService) : base(
            termService)
        {
            _courseService = courseService;
            _sectionService = sectionService;
            _classService = classService;
            _studentService = studentService;
            _evaluationService = evaluationService;
        }

        /// <summary>
        /// Vista donde se detalla la carga lectiva del docente
        /// </summary>
        /// <returns>Vista principal del controladaor</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de cursos asignados al docente en el periodo activo.
        /// </summary>
        /// <param name="id">Identificador del docente</param>
        /// <returns>Objeto que contiene el listado de cursos</returns>
        [HttpGet("{id}/cursos/get")]
        public async Task<IActionResult> GetCourses(string id)
        {
            var term = await GetActiveTerm();
            if (term == null)
                term = await _termService.GetLastTerm();
            if (term == null)
                term = new ENTITIES.Models.Enrollment.Term();
            var result = await _courseService.GetAllByTeacherId(id, term.Id);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de secciones asignados al docente en el periodo activo.
        /// </summary>
        /// <param name="cid">Identificador del curso</param>
        /// <param name="id">Identificador del docente</param>
        /// <returns>Objeto que contiene el listado de secciones</returns>
        [HttpGet("{id}/cursos/{cid}/secciones/get")]
        public async Task<IActionResult> GetSections(Guid cid, string id)
        {
            var term = await GetActiveTerm();
            var result = await _sectionService.GetAllByCourseAndTerm(cid, term.Id, id);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de estudiantes matriculados de una sección
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <returns>Objeto que contiene el listado de estudiantes</returns>
        [HttpGet("{id}/cursos/{cid}/secciones/{sid}/alumnos/get")]
        public async Task<IActionResult> GetStudents(Guid sid)
        {
            var result = await _studentService.GetAllBySectionId(sid);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de evaluaciones asignados al curso
        /// </summary>
        /// <param name="cid">Identificador del curso</param>
        /// <param name="sid">Identificador de la sección</param>
        /// <returns>Objeto que contiene el listado de evaluaciones</returns>
        [HttpGet("{id}/cursos/{cid}/secciones/{sid}/evaluaciones/get")]
        public async Task<IActionResult> GetEvaluations(Guid cid, Guid sid)
        {
            var term = await GetActiveTerm();
            var result = await _evaluationService.GetAllByCourseAndTermWithTaken(cid, term.Id, sid);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de clases asignados al docente
        /// </summary>
        /// <param name="id">Identificador del docente</param>
        /// <param name="start">Fecha de inicio</param>
        /// <param name="end">Fecha fin</param>
        /// <returns>Obtiene el listado de clases</returns>
        [HttpGet("get-horario/{id}")]
        public async Task<IActionResult> GetClasses(string id, DateTime start, DateTime end)
        {
            var dateStart = start.ToUniversalTime();
            var dateEnd = end.ToUniversalTime();

            var term = await GetActiveTerm();
            if (term == null)
                return BadRequest();

            id = id ?? _userManager.GetUserId(User);

            return Ok(await _classService.GetAllByTermIdTeacherIdAndDateRange(term.Id, id, dateStart, dateEnd));
        }

    }
}
