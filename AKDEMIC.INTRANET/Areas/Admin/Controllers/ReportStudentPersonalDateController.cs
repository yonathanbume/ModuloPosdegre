using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{

    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.INSTITUTIONAL_WELFARE + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE)]
    [Area("Admin")]
    [Route("admin/reporte_alumnos_datos_personales")]
    public class ReportStudentPersonalDateController : INTRANET.Controllers.BaseController
    {
        private readonly ISelect2Service _select2Service;
        private readonly IDataTablesService _dataTablesService;
        private readonly IStudentService _studentService;

        public ReportStudentPersonalDateController(
                         IDataTablesService dataTablesService,
            ISelect2Service select2Service,
            IStudentService studentService)
        {
            _select2Service = select2Service;
            _dataTablesService = dataTablesService;
            _studentService = studentService;
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
        /// Obtiene el listado de estudiantes para ser usado en tablas
        /// </summary>
        /// <param name="caid">Identificador de la escuela profesional</param>
        /// <param name="did">Identificador del departamento</param>
        /// <param name="six">Sexo</param>
        /// <param name="scid">Tipo de escuela</param>
        /// <param name="upid">Preparación universitaria</param>
        /// <param name="atid">Identificador de la modalidad de ingreso</param>
        /// <param name="starage">Diferencia de edad</param>
        /// <param name="endage">Edad final</param>
        /// <returns>Objeto que contiene el listado de estudiantes</returns>
        [HttpGet("get/{caid?}/{did?}/{six?}/{scid?}/{upid?}/{atid?}/{starage?}/{endage?}")]
        public async Task<IActionResult> GetReportPersonalInformation(Guid? caid, Guid? did, int? six, int? scid, int? upid, Guid? atid, int? starage, int? endage)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetReportPersonalInformation(parameters, caid, did, null, null, six, scid, upid, atid, starage, endage);
            return Ok(result);
        }
    }
}
