using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using AKDEMIC.CORE.Services;
using DinkToPdf.Contracts;
using AKDEMIC.INTRANET.Areas.Teacher.Models.SectionViewModels;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.INTRANET.Model;
using Microsoft.Extensions.Options;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/secciones")]
    public class SectionController : BaseController
    {
        private readonly IConfigurationService _configurationService;
        private readonly IAcademicYearCourseService _academicYearCourseService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ITeacherSectionService _teacherSectionService;
        private readonly IViewRenderService _viewRenderService;
        private readonly IConverter _dinkConverter;
        private readonly ISectionGroupService _sectionGroupService;
        private readonly ISectionService _sectionService;
        protected ReportSettings _reportSettings;

        public SectionController(IUserService userService,
            ITermService termService,
            IConfigurationService configurationService,
            IAcademicYearCourseService academicYearCourseService,
            IStudentSectionService studentSectionService,
            IWebHostEnvironment hostingEnvironment,
            ITeacherSectionService teacherSectionService,
            IViewRenderService viewRenderService,
            IConverter dinkConverter,
            ISectionGroupService sectionGroupService,
            ISectionService sectionService,
            IOptionsSnapshot<ReportSettings> reportSettings
        ) : base(userService, termService)
        {
            _academicYearCourseService = academicYearCourseService;
            _studentSectionService = studentSectionService;
            _configurationService = configurationService;
            _hostingEnvironment = hostingEnvironment;
            _teacherSectionService = teacherSectionService;
            _viewRenderService = viewRenderService;
            _dinkConverter = dinkConverter;
            _sectionGroupService = sectionGroupService;
            _sectionService = sectionService;
            _reportSettings = reportSettings.Value;
        }

        /// <summary>
        /// Obtiene el listado de secciones asignadas al docente logueado.
        /// Secciones habilitdas en el periodo activo.
        /// </summary>
        /// <returns>Listado de secciones</returns>
        [Route("get")]
        public async Task<IActionResult> GetSections()
        {
            var userId = GetUserId();
            var term = await GetActiveTerm();
            if (term == null)
                term = new ENTITIES.Models.Enrollment.Term();
            var sections = await _sectionService.GetAll(userId, null, term.Id);

            var result = sections.Select(x => new
            {
                id = x.Id,
                teacherId = x.TeacherSections.Select(ts => ts.TeacherId),
                code = x.Code,
                courseTerm = new
                {
                    courseId = x.CourseTerm.CourseId,
                    term = new
                    {
                        status = x.CourseTerm.Term.Status
                    }
                }
            }).Distinct()
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Genera un reporte que contiene el listado de estudiantes matriculados
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("{sectionId}/matriculados/pdf")]
        public async Task<IActionResult> SectionEnrolledReportPDF(Guid sectionId, Guid? sectionGroupId)
        {
            var sectionCourse = await _sectionService.GetWithIncludes(sectionId);
            var teachersSection = await _teacherSectionService.GetListBySectionId(sectionId);
            var academicYearCourses = await _academicYearCourseService.GetAllAcademicYearCoursesByCourseId(sectionCourse.CourseTerm.CourseId);

            var SectionDetail = new SectionViewModel
            {
                Term = sectionCourse.CourseTerm.Term.Name,
                Teachers = teachersSection.Where(x => x.Teacher != null).Select(x => x.Teacher.User.FullName).ToList(),
                Faculty = sectionCourse.CourseTerm.Course.Career.Faculty.Name,
                Career = sectionCourse.CourseTerm.Course.Career.Name,
                Course = $"{sectionCourse.CourseTerm.Course.Code} - {sectionCourse.CourseTerm.Course.Name}",
                Modality = sectionCourse.CourseTerm.Term.IsSummer ? "NIVELACIÓN" : "REGULAR",
                Section = sectionCourse.Code,
                Credits = sectionCourse.CourseTerm.Course.Credits.ToString(),
                Level = "",
                Semester = string.Join(", ", academicYearCourses.Select(x => (ConstantHelpers.ACADEMIC_YEAR.TEXT.ContainsKey(x.AcademicYear) ? ConstantHelpers.ACADEMIC_YEAR.TEXT[x.AcademicYear].ToUpper() : "Sin Asignar")))
            };

            if (sectionGroupId.HasValue)
            {
                var sectionGroup = await _sectionGroupService.Get(sectionGroupId.Value);
                SectionDetail.SectionGroupName = sectionGroup.Code;
            }

            var stundetSections = await _studentSectionService.GetAllBySectionIdWithIncludes(sectionId);

            if (sectionGroupId.HasValue)
                stundetSections = stundetSections.Where(x => x.SectionGroupId == sectionGroupId).ToList();

            var enrolledStudents = stundetSections
                .Select(x => new EnrolledStudentViewModel
                {
                    UserName = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                    Try = x.Try,
                    Modality = GetModalityName(x.Try)
                })
                .OrderBy(x => x.FullName)
                .ToList();

            var model = new SectionEnrolledReportPDFViewModel
            {
                SectionDetail = SectionDetail,
                Image = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png"),
                EnrolledStudents = enrolledStudents,
                HeaderText = _reportSettings.SectionEnrolledReportPDF
            };


            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = $"REPORTE DE MATRICULADOS POR SECCIÓN"
            };
            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Views/Section/Pdf/SectionEnrolledReportPDF.cshtml", model);
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
            return File(fileByte, "application/pdf");
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet("{sectionId}/registro-avance/pdf")]
        public async Task<IActionResult> SectionProgressReportPDF(Guid sectionId)
        {
            var userId = GetUserId();

            var template = await _sectionService.GetSectionProgressReportTemplate(sectionId, userId);

            var model = new SectionProgressReportPDF
            {
                Image = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png"),
                HeaderText = _reportSettings.SectionEnrolledReportPDF,
                Career = template.Career,
                AcademicDepartment = template.AcademicDepartment,
                Teacher = template.Teacher,
                Course = template.Course,
                Term = template.Term,
                Curriculum = template.Curriculum,
                AcademicYear = template.AcademicYear,
                Section = template.Section,
                Enrolled = template.Enrolled,
                HT = template.HT,
                HP = template.HP,
                Progress = template.Progress,
                Details = template.Details.Select(x=> new SectionProgressDetailReportPDF
                {
                    Date = x.Date,
                    Observation = x.Observation,
                    Students = x.Students,
                    Subject = x.Subject,
                    SessionType = x.SessionType
                }).ToList()
            };

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = $"REGISTRO DE AVANCE DE CURSOS"
            };

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Views/Section/Pdf/SectionProgressReportPDF.cshtml", model);
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
            return File(fileByte, "application/pdf");
        }

        /// <summary>
        /// Obtiene el nombre de la modalidad
        /// </summary>
        /// <param name="try">Número de intento</param>
        /// <returns>Nombre de la modalidad</returns>
        private string GetModalityName(int @try)
        {
            switch (@try)
            {
                case 0:
                    return "REGULAR";
                case 1:
                    return "REGULAR";
                case 2:
                    return "REGULAR";
                case 3:
                    return "TERCERA MAT.";
                case 4:
                    return "CUARTA MAT.";
                case 5:
                    return "QUINTA MAT.";
                case 6:
                    return "SEXTA MAT.";
                case 7:
                    return "SEPTIMA MAT.";
                case 8:
                    return "OCTAVA MAT.";
                case 9:
                    return "NOVENA MAT.";
                case 10:
                    return "DÉCIMA MAT.";
                default:
                    return string.Empty;
            }
        }
    }
}
