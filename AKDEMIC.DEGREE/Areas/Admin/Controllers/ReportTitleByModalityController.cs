using AKDEMIC.CORE.Helpers;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR+ "," + ConstantHelpers.ROLES.REPORT_QUERIES)]
    [Area("Admin")]
    [Route("admin/reporte-alumnos-titulados-segun-modalidad")]
    public class ReportTitleByModalityController : BaseController
    {
        private readonly IRegistryPatternService _registryPatternService;
        public ReportTitleByModalityController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            ICareerService careerService,
            IRegistryPatternService registryPatternService,
            IConfigurationService configurationService) : base(careerService, configurationService, userManager)
        {
            _registryPatternService = registryPatternService;
        }

        /// <summary>
        /// Vista principal donde se muestra el reporte de estudiantes según modalidad de solicitud
        /// </summary>
        /// <returns>Retorna la vista</returns>
        public IActionResult Index()
        {
            if (User.IsInRole(ConstantHelpers.ROLES.SUPERADMIN) || User.IsInRole(ConstantHelpers.ROLES.DEGREE_ADMIN))
            {
                ViewBag.CareerId = new Guid();
            }

            return View();

        }

        /// <summary>
        /// Obtiene el reporte de estudiantes según modalidad de solicitud
        /// </summary>
        /// <param name="startDate">Fec. Inicio</param>
        /// <param name="endDate">Fec. Fin</param>
        /// <returns>Retorna un Ok con la información de los estudiantes para su uso en gráficos</returns>
        [HttpGet("reporte-titulados-segun-modalidad")]
        public async Task<IActionResult> GetQualifiersByModality(string startDate , string endDate)
        {
            if (User.IsInRole(ConstantHelpers.ROLES.SUPERADMIN) || User.IsInRole(ConstantHelpers.ROLES.DEGREE_ADMIN))
            {
                var result = await _registryPatternService.GetReportProfessionalTitleByModality(startDate, endDate);
                return Ok(result);
            }
            else
            {
                var values2 = await GetCareersAndFacultiesByAcademicCoordinator();
                var result = await _registryPatternService.GetReportProfessionalTitleByModality(startDate, endDate,values2.Item1.Select(x => x.Id).ToList());
                return Ok(result);
            }

        }
    }
}
