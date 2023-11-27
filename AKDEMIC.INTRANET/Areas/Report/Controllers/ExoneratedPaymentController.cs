// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Report")]
    [Route("reporte/pagos-exonerados")]
    public class ExoneratedPaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        public ExoneratedPaymentController(IDataTablesService dataTablesService,
            IPaymentService paymentService) : base(dataTablesService)
        {
            _paymentService = paymentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid termId, Guid? careerId, byte? type, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _paymentService.GetExoneratedPaymentsDatatableData(sentParameters, termId, careerId, type, search);
            return Ok(result);
        }

        [HttpGet("reporte-excel")]
        public async Task<IActionResult> ExcelReport(Guid termId, Guid? careerId, byte? type, string search)
        {
            var payments = await _paymentService.GetExoneratedPaymentsData(termId, careerId, type, search);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Código");
            dt.Columns.Add("Nombre completo");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Tipo");
            dt.Columns.Add("Concepto");
            //dt.Columns.Add("Monto");

            foreach (var item in payments.OrderBy(x => x.Career).ThenBy(x => x.FullName).ToList())
                dt.Rows.Add(item.User, item.FullName, item.Career, item.Type, item.Concept/*, item.Amount*/);

            dt.AcceptChanges();

            var fileName = $"Reporte de Pagos Exonerados.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Listado de Pagos Exonerados", null, 5);
                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
