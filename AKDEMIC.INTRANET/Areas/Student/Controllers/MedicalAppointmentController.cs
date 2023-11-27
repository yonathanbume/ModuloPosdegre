using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize]
    [Area("Student")]
    [Route("alumno/citas_medicas")]
    public class MedicalAppointmentController : BaseController
    {
        public MedicalAppointmentController(AkdemicContext context, UserManager<ApplicationUser> userManager) : base(context, userManager) { }

        /// <summary>
        /// Vista donde se muestran las citas médicas programadas para el alumno logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// Listado de citas médicas programadas
        /// </summary>
        /// <param name="did">Identificador del doctor</param>
        /// <returns>Listado de citas médicas</returns>
        [HttpGet("doctor/{did}/citas/get")]
        public async Task<IActionResult> GetDoctorMedicalAppointments(string did, DateTime start, DateTime end)
        {
            var Id = _userManager.GetUserId(User);
            var Student = await _context.Students.Where(x => x.UserId == Id).FirstOrDefaultAsync();
            var today = (int)(DateTime.UtcNow.DayOfWeek);
            //ToUtcTime
            start = start.ToUtcDateTime();
            end = end.ToUtcDateTime();
            var result = await _context.MedicalAppointments
                .Where(x => x.DoctorId == did && x.StartTimeMedicalCare >= start && x.EndTimeMedicalCare <= end)
                .Select(x => new
                {
                    id = x.Id,
                    doc = x.DoctorId,
                    title = "Cita con el doctor " + x.Doctor.FullName,
                    allDay = false,
                    start = x.StartTimeMedicalCare.ToDefaultTimeZone().ToString("yyyy-MM-dd HH:mm:ss"),
                    end = x.EndTimeMedicalCare.ToDefaultTimeZone().ToString("yyyy-MM-dd HH:mm:ss"),
                    busy = false
                }).ToListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Método para reservar una cita médica
        /// </summary>
        /// <param name="mid">Identificador de la cita médica</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("reservar_cita_medica/{mid}/post")]
        public async Task<IActionResult> BookMedicalAppointment(Guid mid)
        {
            //Ver si se rehace la logica
            var userId = _userManager.GetUserId(User);
            var medicalAppointment = await _context.MedicalAppointments.Where(x => x.Id == mid).FirstOrDefaultAsync();
            var result = await _context.MedicalAppointments.AnyAsync(x => x.UserId == userId && !x.Attended);
            if (result)
            {
                return BadRequest("No puede reservar más citas , ya que presenta citas previas aún sin atender");
            }

            //medicalAppointment.UserId = userId;
            //await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
