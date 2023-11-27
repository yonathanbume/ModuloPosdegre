// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Report")]
    [Route("reporte/estudiantes-renuncia")]
    public class ResignationStudentController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IStudentService _studentService;

        public ResignationStudentController(
            IDataTablesService dataTablesService,
            IStudentService studentService
            )
        {
            _dataTablesService = dataTablesService;
            _studentService = studentService;
        }

        public IActionResult Index() => View();

        [HttpGet("get")]
        public async Task<IActionResult> Get(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var status = new List<int>
            {
                ConstantHelpers.Student.States.RESIGNATION
            };

            var result = await _studentService.GetStudentByStatusListDatatable(parameters, status);
            return Ok(result);
        }

        [HttpGet("get-excel")]
        public async Task<IActionResult> GetExcel()
        {
            var status = new List<int>
            {
                ConstantHelpers.Student.States.RESIGNATION
            };

            var result = await _studentService.GetStudentFilterTemplatesByStatus(status);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Usuario");
            dt.Columns.Add("Nombre Completo");
            dt.Columns.Add("Documento");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Fec. Renuncia");

            foreach (var item in result)
                dt.Rows.Add(item.UserName, item.FullName, item.Dni, item.Career, item.ResignationDateTime);

            dt.AcceptChanges();

            string fileName = $"Reporte de estudiantes con renuncia.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Estudiantes con renuncia", null);

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
