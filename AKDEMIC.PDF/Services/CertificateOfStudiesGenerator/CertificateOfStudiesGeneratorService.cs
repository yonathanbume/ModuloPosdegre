using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.PDF.Models;
using AKDEMIC.PDF.Services.CertificateOfStudiesGenerator.Models;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DinkToPdf.Contracts;
using DocumentFormat.OpenXml.Office.CustomUI;
using Flurl.Http.Content;
using iTextSharp.text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.CertificateOfStudiesGenerator
{
    public class CertificateOfStudiesGeneratorService : ICertificateOfStudiesGeneratorService
    {
        private readonly AkdemicContext _context;
        private readonly IConfigurationService _configurationService;
        private readonly IViewRenderService _viewRenderService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConverter _converter;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITextSharpService _textSharpService;

        public CertificateOfStudiesGeneratorService(
            AkdemicContext context,
            IConfigurationService configurationService,
            IViewRenderService viewRenderService,
            IWebHostEnvironment webHostEnvironment,
            IConverter converter,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            ITextSharpService textSharpService
            )
        {
            _context = context;
            _configurationService = configurationService;
            _viewRenderService = viewRenderService;
            _webHostEnvironment = webHostEnvironment;
            _converter = converter;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _textSharpService = textSharpService;
        }

        public async Task<Result> GetCertificateOfStudiesPDF(Guid studentId, Guid? userProcedureId, Guid? recordHistoryId = null)
        {
            var userLoggedIn = await _userService.GetUserByClaim(_httpContextAccessor.HttpContext.User);
            var userProcedure = await _context.UserProcedures.Where(x => x.Id == userProcedureId).FirstOrDefaultAsync();

            var student = await _context.Students.Where(x => x.Id == studentId)
                .Select(x => new
                {
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    x.Career.FacultyId,
                    x.User.FullName,
                    x.User.UserName,
                    CareerCode = x.Career.Code,
                    Curriculum = x.Curriculum.Code,
                    CurriculumYear = x.Curriculum.Year,
                    x.CurriculumId,
                    x.User.Document,
                    AcademicProgram = x.AcademicProgram.Name,
                    x.AcademicProgramId,
                    Campus = x.Campus.Name,
                    RequiredCredits = x.Curriculum.RequiredCredits + x.Curriculum.ElectiveCredits,
                    Surnames = $"{x.User.PaternalSurname} {x.User.MaternalSurname}",
                    x.User.Name
                })
                .FirstOrDefaultAsync();

            var query = _context.AcademicYearCourses.AsNoTracking();
            var lastAcademiCSummary = await _context.AcademicSummaries.Where(y => y.StudentId == studentId)
                .OrderByDescending(x => x.Term.Year).ThenByDescending(x => x.Term.Number).FirstOrDefaultAsync();

            var academicYearCourses = await query.Where(x => x.CurriculumId == student.CurriculumId)
                .Select(x => new
                {
                    x.AcademicYear,
                    x.CourseId,
                    x.IsElective
                })
                .ToListAsync();

            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == studentId && academicYearCourses.Select(y => y.CourseId).Contains(x.CourseId) && x.Approved)
                .Select(x => new
                {
                    x.CourseId,
                    x.Course.Code,
                    x.Course.Name,
                    x.Course.Credits,
                    x.Grade,
                    x.Observations,
                    Term = x.Term.Name,
                    x.TermId,
                    TermNumber = x.Term.Number,
                    x.Validated,
                    TermYear = x.Term.Year,
                    x.Approved,
                    x.EvaluationReport,
                    x.Type
                })
                .ToListAsync();

            var studentObservations = await _context.StudentObservations.Where(x => x.StudentId == studentId)
                .Select(x => new
                {
                    x.Id,
                    x.Type,
                    Term = x.Term.Name,
                    x.Observation
                })
                .ToListAsync();

            var deanFaculty = await _context.Faculties.Where(x => x.Id == student.FacultyId)
                .Select(x => new
                {
                    Dean = x.Dean.FullName,
                    x.DeanGrade
                })
                .FirstOrDefaultAsync();

            var model = new CertificateOfStudiesModel
            {
                //HeaderText = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.DOCUMENT_HEADER_TEXT),
                //SubHeaderText = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.DOCUMENT_SUBHEADER_TEXT),
                UserProcedureCode = userProcedure?.Correlative,
                Img = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                Student = new StudentModel
                {
                    Photo = userProcedure?.UrlImage,
                    WeightedAverageGrade = lastAcademiCSummary?.WeightedAverageGrade ?? 0,
                    WeightedAverageCumulative = lastAcademiCSummary?.WeightedAverageCumulative ?? 0,
                    AcademicProgram = student.AcademicProgram,
                    Career = student.Career,
                    Curriculum = student.Curriculum,
                    CurriculumYear = student.CurriculumYear.ToString(),
                    Faculty = student.Faculty,
                    RequiredCredits = student.RequiredCredits,
                    FullName = student.FullName,
                    Surnames = student.Surnames,
                    Names = student.Name,
                    UserName = student.UserName,
                    Document = student.Document,
                    Campus = student.Campus,
                    //PPG = academicHistories.Any() ? academicHistories.Where(x=>x.Approved).Sum(x=>x.Credits * x.Grade) / academicHistories.Where(x => x.Approved).Sum(x => x.Credits) : 0M,
                    StudentObservations = studentObservations.Select(y => new StudentObservationModel
                    {
                        Id = y.Id,
                        Observation = y.Observation,
                        Term = y.Term,
                        Type = y.Type
                    }).ToList()
                },
                University = new UniversityInformationModel
                {
                    Address = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.INSTITUTION_ADDRESS),
                    PhoneNumber = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.INSTITUTION_PHONENUMBER),
                    Campus = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_MAIN_CAMPUS),
                    HeaderOffice = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_MAIN_OFFICE),
                    Office = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_OFFICE),
                    Sender = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_SENDER_COORDINATOR),
                    WebSite = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.INSTITUTION_WEBSITE),
                    AcademicRecordSigning = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.ACADEMIC_RECORD_SIGNING),
                    DeanFullName = string.IsNullOrEmpty(deanFaculty.DeanGrade) ? $"{deanFaculty.Dean}" : $"{deanFaculty.DeanGrade}. {deanFaculty.Dean}",
                    CurrentLocalDateTime = DateTime.UtcNow.ToDefaultTimeZone()
                },
                AcademicYearCourses = new List<AcademicYearCourseModel>()
            };

            var evaluationReportDateConfi = Convert.ToByte(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE));

            foreach (var item in academicYearCourses.OrderBy(x => x.AcademicYear).GroupBy(x => x.AcademicYear).ToList())
            {
                var courses = item.Select(x => x).ToList();
                var academicHistoriesByAcademicYear = academicHistories.Where(x => courses.Select(y => y.CourseId).Contains(x.CourseId)).ToList();

                var academicYearCourseModel = new AcademicYearCourseModel
                {
                    AcademicYear = item.Key,
                    Courses = academicHistoriesByAcademicYear.GroupBy(x => x.CourseId)
                    .Select(x => new CourseModel
                    {
                        Code = x.Select(y => y.Code).FirstOrDefault(),
                        Type = x.Select(y => y.Type).FirstOrDefault(),
                        Credits = x.Select(y => y.Credits).FirstOrDefault(),
                        IsElective = courses.Where(y => y.CourseId == x.Select(z => z.CourseId).FirstOrDefault()).Select(y => y.IsElective).FirstOrDefault(),
                        Name = x.Select(y => y.Name).FirstOrDefault(),
                        Observation = x.ToList().OrderByDescending(y => y.TermYear).ThenByDescending(y => y.TermNumber).ThenByDescending(y => y.Grade).Select(y => y.Observations).FirstOrDefault(),
                        Grade = x.ToList().OrderByDescending(y => y.TermYear).ThenByDescending(y => y.TermNumber).ThenByDescending(y => y.Grade).Select(y => y.Grade).FirstOrDefault(),
                        Term = x.ToList().OrderByDescending(y => y.TermYear).ThenByDescending(y => y.TermNumber).ThenByDescending(y => y.Grade).Select(y => y.Term).FirstOrDefault(),
                        Validated = x.ToList().OrderByDescending(y => y.TermYear).ThenByDescending(y => y.TermNumber).ThenByDescending(y => y.Grade).Select(y => y.Validated).FirstOrDefault(),
                        DateByConfiguration =
                        x.ToList().OrderByDescending(y => y.TermYear).ThenByDescending(y => y.TermNumber).ThenByDescending(y => y.Grade).Select(y => y.EvaluationReport).FirstOrDefault() == null ? null : (
                        evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.CreatedAt ?
                        x.ToList().OrderByDescending(y => y.TermYear).ThenByDescending(y => y.TermNumber).ThenByDescending(y => y.Grade).Select(y => y.EvaluationReport.CreatedAt).FirstOrDefault()
                        : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.ReceptionDate ?
                        x.ToList().OrderByDescending(y => y.TermYear).ThenByDescending(y => y.TermNumber).ThenByDescending(y => y.Grade).Select(y => y.EvaluationReport.ReceptionDate).FirstOrDefault()
                        : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.LastGradePublished ?
                        x.ToList().OrderByDescending(y => y.TermYear).ThenByDescending(y => y.TermNumber).ThenByDescending(y => y.Grade).Select(y => y.EvaluationReport.LastGradePublishedDate).FirstOrDefault()
                        :
                        null
                        )

                    })
                    .OrderBy(x => x.Code)
                    .ToList(),
                };

                model.AcademicYearCourses.Add(academicYearCourseModel);
            }

            if (model.AcademicYearCourses != null && model.AcademicYearCourses.Any())
            {
                var sumCredits = model.AcademicYearCourses.Sum(x => x.Courses.Sum(y => y.Credits));
                var sumGradeXCredit = model.AcademicYearCourses.Sum(x => x.Courses.Sum(y => y.Credits * y.Grade));

                model.Student.PPG = sumCredits != 0 ? Math.Round((sumGradeXCredit / sumCredits), 2, MidpointRounding.AwayFromZero) : 0;
            }

            if (recordHistoryId.HasValue)
            {
                var recordHistoy = await _context.RecordHistories.Where(x => x.Id == recordHistoryId).FirstOrDefaultAsync();
                if (recordHistoy != null && recordHistoy.Type == ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIESPARTIAL && !string.IsNullOrEmpty(recordHistoy.JsonAcademicYears))
                {
                    var academicYearsFilter = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(recordHistoy.JsonAcademicYears);
                    model.AcademicYearCourses = model.AcademicYearCourses.Where(x => academicYearsFilter.Contains(x.AcademicYear)).ToList();
                }

                if (string.IsNullOrEmpty(model.UserProcedureCode))
                    model.UserProcedureCode = recordHistoy.Code;

                model.ImgQR = GetImageQR(recordHistoryId);
            }

            var certificateOfStudiesConfiguration = Convert.ToByte(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.RecordFormat.CERTIFICATEOFSTUDIES));

            return await GetCertificateOfStudiesByFormat(model, certificateOfStudiesConfiguration);
        }

        #region FORMATOS

        private async Task<Result> GetCertificateOfStudiesByFormat(CertificateOfStudiesModel model, byte formatType)
        {
            var result = new Result();

            switch (formatType)
            {
                case 1:
                    result = await GetCertificateOfStudiesFormat1(model);
                    break;
                case 2: //UNAPI
                    result = await GetCertificateOfStudiesFormat2(model);
                    break;
                case 3: //UNAH
                    result = await GetCertificateOfStudiesFormat3(model);
                    break;
                case 4: //UNSCH
                    result = await GetCertificateOfStudiesFormat4(model);
                    break;
                case 5: //UNAJMA
                    result = await GetCertificateOfStudiesFormat5(model);
                    break;
                case 6: //UNISCJSA
                    result = await GetCertificateOfStudiesFormat6(model);
                    break;
                case 7: //UNJBG
                    result = await GetCertificateOfStudiesFormat7(model);
                    break;
                case 8: //UIGV
                    result = await GetCertificateOfStudiesFormat7(model);
                    break;
                case 9: //UNIFSLB
                    result = await GetCertificateOfStudiesFormat9(model);
                    break;
                default:
                    result = await GetCertificateOfStudiesDefaultFormat(model);
                    break;
            }

            var confiImageWatermark = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IMAGE_WATERMARK_RECORD);

            if (!string.IsNullOrEmpty(confiImageWatermark))
            {
                var bytes = result.Pdf;
                _textSharpService.AddImageWatermarkToAllPages(ref bytes, $"{GeneralHelpers.GetApplicationRoute(ConstantHelpers.Solution.Intranet, false)}/imagenes/{confiImageWatermark}");
                result.Pdf = bytes;
            }

            return result;
        }
        private async Task<Result> GetCertificateOfStudiesDefaultFormat(CertificateOfStudiesModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/CertificateOfStudiesGenerator/Pdf/Default/CertificateOfStudies.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 18, Left = 10, Right = 10 },
                    DocumentTitle = "Certificado de Estudios"
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

            //if (!_httpContextAccessor.HttpContext.User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF) && !_httpContextAccessor.HttpContext.User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD))
            //{
            //    if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAB && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAH && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNIFSLB)
            //        _textSharpService.AddWatermarkToAllPages(ref fileByte, "Solo Lectura", 130);
            //}

            var result = new Result
            {
                PdfName = pdf.GlobalSettings.DocumentTitle,
                Pdf = fileByte
            };

            return result;
        }
        private async Task<Result> GetCertificateOfStudiesFormat1(CertificateOfStudiesModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/CertificateOfStudiesGenerator/Pdf/Format1/CertificateOfStudies.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 400,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Certificado de Estudios"
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
        private async Task<Result> GetCertificateOfStudiesFormat2(CertificateOfStudiesModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/CertificateOfStudiesGenerator/Pdf/Format2/CertificateOfStudies.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 18, Left = 10, Right = 10 },
                    DocumentTitle = "Certificado de Estudios"
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
                                Center = "Pág: [page]/[toPage]",
                                Right = ""
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
        private async Task<Result> GetCertificateOfStudiesFormat3(CertificateOfStudiesModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/CertificateOfStudiesGenerator/Pdf/Format3/CertificateOfStudies.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 5, Right = 5 },
                    DocumentTitle = "Certificado de Estudios"
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
                                Right = "Pág: [page] de [toPage]",
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
        private async Task<Result> GetCertificateOfStudiesFormat4(CertificateOfStudiesModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/CertificateOfStudiesGenerator/Pdf/Format4/CertificateOfStudies.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 10, Left = 5, Right = 5 },
                    DocumentTitle = "Certificado de Estudios"
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
                                FontSize = 7,
                                Line = false,
                                Left = "Toda raspadura o inmadura invalida el presente certificado.",
                                Center = "Pág: [page] de [toPage]",
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
        private async Task<Result> GetCertificateOfStudiesFormat5(CertificateOfStudiesModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/CertificateOfStudiesGenerator/Pdf/Format5/CertificateOfStudies.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 5, Right = 5 },
                    DocumentTitle = "Certificado de Estudios"
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
                                Right = "Pág: [page] de [toPage]",
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
        private async Task<Result> GetCertificateOfStudiesFormat6(CertificateOfStudiesModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/CertificateOfStudiesGenerator/Pdf/Format6/CertificateOfStudies.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 45, Left = 5, Right = 5 },
                    DocumentTitle = "Certificado de Estudios"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        },

                    }
                }
            };

            var htmlSignature = await _viewRenderService.RenderToStringAsync("/Services/CertificateOfStudiesGenerator/Pdf/Format6/Signature.cshtml", model);
            var pdfSignature = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Certificado de Estudios",
                    DPI = 300
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = htmlSignature,
                        WebSettings =
                        {
                            DefaultEncoding = "utf-8"
                        },
                              FooterSettings = {
                                FontName = "Arial",
                                FontSize = 9,
                                Line = false,
                                Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                            }
                    }
                }
            };

            var bytesPDF = _converter.Convert(pdf);
            var bytesSignature = _converter.Convert(pdfSignature);

            var resultPdf = _textSharpService.AddHeaderToAllPages(bytesPDF, bytesSignature);

            resultPdf = _textSharpService.AddPagination(resultPdf, BaseColor.BLACK, 9, false, 540, 6, 0, 1);

            var result = new Result
            {
                PdfName = pdf.GlobalSettings.DocumentTitle,
                Pdf = resultPdf
            };

            return result;
        }
        private async Task<Result> GetCertificateOfStudiesFormat7(CertificateOfStudiesModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/CertificateOfStudiesGenerator/Pdf/Format7/CertificateOfStudies.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 10, Left = 5, Right = 5 },
                    DocumentTitle = "Certificado de Estudios"
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
                                FontSize = 7,
                                Line = false,
                                Left = "Toda raspadura o inmadura invalida el presente certificado.",
                                Center = "Pág: [page] de [toPage]",
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
        private async Task<Result> GetCertificateOfStudiesFormat9(CertificateOfStudiesModel model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/CertificateOfStudiesGenerator/Pdf/Format9/CertificateOfStudies.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 18, Left = 10, Right = 10 },
                    DocumentTitle = "Certificado de Estudios"
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

            //if (!_httpContextAccessor.HttpContext.User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF) && !_httpContextAccessor.HttpContext.User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD))
            //{
            //    if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAB && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAH && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNIFSLB)
            //        _textSharpService.AddWatermarkToAllPages(ref fileByte, "Solo Lectura", 130);
            //}

            var result = new Result
            {
                PdfName = pdf.GlobalSettings.DocumentTitle,
                Pdf = fileByte
            };

            return result;
        }


        #endregion

        #region HELPERS

        private string GetImageQR(Guid? recordId)
        {
            if (!recordId.HasValue)
                return string.Empty;

            //QR GENERATOR
            var qrGenerator = new QRCodeGenerator();
            var intranetSchema = ConstantHelpers.Solution.Routes.ContainsKey(ConstantHelpers.GENERAL.Institution.Value) ? ConstantHelpers.Solution.Routes[ConstantHelpers.GENERAL.Institution.Value][ConstantHelpers.Solution.Intranet] : string.Empty;
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
