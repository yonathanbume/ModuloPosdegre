// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Services.EvaluationReportGenerator.Models;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using DinkToPdf.Contracts;
using iTextSharp.text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PdfSharp.Pdf.Advanced;

namespace AKDEMIC.INTRANET.Services.EvaluationReportGenerator
{
    public class EvaluationReportGeneratorService : IEvaluationReportGeneratorService
    {
        private readonly IEvaluationReportService _evaluationReportService;
        private readonly IConfigurationService _configurationService;
        private readonly IViewRenderService _viewRenderService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConverter _converter;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly ITextSharpService _textSharpService;

        public EvaluationReportGeneratorService(
            IEvaluationReportService evaluationReportService,
            IConfigurationService configurationService,
            IViewRenderService viewRenderService,
            IWebHostEnvironment webHostEnvironment,
            IConverter converter,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            ITextSharpService textSharpService
            )
        {
            _evaluationReportService = evaluationReportService;
            _configurationService = configurationService;
            _viewRenderService = viewRenderService;
            _webHostEnvironment = webHostEnvironment;
            _converter = converter;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _textSharpService = textSharpService;
        }

        public async Task<Result> GetActEvaluationReport(Guid sectionId, int? code = null, string issueDate = null, string receptionDate = null)
        {
            var model = await _evaluationReportService.GetEvaluationReportInformation(sectionId, code, issueDate, receptionDate);
            model.FinalQR = GetBarCode(model.BasicInformation.Code);
            model.DocumentTitle = $"A-{model.Course.Career.Code}-{model.Course.Curriculum}-{model.Course.Code}-{model.BasicInformation.Code}";
            var userLoggedIn = await _userService.GetUserByClaim(_httpContextAccessor.HttpContext.User);
            model.UserLoggedIn = userLoggedIn.UserName;
            model.UserLoggedInFullName = userLoggedIn.FullName;
            model.Img = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png");
            model.ImgNationalEmblem = Path.Combine(_webHostEnvironment.WebRootPath, @"images\escudo.png");

            return await GetActEvaluationReportByFormat(model);
        }

        public async Task<Result> GetActEvaluationReportDeferredExam(Guid deferredExamId)
        {
            var model = await _evaluationReportService.GetEvaluationReportDeferredExamInformation(deferredExamId);
            model.FinalQR = GetBarCode(model.BasicInformation.Code);
            model.DocumentTitle = $"A-{model.Course.Career.Code}-{model.Course.Curriculum}-{model.Course.Code}-{model.BasicInformation.Code}";
            var userLoggedIn = await _userService.GetUserByClaim(_httpContextAccessor.HttpContext.User);
            model.UserLoggedIn = userLoggedIn.UserName;
            model.UserLoggedInFullName = userLoggedIn.FullName;
            model.Img = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png");
            model.ImgNationalEmblem = Path.Combine(_webHostEnvironment.WebRootPath, @"images\escudo.png");

            return await GetActEvaluationReportByFormat(model);
        }

        public async Task<Result> GetActEvaluationReportCorrectionExam(Guid correctionExamId)
        {
            var model = await _evaluationReportService.GetEvaluationReportCorrectionExamInformation(correctionExamId);
            model.FinalQR = GetBarCode(model.BasicInformation.Code);
            model.DocumentTitle = $"A-{model.Course.Career.Code}-{model.Course.Curriculum}-{model.Course.Code}-{model.BasicInformation.Code}";
            var userLoggedIn = await _userService.GetUserByClaim(_httpContextAccessor.HttpContext.User);
            model.UserLoggedIn = userLoggedIn.UserName;
            model.UserLoggedInFullName = userLoggedIn.FullName;
            model.Img = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png");
            model.ImgNationalEmblem = Path.Combine(_webHostEnvironment.WebRootPath, @"images\escudo.png");

            return await GetActEvaluationReportByFormat(model);
        }

