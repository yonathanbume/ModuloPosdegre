using System.Threading.Tasks;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize]
    [Route("calendario-academico")]
    public class AcademicCalendarDateController : BaseController
    {
        private readonly IAcademicCalendarDateService _academicCalendarDateService;

        public AcademicCalendarDateController(IAcademicCalendarDateService academicCalendarDateService) : base()
        {
            _academicCalendarDateService = academicCalendarDateService;
        }

        /// <summary>
        /// Obtiene la vista inicial del calendario académico
        /// </summary>
        /// <returns>Retorna una vista</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene una lista de fechas del calendario académico
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetAcademicCalendarDates()
        {
            var academicCalendarDates = await _academicCalendarDateService.GetAcademicCalendarDateForActiveTerm();

            return Ok(academicCalendarDates);
        }
    }
}
