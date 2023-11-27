using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.ViewModels.SurveyViewModels;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS + "," + ConstantHelpers.ROLES.TEACHERS + "," + ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Route("encuestas")]
    public class SurveyUserController : BaseController
    {
        private readonly ISurveyUserService _surveyUserService;
        private readonly IConfigurationService _configurationService;
        private readonly IAnswerByUserService _answerByUserService;
        private readonly IEnrollmentTurnService _enrollmentTurnService;
        private readonly IStudentService _studentService;
        private readonly ITermService _termService;

        public SurveyUserController(
            ISurveyUserService surveyUserService,
            IConfigurationService configurationService,
            IDataTablesService dataTablesService,
            IAnswerByUserService answerByUserService,
            IEnrollmentTurnService enrollmentTurnService,
            UserManager<ApplicationUser> userManager,
            IStudentService studentService,
            ITermService termService
        ) : base(userManager, dataTablesService)
        {
            _answerByUserService = answerByUserService;
            _enrollmentTurnService = enrollmentTurnService;
            _studentService = studentService;
            _termService = termService;
            _surveyUserService = surveyUserService;
            _configurationService = configurationService;
        }
        /// <summary>
        /// Obtiene la vista inicial de encuestas de los usuarios
        /// </summary>
        /// <returns>Retorna una vista</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene las encuestas creadas en intranet o gestion docente, de tipo general, que estan dentro del rango para responderlas
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400, con la lista de encuestas</returns>
        [HttpGet("getsurveybyuser")]
        public async Task<IActionResult> GetSurvies()
        {
            string userId = _userManager.GetUserId(User);

            var query = await _surveyUserService.GetAllFirstLevelByUser(userId);

            var result = query
                .Where(x => (x.Survey.System == ConstantHelpers.Solution.Intranet || x.Survey.System == ConstantHelpers.Solution.TeachingManagement)
                 && x.Survey.Type == ConstantHelpers.TYPE_SURVEY.GENERAL
                 && x.Survey.PublicationDate <= DateTime.Now
                 && x.Survey.FinishDate >= DateTime.Now)
                .Select(x => new SurveyViewModel
                {
                    Id = x.Id,
                    Name = x.Survey.Name,
                    Description = x.Survey.Description,
                    Code = x.Survey.Code,
                    PublicationDate = x.Survey.PublicationDate.ToString("dd/MM/yyyy"),
                    FinishDate = x.Survey.FinishDate.ToString("dd/MM/yyyy")
                }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene una vista parcial de las preguntas de la encuesta-usuario
        /// </summary>
        /// <param name="surveyUserId">Identificador de encuesta-usuario</param>
        /// <returns>Retorna una vista parcial</returns>
        [HttpGet("{surveyUserId}/preguntas")]
        public async Task<IActionResult> _Questions(Guid surveyUserId)
        {
            var surveyUser = await _surveyUserService.GetSurveyUserTemplate(surveyUserId);
            return PartialView(surveyUser);
        }

        /// <summary>
        /// Obtiene la vista inicial que servira para responder una encuesta al usuario
        /// </summary>
        /// <param name="surveyUserId">Identificador de la encuesta - usuario</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("responder/{surveyUserId}")]
        public async Task<IActionResult> RespondSurvey(Guid surveyUserId)
        {
            var surveyUser = await _surveyUserService.GetIncludeFirstLevel(surveyUserId);
            var user = await _userManager.GetUserAsync(User);

            if (surveyUser == null)
                return RedirectToAction("Index", "Home");

            if (user == null || user.Id != surveyUser.UserId)
                return RedirectToAction("Index", "Home");

            bool sended = false;
            if (surveyUser.DateTime.HasValue)
            {
                sended = true;
            }


            ViewBag.Sended = sended;
            ViewBag.SurveyUserId = surveyUserId;
            return View();
        }

        /// <summary>
        /// Responde una encuesta
        /// </summary>
        /// <param name="model">Modelo que contiene las respuestas de una encuesta</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("enviar")]
        public async Task<IActionResult> SendAnswers(SurveyUserTemplate model)
        {
            var surveyUser = await _surveyUserService.GetIncludeFirstLevel(model.SurveyUserId);

            if (surveyUser == null)
            {
                return BadRequest("La encuesta no se encuentra disponible");
            }

            if (surveyUser.DateTime != null || surveyUser.AnswerByUsers.Count() > 0)
            {
                return BadRequest("La encuesta ya fue resulta con anterioridad");
            }

            var answersByUser = new List<AnswerByUser>();
            foreach (var item in model.SurveyItems)
            {
                foreach (var question in item.Questions)
                {
                    if (question.Type == ConstantHelpers.SURVEY.TEXT_QUESTION || question.Type == ConstantHelpers.SURVEY.LIKERT_QUESTION)
                    {
                        AnswerByUser answerUser = new AnswerByUser
                        {
                            Description = question.Description ?? "",
                            SurveyUserId = model.SurveyUserId,
                            QuestionId = question.Id
                        };
                        answersByUser.Add(answerUser);
                    }
                    else
                    {
                        if (question.Selection != null)
                        {
                            foreach (var selection in question.Selection)
                            {
                                AnswerByUser answerUser = new AnswerByUser
                                {
                                    SurveyUserId = model.SurveyUserId,
                                    QuestionId = question.Id,
                                    AnswerId = selection
                                };
                                answersByUser.Add(answerUser);
                            }
                        }
                    }
                }
            }
            surveyUser.DateTime = DateTime.Now;
            await _answerByUserService.AddRange(answersByUser);
            await _surveyUserService.Update(surveyUser);

            if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS))
            {
                var enabledPreEnrollmentSurvey = Guid.TryParse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.Enrollment.PRE_ENROLLMENT_SURVEY), out var preEnrollmentSurvey);
                if (enabledPreEnrollmentSurvey)
                {
                    if (surveyUser.SurveyId == preEnrollmentSurvey)
                    {
                        var term = await _termService.GetActiveTerm();
                        var student = await _studentService.GetStudentByUser(surveyUser.UserId);
                        if (term != null)
                        {
                            var enrollmentTurn = await _enrollmentTurnService.GetByStudentIdAndTerm(student.Id, term.Id);
                            if (enrollmentTurn != null && !enrollmentTurn.IsConfirmed)
                            {
                                var url = Flurl.Url.Combine(CORE.Helpers.GeneralHelpers.GetApplicationRoute(CORE.Helpers.ConstantHelpers.Solution.Enrollment), "matricula");
                                return Ok(url);
                            }
                        }
                    }

                }
            }
            return Ok("/");
        }

        /// <summary>
        /// Obtiene la vista para encuesta respondida
        /// </summary>
        /// <returns>Retorna una vista</returns>
        [HttpGet("responder/encuesta-respondida")]
        public IActionResult SurveyAnswered() => View();
    }
}
