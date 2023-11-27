using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.PDF.Models;
using AKDEMIC.PDF.Services.AcademicRecordGenerator.Models;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DinkToPdf.Contracts;
using iTextSharp.text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.AcademicRecordGenerator
{
    public class AcademicRecordGeneratorService : IAcademicRecordGeneratorService
    {
        private readonly AkdemicContext _context;
        private readonly IViewRenderService _viewRenderService;
        private readonly IStudentService _studentService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConverter _converter;
        private readonly IConfigurationService _configurationService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITextSharpService _textSharpService;

        public AcademicRecordGeneratorService(
            AkdemicContext context,
            IViewRenderService viewRenderService,
            IStudentService studentService,
            IWebHostEnvironment webHostEnvironment,
            IConverter converter,
            IConfigurationService configurationService,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            ITextSharpService textSharpService
            )
        {
            _context = context;
            _viewRenderService = viewRenderService;
            _studentService = studentService;
            _webHostEnvironment = webHostEnvironment;
            _converter = converter;
            _configurationService = configurationService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _textSharpService = textSharpService;
        }

        public async Task<Result> GetAcademicRecordPDF(Guid studentId, Guid? userProcedureId, Guid? recordHistoryId = null)
        {
            var userLoggedIn = await _userService.GetUserByClaim(_httpContextAccessor.HttpContext.User);
            var userProcedure = await _context.UserProcedures.Where(x => x.Id == userProcedureId).FirstOrDefaultAsync();
            var recordHistory = await _context.RecordHistories.Where(x => x.Id == recordHistoryId).FirstOrDefaultAsync();

            var student = await _context.Students.Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.Id,
                    x.User.UserName,
                    x.User.FullName,
                    Career = x.Career.Name,
                    Campus = x.Campus.Name,
                    x.CurriculumId,
                    CoursesId = x.Curriculum.AcademicYearCourses.Select(y => y.CourseId).ToArray(),
                    Faculty = x.Career.Faculty.Name
                })
                .FirstOrDefaultAsync();

            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == student.Id && student.CoursesId.Contains(x.CourseId) && !x.Withdraw)
                .Select(x => new
                {
                    x.CourseId,
                    x.Course.Code,
                    x.Course.Name,
                    ByDeferred = x.Type == ConstantHelpers.AcademicHistory.Types.DEFERRED,
                    x.Course.Credits,
                    IsElective = x.Course.AcademicYearCourses.Where(y => y.CurriculumId == student.CurriculumId && y.CourseId == x.CourseId).Select(y => y.IsElective).FirstOrDefault(),
                    FinalGrade = x.Grade,
                    x.Approved,
                    FinalGradeText = ConstantHelpers.GRADES.TEXT.ContainsKey(x.Grade) ? ConstantHelpers.GRADES.TEXT[x.Grade] : "-",
                    Term = x.Term.Name,
                    x.TermId,
                    TermNumber = x.Term.Number,
                    TermYear = x.Term.Year
                })
                .ToListAsync();

            var lastWeightedAverageGrade = await _context.AcademicSummaries.Where(x => x.StudentId == studentId).OrderByDescending(x => x.Term.Year).ThenByDescending(x => x.Term.Number).Select(x => x.WeightedAverageCumulative).FirstOrDefaultAsync();

            var observations = await _context.StudentObservations.Where(x => x.StudentId == studentId)
                .Select(x => new
                {
                    x.Type,
                    Term = x.Term.Name
                })
                .ToListAsync();

            var reservedTerms = await _context.EnrollmentReservations.Where(x => x.StudentId == studentId)
                 .Select(x => new
                 {
                     Term = x.Term.Name
                 })
                .ToListAsync();

            var creditsApproved = await _studentService.GetApprovedCreditsByStudentId(studentId);

            var model = new AcademicRecordModel
            {
                HeaderText = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.DOCUMENT_HEADER_TEXT),
                SubHeaderText = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.DOCUMENT_SUBHEADER_TEXT),
                Student = new StudentModel
                {
                    Campus = student.Campus,
                    Faculty = student.Faculty,
                    Career = student.Career,
                    FullName = student.FullName,
                    UserName = student.UserName,
                    ApprovedCourses = academicHistories.Where(x=>x.Approved).GroupBy(x=>x.CourseId).Count(),
                    ApprovedCredits = creditsApproved,
                    LastWeightedAverageGrade = lastWeightedAverageGrade,
                    AverageApprovedCredits = academicHistories.Any(y => y.Approved) ? Math.Round(academicHistories.Where(x => x.Approved).Sum(x => x.Credits * x.FinalGrade) / academicHistories.Where(x => x.Approved).Sum(x => x.Credits), 2) : 0,
                    WithdrawalTerms = observations.Where(x => x.Type == ConstantHelpers.OBSERVATION_TYPES.WITHDRAWN).GroupBy(x => x.Term).Select(x => x.Key).ToList(),
                    SuspendedTerms = observations.Where(x => x.Type == ConstantHelpers.OBSERVATION_TYPES.SANCTIONED).GroupBy(x => x.Term).Select(x => x.Key).ToList(),
                    ReservedTerms = reservedTerms.GroupBy(x => x.Term).Select(x => x.Key).ToList(),
                    AbandonedTerms = observations.Where(x => x.Type == ConstantHelpers.OBSERVATION_TYPES.ABANDONMENT).GroupBy(x => x.Term).Select(x => x.Key).ToList(),
                },
                Terms = academicHistories.GroupBy(x => x.TermId)
                .Select(x => new TermModel
                {
                    Id = x.Key,
                    Name = x.Select(y => y.Term).FirstOrDefault(),
                    Number = x.Select(y => y.TermNumber).FirstOrDefault(),
                    Year = x.Select(y => y.TermYear).FirstOrDefault(),
                    Courses = x.Select(y => new CourseModel
                    {
                        Approved = y.Approved,
                        ByDeferred = y.ByDeferred,
                        Code = y.Code,
                        Credits = y.Credits,
                        FinalGrade = y.FinalGrade,
                        FinalGradeText = y.FinalGradeText,
                        IsElective = y.IsElective,
                        Name = y.Name
                    })
                    .OrderBy(x => x.Code)
                    .ToList()
                }).OrderBy(x => x.Year).ThenBy(x => x.Number).ToList(),
                LogoImg = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                UserLoggedIn = userProcedure != null && !string.IsNullOrEmpty(userProcedure.UpdatedBy) ? userProcedure.UpdatedBy : userLoggedIn.UserName,
                UserProcedureCode = userProcedure?.Correlative ?? recordHistory?.Code
            };

            if(recordHistory != null)
                model.QrCode = GetImageQR(recordHistory?.Id);

            if(userProcedure != null)
                model.QrCode = GetImageQR(userProcedure?.RecordHistoryId);

            return await GetAcademicRecordByFormat(model, 1);
        }

        #region FORMATOS

        private async Task<Result> GetAcademicRecordByFormat(AcademicRecordModel model, byte formatType)
        {
            switch (formatType)
            {
                default:
                    return await GetAcademicRecordDefaultFormat(model);
            }
        }

        private async Task<Result> GetAcademicRecordDefaultFormat(AcademicRecordModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/AcademicRecordGenerator/Pdf/Default/AcademicRecord.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 2, Right = 2 },
                    DocumentTitle = "Record Académico"
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

            fileByte = _textSharpService.AddPagination(fileByte, BaseColor.BLACK, 9, false, 536, 737, 0, 4);

            var result = new Result
            {
                PdfName = "Record Académico",
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
