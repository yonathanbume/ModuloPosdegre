using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.SubstituteExamViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.ACADEMIC_SECRETARY + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/examen-sustitutorio")]
    public class SubstituteExamController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IStudentService _studentService;
        private readonly AkdemicContext _context;
        private readonly ICareerService _careerService;
        private readonly ISectionService _sectionService;
        private readonly ICourseTermService _courseTermService;
        private readonly ICourseService _courseService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly ISubstituteExamDetailService _substituteExamDetailService;
        private readonly IConfigurationService _configurationService;
        private readonly ISubstituteExamService _substituteExamService;
        private readonly ICurriculumService _curriculumService;
        private IWebHostEnvironment _hostingEnvironment;
        public IConverter _dinkConverter { get; }
        private readonly IViewRenderService _viewRenderService;
        public SubstituteExamController(
             IConverter dinkConverter,
            IViewRenderService viewRenderService,
            IWebHostEnvironment environment,
            ICourseTermService courseTermService,
            ICourseService courseService,
            IStudentSectionService studentSectionService,
            IAcademicSummariesService academicSummariesService,
            ISubstituteExamDetailService substituteExamDetailService,
            ITermService termService,
            IConfigurationService configurationService,
            ICareerService careerService,
            ISectionService sectionService,
            ICurriculumService curriculumService,
            IStudentService studentService,
            AkdemicContext context,
            ISubstituteExamService substituteExamService,
            IDataTablesService dataTablesService
            ) : base(termService)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
            _curriculumService = curriculumService;
            _dataTablesService = dataTablesService;
            _studentService = studentService;
            _context = context;
            _courseTermService = courseTermService;
            _courseService = courseService;
            _studentSectionService = studentSectionService;
            _academicSummariesService = academicSummariesService;
            _substituteExamDetailService = substituteExamDetailService;
            _configurationService = configurationService;
            _sectionService = sectionService;
            _careerService = careerService;
            _substituteExamService = substituteExamService;
        }

        /// <summary>
        /// Vista donde se gestionan los exámenes sustitutorios
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de exámenes sustitutorios para ser usado en tablas
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="cycleId">Ciclo académico</param>
        /// <param name="courseId">Identificador del curso</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de exámenes sustitutorios</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetSectionsDataTable(Guid? termId, Guid? careerId, Guid? curriculumId, int cycleId, Guid? courseId, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            if (!termId.HasValue)
            {
                var term = await _termService.GetActiveTerm();
                termId = term.Id;
            }


            if (User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var result2 = await _sectionService.GetSectionsByTermCareerCurriculumCycleCourseDataTable(parameters, termId.Value, careerId, curriculumId, cycleId, courseId, User, search);
                return Ok(result2);
            }
            var result = await _sectionService.GetSectionsByTermCareerCurriculumCycleCourseDataTable(parameters, termId.Value, careerId, curriculumId, cycleId, courseId, search: search);
            return Ok(result);
        }

        /// <summary>
        /// Genera el reporte donde se listan las secciones con examen sustitutorio
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="cycleId">Ciclo académico</param>
        /// <param name="courseId">Identificador del curso</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("get/pdf")]
        public async Task<IActionResult> GetSectionsPdf(Guid? careerId, Guid? curriculumId, int cycleId, Guid? courseId, string search)
        {
            var term = await _termService.GetActiveTerm();
            if (term == null)
                term = await _termService.GetLastTerm();
            if (term == null)
                term = new ENTITIES.Models.Enrollment.Term();

            var data = new List<SectionDataPdfViewModel>();
            if (User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var result2 = await _sectionService.GetSectionsByTermCareerCurriculumCycleCourseData(term.Id, careerId, curriculumId, cycleId, courseId, User, search);
                data = result2.Select(s => new SectionDataPdfViewModel
                {
                    id = s.id,
                    courseTermId = s.courseTermId,
                    code = s.code,
                    career = s.career,
                    name = s.name,
                    academicYear = s.academicYear,
                    modality = s.modality,
                    teachers = s.teachers,
                    group = s.group,
                    groupcycle = s.groupcycle,
                    EnrolledStudents = s.EnrolledStudents,
                    StudentsThatFit = s.StudentsThatFit,
                }).ToList();
            }
            else
            {
                var result = await _sectionService.GetSectionsByTermCareerCurriculumCycleCourseData(term.Id, careerId, curriculumId, cycleId, courseId, search: search);
                data = result.Select(s => new SectionDataPdfViewModel
                {
                    id = s.id,
                    courseTermId = s.courseTermId,
                    code = s.code,
                    career = s.career,
                    name = s.name,
                    academicYear = s.academicYear,
                    modality = s.modality,
                    teachers = s.teachers,
                    group = s.group,
                    groupcycle = s.groupcycle,
                    EnrolledStudents = s.EnrolledStudents,
                    StudentsThatFit = s.StudentsThatFit,
                }).ToList();
            }
            var career = "Todas";
            if (careerId.HasValue && careerId != Guid.Empty)
            {
                var qcareer = await _careerService.Get(careerId.Value);
                career = qcareer.Name;
            }
            var course = "Todos";
            if (courseId.HasValue && courseId != Guid.Empty)
            {
                var qcourse = await _courseService.GetAsync(courseId.Value);
                course = qcourse.Name;
            }

            var curriculum = "Todos";
            if (curriculumId.HasValue && curriculumId != Guid.Empty)
            {
                var qcurriculum = await _curriculumService.Get(curriculumId.Value);
                curriculum = qcurriculum.Name;
            }
            var cycle = "Todos";
            if (cycleId != 0)
            {
                cycle = ConstantHelpers.ACADEMIC_YEAR.TEXT[cycleId];
            }
            var model = new SectionsPdfViewModel
            {
                Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images/themes/" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png"),
                Career = career,
                Course = course,
                Curriculum = curriculum,
                Cycle = cycle,
                Rows = data
            };
            DinkToPdf.GlobalSettings globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = $"Secciones Examen sustitutorio"
            };

            string viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/SubstituteExam/SectionsPdf.cshtml", model);

            DinkToPdf.ObjectSettings objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            DinkToPdf.HtmlToPdfDocument pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            byte[] fileByte = _dinkConverter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf", $"Secciones Examen sustitutorio.pdf");
        }

        /// <summary>
        /// Método para crear un examen sustitutorio
        /// </summary>
        /// <param name="substituteExam">Objeto que contiene los datos del examen sustitutorio</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("nuevo")]
        public async Task<IActionResult> PostNewSubstituteExam(NewSubstituteExamViewModel substituteExam)
        {
            var section = await _context.Sections.Where(x => x.Id == substituteExam.SectionId).FirstOrDefaultAsync();

            if (section == null) return BadRequest("No existe la seccion");

            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Id == courseTerm.TermId).FirstOrDefaultAsync();

            if (term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                return BadRequest("Solo se pueden crear examenes sustitutorios para el periodo académico activo.");

            var evaluations = await _context.Evaluations.Where(x => x.CourseTermId == section.CourseTermId).ToListAsync();
            foreach (var item in evaluations)
            {
                if (!await _context.Grades.AnyAsync(x => x.StudentSection.SectionId == section.Id && x.EvaluationId == item.Id))
                    return BadRequest($"La evaluación '{item.Name}' no tiene notas registradas.");
            }

            var exist = await _substituteExamService.AnyBySectionId(substituteExam.SectionId);

            if (exist)
            {
                return BadRequest("Ya existe un examen sustitutorio para esta sección");
            }

            var startDate = DateTime.UtcNow.ToDefaultTimeZone().Date;
            var startTime = DateTime.UtcNow.ToDefaultTimeZone();

            if (!string.IsNullOrEmpty(substituteExam.StartDate))
                startDate = ConvertHelpers.DatepickerToDatetime(substituteExam.StartDate);

            if (!string.IsNullOrEmpty(substituteExam.StartTime))
                startTime = ConvertHelpers.TimepickerToDateTime(substituteExam.StartTime);

            var startDateTime = startDate.Date.Add(startTime.TimeOfDay);
            var startDateTimeUTC = startDateTime.ToUtcDateTime();

            //if (startDateTimeUTC < DateTime.UtcNow)
            //    return BadRequest("La fecha y hora del examen debe ser mayor a la fecha y hora actual.");

            if (substituteExam.Duration <= 0)
                substituteExam.Duration = 60;

            var endDateTimeUTC = startDateTimeUTC.AddMinutes(substituteExam.Duration);

            if (substituteExam.ClassroomId == Guid.Empty)
            {
                var classrom = await _context.Classrooms.Where(x => x.Description == "Sin Asignar").FirstOrDefaultAsync();
                substituteExam.ClassroomId = classrom.Id;
            }

            var result = await _studentService.GetStudentsForSubstituteExamDataAsync(term.Id, section.Id, "");
            if (result.Count != 0)
            {
                var list = new List<SubstituteExam>();
                foreach (var item in result)
                {
                    var newsubstitueExam = new SubstituteExam
                    {
                        ExamScore = Convert.ToInt32(item.score),
                        SectionId = substituteExam.SectionId,
                        CourseTermId = section.CourseTermId,
                        StudentId = item.id,
                        Underpin = "",
                        PaymentReceipt = "",
                        Status = ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED
                    };
                    list.Add(newsubstitueExam);
                }

                var exam = new SubstituteExamDetail
                {
                    ClassroomId = substituteExam.ClassroomId,
                    StartTime = startDateTimeUTC,
                    EndTime = endDateTimeUTC,
                    SubstituteExams = list,
                };

                await _substituteExamDetailService.Insert(exam);
                return Ok("Se ha creado el exámen sustitutorio.");
            }
            return BadRequest("No hay estudiantes para el exámen sustitutorio.");
        }

        /// <summary>
        /// Vista donde se gestionan los alumnos del examen sustitutorio
        /// </summary>
        /// <param name="id">Identificador del examen sustitutorio</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("estudiantes/{id}")]
        public async Task<IActionResult> Students(Guid id)
        {
            var newmodel = new SectionSubstituteExamViewModel();
            var model = await _sectionService.GetSubstituteSectionById(id);
            newmodel.academicYear = model.academicYear;
            newmodel.career = model.career;
            newmodel.code = model.code;
            newmodel.courseTermId = model.courseTermId;
            newmodel.group = model.group;
            newmodel.groupcycle = model.groupcycle;
            newmodel.id = model.id;
            newmodel.modality = model.modality;
            newmodel.name = model.name;
            newmodel.teachers = model.teachers;

            return View(newmodel);
        }

        /// <summary>
        /// Obtiene el listado de estudiantes matriculados en la sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de estudiantes</returns>
        [HttpGet("estudiantes/{sectionId}/getstudents")]
        public async Task<IActionResult> GetStudentsForSubstiteExam(Guid sectionId, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var term = await _termService.GetActiveTerm();
            var result = await _studentService.GetStudentsForSubstiteExam(parameters, term.Id, sectionId, search);
            return Ok(result);
        }

        /// <summary>
        /// Genera el reporte donde se listan los alumnos inscritos en el examen sustitutorio
        /// </summary>
        /// <param name="id">Identificador del examen sustitutorio</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("estudiantes/{id}/pdf")]
        public async Task<IActionResult> DownloadPdf(Guid id)
        {
            var term = await _termService.GetActiveTerm();

            var result = await _context.SubstituteExams.Where(x => x.SectionId == id).Where(x => x.Status != ConstantHelpers.SUBSTITUTE_EXAM_STATUS.DELETED)
                .Select(x => new SubstituteExamPdfViewModel
                {
                    Code = x.Student.User.UserName,
                    UserName = x.Student.User.FullName,
                    Grade = x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED ? x.TeacherExamScore.HasValue ? x.TeacherExamScore.ToString() : x.ExamScore.ToString() : "Sin Evaluar"
                })
                .ToListAsync();

            var section = await _sectionService.GetSubstituteSectionById(id);

            var model = new SectionSubstituteExamPdfViewModel
            {
                Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images/themes/" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png"),
                Term = term.Name ?? "",
                Course = section.name,
                Group = section.group,
                Career = section.career,
                Rows = result
            };
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = $"Alumnos Examen sustitutorio-{section.code}"
            };

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/SubstituteExam/StudentsPdf.cshtml", model);

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileByte = _dinkConverter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf", $"Estudiantes Examen sustitutorio-{section.code} .pdf");

        }

        /// <summary>
        /// Método para asignar estudiantes al examen sustitutorio
        /// </summary>
        /// <param name="Student">Objeto que contiene los datos de los alumnos</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("estudiantes/guardartodos/v3")]
        public async Task<IActionResult> PostStudentsForSubstituteExamAll(SubstituteExamViewModel Student)
        {
            var term = await _termService.GetActiveTerm();
            await _substituteExamService.SaveStudentsForSubstituteExam(term.Id, Student.SectionId, Student.IsCheckAll, Student.LstToAdd, Student.LstToAvoid);

            return Ok("La solicitud ha sido generada satisfactoriamente.");
        }

        /// <summary>
        /// Método para crear el examen sustitutorio del estudiante
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del estudiante</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("actualizar-alumno-examen")]
        public async Task<IActionResult> UpdateSubstitueExamStudent(SubstituteExamStudentViewModel model)
        {
            var exam = await _context.SubstituteExams.Where(x => x.SectionId == model.SectionId && x.StudentId == model.StudentId).FirstOrDefaultAsync();

            if (exam == null)
            {
                var section = await _context.Sections.Where(x => x.Id == model.SectionId).FirstOrDefaultAsync();
                var studentSection = await _context.StudentSections.Where(x => x.SectionId == model.SectionId && x.StudentId == model.StudentId).FirstOrDefaultAsync();
                exam = new SubstituteExam
                {
                    StudentId = model.StudentId,
                    SectionId = model.SectionId,
                    CourseTermId = section.CourseTermId,
                    ExamScore = studentSection.FinalGrade,
                    Status = model.Checked ? ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED : ConstantHelpers.SUBSTITUTE_EXAM_STATUS.DELETED
                };

                await _context.SubstituteExams.AddAsync(exam);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                if (exam.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED)
                    return BadRequest("El examen del estudiante ya ha sido evaluado.");

                exam.Status = model.Checked ? ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED : ConstantHelpers.SUBSTITUTE_EXAM_STATUS.DELETED;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de planes de estudio
        /// </summary>
        /// <param name="id">Identificador de la escuela profesional</param>
        /// <returns>Listado dee planes de estudio</returns>
        [HttpGet("carreras/{id}/planestudio/get")]
        public async Task<IActionResult> GetCurriculumByCareer(Guid id)
        {
            var substis = await _context.Sections
                .Where(x => x.SubstituteExams.Count() > 0 && x.CourseTerm.Course.CareerId == id)
                .Select(x => x.CourseTerm.CourseId)
                .ToListAsync();
            var cur = await _context.AcademicYearCourses.Where(x => substis.Contains(x.CourseId)).Select(x => x.Curriculum).ToListAsync();

            var result = cur
              .OrderByDescending(x => x.Year)
              .ThenByDescending(x => x.Code)
              .Select(ay => new
              {
                  id = ay.Id,
                  text = $"{ay.Year}-{ay.Code}"
              })
              .Distinct()
              .ToList();

            return Ok(new { items = result });
        }

        /// <summary>
        /// Método para actualizar los alumnos asignados al examen sustitutorio
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("actualizar-alumnos")]
        public async Task<IActionResult> UpdateSubstituteExams(Guid sectionId)
        {
            var section = await _sectionService.Get(sectionId);
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
            var examns = await _context.SubstituteExams.Where(x => x.SectionId == sectionId).ToListAsync();

            var studentsAssigned = await _context.SubstituteExams.Where(x => x.SectionId == sectionId).ToListAsync();

            var result = await _studentService.GetStudentsForSubstituteExamDataAsync(courseTerm.TermId, sectionId, "");

            var list = new List<SubstituteExam>();

            foreach (var item in result)
            {
                if (!studentsAssigned.Any(y => y.StudentId == item.id))
                {
                    var newsubstitueExam = new SubstituteExam
                    {
                        ExamScore = Convert.ToInt32(item.score),
                        SectionId = sectionId,
                        CourseTermId = section.CourseTermId,
                        StudentId = item.id,
                        Underpin = "",
                        PaymentReceipt = "",
                        Status = ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED
                    };

                    list.Add(newsubstitueExam);
                }
            }

            var studentsAssignedToDeleted = studentsAssigned.Where(x => x.Status != ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED && !result.Any(y => y.StudentId == x.StudentId)).ToList();

            _context.SubstituteExams.RemoveRange(studentsAssignedToDeleted);
            await _context.SubstituteExams.AddRangeAsync(list);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Método para calificar el examen sustitutorio de un alumno
        /// </summary>
        /// <param name="model">Objeto que contiene la nota del alumno</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("asignar-nota")]
        public async Task<IActionResult> AssignScore(AssignScoreViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var substitute_exam_evaluation_type = int.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAMEN_EVALUATION_TYPE));

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNTUMBES)
            {
                if (model.Score > 11)
                {
                    return BadRequest("La nota no puede ser mayor a 11.");
                }
            }

            var exam = await _substituteExamService.GetAsync(model.Id);
            var courseTerm = await _courseTermService.GetAsync(exam.CourseTermId);
            var term = await _termService.Get(courseTerm.TermId);

            if (term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                return BadRequest("Solo se puede calificar examenes del periodo académico activo.");

            if (exam.Status != ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED)
                return BadRequest("El estudiante no ha sido registrado para el examen.");

            exam.Status = ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED;

            var studentSection = await _studentSectionService.GetByStudentAndCourseTerm(exam.StudentId, exam.Section.CourseTermId);
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
        /// Obtiene el valor de la configuración
        /// </summary>
        /// <param name="key">Identificador de la configuración</param>
        /// <returns>Valor de la configuración</returns>
        private async Task<string> GetConfigurationValue(string key)
        {
            var values = await _configurationService.GetDataDictionary();
            return values.ContainsKey(key) ? values[key] :

                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES.ContainsKey(key) ?
                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[key] : "";
        }
    }
}
