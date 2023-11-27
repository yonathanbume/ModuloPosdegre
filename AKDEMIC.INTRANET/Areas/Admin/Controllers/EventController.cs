using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.EventViewModels;
using AKDEMIC.INTRANET.Areas.Admin.Models.StudentEventViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{

    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/eventos")]
    public class EventController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IEventService _eventService;
        private readonly IEventTypeService _eventTypeService;
        private readonly IEventRoleService _eventRoleService;
        private readonly IUserEventService _userEventService;
        private readonly IEventFileService _eventFileService;
        private readonly IMapper _mapper;
        public EventController(
            IUserService userService,
            ICloudStorageService cloudStorageService,
            IDataTablesService dataTablesService,
            IEventService eventService,
            IEventTypeService eventTypeService,
            IEventRoleService eventRoleService,
            IUserEventService userEventService,
            IEventFileService eventFileService,
            IMapper mapper) : base(userService, cloudStorageService)
        {
            _eventService = eventService;
            _eventTypeService = eventTypeService;
            _eventRoleService = eventRoleService;
            _userEventService = userEventService;
            _eventFileService = eventFileService;
            _mapper = mapper;
            _dataTablesService = dataTablesService;
        }


        /// <summary>
        /// Obtiene el listado de usuarios registrados al evento
        /// </summary>
        /// <param name="eid">identificador del evento</param>
        /// <returns>Objeto que contiene el listado de usuarios</returns>
        [HttpGet("registrados/detalle/{eid}")]
        public async Task<IActionResult> GetRegistered(Guid eid)
        {
            if (eid == Guid.Empty) { return Ok(); }
            var model = await _userEventService.GetRegisteredByEventId(eid);
            var result = _mapper.Map<List<StudentEnrolledViewModel>>(model);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de roles
        /// </summary>
        /// <returns>Objeto que contiene el listado de roles</returns>
        [HttpGet("roles/get")]
        public async Task<IActionResult> GetUserRoles()
        {
            var result = await _eventService.GetUserRoles();
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene la información del evento
        /// </summary>
        /// <param name="id">Identificador del evento</param>
        /// <returns>Objeto que contiene los datos del evento</returns>
        [Route("detalle/{id}")]
        [HttpPost]
        public async Task<IActionResult> Detail(Guid id)
        {
            var Event = await _eventService.GetEventById(id);

            return Ok(Event);
        }

        /// <summary>
        /// Método para agregar un evento
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del nuevo evento</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("agregar/post")]
        public async Task<IActionResult> AddEvent(EventViewModel model)
        {
            var user = await GetCurrentUserAsync();
            var eventType = await _eventTypeService.GetEventTypeByType(model.Type);

            string path = "";
            if (model.File != null)
            {
                if (model.File.Length > ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.IMAGE.GENERIC)
                {
                    return BadRequest($"El tamaño del archivo '{model.File.FileName}' excede el límite de {ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.IMAGE.GENERIC / 1024 / 1024}MB");
                }

                if (!model.File.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.IMAGE.GENERIC))
                {
                    return BadRequest($"El contenido del archivo '{model.File.FileName}' es inválido");
                }
                string extension = Path.GetExtension(model.File.FileName);
                path = await _cloudStorageService.UploadFile(model.File.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.EVENT_PICTURE,
                    extension, CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            var nEvent = new Event
            {
                Name = model.Name,
                Description = model.Description,
                Organizer = user,
                EventType = eventType,
                Place = model.Place,
                PathPicture = path,
                Cost = model.Cost,
                UrlVideo = model.UrlVideo,
                EventDate = Helpers.ConvertHelpers.DatepickerToUtcDateTime(model.EventDate),
                RegistrationStartDate = Helpers.ConvertHelpers.DatepickerToUtcDateTime(model.RegistrationStartDate),
                RegistrationEndDate = Helpers.ConvertHelpers.DatepickerToUtcDateTime(model.RegistrationEndDate),
                System = ConstantHelpers.EVENT.INTRANET
            };

            if (model.EventFiles != null && model.EventFiles.Any())
            {
                nEvent.EventFiles = new List<EventFile>();
                foreach (var item in model.EventFiles)
                {
                    nEvent.EventFiles.Add(new EventFile
                    {
                        Name = item.Name,
                        UrlFile = await _cloudStorageService.UploadFile(item.File.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.EVENT_FILES,
                            Path.GetExtension(item.File.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET)
                    });
                }
            }

            await _eventService.InsertEvent(nEvent);

            foreach (var role in model.SelectedRoles)
            {
                var eventRole = new EventRole
                {
                    EventId = nEvent.Id,
                    RoleId = role
                };
                await _eventRoleService.InsertEventRole(eventRole);
            }

            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de eventos 
        /// </summary>
        /// <param name="eventTypeId">Identificador del tipo de evento</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de eventos</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetEvents(Guid eventTypeId, string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _eventService.GetEvents(sentParameters, ConstantHelpers.EVENT.INTRANET, eventTypeId, searchValue);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de archivos de un evento
        /// </summary>
        /// <param name="eventId">Identificador del evento</param>
        /// <returns>Objeto que contiene el listado de archivos</returns>
        [HttpGet("get-archivos")]
        public async Task<IActionResult> GetFiles(Guid eventId)
        {
            var data = await _eventFileService.GetAllByEvent(eventId);
            var result = data.OrderBy(x => x.CreatedAt).Select(x => new
            {
                x.Id,
                x.Name,
                x.UrlFile
            }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Vista donde se gestionan los eventos
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            EventViewModel eventViewModel = new EventViewModel
            {
                EventTypes = await _eventTypeService.EventTypesDelete()
            };
            return View(eventViewModel);
        }

        /// <summary>
        /// Método para agregar un evento
        /// </summary>
        /// <returns>Código de estado HTTP</returns>
        [HttpGet("agregar")]
        public async Task<IActionResult> Add()
        {
            var model = new EventViewModel
            {
                EventTypes = await _eventTypeService.EventTypesDelete()
            };
            return View(model);
        }

        /// <summary>
        /// Obtiene el listado de eventos
        /// </summary>
        /// <param name="isAll">¿Todos?</param>
        /// <returns>Objeto que contiene el listado de eventos</returns>
        [HttpGet("tipo-eventos")]
        public async Task<IActionResult> EventTypesSelect(bool isAll = false)
        {
            var result = await _eventTypeService.GetAllEventTypesInscription(isAll);
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se edita el evento
        /// </summary>
        /// <param name="id">Identificador del evento</param>
        /// <returns>Vista de edición</returns>
        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var Event = await _eventService.GetEventById(id);
            var roles = await _eventRoleService.GetAllEventRolesByEventId(Event.Id);

            var roleslist = roles.Select(x => x.RoleId).ToList();

            var model = new EventViewModel
            {
                Cost = Event.Cost,
                Description = Event.Description,
                EventDate = Event.EventDate.ToLocalDateFormat(),
                EventTypes = await _eventTypeService.EventTypesDelete(),
                Id = Event.Id,
                Name = Event.Name,
                Place = Event.Place,
                RegistrationEndDate = Event.RegistrationEndDate.ToLocalDateFormat(),
                RegistrationStartDate = Event.RegistrationStartDate.ToLocalDateFormat(),
                Type = Event.EventTypeId,
                SelectedRoles = roleslist,
                SelectedRoles2 = string.Join(',', roleslist),
                Path = Event.PathPicture,
                UrlVideo = Event.UrlVideo
            };

            return View(model);
        }

        /// <summary>
        /// Método para editar el evento
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del evento</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar/post")]
        public async Task<IActionResult> EditEvent(EventViewModel model)
        {
            var Event = await _eventService.GetEventById(model.Id);
            var user = await GetCurrentUserAsync();

            string path = "";
            if (model.File != null)
            {
                if (model.File.Length > ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.IMAGE.GENERIC)
                {
                    return BadRequest($"El tamaño del archivo '{model.File.FileName}' excede el límite de {ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.IMAGE.GENERIC / 1024 / 1024}MB");
                }

                if (!model.File.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.IMAGE.GENERIC))
                {
                    return BadRequest($"El contenido del archivo '{model.File.FileName}' es inválido");
                }

                string extension = Path.GetExtension(model.File.FileName);
                path = await _cloudStorageService.UploadFile(model.File.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.EVENT_PICTURE,
                    extension, CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                Event.PathPicture = path;
            }

            Event.Name = model.Name;
            Event.Description = model.Description;
            Event.Organizer = user;
            Event.EventTypeId = model.Type;
            Event.Place = model.Place;
            Event.Cost = model.Cost;
            Event.EventDate = Helpers.ConvertHelpers.DatepickerToUtcDateTime(model.EventDate);
            Event.RegistrationStartDate = Helpers.ConvertHelpers.DatepickerToUtcDateTime(model.RegistrationStartDate);
            Event.RegistrationEndDate = Helpers.ConvertHelpers.DatepickerToUtcDateTime(model.RegistrationEndDate);
            Event.UrlVideo = model.UrlVideo;

            if (model.EventFiles != null)
            {
                foreach (var item in model.EventFiles)
                {
                    switch (item.ToDo)
                    {
                        case 1:
                            var fileToAdd = new EventFile
                            {
                                EventId = Event.Id,
                                Name = item.Name,
                                UrlFile = await _cloudStorageService.UploadFile(item.File.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.EVENT_FILES,
                                Path.GetExtension(item.File.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET)
                            };

                            await _eventFileService.Insert(fileToAdd);
                            break;

                        case 3:
                            var eventDelete = await _eventFileService.Get(item.Id.Value);
                            await _cloudStorageService.TryDeleteProductBinary(eventDelete.UrlFile);
                            await _eventFileService.Delete(eventDelete);
                            break;

                        default:
                            break;
                    }
                }
            }

            await _eventService.UpdateEvent(Event);

            //delete roles 
            var roles = await _eventRoleService.GetAllEventRolesByEventId(model.Id);
            await _eventRoleService.DeleteRange(roles);

            //agregar nuevos
            foreach (var role in model.SelectedRoles)
            {
                var eventRole = new EventRole
                {
                    EventId = model.Id,
                    RoleId = role
                };
                await _eventRoleService.InsertEventRole(eventRole);
            }

            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de asistencia del evento
        /// </summary>
        /// <param name="eid">Identificador del evento</param>
        /// <returns>Objeto que contiene el listado de asistencia</returns>
        [HttpGet("asistencia/detalle/{eid}")]
        public async Task<IActionResult> AssistanceDetail(Guid eid)
        {
            var result = await _userEventService.GetAssistanceDetailByEventId(eid);

            return Ok(result);
        }

        /// <summary>
        /// Vista de asistencia
        /// </summary>
        /// <param name="eid">Identificador del evento</param>
        /// <returns>Vista asistencia</returns>
        [HttpGet("asistencia/{eid}")]
        public ActionResult Assistance(Guid eid)
        {
            return View(eid);
        }

        /// <summary>
        /// Vista donde se lista los registrados al evento
        /// </summary>
        /// <param name="eid">Identificador del evento</param>
        /// <returns>Vista registrados</returns>
        [HttpGet("registrados/{eid}")]
        public ActionResult Registered(Guid eid)
        {
            return View(eid);
        }

        /// <summary>
        /// Marca inasistenacia a todos los enviados en la lista
        /// </summary>
        /// <param name="eventId">Identificador del evento</param>
        /// <param name="data">Modelo que contiene los eventos y usuarios que inasistieron</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("asistencia/detalle/{eventId}/post")]
        public async Task<IActionResult> AssistancePost(StudentEventViewModel data)
        {
            //Si alguien copia este codigo modificado, debe revisar el JS que tambien se cambio
            if (data == null)
            {
                return BadRequest("No se ha encontrado usuarios registrados");
            }

            bool updated = false;
            foreach (var item in data.List)
            {
                var studentEvent = await _userEventService.GetUserEventById(item.Id);

                if (studentEvent == null) continue;

                if (studentEvent.Absent != item.Absent)
                {
                    updated = true;
                    studentEvent.Absent = item.Absent;
                }

            }

            if (updated)
            {
                await _userEventService.Save();
            }

            return Ok();
        }
    }
}
