using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/facultades")]
    public class FacultyController : BaseController
    {
        private readonly IFacultyService _facultyService;
        public FacultyController(
            IFacultyService facultyService) : base()
        {
            _facultyService = facultyService;
        }


        /// <summary>
        /// Obtiene el listado de facultades
        /// </summary>
        /// <returns>Objeto que contiene el listado de facultades</returns>
        [Route("get")]
        public async Task<IActionResult> GetFaculties()
        {
            var result = await _facultyService.GetFaculties3();

            return Ok(result);
        }
    }
}
