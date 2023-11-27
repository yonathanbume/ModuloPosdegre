// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.INTRANET.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize]
    [Area("Report")]
    [Route("reportes")]
    public class IntranetReportController : BaseController
    {
        public IntranetReportController()
        {

        }

        /// <summary>
        /// Retorna la vista de reportes comunidad del sistema, aquí se mostrarán los links a diferentes reportes
        /// </summary>
        /// <returns>Vista de reportes consolidados</returns>
        [HttpGet("comunidad")]
        public IActionResult Community()
            => View();

        #region Student

        /// <summary>
        /// Retorna la vista de reportes de estudiantes del sistema, aquí se mostrarán los links a diferentes reportes
        /// </summary>
        /// <returns>Vista de reportes consolidados</returns>
        [HttpGet("estudiantes")]
        public IActionResult Student()
            => View();

        [HttpGet("estudiantes/egresados")]
        public IActionResult GraduatedStudents()
            => View();

        [HttpGet("estudiantes/asistencia")]
        public IActionResult StudentAttendance()
            => View();


        #endregion

        /// <summary>
        /// Retorna la vista de reportes de docentes del sistema, aquí se mostrarán los links a diferentes reportes
        /// </summary>
        /// <returns>Vista de reportes consolidados</returns>
        [HttpGet("docentes")]
        public IActionResult Teacher()
            => View();

    }
}
