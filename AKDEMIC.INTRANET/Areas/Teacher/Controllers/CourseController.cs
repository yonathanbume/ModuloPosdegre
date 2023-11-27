using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/cursos")]
    public class CourseController : BaseController
    {
        private readonly ISectionService _sectionService;

        public CourseController(IUserService userService,
            ITermService termService,
            ISectionService sectionService) : base(userService, termService)
        {
            _sectionService = sectionService;
        }


        /// <summary>
        /// Método para obtener el listado de secciones asigandas al docente logueado.
        /// Secciones habilitadas en el periodo activo.
        /// </summary>
        /// <returns>Listado de secciones</returns>
        [Route("get")]
        public async Task<IActionResult> GetCourses()
        {
            var userId = GetUserId();
            var term = await GetActiveTerm();
            if (term == null)
                term = new ENTITIES.Models.Enrollment.Term();
            var sections = await _sectionService.GetAll(userId, null, term.Id);
            var result = sections.Select(x => new
            {
                teacherId = x.TeacherSections.Select(ts => ts.TeacherId),
                courseTerm = new
                {
                    courseId = x.CourseTerm.CourseId,
                    course = new
                    {
                        fullName = x.CourseTerm.Course.FullName
                    },
                    term = new
                    {
                        status = x.CourseTerm.Term.Status
                    }
                }
            }).ToList();
            return Ok(result);
        }
    }
}
