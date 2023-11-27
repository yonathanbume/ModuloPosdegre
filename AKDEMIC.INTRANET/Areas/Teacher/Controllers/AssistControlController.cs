using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.Intranet;
//using AKDEMIC.INTRANET.Areas.Admin.Models.ClassroomScheduleViewModels;
using AKDEMIC.INTRANET.Areas.Teacher.Models.StudentAssistanceViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/reporte_asistencia")]
    public class AssistControlController : BaseController
    {
        private readonly ISectionService _sectionService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IClassService _classService;
        private readonly IClassStudentService _classStudentService;
        private readonly IStudentService _studentService;
        private readonly IWeeklyAttendanceReportService _weeklyAttendanceReportService;
        private readonly IEvaluationService _evaluationService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly AkdemicContext _context;
        private readonly IConfigurationService _configurationService;
        private readonly ICourseService _courseService;
        private readonly ICourseTermService _courseTermService;

        public AssistControlController(IUserService userService,
            ITermService termService,
            ISectionService sectionService,
            IStudentSectionService studentSectionService,
            IClassService classService,
            IStudentService studentService,
            IWeeklyAttendanceReportService weeklyAttendanceReportService,
            IEvaluationService evaluationService,
            IClassStudentService classStudentService,
            IWebHostEnvironment environment,
            AkdemicContext context,
            IConfigurationService configurationService,
            ICourseService courseService,
            ICourseTermService courseTermService) : base(userService, termService)
        {
            _sectionService = sectionService;
            _studentSectionService = studentSectionService;
            _classService = classService;
            _studentService = studentService;
            _weeklyAttendanceReportService = weeklyAttendanceReportService;
            _evaluationService = evaluationService;
            _classStudentService = classStudentService;
            _hostingEnvironment = environment;
            _context = context;
            _configurationService = configurationService;
            _courseService = courseService;
            _courseTermService = courseTermService;
        }

        /// <summary>
        /// Vista principal del controlador donde se muestra el listado de cursos
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            var userId = GetUserId();
            return View(model: userId);
        }

        /// <summary>
        /// Obtiene el listado de las secciones asignadas al docente logueado
        /// </summary>
        /// <returns>Listado de secciones</returns>
        [HttpGet("cursos")]
        public async Task<IActionResult> GetCourses(Guid? termId)
        {
            var userId = GetUserId();

            var query = _context.Sections.Where(x => x.CourseTerm.TermId == termId && x.TeacherSections.Any(y => y.TeacherId == userId))
                .AsNoTracking();

            var result = await query
                .Select(x => new
                {
                    sectionGroups = x.ClassSchedules.Any(y=>y.SectionGroupId.HasValue) ? x.ClassSchedules.Where(x=>x.SectionGroupId.HasValue).Select(y=>new { Id = y.SectionGroupId , y.SectionGroup.Code }).ToList() : null,
                    sectioncode = x.Code,
                    idsection = x.Id,
                    idteacher = x.TeacherSections.Select(ts => ts.TeacherId).ToList(),
                    code = x.CourseTerm.Course.Code,
                    name = x.CourseTerm.Course.Name
                })
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Vista detalle de una sección asignada al docente
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <returns>Vista detalle de la sección</returns>
        [HttpGet("secciones/detalles/{sid}")]
        public async Task<IActionResult> Detail(Guid sid)
        {
            var result = await _sectionService.Get(sid);
            return View(result.Id);
        }

        /// <summary>
        /// Método para obtener el listado de alumnos matriculados en una sección detallando clases asistidas, 
        /// clases ausentes, porcentaje de inasistencias.
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <returns>Listado de alumnos matriculados</returns>
        [HttpGet("alumnos/get/{sid}")]
        public async Task<IActionResult> GetStudentsDetail(Guid sid)
        {
            var userId = GetUserId();
            var result = await _classStudentService.GetClassStudentReport(sid);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de faltas y asistencias del alumno en una sección
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <returns>Listado de faltas y asistencias</returns>
        [HttpGet("alumnos/get-inasistencias/{sid}/{studentId}")]
        public async Task<IActionResult> GetStudentsAbsencesDetail(Guid sid, Guid studentId)
        {
            var userId = GetUserId();
            var studentSection = await _context.StudentSections.Where(x => x.StudentId == studentId && x.SectionId == sid).FirstOrDefaultAsync();
            var classStudents = await _context.ClassStudents
                .Where(x => x.StudentId == studentId && x.Class.SectionId == sid && (!x.Class.ClassSchedule.SectionGroupId.HasValue || x.Class.ClassSchedule.SectionGroupId == studentSection.SectionGroupId))
                .Select(x => new
                {
                    x.ClassId,
                    x.IsAbsent,
                    x.Class.ClassSchedule.SectionGroupId,
                    x.Class.StartTime
                })
                .ToListAsync();

            var classs = classStudents.Where(x => x.IsAbsent).Select(x => x.StartTime.ToLocalDateFormat()).ToList();

            return Ok(classs);
        }

        /// <summary>
        /// Vista donde se lista las clases pasadas sin toma de asistencias
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <returns>Vista</returns>
        [HttpGet("asistencias/{sid}")]
        public IActionResult Assistance(Guid sid)
        {
            return View(sid);
        }

        /// <summary>
        /// Vista donde se muestra el historial de asistencias de la sección
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <returns>Vista</returns>
        [HttpGet("historial/{sid}")]
        public IActionResult History(Guid sid)
        {
            return View(sid);
        }

        /// <summary>
        /// Obtiene el listado de clases pasadas que no hayan sido dictadas de una sección
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <param name="date">Fecha de clase</param>
        /// <returns>Listado de clases pasadas</returns>
        [HttpGet("asistencias/{sid}/get")]
        public async Task<IActionResult> GetClasses(Guid sid, string date)
        {
            var userId = GetUserId();

            var day = (DateTime?)null;
            if (!string.IsNullOrEmpty(date)) day = ConvertHelpers.DatepickerToDatetime(date);

            var classes = await _classService.GetOldClassesDatatableClientSide(sid, day, userId);
            return Ok(classes);
        }

        /// <summary>
        /// Obtiene el listado de clases dictadas de una sección
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <param name="date">Fecha de Clase</param>
        /// <returns>Listado de clases dictadas</returns>
        [HttpGet("historial/{sid}/get")]
        public async Task<IActionResult> GetHistory(Guid sid, string date)
        {
            var userId = GetUserId();

            var day = (DateTime?)null;
            if (!string.IsNullOrEmpty(date)) day = ConvertHelpers.DatepickerToDatetime(date);

            var classes = await _classService.GetHistoryClassesDatatableClientSide(sid, day, userId);
            return Ok(classes);
        }

        /// <summary>
        /// Vista donde se muestra las asistencias de una clase
        /// </summary>
        /// <param name="cid">Identificador de la clase</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("historial/detalle/{cid}")]
        public async Task<IActionResult> HistoryClassAssistance(Guid cid)
        {
            var c = await _classService.GetWithTeacherSchedules(cid);

            var enableUpdateClass = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.TeacherManagement.ENABLE_UPDATE_CLASS);

            var model = new IndexViewModel
            {
                EnableUpdateClass = Convert.ToBoolean(enableUpdateClass),
                Class = new ClassViewModel
                {
                    Id = c.Id,
                    WeekNumber = c.WeekNumber,
                    ClassNumber = c.ClassNumber,
                    ActivityId = c.UnitActivityId,
                    Date = c.StartTime.ToLocalDateFormat(),
                    EndTime = c.EndTime.ToLocalTimeFormat(),
                    StartTime = c.StartTime.ToLocalTimeFormat(),
                    VirtualClassId = c.VirtualClassId,
                    Commentary = c.Commentary,
                    Section = new SectionViewModel
                    {
                        Id = c.ClassSchedule.SectionId,
                        Code = c.ClassSchedule.Section.Code,
                        Course = new CourseViewModel
                        {
                            Id = c.ClassSchedule.Section.CourseTerm.CourseId,
                            Code = c.ClassSchedule.Section.CourseTerm.Course.Code,
                            Name = c.ClassSchedule.Section.CourseTerm.Course.Name
                        }
                    }
                }
            };
            return View(model);
        }

        [HttpPost("historial/actualizar-asistencia")]
        public async Task<IActionResult> UpdateHistoryClassAssistance(IndexViewModel model)
        {
            var enableUpdateClass = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.TeacherManagement.ENABLE_UPDATE_CLASS);

            if (!Convert.ToBoolean(enableUpdateClass))
                return BadRequest("No se encuentra habilitada la opción de actualizar clase.");

            var entity = await _classService.Get(model.Class.Id);

            if (entity == null)
                return BadRequest("No se encontró la clase seleccionada");

            entity.UnitActivityId = model.Class.ActivityId;
            entity.VirtualClassId = model.Class.VirtualClassId;

            await _classService.Update(entity);
            return Ok();
        }

        /// <summary>
        /// Vista para guardar la asistencia de clases pasadas
        /// </summary>
        /// <param name="cid">Identificador de la clase</param>
        /// <returns>Vista registro de asistencia</returns>
        [HttpGet("asistencias/tomar-asistencia/{cid}")]
        public async Task<IActionResult> ClassAssistance(Guid cid)
        {
            var c = await _classService.GetWithTeacherSchedules(cid);
            var courseTerm = await _courseTermService.GetAsync(c.Section.CourseTermId);
            var term = await _termService.Get(courseTerm.TermId);
            var evaluations = await _evaluationService.GetEvaluationsByClass(c.Id);

            var maxAbsencesPercentage = term.AbsencePercentage;

            bool.TryParse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.ENABLED_SPECIAL_ABSENCE_PERCENTAGE), out var enabledSpecialAbsencePercentage);

            if (enabledSpecialAbsencePercentage)
            {
                float.TryParse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE), out var specialAbsencePercentage);
                var absencePercentageDescription = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE_DESCRIPTION);
                var courseName = c.ClassSchedule.Section.CourseTerm.Course.Name;

                if (!string.IsNullOrEmpty(absencePercentageDescription) && courseName.ToLower().Trim().Contains(absencePercentageDescription.Trim().ToLower()))
                    maxAbsencesPercentage = specialAbsencePercentage;
            }

            var model = new IndexViewModel
            {
                Class = new ClassViewModel
                {
                    AbsencePercentage = maxAbsencesPercentage,
                    Id = c.Id,
                    WeekNumber = c.WeekNumber,
                    ClassNumber = c.ClassNumber,
                    ActivityId = c.UnitActivityId,
                    Date = c.StartTime.ToLocalDateFormat(),
                    EndTime = c.EndTime.ToLocalTimeFormat(),
                    StartTime = c.StartTime.ToLocalTimeFormat(),
                    Section = new SectionViewModel
                    {
                        Id = c.ClassSchedule.SectionId,
                        Code = c.ClassSchedule.Section.Code,
                        Course = new CourseViewModel
                        {
                            Id = c.ClassSchedule.Section.CourseTerm.CourseId,
                            Code = c.ClassSchedule.Section.CourseTerm.Course.Code,
                            Name = c.ClassSchedule.Section.CourseTerm.Course.Name
                        }
                    },
                    Evaluations = evaluations.Select(x => new EvaluationViewModel
                    {
                        EvaluationId = x.Id,
                        Description = x.Description,
                        Name = x.Name,
                        Percentage = x.Percentage,
                        Taken = c.EvaluationId == x.Id
                    })
                    .ToList()
                }
            };
            return View(model);
        }

        /// <summary>
        /// Obtiene la lista de alumnos y su asistencia o falta
        /// </summary>
        /// <param name="classId">Identificador de la clase</param>
        /// <returns>Listado de alumnos</returns>
        [HttpGet("asistencias/tomar-asistencia/get")]
        public async Task<IActionResult> GetStudentAssistance(Guid classId)
        {
            var userId = GetUserId();
            var result = await _classStudentService.GetClassStudentsOldAssistance(classId, userId);
            return Ok(result);
        }

        /// <summary>
        /// Método para registrar la asistencia de los alumnos de una sección en una clase
        /// </summary>
        /// <param name="classId">Identificador de la clase</param>
        /// <param name="assists">Objeto que contiene los datos de los alumnos</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("registrar/post")]
        public async Task<IActionResult> PostStudentAssistance(Guid classId, AssistanceViewModel assists)
        {
            if (classId.Equals(Guid.Empty)) return BadRequest($"No se pudo encontrar la clase con el Id {classId}.");

            if (assists.Evaluations != null && assists.Evaluations.Count(x => x.Taken) > 1)
            {
                return BadRequest("Solo es posible seleccionar una evaluación para asignar a la clase.");
            }


            var userId = GetUserId();
            var @class = await _classService.Get(classId);

            var classStudents = await _context.ClassStudents.Where(x => x.ClassId == @class.Id).ToListAsync();

            var section = await _sectionService.GetWithTeacherSections(@class.ClassSchedule.SectionId);

            if (section.TeacherSections.All(ts => ts.TeacherId != userId)) return BadRequest("No está autorizado para esta sección.");

            foreach (var a in assists.List.Where(x => !x.DPI))
            {

                if (classStudents.Any(y => y.StudentId == a.StudentId))
                {
                    var classStudent = classStudents.Where(y => y.StudentId == a.StudentId).FirstOrDefault();
                    classStudent.IsAbsent = a.IsAbsent;
                }
                else
                {
                    var classStudent = new ClassStudent
                    {
                        ClassId = classId,
                        IsAbsent = a.IsAbsent,
                        StudentId = a.StudentId
                    };

                    await _classStudentService.Insert(classStudent);
                }
            }

            @class.VirtualClassId = assists.VirtualClassId;
            @class.UnitActivityId = assists.ActivityId;
            @class.IsDictated = true;
            @class.DictatedDate = DateTime.UtcNow;
            @class.Commentary = assists.Commentary;
            @class.TeacherId = userId;

            if (assists.Evaluations != null && assists.Evaluations.Any(x => x.Taken))
            {
                @class.EvaluationId = assists.Evaluations.Where(x => x.Taken).Select(x => x.EvaluationId).FirstOrDefault();
            }
            else
            {
                @class.EvaluationId = null;
            }

            await _classService.Update(@class);
            await _weeklyAttendanceReportService.SaveWeeklyAttendanceReport(section.Id, @class.WeekNumber);
            await _classStudentService.AssignDPI(section.Id);
            return Ok();
        }

        /// <summary>
        /// Reporte consolidado de los estudiantes matriculados en una sección detallando 
        /// su cantidad de asistencias y faltas
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("reporte-excel/{sectionId}")]
        public async Task<IActionResult> GetSectionReportExcel(Guid sectionId)
        {
            var userId = GetUserId();
            var section = await _sectionService.Get(sectionId);
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
            var course = await _courseService.GetAsync(courseTerm.CourseId);
            var result = await _classStudentService.GetClassStudentReport(sectionId);
            var teachers = await _context.TeacherSections.Where(x => x.SectionId == sectionId).Select(x => x.Teacher.User.FullName).ToListAsync();

            var classes = await _context.Classes.Where(x => x.SectionId == sectionId)
                .OrderBy(x => x.StartTime)
                .Select(x => new
                {
                    x.Id,
                    x.StartTime,
                    x.ClassSchedule.SessionType
                })
                .ToListAsync();

            var dt = new DataTable
            {
                TableName = "Listado de alumnos"
            };

            var cont = 1;
            dt.Columns.Add("Nro");
            dt.Columns.Add("Código");
            dt.Columns.Add("Estudiante");
            dt.Columns.Add("Faltas");
            dt.Columns.Add("Clases Dictadas");
            dt.Columns.Add("% Asistencia");


            foreach (var item in result)
                dt.Rows.Add(
                    cont++,
                    item.UserName,
                    item.FullName,
                    item.Absences,
                    item.Dictated,
                    Math.Round((100 - item.AbsencesPercentage), 2, MidpointRounding.AwayFromZero)
                    );

            dt.AcceptChanges();

            var img = Path.Combine(_hostingEnvironment.WebRootPath, @"images/themes/" + CORE.Helpers.GeneralHelpers.GetTheme() + @"/logo-report.png");

            string fileName = $"Reporte asistencia estudiantes.xlsx";
            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.Row(1).InsertRowsAbove(7);
                var mergeRangeColumn = 'F';

                ws.Cell(2, 1).Value = CORE.Helpers.GeneralHelpers.GetInstitutionName().ToUpper();
                ws.Cell(2, 1).Style.Font.FontSize = 12;
                ws.Cell(2, 1).Style.Font.Bold = true;
                ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range($"A2:{mergeRangeColumn}2").Merge();

                ws.Cell(3, 1).Value = "Listado de Estudiantes";
                ws.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(3, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range($"A3:{mergeRangeColumn}3").Merge();

                ws.Cell(4, 1).Value = "CURSO";
                ws.Cell(4, 1).Style.Font.Bold = true;
                ws.Cell(4, 1).Style.Font.FontSize = 11;
                ws.Cell(4, 2).Value = $"{course.Code}-{course.Name}";
                ws.Range($"B4:{mergeRangeColumn}4").Merge();

                ws.Cell(5, 1).Value = "SECCIÓN";
                ws.Cell(5, 1).Style.Font.Bold = true;
                ws.Cell(5, 1).Style.Font.FontSize = 11;
                ws.Cell(5, 2).Value = section.Code;

                ws.Cell(6, 1).Value = "DOCENTE(S)";
                ws.Cell(6, 1).Style.Font.Bold = true;
                ws.Cell(6, 1).Style.Font.FontSize = 11;
                ws.Cell(6, 2).Value = $"{string.Join("; ", teachers)}";
                ws.Range($"D4:{mergeRangeColumn}4").Merge();

                ws.Rows().AdjustToContents();
                ws.Columns().AdjustToContents();


                wb.Worksheets.Add("Detallado");
                var ws_detailed = wb.Worksheet("Detallado");

                ws_detailed.Cell(2, 1).Value = CORE.Helpers.GeneralHelpers.GetInstitutionName().ToUpper();
                ws_detailed.Cell(2, 1).Style.Font.FontSize = 13;
                ws_detailed.Cell(2, 1).Style.Font.Bold = true;
                ws_detailed.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws_detailed.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws_detailed.Range(2, 1, 2, (classes.Count() + 2)).Merge();

                ws_detailed.Cell(3, 1).Value = "ASISTENCIA DE ALUMNOS";
                ws_detailed.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws_detailed.Cell(3, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws_detailed.Cell(3, 1).Style.Font.FontSize = 12;
                ws_detailed.Range(3, 1, 3, (classes.Count() + 2)).Merge();

                ws_detailed.Cell(4, 1).Value = "CURSO";
                ws_detailed.Cell(4, 1).Style.Font.Bold = true;
                ws_detailed.Cell(4, 1).Style.Font.FontSize = 11;
                ws_detailed.Cell(4, 2).Value = $"{course.Code}-{course.Name}";
                ws_detailed.Range($"B4:{mergeRangeColumn}4").Merge();

                ws_detailed.Cell(5, 1).Value = "SECCIÓN";
                ws_detailed.Cell(5, 1).Style.Font.Bold = true;
                ws_detailed.Cell(5, 1).Style.Font.FontSize = 11;
                ws_detailed.Cell(5, 2).Value = $"'{section.Code}";

                ws_detailed.Cell(6, 1).Value = "DOCENTE(S)";
                ws_detailed.Cell(6, 1).Style.Font.Bold = true;
                ws_detailed.Cell(6, 1).Style.Font.FontSize = 11;
                ws_detailed.Cell(6, 2).Value = $"{string.Join("; ", teachers)}";
                ws_detailed.Range($"D4:{mergeRangeColumn}4").Merge();

                //Table

                ws_detailed.Cell(8, 1).Value = "Usuario";
                ws_detailed.Cell(8, 1).Style.Font.Bold = true;
                ws_detailed.Cell(8, 1).Style.Font.FontSize = 11;
                ws_detailed.Cell(8, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws_detailed.Cell(8, 1).Style.Border.OutsideBorderColor = XLColor.Black;
                ws_detailed.Cell(8, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws_detailed.Cell(8, 2).Value = "Nombre Completo";
                ws_detailed.Cell(8, 2).Style.Font.Bold = true;
                ws_detailed.Cell(8, 2).Style.Font.FontSize = 11;
                ws_detailed.Cell(8, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws_detailed.Cell(8, 2).Style.Border.OutsideBorderColor = XLColor.Black;
                ws_detailed.Cell(8, 2).Style.Fill.BackgroundColor = XLColor.LightGray;

                var initClass = 3;

                foreach (var item in classes)
                {
                    var startClass = item.StartTime.ToDefaultTimeZone();

                    ws_detailed.Cell(8, initClass).Value = $"'{startClass.Day:00} {Environment.NewLine} {startClass.Month:00} {Environment.NewLine} {ConstantHelpers.SESSION_TYPE.VALUES[item.SessionType].Substring(0, 1)}";
                    ws_detailed.Cell(8, initClass).Style.Font.Bold = true;
                    ws_detailed.Cell(8, initClass).Style.Font.FontSize = 11;
                    ws_detailed.Cell(8, initClass).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws_detailed.Cell(8, initClass).Style.Border.OutsideBorderColor = XLColor.Black;
                    ws_detailed.Cell(8, initClass).Style.Fill.BackgroundColor = XLColor.LightGray;
                    ws_detailed.Cell(8, initClass).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws_detailed.Cell(8, initClass).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    initClass++;
                }

                var start = 9;

                foreach (var item in result)
                {
                    ws_detailed.Cell(start, 1).Value = $"'{item.UserName}";
                    ws_detailed.Cell(start, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws_detailed.Cell(start, 1).Style.Border.OutsideBorderColor = XLColor.Black;
                    ws_detailed.Cell(start, 1).Style.Font.FontSize = 11;
                    ws_detailed.Cell(start, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    ws_detailed.Cell(start, 2).Value = $"'{item.FullName}";
                    ws_detailed.Cell(start, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws_detailed.Cell(start, 2).Style.Border.OutsideBorderColor = XLColor.Black;
                    ws_detailed.Cell(start, 2).Style.Font.FontSize = 11;
                    ws_detailed.Cell(start, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    var detailStudentStart = 3;

                    foreach (var classEntity in classes)
                    {
                        var text = "-";

                        var classStudent = item.ClassStudentDetail.Where(y => y.ClassId == classEntity.Id).FirstOrDefault();

                        if (classStudent != null)
                        {
                            text = classStudent.IsAbsent ? "F" : "A";
                        }

                        ws_detailed.Cell(start, detailStudentStart).Value = $"'{text}";
                        ws_detailed.Cell(start, detailStudentStart).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws_detailed.Cell(start, detailStudentStart).Style.Border.OutsideBorderColor = XLColor.Black;
                        ws_detailed.Cell(start, detailStudentStart).Style.Font.FontSize = 11;
                        ws_detailed.Cell(start, detailStudentStart).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws_detailed.Cell(start, detailStudentStart).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws_detailed.Cell(start, detailStudentStart).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                        detailStudentStart++;
                    }

                    start++;
                }

                ws_detailed.Rows().AdjustToContents();
                ws_detailed.Columns().AdjustToContents();


                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
