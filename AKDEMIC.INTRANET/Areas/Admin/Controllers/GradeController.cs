using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Areas.Admin.Models.GradeViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/notas")]
    public class GradeController : BaseController
    {
        private readonly ICourseTermService _courseTermService;
        private readonly IDataTablesService _dataTablesService;
        private readonly ICourseService _courseService;
        private readonly ITermService _termService;
        private readonly IEvaluationService _evaluationService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly ISectionService _sectionService;

        public GradeController(AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            ICourseTermService courseTermService,
            IDataTablesService dataTablesService,
            ICourseService courseService,
            ITermService termService,
            IEvaluationService evaluationService,
            IStudentSectionService studentSectionService,
            ISectionService sectionService
            ) : base(context, userManager)
        {
            _courseTermService = courseTermService;
            _dataTablesService = dataTablesService;
            _courseService = courseService;
            _termService = termService;
            _evaluationService = evaluationService;
            _studentSectionService = studentSectionService;
            _sectionService = sectionService;
        }

        /// <summary>
        /// Vista donde se muestra el listado de estudiantes
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de alumnos según los filtros del usuario
        /// </summary>
        /// <param name="term">Identificador del periodo académico</param>
        /// <param name="faculty">Idedntificador de la facultad</param>
        /// <param name="career">Identificador de la escuela profesional</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de alumnos</returns>
        [HttpGet("alumnos/get")]
        public async Task<IActionResult> GetStudents(Guid term, Guid faculty, Guid career, string search)
        {
            //var term = await GetActiveTerm();

            var paginationParameter = GetDataTablePaginationParameter();

            var query = _context.Students
                .AsQueryable();

            if (term != Guid.Empty)
            {
                query = query.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == term));
            }

            if (faculty != Guid.Empty)
            {
                query = query.Where(x => x.Career.FacultyId == faculty);
            }

            if (career != Guid.Empty)
            {
                query = query.Where(x => x.CareerId == career);
            }

            //switch (paginationParameter.SortField)
            //{
            //    case "0":
            //        query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Name) : query.OrderBy(q => q.Name);
            //        break;
            //    //case "1":
            //    //    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ExpeditionDate) : query.OrderBy(q => q.ExpeditionDate);
            //    //    break;
            //    default:
            //        query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Name) : query.OrderBy(q => q.Name);
            //        break;
            //}

            var pagedList = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage)
                   .Select(s => new
                   {
                       code = s.User.UserName,
                       name = s.User.FullName,
                       career = s.Career.Name,
                       faculty = s.Career.Faculty.Name,
                       academicYear = s.CurrentAcademicYear,
                       //credits = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == term.Id).Sum(ss => ss.Section.CourseTerm.Credits),
                       id = s.Id
                   }).ToListAsync();

            var filterRecords = await _context.Students.CountAsync();
            var result = GetDataTablePaginationObject(filterRecords, pagedList);

            return Ok(result);
        }

        /// <summary>
        /// Vista detalle del alumno
        /// </summary>
        /// <param name="id">Identificador del alumno</param>
        /// <returns>Vistaa detalle del alumno</returns>
        [HttpGet("detalle/{id}")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var model = await _context.Students
                .Where(x => x.Id == id)
                .Select(x => new DetailViewModel
                {
                    Id = x.Id,
                    FullName = x.User.FullName,
                    UserName = x.User.UserName,
                    Career = x.Career.Name

                    //ActiveTerm = _context.Terms.First(x => x.Status.Equals(ConstantHelpers.TERM_STATES.ACTIVE)).Id,
                    //Terms = await _context.Terms.Select(x => new TermViewModel()
                    //{
                    //    Name = x.Name,
                    //    MinimumValue = x.MinGrade,
                    //    Id = x.Id
                    //}).ToListAsync()
                }).FirstOrDefaultAsync();

            model.Terms = await _context.Terms
               .Select(x => new SelectListItem
               {
                   Value = x.Id.ToString(),
                   Text = x.Name
               }).OrderByDescending(x => x.Text).ToListAsync();

            var active = _context.Terms.FirstOrDefault(x => x.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.ACTIVE);
            model.ActiveTerm = active.Id;
            return View(model);
        }

        /// <summary>
        /// Obtiene la vista parcial donde se detalla los cursos matriculados del alumno
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("detalle/{studentId}/periodo/{termId}/get")]
        public async Task<IActionResult> GetStudentGrades(Guid studentId, Guid termId)
        {
            if (termId == Guid.Empty)
                return BadRequest($"No se pudo encontrar un Periodo Académico con el id {termId}.");

            var term = await _context.Terms.FindAsync(termId);
            var student = await _context.Students.FindAsync(studentId);

            if (term == null)
            {
                return BadRequest($"No se pudo encontrar un Periodo Académico con el id {term}.");
            }

            if (!_context.StudentSections.Any(x => x.StudentId.Equals(student.Id) && x.Section.CourseTerm.TermId.Equals(term.Id)))
            {
                return PartialView("_ErrorMessage", "No se encuentra matriculado en el periodo seleccionado.");
            }

            var enrollmentModel = await _context.StudentSections
                .Where(x => x.StudentId.Equals(student.Id) && x.Section.CourseTerm.TermId.Equals(term.Id))
                .Select(x => new EnrolledCourseViewModel()
                {
                    CurriculumAcademicYear = _context.AcademicYearCourses.First(ac => ac.CourseId.Equals(x.Section.CourseTerm.CourseId) && ac.Curriculum.CareerId.Equals(student.CareerId) && ac.Curriculum.IsActive).AcademicYear,
                    StudentSection = new StudentSectionViewModel()
                    {
                        Try = x.Try,
                        Observations = x.Observations,
                        Status = x.Status,
                        MinGradeTerm = term.MinGrade,
                        FinalGrade = x.FinalGrade,
                        Section = new SectionViewModel()
                        {
                            Code = x.Section.Code,
                            CourseTerm = new CourseTermViewModel()
                            {
                                TemaryUrl = x.Section.CourseTerm.Temary,
                                Credits = x.Section.CourseTerm.Course.Credits,
                                Course = new CourseViewModel()
                                {
                                    FullName = x.Section.CourseTerm.Course.FullName
                                }
                            },
                            Grades = x.Grades.Select(g => new GradeViewModel()
                            {
                                Value = g.Value,
                                Attended = g.Attended,
                                Approved = g.Attended && g.Value >= term.MinGrade,
                                IsDefaultValue = false,
                                Evaluation = new EvaluationViewModel()
                                {
                                    Name = g.Evaluation.Name,
                                    Percentage = g.Evaluation.Percentage
                                }
                            }).Concat(x.Section.CourseTerm.Evaluations
                            .Where(e => !x.Grades.Any(g => g.EvaluationId.Equals(e.Id)))
                            .Select(g => new GradeViewModel()
                            {
                                Value = 0.00M,
                                Attended = false,
                                Approved = false,
                                IsDefaultValue = true,
                                Evaluation = new EvaluationViewModel()
                                {
                                    Name = g.Name,
                                    Percentage = g.Percentage
                                }
                            })).OrderBy(g => g.Evaluation.Percentage).ToList()
                        }
                    }
                }).ToListAsync();

            var model = new AcademicHistoryViewModel()
            {
                Term = new TermViewModel()
                {
                    Name = term.Name,
                    MinimumValue = term.MinGrade
                },
                EnrolledCourses = enrollmentModel
            };

            return PartialView("_AcademicHistory", model);
        }

        #region Delete Grades

        /// <summary>
        /// Vista donde se listan los cursos
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("limpiarnotas")]
        public IActionResult DeleteGrades()
            => View();

        /// <summary>
        /// Obtiene el listado de cursos según los filtros del usuario
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="academicProgramId">Identificador del programa académico</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="academicYear">Ciclo</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de cursos</returns>
        [HttpGet("limpiarnotas/get-cursos")]
        public async Task<IActionResult> GetCoursesDatatable(Guid? termId, Guid? careerId, Guid? academicProgramId, Guid? curriculumId, int? academicYear, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            if (!termId.HasValue)
            {
                var activeTerm = await _termService.GetActiveTerm();
                termId = activeTerm.Id;
            }

            var result = await _courseService.GetAllAsModelB(parameters, termId.Value, areaCareerId: careerId, planId: curriculumId, programId: academicProgramId, cycle: academicYear, search: search, withSections: true);
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se listan las secciones aperturadas
        /// </summary>
        /// <param name="courseId">Identificador del curso</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Vista</returns>
        [HttpGet("limpiarnotas/curso/{courseId}/periodo/{termId}")]
        public async Task<IActionResult> CourseTerm(Guid courseId, Guid termId)
        {
            var model = await _context.CourseTerms.Where(x => x.CourseId == courseId && x.TermId == termId)
                .Select(x => new CourseTermViewModelV2
                {
                    Term = x.Term.Name,
                    CourseTermId = x.Id,
                    Course = $"{x.Course.Code}-{x.Course.Name}",
                    Curriculum = x.Course.AcademicYearCourses.Select(y => y.Curriculum.Name).FirstOrDefault(),
                    Career = x.Course.Career.Name,
                })
                .FirstOrDefaultAsync();

            return View(model);
        }

        /// <summary>
        /// Obtiene las secciones aperturadas
        /// </summary>
        /// <param name="courseTermId">Identificador del curso-periodo</param>
        /// <returns>Objeto que contiene la lista de las secciones</returns>
        [HttpGet("limpiarnotas/curso-periodo/{courseTermId}/get-secciones")]
        public async Task<IActionResult> GetCourseTermSections(Guid courseTermId)
        {
            var sections = await _sectionService.GetSectionsByCourseTermIdSelect2ClientSide(courseTermId);
            return Ok(sections);
        }

        /// <summary>
        /// Obtiene la vista parcial donde se listan las evaluaciones junto a las notas de cada alumno
        /// </summary>
        /// <param name="sectionId">Idedntificador de la sección</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("get-evaluaciones-notas/{sectionId}")]
        public async Task<IActionResult> CourseTermEvaluationsPartialView(Guid sectionId)
        {
            var model = await _studentSectionService.GetSectionPartialGradesTemplate(sectionId);
            return PartialView(model);
        }

        /// <summary>
        /// Método para mover las notas oficiales a las notas temporales de la evaluación seleccionada
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <param name="evaluationId">Identificador de la evaluación</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("limpiarnotas/seccion/{sectionId}/evaluacion/{evaluationId}")]
        public async Task<IActionResult> DeleteGrades(Guid sectionId, Guid evaluationId)
        {
            var section = await _context.Sections.Where(x => x.Id == sectionId).FirstOrDefaultAsync();
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Id == courseTerm.TermId).FirstOrDefaultAsync();

            if (term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                return BadRequest("Solo se pueden eliminar notas de periodos activos.");

            var studentSections = await _context.StudentSections.Where(x => x.SectionId == sectionId).Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).ToListAsync();
            var studentsId = studentSections.Select(x => x.StudentId).ToList();
            var grades = await _context.Grades.Where(x => x.StudentSection.SectionId == sectionId && x.EvaluationId == evaluationId).ToListAsync();
            var gradeRegistration = await _context.GradeRegistrations.Where(x => x.SectionId == sectionId && x.EvaluationId == evaluationId).FirstOrDefaultAsync();
            var academicHistories = await _context.AcademicHistories.Where(x => studentsId.Contains(x.StudentId) && x.SectionId == section.Id && x.TermId == courseTerm.TermId && x.Type == ConstantHelpers.AcademicHistory.Types.REGULAR).ToListAsync();
            var hasMeritOrder = await _context.AcademicSummaries.Where(x => studentsId.Contains(x.StudentId) && x.TermId == courseTerm.TermId).AnyAsync(y => y.MeritOrder > 0);

            var gradesId = grades.Select(x => x.Id).ToList();
            var gradeCorrections = await _context.GradeCorrections.Where(x => gradesId.Contains(x.GradeId)).ToListAsync();

            var gradeRecoveryExams = await _context.GradeRecoveries.Where(x => x.GradeId.HasValue && gradesId.Contains(x.GradeId.Value)).ToListAsync();
            if (gradeRecoveryExams.Any())
                return BadRequest("Se encontraron examanes de recuperacion de nota.");

            if (hasMeritOrder)
                return BadRequest("Se encontraron alumnos con cuadro de mérito.");

            if (!grades.Any())
                return BadRequest("No se encontraron notas.");

            var temporalGrades = new List<TemporalGrade>();

            foreach (var grade in grades)
            {
                var temporalGrade = new TemporalGrade
                {
                    Attended = grade.Attended,
                    CreatorIP = grade.CreatorIP,
                    EvaluationId = grade.EvaluationId.Value,
                    StudentSectionId = grade.StudentSectionId,
                    Value = grade.Value
                };

                temporalGrades.Add(temporalGrade);
            }

            if (gradeRegistration != null)
            {
                gradeRegistration.WasLate = false;
                gradeRegistration.WasPublished = false;
            }

            if (academicHistories.Any())
                _context.AcademicHistories.RemoveRange(academicHistories);

            _context.GradeCorrections.RemoveRange(gradeCorrections);
            await _context.TemporalGrades.AddRangeAsync(temporalGrades);
            var studenSectionsToUpdate = studentSections.Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI).ToList();
            studenSectionsToUpdate.ForEach(x => x.Status = ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS);
            _context.Grades.RemoveRange(grades);
            await _context.SaveChangesAsync();
            await _studentSectionService.RecalculateFinalGradeBySectionId(section.Id);

            var rpta = temporalGrades.Count == 1 ? $"Se agregó 1 nota temporal." : $"Se agregaron {temporalGrades.Count} notas temporales.";
            return Ok(rpta);
        }

        #endregion

        #region Delete Grades By Section and Student

        [HttpGet("borrar-notas")]
        public IActionResult DeleteGradesByStudent()
            => View();

        [HttpGet("get-notas/estudiante/{username}/seccion/{sectionId}")]
        public async Task<IActionResult> GradesByStudentPartialView(Guid sectionId, string username)
        {
            var student = await _context.Students.Where(x => x.User.UserName.Trim().ToLower() == username.Trim().ToLower()).FirstOrDefaultAsync();
            var section = await _context.Sections.Where(x => x.Id == sectionId).FirstOrDefaultAsync();

            if (student is null)
                return BadRequest("No se encontró al estudiante.");

            if (section is null)
                return BadRequest("No se econtró la sección.");

            var model = await _studentSectionService.GetSectionPartialGradesTemplate(section.Id, student.Id);
            return PartialView(model);
        }

        [HttpPost("borrar-notas")]
        public async Task<IActionResult> DeleteGradesByStudent(DeleteGradesByStudent model)
        {
            var student = await _context.Students.Where(x => x.Id == model.StudentId).FirstOrDefaultAsync();
            var section = await _context.Sections.Where(x => x.Id == model.SectionId).FirstOrDefaultAsync();

            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Id == courseTerm.TermId).FirstOrDefaultAsync();

            if (term.Status != ConstantHelpers.TERM_STATES.FINISHED)
                return BadRequest("El periodo no se encuentra finalizado.");

            if (student is null)
                return BadRequest("No se encontró al estudiante.");

            if (section is null)
                return BadRequest("No se econtró la sección.");

            var studenSection = await _context.StudentSections.Where(x => x.StudentId == student.Id && x.SectionId == section.Id).FirstOrDefaultAsync();

            if (studenSection is null)
                return BadRequest("No se econtró la entidad estudiante-sección.");

            if (studenSection.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                return BadRequest("El alumno tiene estado retirado.");

            var grades = await _context.Grades.Where(x => x.StudentSectionId == studenSection.Id).ToListAsync();

            var gradesId = grades.Select(x => x.Id).ToList();
            var gradeCorrections = await _context.GradeCorrections.Where(x => gradesId.Contains(x.GradeId)).ToListAsync();

            var gradeRecoveryExams = await _context.GradeRecoveries.Where(x => x.GradeId.HasValue && gradesId.Contains(x.GradeId.Value)).ToListAsync();
            if (gradeRecoveryExams.Any())
                return BadRequest("Se encontraron examanes de recuperación de nota.");


            if (!grades.Any())
                return BadRequest("No se econtraron notas.");

            var hasMeritOrder = await _context.AcademicSummaries.Where(x => x.StudentId == student.Id && x.TermId == courseTerm.TermId).AnyAsync(y => y.MeritOrder > 0);

            if (hasMeritOrder)
                return BadRequest("Se encontraron alumnos con cuadro de mérito.");

            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == student.Id && x.CourseId == courseTerm.CourseId && x.TermId == courseTerm.TermId).ToListAsync();

            var temporalGrades = new List<TemporalGrade>();

            foreach (var grade in grades)
            {
                var temporalGrade = new TemporalGrade
                {
                    Attended = grade.Attended,
                    CreatorIP = grade.CreatorIP,
                    EvaluationId = grade.EvaluationId.Value,
                    StudentSectionId = grade.StudentSectionId,
                    Value = grade.Value
                };

                temporalGrades.Add(temporalGrade);
            }

            if (academicHistories.Any())
                _context.AcademicHistories.RemoveRange(academicHistories);

            _context.GradeCorrections.RemoveRange(gradeCorrections);
            _context.Grades.RemoveRange(grades);
            await _context.TemporalGrades.AddRangeAsync(temporalGrades);
            if (studenSection.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI && studenSection.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                studenSection.Status = ConstantHelpers.STUDENTSECTION_STATES.IN_PROCESS;

            await _context.SaveChangesAsync();
            await _studentSectionService.RecalculateFinalGrade(studenSection.Id);
            return Ok();
        }

        #endregion

    }
}
