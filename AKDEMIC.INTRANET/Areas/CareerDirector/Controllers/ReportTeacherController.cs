using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)]
    [Area("CareerDirector")]
    [Route("director-carrera/reporte_docentes")]
    public class ReportTeacherController : BaseController
    {
        private readonly ISelect2Service _select2Service;
        private readonly ICourseService _courseService;
        private readonly IDataTablesService _dataTablesService;

        public ReportTeacherController(UserManager<ApplicationUser> userManager,ITermService termService,
            ISelect2Service select2Service,ICourseService courseService,IDataTablesService dataTablesService) : base(userManager,termService)
        {
            _select2Service = select2Service;
            _courseService = courseService;
            _dataTablesService = dataTablesService;
        }

        /// <summary>
        /// Vista donde se muestra el reporte docente
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            var Term = await GetActiveTerm();
            ViewBag.DefaultTermId = Term.Id;
            return View();
        }

        /// <summary>
        /// Obtiene el listado de periodos académicos para ser usado en select
        /// </summary>
        /// <returns>Listado de periodos académicos</returns>
        [HttpGet("getTerms")]
        public async Task<IActionResult> GetTerms()
        {
            var result = await _termService.GetTermsSelect2ClientSide();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de docentes para ser usado en tablas
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="name">Texto de búsqueda</param>
        /// <returns>Listado de docentes</returns>
        [HttpGet("getTeachers")]
        public async Task<IActionResult> GetTeachers(Guid termId,string name)
        {
            var paginationParameter = _dataTablesService.GetSentParameters();
            var result = await _courseService.GetAllByTermIdAndPaginationParameters(paginationParameter, termId, name);
            return Ok(result);
        }

    }
}
