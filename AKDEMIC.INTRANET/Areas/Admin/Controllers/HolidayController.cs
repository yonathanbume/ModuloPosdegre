using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.HolidayViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/feriados")]
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    public class HolidayController : BaseController
    {
        private readonly AkdemicContext _context;
        private readonly IHolidayService _holidayService;

        public HolidayController(
            IDataTablesService dataTablesService,
            AkdemicContext context,
            IHolidayService holidayService) : base(dataTablesService)
        {
            _context = context;
            _holidayService = holidayService;
        }

        /// <summary>
        /// Vista donde se gestionan los feriados
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public ActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de feriados
        /// </summary>
        /// <param name="searchValue">texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de feriados</returns>
        [HttpGet("listar")]
        public async Task<IActionResult> GetHolidays(string searchValue) => Ok(await _holidayService.GetHolidaysDatatable(_dataTablesService.GetSentParameters(), searchValue));

        /// <summary>
        /// Obtiene el detalle del feriado
        /// </summary>
        /// <param name="id">Identificador del feriado</param>
        /// <returns>objeto que contiene los datos del feriado</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHoliday(Guid id) => Ok(await _holidayService.GetHoliday(id));

        /// <summary>
        /// Método para agregar un nuevo feriado
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del feriado</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("agregar")]
        public async Task<IActionResult> Add([Bind(Prefix = "Add")] HolidayViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Verifique los datos ingresados.");
                }

                string trimName = model.Name.Trim();

                if (await _holidayService.AnyHolidayByName(trimName, null))
                {
                    return BadRequest("Ya existe un feriado con el mismo nombre.");
                }

                if (await _holidayService.AnyHolidayByDate(ConvertHelpers.DatepickerToUtcDateTime(model.Date), null))
                {
                    return BadRequest("Ya existe un feriado con la misma fecha.");
                }

                var template = new Holiday
                {
                    Name = trimName,
                    Date = ConvertHelpers.DatepickerToUtcDateTime(model.Date),
                    Type = model.Type,
                    NeedReschedule = model.NeedReschedule
                };

                if (model.NeedReschedule)
                {
                    var classesFiltered = await _context.Classes.Where(x => x.StartTime.Date >= template.Date.AddDays(-1).Date && x.StartTime.Date <= template.Date.AddDays(1).Date).ToListAsync();
                    var classes = classesFiltered.Where(x => x.StartTime.ToDefaultTimeZone().Date == template.Date.Date).ToList();

                    classes.ForEach(x => x.NeedReschedule = true);
                }

                await _context.Holidays.AddAsync(template);
                await _context.SaveChangesAsync();

                return Ok("Feriado creado satisfactoriamente.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al agregar el feriado.");
            }
        }

        /// <summary>
        /// Método para editar un feriado
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del feriado</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar")]
        public async Task<IActionResult> Edit([Bind(Prefix = "Edit")] HolidayViewModel model)
        {
            try
            {
                string trimName = model.Name.Trim();

                if (await _holidayService.AnyHolidayByName(trimName, model.Id))
                {
                    return BadRequest("Ya existe un feriado con el mismo nombre.");
                }

                Holiday template = await _holidayService.Get(model.Id.Value);
                template.Name = trimName;

                await _holidayService.Update(template);
                return Ok("Feriado actualizado satisfactoriamente.");
            }
            catch (Exception)
            {
                return BadRequest("Error al editar el feriado.");
            }
        }

        /// <summary>
        /// Método para eliminar un feriado
        /// </summary>
        /// <param name="id">Identificador del feriado</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var entity = await _holidayService.Get(id);

                if (entity.NeedReschedule)
                {
                    var classesFiltered = await _context.Classes.Where(x => x.StartTime.Date >= entity.Date.AddDays(-1).Date && x.StartTime.Date <= entity.Date.AddDays(1).Date).ToListAsync();
                    var classes = classesFiltered.Where(x => x.StartTime.ToDefaultTimeZone().Date == entity.Date.Date).ToList();

                    classes.ForEach(x => x.NeedReschedule = false);
                    await _context.SaveChangesAsync();
                }

                await _holidayService.Delete(entity);
                return Ok("El feriado ha sido eliminado.");
            }
            catch (Exception)
            {
                return BadRequest("Error al eliminar el feriado.");
            }
        }
    }
}
