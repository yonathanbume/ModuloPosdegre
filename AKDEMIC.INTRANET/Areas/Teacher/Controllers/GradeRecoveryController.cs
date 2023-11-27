using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Areas.Teacher.Models.GradeRecoveryViewModels;
using AKDEMIC.INTRANET.Controllers;
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
    [Route("profesor/examenes-recuperacion-nota")]
    public class GradeRecoveryController : BaseController
    {
        private readonly IGradeRecoveryExamService _gradeRecoveryExamService;
        private readonly ISectionService _sectionService;
        private readonly IGradeService _gradeService;
        private readonly IGradeRecoveryService _gradeRecoveryService;
        private readonly IConfigurationService _configurationService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IClassroomService _classroomService;
        private readonly ISubstituteExamService _substituteExamService;

        public GradeRecoveryController(
            IGradeRecoveryExamService gradeRecoveryExamService,
            ISectionService sectionService,
            IGradeService gradeService,
            IGradeRecoveryService gradeRecoveryService,
            ITermService termService,
            IConfigurationService configurationService,
            IStudentSectionService studentSectionService,
            IClassroomService classroomService,
            IUserService userService,
            IDataTablesService dataTablesService,
            ISubstituteExamService substituteExamService
            ) : base(termService, userService, dataTablesService)
        {
            _gradeRecoveryExamService = gradeRecoveryExamService;
            _sectionService = sectionService;
            _gradeService = gradeService;
            _gradeRecoveryService = gradeRecoveryService;
            _configurationService = configurationService;
            _studentSectionService = studentSectionService;
            _classroomService = classroomService;
            _substituteExamService = substituteExamService;
        }

        /// <summary>
        /// Vista donde se gestionan las recuperaciones de notas
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            var enabled_grade_recovery = bool.Parse(await _configurationService.GetValueByKey(AKDEMIC.CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ENABLED_GRADE_RECOVERY));

            if (!enabled_grade_recovery)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            return View();
        }

        /// <summary>
        /// Obtiene el listado de exámenes de recuperación de notas asignados al docente logueado
        /// </summary>
        /// <param name="status">Identificador del estado</param>
        /// <returns>Listado de exámenes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(byte? status)
        {
            var teacherId = GetUserId();
            var sentParameters = _dataTablesService.GetSentParameters();

            var result = await _gradeRecoveryExamService.GetGradeRecoveryExamByTeacherDatatable(sentParameters, status, teacherId);
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se detalla el examen de recuperación de notas
        /// </summary>
        /// <param name="gradeRecoveryExamId">Identificador del examen de recuperación de notas</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("detalles/{gradeRecoveryExamId}")]
        public async Task<IActionResult> Detail(Guid gradeRecoveryExamId)
        {
            var enabled_grade_recovery = bool.Parse(await _configurationService.GetValueByKey(AKDEMIC.CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ENABLED_GRADE_RECOVERY));

            if (!enabled_grade_recovery)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            var entity = await _gradeRecoveryExamService.Get(gradeRecoveryExamId);
            var section = await _sectionService.GetWithIncludes(entity.SectionId);
            var classroom = await _classroomService.Get(entity.ClassroomId);

            var model = new GradeRecoveryExamViewModel
            {
                GradeRecoveryExamId = entity.Id,
                StartDateTime = entity.StartTime == DateTime.MinValue ? "-" : entity.StartTime.ToLocalDateTimeFormat(),
                EndDateTime = entity.EndTime == DateTime.MinValue ? "-" : entity.EndTime.ToLocalDateTimeFormat(),
                Section = section.Code,
                Course = section.CourseTerm.Course.FullName,
                Term = section.CourseTerm.Term.Name,
                Classroom = classroom.Code,
                Status = entity.Status
            };

            return View(model);
        }

        /// <summary>
        /// Obtiene el listado de alumnos asignados al examen de recuperación de notas
        /// </summary>
        /// <param name="gradeRecoveryExamId">Identificador del examen de recuperación de notas</param>
        /// <returns>Listado de alumnos asignados</returns>
        [HttpGet("get-estudiantes-asignados")]
        public async Task<IActionResult> GetAssignedStudents(Guid gradeRecoveryExamId)
        {
            var students = await _gradeRecoveryService.GetAssignedStudentsWithData(gradeRecoveryExamId);
            return Ok(students);
        }

        /// <summary>
        /// Obtiene el listado de alumnos calificados asignados al examen de recuperación de notas
        /// </summary>
        /// <param name="gradeRecoveryExamId">Identificador del examen de recuperación de notas</param>
        /// <returns>Listado de alumnos</returns>
        [HttpGet("get-estudiantes-asignados-examen-concluido")]
        public async Task<IActionResult> GetAssignedStudentsExecuted(Guid gradeRecoveryExamId)
        {
            var students = await _gradeRecoveryService.GetAssignedStudentsExecuted(gradeRecoveryExamId);
            return Ok(students);
        }

        /// <summary>
        /// Método para registrar las notas del examen.
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del examen de recuperación de notas.</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("guardar-notas")]
        public async Task<IActionResult> SaveGrades(GradeRecoveryStudentViewModel model)
        {
            var exam = await _gradeRecoveryExamService.Get(model.GradeRecoveryExam);
            var section = await _sectionService.Get(exam.SectionId);

            var gradeRecoveryModality = Convert.ToByte(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_MODALITY));

            if (exam.Status != ConstantHelpers.GRADE_RECOVERY_EXAM_STATUS.CONFIRMED)
                return BadRequest("No se puede calificar este examen");

            if (model.Students.Any(y => !y.Value.HasValue) || model.Students.Any(y => y.Value > 20))
            {
                return BadRequest("Es necesario ingresar todas las notas.");
            }

            var studentsAssigned = await _gradeRecoveryService.GetByGradeRecoveryExamId(model.GradeRecoveryExam);
            var grades = await _gradeService.GetGradesBySectionId(exam.SectionId);

            var confi = await _configurationService.GetByKey(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_TYPE_GRADE_RECOVERY);
            var evaluationTypeAssigned = Guid.Parse(confi.Value);

            if (evaluationTypeAssigned != Guid.Empty)
            {
                grades = grades.Where(x => x.Evaluation.EvaluationTypeId == evaluationTypeAssigned).ToList();
            }

            foreach (var item in studentsAssigned)
            {
                var bymodel = model.Students.Where(x => x.StudentId == item.StudentId).FirstOrDefault();
                var gradesByStudent = grades.Where(x => x.StudentSection.StudentId == item.StudentId).ToList();
                var minGrade = gradesByStudent.OrderBy(y => y.Value).ThenByDescending(y => y.Evaluation.Percentage).FirstOrDefault();
                var anySubstituteExams = await _substituteExamService.AnySubstituteExamByStudent(item.StudentId, section.Id, ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED);

                if (gradesByStudent is null || !gradesByStudent.Any())
                {
                    return BadRequest($"No se encontró nota a reemplazar para el alumno {item.Student.User.FullName}. Verificar la configuración del curso.");
                }

                if (anySubstituteExams)
                {
                    return BadRequest($"El estudiante {item.Student.User.FullName} tiene un examen sustitutorio pendiente.");
                }


                if (bymodel != null)
                {
                    item.Absent = bymodel.IsAbsent;
                    item.PrevFinalScore = minGrade.Value;
                    item.ExamScore = bymodel.Value;
                    item.GradeId = minGrade.Id;

                    if (gradeRecoveryModality == ConstantHelpers.GRADE_RECOVERY_EXAM_MODALITY.DIRECT_REPLACEMENT)
                    {
                        minGrade.Value = bymodel.Value.Value;
                    }
                    else if (gradeRecoveryModality == ConstantHelpers.GRADE_RECOVERY_EXAM_MODALITY.HIGHEST_GRADE)
                    {
                        if (bymodel.Value.Value > minGrade.Value)
                        {
                            minGrade.Value = bymodel.Value.Value;
                        }
                    }
                    else
                    {
                        return BadRequest("No se encontró la modalidad de la recuperación de nota");
                    }
                }
            }

            exam.Status = ConstantHelpers.GRADE_RECOVERY_EXAM_STATUS.EXECUTED;

            await _gradeRecoveryService.Updaterange(studentsAssigned);
            await _studentSectionService.RecalculateFinalGradeBySectionId(exam.SectionId);
            return Ok();
        }
    }
}
