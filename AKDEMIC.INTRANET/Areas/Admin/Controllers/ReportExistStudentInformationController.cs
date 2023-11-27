using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/reporte_estudiante_fichas")]
    public class ReportExistStudentInformationController : BaseController
    {
        private readonly ISelect2Service _select2Service;
        private readonly IDataTablesService _dataTablesService;
        private readonly IStudentService _studentService;
        private readonly ICareerService _careerService;

        public ReportExistStudentInformationController(
             IDataTablesService dataTablesService,
            ISelect2Service select2Service,
            IStudentService studentService,
             ICareerService careerService
            )
        {
            _select2Service = select2Service;
            _dataTablesService = dataTablesService;
            _studentService = studentService;
            _careerService = careerService;
        }

        /// <summary>
        /// Vista principal del controlador
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de estudiantes
        /// </summary>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <returns>Listado de estudiantes</returns>
        [HttpGet("get/{cid}")]
        public async Task<IActionResult> GetStudentInformationFile(Guid cid)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetStudentInformationDataTable(parameters, cid);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <param name="fid">Identificador de la facultad</param>
        /// <returns>Objeto que contiene el listado de escuelas profesionales</returns>
        [HttpGet("carreras/get")]
        public async Task<IActionResult> GetCareers(string q, [FromQuery] Guid? fid)
        {
            var requestParameters = _select2Service.GetRequestParameters();
            var result = await _careerService.GetCareerSelect2(requestParameters, q);
            return Ok(result);
        }
    }
}
