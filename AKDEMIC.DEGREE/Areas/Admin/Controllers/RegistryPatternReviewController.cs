using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/gestion-aprobacion-registros-padrones")]
    public class RegistryPatternReviewController : BaseController
    {
        private readonly IRegistryPatternService _registryPatternService;
        private readonly IProcedureService _procedureService;
        private readonly IUserProcedureService _userProcedureService;

        private readonly IDataTablesService _dataTablesService;
        public RegistryPatternReviewController(
            IConfigurationService configurationService,
            IDataTablesService dataTablesService,
            IProcedureService procedureService,
            IUserProcedureService userProcedureService,
            IRegistryPatternService registryPatternService,
            ICareerService careerService,

            IViewRenderService viewRenderService
        ) : base(careerService, configurationService)
        {
            _dataTablesService = dataTablesService;
            _procedureService = procedureService;
            _userProcedureService = userProcedureService;
            _registryPatternService = registryPatternService;
        }

        /// <summary>
        /// Vista donde se encuentra el listado de padrones pendientes de aprobación
        /// </summary>
        /// <returns>Retorna la Vista</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de registros de padron con estado GENERADO
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="searchBookNumber">Texto de búsqueda número de libro</param>
        /// <param name="dateStartFilter">Fec. Inicio</param>
        /// <param name="dateEndFilter">Fec. Fin</param>
        /// <returns>Retorna un Ok con la información de los registros de padrón para ser usado en tablas</returns>
        [HttpGet("lista-registros-aprobados")]
        public async Task<IActionResult> GetRegistryPatterns(string searchValue, Guid? careerId, string searchBookNumber, string dateStartFilter, string dateEndFilter)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _registryPatternService.GetRegistryPatternApprovedDatatableByConfiguration(sentParameters, careerId, searchBookNumber, dateStartFilter, dateEndFilter, searchValue, ConstantHelpers.REGISTRY_PATTERN.STATUS.GENERATED);
            return Ok(result);
        }

        /// <summary>
        /// Método para cambiar el estado del registro de padrón
        /// </summary>
        /// <param name="registryPatternId">Identificador del registro de padrón</param>
        /// <returns>Retorna un Ok o BadRequest</returns>
        [HttpPost("cambiar-estado/{registryPatternId}")]
        public async Task<IActionResult> ChangeStatus(Guid registryPatternId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var registryPattern = await _registryPatternService.GetWithIncludes(registryPatternId);
            registryPattern.Status = ConstantHelpers.REGISTRY_PATTERN.STATUS.DENIED;
            var userProcedure = new UserProcedure();
            if (registryPattern.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
            {
                userProcedure = await _userProcedureService.GetUserProcedureByStaticType(registryPattern.Student.UserId, ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR_DEGREE_APPLICATION);
            }
            else
            {
                userProcedure = await _userProcedureService.GetUserProcedureByStaticType(registryPattern.Student.UserId, ConstantHelpers.PROCEDURES.STATIC_TYPE.TITLE_DEGREE_APPLICATION);
            }

            userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.OBSERVED;

            await _registryPatternService.Update(registryPattern);
            return Ok();
        }

        /// <summary>
        /// Método para aprobar los registros de padrón en base a los siguientes filtros
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="careerId">Identificar de la escuela profesional</param>
        /// <param name="searchBookNumber">Texto de búsqueda para el número de libro</param>
        /// <param name="dateStartFilter">Fec. Inicio</param>
        /// <param name="dateEndFilter">Fec. Fin</param>
        /// <returns></returns>
        [HttpPost("aprobacion")]
        public async Task<IActionResult> ApprovedAll(string searchValue, Guid? careerId, string searchBookNumber, string dateStartFilter, string dateEndFilter)
        {
            await _registryPatternService.ApprovedAll(careerId, searchBookNumber, dateStartFilter, dateEndFilter, searchValue);
            return Ok();
        }
    }
}
