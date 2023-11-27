using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.EventTypesViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/tipo-eventos")]
    public class EventTypeController : BaseController
    {
        private readonly IEventTypeService _eventTypeService;
        public EventTypeController(UserManager<ApplicationUser> userManager,
            IEventTypeService eventTypeService) : base(userManager)
        {
            _eventTypeService = eventTypeService;
        }

        /// <summary>
        /// Obtiene el listado de tipos de evento
        /// </summary>
        /// <returns>Objeto que contiene el listado de tipos de evento</returns>
        [Route("get")]
        public async Task<IActionResult> GetEventTypes()
        {
            var result = await _eventTypeService.GetEventTypes();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle del tipo de evento
        /// </summary>
        /// <param name="id">Identificador del tipo de evento</param>
        /// <returns>Objeto que contiene los datos del tipo de evento</returns>
        [Route("get/{id}")]
        public async Task<IActionResult> GetEventType(Guid id)
        {
            var result = await _eventTypeService.GetEventTypeById(id);
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se gestiona los tipos de evento
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            EventTypeViewModel eventTypeViewModel = new EventTypeViewModel();
            return View(eventTypeViewModel);
        }

        /// <summary>
        /// Método para crear un tipo de evento
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del nuevo tipo de evento</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("crear/post")]
        [HttpPost]
        public async Task<IActionResult> AddType(EventTypeViewModel model)
        {
            EventType eventType = new EventType
            {
                Name = model.Name,
                Color = model.Color
            };

            await _eventTypeService.InsertEventType(eventType);
            return Ok();
        }

        /// <summary>
        /// Método para editar un tipo de evento
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del tipo de evento</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("editar")]
        [HttpPost]
        public async Task<IActionResult> UpdateType(EventTypeViewModel model)
        {
            var eventTypeAny = true;
            if (model.Id != null)
            {
                eventTypeAny = await _eventTypeService.FindAnyEventType(model.Id);
            }
            if (eventTypeAny)
            {
                var eventType = await _eventTypeService.GetEventTypeById(model.Id);

                eventType.Name = model.Name;
                eventType.Color = model.Color;
                await _eventTypeService.UpdateEventType(eventType);

                return Ok(eventType);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Método para eliminar un tipo de evento
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del tipo de evento</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("eliminar/post")]
        [HttpPost]
        public async Task<IActionResult> DeleteProcedure(EventType model)
        {
            var eventType = await _eventTypeService.GetEventTypeById(model.Id);

            await _eventTypeService.DeleteEventType(eventType);

            return Ok(eventType);
        }
    }
}
