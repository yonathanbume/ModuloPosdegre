using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Areas.Teacher.Models.SubstituteExamRequestViewModel;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/solicitudes-de-examen-sustitutorio")]
    public class SubstituteExamRequestController : BaseController
    {
        private readonly ISubstituteExamService _substituteExamService;
        private readonly ISectionService _sectionService;
        private readonly ICourseTermService _courseTermService;
        private readonly AkdemicContext _context;
        private readonly ITeacherSectionService _teacherSectionService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly IStudentService _studentService;
        private readonly IConfigurationService _configurationService;
        public SubstituteExamRequestController(IUserService userService,
            IConfigurationService configurationService,
            ITermService termService,
            IDataTablesService dataTablesService,
            ISectionService sectionService,
            ICourseTermService courseTermService,
            AkdemicContext context,
            IStudentSectionService studentSectionService,
            IAcademicSummariesService academicSummariesService,
            IStudentService studentService,
            ISubstituteExamService substituteExamService,
            ITeacherSectionService teacherSectionService) : base(termService, userService, dataTablesService)
        {
            _sectionService = sectionService;
            _courseTermService = courseTermService;
            _context = context;
            _studentSectionService = studentSectionService;
            _academicSummariesService = academicSummariesService;
            _studentService = studentService;
            _teacherSectionService = teacherSectionService;
            _substituteExamService = substituteExamService;
            _configurationService = configurationService;
        }

        /// <summary>
        /// Vista principal donde se califica los examenes sustitutorios
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            var enabled_substitute_exam = bool.Parse(await _configurationService.GetValueByKey(AKDEMIC.CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ENABLED_SUBSTITUTE_EXAM));

            if (!enabled_substitute_exam)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            return View();
        }

        /// <summary>
        /// Obtiene el listado de exámenes sustitutorios asignados al docente logueado
        /// </summary>
        /// <param name="courseId">Identificador del curso</param>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <param name="statusId">Identificador del estado</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Listado de exámenes sustitutorios</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid? courseId, Guid? sectionId, byte? statusId, Guid? termId)
        {
            var teacherId = GetUserId();
            var sentParameters = _dataTablesService.GetSentParameters();

            if (!termId.HasValue)
            {
                var term = await GetActiveTerm();
                termId = term.Id;
            }

            var result = await _substituteExamService.GetDatatableByFilters(sentParameters, teacherId, termId, courseId, sectionId, statusId);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene los detalles del examen sustitutorios
        /// </summary>
        /// <param name="id">Identificador del examen sustitutorio</param>
        /// <returns>Detalles del examen</returns>
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var substituteExam = await _substituteExamService.GetAsync(id);
            var exam = new
            {
                id = substituteExam.Id,
                coursName = substituteExam.Section.CourseTerm.Course.Name,
                studentName = substituteExam.Student.User.FullName
            };
            return Ok(exam);
        }

        /// <summary>
        /// Obtiene los detalles del examen sustitutorios
        /// </summary>
        /// <param name="id">Identificador del examen sustitutorio</param>
        /// <returns>Detalles del examen</returns>
        [HttpGet("{id}/detalles")]
        public async Task<IActionResult> GetDetails(Guid id)
        {
            var entity = await _substituteExamService.GetAsync(id);
            var student = await _studentService.Get(entity.StudentId);
            student.User = await _userService.Get(student.UserId);

            var result = new
            {
                entity.PrevFinalScore,
                entity.ExamScore,
                entity.TeacherExamScore,
                student.User.FullName
            };

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado cursos asignados al docente logueado
        /// </summary>
        /// <returns>Listado de cursos</returns>
        [HttpGet("cursos")]
        public async Task<IActionResult> GetCourses()
        {
            var teacherId = GetUserId();
            var term = await GetActiveTerm();
            var sections = await _sectionService.GetAll(teacherId, null, term.Id);

            var courses = sections.Select(
                    x => new
                    {
                        Id = x.CourseTerm.CourseId,
                        Text = x.CourseTerm.Course.Name,
                    }
                ).ToList();

            courses.Insert(0, new { Id = Guid.Empty, Text = "Todos" });

            return Ok(courses);
        }

        /// <summary>
        /// Obtiene el listado de secciones asignadas al docente logueado
        /// </summary>
        /// <param name="courseId">Identificador del curso</param>
        /// <returns>Listado de secciones</returns>
        [HttpGet("cursos/{courseId}/secciones")]
        public async Task<IActionResult> GetSections(Guid courseId)
        {
            var teacherId = GetUserId();
            var term = await GetActiveTerm();

            var data = await _sectionService.GetAll(teacherId, null, term.Id, courseId);

            var sections = data.Select(
                    x => new
                    {
                        Id = x.Id,
                        Text = x.Code
                    }
                ).ToList();
            sections.Insert(0, new { Id = Guid.Empty, Text = "Todos" });

            return Ok(sections);
        }

        /// <summary>
        /// Método para asignar nota a un examen sustitutorio
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del examen sustitutorio</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("asignar-nota")]
        public async Task<IActionResult> AssignScore(AssignScoreViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var exam = await _substituteExamService.GetAsync(model.Id);

            var courseTerm = await _courseTermService.GetAsync(exam.CourseTermId);
            var term = await _termService.Get(courseTerm.TermId);

            if (term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                return BadRequest("Solo se puede calificar examenes del periodo académico activo.");

            if (exam.Status != ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED)
                return BadRequest("El alumno no ha sido registrado para el examen.");

            var studentSection = await _studentSectionService.GetByStudentAndCourseTerm(exam.StudentId, exam.Section.CourseTermId);

            if (await _context.GradeRecoveries.AnyAsync(x => x.StudentId == studentSection.StudentId && x.GradeRecoveryExam.SectionId == studentSection.SectionId && (x.GradeRecoveryExam.Status == ConstantHelpers.GRADE_RECOVERY_EXAM_STATUS.PENDING || x.GradeRecoveryExam.Status == ConstantHelpers.GRADE_RECOVERY_EXAM_STATUS.CONFIRMED)))
                return BadRequest("El alumno tiene un examen de recuperación de nota pendiente.");

            if (await _context.GradeCorrections.AnyAsync(y => y.Grade.StudentSectionId == studentSection.Id && y.State == ConstantHelpers.GRADECORRECTION_STATES.PENDING))
                return BadRequest("El alumno tiene una corrección de nota pendiente.");

            var substitute_exam_evaluation_type = int.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAMEN_EVALUATION_TYPE));

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNTUMBES)
            {
                if (model.Score > 11)
                {
                    return BadRequest("La nota no puede ser mayor a 11.");
                }
            }

            exam.Status = ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED;
            exam.PrevFinalScore = studentSection.FinalGrade;
            exam.TeacherExamScore = model.Score;

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UIGV)
            {
                var newScore = (int)Math.Round(model.Score);
                exam.ExamScore = newScore;

                await _substituteExamService.UpdateAsync(exam);
            }
            else
            {
                //CONFIGURACION DE EXAMEN SUSTITUTORIO
                if (substitute_exam_evaluation_type == ConstantHelpers.SUBSTITUTE_EXAM_EVALUATION_TYPE.DIRECTED_FINAL_GRADE)
                {
                    var newScore = (int)Math.Round(model.Score);
                    exam.ExamScore = newScore;
                    studentSection.FinalGrade = studentSection.FinalGrade > newScore ? studentSection.FinalGrade : newScore;
                }
                else if (substitute_exam_evaluation_type == ConstantHelpers.SUBSTITUTE_EXAM_EVALUATION_TYPE.AVERAGE_WITH_PREVIOUS_GRADE)
                {
                    var newScore = (int)Math.Round(((exam.PrevFinalScore.Value + model.Score) / 2), 0, MidpointRounding.AwayFromZero);
                    exam.ExamScore = newScore;
                    studentSection.FinalGrade = studentSection.FinalGrade > newScore ? studentSection.FinalGrade : newScore;
                }
                else if (substitute_exam_evaluation_type == ConstantHelpers.SUBSTITUTE_EXAM_EVALUATION_TYPE.GRADE_BY_FACTOR)
                {
                    var newScore = (int)Math.Round(Convert.ToDouble(model.Score) * ConstantHelpers.SUBSTITUTE_EXAM_EVALUATION_FACTOR.FACTORS[(int)Math.Round(model.Score)]);
                    exam.ExamScore = newScore;
                    studentSection.FinalGrade = studentSection.FinalGrade > newScore ? studentSection.FinalGrade : newScore;
                }

                if (studentSection.Status != ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS && studentSection.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI)
                    studentSection.Status = studentSection.FinalGrade >= term.MinGrade ? ConstantHelpers.STUDENT_SECTION_STATES.APPROVED : ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED;
                //exam.ExamScore = studentSection.FinalGrade;

                await _substituteExamService.UpdateAsync(exam);

                var academicHistory = await _context.AcademicHistories.Where(x => x.StudentId == exam.StudentId && x.SectionId == exam.SectionId).FirstOrDefaultAsync();
                if (academicHistory != null)
                {
                    var section = await _context.Sections.Where(x => x.Id == exam.SectionId).FirstOrDefaultAsync();

                    academicHistory.Grade = studentSection.FinalGrade;
                    academicHistory.Approved = studentSection.FinalGrade >= term.MinGrade;

                    await _context.SaveChangesAsync();

                    await _academicSummariesService.ReCreateStudentAcademicSummaries(studentSection.StudentId);
                }
            }

            return Ok("La nota ha sido asignada satisfactoriamente");
        }

        /// <summary>
        /// Método para obtener el valor de la configuración
        /// </summary>
        /// <param name="key">Identificador de la configuración</param>
        /// <returns>valor de la configuración</returns>
        private async Task<string> GetConfigurationValue(string key)
        {
            var values = await _configurationService.GetDataDictionary();
            return values.ContainsKey(key) ? values[key] :

                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES.ContainsKey(key) ?
                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[key] : "";
        }
    }
}
