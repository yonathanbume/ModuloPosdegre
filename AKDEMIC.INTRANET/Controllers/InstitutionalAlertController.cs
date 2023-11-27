using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Hubs;
using AKDEMIC.INTRANET.ViewModels.InstitutionalAlertViewModels;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize]
    [Route("alertaInstitucional")]
    public class InstitutionalAlertController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IInstitutionalAlertService _institutionalAlertService;
        private readonly IMapper _mapper;
        public InstitutionalAlertController(
           UserManager<ApplicationUser> userManager,
           IConfiguration configuration,
           IHubContext<AkdemicHub> hubContext,
           SignInManager<ApplicationUser> signInManager,
           IUserService userService,
           IInstitutionalAlertService institutionalAlertService,
           IMapper mapper) : base( userManager, configuration, hubContext, userService)
        {
            _signInManager = signInManager;
            _institutionalAlertService = institutionalAlertService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene la vista de alerta institucional
        /// </summary>
        /// <returns>Retorna una vista</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Envia un alerta institucional del usuario logeado
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("enviar")]
        public async Task<IActionResult> SendAlert()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var user = await _userService.GetUserWithDependecies(userId);
                var dependency = user.UserDependencies.First();

                var institutionalAlert = new InstitutionalAlert
                {
                    Status = true,
                    RegisterDate = DateTime.UtcNow,
                    DependencyId = dependency.DependencyId,
                    ApplicantId = userId
                };

                await SendNotificationToRole(ConstantHelpers.ROLES.INSTITUTIONAL_WELFARE, "Nueva alerta institucional", Url.GenerateLink(nameof(InstitutionalAlertController.Index), "InstitutionalAlert", null, Request.Scheme), "Urgente", ConstantHelpers.NOTIFICATIONS.COLORS.BRAND, true);
                await SendNotificationToRole(ConstantHelpers.ROLES.INFIRMARY, "Nueva alerta institucional", Url.GenerateLink(nameof(InstitutionalAlertController.Index), "InstitutionalAlert", null, Request.Scheme), "Urgente", ConstantHelpers.NOTIFICATIONS.COLORS.BRAND, true);

                await _institutionalAlertService.InsertInstitutionalAlert(institutionalAlert);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene una lista de las alertas institucionales
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("getInstitutionalAlerts")]
        public async Task<IActionResult> GetInstitutionalAlerts()
        {
            try
            {
                var model = await _institutionalAlertService.GetInstitutionalAlerts();
                var result = _mapper.Map<InstitutionalAlertViewModel>(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la vista para responder una alerta institucional
        /// </summary>
        /// <param name="id">Identificador de la alerta institucional</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("responder/{id}")]
        public async Task<IActionResult> AlertResponse(Guid id)
        {
            var model = await _institutionalAlertService.GetAlertReponse(id);
            var result = _mapper.Map<InstitutionalAlertDetailViewModel>(model);
            return View(result);
        }

        /// <summary>
        /// Responde la alerta institucional
        /// </summary>
        /// <param name="model">Modelo que contiene la alerta institucional</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("responderalertainstitucional")]
        public async Task<IActionResult> ResponseInstitutionalAlert(InstitutionalAlertDetailViewModel model)
        {
            var institutionalAlert = await _institutionalAlertService.GetInstitutionalAlertById(model.Id);
            var userId = _userManager.GetUserId(User);
            var user = await _userService.GetUserById(userId);

            institutionalAlert.AssistantId = user.Id;
            institutionalAlert.Type = model.Type;
            institutionalAlert.Description = model.Description;
            institutionalAlert.AttentionDate = DateTime.UtcNow;
            institutionalAlert.Status = false;
            await _institutionalAlertService.UpdateInstitutionalAlert(institutionalAlert);
            return Ok();
        }

    }
}
