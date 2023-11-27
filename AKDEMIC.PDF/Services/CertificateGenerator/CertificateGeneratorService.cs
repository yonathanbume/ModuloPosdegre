using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.PDF.Models;
using AKDEMIC.PDF.Services.CertificateGenerator.Models;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Implementations;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using DinkToPdf.Contracts;
using DocumentFormat.OpenXml.Office2010.Excel;
using Flurl.Http.Content;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AKDEMIC.PDF.Services.CertificateGenerator
{
    public class CertificateGeneratorService : ICertificateGeneratorService
    {
        private readonly AkdemicContext _context;
        private readonly IViewRenderService _viewRenderService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConverter _converter;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IDocumentFormatService _documentFormatService;
        private readonly IConfigurationService _configurationService;
        private readonly ITextSharpService _textSharpService;

        public CertificateGeneratorService(
            AkdemicContext context,
            IViewRenderService viewRenderService,
            IWebHostEnvironment webHostEnvironment,
            IConverter converter,
            IStudentService studentService,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            ICloudStorageService cloudStorageService,
            IDocumentFormatService documentFormatService,
            IConfigurationService configurationService,
            ITextSharpService textSharpService)
        {
            _context = context;
            _viewRenderService = viewRenderService;
            _webHostEnvironment = webHostEnvironment;
            _converter = converter;
            _studentService = studentService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _cloudStorageService = cloudStorageService;
            _documentFormatService = documentFormatService;
            _configurationService = configurationService;
            _textSharpService = textSharpService;
        }

        public async Task<Result> GeneratePdf(byte recordType, Guid recordHistoryId, Guid? userProcedureId = null)
        {

            #region Modelo Base

            var userLoggedIn = await _userService.GetUserByClaim(_httpContextAccessor.HttpContext.User);
            var recordHistory = await _context.RecordHistories.Where(x => x.Id == recordHistoryId).FirstOrDefaultAsync();
            var userProcedure = await _context.UserProcedures.Where(x => x.Id == userProcedureId)
                .Select(x=> new
                {
                    x.Correlative,
                    x.RecordHistoryId,
                    paymentSerialNumber = x.PaymentId.HasValue ? ( x.Payment.InvoiceId.HasValue ? $"{x.Payment.Invoice.Series}-{x.Payment.Invoice.Number}" : x.Payment.OperationCodeB ) : null
                })
                .FirstOrDefaultAsync();

            var student = await _context.Students.Where(x => x.Id == recordHistory.StudentId)
              .Select(x => new
              {
                  x.Id,
                  x.User.UserName,
                  x.User.Document,
                  x.User.FullName,
                  x.CareerId,
                  Career = x.Career.Name,
                  Faculty = x.Career.Faculty.Name,
                  x.FirstEnrollmentDate,
                  x.User.Sex,
                  x.CurrentAcademicYear,

                  //Admissionterm
                  x.AdmissionTermId,
                  AdmissionType = x.AdmissionType.Name,

                  //FirstEnrollment
                  FirstEnrollmentTerm = x.FirstEnrollmentTermId.HasValue ? x.FirstEnrollmentTerm.Name : null,
                  CountFirstEnrollment = x.StudentSections.Where(y => y.Section.CourseTerm.TermId == x.FirstEnrollmentTermId).Count(),
                  CreditsFirstEnrollment = x.StudentSections.Where(y => y.Section.CourseTerm.TermId == x.FirstEnrollmentTermId).Sum(x => x.Section.CourseTerm.Course.Credits),
                  //

                  //Graduation
                  x.GraduationTerm,
                  //

                  LastAcademicSummary = x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number)
                  .Select(y => new
                  {
                      y.MeritOrder,
                      y.TotalOrder,
                      y.WeightedAverageGrade,
                      Term = y.Term.Name,
                      y.StudentAcademicYear,
                  })
                  .FirstOrDefault()
              })
              .FirstOrDefaultAsync();

            var activeTerm = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();

            var signatureImageUrl = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IMAGE_CERTIFICATE_SIGNATURE);
            var signatureBase64 = string.Empty;

            if (!string.IsNullOrEmpty(signatureImageUrl))
            {
                using (var mem = new MemoryStream())
                {
                    await _cloudStorageService.TryDownload(mem, ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION, signatureImageUrl);
                    signatureBase64 = $"data:image/png;base64, {Convert.ToBase64String(mem.ToArray())}";
                }
            }

            int.TryParse(student.GraduationTerm?.Number, out var graduationTermNumber);

            var model = new CertificateModel
            {
                GeneratedBy = _httpContextAccessor.HttpContext.User.IsInRole(ConstantHelpers.ROLES.STUDENTS) ? null : userLoggedIn.FullName,
                ImageQRBase64 = GetImageQR(userProcedure?.RecordHistoryId ?? recordHistoryId),
                UserProcedureCode = userProcedure?.Correlative,
                RecordType = recordType,
                University = new Models.UniversityInformationModel
                {
                    AcademicRecordSigned = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.ACADEMIC_RECORD_SIGNING),
                    HeaderType = Convert.ToByte(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.CONSTANCY_RECORD_HEADER_TYPE)),
                    HeaderText = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.DOCUMENT_HEADER_TEXT),
                    SubHeaderText = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.DOCUMENT_SUBHEADER_TEXT),
                    LogoImgPath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                    Office = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_OFFICE),
                    SignatuareImgBase64 = signatureBase64,
                    BossPositionRecordSigning = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.BOSS_POSITION_RECORD_SIGNING),
                    Address = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.INSTITUTION_ADDRESS),
                    PhoneNumber = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.INSTITUTION_PHONENUMBER),
                    WebSite = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.INSTITUTION_WEBSITE)
                },
                CurrentTerm = new TermModel
                {
                    Name = activeTerm?.Name
                },
                Student = new StudentModel
                {
                    AcademicYear = student.CurrentAcademicYear,
                    Career = student.Career,
                    Faculty = student.Faculty,
                    FullName = student.FullName,
                    UserName = student.UserName,
                    Document = student.Document,
                    Sex = student.Sex,
                    GraduationTerm = new TermModel
                    {
                        Name = student.GraduationTerm?.Name,
                        Number = graduationTermNumber,
                        Year = student.GraduationTerm?.Year,
                        EndDate = student.GraduationTerm?.EndDate
                    },
                    FirstEnrollment = new TermModel
                    {
                        Name = student.FirstEnrollmentTerm,
                        EnrolledCourses = student.CountFirstEnrollment,
                        EnrolledCredits = student.CreditsFirstEnrollment
                    }
                },
                Payment = new PaymentModel
                {
                    SerialNumber = userProcedure?.paymentSerialNumber
                }
            };

            if (recordHistory.RecordTermId.HasValue)
            {
                var recordTerm = await _context.Terms.Where(x => x.Id == recordHistory.RecordTermId).FirstOrDefaultAsync();
                int.TryParse(recordTerm.Number, out var recordTermNumber);

                var academicSummary = await _context.AcademicSummaries.Where(x => x.StudentId == student.Id && x.TermId == recordHistory.RecordTermId).FirstOrDefaultAsync();

                model.Student.RecordTerm = new TermModel
                {
                    Name = recordTerm.Name,
                    Year = recordTerm.Year,
                    Number = recordTermNumber,
                    EnrollmentStartDate = recordTerm.EnrollmentStartDate,
                    AcademicYear = academicSummary?.StudentAcademicYear,
                    EndDate = recordTerm.EndDate,
                    EnrolledCredits = academicSummary?.TotalCredits ?? 0,
                    MeritOrder = academicSummary?.MeritOrder,
                    StudentStatus = academicSummary?.StudentStatus ?? 2,
                    TotalMeritOrder = academicSummary?.TotalOrder,
                    WeightedAverageGrade = academicSummary?.WeightedAverageGrade,
                };
            }

            if (recordType == ConstantHelpers.RECORDS.PROOFONINCOME)
            {

                var totalPostulants = await _context.Postulants.Where(x => x.ApplicationTermId == student.AdmissionTermId && x.CareerId == student.CareerId).CountAsync();

                model.Student.AdmissionTerm = await _context.Terms.Where(x => x.Id == student.AdmissionTermId)
                    .Select(x => new TermModel
                    {
                        Name = x.Name,
                        Year = x.Year
                    })
                    .FirstOrDefaultAsync();

                model.Student.Postulant = await _context.Postulants.Where(x => x.Document == student.Document && x.ApplicationTermId == student.AdmissionTermId)
                    .Select(x => new PostulantModel
                    {
                        Place = x.OrderMerit,
                        Score = x.FinalScore,
                        Modality = student.AdmissionType,
                        TotalStudents = totalPostulants
                    })
                    .FirstOrDefaultAsync();
            }

            if (recordType == ConstantHelpers.RECORDS.EGRESS)
            {
                model.Student.ApprovedCredits = await _studentService.GetApprovedCreditsByStudentId(student.Id);
            }

            var documentFormat = await _documentFormatService.GetParsedDocumentFormat(recordType, student.Id, recordHistory.RecordTermId);

            if (documentFormat != null)
            {
                model.DocumentFormat = new DocumentFormatModel
                {
                    CustomTitle = documentFormat.Title,
                    CustomContent = documentFormat.Content
                };
            }

            #endregion

            #region PDF

            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/CertificateGenerator/Pdf/Default/Certificate.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = ConstantHelpers.RECORDS.VALUES[recordType]
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


            var confiImageWatermark = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IMAGE_WATERMARK_RECORD);

            if (!string.IsNullOrEmpty(confiImageWatermark))
                _textSharpService.AddImageWatermarkToAllPages(ref fileByte, $"{GeneralHelpers.GetApplicationRoute(ConstantHelpers.Solution.Intranet, false)}imagenes/{confiImageWatermark}");

            #endregion

            var result = new Result
            {
                PdfName = pdf.GlobalSettings.DocumentTitle,
                Pdf = fileByte
            };

            return result;
        }

        #region Helpers

        private string GetImageQR(Guid? recordId)
        {
            if (!recordId.HasValue)
                return string.Empty;

            //QR GENERATOR
            var qrGenerator = new QRCodeGenerator();
            var intranetSchema = ConstantHelpers.Solution.Routes[ConstantHelpers.GENERAL.Institution.Value][ConstantHelpers.Solution.Intranet];
            var URLAbsolute = Path.Combine(intranetSchema, "verificar-documento", "constancias");
            URLAbsolute = $"{URLAbsolute}?id={recordId}";
            var qrCodeData = qrGenerator.CreateQrCode(URLAbsolute, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(5);
            var finalQR = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(qrCodeImage));
            return finalQR;
        }

        #endregion
    }
}
