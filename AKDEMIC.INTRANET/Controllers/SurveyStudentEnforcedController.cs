using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Controllers
{
    //[Authorize(Roles = ConstantHelpers.ROLES.STUDENTS + "," + ConstantHelpers.ROLES.TEACHERS)]
    public class SurveyStudentEnforcedController : BaseController
    {
        private readonly ISurveyUserService _surveyUserService;
        private readonly IConfigurationService _configurationService;

        public SurveyStudentEnforcedController(
            IConfigurationService configurationService,
            ISurveyUserService surveyUserService,
            UserManager<ApplicationUser> userManager
        ) :base(userManager)
        {
            _configurationService = configurationService;
            _surveyUserService = surveyUserService;
        }

        /// <summary>
        /// Obtiene la encuesta pendiente del usuario logeado que este marcada como requerida
        /// </summary>
        /// <returns>Retorna una vista o redirecciona en caso de no encontrar ninguna encuesta requerida</returns>
        public async Task<IActionResult> AnswerEnforcedSurvey()
        {
            var userId = _userManager.GetUserId(User);
            var surveyUser = await _surveyUserService.GetFirstUserSurvey(true, userId);

            var configurationSurvey = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.IntranetManagement.SURVEY_ENFORCE_REQUIRED);
            bool requiredEnabled = false;
            if (configurationSurvey != null)
            {
                requiredEnabled = bool.Parse(configurationSurvey.Value);
            }

            if (requiredEnabled)
            {
                //Primera encuesta en el periodo de tiempo
                if (surveyUser != null)
                {
                    if (surveyUser.DateTime.HasValue)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Sended = false;
                        ViewBag.SurveyUserId = surveyUser.Id;
                        return View();
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