        public async Task<Result> GetActEvaluationReportExtraordinaryEvaluation(Guid extraordinaryEvaluationId)
        {
            var model = await _evaluationReportService.GetEvaluationReportExtraordinaryEvaluationInformation(extraordinaryEvaluationId);

            model.FinalQR = GetBarCode(model.BasicInformation.Code);
            model.DocumentTitle = $"A-{model.Course.Career.Code}-{model.Course.Curriculum}-{model.Course.Code}-{model.BasicInformation.Code}";
            var userLoggedIn = await _userService.GetUserByClaim(_httpContextAccessor.HttpContext.User);
            model.UserLoggedIn = userLoggedIn.UserName;
            model.UserLoggedInFullName = userLoggedIn.FullName;
            model.Img = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png");
            model.ImgNationalEmblem = Path.Combine(_webHostEnvironment.WebRootPath, @"images\escudo.png");

            return await GetActEvaluationReportByFormat(model);
        }

        public async Task<Result> GetRegisterEvaluationReport(Guid sectionId)
        {
            var model = await _evaluationReportService.GetEvaluationReportInformation(sectionId, isRegister: true);
            model.FinalQR = GetBarCode(model.BasicInformation.Code);
            model.DocumentTitle = $"R-{model.Course.Career.Code}-{model.Course.Curriculum}-{model.Course.Code}-{model.BasicInformation.Code}";
            var userLoggedIn = await _userService.GetUserByClaim(_httpContextAccessor.HttpContext.User);
            model.UserLoggedIn = userLoggedIn.Name;
            model.UserLoggedInFullName = userLoggedIn.FullName;
            model.Img = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png");
            model.FinalQR = GetBarCode(model.BasicInformation.Code);
            model.ImgNationalEmblem = Path.Combine(_webHostEnvironment.WebRootPath, @"images\escudo.png");

            return await GetEvaluationRegisterReportByFormat(model);
        }

        #region Formats Act

        private async Task<Result> GetActFormat1(EvaluationReportInformationTemplate model)
        {
            var headerHtmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format1/EvaluationReportHeader.cshtml", model);
            var pdfHeader = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 0, Bottom = 0, Left = 5, Right = 5 },
                    DocumentTitle = model.DocumentTitle,
                    DPI = 300
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = headerHtmlContent,
                        WebSettings =
                        {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var bodyHtmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format1/EvaluationReportBody.cshtml", model);

            var pdfBody = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 63, Bottom = 82, Left = 5, Right = 5 },
                    DocumentTitle = model.DocumentTitle,
                    DPI = 300
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = bodyHtmlContent,
                        WebSettings =
                        {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var headerBytesPdf = _converter.Convert(pdfHeader);
            var bodyBytesPdf = _converter.Convert(pdfBody);

            var resultPdf = _textSharpService.AddHeaderToAllPages(bodyBytesPdf, headerBytesPdf);
            resultPdf = _textSharpService.AddPagination(resultPdf);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = resultPdf
            };

            return result;
        }

