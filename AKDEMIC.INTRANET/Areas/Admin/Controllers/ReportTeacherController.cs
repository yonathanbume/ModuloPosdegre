using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Areas.Admin.Models.ReportTeacherViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using ClosedXML.Excel;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
        ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.DEAN + "," +
        ConstantHelpers.ROLES.DEAN_SECRETARY + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        ConstantHelpers.ROLES.REPORT_QUERIES + "," +
        ConstantHelpers.ROLES.VICERRECTOR)]
    [Area("Admin")]
    [Route("admin/reporte-docentes")]
    public class ReportTeacherController : BaseController
    {
        private readonly ISectionService _sectionService;
        private readonly ITeacherService _teacherService;
        private readonly ISectionGroupService _sectionGroupService;
        private readonly IClassService _classService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly AkdemicContext _context;
        private readonly IClassStudentService _classStudentService;
        private readonly IViewRenderService _viewRenderService;
        private IWebHostEnvironment _hostingEnvironment;
        public IConverter _dinkConverter { get; }
        public ReportTeacherController(AkdemicContext context,
              IConverter dinkConverter,
              IClassStudentService classStudentService,
            IViewRenderService viewRenderService,
            IWebHostEnvironment environment,
            IDataTablesService dataTablesService,
            ISectionService sectionService,
            ITeacherService teacherService,
            ISectionGroupService sectionGroupService,
            IClassService classService,
            IStudentSectionService studentSectionService) : base(dataTablesService)
        {
            _context = context;
            _sectionService = sectionService;
            _teacherService = teacherService;
            _sectionGroupService = sectionGroupService;
            _classService = classService;
            _studentSectionService = studentSectionService;
            _dinkConverter = dinkConverter;
            _classStudentService = classStudentService;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
        }

        /// <summary>
        /// Vista principal del controlador
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de docentes para ser usado en tablas
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="academicDepartmentId">Identificador del departamento académico</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de docentes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid termId, Guid? academicDepartmentId = null, string search = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _teacherService.GetTeachersDatatable(sentParameters, null, null, search, termId, academicDepartmentId, User);
            return Ok(result);
        }

        /// <summary>
        /// Vista detalle del docente
        /// </summary>
        /// <param name="teacherId">Identificador del docente</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("detalle/{teacherId}/{termId}")]
        public async Task<IActionResult> Detail(string teacherId, Guid termId)
        {
            var model = await _context.Teachers.Where(x => x.UserId == teacherId)
                .Select(x => new TeacherDetailViewModel
                {
                    AcademicDepartment = x.AcademicDepartment.Name ?? "-",
                    Teacher = x.User.FullName,
                    TeacherId = x.User.Id,
                    Username = x.User.UserName,
                    TermId = termId
                })
                .FirstOrDefaultAsync();

            return View(model);
        }

        /// <summary>
        /// Obtiene el listado de alumnos de una sección detallando sus asistencias y faltas
        /// </summary>
        /// <param name="sectionId">Identificador del a sección</param>
        /// <returns>Objeto que contiene el listado de alumnos</returns>
        [HttpGet("detalle/asistencia/get")]
        public async Task<IActionResult> GetAssistanceReport(Guid sectionId)
        {
            var result = await _studentSectionService.GetStudentAssistanceReportDataTable(sectionId);
            return Ok(result);
        }

        /// <summary>
        /// Genera el reporte de asistencia de una sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("detalle/{sectionId}/asistencia/excel")]
        public async Task<IActionResult> GetAssistanceExcelReport(Guid sectionId)
        {
            var data = await _studentSectionService.GetStudentAssistanceReportDataTable(sectionId);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            var cont = 0;
            dt.Columns.Add("Nro");
            dt.Columns.Add("Estudiante");
            dt.Columns.Add("Faltas");
            dt.Columns.Add("Asistidas");
            dt.Columns.Add("Dictadas");

            foreach (var item in data.Data)
                dt.Rows.Add(
                    cont++,
                    item.student,
                    item.absences,
                    item.assisted,
                    item.dictated
                    );

            dt.AcceptChanges();

            var img = Path.Combine(_hostingEnvironment.WebRootPath, @"images/themes/" + CORE.Helpers.GeneralHelpers.GetTheme() + @"/logo-report.png");

            string fileName = $"Reporte asistencia estudiantes.xlsx";
            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                //ws.AddHeaderToWorkSheet("Reporte asistencia estudiantes", img);
                ws.AddHeaderToWorkSheet("Reporte asistencia estudiantes", null);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        /// <summary>
        /// Genera el reporte de asistencia de una sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <param name="teacherId">Identificador del docente</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("detalle/{sectionId}/asistencia/pdf")]
        public async Task<IActionResult> GetAssistancePdfReport(Guid sectionId, string teacherId)
        {
            var data = await _studentSectionService.GetStudentAssistanceReportDataTable(sectionId);
            var section = await _sectionService.GetWithTeacherSections(sectionId);
            var model = new AssistancePdfReportViewModel
            {
                Teacher = string.Join(",", section.TeacherSections.Where(x => x.TeacherId == teacherId).Select(x => x.Teacher.User.FullName)),
                Course = section.CourseTerm.Course.Name,
                Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                ListItems = data.Data.Select(x => new AssistancePdfReportData
                {
                    absences = x.absences,
                    assisted = x.assisted,
                    dictated = x.dictated,
                    maxAbsences = x.maxAbsences,
                    student = x.student
                }).ToList()
            };

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = "Reporte asistencia estudiantes"
            };

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/ReportTeacher/StudentAssistanceReportPdf.cshtml", model);

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
            return File(fileByte, "application/pdf", $"Reporte asistencia estudiantes.pdf");

            //return Ok(result);
        }

        /// <summary>
        /// Vista parcial donde se detalla las evaluaciones del curso y las notas oficiales de cada alumno.
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("detalle/notas/get")]
        public async Task<IActionResult> GradeReportPartialView(Guid sectionId)
        {
            var model = await _studentSectionService.GetSectionPartialGradesTemplate(sectionId);
            return PartialView(model);
        }

        /// <summary>
        /// Obtiene el listado de clases asignadas al docente para ser usado en el horario
        /// </summary>
        /// <param name="id">Identificador del docente</param>
        /// <param name="start">Fecha de inicio</param>
        /// <param name="end">Fecha fin</param>
        /// <returns>Objeto que contiene el listado de clases</returns>
        [HttpGet("get-horario/{id}")]
        public async Task<IActionResult> GetClasses(string id, DateTime start, DateTime end)
        {
            var dateStart = start.ToUniversalTime();
            var dateEnd = end.ToUniversalTime();

            var term = await GetActiveTerm();
            if (term == null)
                return BadRequest();

            id = id ?? _userManager.GetUserId(User);

            return Ok(await _classService.GetAllByTermIdTeacherIdAndDateRange(term.Id, id, dateStart, dateEnd));
        }

        /// <summary>
        /// Método para borrar las asistencias de una sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("detalle/{sectionId}/limpiar-asistencias")]
        public async Task<IActionResult> CleanClassStudents(Guid sectionId)
        {
            var classStudents = await _context.ClassStudents.Where(x => x.Class.ClassSchedule.SectionId == sectionId).ToListAsync();
            var classReschedules = await _context.ClassReschedules.Where(x => x.Class.ClassSchedule.SectionId == sectionId).ToListAsync();
            var studentAbsenceJustifications = await _context.StudentAbsenceJustifications.Where(x => x.ClassStudent.Class.ClassSchedule.SectionId == sectionId).ToListAsync();
            var classes = await _context.Classes.Where(x => x.SectionId == sectionId).ToListAsync();

            foreach (var item in classes)
            {
                item.IsDictated = false;
                item.DictatedDate = null;
            }

            _context.StudentAbsenceJustifications.RemoveRange(studentAbsenceJustifications);
            _context.ClassReschedules.RemoveRange(classReschedules);
            _context.ClassStudents.RemoveRange(classStudents);
            await _context.SaveChangesAsync();
            await _classStudentService.AssignDPI(sectionId);

            return Ok($"Se eliminaron {classStudents.Count()} asistencias.");
        }

        [HttpGet("seccion/{sectionId}/control-asistencia-excel")]
        public async Task<IActionResult> GetAssisControlExceL(Guid sectionId)
        {
            var course = await _context.Sections.Where(x => x.Id == sectionId)
                .Select(x => new
                {
                    section = x.Code,
                    name = $"{x.CourseTerm.Course.Code} {x.CourseTerm.Course.Name}",
                })
                .FirstOrDefaultAsync();

            var schedules = await _context.ClassSchedules.Where(x => x.SectionId == sectionId).OrderBy(x => x.WeekDay).ThenBy(x => x.StartTime).ToListAsync();
            var classes = await _context.Classes.Where(x => x.SectionId == sectionId).OrderBy(x => x.WeekNumber).ThenBy(y => y.ClassNumber).ToListAsync();
            var dictatedClases = classes.Where(x => x.IsDictated).Count();
            var totalWeeks = classes.Max(y => y.WeekNumber);
            var classStudents = await _context.ClassStudents.Where(x => x.Class.ClassSchedule.SectionId == sectionId).ToListAsync();
            var students = await _context.StudentSections.Where(x => x.SectionId == sectionId)
                .OrderBy(x => x.Student.User.FullName)
                .Select(x => new
                {
                    x.StudentId,
                    x.Student.User.FullName,
                })
                .ToListAsync();

            string fileName = $"Reporte de estudiantes con encuestas pendientes.xlsx";
            using (var wb = new XLWorkbook())
            {
                wb.AddWorksheet("Reporte");
                var worksheet = wb.Worksheet("Reporte");

                var mergeRangeColumn = 'J';

                worksheet.Cell(2, 1).Value = GeneralHelpers.GetInstitutionName().ToUpper();
                worksheet.Cell(2, 1).Style.Font.FontSize = 12;
                worksheet.Cell(2, 1).Style.Font.Bold = true;
                worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A2:{mergeRangeColumn}2").Merge();

                worksheet.Cell(3, 1).Value = $"FORMATO DE CONTROL DE ASISTENIA";
                worksheet.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(3, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A3:{mergeRangeColumn}3").Merge();

                worksheet.Cell(5, 1).Value = $"{course.name} - {course.section}";
                worksheet.Cell(5, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell(5, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(5, 1).Style.Font.Bold = true;
                worksheet.Range($"A5:{mergeRangeColumn}5").Merge();

                //Cabeceras
                worksheet.Cell(7, 1).Value = "Nª";
                worksheet.Cell(7, 1).Style.Font.Bold = true;
                worksheet.Cell(7, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(7, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range(7, 1, 9, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(7, 1, 9, 1).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Range(7, 1, 9, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

                worksheet.Range(7, 1, 9, 1).Merge();
                worksheet.Cell(7, 2).Value = "Apellidos y Nombres del Alumno";
                worksheet.Cell(7, 2).Style.Font.Bold = true;
                worksheet.Cell(7, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(7, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range(7, 2, 9, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(7, 2, 9, 2).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Range(7, 2, 9, 2).Style.Fill.BackgroundColor = XLColor.LightGray;
                worksheet.Range(7, 2, 9, 2).Merge();

                var initRange = 3;
                for (int i = 1; i <= totalWeeks; i++)
                {
                    worksheet.Cell(7, initRange).Value = $"Semana {i}";
                    worksheet.Cell(7, initRange).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(7, initRange).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell(7, initRange).Style.Font.Bold = true;

                    var toMerge = initRange + schedules.Count() - 1;
                    worksheet.Range(7, initRange, 7, toMerge).Merge();
                    worksheet.Range(7, initRange, 7, toMerge).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Range(7, initRange, 7, toMerge).Style.Border.OutsideBorderColor = XLColor.Black;
                    worksheet.Range(7, initRange, 7, toMerge).Style.Fill.BackgroundColor = XLColor.LightGray;

                    initRange += schedules.Count();
                }

                var initClass = 3;
                foreach (var @class in classes)
                {
                    worksheet.Cell(8, initClass).Value = @class.StartTime.ToLocalDateFormat();
                    worksheet.Cell(8, initClass).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(8, initClass).Style.Border.OutsideBorderColor = XLColor.Black;
                    //worksheet.Cell(7, initClass).Style.Alignment.SetTextRotation(180);
                    worksheet.Cell(9, initClass).Value = ConstantHelpers.WEEKDAY.ENUM_TO_STRING(@class.StartTime.ToDefaultTimeZone().DayOfWeek);
                    worksheet.Cell(9, initClass).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(9, initClass).Style.Border.OutsideBorderColor = XLColor.Black;

                    initClass++;
                }

                worksheet.Cell(7, initClass).Value = "% Asist.";
                worksheet.Cell(7, initClass).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(7, initClass).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(7, initClass).Style.Font.Bold = true;
                worksheet.Range(7, initClass, 9, initClass).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(7, initClass, 9, initClass).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Range(7, initClass, 9, initClass).Style.Fill.BackgroundColor = XLColor.LightGray;
                worksheet.Range(7, initClass, 9, initClass).Merge();


                //Detalles
                var initStudents = 10;
                for (int i = 0; i < students.Count(); i++)
                {
                    worksheet.Cell(initStudents, 1).Value = (i + 1);
                    worksheet.Cell(initStudents, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(initStudents, 1).Style.Border.OutsideBorderColor = XLColor.Black;
                    worksheet.Cell(initStudents, 2).Value = students[i].FullName;
                    worksheet.Cell(initStudents, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(initStudents, 2).Style.Border.OutsideBorderColor = XLColor.Black;

                    var assists = 0;
                    var initClassStudent = 3;
                    foreach (var @class in classes)
                    {
                        var classStudent = classStudents.Where(x => x.StudentId == students[i].StudentId && x.ClassId == @class.Id).FirstOrDefault();

                        worksheet.Cell(initStudents, initClassStudent).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(initStudents, initClassStudent).Style.Border.OutsideBorderColor = XLColor.Black;

                        if (classStudent != null)
                        {
                            worksheet.Cell(initStudents, initClassStudent).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(initStudents, initClassStudent).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            if (classStudent.IsAbsent)
                            {
                                worksheet.Cell(initStudents, initClassStudent).Style.Fill.BackgroundColor = XLColor.Yellow;
                                worksheet.Cell(initStudents, initClassStudent).Value = "F";
                            }
                            else
                            {
                                assists++;
                                worksheet.Cell(initStudents, initClassStudent).Value = "A";
                            }
                        }
                        initClassStudent++;
                    }

                    decimal attendancePercentage = 0M;

                    if(dictatedClases != 0) 
                        attendancePercentage = Math.Round((decimal)assists * 100M / (decimal)dictatedClases, 2, MidpointRounding.AwayFromZero);

                    worksheet.Cell(initStudents, initClassStudent).Value = $"'{attendancePercentage:0.00}";
                    worksheet.Cell(initStudents, initClassStudent).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(initStudents, initClassStudent).Style.Border.OutsideBorderColor = XLColor.Black;

                    initStudents++;
                }

                var detailAssist = initStudents;
                var detailAbsent = initStudents + 1;

                worksheet.Cell(detailAssist, 2).Value = "Asistencias";
                worksheet.Cell(detailAssist, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                worksheet.Cell(detailAssist, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(detailAbsent, 2).Value = "Inasistencias";
                worksheet.Cell(detailAbsent, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                worksheet.Cell(detailAbsent, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                var initDetailClass = 3;
                foreach (var @class in classes)
                {
                    var assits = classStudents.Where(x => x.ClassId == @class.Id).Where(x => !x.IsAbsent).Count();
                    var absent = classStudents.Where(x => x.ClassId == @class.Id).Where(x => x.IsAbsent).Count();

                    worksheet.Cell(detailAssist, initDetailClass).Value = assits;
                    worksheet.Cell(detailAbsent, initDetailClass).Value = absent;
                    initDetailClass++;
                }

                worksheet.Rows().AdjustToContents();
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }

        [HttpPost("detalle/{sectionId}/borrar-asistencias-seleccionadas")]
        public async Task<IActionResult> CleanSelectedClassStudents(CleanClassesViewModel model)
        {
            var section = await _context.Sections.Where(x => x.Id == model.SectionId).FirstOrDefaultAsync();
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Id == courseTerm.TermId).FirstOrDefaultAsync();

            if (term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                return BadRequest("Solo se pueden eliminar clases dictadas del periodo activo.");

            var classes = await _context.Classes.Where(x => model.ClassesId.Contains(x.Id)).ToListAsync();
            var classStudents = await _context.ClassStudents.Where(x => model.ClassesId.Contains(x.ClassId)).ToListAsync();
            var classStudentsIdHash = classStudents.Select(x => x.Id).ToHashSet();
            var studentAbsenceJustifications = await _context.StudentAbsenceJustifications.Where(x => classStudentsIdHash.Contains(x.ClassStudentId)).ToListAsync();

            foreach (var item in classes)
            {
                item.IsDictated = false;
                item.DictatedDate = null;
            }

            _context.StudentAbsenceJustifications.RemoveRange(studentAbsenceJustifications);
            _context.ClassStudents.RemoveRange(classStudents);
            await _context.SaveChangesAsync();
            await _classStudentService.AssignDPI(section.Id);

            return Ok($"Se eliminaron {classStudents.Count()} asistencias.");
        }

    
        [HttpGet("get-clases-dictadas")]
        public async Task<IActionResult> GetDictatedClassesSelect(Guid sectionId, string startDate, string endDate, Guid? sectionGroupId)
        {
            var query = _context.Classes.Where(x => x.SectionId == sectionId && x.IsDictated).AsNoTracking();

            if (sectionGroupId.HasValue)
                query = query.Where(x => x.ClassSchedule.SectionGroupId == sectionGroupId);

            var classes = await query
                .Select(x=>new
                {
                    x.Id,
                    x.StartTime,
                    x.EndTime,
                    SectionGroup = x.ClassSchedule.SectionGroup.Code
                })
                .ToListAsync();

            if (!string.IsNullOrEmpty(startDate))
            {
                var startDateTime = ConvertHelpers.DatepickerToDatetime(startDate);
                classes = classes.Where(x => x.StartTime.ToDefaultTimeZone().Date >= startDateTime.Date).ToList();
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                var endDateTime = ConvertHelpers.DatepickerToDatetime(endDate);
                classes = classes.Where(x => x.EndTime.ToDefaultTimeZone().Date <= endDateTime.Date).ToList();
            }

            var result = classes
                .OrderBy(x => x.StartTime)
                .Select(x => new
                {
                    x.Id,
                    Text = $"{(string.IsNullOrEmpty(x.SectionGroup) ? "" : $"{x.SectionGroup} - ")} {x.StartTime.ToLocalDateTimeFormat()} - {x.EndTime.ToLocalDateTimeFormat()}"
                })
                .ToList();

            return Ok(result);
        }
    }
}
