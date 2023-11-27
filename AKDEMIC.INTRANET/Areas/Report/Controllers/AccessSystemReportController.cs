// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Report")]
    [Route("admin/accesos-sistema")]
    public class AccessSystemReportController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IStudentService _studentService;

        public AccessSystemReportController(IDataTablesService dataTablesService,
            IUserService userService,
            IStudentService studentService)
        {
            _dataTablesService = dataTablesService;
            _studentService = studentService;
        }

        /// <summary>
        /// Vista donde se listan los usuarios que accedieron al sistema
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de usuarios que accedieron al sistema
        /// </summary>
        /// <param name="roleType">Rol</param>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha fin</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de usuarios</returns>
        [HttpGet("reporte-accesos-sistema-listado")]
        public async Task<ActionResult> AccessSystemReportDatatable(byte roleType, string startDate = null, string endDate = null, string search = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetUserLoginStudentsDatatable(sentParameters, ConstantHelpers.SYSTEMS.INTRANET, roleType, startDate, endDate, search);
            return Ok(result);
        }

        [HttpGet("reporte-accesos-sistema-listado/get-excel")]
        public async Task<IActionResult> AccessSystemReportDatatableExcel(byte roleType, string startDate = null, string endDate = null)
        {
            var result = await _studentService.GetUserLoginStudentsTemplate(ConstantHelpers.SYSTEMS.INTRANET, roleType, startDate, endDate);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Usuario");
            dt.Columns.Add("Nombre Completo");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Primer Logeo");
            dt.Columns.Add("Último Logeo");


            foreach (var item in result)
                dt.Rows.Add(item.UserName, item.FullName, item.Career, item.FirstLoginStr, item.LastLoginStr);

            dt.AcceptChanges();

            string fileName = $"Registro de acceso al sistema.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Registro de acceso al sistema", null);

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
