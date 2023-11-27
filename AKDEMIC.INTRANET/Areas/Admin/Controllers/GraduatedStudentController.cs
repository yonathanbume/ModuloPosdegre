using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/estudiantes-egresados")]
    public class GraduatedStudentController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IDataTablesService _dataTablesService;
        private readonly ICurriculumService _curriculumService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IAcademicYearCourseService _academicYearCourseService;
        private readonly ITermService _termService;

        public GraduatedStudentController(
            IStudentService studentService,
            IDataTablesService dataTablesService,
            ICurriculumService curriculumService,
            IAcademicHistoryService academicHistoryService,
            IAcademicYearCourseService academicYearCourseService,
            ITermService termService
            )
        {
            _studentService = studentService;
            _dataTablesService = dataTablesService;
            _curriculumService = curriculumService;
            _academicHistoryService = academicHistoryService;
            _academicYearCourseService = academicYearCourseService;
            _termService = termService;
        }

        /// <summary>
        /// Vista donde se listan los estudiantes que pueden cambiar a estado egresado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
            => View();

        /// <summary>
        /// Obtiene el listado que pueden pasar a estado de egresados
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de estudiantes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetGraduatedStudentsDatatable(Guid? facultyId, Guid? careerId, Guid? curriculumId, Guid? termId, string searchValue)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetStudentsToGraduateDatatable(parameters, termId, facultyId, careerId, curriculumId, searchValue, User);
            return Ok(result);
        }

        /// <summary>
        /// Método para cambiar el estado del estudiante a egresado
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("cambiar-estado")]
        public async Task<IActionResult> UpdateStudentStatus(Guid studentId)
        {
            var student = await _studentService.Get(studentId);
            var curriculum = await _curriculumService.Get(student.CurriculumId);
            var academicYearCourses = await _academicYearCourseService.GetAcademicYearCoursesByCurriculumId(student.CurriculumId);

            //var creditsApproved = await _studentService.GetApprovedCreditsByStudentId(student.Id);

            var requiredCreditsApproved = await _studentService.GetRequiredApprovedCredits(student.Id);
            var electiveCreditsApproved = await _studentService.GetElectiveApprovedCredits(student.Id);

            var academicHistories = await _academicHistoryService.GetByStudentId(student.Id);
            academicHistories = academicHistories.Where(x => academicYearCourses.Any(y => y.CourseId == x.CourseId && !y.IsElective)).ToList();

            var academicHistoriesGroupCourse = academicHistories.GroupBy(x => x.CourseId)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count(),
                    listApproved = x.Select(x => x.Approved).ToList()
                })
                .ToList();

            if (academicHistoriesGroupCourse.Any(y => !y.listApproved.Any(y => y)))
            {
                var academicHistory = academicHistoriesGroupCourse.Where(x => !x.listApproved.Any(y => y)).FirstOrDefault();
                var course = academicHistories.Where(x => x.CourseId == academicHistory.Key).Select(x => x.Course.Name).FirstOrDefault();
                return BadRequest($"El estudiante debe el curso {course}");
            }

            if (curriculum.RequiredCredits > requiredCreditsApproved)
                return BadRequest("El estudiante no tiene los créditos requeridos suficientes para cambiar su estado.");

            if (curriculum.ElectiveCredits > electiveCreditsApproved)
                return BadRequest("El estudiante no tiene los créditos electivos requeridos para cambiar su estado.");

            var term = await _studentService.GetLastTermEnrolled(studentId);
            if (term != null && !student.GraduationTermId.HasValue) student.GraduationTermId = term.Id;

            student.Status = ConstantHelpers.Student.States.GRADUATED;
            await _studentService.Update(student);
            return Ok();
        }

        /// <summary>
        /// Obtiene la cantidad de créditos aprobados por el alumno
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <returns>Objeto que contiene la cantidad de créditos</returns>
        [HttpGet("creditos-aprobados/alumno")]
        public async Task<IActionResult> GetApprovedCredits(Guid studentId)
        {
            var credits = await _studentService.GetApprovedCreditsByStudentId(studentId);
            return Ok(credits);
        }

        /// <summary>
        /// Obtiene el historial académico del alumno
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <returns>Objeto que contiene el listado de cursos del plan académico</returns>
        [HttpGet("historial-academico/alumno")]
        public async Task<IActionResult> GetCurriculumCourses(Guid studentId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _academicYearCourseService.GetCurriculumCoursesDatatable(sentParameters, studentId);
            return Ok(result);
        }
    }
}
