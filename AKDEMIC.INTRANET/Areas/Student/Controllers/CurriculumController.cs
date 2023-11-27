using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Areas.Student.Models.CurriculumViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/malla-curricular")]
    public class CurriculumController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IAcademicYearCourseService _academicYearCourseService;

        public CurriculumController(IUserService userService, IStudentService studentService,
            IAcademicYearCourseService academicYearCourseService)
            : base(userService)
        {
            _studentService = studentService;
            _academicYearCourseService = academicYearCourseService;
        }

        /// <summary>
        /// Vista donde se muestra la malla curricular del alumno logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);
            var academicYearCourses = await _academicYearCourseService.GetAllByCurriculumWithPrerequisites(student.CurriculumId);

            ////FILTRO POR PROGRAMA ACADEMICO
            if (student.AcademicProgramId.HasValue)
                academicYearCourses = academicYearCourses.Where(x => !x.Course.AcademicProgramId.HasValue
                    || (x.Course.AcademicProgramId.HasValue && (x.Course.AcademicProgramId == student.AcademicProgramId || x.Course.AcademicProgram.Code == "00")))
                    .ToList();
            else
                academicYearCourses = academicYearCourses.Where(x => !x.Course.AcademicProgramId.HasValue).ToList();

            var result = academicYearCourses.Select(x => new CurriculumViewModel
            {
                Cycle = x.AcademicYear,
                CodeCourse = x.Course.Code,
                Credits = x.Course.Credits,
                PracticalHours = x.Course.PracticalHours,
                SeminarHours = x.Course.SeminarHours,
                TheoreticalHours = x.Course.TheoreticalHours,
                VirtualHours = x.Course.VirtualHours,
                NameCourse = x.Course.Name,
                AcademicProgram = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Name : "00",
                Requisites = x.PreRequisites
                        .Select(s => new PreRequisiteViewModel { Name = $"{s.Course.Code} - {s.Course.Name}" })
                        .ToList()
            })
                .OrderBy(x => x.Cycle)
                .ToList();

            return View(result);
        }
    }
}

