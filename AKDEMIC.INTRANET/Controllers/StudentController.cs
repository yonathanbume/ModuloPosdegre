using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize]
    [Route("alumnos")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        public StudentController(
            IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Obtiene una vista inicial de alumnos
        /// </summary>
        /// <returns>Retorna una vista</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene todos los estudiantes
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [Route("getalumnos")]
        public JsonResult GetStudents()
        {
            var result = _studentService.GetStudentWitData();

            return Json(result);
        }
    }
}
