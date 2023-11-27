using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_SECRETARY + "," +
        ConstantHelpers.ROLES.DEAN + "," +
        ConstantHelpers.ROLES.DEAN_SECRETARY + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        ConstantHelpers.ROLES.VICERRECTOR)]
    [Area("CareerDirector")]
    [Route("director-carrera/reporte-aprobados-desaprobados")]
    public class ReportApprovedController : BaseController
    {
        private readonly ISelect2Service _select2Service;
        private readonly IDataTablesService _dataTablesService;
        private readonly ICareerService _careerService;
        private readonly IAcademicSummariesService _academicSummariesService;

        public ReportApprovedController(UserManager<ApplicationUser> userManager,
            IDataTablesService dataTablesService,
            ISelect2Service select2Service,
            ICareerService careerService,
             IAcademicSummariesService academicSummariesService
            ) : base(userManager)
        {
            _select2Service = select2Service;
            _dataTablesService = dataTablesService;
            _careerService = careerService;
            _academicSummariesService = academicSummariesService;
        }

        /// <summary>
        /// vista donda se muestra el reporte de aprobados vs desaprobdos
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listdo de escuelas profesionales para ser usado en select
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de escuelas profesionales</returns>
        [HttpGet("getCarreras")]
        public async Task<IActionResult> GetCareers(string search)
        {
            var requestParameters = _select2Service.GetRequestParameters();
            var userId = _userManager.GetUserId(User);
            if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var resultdean = await _careerService.GetCareerSelect2(requestParameters, search, null, User);
                return Ok(resultdean);
            }

            var result = await _careerService.GetCareerByUserCoordinatorIdSelect2(requestParameters, userId, search);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de estudiantes donde se detalla su estado (cursos aprobados, cursos desaprobaados) para ser usado en tablas.
        /// </summary>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <param name="name">Texto de búsqueda</param>
        /// <returns>Listado de estudiantes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStatusStudent(Guid cid, Guid pid, string name)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _academicSummariesService.GetAcademicSummaryDatatable(parameters, cid, pid, name);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene la cantidad de alumnos aprobados vs desaprobados.
        /// </summary>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene la información de aprobados y desaprobados</returns>
        [HttpGet("chart/{cid}/{pid}")]
        public async Task<IActionResult> GetChartReport(Guid cid, Guid pid)
        {
            var result = await _academicSummariesService.GetAcademicSummariesReportAsData(cid, pid);
            return Ok(result);
        }
    }
}
