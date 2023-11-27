using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.PDF.Models;
using AKDEMIC.PDF.Services.ReportCardGenerator.Models;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.ReportCardGenerator
{
    public class ReportCardGeneratorService : IReportCardGeneratorService
    {
        private readonly AkdemicContext _context;
        private readonly IConfigurationService _configurationService;
        private readonly IViewRenderService _viewRenderService;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConverter _converter;

        public ReportCardGeneratorService(
            AkdemicContext context,
            IConfigurationService configurationService,
            IViewRenderService viewRenderService,
            ICloudStorageService cloudStorageService,
            IWebHostEnvironment webHostEnvironment,
            IConverter converter
            )
        {
            _context = context;
            _configurationService = configurationService;
            _viewRenderService = viewRenderService;
            _cloudStorageService = cloudStorageService;
            _webHostEnvironment = webHostEnvironment;
            _converter = converter;
        }

        public async Task<Result> GetReportCardPDF(Guid studentId, Guid termId, Guid? userProcedureId = null)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Id == termId);

            var student = await _context.Students.Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.Id,
                    x.User.UserName,
                    x.User.FullName,
                    x.CurriculumId,
                    Career = x.Career.Name,
                    Campus = x.Campus.Name,
                    Curriculum = x.Curriculum.Code,
                    AdmissionType = x.AdmissionType.Name,
                    x.CurrentAcademicYear
                })
                .FirstOrDefaultAsync();

            var academicYearCourses = await _context.AcademicYearCourses
                  .Where(x => x.CurriculumId == student.CurriculumId)
                  .Select(x => new
                  {
                      x.CourseId,
                      x.AcademicYear
                  })
                  .ToListAsync();

            var academicSummary = await _context.AcademicSummaries.Where(x => x.StudentId == studentId && x.TermId == termId && x.CurriculumId == student.CurriculumId).FirstOrDefaultAsync();

            var totalCredits = 0.0M;
            var totalGradeCredits = 0.0M;
            var courses = new List<CourseModel>();

            var evaluationReportDateConfi = Convert.ToByte(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE));


            if (term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
            {
                var studentsections = await _context.StudentSections
                                  .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId)
                                  .Select(x => new
                                  {
                                      x.Section.CourseTerm.Course.Credits,
                                      x.Status,
                                      x.FinalGrade,
                                      x.Section.CourseTerm.CourseId,
                                      x.Section.CourseTerm.Course.Name,
                                      x.Section.CourseTerm.Course.Code,
                                      x.Section.IsDirectedCourse,
                                      SectionCode =x.Section.Code
                                  })
                                  .ToListAsync();

                foreach (var studentsection in studentsections)
                {
                    var academicyearCourse = academicYearCourses.FirstOrDefault(x => x.CourseId == studentsection.CourseId);

                    var course = new CourseModel
                    {
                        AcademicYear = academicyearCourse?.AcademicYear,
                        Code = studentsection.Code,
                        Credits = studentsection.Credits,
                        Approved = studentsection.FinalGrade >= term.MinGrade,
                        Withdraw = studentsection.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN,
                        Name = studentsection.Name,
                        Grade = studentsection.FinalGrade,
                        Section = studentsection.SectionCode,
                        Observation = ConstantHelpers.STUDENT_SECTION_STATES.VALUES[studentsection.Status],
                        Type = studentsection.IsDirectedCourse ? ConstantHelpers.AcademicHistory.Types.DIRECTED : ConstantHelpers.AcademicHistory.Types.REGULAR
                    };

                    totalCredits += studentsection.Credits;
                    totalGradeCredits += (studentsection.FinalGrade * studentsection.Credits);

                    courses.Add(course);
                }
            }
            else
            {
                var academicHistories = await _context.AcademicHistories
                                       .Where(x => x.TermId == term.Id && x.StudentId == student.Id && !x.Validated
                                       //&& x.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED && x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION
                                       )
                                       .Select(x => new
                                       {
                                           x.CourseId,
                                           x.Grade,
                                           x.Course.Credits,
                                           x.Approved,
                                           x.Course.Name,
                                           x.Course.Code,
                                           x.Withdraw,
                                           x.Type,
                                           SectionCode = x.Section != null ? x.Section.Code : null,
                                           x.EvaluationReport
                                       })
                                       .ToListAsync();

                foreach (var item in academicHistories)
                {
                    var academicyearCourse = academicYearCourses.FirstOrDefault(x => x.CourseId == item.CourseId);

                    var course = new CourseModel
                    {
                        AcademicYear = academicyearCourse?.AcademicYear,
                        Code = item.Code,
                        Name = item.Name,
                        Credits = item.Credits,
                        Grade = item.Grade,
                        Observation = item.Withdraw ? "Retirado" : item.Approved ? "Aprobado" : "Desaprobado",
                        Approved = item.Approved,
                        Withdraw = item.Withdraw,
                        Type = item.Type,
                        Section = item.SectionCode,
                        Date = evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.CreatedAt ? item.EvaluationReport?.CreatedAt.ToDefaultTimeZone()?.ToString("yyyy-MM-dd") : 
                        evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.ReceptionDate ? item.EvaluationReport?.ReceptionDate.ToDefaultTimeZone()?.ToString("yyyy-MM-dd") : 
                        evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.LastGradePublished ? item.EvaluationReport?.LastGradePublishedDate.ToDefaultTimeZone()?.ToString("yyyy-MM-dd") : null
                    };

                    if (!item.Withdraw)
                    {
                        totalCredits += item.Credits;
                        totalGradeCredits += (item.Grade * item.Credits);
                    }

                    courses.Add(course);
                }
            }

            courses = courses.OrderBy(x => x.AcademicYear).ThenBy(x => x.Code).ToList();

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

            var model = new ReportCardModel
            {
                ImgPathLogo = Path.Combine(_webHostEnvironment.WebRootPath, $"images/themes/{GeneralHelpers.GetTheme()}/logo-report.png"),
                HeaderText = "VICERRECTORADO ACADÉMICO",
                IsProcedure = userProcedureId.HasValue,
                SignatuareImgBase64 = signatureBase64,
                Student = new StudentModel
                {
                    FullName = student.FullName,
                    UserName = student.UserName,
                    Campus = student?.Campus,
                    Career = student.Career,
                    Courses = courses,
                    AdmissionType = student?.AdmissionType,
                    Curriculum = student.Curriculum,
                    AcademicYear = term.Status == ConstantHelpers.TERM_STATES.ACTIVE ? student.CurrentAcademicYear : academicSummary?.StudentAcademicYear,
                    PPA = academicSummary?.WeightedAverageCumulative,
                    PPS = academicSummary?.WeightedAverageGrade
                },
                IsActiveTerm = term.Status == ConstantHelpers.TERM_STATES.ACTIVE,
                Term = term.Name,
            };

            if (userProcedureId.HasValue)
            {
                var userProcedure = await _context.UserProcedures.Where(x => x.Id == userProcedureId).FirstOrDefaultAsync();
                model.UserProcedureCode = userProcedure.Correlative;
                model.QrCode = GetImageQR(userProcedure?.RecordHistoryId);
            }

            if (!model.Student.PPS.HasValue)
            {
                if (totalGradeCredits != 0 && totalCredits != 0)
                    model.Student.PPS = Math.Round(Convert.ToDecimal(totalGradeCredits) / Convert.ToDecimal(totalCredits), 2);
            }

            var reportCardFormat = Convert.ToByte(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.RecordFormat.REPORTCARD));

            return await GetReportCardByFormat(model, reportCardFormat);

        }

        #region FORMATOS

        private async Task<Result> GetReportCardByFormat(ReportCardModel model, byte formatType)
        {
            switch (formatType)
            {
                case 1:
                    return await GetReportCardFormat1(model);

                default:
                    return await GetReportCardDefaultFormat(model);
            }
        }

        private async Task<Result> GetReportCardDefaultFormat(ReportCardModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/ReportCardGenerator/Pdf/Default/ReportCard.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Boleta de Notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        },
                        FooterSettings = {
                            FontName = "Arial",
                            FontSize = 9,
                            Line = false,
                            Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                            Center = "",
                            Right = "Pág: [page]/[toPage]"
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

        private async Task<Result> GetReportCardFormat1(ReportCardModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/ReportCardGenerator/Pdf/Format1/ReportCard.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Boleta de Notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        },
                        FooterSettings = {
                            FontName = "Arial",
                            FontSize = 9,
                            Line = false,
                            Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                            Center = "",
                            Right = "Pág: [page]/[toPage]"
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
