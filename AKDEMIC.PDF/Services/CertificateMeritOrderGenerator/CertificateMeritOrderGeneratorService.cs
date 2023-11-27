using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.PDF.Models;
using AKDEMIC.PDF.Services.CertificateMeritOrderGenerator.Models;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.CertificateMeritOrderGenerator
{
    public class CertificateMeritOrderGeneratorService : ICertificateMeritOrderGeneratorService
    {
        private readonly AkdemicContext _context;
        private readonly IViewRenderService _viewRenderService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConverter _converter;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfigurationService _configurationService;

        public CertificateMeritOrderGeneratorService(
            AkdemicContext context,
            IViewRenderService viewRenderService,
            IWebHostEnvironment webHostEnvironment,
            IConverter converter,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            IConfigurationService configurationService)
        {
            _context = context;
            _viewRenderService = viewRenderService;
            _webHostEnvironment = webHostEnvironment;
            _converter = converter;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _configurationService = configurationService;
        }

        public async Task<Result> GetCertificateMeritOrderPDF(Guid recordHistoryId, Guid? userProcedureId)
        {
            var userLoggedIn = await _userService.GetUserByClaim(_httpContextAccessor.HttpContext.User);
            var userProcedure = await _context.UserProcedures.Where(x => x.Id == userProcedureId).FirstOrDefaultAsync();
            var recordHistory = await _context.RecordHistories.Where(x => x.Id == recordHistoryId).FirstOrDefaultAsync();

            var student = await _context.Students.Where(x => x.Id == recordHistory.StudentId)
              .Select(x => new
              {
                  x.Id,
                  x.User.UserName,
                  x.User.FullName,
                  Career = x.Career.Name,
                  Faculty = x.Career.Faculty.Name,
              })
              .FirstOrDefaultAsync();

            var lastAcademicSummary = await _context.AcademicSummaries.Where(x => x.StudentId == student.Id)
                .OrderByDescending(x => x.Term.Year).ThenByDescending(x => x.Term.Number)
                .Select(x => new
                {
                    x.MeritOrder,
                    x.TotalOrder,
                    x.WeightedAverageGrade,
                    Term = x.Term.Name
                })
                .FirstOrDefaultAsync();

            var model = new CertificateMeritOrderModel
            {
                LogoImg = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                UserLoggedIn = userLoggedIn.UserName,
                HeaderText = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.DOCUMENT_HEADER_TEXT),
                SubHeaderText = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.DOCUMENT_SUBHEADER_TEXT),
                Career = student.Career,
                Faculty = student.Faculty,
                UserName = student.UserName,
                FullName = student.FullName,
                University = new UniversityInformationModel
                {
                    Address = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.INSTITUTION_ADDRESS),
                    PhoneNumber = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.INSTITUTION_PHONENUMBER),
                    Campus = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_MAIN_CAMPUS),
                    WebSite = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.INSTITUTION_WEBSITE)
                },
                HmltContent = recordHistory.HtmlContent,
                UserProcedureCode = userProcedure?.Correlative,
                QrCode = GetImageQR(userProcedure?.RecordHistoryId),
            };

            if (lastAcademicSummary != null)
            {
                model.MeritOrder = lastAcademicSummary.MeritOrder;
                model.TotalMeritOrder = lastAcademicSummary.TotalOrder;
                model.WeightedAverageGrade = lastAcademicSummary.WeightedAverageGrade;
                model.Term = lastAcademicSummary.Term;
            }

            return await GetCertificateMeritOrderByFormat(model, 0);
        }


        #region FORMATOS

        private async Task<Result> GetCertificateMeritOrderByFormat(CertificateMeritOrderModel model, byte formatType)
        {
            switch (formatType)
            {
                default:
                    return await GetCertificateMeritOrderDefaultFormat(model);
            }
        }

        private async Task<Result> GetCertificateMeritOrderDefaultFormat(CertificateMeritOrderModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/CertificateMeritOrderGenerator/Pdf/Default/CertificateMeritOrder.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Constancia de Tercio Superior"
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
