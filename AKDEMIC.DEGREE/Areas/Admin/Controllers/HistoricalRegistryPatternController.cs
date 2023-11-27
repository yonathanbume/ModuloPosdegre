using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DEGREE.Areas.Admin.Models.HistoricalRegistryPatternViewModels;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/historial-padrones")]
    public class HistoricalRegistryPatternController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly AkdemicContext _context;
        private readonly IHistoricalRegistryPatternService _historicalRegistryPatternService;
        private readonly IRegistryPatternService _registryPatternService;

        public HistoricalRegistryPatternController(IDataTablesService dataTablesService,
            IOptions<CloudStorageCredentials> storageCredentials,
            AkdemicContext context,
            IHistoricalRegistryPatternService historicalRegistryPatternService,
            IRegistryPatternService registryPatternService) : base()
        {
            _dataTablesService = dataTablesService;
            _storageCredentials = storageCredentials;
            _context = context;
            _historicalRegistryPatternService = historicalRegistryPatternService;
            _registryPatternService = registryPatternService;
        }

        /// <summary>
        /// Vista principal donde se lista el historial de padrones
        /// </summary>
        /// <returns>Retorna la vista</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el historial de padrones
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Retorna un Ok con los datos para ser usado en tablas</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetCareers(string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _historicalRegistryPatternService.GetHistoricalRegistryPatternsDatatable(sentParameters, search);
            return Ok(result);
        }

        /// <summary>
        /// Método para agregar un historial de padrón
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del nuevo historial de padrón</param>
        /// <returns>Retorna un Ok o BadRequest</returns>
        [HttpPost("registrar")]
        public async Task<IActionResult> Create(HistoricalRegistryPatternViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Debe completar todos los campos");
            }
            try
            {
                var pattern = new HistoricalRegistryPattern
                {
                    OfficeNumber = model.OfficeNumber,
                    Description = model.Description
                };

                var registryPatterns = new List<RegistryPattern>();
                var usernameNotFound = new List<string>();

                if (model.File != null)
                {

                    using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(model.File.OpenReadStream(), false))
                    {
                        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

                        int currentRow = 2;

                        foreach (Sheet s in workbookPart.Workbook.Descendants<Sheet>())
                        {
                            if (!(workbookPart.GetPartById(s.Id) is WorksheetPart wsPart))
                                continue;

                            foreach (Row row in wsPart.Worksheet.Descendants<Row>().Skip(1))
                            {
                                try
                                {
                                    IEnumerable<Cell> cells = row.Elements<Cell>();

                                    //string username = OpenXmlHelpers.Excel.GetCellValue(cells.ElementAt(59));
                                    string username = OpenXmlHelpers.Excel.GetCellValue(cells.ElementAt(69));
                                    //string universityCouncil = OpenXmlHelpers.Excel.GetCellValue(cells.ElementAt(55));                       
                                    DateTime universityCouncil = DateTime.FromOADate(int.Parse(OpenXmlHelpers.Excel.GetCellValue(cells.ElementAt(55))));
                                    string abreviature = OpenXmlHelpers.Excel.GetCellValue(cells.ElementAt(27));
                                    var registryPattern = await _context.RegistryPatterns.Where(x => x.Student.User.UserName == username && x.GradeAbbreviation == abreviature && x.UniversityCouncilDate.Value.Date == universityCouncil.Date).FirstOrDefaultAsync();
                                    if (registryPattern is null)
                                    {
                                        usernameNotFound.Add(username);
                                    }
                                    else
                                    {
                                        registryPattern.ReadyToSunedu = true;
                                        registryPattern.OfficeNumber = model.OfficeNumber;
                                        registryPatterns.Add(registryPattern);
                                    }
                                }
                                catch (Exception)
                                {
                                    continue;
                                }

                                currentRow++;
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                if (model.File != null)
                {
                    var storage = new CloudStorageService(_storageCredentials);

                    pattern.File = await storage.UploadFile(model.File.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.USER_REQUIREMENT_DEGREE,
                        Path.GetExtension(model.File.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.DEGREE);
                }

                await _historicalRegistryPatternService.Insert(pattern);

                var message = "Tarea completada con éxito.";

                if (usernameNotFound.Any())
                {
                    message += "<br>";
                    message += $"Padrones de Registro Actualizados : {registryPatterns.Count()}";
                    message += "<br>";
                    message += $"Padrones de usuarios no encontrados : {string.Join("; ", usernameNotFound)}";
                }

                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest("Ingresar un formato válido.");
            }
        }

        /// <summary>
        /// Método para editar un historial de padrón
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del historial de padrón</param>
        /// <returns>Retorna un Ok o BadRequest</returns>
        [HttpPost("editar")]
        public async Task<IActionResult> Edit(HistoricalRegistryPatternViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Debe completar todos los campos");
            }

            var pattern = await _historicalRegistryPatternService.Get(model.Id);

            pattern.OfficeNumber = model.OfficeNumber;
            pattern.Description = model.Description;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                pattern.File = await storage.UploadFile(model.File.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.USER_REQUIREMENT_DEGREE,
                    Path.GetExtension(model.File.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.DEGREE);
            }

            await _historicalRegistryPatternService.Update(pattern);
            return Ok();
        }

        /// <summary>
        /// Mëtodo para eliminar el historial de padrón
        /// </summary>
        /// <param name="id">Identificador del historial de padrón</param>
        /// <returns>Retorna un Ok o BadRequest</returns>
        [HttpPost("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var pattern = await _historicalRegistryPatternService.Get(id);
            await _historicalRegistryPatternService.Delete(pattern);
            return Ok();
        }

        /// <summary>
        /// Método para descargar el archivo adjunto al historial de padrón
        /// </summary>
        /// <param name="id">Identificador del historial de padroón</param>
        /// <returns>Retorna un archivo</returns>
        [HttpGet("descargar/{id}")]
        public async Task DonwloadDocumentEnterprise(Guid id)
        {

            var historical = await _historicalRegistryPatternService.Get(id);
            var fileName = historical.File;
            await AKDEMIC.CORE.Helpers.GeneralHelpers.GetFileForDownload(HttpContext, _storageCredentials, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.USER_REQUIREMENT_DEGREE, fileName);

        }
    }
}
