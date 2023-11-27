using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/mis-solicitudes")]
    public class MyRequestController : BaseController
    {

        private readonly ISelect2Service _select2Service;
        private readonly IRecordHistoryService _recordHistoryService;
        private readonly ICareerService _careerService;
        private readonly IStudentService _studentService;
        private readonly IInternalProcedureService _internalProcedureService;
        private readonly IRecordHistoryObservationService _recordHistoryObservationService;
        private readonly IRegistryPatternService _registryPatternService;
        private readonly IGradeReportService _gradeReportService;

        public MyRequestController(
            IDataTablesService dataTablesService,
            UserManager<ApplicationUser> userManager,
            IInternalProcedureService internalProcedureService,
            IRecordHistoryObservationService recordHistoryObservationService,
            IRegistryPatternService registryPatternService,
            IStudentService studentService,
            ISelect2Service select2Service,
            IRecordHistoryService recordHistoryService,
            ICareerService careerService,
            IGradeReportService gradeReportService
        ) : base(userManager, dataTablesService)
        {
            _gradeReportService = gradeReportService;
            _careerService = careerService;
            _select2Service = select2Service;
            _recordHistoryService = recordHistoryService;
            _internalProcedureService = internalProcedureService;
            _recordHistoryObservationService = recordHistoryObservationService;
            _registryPatternService = registryPatternService;
            _studentService = studentService;
        }

        /// <summary>
        /// Vista donde se listan las solicitudes generadas por el alumno logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de solicitudes generadas por el alumno logueado
        /// </summary>
        /// <returns>Listado de solicitudes</returns>
        [HttpGet("get-records")]
        public async Task<IActionResult> GetGradeReportDatatable()
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _studentService.GetStudentByUser(user.Id);
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _recordHistoryService.GetRecordsByStudentDatatable(sentParameters, student.Id);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de observaciones de la solicitud
        /// </summary>
        /// <param name="recordHistoryId">Identificador de la solicitud</param>
        /// <returns>Listado de observaciones</returns>
        [HttpGet("get-observaciones")]
        public async Task<IActionResult> GetObservations(Guid recordHistoryId)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _recordHistoryObservationService.GetObservationsDatatableByRecordHistoryId(parameters, recordHistoryId);
            return Ok(result);
        }
    }
}
