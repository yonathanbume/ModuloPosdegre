using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
        ConstantHelpers.ROLES.DEAN + "," +
        ConstantHelpers.ROLES.DEAN_SECRETARY + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)]
    [Area("CareerDirector")]
    [Route("director-carrera/estudiantes-invictos")]
    public class UnbeatenStudentController : BaseController
    {
        private readonly IStudentService _studentService;

        public UnbeatenStudentController(UserManager<ApplicationUser> userManager, IDataTablesService dataTablesService, IStudentService studentService) : base(userManager, dataTablesService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Vista donde se muestra el listado de estudiantes invictos
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
            => View();

        /// <summary>
        /// Obtiene el listado de estudiantes invictos para ser usado en tablas
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de estudiantes invictos</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStudents(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var userId = _userManager.GetUserId(User);
            var isDean = User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY);
            var result = await _studentService.GetUnbeatenStudentsDatatable(parameters, search, userId);
            return Ok(result);
        }
    }
}
