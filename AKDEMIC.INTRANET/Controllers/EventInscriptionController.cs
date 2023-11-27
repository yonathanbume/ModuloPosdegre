using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.ViewModels.EventViewModels;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize]
    [Route("inscripcion_eventos")]
    public class EventInscriptionController : BaseController
    {
        private readonly IEventTypeService _eventTypeService;
        private readonly IDataTablesService _dataTablesService;
        private readonly IUserEventService _userEventService;
        private readonly IEventRoleService _eventRoleService;
        private readonly IMapper _mapper;
        private readonly IEventFileService _eventFileService;
        private readonly IEventService _eventService;
        private readonly IViewRenderService _viewRenderService;
        public EventInscriptionController(UserManager<ApplicationUser> userManager,
            IEventTypeService eventTypeService,
            IDataTablesService dataTablesService,
            IUserEventService userEventService,
            IEventRoleService eventRoleService,
            IMapper mapper,
            IEventFileService eventFileService,
            IEventService eventService,
             IViewRenderService viewRenderService,
        IUserService userService) : base(userManager, userService)
        {
            _eventTypeService = eventTypeService;
            _dataTablesService = dataTablesService;
            _userEventService = userEventService;
            _eventRoleService = eventRoleService;
            _mapper = mapper;
            _eventFileService = eventFileService;
            _eventService = eventService;
             _viewRenderService = viewRenderService;
        }

        /// <summary>
        /// Obtiene la vista inicial de inscripción de eventos
        /// </summary>
        /// <returns>Retorna una vista</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene los tipos de eventos
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("getEventTypes")]
        public async Task<IActionResult> GetEventTypes()
        {
            var eventTypes = await _eventTypeService.GetAllEventTypesInscription();
            return Ok(new { eventTypes });
        }

        /// <summary>
        /// Obtiene la vista de eventos según el tipo de evento
        /// </summary>
        /// <param name="eventTypeId">Identificador del tipo de evento</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("getEvents/{eventTypeId}")]
        public async Task<IActionResult> GetEvents(Guid eventTypeId)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userService.GetUserById(userId);
            var roles = await _userManager.GetRolesAsync(user);

            var intranetEvents = await _eventRoleService.GetEventsInscription(roles, userId, false, AKDEMIC.CORE.Helpers.ConstantHelpers.EVENT.INTRANET);
            var institutionalWelfareEvents = await _eventRoleService.GetEventsInscription(roles, userId, false, AKDEMIC.CORE.Helpers.ConstantHelpers.EVENT.INSTITUTIONALWELFARE);
            intranetEvents.AddRange(institutionalWelfareEvents);

            var events = _mapper.Map<List<EventViewModel>>(intranetEvents);

            if (eventTypeId != Guid.Empty)
            {
                events = events.Where(x => x.EventTypeId == eventTypeId).ToList();
            }


            var viewToString = await _viewRenderService.RenderToStringAsync("/Views/EventInscription/_Events.cshtml", events);
            return Ok(viewToString);
        }

        /// <summary>
        /// Obtiene los archivos del evento
        /// </summary>
        /// <param name="eventId">Identificador del evento</param>
        /// <returns>Retorna un objeto con la estructura de la tabla</returns>
        [HttpGet("get-archivos")]
        public async Task<IActionResult> GetFiles(Guid eventId)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _eventFileService.GetDatatable(parameters, eventId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene la información del evento especificado
        /// </summary>
        /// <param name="id">Identificador del evento</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost]
        [Route("getEvent")]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            var model = await _eventService.GetEventInscriptionById(id);
            var result = _mapper.Map<EventViewModel>(model);

            return Ok(result);
        }

        /// <summary>
        /// Agrega una inscripción al evento para el usuario logeado
        /// </summary>
        /// <param name="id">Identificador del evento</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("agregar")]
        public async Task<IActionResult> AddInscription(Guid id)
        {
            var userId = _userManager.GetUserId(User);
            var studentEvent = new UserEvent
            {
                UserId = userId,
                EventId = id
            };
            await _userEventService.InsertUserEvent(studentEvent);
            return Ok();
        }

    }
}
