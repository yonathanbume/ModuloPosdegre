using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.Format;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Geo.Interfaces;
using AutoMapper;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/format")]
    public class FormatController : BaseController
    {
        public IConverter _dinkConverter { get; }
        private readonly IViewRenderService _viewRenderService;
        private readonly ILaboratoryRequestService _laboratoyRequestService;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;
        public FormatController(IConverter dinkConverter, IWebHostEnvironment environment, IViewRenderService viewRenderService
            , UserManager<ApplicationUser> _userManager,
            ILaboratoryRequestService laboratoyRequestService,
            IMapper mapper) : base(_userManager)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
            _laboratoyRequestService = laboratoyRequestService;
            _mapper = mapper;
        }

        /// <summary>
        /// Genera el formato ATS
        /// </summary>
        /// <param name="id">Identificador del ATS</param>
        /// <returns>Archivo en formato PDF</returns>
        [Route("ats/{id?}")]
        public async Task<IActionResult> ATS(Guid id)
        {
            var result = await _laboratoyRequestService.GetATS(id);
            var model = _mapper.Map<ATSViewModel>(result);

            model.Image = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png");

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Landscape,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = $"Reporte Carga Académica"
            };

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/Format/ATS.cshtml", model);
            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var fileByte = _dinkConverter.Convert(pdf);
            //return View(model);
            return File(fileByte, "application/pdf");
        }
    }
}
