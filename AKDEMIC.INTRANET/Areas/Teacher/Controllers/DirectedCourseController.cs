using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Teacher.Models.DirectedCourseViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.TEACHERS)]
    [Route("profesor/cursos-dirigidos")]
    public class DirectedCourseController : BaseController
    {
        private readonly IDirectedCourseService _directedCourseService;
        private readonly IDirectedCourseStudentService _directedCourseStudentService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly ITermService _termService;
        private readonly ICareerService _careerService;

        public DirectedCourseController(IDataTablesService dataTablesService,
            IDirectedCourseService directedCourseService,
            IDirectedCourseStudentService directedCourseStudentService,
            ITermService termService,
            IAcademicHistoryService academicHistoryService,
            ICareerService careerService
            ) : base(dataTablesService)
        {
            _directedCourseService = directedCourseService;
            _termService = termService;
            _academicHistoryService = academicHistoryService;
            _directedCourseStudentService = directedCourseStudentService;
        }

        /// <summary>
        /// Vista donde se califica los cursos dirigidos asignados al docente logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Método para obtener el listado de alumnos asignados a cursos drigidos del docente logueado
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de alumnos</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStudents(string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _directedCourseService.GetDataDatatable(sentParameters, null, null, null, null, search, User);
            return Ok(result);
        }

        /// <summary>
        /// Método para calificar al alumno
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del curso dirigido</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("calificar")]
        public async Task<IActionResult> DirectedCourseGrade(GradeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Verifique la información ingresada");
            }
            var directedCourseStudent = await _directedCourseStudentService.Get(model.Id);

            directedCourseStudent.Grade = model.Grade;
            directedCourseStudent.Status = model.Grade >= directedCourseStudent.DirectedCourse.Term.MinGrade
                ? CORE.Helpers.ConstantHelpers.Student.Procedures.DirectedCourse.States.APPROVED
                : CORE.Helpers.ConstantHelpers.Student.Procedures.DirectedCourse.States.DISAPPROVED;

            await _directedCourseStudentService.Update(directedCourseStudent);

            var finalGrade = (int)Math.Round(model.Grade, 0, MidpointRounding.AwayFromZero);

            var lastTry = await _academicHistoryService.GetLastByStudentAndCourse(directedCourseStudent.StudentId, directedCourseStudent.DirectedCourse.CourseId);

            var academicHistory = new AcademicHistory
            {
                Approved = finalGrade >= directedCourseStudent.DirectedCourse.Term.MinGrade,
                CourseId = directedCourseStudent.DirectedCourse.CourseId,
                Grade = finalGrade,
                Observations = "Curso Dirigido",
                StudentId = directedCourseStudent.StudentId,
                TermId = directedCourseStudent.DirectedCourse.Term.Id,
                Try = lastTry != null ? lastTry.Try + 1 : 1,
                Validated = true,
                Type = CORE.Helpers.ConstantHelpers.AcademicHistory.Types.DIRECTED,
                CurriculumId = directedCourseStudent.Student.CurriculumId
            };

            await _academicHistoryService.Insert(academicHistory);

            return Ok();
        }
    }
}
