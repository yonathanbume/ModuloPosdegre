// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Report")]
    [Route("reporte/estudiantes-retiro-ciclo")]
    public class WithdrawnStudentController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IStudentService _studentService;
        private readonly ITermService _termService;

        public WithdrawnStudentController(
            IDataTablesService dataTablesService,
            IStudentService studentService,
            ITermService termService
            )
        {
            _dataTablesService = dataTablesService;
            _studentService = studentService;
            _termService = termService;
        }

        public IActionResult Index() => View();

        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid? termId, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetWithdrawnStudentsDatatable(sentParameters, termId, search);
            return Ok(result);
        }

        [HttpGet("get-excel")]
        public async Task<IActionResult> GetExcel(Guid? termId)
        {
            var result = await _studentService.GetWithdrawnStudentsTemplate(termId);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Usuario");
            dt.Columns.Add("Nombre Completo");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Fec. Retiro");

            foreach (var item in result)
                dt.Rows.Add(item.UserName, item.FullName, item.Career, item.CreatedAt);

            dt.AcceptChanges();

            string fileName = $"Reporte de estudiantes con retiro de ciclo.xlsx";

            if (termId.HasValue && termId != Guid.Empty)
            {
                var term = await _termService.Get(termId.Value);
                fileName = $"Reporte de estudiantes con retiro de ciclo {term.Name}.xlsx";
            }

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Estudiantes con retiro de ciclo", null);

                ws.Rows().AdjustToContents();
                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }
    }
}
