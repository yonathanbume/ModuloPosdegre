using System.IO;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.REPOSITORY.Repositories.Degree.Templates;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/generacion-de-formato-registro")]
    public class GenerationFormatRegisterDegreeController : BaseController
    {
        private readonly IConverter _dinkConverter;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IViewRenderService _viewRenderService;
        private readonly IDataTablesService _dataTablesService;
        private readonly IRegistryPatternService _registryPatternService;
        public GenerationFormatRegisterDegreeController(
            IViewRenderService viewRenderService,
            IConverter dinkConverter,
            IWebHostEnvironment hostingEnvironment,
            IDataTablesService dataTablesService,
            IRegistryPatternService registryPatternService)
        {
            _dinkConverter = dinkConverter;
            _hostingEnvironment = hostingEnvironment;
            _viewRenderService = viewRenderService;
            _dataTablesService = dataTablesService;
            _registryPatternService = registryPatternService;
        }

        /// <summary>
        /// Vista principal para generar el formato de registros
        /// </summary>
        /// <returns>Retorna Vista</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Listado de alumnos detallando su grado académico
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="searchBookNumber">Texto búsqueda número de libro</param>
        /// <param name="dateStartFilter">Fec. Inicio</param>
        /// <param name="dateEndFilter">Fec. Fin</param>
        /// <returns>Retorna un OK</returns>
        [HttpGet("listado")]
        public async Task<IActionResult> GetDiplomas(string searchValue, string searchBookNumber, string dateStartFilter, string dateEndFilter)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _registryPatternService.GetRegistryPatternDatatableByConfiguration(sentParameters, null, null, null, searchBookNumber, dateStartFilter, dateEndFilter, searchValue, null, null, ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED);
            return Ok(result);
        }

        /// <summary>
        /// Genera el formato de grados
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="searchBookNumber">Texto búsqueda número de libro</param>
        /// <param name="dateStartFilter">Fec. Inicio</param>
        /// <param name="dateEndFilter">Fec. Fin</param>
        /// <returns>Retorna un archivo en formato PDF</returns>
        [HttpGet("generar-formatos")]
        public async Task<IActionResult> GenerateFormatRegister(string searchValue, string searchBookNumber, string dateStartFilter, string dateEndFilter)
        {
            var lstFormatRegister = await _registryPatternService.GetFormatRegisterTemplate(searchValue, searchBookNumber, dateStartFilter, dateEndFilter);

            var model = new ListFormatRegisterTemplate();
            model.LstFormatRegisterTemplate = lstFormatRegister;
            model.Img = Path.Combine(_hostingEnvironment.WebRootPath, @$"images/themes/{ConstantHelpers.Institution.Values[ConstantHelpers.GENERAL.Institution.Value]}/logo-sm.png");

            if (
                ConstantHelpers.Institution.UNICA == ConstantHelpers.GENERAL.Institution.Value ||
                ConstantHelpers.Institution.UNSM == ConstantHelpers.GENERAL.Institution.Value
                )
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (PdfDocument outPdf = new PdfDocument())
                {
                    foreach (var item in lstFormatRegister)
                    {
                        item.Logo = Path.Combine(_hostingEnvironment.WebRootPath, @$"images/themes/{ConstantHelpers.Institution.Values[ConstantHelpers.GENERAL.Institution.Value]}/logo-sm.png");
                        var viewToString = await _viewRenderService.RenderToStringAsync($"/Areas/Admin/Views/GenerationFormatRegisterDegree/{ConstantHelpers.Institution.Values[ConstantHelpers.GENERAL.Institution.Value]}/GenerateFormatRegister.cshtml", item);
                        var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, $@"css/areas/admin/{ConstantHelpers.Institution.Values[ConstantHelpers.GENERAL.Institution.Value]}/generation_format_register.css");

                        var globalSettings = new GlobalSettings
                        {
                            ColorMode = ColorMode.Color,
                            Orientation = Orientation.Portrait,
                            PaperSize = PaperKind.A4,
                            Margins = new MarginSettings { Top = 1, Bottom = 1, Left = 1.2, Right = 1.2, Unit = Unit.Centimeters },
                            DocumentTitle = "Generación de formato de registro "
                        };

                        ObjectSettings objectSettings = new ObjectSettings
                        {
                            HtmlContent = viewToString,
                            WebSettings = { DefaultEncoding = "utf-8" ,
                            UserStyleSheet = userStyleSheet},
                        };

                        HtmlToPdfDocument pdf = new HtmlToPdfDocument()
                        {
                            GlobalSettings = globalSettings,
                            Objects = { objectSettings }
                        };

                        var partiaLPdfBytes = _dinkConverter.Convert(pdf);
                        var partialPdf = PdfReader.Open(new MemoryStream(partiaLPdfBytes), PdfDocumentOpenMode.Import);
                        CopyPages(partialPdf, outPdf);
                    }

                    MemoryStream stream = new MemoryStream();
                    outPdf.Save(stream, false);
                    var bytes = stream.ToArray();

                    var fileDownloadName = System.Web.HttpUtility.UrlEncode($"Informe-de-grado-bachiller.pdf");
                    HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

                    return File(bytes, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
                }
            }
            else
            {
                var viewToString = await _viewRenderService.RenderToStringAsync($"/Areas/Admin/Views/GenerationFormatRegisterDegree/GenerateFormatRegister.cshtml", model);

                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 40, Bottom = 40, Left = 10, Right = 10 },
                    DocumentTitle = "Generación de formato de registro "
                };

                ObjectSettings objectSettings = new ObjectSettings
                {
                    HtmlContent = viewToString,
                    WebSettings = { DefaultEncoding = "utf-8" },
                };

                HtmlToPdfDocument pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };
                var fileContents = _dinkConverter.Convert(pdf);
                var fileDownloadName = System.Web.HttpUtility.UrlEncode($"Informe-de-grado-bachiller.pdf");
                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

                return File(fileContents, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, fileDownloadName);
            }
        }


        private void CopyPages(PdfDocument from, PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }
    }
}