        private async Task<Result> GetActFormat2(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format2/EvaluationReport.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
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
                            FontSize = 6,
                            Line = false,
                            Left = "DIRECCION UNIVERSITARIA DE ASUNTOS ACADEMICOS",
                            Right = $"Impreso por: {model.UserLoggedIn} {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                            Center ="Página [page]/[toPage]"
                        }
                    }
                }
            };

            var fileByte = _converter.Convert(pdf);
            _textSharpService.AddWatermarkToAllPages(ref fileByte, ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value], 130);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = fileByte
            };

            return result;
        }

        private async Task<Result> GetActFormat3(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format3/EvaluationReport.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
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
                            FontSize = 6,
                            Line = false,
                            Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                            Right = "Página [page] de [toPage]"
                        }
                    }
                }
            };

            var fileByte = _converter.Convert(pdf);
            _textSharpService.AddWatermarkToAllPages(ref fileByte, ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value], 130);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = fileByte
            };

            return result;
        }

        private async Task<Result> GetActFormat4(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format4/EvaluationReport.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 15, Right = 15 },
                    DocumentTitle = "Acta de notas"
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
                            FontSize = 6,
                            Line = false,
                            Center = "Página [page] de [toPage]",
                        }
                    }
                }
            };

            var fileByte = _converter.Convert(pdf);
            //_textSharpService.AddWatermarkToAllPages(ref fileByte, ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value], 130);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = fileByte
            };

            return result;
        }

        private async Task<Result> GetActFormat5(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format5/EvaluationReport.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 400,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileByte = _converter.Convert(pdf);
            fileByte = _textSharpService.RemoveEmptyPages(fileByte, 30500);
            fileByte = _textSharpService.AddPagination(fileByte, BaseColor.BLACK, 9, true, 515, 755, 0, 2);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = fileByte
            };

            return result;
        }

        private async Task<Result> GetActFormat6(EvaluationReportInformationTemplate model)
        {
            var htmlBackground = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format6/EvaluationReportBackground.cshtml", model);
            var pdfBackground = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlBackground,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format6/EvaluationReport.cshtml", model);
            var pdfContent = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 400,
                    Margins = new DinkToPdf.MarginSettings { Top = 75, Bottom = 30, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileBackground = _converter.Convert(pdfBackground);
            var fileContent = _converter.Convert(pdfContent);

            var resultPdf = _textSharpService.AddHeaderToAllPages(fileContent, fileBackground);
            resultPdf = _textSharpService.AddPagination(resultPdf, BaseColor.BLACK, 9, false, 25, 50, 0, 2);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = resultPdf
            };

            return result;
        }

        private async Task<Result> GetActFormat7(EvaluationReportInformationTemplate model)
        {
            var htmlBackground = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format7/EvaluationReportBackground.cshtml", model);
            var pdfBackground = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 10, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de Evaluación Final"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        FooterSettings =
                        {
                            Left = $"{model.UserLoggedIn}",
                            Right = $"{model.BasicInformation.CreatedAt.Value.ToDefaultTimeZone().ToString("dd-MM-yyyy HH:mm:ss")}",
                            FontSize = 8
                        },
                        HtmlContent = htmlBackground,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format7/EvaluationReport.cshtml", model);
            var pdfContent = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 400,
                    Margins = new DinkToPdf.MarginSettings { Top = 70, Bottom = 30, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de Evaluación Final"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileBackground = _converter.Convert(pdfBackground);
            var fileContent = _converter.Convert(pdfContent);

            var resultPdf = _textSharpService.AddHeaderToAllPages(fileContent, fileBackground);
            resultPdf = _textSharpService.AddPagination(resultPdf, BaseColor.BLACK, 9, false, 275, 20, 0, 3);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = resultPdf
            };

            return result;
        }

        private async Task<Result> GetActFormat8(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format8/EvaluationReport.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de Evaluación Final"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        FooterSettings =
                        {
                            Right = "Página [page] de [toPage]",
                            FontSize = 8
                        },
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
                PdfName = model.DocumentTitle,
                Pdf = fileByte
            };

            return result;
        }

        private async Task<Result> GetActFormat9(EvaluationReportInformationTemplate model)
        {
            var headerHtmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format9/EvaluationReportHeader.cshtml", model);
            var pdfHeader = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 0, Bottom = 0, Left = 29, Right = 5 },
                    DocumentTitle = model.DocumentTitle,
                    DPI = 300
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = headerHtmlContent,
                        WebSettings =
                        {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var bodyHtmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format9/EvaluationReportBody.cshtml", model);

            var pdfBody = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 65, Bottom = 70, Left = 29, Right = 5 },
                    DocumentTitle = model.DocumentTitle,
                    DPI = 300
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = bodyHtmlContent,
                        WebSettings =
                        {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var headerBytesPdf = _converter.Convert(pdfHeader);
            var bodyBytesPdf = _converter.Convert(pdfBody);

            var resultPdf = _textSharpService.AddHeaderToAllPages(bodyBytesPdf, headerBytesPdf);
            resultPdf = _textSharpService.AddPagination(resultPdf, BaseColor.BLACK, 9, true, 532, 749, 0, 1);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = resultPdf
            };

            return result;
        }

        private async Task<Result> GetActFormat11(EvaluationReportInformationTemplate model)
        {
            var htmlBackground = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format11/EvaluationReportBackground.cshtml", model);
            var pdfBackground = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "ACTA FINAL DE EVALUACIÓN"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlBackground,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format11/EvaluationReport.cshtml", model);
            var pdfContent = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 400,
                    Margins = new DinkToPdf.MarginSettings { Top = 70, Bottom = 60, Left = 5, Right = 5 },
                    DocumentTitle = "ACTA FINAL DE EVALUACIÓN"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileBackground = _converter.Convert(pdfBackground);
            var fileContent = _converter.Convert(pdfContent);

            fileContent = _textSharpService.RemoveEmptyPages(fileContent, 1500);

            var resultPdf = _textSharpService.AddHeaderToAllPages(fileContent, fileBackground);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = resultPdf
            };

            return result;
        }

        private async Task<Result> GetActFormat12(EvaluationReportInformationTemplate model)
        {
            var htmlBackground = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format12/EvaluationReportBackground.cshtml", model);
            var pdfBackground = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 400,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlBackground,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format12/EvaluationReport.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 400,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 75, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileBackground = _converter.Convert(pdfBackground);

            var fileContent = _converter.Convert(pdf);
            fileContent = _textSharpService.RemoveEmptyPages(fileContent, 1500);
            fileContent = _textSharpService.AddPagination(fileContent, BaseColor.BLACK, 9, true, 515, 755, 0, 2);

            var resultPdf = _textSharpService.AddHeaderToAllPages(fileContent, fileBackground);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = resultPdf
            };

            return result;
        }

        private async Task<Result> GetActFormat13(EvaluationReportInformationTemplate model)
        {
            var htmlBackground = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format13/EvaluationReportBackground.cshtml", model);
            var pdfBackground = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Landscape,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 72, Bottom = 5, Left = 10, Right = 10 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlBackground,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format13/EvaluationReport.cshtml", model);
            var pdfContent = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Landscape,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 400,
                    Margins = new DinkToPdf.MarginSettings { Top = 6, Bottom = 35, Left = 25, Right = 25 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileBackground = _converter.Convert(pdfBackground);
            var fileContent = _converter.Convert(pdfContent);

            var resultPdf = _textSharpService.AddHeaderToAllPages(fileContent, fileBackground);
            //resultPdf = _textSharpService.AddPagination(resultPdf, BaseColor.BLACK, 9, false, 25, 50, 0, 2);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = resultPdf
            };

            return result;
        }

        private async Task<Result> GetActFormat14(EvaluationReportInformationTemplate model)
        {
            var htmlBackground = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format14/EvaluationReportBackground.cshtml", model);
            var pdfBackground = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 400,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 6.5, Right = 6.5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlBackground,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format14/EvaluationReport.cshtml", model);
            var pdfContent = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 400,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 70, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileBackground = _converter.Convert(pdfBackground);
            var fileContent = _converter.Convert(pdfContent);


            var resultPdf = _textSharpService.AddHeaderToAllPages(fileContent, fileBackground);
            resultPdf = _textSharpService.AddPagination(resultPdf, BaseColor.BLACK, 8, false, 520, 100, 0, 3);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = resultPdf
            };

            return result;
        }

        private async Task<Result> GetActFormat16(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format16/EvaluationReport.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
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
                            FontSize = 6,
                            Line = false,
                            Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                            Right = "Página [page] de [toPage]"
                        }
                    }
                }
            };

            var fileByte = _converter.Convert(pdf);
            _textSharpService.AddImageWatermarkToAllPages(ref fileByte, $"{GeneralHelpers.GetApplicationRoute(ConstantHelpers.Solution.Intranet, false)}/images/themes/ensdf/logo-marca-agua.jpg");

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = fileByte
            };

            return result;
        }

        #endregion

        #region Formats Register

        private async Task<Result> GetRegisterFormat1(EvaluationReportInformationTemplate model)
        {
            var headerHtmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format1/EvaluationReportRegisterHeader.cshtml", model);
            var pdfHeader = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 0, Bottom = 0, Left = 5, Right = 5 },
                    DocumentTitle = model.DocumentTitle,
                    DPI = 300
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = headerHtmlContent,
                        WebSettings =
                        {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var bodyHtmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format1/EvaluationReportRegisterBody.cshtml", model);

            var pdfBody = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 65, Bottom = 70, Left = 5, Right = 5 },
                    DocumentTitle = model.DocumentTitle,
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = bodyHtmlContent,
                        WebSettings =
                        {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var headerBytesPdf = _converter.Convert(pdfHeader);
            var bodyBytesPdf = _converter.Convert(pdfBody);

            var resultPdf = _textSharpService.AddHeaderToAllPages(bodyBytesPdf, headerBytesPdf);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = resultPdf
            };

            return result;
        }

        private async Task<Result> GetRegisterFormat4(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format4/EvaluationReportRegister.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 15, Right = 15 },
                    DocumentTitle = "Acta de notas"
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
                            FontSize = 6,
                            Line = false,
                            Center = "Página [page] de [toPage]",
                        }
                    }
                }
            };

            var fileByte = _converter.Convert(pdf);
            //_textSharpService.AddWatermarkToAllPages(ref fileByte, ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value], 130);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = fileByte
            };

            return result;
        }

        private async Task<Result> GetRegisterFormat5(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format5/EvaluationReportRegister.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileByte = _converter.Convert(pdf);
            fileByte = _textSharpService.AddPagination(fileByte, BaseColor.BLACK, 9, true, 515, 755, 0, 2);
            fileByte = _textSharpService.RemoveEmptyPages(fileByte, 30100);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = fileByte
            };

            return result;
        }

        private async Task<Result> GetRegisterFormat6(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format6/EvaluationReportRegister.cshtml", model);
            var pdfContent = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Landscape,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 400,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileContent = _converter.Convert(pdfContent);

            fileContent = _textSharpService.AddPagination(fileContent, BaseColor.BLACK, 9, false, 25, 50, 0, 2);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = fileContent
            };

            return result;
        }

        private async Task<Result> GetRegisterFormat7(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format7/EvaluationReportRegister.cshtml", model);
            var pdfContent = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de Evaluación Final"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        },
                        FooterSettings = {
                            FontName = "Arial",
                            FontSize = 6,
                            Line = false,
                            Left = $"{model.UserLoggedIn}",
                            Right = $"{DateTime.UtcNow.ToDefaultTimeZone().ToString("dd-MM-yyyy HH:mm:ss")}",
                            Center = "Página [page] de [toPage]",
                        }
                    }
                }
            };

            var fileContent = _converter.Convert(pdfContent);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = fileContent
            };

            return result;
        }

        private async Task<Result> GetRegisterFormat8(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format8/EvaluationReportRegister.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Landscape,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Registro de Notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        FooterSettings =
                        {
                            Right = "Página [page] de [toPage]",
                            FontSize = 8
                        },
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
                PdfName = model.DocumentTitle,
                Pdf = fileByte
            };

            return result;
        }

        private async Task<Result> GetRegisterFormat10(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format10/EvaluationReportRegister.cshtml", model);
            var pdfContent = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de Evaluación Final",
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        FooterSettings =
                        {
                            FontSize = 8,
                            Right = "Página [page]/[toPage]"
                        },
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileContent = _converter.Convert(pdfContent);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = fileContent
            };

            return result;
        }

        private async Task<Result> GetRegisterFormat11(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format11/EvaluationReportRegister.cshtml", model);
            var pdfContent = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Landscape,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 400,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileContent = _converter.Convert(pdfContent);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = fileContent
            };

            return result;
        }

        private async Task<Result> GetRegisterFormat12(EvaluationReportInformationTemplate model)
        {
            var htmlBackground = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format12/EvaluationReportRegisterBackground.cshtml", model);
            var pdfBackground = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlBackground,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format12/EvaluationReportRegister.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 75, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileBackground = _converter.Convert(pdfBackground);

            var fileContent = _converter.Convert(pdf);
            fileContent = _textSharpService.AddPagination(fileContent, BaseColor.BLACK, 9, true, 515, 755, 0, 2);
            fileContent = _textSharpService.RemoveEmptyPages(fileContent, 1500);

            var resultPdf = _textSharpService.AddHeaderToAllPages(fileContent, fileBackground);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = resultPdf
            };

            return result;
        }

        private async Task<Result> GetRegisterFormat14(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format14/EvaluationReportRegister.cshtml", model);
            var pdfContent = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 400,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileContent = _converter.Convert(pdfContent);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = fileContent
            };

            return result;
        }

        private async Task<Result> GetRegisterFormat15(EvaluationReportInformationTemplate model)
        {
            var htmlContent = await _viewRenderService.RenderToStringAsync("/Services/EvaluationReportGenerator/Pdf/Format15/EvaluationReportRegister.cshtml", model);
            var pdfContent = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    DPI = 300,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Registro de Evaluación",
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        FooterSettings =
                        {
                            FontSize = 8,
                            Right = "Página [page]/[toPage]"
                        },
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileContent = _converter.Convert(pdfContent);

            var result = new Result
            {
                PdfName = model.DocumentTitle,
                Pdf = fileContent
            };

            return result;
        }


        #endregion

        #region Helpers

        private string GetBarCode(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            using (var barCode = new BarcodeLib.Barcode())
            {
                using (var barcodeImage = barCode.Encode(BarcodeLib.TYPE.CODE128, str, 200, 50))
                {
                    using (var ms = new MemoryStream())
                    {
                        barcodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = ms.ToArray();
                        barcodeImage.Dispose();

                        return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(imageBytes));
                    }
                }
            }
        }

        private async Task<Result> GetActEvaluationReportByFormat(EvaluationReportInformationTemplate model, byte? formatType = null)
        {
            if (formatType is null)
            {
                var configuration = await _configurationService.GetByKey(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_ACT_FORMAT);

                if (configuration is null)
                {
                    configuration = new ENTITIES.Models.Configuration
                    {
                        Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_ACT_FORMAT,
                        Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_ACT_FORMAT]
                    };

                    await _configurationService.Insert(configuration);
                }

                formatType = Convert.ToByte(configuration.Value);
            }

            switch (formatType)
            {
                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_1:
                    return await GetActFormat1(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_2:
                    return await GetActFormat2(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_3:
                    return await GetActFormat3(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_4:
                    return await GetActFormat4(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_5:
                    return await GetActFormat5(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_6:
                    return await GetActFormat6(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_7:
                    return await GetActFormat7(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_8:
                    return await GetActFormat8(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_9:
                    return await GetActFormat9(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_11:
                    return await GetActFormat11(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_12:
                    return await GetActFormat12(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_13:
                    return await GetActFormat13(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_14:
                    return await GetActFormat14(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_16:
                    return await GetActFormat16(model);

                default:
                    return await GetActFormat1(model);
            }
        }

        private async Task<Result> GetEvaluationRegisterReportByFormat(EvaluationReportInformationTemplate model, byte? formatType = null)
        {
            if (formatType is null)
            {
                var configuration = await _configurationService.GetByKey(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_REGISTER_FORMAT);

                if (configuration is null)
                {
                    configuration = new ENTITIES.Models.Configuration
                    {
                        Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_REGISTER_FORMAT,
                        Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_REGISTER_FORMAT]
                    };

                    await _configurationService.Insert(configuration);
                }

                formatType = Convert.ToByte(configuration.Value);
            }


            switch (formatType)
            {
                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.REGISTER_FORMAT.FORMAT_1:
                    return await GetRegisterFormat1(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.REGISTER_FORMAT.FORMAT_4:
                    return await GetRegisterFormat4(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.REGISTER_FORMAT.FORMAT_5:
                    return await GetRegisterFormat5(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.REGISTER_FORMAT.FORMAT_6:
                    return await GetRegisterFormat6(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.REGISTER_FORMAT.FORMAT_7:
                    return await GetRegisterFormat7(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.REGISTER_FORMAT.FORMAT_8:
                    return await GetRegisterFormat8(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.REGISTER_FORMAT.FORMAT_10:
                    return await GetRegisterFormat10(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.REGISTER_FORMAT.FORMAT_11:
                    return await GetRegisterFormat11(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.REGISTER_FORMAT.FORMAT_12:
                    return await GetRegisterFormat12(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.REGISTER_FORMAT.FORMAT_14:
                    return await GetRegisterFormat14(model);

                case ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.REGISTER_FORMAT.FORMAT_15:
                    return await GetRegisterFormat15(model);

                default:
                    return await GetRegisterFormat1(model);
            }
        }

        public async Task<Result> GetActEvaluationReportPreview(byte format)
        {
            var model = new EvaluationReportInformationTemplate
            {
                DocumentTitle = "Formato Prueba",
                Header = "Encabezado",
                SubHeader = "Subencabezado",
                BasicInformation = new EvaluationReportBasicInformationTemplate
                {
                    CreatedAt = DateTime.UtcNow
                },
                Course = new EvaluationReportCourseTemplate
                {
                    Career = new EvaluationReportCareerTemplate(),
                    Section = new EvaluationReportSectionTemplate
                    {
                        Code = "SECCIÓN",
                        Students = new System.Collections.Generic.List<EvaluationReportStudent>()
                    }
                },
                Term = new EvaluationReportTermTemplate
                {
                    Name = "PERIODO"
                }
            };

            var userLoggedIn = await _userService.GetUserByClaim(_httpContextAccessor.HttpContext.User);
            model.UserLoggedIn = userLoggedIn.Name;
            model.Img = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png");

            return await GetActEvaluationReportByFormat(model, format);
        }

        #endregion
    }
}
