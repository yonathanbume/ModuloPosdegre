using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/horario-semana")]
    public class WeekClassScheduleController : BaseController
    {
        private readonly ITeacherService _teacherService;
        private readonly AkdemicContext _context;
        private readonly INonTeachingLoadScheduleService _nonTeachingLoadScheduleService;
        private readonly IClassService _classService;

        public WeekClassScheduleController(IUserService userService,
            ITermService termService,
            ITeacherService teacherService,
            AkdemicContext context,
            INonTeachingLoadScheduleService nonTeachingLoadScheduleService,
            IClassService classService) : base(userService, termService)
        {
            _teacherService = teacherService;
            _context = context;
            _nonTeachingLoadScheduleService = nonTeachingLoadScheduleService;
            _classService = classService;
        }

        /// <summary>
        /// Vista donde se muestra el horario del docente logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene las clases de la semana actual
        /// </summary>
        /// <returns>Listado de clases</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetClasses(DateTime start, DateTime end)
        {
            var term = await GetActiveTerm();
            if (term == null)
                term = new ENTITIES.Models.Enrollment.Term();

            var userId = GetUserId();
            var result = await _teacherService.GetTeacherCompleteSchedule(term.Id, userId, start.AddDays(-1), end.AddDays(1));
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle de la clase
        /// </summary>
        /// <param name="id">Identificador de la clase</param>
        /// <returns>Detalles de la clase</returns>
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetClass(Guid id)
        {
            var term = await GetActiveTerm();
            var userId = GetUserId();

            var @class = await _classService.GetAsModelAByIdAndTeacherId(id, userId);

            if (@class != null)
            {
                return Ok(@class);
            }
            else
            {
                var nonTeachingLoad = await _nonTeachingLoadScheduleService.GetScheduleObjectDetail(id);
                return Ok(nonTeachingLoad);
            }
        }
    }
}
