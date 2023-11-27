using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Teacher.Models.ClassRescheduleViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/clases/reprogramaciones")]
    public class ClassRescheduleController : BaseController
    {
        private readonly IClassService _classService;
        private readonly IDataTablesService _dataTablesService;
        private readonly ITermService _termService;
        private readonly IClassRescheduleService _classRescheduleService;

        public ClassRescheduleController(IUserService userService,
            IClassService classService,
            IDataTablesService dataTablesService,
            ITermService termService,
            IClassRescheduleService classRescheduleService) : base(userService)
        {
            _classService = classService;
            _dataTablesService = dataTablesService;
            _termService = termService;
            _classRescheduleService = classRescheduleService;
        }

        /// <summary>
        /// Vista donde se gestionan las reprogramaciones de clase
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            var classRescheduleViewModel = new ClassRescheduleViewModel
            {
                ClassRescheduleStatusIndices = ConstantHelpers.CLASS_RESCHEDULE.STATUS.INDICES,
                ClassRescheduleStatusValues = ConstantHelpers.CLASS_RESCHEDULE.STATUS.VALUES2
            };

            return View(classRescheduleViewModel);
        }

        /// <summary>
        /// Obtiene el listado de reprogramaciones creadas por el usuario logueado
        /// </summary>
        /// <param name="status">Identificador del estado</param>
        /// <returns>Listado de reprogramaciones</returns>
        [Route("get")]
        public async Task<IActionResult> GetClassReschedules(int? status = null)
        {
            var userId = GetUserId();
            var classReschedules = await _classRescheduleService.GetAll(userId, status);
            var result = classReschedules.Select(x => new
            {
                id = x.Id,
                userId = x.UserId,
                endDateTime = x.EndDateTime.ToLocalDateTimeFormat(),
                justification = x.Justification,
                startDateTime = x.StartDateTime.ToLocalDateTimeFormat(),
                status = x.Status,
                isPermanent = x.Replicate,
                @class = new
                {
                    classNumber = x.Class.ClassNumber,
                    endTime = x.Class.EndTime.ToLocalDateTimeFormat(),
                    startTime = x.Class.StartTime.ToLocalDateTimeFormat(),
                    weekNumber = x.Class.WeekNumber,
                    classSchedule = new
                    {
                        section = new
                        {
                            code = x.Class.ClassSchedule.Section.Code,
                            courseTerm = new
                            {
                                course = new
                                {
                                    fullName = x.Class.ClassSchedule.Section.CourseTerm.Course.FullName
                                }
                            }
                        }
                    }
                },
                createdAt = x.CreatedAt.Value.ToLocalDateTimeFormat()
            }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Método para crear una solicitud de reprogramación de clase
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la reprogramación de clase</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("crear/post")]
        public async Task<IActionResult> CreateClassReschedule(ClassRescheduleViewModel model)
        {
            var classId = model.ClassId;
            var classAny = await _classRescheduleService.AnyByClass(classId, ConstantHelpers.CLASS_RESCHEDULE.STATUS.IN_PROCESS);

            if (classAny)
            {
                return BadRequest("Ya hay una reprogramación en proceso para la clase seleccionada");
            }

            var date = ConvertHelpers.DatepickerToDatetime(model.Date);
            var start = ConvertHelpers.TimepickerToTimeSpan(model.StartTime);
            var end = ConvertHelpers.TimepickerToTimeSpan(model.EndTime);

            var endDateTime = date.Add(end).ToUtcDateTime();
            var startDateTime = date.Add(start).ToUtcDateTime();

            if (DateTime.Compare(startDateTime, endDateTime) >= 0)
            {
                return BadRequest("La fecha inicial no puede ser mayor o igual a la fecha final");
            }

            var userId = GetUserId();
            var classReschedule = new ClassReschedule
            {
                ClassId = classId,
                UserId = userId,
                Justification = model.Justification,
                EndDateTime = endDateTime,
                StartDateTime = startDateTime,
                Replicate = model.IsPermanent
            };

            await _classRescheduleService.Insert(classReschedule);
            return Ok();
        }

        /// <summary>
        /// Método para eliminar una solicitud de reprogramación de clase
        /// </summary>
        /// <param name="id">Identificador de la reprogramación de clase</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("eliminar/post")]
        [HttpPost]
        public async Task<IActionResult> DeleteClassReschedulingRequest(Guid id)
        {
            var classReschedulingRequest = await _classRescheduleService.Get(id);

            if (classReschedulingRequest.Status != ConstantHelpers.CLASS_RESCHEDULE.STATUS.IN_PROCESS)
                return BadRequest("No puede eliminar una solicitud ya respondida");

            await _classRescheduleService.Delete(classReschedulingRequest);
            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de clases asignadas al docente logueado que necesitan reprogramación
        /// </summary>
        /// <returns>Listado de clases que necesitan reprogramación</returns>
        [HttpGet("clases-necesitan-reprogramacion/get")]
        public async Task<IActionResult> GetClassesNeedReschedule()
        {
            var userId = GetUserId();
            var term = await _termService.GetActiveTerm();
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _classService.GetClassesToNeedReschedule(parameters, term.Id, userId);
            return Ok(result);
        }

        /// <summary>
        /// Método para crear la reprogramación de clases
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la reprogramación de clase</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("clases-necesitan-reprogramacion/crear")]
        public async Task<IActionResult> CreateClassNeedReschedule(ClassNeedRescheduleViewModel model)
        {
            var classAny = await _classRescheduleService.AnyByClass(model.ClassId, ConstantHelpers.CLASS_RESCHEDULE.STATUS.IN_PROCESS);

            if (classAny)
                return BadRequest("Ya hay una reprogramación en proceso para la clase seleccionada");

            var date = ConvertHelpers.DatepickerToDatetime(model.Date);
            var startTime = ConvertHelpers.TimepickerToTimeSpan(model.StartTime);
            var endTime = ConvertHelpers.TimepickerToTimeSpan(model.EndTime);

            var startDateTime = date.Add(startTime).ToUtcDateTime();
            var endDateTime = date.Add(endTime).ToUtcDateTime();

            if (DateTime.Compare(startDateTime, endDateTime) >= 0)
            {
                return BadRequest("La fecha inicial no puede ser mayor o igual a la fecha final");
            }

            var userId = GetUserId();
            var classReschedule = new ClassReschedule
            {
                ClassId = model.ClassId,
                UserId = userId,
                Justification = model.Justification,
                EndDateTime = endDateTime,
                StartDateTime = startDateTime,
                Replicate = false
            };

            await _classRescheduleService.Insert(classReschedule);
            return Ok();
        }
    }
}
