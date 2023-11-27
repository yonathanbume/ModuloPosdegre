using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.CareerDirector.Models.ReportSurveyViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_SECRETARY + "," +
        ConstantHelpers.ROLES.DEAN + "," +
        ConstantHelpers.ROLES.DEAN_SECRETARY + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)]
    [Area("CareerDirector")]
    [Route("director-carrera/reporte-encuesta")]
    public class ReportSurveyController : BaseController
    {
        private readonly ICareerService _careerService;
        private readonly ISurveyService _surveyService;
        private readonly ISurveyItemService _surveyItemService;
        private readonly IAnswerByUserService _answerByUserService;

        public ReportSurveyController(UserManager<ApplicationUser> userManager,
            IDataTablesService dataTablesService,
            ICareerService careerService,
            ISurveyService surveyService,
            ISurveyItemService surveyItemService,
            IAnswerByUserService answerByUserService
            ) : base(userManager, dataTablesService)
        {
            _careerService = careerService;
            _surveyService = surveyService;
            _surveyItemService = surveyItemService;
            _answerByUserService = answerByUserService;
        }

        /// <summary>
        /// Vista donde se muestra el listado de encuestas
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de encuestas generales para ser usado en tablas
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Listado de encuestas generales</returns>
        [HttpGet("getAllSurveys")]
        public async Task<IActionResult> GetAllSurveys(string search, Guid careerId)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _surveyService.GetGeneralSurveysDatatable(parameters, ConstantHelpers.Solution.Intranet, careerId, null, null, search, User);
            return Ok(result);
        }

        /// <summary>
        /// Vista detalle de la encuesta
        /// </summary>
        /// <param name="surveyId">Identificador de la encuesta</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("detalle/{surveyId}")]
        public IActionResult Detail(Guid surveyId)
        {
            ViewBag.Id = surveyId;
            return View();
        }

        /// <summary>
        /// Obtiene la vista parcial donde se listan las preguntas de la encuesta
        /// </summary>
        /// <param name="surveyId">Identificador de la encuesta</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("get-preguntas")]
        public async Task<IActionResult> GetNumberQuestions(Guid surveyId)
        {
            var result = await _surveyItemService.GetSurveyItemTemplate(surveyId);
            return PartialView("/Areas/Admin/Views/ReportSurvey/_GetReportGeneralSurvey.cshtml", result);
        }

        /// <summary>
        /// Obtiene el listado de usuarios que han resuelto la encuesta para ser usado en tablas
        /// </summary>
        /// <param name="surveyId">Identificador de la encuesta</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de usuarios</returns>
        [HttpGet("get-usuarios")]
        public async Task<IActionResult> GetUsers(Guid? surveyId, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _answerByUserService.ReportDatatable(parameters, surveyId, search);
            return Ok(result);
        }

        #region JSON

        /// <summary>
        /// Obtiene el listado de escuelas profesionales para ser usado en select
        /// </summary>
        /// <returns>Listado de escuelas profesionales</returns>
        [HttpGet("carreras/get")]
        public async Task<IActionResult> GetCareers()
        {
            var userId = _userManager.GetUserId(User);

            var result = await _careerService.GetAllAsSelect2ClientSide(user: User);

            //if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            //{
            //    var resultdean = await _careerService.GetAllAsSelect2ClientSide(user: User);
            //    return Ok(new
            //    {
            //        items = resultdean
            //    });
            //}

            //var result = await _careerService.GetCareerSelect2ByAcademicCoordinatorClientSide(userId);
            return Ok(new
            {
                items = result
            });
        }

        #endregion

    }
}
