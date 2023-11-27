using AKDEMIC.CORE.Helpers;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/mis-cursos-extracurriculares")]
    public class ExtracurricularCourseStudentController : BaseController
    {
        private readonly IExtracurricularCourseGroupStudentService _extracurricularCourseGroupStudentService;
        private readonly IStudentService _studentService;

        public ExtracurricularCourseStudentController(IUserService userService,
            IStudentService studentService,
            IExtracurricularCourseGroupStudentService extracurricularCourseGroupStudentService)
            : base(userService)
        {
            _studentService = studentService;
            _extracurricularCourseGroupStudentService = extracurricularCourseGroupStudentService;
        }

        /// <summary>
        /// Vista donde se muestra los cursos extracurriculares donde el alumno logueado se ha inscrito
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de los cursos extracurriculares donde el alumno logueado se ha inscrito
        /// </summary>
        /// <returns>Listado de cursos extracurriculares</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetExtracurricularCourses()
        {
            var userId = GetUserId();
            var student = await _studentService.GetStudentByUser(userId);
            var extracurricularCourseGroupStudents = await _extracurricularCourseGroupStudentService.GetAllByStudent(student.Id);
            return Ok(extracurricularCourseGroupStudents.Select(x => new
            {
                id = x.Id,
                group = $"{x.Group.Code}",
                course = $"{x.Group.ExtracurricularCourse.Code} - {x.Group.ExtracurricularCourse.Name}",
                state = x.Payment.Status == 1 ? "Pago pendiente de: S/." + x.Group.ExtracurricularCourse.Price : "Matriculado",
                score = x.Payment.Status == 1 ? "-" : x.Score.ToString("0.00")
            }).ToList());
        }
    }
}
