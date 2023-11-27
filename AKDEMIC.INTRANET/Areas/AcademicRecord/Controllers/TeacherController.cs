using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF)]
    [Area("AcademicRecord")]
    [Route("registrosacademicos/docentes")]
    public class TeacherController : BaseController
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService, IDataTablesService dataTablesService) : base(dataTablesService)
        {
            _teacherService = teacherService;
        }

        /// <summary>
        /// Vista donde se muestra el listado de docentes
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de docentes para ser usado en tablas
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de docentes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(string search, Guid? facultyId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _teacherService.GetTeacherByClaimDatatable(sentParameters, User, facultyId, search);
            return Ok(result);
        }
    }
}
