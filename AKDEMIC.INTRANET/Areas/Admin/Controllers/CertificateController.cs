
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels;
using AKDEMIC.INTRANET.Controllers;
using DinkToPdf.Contracts;
using AKDEMIC.CORE.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using AKDEMIC.CORE.Extensions;
using System.Collections.Generic;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AutoMapper;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using Microsoft.Extensions.Options;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/certificado-de-estudios")]
    public class CertificateController : BaseController
    {
        private readonly AppCustomSettings _appConfig;
        private IWebHostEnvironment _hostingEnvironment;
        public IConverter _dinkConverter { get; }
        private readonly IViewRenderService _viewRenderService;
        private readonly IStudentService _studentService;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly IMapper _mapper;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IAcademicYearCourseService _academicYearCourseService;

        public CertificateController(
            IOptions<AppCustomSettings> optionsAccessor,
            AkdemicContext context,
            UserManager<ApplicationUser> userManager, IConverter dinkConverter, IViewRenderService viewRenderService, IWebHostEnvironment environment,
            IStudentService studentService,
            IAcademicSummariesService academicSummariesService,
            IAcademicYearCourseService academicYearCourseService,
            IMapper mapper,
            IAcademicHistoryService academicHistoryService) : base(context, userManager)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
            _studentService = studentService;
            _academicSummariesService = academicSummariesService;
            _mapper = mapper;
            _academicHistoryService = academicHistoryService;
            _academicYearCourseService = academicYearCourseService;

            if (optionsAccessor == null) throw new ArgumentNullException(nameof(optionsAccessor));
            _appConfig = optionsAccessor.Value;
        }

        /// <summary>
        /// Vista donde se gestionan los certificados de estudio
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Genera el certificado de estudios
        /// </summary>
        /// <param name="sid">Identificador del estudiante</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("pdf/{sid}")]
        public async Task<IActionResult> CertificatePDF(Guid sid)
        {
            var universityName = GeneralHelpers.GetInstitutionName();
            var model = await _studentService.GetStudntCertificate(sid, universityName);
            var student = _mapper.Map<HeadBoardCertificateViewModel>(model);

            var bytess = await this.GetFunction<CertificateController, byte[]>(_appConfig.CertificatePDF, sid, student);
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(bytess, "application/pdf", "certificado_de_estudios_" + student.UserName + ".pdf");
        }

        /// <summary>
        /// Genera el certificado de estudios
        /// </summary>
        /// <param name="sid">Identificador del estudiante</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("testpdf/{sid}")]
        public async Task<IActionResult> TestCertificatePDF(Guid sid)
        {
            var term = await GetActiveTerm();
            var student = await _context.Students
                .Include(x => x.User).Include(x => x.Career)
                .FirstOrDefaultAsync(x => x.Id == sid);

            //var academicYears = await _context.AcademicYears
            //    .Include(x => x.AcademicYearCourses).ThenInclude(x => x.Course)
            //    .Where(x => x.CurriculumId == student.CurriculumId)
            //    .OrderBy(x => x.Number)
            //    .ToListAsync();

            var academicYears = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId)
                .Include(x => x.Course)
                .OrderBy(x => x.AcademicYear)
                .GroupBy(x => x.AcademicYear)
                .ToListAsync();

            var model = new UnamCertificateViewModel
            {
                Header = new UnamHeaderCertificateViewModel
                {
                    Number = new Random().Next(1000, 9999).ToString(),
                    Code = student.User.UserName,
                    CareerName = student.Career.Name,
                    CampusName = GeneralHelpers.GetInstitutionLocation(),
                    StudentName = student.User.FullName,
                    UrlToPicture = string.IsNullOrEmpty(student.User.Picture)
                        ? Path.Combine(_hostingEnvironment.WebRootPath, @"\images\demo\user.png")
                        : student.User.Picture
                },
                Today = DateTime.UtcNow.ToDefaultTimeZone(),
                ImagePathLogo = "",
                CurrentWeightedGrade = await _context.AcademicSummaries.Where(x => x.StudentId == student.Id && term != null && x.TermId != term.Id)
                    .OrderByDescending(x => x.Term.StartDate).Select(x => x.WeightedAverageGrade).FirstOrDefaultAsync(),
                MaxPassingGrade = 20,
                MinPassingGrade = term.MinGrade,
                TotalCredits = academicYears.Sum(x => x.Sum(ayc => ayc.Course.Credits)),
                AcademicYears = await Task.WhenAll(academicYears.Select(async x => new UnamAcademicYearCertificateViewModel
                {
                    AcademicYearNumber = x.Key,
                    Courses = await Task.WhenAll(x.Select(async ayc =>
                    {
                        var academicHistory = await _context.AcademicHistories.Include(ah => ah.Term)
                           .Where(ah => ah.CourseId == ayc.CourseId && ah.StudentId == student.Id)
                           .OrderByDescending(ah => ah.Term.StartDate).AsNoTracking().FirstOrDefaultAsync();

                        return new UnamCourseCertificateViewModel
                        {
                            CourseName = ayc.Course.Name,
                            CourseCredits = ayc.Course.Credits,
                            IsElective = ayc.IsElective,
                            TermName = academicHistory?.Term.Name ?? "-",
                            CourseFinalGrade = academicHistory?.Grade ?? 0,
                            Validated = academicHistory?.Validated ?? false,
                            Approved = academicHistory?.Approved ?? false
                        };
                    }))
                }))
            };

            return View("UnamCertificatePDF", model);
        }


        #region PRIVATES

        /// <summary>
        /// Formato 1 del certificado de estudios
        /// </summary>
        /// <param name="sid">Identificador del estudiante</param>
        /// <param name="student">Cabecera del pdf</param>
        /// <returns></returns>
        private async Task<byte[]> StandardFormat(Guid sid, HeadBoardCertificateViewModel student)
        {
            var viewToString = "";
            var cssPath = "";
            var studentCode = "";
            var objectsSettings = new List<DinkToPdf.ObjectSettings>();

            var modelLstCertificate = await _academicHistoryService.GetListCertificateByStudentAndCurriculum(student.IdStudent, student.CurriculumId);
            var lstCertificate = _mapper.Map<List<CertificateViewModel>>(modelLstCertificate);

            CertificateCompleteViewModel certificateComplete = new CertificateCompleteViewModel
            {
                HeaderBoard = student,
                Certificate = lstCertificate,
                ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{@GeneralHelpers.GetTheme()}/logo-report.png"),
                Today = DateTime.UtcNow.ToShortDateString(),
                YearOfStudies = Math.Truncate((decimal)lstCertificate.GroupBy(x => new { x.TermName }).Count() / 2)
            };

            viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/Certificate/CertificatePDF.cshtml", certificateComplete);
            //cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css\pages\pdf\certificatereport.css");
            studentCode = student.UserName;
            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8"/*, UserStyleSheet = cssPath */},
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };
            objectsSettings.Add(objectSettings);

            //return View("/Areas/Admin/Views/Certificate/CertificatePDF.cshtml", certificateComplete);


            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,

                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings
            };
            pdf.Objects.AddRange(objectsSettings);

            return _dinkConverter.Convert(pdf);
        }

        /// <summary>
        /// Formato 2 del certificado de estudios
        /// </summary>
        /// <param name="sid">Identificador del estudiante</param>
        /// <param name="student">Cabecera del pdf</param>
        private async Task<byte[]> ReportFormat1(Guid sid, HeadBoardCertificateViewModel student)
        {
            var viewToString = "";
            var cssPath = "";
            var studentCode = "";
            var objectsSettings = new List<DinkToPdf.ObjectSettings>();

            //var term = await GetActiveTerm();
            ////var student = await _context.Students
            ////    .Include(x => x.User).Include(x => x.Career)
            ////    .FirstOrDefaultAsync(x => x.Id == sid);
            //var student = await _studentService.GetStudentWithCareerAndUser(sid);

            //var academicYears = await _academicYearService.GetAllWithAcademicYearCoursesByCurriculum(student.CurriculumId);
            ////var academicYears = await _context.AcademicYears
            ////    .Include(x => x.AcademicYearCourses).ThenInclude(x => x.Course)
            ////    .Where(x => x.CurriculumId == student.CurriculumId)
            ////    .OrderBy(x => x.Number)
            ////    .ToListAsync();

            //var model = new UnamCertificateViewModel
            //{
            //    Header = new UnamHeaderCertificateViewModel
            //    {
            //        Number = new Random().Next(1, 9999).ToString("D4"),
            //        Code = student.User.UserName,
            //        CareerName = student.Career.Name,
            //        CampusName = GeneralHelpers.GetInstitutionLocation(),
            //        StudentName = student.User.FullName,
            //        UrlToPicture = string.IsNullOrEmpty(student.User.Picture)
            //        ? Path.Combine(_hostingEnvironment.WebRootPath, @"\images\demo\user.png")
            //        : student.User.Picture
            //    },
            //    Today = DateTime.UtcNow.ToDefaultTimeZone(),
            //    ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, @"images\unam\shield.png"),
            //    CurrentWeightedGrade = await _academicSummariesService.GetCurrentWeightedGrade(student.Id, term.Id),
            //    //CurrentWeightedGrade = await _context.AcademicSummaries.Where(x => x.StudentId == student.Id && term != null && x.TermId != term.Id)
            //    //    .OrderByDescending(x => x.Term.StartDate).Select(x => x.WeightedAverageGrade).FirstOrDefaultAsync(),
            //    MaxPassingGrade = 20,
            //    MinPassingGrade = term.MinGrade,
            //    TotalCredits = academicYears.Sum(x => x.AcademicYearCourses.Sum(ayc => ayc.Course.Credits)),
            //    AcademicYears = await Task.WhenAll(academicYears.Select(async x => new UnamAcademicYearCertificateViewModel
            //    {
            //        AcademicYearNumber = x.Number,
            //        Courses = await Task.WhenAll(x.AcademicYearCourses.Select(async ayc =>
            //        {
            //            var academicHistory = await _context.AcademicHistories.Include(ah => ah.Term)
            //               .Where(ah => ah.CourseId == ayc.CourseId && ah.StudentId == student.Id)
            //               .OrderByDescending(ah => ah.Term.StartDate).AsNoTracking().FirstOrDefaultAsync();

            //            return new UnamCourseCertificateViewModel
            //            {
            //                CourseName = ayc.Course.Name,
            //                CourseCredits = ayc.Course.Credits,
            //                IsElective = ayc.IsElective,
            //                TermName = academicHistory?.Term.Name ?? "-",
            //                CourseFinalGrade = academicHistory?.Grade ?? 0,
            //                Validated = academicHistory?.Validated ?? false,
            //                Approved = academicHistory?.Approved ?? false
            //            };
            //        }))
            //    }))
            //};

            //viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/Certificate/UnamCertificatePDF.cshtml", model);
            //cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css\pages\pdf\unamcertificatepdf.css");
            //studentCode = student.User.UserName;
            //var objectSettings = new DinkToPdf.ObjectSettings
            //{
            //    PagesCount = true,
            //    HtmlContent = viewToString,
            //    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPath, EnableJavascript = true },
            //    HeaderSettings = new DinkToPdf.HeaderSettings
            //    {
            //        FontSize = 8,
            //        Spacing = 1,
            //        Right = "Pag: [page]/[toPage]"
            //    },
            //    FooterSettings = new DinkToPdf.FooterSettings
            //    {
            //        FontSize = 8,
            //        Line = true,
            //        Spacing = 1,
            //        Left = $"NOTA APROBATORIA DE {term.MinGrade:#0} A 20 PUNTOS", //Nota:\n(EL) Curso Electivo\nLas enmendaduras invalidan el certificado\n
            //        Center = "(EL) Curso Electivo",
            //        Right = "\\... Continua"
            //    }
            //};
            //objectsSettings.Add(objectSettings);


            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,

                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings
            };
            pdf.Objects.AddRange(objectsSettings);

            return _dinkConverter.Convert(pdf);
        }
        #endregion

    }
}
