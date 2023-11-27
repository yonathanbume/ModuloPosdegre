using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.DEGREE.Areas.Admin.Models.DiplomaDeliveryViewModels;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/entrega-de-diplomas")]
    public class DiplomaDeliveryController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IRegistryPatternService _registryPatternService;
        private readonly IProcedureService _procedureService;
        private readonly IUserProcedureService _userProcedureService;

        public DiplomaDeliveryController(IConfigurationService configurationService,
            ICareerService careerService, IProcedureService procedureService,
            IUserProcedureService userProcedureService,
             IRegistryPatternService registryPatternService, IDataTablesService dataTablesService) : base(careerService, configurationService)
        {
            _dataTablesService = dataTablesService;
            _registryPatternService = registryPatternService;
            _procedureService = procedureService;
            _userProcedureService = userProcedureService;
        }

        /// <summary>
        /// Vista principal donde se listan los diplomas
        /// </summary>
        /// <returns>Retorna la Vista</returns>
        public async Task<IActionResult> Index()
        {
            var configurationSystemIntegrated = Boolean.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM));
            return View(configurationSystemIntegrated);
        }

        /// <summary>
        /// Obtien el listado de registros de padrones
        /// </summary>
        /// <param name="searchValue">TExto de búsqueda</param>
        /// <param name="searchBookNumber">Texto búsqueda número de libro</param>
        /// <param name="careerId">Identificador de la carrera</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="academicProgramId">Identificador del programa académico</param>
        /// <param name="diplomaStatus">Estado del diploma</param>
        /// <returns>Retorna un Ok</returns>
        [HttpGet("lista-registro-padrones")]
        public async Task<IActionResult> GetUserProcedures(string searchValue, string searchBookNumber, Guid? careerId, Guid? facultyId, Guid? academicProgramId, int? diplomaStatus)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _registryPatternService.GetRegistryPatternDatatableByConfiguration(sentParameters, facultyId, careerId, academicProgramId, searchBookNumber, null,null,searchValue, null, null, ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED, null,diplomaStatus);
            return Ok(result);
        }

        /// <summary>
        /// Método para cambiar el estado del registro de padrón
        /// </summary>
        /// <param name="registryPatternId">Identificador del registro del padrón</param>
        /// <param name="deliveryDiploma">Estado</param>
        /// <returns>Retorna un Ok o BadRequest</returns>
        [HttpPost("cambiar-estado")]
        public async Task<IActionResult> ChangeStatus(Guid registryPatternId, int deliveryDiploma)
        {
            var registryPattern = await _registryPatternService.GetWithIncludes(registryPatternId);
            registryPattern.DiplomaStatus = deliveryDiploma;

            //var userProcedure = new UserProcedure();
            //if (registryPattern.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
            //{
            //    userProcedure = await _userProcedureService.GetUserProcedureByStaticType(registryPattern.Student.UserId, ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR_DEGREE_APPLICATION);
            //}
            //else
            //{
            //    userProcedure = await _userProcedureService.GetUserProcedureByStaticType(registryPattern.Student.UserId, ConstantHelpers.PROCEDURES.STATIC_TYPE.TITLE_DEGREE_APPLICATION);
            //}

            //switch (deliveryDiploma)
            //{
            //    case ConstantHelpers.REGISTRY_PATTERN.DIPLOMA_DELIVERY.PENDING:
            //        userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.PENDING;
            //        break;
            //    case ConstantHelpers.REGISTRY_PATTERN.DIPLOMA_DELIVERY.SIGNED:
            //        userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.GENERATED;
            //        break;
            //    case ConstantHelpers.REGISTRY_PATTERN.DIPLOMA_DELIVERY.DELIVERED:
            //        userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED;
            //        break;
            //}
            await _registryPatternService.Update(registryPattern);
            return Ok();
        }

        /// <summary>
        /// Obtiene los detalle del registro de padrón
        /// </summary>
        /// <param name="registryPatternId">Identificador del registro de padrón</param>
        /// <returns>Retorna un OK</returns>
        [HttpGet("informacion/{registryPatternId}")]
        public async Task<IActionResult> GetInformation(Guid registryPatternId)
        {
            var result = await _registryPatternService.GetStudentBasicInformation(registryPatternId);
            return Ok(result);
        }

        /// <summary>
        /// Método para obtener el estado actual del registro de padrón
        /// </summary>
        /// <param name="registryPatternId">Identificador del registro de padrón</param>
        /// <returns>Retorna un Ok</returns>
        [HttpGet("obtener-estado-de-entrega/{registryPatternId}")]
        public async Task<IActionResult> GetDiplomaStatus(Guid registryPatternId)
        {
            var result = await _registryPatternService.Get(registryPatternId);
            return Ok(result.DiplomaStatus);
        }

        [HttpGet("reporte-excel")]
        public async Task<IActionResult> GetRegistryPatternExcel()
        {
            var registryPatterns = await _registryPatternService.GetRegistryPatternData();

            var dt = new DataTable();

            dt.TableName = "Diplomas";

            dt.Columns.Add("Identificador");
            dt.Columns.Add("Trámite");
            dt.Columns.Add("Tipo");
            dt.Columns.Add("Usuario");
            dt.Columns.Add("Paterno");
            dt.Columns.Add("Materno");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Dni");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Estado");

            foreach (var item in registryPatterns)
            {
                dt.Rows.Add(item.Id, item.RequestName, item.GradeType, item.UserName, item.PaternalSurname, item.MaternalSurname, item.Name, item.Dni, item.CareerName, item.DiplomaDelivery);
            }
            dt.AcceptChanges();

            string fileName = "ReporteEntregaDeDiplomas.xlsx";
            using (var wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                wb.Worksheets.Add(dt);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //Return xlsx Excel File  
                    Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }


        [HttpGet("template/reporte-excel")]
        public IActionResult GetRegistryPatternExcelTemplate()
        {

            #region Diplomas HOJA 1
            var dt = new DataTable
            {
                TableName = "Diplomas"
            };

            dt.Columns.Add("Identificador");
            dt.Columns.Add("Identificador de Estado");

            var diplomaStatusValues = ConstantHelpers.REGISTRY_PATTERN.DIPLOMA_DELIVERY.VALUES
                .Select(x => new
                {
                    Id = x.Key,
                    Text = x.Value
                })
                .ToList();


            dt.AcceptChanges();
            #endregion


            #region Estado de Delivery HOJA2
            var dtDiplomaStatus = new DataTable
            {
                TableName = "Estados"
            };

            dtDiplomaStatus.Columns.Add("Identificador de Estado");
            dtDiplomaStatus.Columns.Add("Estado");

            foreach (var item in diplomaStatusValues)
            {
                dtDiplomaStatus.Rows.Add(item.Id, item.Text);
            }

            dtDiplomaStatus.AcceptChanges();
            #endregion


            string fileName = "FormatoEntregaDeDiplomas.xlsx";
            using (var wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                wb.Worksheets.Add(dt);
                wb.Worksheets.Add(dtDiplomaStatus);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //Return xlsx Excel File  
                    Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost("actualizar/delivery-status/excel")]
        public async Task<IActionResult> UpdateRegistryPatternExcel(IFormFile file)
        {
            var errors = new List<dynamic>();
            var diplomaDeliveries = new List<DiplomaDeliveryViewModel>();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(file.OpenReadStream(), false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                int currentRow = 1;

                foreach (Sheet s in workbookPart.Workbook.Descendants<Sheet>())
                {
                    if (!(workbookPart.GetPartById(s.Id) is WorksheetPart wsPart)) continue;

                    foreach (Row row in wsPart.Worksheet.Descendants<Row>().Skip(1))
                    {
                        currentRow++;
                        var cells = row.Elements<Cell>();

                        if (cells.Count() < 2)
                        {
                            errors.Add($"La fila {currentRow} tiene menos de 2 columnas de dato");
                            continue;
                        }

                        string registryPatternIdString = OpenXmlHelpers.Excel.GetCellValue(cells.ElementAt(0));
                        string deliveryStatusString = OpenXmlHelpers.Excel.GetCellValue(cells.ElementAt(1));

                        int deliveryStatus = -1;
                        Guid registryPatternId = Guid.Empty;

                        bool result = false;
                        result = int.TryParse(deliveryStatusString, out deliveryStatus);
                        if (!result)
                        {
                            errors.Add($"El identificador de estado de delivery no es un valor correcto. Fila {currentRow} ");
                            continue;
                        }

                        if (!ConstantHelpers.REGISTRY_PATTERN.DIPLOMA_DELIVERY.VALUES.ContainsKey(deliveryStatus))
                        {
                            errors.Add($"El identificador de estado de delivery no se encuentra en la lista. Fila {currentRow} ");
                            continue;
                        }

                        result = Guid.TryParse(registryPatternIdString, out registryPatternId);

                        if (!result || registryPatternId == Guid.Empty)
                        {
                            errors.Add($"El identificador no es un valor correcto. Fila {currentRow} ");
                            continue;
                        }

                        var diplomaDelivery = new DiplomaDeliveryViewModel
                        {
                            RegistryPatternId = registryPatternId,
                            DiplomaDeliveryStatus = deliveryStatus,
                        };

                        diplomaDeliveries.Add(diplomaDelivery);
                    }
                }
            }

            var diplomaDeliveriesDb = await _registryPatternService.GetAll();
            int updated = 0;
            for (int i = 0; i < diplomaDeliveries.Count; i++)
            {
                var currentDiplomaDelivery = diplomaDeliveriesDb.Where(x => x.Id == diplomaDeliveries[i].RegistryPatternId).FirstOrDefault();
                if (currentDiplomaDelivery == null)
                {
                    errors.Add($"No se ha encontrado el diploma con identificador {diplomaDeliveries[i].RegistryPatternId}");
                    continue;
                }

                currentDiplomaDelivery.DiplomaStatus = diplomaDeliveries[i].DiplomaDeliveryStatus;
                updated++;
            }

            if (updated > 0)
                await _registryPatternService.SaveChangesAsync();

            return Ok();
        }
    }
}
