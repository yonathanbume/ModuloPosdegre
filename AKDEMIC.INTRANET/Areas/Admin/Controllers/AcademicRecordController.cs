using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AKDEMIC.INTRANET.Areas.Admin.Models.AcademicRecordViewModels;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/record_academico")]
    public class AcademicRecordController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IAcademicHistoryService _academicHistoryService;

        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public IConverter _dinkConverter { get; }
        private readonly IViewRenderService _viewRenderService;
        private IWebHostEnvironment _hostingEnvironment;

        public AcademicRecordController(
            IOptions<CloudStorageCredentials> storageCredentials,
            IConverter dinkConverter,
            IViewRenderService viewRenderService,
            IWebHostEnvironment environment,
            IStudentService studentService,
            IAcademicHistoryService academicHistoryService) : base()
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _storageCredentials = storageCredentials;
            _hostingEnvironment = environment;
            _studentService = studentService;
            _academicHistoryService = academicHistoryService;
        }

        #region GENERAR_PDF

        /// <summary>
        /// Genera el record académico
        /// </summary>
        /// <param name="sid">Identificador del estudiante</param>
        /// <returns>Archivo en formato PDF</returns>
        [Route("pdf/{id}")]
        public async Task<IActionResult> DownloadRecordGeneralPdf(Guid sid)
        {
            var student = await _studentService.GetStudentWithCareerAndUser(sid);
            var headBoard = new AcademicRecordGeneralHeadBoard
            {
                FacultyName = student.Career.Faculty.Name,
                CareerName = student.Career.Name,
                CodeFullName = $"{student.User.UserName} - {student.User.FullName}",
                CurrentDate = DateTime.UtcNow.ToShortDateString()
            };

            var academicHistories = await _academicHistoryService.GetAllWithSectionAndTeacherSections(sid);

            var AcademicRecordGeneralDetails = academicHistories.Select(x => new AcademicRecordGeneralViewModel
            {
                TermName = x.Term.Name,
                CourseCode = x.Course.Code,
                CourseName = x.Course.Name,
                SectionCode = x.Section.Code,
                Credits = x.Course.Credits,
                Grade = x.Grade,
                DateEndPeriodAcademy = x.Term.EndDate.ToShortDateString(),
                StatusRecord = "CERRADO",
                TeacherName = string.Join(", ", x.Section.TeacherSections.Select(ts => ts.Teacher.User.FullName).ToList())
            }).ToList();

            headBoard.Details = AcademicRecordGeneralDetails;
            var Settings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 0, Left = 0, Right = 10 },
            };
            var img = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{@GeneralHelpers.GetTheme()}/logo-report.png");
            var viewToString = await _viewRenderService.RenderToStringAsync("/Views/Pdf/CurriculumReport.cshtml", headBoard);
            var cssPtah = Path.Combine(_hostingEnvironment.WebRootPath, @"css/pages/pdf/curriculumreport.css");


            //    var objectSettings = new DinkToPdf.ObjectSettings()
            //    {
            //        PagesCount = true,
            //        HtmlContent = viewToString,
            //        WebSettings =
            //        {
            //            DefaultEncoding = "utf-8",
            //            UserStyleSheet = cssPtah
            //        }
            //    };

            //    var pdf = new DinkToPdf.HtmlToPdfDocument()
            //    {
            //        GlobalSettings = Settings,
            //        Objects = { objectSettings }
            //    };

            //    var fileByte = _dinkConverter.Convert(pdf);

            //    HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            //    return File(fileByte, "application/pdf", "Maya Curricular.pdf");
            return Ok();
        }
        #endregion

    }
}
