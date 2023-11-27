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

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/clases")]
    public class ClassController : BaseController
    {
        private readonly IClassService _classService;

        public ClassController(IUserService userService,
            ITermService termService,
            IClassService classService) : base(userService, termService)
        {
            _classService = classService;
        }

        /// <summary>
        /// Obtiene el listado de clases sin dictar de la fecha actual en adelante del usuario logueado
        /// </summary>
        /// <returns>Listado de clases sin dictar</returns>
        [Route("get")]
        public async Task<IActionResult> GetClasses()
        {
            var userId = GetUserId();
            var term = await GetActiveTerm();
            var classes = await _classService.GetAll(null, term.Id, userId, null, DateTime.UtcNow, null, false);

            var result = classes.Select(x => new
            {
                id = x.Id,
                sectionId = x.SectionId,
                classNumber = x.ClassNumber,
                isDictated = x.IsDictated,
                startTime = x.StartTime.ToLocalDateTimeFormat(),
                weekNumber = x.WeekNumber,
                section = new
                {
                    teacherId = x.Section.TeacherSections.Select(ts => ts.TeacherId)
                }
            }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de clases asociadas al periodo actual del usuario logueado
        /// </summary>
        /// <returns>Listado de clases sin dictar</returns>
        [Route("get-no-dictadas")]
        public async Task<IActionResult> GetClassesNotDictated()
        {
            var userId = GetUserId();
            var term = await GetActiveTerm();
            if (term == null)
                term = new ENTITIES.Models.Enrollment.Term();
            var classes = await _classService.GetAll(null, term.Id, userId, null, null, null, false);

            var result = classes.Select(x => new
            {
                id = x.Id,
                sectionId = x.SectionId,
                classNumber = x.ClassNumber,
                isDictated = x.IsDictated,
                startTime = x.StartTime.ToLocalDateTimeFormat(),
                weekNumber = x.WeekNumber,
                section = new
                {
                    teacherId = x.Section.TeacherSections.Select(ts => ts.TeacherId)
                }
            }).ToList();

            return Ok(result);
        }
    }
}
