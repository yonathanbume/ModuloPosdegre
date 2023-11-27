using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Geo;
using AKDEMIC.INTRANET.Areas.Admin.Models.LaboratoryViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Geo.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/laboratory")]
    public class LaboratoryController : BaseController
    {
        private readonly ILaboratoryRequestService _laboratoryRequestService;
        private readonly ITeacherSectionService _teacherSectionService;
        public LaboratoryController(AkdemicContext _context,
                                 UserManager<ApplicationUser> _userManager,
                                 ILaboratoryRequestService laboratoryRequestService,
                                 ITeacherSectionService teacherSectionService) : base(_context, _userManager)
        {
            _laboratoryRequestService = laboratoryRequestService;
            _teacherSectionService = teacherSectionService;
        }

        /// <summary>
        /// Obtiene el listado de solicitudes de reserva de labotaroios
        /// </summary>
        /// <returns>Objeto que contiene el listado de solicitudes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetRequests()
        {
            var userId = _userManager.GetUserId(User);

            var result = await _laboratoryRequestService.GetRequestsByUser(userId);

            //var query = _context.LaboratoyRequests
            //    .Where(x => x.TeacherId == userId)
            //    .AsQueryable();

            //var result = await query.Select(x => new
            //{
            //    id = x.Id,
            //    dateRequest = $"{x.Date.ToShortDateString()} {x.StartTime.ToLocalDateTimeFormat()} - {x.EndTime.ToLocalDateTimeFormat()}",
            //    description = x.Description,
            //    section = $"{x.Section.CourseTerm.Course.Name} - {x.Section.CourseTerm.Course.Code} - {x.Section.Code}",
            //    date = x.CreatedAt.ToLocalDateTimeFormat(),
            //    teacher = $"{x.Teacher.User.FullName}",
            //    string_state = ConstantHelpers.GEO.REQUEST.STATES[x.State],
            //    state = x.State,
            //    answered = x.State != ConstantHelpers.GEO.REQUEST.ACCEPTED && x.State != ConstantHelpers.GEO.REQUEST.DENIED,
            //    denied = x.State == ConstantHelpers.GEO.REQUEST.DENIED,
            //    createdAt = x.CreatedAt
            //}).OrderByDescending(x => x.createdAt.Value).ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de secciones aperturadas del docente logueado
        /// </summary>
        /// <param name="term">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene el listado de secciones</returns>
        [HttpGet("sections/select/filter/get")]
        public async Task<IActionResult> GetSections(string term)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _teacherSectionService.GetSectionsByUser(userId, term);
            //var query = _context.TeacherSections
            //    .Include(x => x.Section.CourseTerm.Course)
            //    .Where(x => x.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
            //    .Where(x => x.TeacherId == userId)
            //    .AsQueryable();

            //foreach (var filtro in term.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries))
            //    query = query.Where(x => x.Section.Code.ToLower().StartsWith(filtro.ToLower()) || x.Section.CourseTerm.Course.Code.StartsWith(filtro.ToLower()) || x.Section.CourseTerm.Course.Name.StartsWith(filtro.ToLower()));

            //var result = await query.Select(x => new
            //{
            //    id = x.SectionId,
            //    text = $"{x.Section.CourseTerm.Course.FullName} / {x.Section.Code}",
            //}).ToListAsync();

            return Ok(new { items = result });
        }

        /// <summary>
        /// Método para crear una solicitud de reserva de laboratorio
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la solicitud</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("request/add/post")]
        public async Task<IActionResult> AddBooking(AnswerRequestViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            var std = ConvertHelpers.TimepickerToUtcDateTime(model.TimeStart);
            var etd = ConvertHelpers.TimepickerToUtcDateTime(model.TimeEnd);

            if (std >= etd)
                return BadRequest("Rango de tiempo de no válido.");

            var Date = DateTime.ParseExact(model.Date,
                                    ConstantHelpers.FORMATS.DATE, CultureInfo.InvariantCulture);

            var StartTime = DateTime.ParseExact(model.TimeStart,
                                    ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay;

            var EndTime = DateTime.ParseExact(model.TimeEnd,
                                    ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay;

            var conflicts = _context.LaboratoyRequests.Any(x => IsInConflict(x.Date, Date, StartTime, x.StartTime, EndTime, x.EndTime) && x.State == ConstantHelpers.GEO.REQUEST.ACCEPTED);

            if (conflicts) return BadRequest("El laboratorio se encuentra ocupa dentro de ese rango de horas.");

            var request = new LaboratoyRequest
            {
                Description = model.Observation,
                SectionId = model.SectionId,
                Date = Date,
                DateResponse = DateTime.UtcNow,
                TeacherId = userId,
                StartTime = StartTime,
                EndTime = EndTime
            };

            await _laboratoryRequestService.InsertLaboratoyRequest(request);
            //await _context.LaboratoyRequests.AddAsync(request);
            //await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Vista principal donde se gestiona las solicitudes de reserva de laboratorio
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        #region --- HELPERS ---

        [NonAction]
        private bool IsInConflict(DateTime date1, DateTime date2, TimeSpan st1, TimeSpan st2, TimeSpan et1, TimeSpan et2)
        {
            return date1.Date == date2.Date && ((st1 <= st2 && et1 > st2) || (st1 < et2 && et1 >= et2) || (st2 <= st1 && et2 > st1) || (st2 < et1 && et2 >= et1));
        }
        #endregion
    }
}
