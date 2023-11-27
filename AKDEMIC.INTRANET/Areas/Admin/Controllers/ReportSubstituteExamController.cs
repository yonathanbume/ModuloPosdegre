using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/reporte-examen-sustitutorio")]
    public class ReportSubstituteExamController : BaseController
    {
        private readonly ISectionService _sectionService;
        private readonly IDataTablesService _dataTablesService;
        private readonly ITermService _termService;

        public ReportSubstituteExamController(
            ISectionService sectionService,
            IDataTablesService dataTablesService,
            ITermService termService
            )
        {
            _sectionService = sectionService;
            _dataTablesService = dataTablesService;
            _termService = termService;
        }

        /// <summary>
        /// Vista donde se listan las secciones habilitades para examen sustitutorio
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        [HttpGet("secciones-habilitadas")]
        public IActionResult EnabledSections()
            => View();

        /// <summary>
        /// Obtiene el listado de secciones habilitadas para examen sustitutorio
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de secciones habilitadas para examen sustitutorio</returns>
        [HttpGet("get-secciones-habilitadas")]
        public async Task<IActionResult> GetEnabledSections(Guid? termId, Guid? careerId, Guid? curriculumId, string searchValue)
        {
            var parameters = _dataTablesService.GetSentParameters();

            if (!termId.HasValue || termId == Guid.Empty)
            {
                var termActive = await _termService.GetActiveTerm();
                termId = termActive.Id;
            }

            var result = await _sectionService.GetSectionsEnabledForSubstituteExam(parameters, termId.Value, careerId, curriculumId, searchValue);
            return Ok(result);
        }
    }
}
