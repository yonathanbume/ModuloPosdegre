using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.GradeRecoveryViewModels;
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

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE)]
    [Area("Admin")]
    [Route("admin/recuperacion-notas")]
    public class GradeRecoveryController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly ITermService _termService;
        private readonly ICourseTermService _courseTermService;
        private readonly IGradeRecoveryExamService _gradeRecoveryExamService;
        private readonly IConfigurationService _configurationService;
        private readonly IGradeRecoveryService _gradeRecoveryService;
        private readonly ISectionService _sectionService;
        private readonly IClassroomService _classroomService;
        private readonly AkdemicContext _context;
        private readonly IEvaluationTypeService _evaluationTypeService;
        private readonly IStudentSectionService _studentSectionService;

        public GradeRecoveryController(
            IDataTablesService dataTablesService,
            ITermService termService,
            ICourseTermService courseTermService,
            IGradeRecoveryExamService gradeRecoveryExamService,
            IConfigurationService configurationService,
            IGradeRecoveryService gradeRecoveryService,
            ISectionService sectionService,
            IClassroomService classroomService,
            AkdemicContext context,
            IEvaluationTypeService evaluationTypeService,
            IStudentSectionService studentSectionService
            )
        {
            _dataTablesService = dataTablesService;
            _termService = termService;
            _courseTermService = courseTermService;
            _gradeRecoveryExamService = gradeRecoveryExamService;
            _configurationService = configurationService;
            _gradeRecoveryService = gradeRecoveryService;
            _sectionService = sectionService;
            _classroomService = classroomService;
            _context = context;
            _evaluationTypeService = evaluationTypeService;
            _studentSectionService = studentSectionService;
        }

        /// <summary>
        /// Vista donde se gestionan los exámenes de recuperación de notas
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de recuperaciones de notas
        /// </summary>
        /// <param name="careerId">identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="courseId">Identificador del curso</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>objeto que contiene el listado de recuperacion</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid? careerId, Guid? curriculumId, Guid? courseId, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _gradeRecoveryExamService.GetGradeRecoveryDetailDatatable(parameters, careerId, curriculumId, null, courseId, search);
            return Ok(result);
        }

        /// <summary>
        /// Método para crear un exámen de recuperación de nota
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la recuperación de nota</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("agregar-seccion")]
        public async Task<IActionResult> AddGradeRevory(GradeRecoveryExamViewModel model)
        {
            if (await _gradeRecoveryExamService.AnyBySectionId(model.SectionId))
                return BadRequest("Ya se ha registrado un examen para la sección seleccionada.");

            if (!model.ClassroomId.HasValue && model.ClassroomId != Guid.Empty)
            {
                var classroom = await _context.Classrooms.Where(x => x.Code.Trim() == "Sin Asignar").FirstOrDefaultAsync();
                if (classroom is null)
                {
                    return BadRequest("Debe seleccionar un aula.");
                }

                model.ClassroomId = classroom.Id;
            }

            var entity = new GradeRecoveryExam
            {
                ClassroomId = model.ClassroomId.Value,
                SectionId = model.SectionId,
                Status = ConstantHelpers.GRADE_RECOVERY_EXAM_STATUS.PENDING
            };

            if (!string.IsNullOrEmpty(model.StartDate) && !string.IsNullOrEmpty(model.StartTime))
            {
                var startDate = ConvertHelpers.DatepickerToDatetime(model.StartDate);
                var startTime = ConvertHelpers.TimepickerToDateTime(model.StartTime);

                var startDateTime = startDate.Date.Add(startTime.TimeOfDay);
                var startDateTimeUTC = startDateTime.ToUtcDateTime();

                var endDateTimeUTC = startDateTimeUTC.AddMinutes(model.Duration);

                if (startDateTimeUTC < DateTime.UtcNow)
                    return BadRequest("La fecha y hora del examen debe ser mayor a la fecha y hora actual.");

                if (model.Duration <= 0)
                    return BadRequest("Ingresar una duración válida para el exámen.");

                entity.StartTime = startDateTimeUTC;
                entity.EndTime = endDateTimeUTC;
            }

            await _gradeRecoveryExamService.Insert(entity);
            return Ok(entity.Id);
        }

        /// <summary>
        /// Vista donde se detalla el examen de recuperación de nota
        /// </summary>
        /// <param name="id">Identificador del examen de recuperación de nota</param>
        /// <returns>Vista</returns>
        [HttpGet("examen/detalles/{id}")]
        public async Task<IActionResult> GradeRecoveryDetail(Guid id)
        {
            var entity = await _gradeRecoveryExamService.Get(id);
            var section = await _sectionService.GetWithIncludes(entity.SectionId);
            var classroom = await _classroomService.Get(entity.ClassroomId);
            var confi = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_TYPE_GRADE_RECOVERY);
            var evaluationTypeAssigned = Guid.Parse(confi);

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

            if (evaluationTypeAssigned != Guid.Empty)
            {
                var evaluationType = await _evaluationTypeService.Get(evaluationTypeAssigned);
                model.EvaluationTypeAssigned = evaluationType.Name;
            }

            return View(model);
        }

        /// <summary>
        /// Obtiene el listado de estudiantes asignados a la sección del examen de recuperación de notas
        /// </summary>
        /// <param name="gradeRecoveryExamId">Identificador del examen de recuperación de nota</param>
        /// <returns>Objeto que contiene el listado de estudiantes asignados</returns>
        [HttpGet("get-estudiantes/{gradeRecoveryExamId}")]
        public async Task<IActionResult> GetStudentSections(Guid gradeRecoveryExamId)
        {
            var exam = await _gradeRecoveryExamService.Get(gradeRecoveryExamId);

            var section = await _context.Sections.Where(x => x.Id == exam.SectionId)
                .Select(x => new
                {
                    x.Id,
                    x.CourseTerm.TermId,
                    x.CourseTerm.Term.MinGrade
                })
                .FirstOrDefaultAsync();

            var studentSections = await _studentSectionService.GetAllSectionStudentsWithUserBySectionId(exam.SectionId);
            var gradesRecovery = await _gradeRecoveryService.GetByGradeRecoveryExamId(gradeRecoveryExamId);

            var confiMinGrade = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_MIN_EVALUATION_GRADE);
            var minGrade = Convert.ToDecimal(confiMinGrade);

            var confiEnabledApproved = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_ENABLED_TO_APPROVED);
            var enabledToApproved = Convert.ToBoolean(confiEnabledApproved);

            var students = studentSections
                .Select(x => new
                {
                    x.StudentId,
                    x.Student.User.FullName,
                    x.Student.User.UserName,
                    x.FinalGrade
                })
                .ToList();

            var studentsAssigned = gradesRecovery.Select(x => x.Student).ToList();

            var result = students
                .Select(x => new
                {
                    Id = x.StudentId,
                    x.FullName,
                    x.UserName,
                    isAssigned = studentsAssigned.Any(y => y.Id == x.StudentId),
                    x.FinalGrade
                })
                .ToList();

            result = result.Where(x => x.isAssigned || x.FinalGrade >= minGrade).ToList();

            if (!enabledToApproved)
            {
                result = result.Where(x => x.isAssigned || x.FinalGrade < section.MinGrade).ToList();
            }

            return Ok(result);

        }

        /// <summary>
        /// Obtiene el listado de estudiantes calificados
        /// </summary>
        /// <param name="gradeRecoveryExamId">Identificador del examen de recuperación de nota</param>
        /// <returns>Objeto que contiene el listado de estudiantes calificados</returns>
        [HttpGet("get-estudiantes-asignados-examen-concluido")]
        public async Task<IActionResult> GetAssignedStudentsExecuted(Guid gradeRecoveryExamId)
        {
            var students = await _gradeRecoveryService.GetAssignedStudentsExecuted(gradeRecoveryExamId);
            return Ok(students);
        }

        /// <summary>
        /// Método para confirmar el examen de recuperación de nota
        /// </summary>
        /// <param name="gradeRecoveryExamId">Identificador del examen de recuperación de nota</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("confirmar-examen")]
        public async Task<IActionResult> ConfirmExam(Guid gradeRecoveryExamId)
        {
            var entity = await _gradeRecoveryExamService.Get(gradeRecoveryExamId);
            var section = await _context.Sections.Where(x => x.Id == entity.SectionId).FirstOrDefaultAsync();
            var term = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).Select(x => x.Term).FirstOrDefaultAsync();

            var studentsAssigned = await _gradeRecoveryService.GetByGradeRecoveryExamId(gradeRecoveryExamId);

            if (!studentsAssigned.Any())
                return BadRequest("No se encontraron estudiantes asignados al examen");

            var studentSectionsAssigned = await _context.StudentSections.Where(x => x.SectionId == entity.SectionId)
                .Select(x => new
                {
                    x.StudentId,
                    x.SectionId,
                    x.Student.User.FullName,
                    x.FinalGrade
                })
                .ToListAsync();

            studentSectionsAssigned = studentSectionsAssigned.Where(x => studentsAssigned.Any(y => y.StudentId == x.StudentId)).ToList();

            var confi = await _configurationService.GetByKey(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_TYPE_GRADE_RECOVERY);
            var confiMinGrade = await _configurationService.GetByKey(ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_MIN_EVALUATION_GRADE);

            var evaluationTypeAssigned = Guid.Parse(confi.Value);
            var minGrade = int.Parse(confiMinGrade.Value);

            var confiEnabledApproved = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_ENABLED_TO_APPROVED);
            var enabledToApproved = Convert.ToBoolean(confiEnabledApproved);

            var queryGrades = _context.Grades.Where(x => x.StudentSection.SectionId == entity.SectionId).AsNoTracking();

            if (evaluationTypeAssigned != Guid.Empty)
            {
                queryGrades = queryGrades.Where(x => x.Evaluation.EvaluationTypeId == evaluationTypeAssigned);
            }

            var grades = await queryGrades
                .Include(x => x.Evaluation)
                .Select(x => new
                {
                    x.Value,
                    x.StudentSection.StudentId
                })
                .ToListAsync();

            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
            var substituteExams = await _context.SubstituteExams.Where(x => (x.SectionId == section.Id || x.CourseTermId == courseTerm.Id) && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED).ToListAsync();

            foreach (var item in studentsAssigned)
            {
                if (!grades.Any(y => y.StudentId == item.StudentId))
                {
                    return BadRequest($"El estudiante {item.Student.User.FullName} no tiene nota a reemplazar.");
                }
            }

            foreach (var student in studentSectionsAssigned)
            {
                if (student.FinalGrade < minGrade)
                {
                    return BadRequest($"El estudiante {student.FullName} no cumple con la nota mínima para dar el examen.");
                }

                if (substituteExams.Any(y => y.StudentId == student.StudentId))
                {
                    return BadRequest($"El estudiante {student.FullName} tiene un examen sustitutorio pendiente.");
                }

                if (!enabledToApproved)
                {
                    if (student.FinalGrade >= term.MinGrade)
                        return BadRequest($"El alumno {student.FullName} se encuentra con promedio aprobatorio.");
                }
            }

            entity.Status = ConstantHelpers.GRADE_RECOVERY_EXAM_STATUS.CONFIRMED;
            await _gradeRecoveryExamService.Update(entity);
            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de estudiantes asignados al examen de recuperación de nota
        /// </summary>
        /// <param name="gradeRecoveryExamId">Identificador del examen de recuperación de nota</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de estudiantes</returns>
        [HttpGet("get-estudiantes-asignados/{gradeRecoveryExamId}")]
        public async Task<IActionResult> GetAssignedStudents(Guid gradeRecoveryExamId, string searchValue)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _gradeRecoveryService.GetGradeRecoveryDatatable(parameters, gradeRecoveryExamId, searchValue);
            return Ok(result);
        }

        /// <summary>
        /// Método para asignar estudiantes al examen de recuperación de nota
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del alumno</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("asignar-estudiantes")]
        public async Task<IActionResult> AssignStudents(GradeRecoveryStudentViewModel model)
        {
            var exam = await _gradeRecoveryExamService.Get(model.GradeRecoveryExamId);

            if (exam.Status != ConstantHelpers.GRADE_RECOVERY_EXAM_STATUS.PENDING)
                return BadRequest("No se puede asignar estudiantes a un exámen confirmado o realizado.");

            var section = await _sectionService.Get(exam.SectionId);
            var term = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).Select(x => x.Term).FirstOrDefaultAsync();
            var studentsAssigned = await _gradeRecoveryService.GetByGradeRecoveryExamId(model.GradeRecoveryExamId);

            var evaluations = await _context.Evaluations.Where(x => x.CourseTermId == section.CourseTermId).ToListAsync();

            var studentSections = await _context.StudentSections.Where(x => x.SectionId == exam.SectionId)
                .Select(x => new
                {
                    x.StudentId,
                    x.FinalGrade,
                    x.Student.User.FullName,
                    Grades = x.Grades.ToList()
                })
                .ToListAsync();

            var confiMinGrade = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_MIN_EVALUATION_GRADE);
            var minGrade = Convert.ToDecimal(confiMinGrade);

            var confiEnabledApproved = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_ENABLED_TO_APPROVED);
            var enabledToApproved = Convert.ToBoolean(confiEnabledApproved);

            var toAssign = new List<GradeRecovery>();

            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
            var substituteExams = await _context.SubstituteExams.Where(x => (x.SectionId == exam.SectionId || x.CourseTermId == courseTerm.Id) && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED).ToListAsync();

            if (model.Students != null)
            {
                foreach (var student in model.Students)
                {
                    var studentSection = studentSections.Where(x => x.StudentId == student).FirstOrDefault();

                    if (studentSection.FinalGrade < minGrade)
                        return BadRequest($"El alumno {studentSection.FullName} no cumple con la nota mínima para poder dar el examen. Promedio Actual : {studentSection.FinalGrade}.");

                    if (substituteExams.Any(x => x.StudentId == studentSection.StudentId))
                        return BadRequest($"El alumno {studentSection.FullName} tiene un examen sustitutorio pendiente.");

                    if (!enabledToApproved)
                    {
                        if (studentSection.FinalGrade >= term.MinGrade)
                            return BadRequest($"El alumno {studentSection.FullName} se encuentra con promedio aprobatorio.");
                    }


                    foreach (var eva in evaluations)
                    {
                        if (!studentSection.Grades.Any(y => y.EvaluationId == eva.Id))
                            return BadRequest($"El alumno {studentSection.FullName} tiene pendiente la evaluación {eva.Name}");
                    }

                    var entity = new GradeRecovery
                    {
                        StudentId = student,
                        GradeRecoveryExamId = model.GradeRecoveryExamId,
                        CourseTermId = section.CourseTermId
                    };

                    toAssign.Add(entity);
                }
            }

            await _gradeRecoveryService.DeleteRange(studentsAssigned);
            await _gradeRecoveryService.InsertRange(toAssign);

            return Ok();
        }
    }
}
