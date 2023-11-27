using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.Report_teacherViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)]
    [Area("CareerDirector")]
    [Route("director-carrera/reporte-asistencia-docentes")]
    public class ReportTeacherAssistanceController : BaseController
    {
        private readonly ITeacherService _teacherService;

        public ReportTeacherAssistanceController(
            AkdemicContext context,
            ITeacherService teacherService,
            IDataTablesService dataTablesService
        ) : base(context, dataTablesService)
        {
            _teacherService = teacherService;
        }

        /// <summary>
        /// Vista principal del reporte de asistencia docente
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de docentes para ser usado en tablas
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <param name="academicDepartmentId">Identificador del departamento académico</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <returns>Listado de docentes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetTeachers(string search, Guid? academicDepartmentId = null, Guid? facultyId = null)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _teacherService.GetTeacherForAsistanceDatatable(parameters, facultyId, academicDepartmentId, User, search);
            return Ok(result);

        }

        /// <summary>
        /// Vista detalle del reporte de asistencia docente
        /// </summary>
        /// <param name="id">Identificador del docente</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var teacher = await _teacherService.GetTeacherWithData(id);
            var model = new Admin.Models.ReportTeacherAssistanceViewModels.TeacherViewModel
            {
                TeacherId = teacher.UserId,
                UserName = teacher.User.UserName,
                Name = teacher.User.FullName,
                Career = teacher.Career.Name,
                Faculty = teacher.Career.Faculty.Name
            };


            return View(model);
        }

        /// <summary>
        /// Obtiene el listado de secciones asignadas al docente para ser usado en tablas
        /// </summary>
        /// <param name="teacherId">Identificador del docente</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Listado de secciones</returns>
        [HttpGet("secciones/get")]
        public async Task<IActionResult> GetSectionAbsenceDetail(string teacherId, Guid termId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            var query = _context.TeacherSections
                .Where(x => x.TeacherId == teacherId && x.Section.CourseTerm.TermId == termId)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var teacherClasses = await _context.Classes
                .Where(x => x.Section.CourseTerm.TermId == termId && x.ClassSchedule.TeacherSchedules.Any(y => y.TeacherId == teacherId))
                //.Select()
                .ToListAsync();

            var dataDB = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    course = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    sectionId = x.SectionId
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.course,
                    x.section,
                    totalClasses = teacherClasses.Where(t => t.SectionId == x.sectionId).Count(),
                    dictatedClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.IsDictated).Count(),
                    rescheduledClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.IsRescheduled).Count()
                })
                .ToList();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return Ok(result);
        }
    }
}
