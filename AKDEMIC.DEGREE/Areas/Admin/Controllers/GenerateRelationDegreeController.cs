using AKDEMIC.CORE.Services;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.REPOSITORY.Repositories.Degree.Templates;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/generacion-de-relacion-de-grados-titulos")]
    public class GenerateRelationDegreeController : BaseController
    {
        private readonly IConverter _dinkConverter;
        private readonly IViewRenderService _viewRenderService;
        private readonly IRegistryPatternService _registryPatternService;
        private readonly IDataTablesService _dataTablesService;
        public GenerateRelationDegreeController(IViewRenderService viewRenderService,
            IDataTablesService dataTablesService,
            IConfigurationService configurationService,
            IRegistryPatternService registryPatternService,
            IConverter dinkConverter) : base(configurationService)
        {
            _dinkConverter = dinkConverter;
            _registryPatternService = registryPatternService;
            _viewRenderService = viewRenderService;
            _dataTablesService = dataTablesService;
        }

        /// <summary>
        /// Vista principal donde se lista la relación de estudiantes 
        /// </summary>
        /// <returns>Retorna Vista</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Listado de estudiantes con padrón registrado
        /// </summary>
        /// <param name="startDate">Fec. Inicio</param>
        /// <param name="endDate">Fec. Fin</param>
        /// <param name="bookCode">Código de libro</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Retorna un OK</returns>
        [HttpGet("listado")]
        public async Task<IActionResult> GetRelationDegreeDatatable(string startDate, string endDate, string bookCode, string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var StartDateTime = CORE.Helpers.ConvertHelpers.DatepickerToUtcDateTime(startDate);
            var EndDateTime = CORE.Helpers.ConvertHelpers.DatepickerToUtcDateTime(endDate);
            var result = await _registryPatternService.GetRelationDegreeDatatable(sentParameters, StartDateTime, EndDateTime, bookCode, searchValue);
            return Ok(result);
        }

        /// <summary>
        /// Método para generar la relación de estudiantes con grados, titulos o diplomas aprobados.
        /// </summary>
        /// <param name="startDate">Fec. Inicio</param>
        /// <param name="endDate">Fec. Fin</param>
        /// <param name="bookCode">Código de libro</param>
        /// <returns>Retorna una rchivo en formato PDF</returns>
        [HttpGet("generar")]
        public async Task<IActionResult> GetRelationDegree(string startDate, string endDate, string bookCode)
        {
            var configurationSystemIntegrated = Boolean.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM));
            var StartDateTime = CORE.Helpers.ConvertHelpers.DatepickerToUtcDateTime(startDate);
            var EndDateTime = CORE.Helpers.ConvertHelpers.DatepickerToUtcDateTime(endDate);
            var resultsToSearch = new List<Guid>();
            var diplomas = new List<DegreeRelationTemplate>();
            var registryPatterns = new List<DegreeRelationTemplate>();
            var total = new List<DegreeRelationTemplate>();

            var model = new ListDegreeRelationTemplate();
            registryPatterns = await _registryPatternService.GetRelationDegreesByProcedure(StartDateTime, EndDateTime, resultsToSearch, bookCode);
            total.AddRange(registryPatterns);

            model.LstRelations = total;
            model.StartDate = startDate;
            model.EndDate = endDate;

            GlobalSettings globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 15, Left = 10, Right = 10 },
                DocumentTitle = "RELACIÓN DE GRADOS, TÍTULOS Y DIPLOMAS APROBADOS "
            };

            string viewToString = await _viewRenderService.RenderToStringAsync($"/Areas/Admin/Views/GenerateRelationDegree/GetRelationDegree.cshtml", model);
            ObjectSettings objectSettings = new ObjectSettings
            {
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8",
                    //UserStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css\areas\student\curriculum\fontawesome.css"),
                },
            };
            HtmlToPdfDocument pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            byte[] fileByte = _dinkConverter.Convert(pdf);
            //HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=false; path=/";
            return File(fileByte, "application/pdf");
        }
    }
}
