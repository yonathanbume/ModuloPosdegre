using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.PDF.Models;
using AKDEMIC.PDF.Services.CompleteCurriculumGenerator.Models;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.CompleteCurriculumGenerator
{
    public class CompleteCurriculumGeneratorService : ICompleteCurriculumGeneratorService
    {
        private readonly AkdemicContext _context;
        private readonly IViewRenderService _viewRenderService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConverter _converter;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITextSharpService _textSharpService;

        public CompleteCurriculumGeneratorService(
            AkdemicContext context,
            IViewRenderService viewRenderService,
            IWebHostEnvironment webHostEnvironment,
            IConverter converter,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            ITextSharpService textSharpService
            )
        {
            _context = context;
            _viewRenderService = viewRenderService;
            _webHostEnvironment = webHostEnvironment;
            _converter = converter;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _textSharpService = textSharpService;
        }

        public async Task<Result> GetCompleteCurriculumPDF(Guid studentId, Guid? userProcedureId)
        {
            var userLoggedIn = await _userService.GetUserByClaim(_httpContextAccessor.HttpContext.User);
            var userProcedure = await _context.UserProcedures.Where(x => x.Id == userProcedureId).FirstOrDefaultAsync();

            var student = await _context.Students.Where(x => x.Id == studentId)
              .Select(x => new
              {
                  x.Id,
                  x.User.UserName,
                  x.User.FullName,
                  Career = x.Career.Name,
                  Faculty = x.Career.Faculty.Name,
                  Campus = x.Campus.Name,
                  AdmissionTerm = x.AdmissionTerm.Name,
                  Curriculum = x.Curriculum.Name,
                  x.CurriculumId,
              })
              .FirstOrDefaultAsync();

            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == studentId && x.Approved)
                .Select(x => new
                {
                    x.CourseId,
                    x.Grade,
                    Term = x.Term.Name,
                    TermNumber = x.Term.Number,
                    TermYear = x.Term.Year
                })
                .ToListAsync();

            var courses = await _context.AcademicYearCourses.Where(x => x.CurriculumId == student.CurriculumId)
                .Select(x => new
                {
                    x.Id,
                    x.AcademicYear,
                    x.Course.Code,
                    x.Course.Name,
                    x.Course.Credits
                })
                .ToListAsync();

            var lastWeightedAverageGrade = await _context.AcademicSummaries.Where(x => x.StudentId == studentId).OrderByDescending(x => x.Term.Year).ThenByDescending(x => x.Term.Year).Select(x => x.WeightedAverageGrade).FirstOrDefaultAsync();

            var model = new CompleteCurriculumModel
            {
                LogoImg = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                UserLoggedIn = userLoggedIn.UserName,
                UserProcedureCode = userProcedure?.Correlative,
                QrCode = GetImageQR(userProcedure?.RecordHistoryId),
                Student = new StudentModel
                {
                    AdmissionTerm = student.AdmissionTerm,
                    Campus = student.Campus,
                    Curriculum = student.Curriculum,
                    FullName = student.FullName,
                    UserName = student.UserName,
                    Career = student.Career,
                    Faculty = student.Faculty,
                    TotalCredits = courses.Where(x => academicHistories.Select(y => y.CourseId).Contains(x.Id)).Sum(x => x.Credits),
                    LastWeightedAverageCumulative = lastWeightedAverageGrade
                },
                Courses = courses.Select(x => new CourseModel
                {
                    AcademicYear = x.AcademicYear,
                    Code = x.Code,
                    Name = x.Name,
                    Credits = x.Credits,
                    Grade =
                    academicHistories.Any(y => y.CourseId == x.Id) ?
                    academicHistories.Where(y => y.CourseId == x.Id).OrderByDescending(y => y.TermYear).ThenByDescending(y => y.TermNumber).Select(y => y.Grade).FirstOrDefault() : (int?)null,
                    TermName =
                    academicHistories.Any(y => y.CourseId == x.Id) ?
                    academicHistories.Where(y => y.CourseId == x.Id).OrderByDescending(y => y.TermYear).ThenByDescending(y => y.TermNumber).Select(y => y.Term).FirstOrDefault() : string.Empty,
                }).ToList()
            };

            return await GetCompleteCurriculumByFormat(model, 1);
        }

        #region FORMATOS

        private async Task<Result> GetCompleteCurriculumByFormat(CompleteCurriculumModel model, byte formatType)
        {
            switch (formatType)
            {
                default:
                    return await GetCompleteCurriculumDefaultFormat(model);
            }
        }

        private async Task<Result> GetCompleteCurriculumDefaultFormat(CompleteCurriculumModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/CompleteCurriculumGenerator/Pdf/Default/CompleteCurriculum.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 2, Right = 2 },
                    DocumentTitle = "Constancia de Plan de Estudios Completo"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileByte = _converter.Convert(pdf);

            var result = new Result
            {
                PdfName = pdf.GlobalSettings.DocumentTitle,
                Pdf = fileByte
            };

            return result;
        }

        #endregion

        #region Helpers

        private string GetImageQR(Guid? recordId)
        {
            if (!recordId.HasValue)
                return string.Empty;

            //QR GENERATOR
            var qrGenerator = new QRCodeGenerator();
            var intranetSchema = ConstantHelpers.Solution.Routes[ConstantHelpers.GENERAL.Institution.Value][ConstantHelpers.Solution.Intranet];
            var URLAbsolute = Path.Combine(intranetSchema, "verificar-documento", "contancias");
            URLAbsolute = $"{URLAbsolute}?id={recordId}";
            var qrCodeData = qrGenerator.CreateQrCode(URLAbsolute, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCoder.PngByteQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(5);
            var finalQR = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(qrCodeImage));
            return finalQR;
        }

        #endregion
    }
}
