using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," + ConstantHelpers.ROLES.VICERRECTOR)]
    [Area("CareerDirector")]
    [Route("director-carrera/reporte-aprobados-desaprobados-cursos")]
    public class ReportApprovedCourseController : BaseController
    {
        private readonly ISelect2Service _select2Service;
        private readonly IDataTablesService _dataTablesService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly ICourseTermService _courseTermService;

        public ReportApprovedCourseController(UserManager<ApplicationUser> userManager,
            IDataTablesService dataTablesService,
            ISelect2Service select2Service,
            IAcademicHistoryService academicHistoryService,
             ICourseTermService courseTermService
            ) : base(userManager)
        {
            _select2Service = select2Service;
            _dataTablesService = dataTablesService;
            _academicHistoryService = academicHistoryService;
            _courseTermService = courseTermService;
        }

        /// <summary>
        /// Vista donde se muestra el reporte de aprobados vs desaprobados
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de cursos aperturados para ser usado en select
        /// </summary>
        /// <param name="cid">Identificador del periodo académico</param>
        /// <returns>Listado de cursos</returns>
        [HttpGet("getCourses/{cid}")]
        public async Task<IActionResult> GetCourses(Guid cid)
        {
            var userId = _userManager.GetUserId(User);
            var courses = await _courseTermService.GetCourseTermByTermSelect2ClientSide(cid, userId);
            return Ok(courses);
        }

        /// <summary>
        /// Obtiene el listado de resumenes académicos de los estudiantes para ser usado en tablas
        /// </summary>
        /// <param name="cid">Identificador del plan de estudio</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <param name="carId">Identificador de la escuela profesional</param>
        /// <param name="curId">Identificador del curso </param>
        /// <param name="name">Texto de búsqueda</param>
        /// <returns>Listado de resumenes académicos</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStudents(Guid cid, Guid pid, Guid carId, Guid curId, string name)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _academicHistoryService.GetAcademicHistoryDatatable(parameters, carId, curId, cid, pid, name);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el reporte de los resumenes académicos
        /// </summary>
        /// <param name="carId">Identificador de la escuela profesional</param>
        /// <param name="curId">Identificador del curso</param>
        /// <param name="cid">Identificador del plan de estudio</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene los datos de los resumenes académicos</returns>
        [HttpGet("chart/{carId}/{curId}/{cid}/{pid}")]
        public async Task<IActionResult> GetChartReport(Guid carId, Guid curId, Guid cid, Guid pid)
        {
            var result = await _academicHistoryService.GetAcademicHistoriesReportAsData(carId, curId, cid, pid);
            return Ok(result);
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
