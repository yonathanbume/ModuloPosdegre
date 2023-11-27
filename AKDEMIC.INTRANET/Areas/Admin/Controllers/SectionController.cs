using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.INTRANET.Areas.Admin.Models.SectionsViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using DinkToPdf.Contracts;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/secciones")]
    public class SectionController : BaseController
    {
        private IWebHostEnvironment _hostingEnvironment;
        public IConverter _dinkConverter { get; }
        private readonly ISelect2Service _select2Service;
        private readonly ICourseService _courseService;
        private readonly IViewRenderService _viewRenderService;

        public SectionController(AkdemicContext context, IConverter dinkConverter,
            IViewRenderService viewRenderService, IWebHostEnvironment environment,
            ISelect2Service select2Service,
            ICourseService courseService) : base(context)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
            _select2Service = select2Service;
            _courseService = courseService;
        }

        /// <summary>
        /// Vista donde se listan las secciones aperturadas
        /// </summary>
        /// <returns>Vista principal del sistema</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de secciones aperturadas filtrados por los datos del usuario
        /// </summary>
        /// <param name="cid">Identificador del curso-periodo</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene el listado de secciones</returns>
        [HttpGet("{cid}/periodo/{pid}")]
        public async Task<IActionResult> GetSectionFilter(Guid? cid, Guid? pid)
        {
            if (cid.Equals(Guid.Empty))
                return NotFound($"No se pudo encontrar un Curso con el id '{cid}'.");
            if (pid.Equals(Guid.Empty))
                return NotFound($"No se pudo encontrar un Periodo con el id '{pid}'.");
            var result = await (from s in _context.Sections
                                join ct in _context.CourseTerms on s.CourseTerm.Id equals ct.Id
                                where ct.Term.Id.Equals(pid) && ct.Course.Id.Equals(cid)
                                select new
                                {
                                    id = s.Id,
                                    code = s.Code,
                                    teacher = s.TeacherSections.Any() ? string.Join(", ", s.TeacherSections.Select(x => x.Teacher.User.Name)) : "No Asignado",
                                    id_teacher = s.TeacherSections.Any() ? s.TeacherSections.Select(x => x.Teacher.UserId) : null,
                                    vacancies = s.Vacancies,
                                    termfinished = (ct.Term.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.FINISHED) ? "Finished" : ""
                                }).ToListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Objeto que contiene el listado de secciones aperturadas
        /// </summary>
        /// <returns>Objeto que contiene el listado de secciones</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetSections()
        {
            var result = await (from s in _context.Sections
                                select new
                                {
                                    code = s.Code,
                                    teacher = s.TeacherSections.Any() ? string.Join(", ", s.TeacherSections.Select(x => x.Teacher.User.Name)) : "No Asignado",
                                    vacancies = s.Vacancies
                                }).ToListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de cursos
        /// </summary>
        /// <returns>Objeto que contiene el listado de cursos</returns>
        [HttpGet("getcareers")]
        public async Task<IActionResult> GetCourses()
        {
            var result = await _context.Courses
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .OrderBy(x => x.text)
                .ToListAsync();

            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de cursos para ser usado en select
        /// </summary>
        /// <param name="selectedId">Identificador del curso seleccionado</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de cursos</returns>
        [HttpGet("obtener-cursos")]
        public async Task<IActionResult> GetSchools(Guid? selectedId, string searchValue)
        {
            var requestParameters = _select2Service.GetRequestParameters();
            var result = await _courseService.GetCoursesServerSideSelect2(requestParameters, searchValue, selectedId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de periodos académicos
        /// </summary>
        /// <returns>Objeto que contiene el listado de periodos académicos</returns>
        [HttpGet("getterms")]
        public async Task<IActionResult> GetTerms()
        {

            var result = await _context.Terms
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .OrderByDescending(x => x.text)
                .ToListAsync();

            return Ok(new { items = result });

        }

        /// <summary>
        /// Objeto que contiene las secciones asignadas al docente
        /// </summary>
        /// <returns>Objeto que contiene el listado de secciones asignadas al docente</returns>
        [HttpGet("editar-seccion-profesor")]
        public async Task<IActionResult> GetProfesores()
        {
            var result = await (from s in _context.TeacherSections
                                select new
                                {
                                    id = s.Teacher.User.Id,
                                    text = s.Teacher.User.FullName
                                }).Distinct().ToListAsync();

            return Ok(new { items = result });
        }

        /// <summary>
        /// Método para asignar una sección al docente
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del docente y la sección</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar/post")]
        public async Task<IActionResult> UpdateSection(SectionViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var section = await _context.Sections.Include(x => x.TeacherSections)
                .FirstOrDefaultAsync(x => x.Id == model.SectionId);
            if (section.TeacherSections.Any())
                _context.TeacherSections.RemoveRange(section.TeacherSections);
            var teacherSection = new TeacherSection
            {
                TeacherId = model.TeacherId,
                SectionId = section.Id
            };
            await _context.TeacherSections.AddAsync(teacherSection);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Genera el acta final de la sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("acta-final/{sectionId}")]
        public async Task<IActionResult> EvaluationReport(Guid sectionId)
        {
            var section = await _context.Sections.Include(x => x.CourseTerm)
                .FirstOrDefaultAsync(x => x.Id == sectionId);

            if (section == null) return BadRequest();

            var careerId = await _context.Sections.Where(x => x.Id == sectionId)
                .Select(x => x.CourseTerm.Course.CareerId)
                .FirstOrDefaultAsync();

            var academicYear = (byte)0;
            var year = (byte)0;
            if (careerId != null)
            {
                academicYear = (await _context.AcademicYearCourses.Where(x =>
                    x.Curriculum.CareerId == careerId &&
                    x.CourseId == section.CourseTerm.CourseId).Select(x => x.AcademicYear).FirstOrDefaultAsync());

                year = (byte)Math.Round(academicYear * 1.00M / 2, 0, MidpointRounding.AwayFromZero);
            }

            var asyncModel = _context.Sections
                .Where(x => x.Id == sectionId)
                .Select(x => new ReportViewModel
                {
                    CourseCode = x.CourseTerm.Course.Code,
                    CourseName = x.CourseTerm.Course.Name,
                    Credits = x.CourseTerm.Course.Credits,
                    Teacher = x.TeacherSections.Any()
                        ? string.Join(", ", x.TeacherSections.Select(ts => ts.Teacher.User.RawFullName))
                        : "No Asignado",
                    Term = x.CourseTerm.Term.Name,
                    Section = x.Code,
                    Carrer = x.CourseTerm.Course.Career.Name,
                    Faculty = x.CourseTerm.Course.Career.Faculty.Name,
                    AcademicYear = academicYear,
                    Year = year
                }).FirstOrDefaultAsync();

            var asyncStudents = _context.StudentSections
                .Where(x => x.Section.Id == sectionId)
                .Select(x => new StudentGradeViewModel
                {
                    Code = x.Student.User.UserName,
                    FullName = x.Student.User.PaternalSurname + " " + x.Student.User.MaternalSurname + " " + x.Student.User.Name,
                    Grade = x.FinalGrade,
                    GradeText = AKDEMIC.CORE.Helpers.ConvertHelpers.NumberToText(x.FinalGrade)
                }).ToListAsync();


            var model = await asyncModel;
            model.ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{@GeneralHelpers.GetTheme()}/logo-report.png");
            model.Students = await asyncStudents;
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 0, Left = 10, Right = 10 }
            };
            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/Section/Report.cshtml", model);
            var cssPtah = Path.Combine(_hostingEnvironment.WebRootPath, @"css/pages/pdf/evaluationreport.css");

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPtah },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };
            var pdf = new DinkToPdf.HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var fileByte = _dinkConverter.Convert(pdf);
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf", "Acta.pdf");
        }

        /// <summary>
        /// Genera el reporte de promedios de la sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("promedios/{sectionId}")]
        public async Task<IActionResult> EvaluationAverageReport(Guid sectionId)
        {
            var section = await _context.Sections.Include(x => x.CourseTerm)
                .FirstOrDefaultAsync(x => x.Id == sectionId);

            if (section == null) return BadRequest();

            var careerId = await _context.Sections.Where(x => x.Id == sectionId)
                .Select(x => x.CourseTerm.Course.CareerId)
                .FirstOrDefaultAsync();

            var academicYear = (byte)0;
            var year = (byte)0;
            if (careerId != null)
            {
                academicYear = (await _context.AcademicYearCourses.Where(x =>
                    x.Curriculum.CareerId == careerId &&
                    x.CourseId == section.CourseTerm.CourseId).Select(x => x.AcademicYear).FirstOrDefaultAsync());

                year = (byte)Math.Round(academicYear * 1.00M / 2, 0, MidpointRounding.AwayFromZero);
            }

            var asyncModel = _context.Sections
                .Where(x => x.Id == sectionId)
                .Select(x => new ReportViewModel
                {
                    CourseCode = x.CourseTerm.Course.Code,
                    CourseName = x.CourseTerm.Course.Name,
                    Credits = x.CourseTerm.Course.Credits,
                    Teacher = x.TeacherSections.Any()
                        ? string.Join(", ", x.TeacherSections.Select(ts => ts.Teacher.User.RawFullName))
                        : "No Asignado",
                    Term = x.CourseTerm.Term.Name,
                    Section = x.Code,
                    Carrer = x.CourseTerm.Course.Career.Name,
                    Faculty = x.CourseTerm.Course.Career.Faculty.Name,
                    AcademicYear = academicYear,
                    Year = year
                }).FirstOrDefaultAsync();

            var asyncStudents = _context.StudentSections
                .Where(x => x.Section.Id == sectionId)
                .Select(x => new StudentGradeViewModel
                {
                    Code = x.Student.User.UserName,
                    FullName = x.Student.User.PaternalSurname + " " + x.Student.User.MaternalSurname + " " + x.Student.User.Name,
                    Grade = x.FinalGrade,
                    GradeText = AKDEMIC.CORE.Helpers.ConvertHelpers.NumberToText(x.FinalGrade)
                }).ToListAsync();


            var model = await asyncModel;
            model.ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{@GeneralHelpers.GetTheme()}/logo-report.png");
            model.Students = await asyncStudents;
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Landscape,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 0, Left = 10, Right = 10 }
            };
            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/Section/Report2.cshtml", model);
            var cssPtah = Path.Combine(_hostingEnvironment.WebRootPath, @"css/pages/pdf/evaluationreport.css");

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPtah },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };
            var pdf = new DinkToPdf.HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var fileByte = _dinkConverter.Convert(pdf);
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf", "Acta.pdf");
        }

    }

}
