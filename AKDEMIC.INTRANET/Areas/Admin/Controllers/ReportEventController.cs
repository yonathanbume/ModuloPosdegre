using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Areas.Admin.Models.Report_eventViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.REPORT_QUERIES)]
    [Area("Admin")]
    [Route("admin/reporte_evento")]
    public class ReportEventController : BaseController
    {
        private readonly ISelect2Service _select2Service;
        private readonly IDataTablesService _dataTablesService;
        private readonly IEventService _eventService;
        private readonly IUserEventService _userEventService;

        public ReportEventController(
            IDataTablesService dataTablesService,
            ISelect2Service select2Service,
            IEventService eventService,
            IUserEventService userEventService) : base()
        {
            _select2Service = select2Service;
            _dataTablesService = dataTablesService;
            _eventService = eventService;
            _userEventService = userEventService;
        }

        /// <summary>
        /// Vista principal del controlador
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de eventos
        /// </summary>
        /// <returns>Objeto que contiene el listado de eventos</returns>
        [HttpGet("getEvents")]
        public async Task<IActionResult> AllEvents()
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _eventService.GetEventsDataTable(parameters);
            return Ok(result);
        }

        /// <summary>
        /// Vista detalle del evento
        /// </summary>
        /// <param name="eid">Identificador del evento</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("detalle/{eid}")]
        public async Task<IActionResult> ReportEventDetail(Guid eid)
        {
            var userEvent = await _userEventService.GetUserEventByEventId(eid);
            var result = new ReportEventViewModel();
            if (userEvent != null)
            {
                result = new ReportEventViewModel
                {
                    Id = userEvent.Id,
                    EventDate = userEvent.EventDate,
                    OrganizerName = userEvent.OrganizerName,
                    Name = userEvent.Name,
                    Description = userEvent.Description,
                    Place = userEvent.Place,
                    StartEndDate = userEvent.StartEndDate,
                    GeneralCost = userEvent.GeneralCost
                };
            }
            else
            {
                var event_ = await _eventService.GetEventWithUserEvents(eid);
                result = new ReportEventViewModel
                {
                    Id = event_.Id,
                    EventDate = event_.EventDate,
                    OrganizerName = event_.OrganizerName,
                    Name = event_.Name,
                    Description = event_.Description,
                    Place = event_.Place,
                    StartEndDate = event_.StartEndDate,
                    GeneralCost = event_.GeneralCost
                };
            }
            return View(result);
        }

        /// <summary>
        /// Obtiene el listado de usuarios al evento
        /// </summary>
        /// <param name="eid">Identificador del evento</param>
        /// <returns>Listado de usuarios</returns>
        [HttpGet("detalle/alumnos/{eid}")]
        public async Task<IActionResult> StudentDetail(Guid eid)
        {
            var result = (await _userEventService.GetUserEventsByEventId(eid))
                .Select(x => new
                {
                    fullname = x.User.FullName,
                    absent = x.Absent
                });
            return Ok();
        }

        /// <summary>
        /// Obtiene la cantidad de usuarios presentes y ausentes
        /// </summary>
        /// <param name="eid">Identificador del evento</param>
        /// <returns>Objeto que contiene los datos para el reporte</returns>
        [HttpGet("getChartAbsents/{eid}")]
        public async Task<IActionResult> GetChartAbsents(Guid eid)
        {
            var result = (await _userEventService.GetUserEventsByEventId(eid))
                .GroupBy(x => new { x.EventId })
                .Select(x => new
                {
                    presentsName = "Presentes",
                    presents = x.Count(c => c.Absent == false),
                    absentsName = "Ausentes",
                    absents = x.Count(c => c.Absent == true)
                });
            return Ok(result);

        }

        /// <summary>
        /// Obtiene el costo esperado vs el costo real del evento
        /// </summary>
        /// <param name="eid">Identificador del evento</param>
        /// <returns>Objeto que contiene los datos para el reporte</returns>
        [HttpGet("getChartCost/{eid}")]
        public async Task<IActionResult> GetChartCost(Guid eid)
        {
            var result = (await _userEventService.GetUserEventsByEventId(eid))
                 .GroupBy(x => new { x.EventId })
                .Select(x => new
                {

                    cost = "Costo",
                    expectedCost = Convert.ToInt32(x.Count() * x.FirstOrDefault().Event.Cost),
                    realCost = Convert.ToInt32(x.Count(c => c.Absent == false) * x.FirstOrDefault().Event.Cost)

                });
            return Ok(result);
        }
    }
}
