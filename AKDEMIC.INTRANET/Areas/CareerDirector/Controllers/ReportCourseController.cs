using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Areas.CareerDirector.Models.Report_courseViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)]
    [Area("CareerDirector")]
    [Route("director-carrera/reporte_asistencia_curso")]
    public class ReportCourseController : BaseController
    {
        private readonly ISelect2Service _select2Service;
        private readonly IDataTablesService _dataTablesService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly ITermService _termService;
        private readonly ICourseTermService _courseTermService;

        public ReportCourseController(
            IDataTablesService dataTablesService,
            ISelect2Service select2Service,
             IStudentSectionService studentSectionService,
             ICourseTermService courseTermService,
            ITermService termService
            ) : base()
        {
            _studentSectionService = studentSectionService;
            _termService = termService;
            _courseTermService = courseTermService;
            _select2Service = select2Service;
            _dataTablesService = dataTablesService;
        }

        /// <summary>
        /// Vista donde se muestra el reporte de asistencia por curso
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de cursos aperturados filtrados por los siguientes parámetros para ser usado en tablas.
        /// </summary>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <param name="carId">Identificador del curso</param>
        /// <param name="curId">Identificador del plan de estudio</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de cursos</returns>
        [HttpGet("curso/periodo")]
        public async Task<IActionResult> GetCourseTermData(Guid cid, Guid pid, Guid carId, Guid curId, string search = null)
        {
            if (cid.Equals(Guid.Empty))
                return NotFound($"No se pudo encontrar una carrera con el id '{cid}'.");
            if (pid.Equals(Guid.Empty))
                return NotFound($"No se pudo encontrar un periodo con el id '{pid}'.");
            var parameters = _dataTablesService.GetSentParameters();
            var query = await _courseTermService.GetCourseTermDataTable(parameters, carId, curId, cid, pid, search);
            return Ok(query);
        }

        /// <summary>
        /// Obtiene el listado de cursos aperturados para ser usado en select
        /// </summary>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <returns>Listado de cursos</returns>
        [HttpGet("cursos/periodo/{pid}")]
        public async Task<IActionResult> GetCourseTermSelect(Guid pid)
        {
            if (pid.Equals(Guid.Empty))
                return NotFound($"No se pudo encontrar un periodo con el id '{pid}'.");

            var result = await _courseTermService.GetCourseTermByTermSelect2ClientSide(pid);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de estudiantes matriculados para ser usado en tablas.
        /// </summary>
        /// <param name="ctid">Identificador del curso-periodo</param>
        /// <returns>Listado de estudiantes</returns>
        [HttpGet("reporte_curso_detalles")]
        public async Task<IActionResult> GetReportCourseDetail(Guid ctid)
        {
            if (ctid.Equals(Guid.Empty))
                return NotFound($"No se pudo encontrar un courseTerm con el id '{ctid}'.");

            var parameters = _dataTablesService.GetSentParameters();
            var query = await _studentSectionService.GetReportCourseDetailDataTable(parameters, ctid);
            return Ok(query);
        }

        /// <summary>
        /// Vista donde se muestra el listado de alumnos matriculados
        /// </summary>
        /// <param name="ctid">Identificador del curso-periodo.</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("reporte_curso_vista/{ctid}")]
        public async Task<IActionResult> ReportCourseView(Guid ctid)
        {
            if (ctid.Equals(Guid.Empty))
                return NotFound($"No se pudo encontrar un courseTerm con el id '{ctid}'.");

            var result = await _courseTermService.GetCourseTermWithCourse(ctid);

            ReportCourseViewModel modelResult = new ReportCourseViewModel()
            {
                IdCourseTerm = ctid,
                Name = result.Course.Name,
                Code = result.Course.Code,
                Credits = result.Course.Credits
            };
            return View(modelResult);
        }

        /// <summary>
        /// Obtiene el listado de periodos académicos para ser usado en select
        /// </summary>
        /// <returns>Listado de periodos académicos</returns>
        [HttpGet("periodos/get")]
        public async Task<IActionResult> GetTerms()
        {
            var requestParameters = _select2Service.GetRequestParameters();
            var courses = await _termService.GetTermsSelect2(requestParameters);

            return Ok(courses);
        }
    }
}
