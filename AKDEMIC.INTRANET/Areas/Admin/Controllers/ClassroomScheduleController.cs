using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Areas.Admin.Models.ClassroomScheduleViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;
using AKDEMIC.CORE.Helpers;
using ClosedXML.Excel;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/horarioaulas")]
    public class ClassroomScheduleController : BaseController
    {
        private readonly IClassService _classService;
        private readonly IClassScheduleService _classScheduleService;
        private readonly AkdemicContext _context;

        public ClassroomScheduleController(
            ITermService termService,
            IClassService classService,
            IClassScheduleService classScheduleService,
            AkdemicContext context
            ) : base(termService)
        {
            _classService = classService;
            _classScheduleService = classScheduleService;
            _context = context;
        }

        /// <summary>
        /// Vista donde se muestra el horario asignado a las aulas
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var term = await GetActiveTerm();
            if (term == null)
            {
                ErrorToastMessage("No existe periodo activo");
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var model = new IndexViewModel()
            {
                Term = new TermViewModel()
                {
                    Name = term?.Name ?? "",
                    ClassStartDate = term.ClassStartDate.ToLocalTime(),
                    ClassEndDate = term.ClassEndDate.ToLocalTime()
                }
            };
            return View(model);
        }

        /// <summary>
        /// Obtiene el listado de clases asignadas al aula
        /// </summary>
        /// <param name="cid">identificador del aula</param>
        /// <returns>Listado de clases</returns>
        [Route("{cid}/get")]
        public async Task<IActionResult> GetClassroomSchedule(Guid cid)
        {
            if (cid.Equals(Guid.Empty))
                return BadRequest($"No se pudo encontrar un aula con el id {cid}.");

            var today = (int)(DateTime.UtcNow.DayOfWeek);
            var term = await GetActiveTerm();
            var classes = await _classScheduleService.GetAllByClassroomAndTerm(cid, term.Id);

            classes = classes.Where(x => x.TeacherSchedules.All(y => y.Teacher != null)).ToList();

            classes = classes.Where(x => x.TeacherSchedules.All(y => y.Teacher.User != null)).ToList();

            var result = classes.Select(x => new
            {
                x.Id,
                title = string.Format("{0}-{1} ({2})", x.Section.CourseTerm.Course.Code,
                        x.Section.CourseTerm.Course.Name, x.Section.Code),
                description = x.TeacherSchedules.Any() ? string.Join(System.Environment.NewLine,
                        x.TeacherSchedules.Select(ts => ts.Teacher.User.FullName)) : "",
                allDay = false,
                start = DateTime.Today.AddDays((x.WeekDay + 1) - today).ToString("yyyy-MM-dd") + "T" +
                            x.StartTime.ToLocalDateTimeUtc().ToString("HH:mm:ss"),
                end = DateTime.Today.AddDays((x.WeekDay + 1) - today).ToString("yyyy-MM-dd") + "T" +
                          x.EndTime.ToLocalDateTimeUtc().ToString("HH:mm:ss")
            }).ToList();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle de la clase
        /// </summary>
        /// <param name="id">Identificador de la clase</param>
        /// <returns>Detalle de la clase</returns>
        [HttpGet("{id}/get-detalle")]
        public async Task<IActionResult> GetClass(Guid id)
        {
            var classSchedule = await _context.ClassSchedules
                .Where(x => x.Id == id).Select(x => new
                {
                    classroom = x.Classroom.Description,
                    teachers = x.TeacherSchedules.Select(cs => cs.Teacher.User.FullName),
                    date = ConstantHelpers.WEEKDAY.VALUES[x.WeekDay],
                    start = x.StartTime.ToLocalDateTimeFormatUtc(),
                    end = x.EndTime.ToLocalDateTimeFormatUtc(),
                    course = $"{x.Section.CourseTerm.Course.Code}-{x.Section.CourseTerm.Course.Name}",
                    section = x.Section.Code,
                    type = ConstantHelpers.SESSION_TYPE.VALUES[x.SessionType]
                })
                .FirstOrDefaultAsync();

            return Ok(classSchedule);
        }

        [HttpGet("get-excel")]
        public async Task<IActionResult> GetExcel(Guid cid)
        {
            var term = await GetActiveTerm();
            var classes = await _classScheduleService.GetAllByClassroomAndTerm(cid, term.Id);
            var classroom = await _context.Classrooms.Where(x => x.Id == cid)
                .Select(x => new
                {
                    x.Description,
                    building = x.Building.Name,
                    campus = x.Building.Campus.Name
                })
                .FirstOrDefaultAsync();

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Dia");
            dt.Columns.Add("Hora Inicio");
            dt.Columns.Add("Hora Fin");
            dt.Columns.Add("Tipo");
            dt.Columns.Add("Curso");
            dt.Columns.Add("Sección");
            dt.Columns.Add("Docentes");

            classes = classes.OrderBy(x => x.WeekDay).ThenBy(x => x.StartTime.ToLocalDateTimeFormatUtc()).ToList();

            foreach (var item in classes)
                dt.Rows.Add(
                    ConstantHelpers.WEEKDAY.VALUES[item.WeekDay],
                    item.StartTime.ToLocalDateTimeFormatUtc(),
                    item.EndTime.ToLocalDateTimeFormatUtc(),
                    ConstantHelpers.SESSION_TYPE.VALUES[item.SessionType],
                    item.Section.CourseTerm.Course.FullName,
                    item.Section.Code,
                    item.TeacherSchedules.Any() ? string.Join(System.Environment.NewLine,
                        item.TeacherSchedules.Select(ts => ts.Teacher.User.FullName)) : string.Empty
                    );

            dt.AcceptChanges();

            var fileName = $"Horarios del aula.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var worksheet = wb.Worksheet(dt.TableName);

                worksheet.Row(1).InsertRowsAbove(9);

                var mergeRangeColumn = 'G';

                worksheet.Cell(2, 1).Value = GeneralHelpers.GetInstitutionName().ToUpper();
                worksheet.Cell(2, 1).Style.Font.FontSize = 12;
                worksheet.Cell(2, 1).Style.Font.Bold = true;
                worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A2:{mergeRangeColumn}2").Merge();

                worksheet.Cell(3, 1).Value = $"Horarios del aula";
                worksheet.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(3, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range($"A3:{mergeRangeColumn}3").Merge();

                worksheet.Cell(5, 1).Value = "Periodo Académico";
                worksheet.Cell(5, 1).Style.Font.Bold = true;
                worksheet.Cell(5, 1).Style.Font.FontSize = 11;
                worksheet.Cell(5, 2).Value = term is null ? "-" : $"'{term.Name}";
                worksheet.Range($"B5:{mergeRangeColumn}5").Merge();

                worksheet.Cell(6, 1).Value = "Campus";
                worksheet.Cell(6, 1).Style.Font.Bold = true;
                worksheet.Cell(6, 1).Style.Font.FontSize = 11;
                worksheet.Cell(6, 2).Value = classroom?.campus;
                worksheet.Range($"B6:{mergeRangeColumn}6").Merge();

                worksheet.Cell(7, 1).Value = "Edificio";
                worksheet.Cell(7, 1).Style.Font.Bold = true;
                worksheet.Cell(7, 1).Style.Font.FontSize = 11;
                worksheet.Cell(7, 2).Value = classroom?.building;
                worksheet.Range($"B7:{mergeRangeColumn}7").Merge();

                worksheet.Cell(8, 1).Value = "Aula";
                worksheet.Cell(8, 1).Style.Font.Bold = true;
                worksheet.Cell(8, 1).Style.Font.FontSize = 11;
                worksheet.Cell(8, 2).Value = classroom?.Description;
                worksheet.Range($"B8:{mergeRangeColumn}8").Merge();

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

    }
}
