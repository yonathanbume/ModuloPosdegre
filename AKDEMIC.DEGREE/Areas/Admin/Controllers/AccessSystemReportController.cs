// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.DEGREE.Areas.Report.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN)]
    [Area("Admin")]
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
        /// Vista prinicipal donde se encuentra el listado de usuarios detallando su primer y último ingreso al sistema.
        /// </summary>
        /// <returns>Retorna la vista</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de usuarios detallando su primer y último ingreso al sistema
        /// </summary>
        /// <param name="roleType">Rol (todos, alumnos, egresados)</param>
        /// <param name="startDate">Fec. Inicio</param>
        /// <param name="endDate">Fec. Fin</param>
        /// <param name="search"></param>
        /// <returns>Retorna un Ok con los datos para ser usado en tablas.</returns>
        [HttpGet("reporte-accesos-sistema-listado")]
        public async Task<ActionResult> AccessSystemReportDatatable(byte roleType, string startDate = null, string endDate = null, string search = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetUserLoginStudentsDatatable(sentParameters, ConstantHelpers.SYSTEMS.DEGREE, roleType, startDate, endDate, search);
            return Ok(result);
        }
    }
}
