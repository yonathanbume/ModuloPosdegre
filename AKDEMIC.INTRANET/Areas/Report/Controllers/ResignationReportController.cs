// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Report")]
    [Route("reporte/alumnos-renuncia")]
    public class ResignationReportController : BaseController
    {
        private readonly IStudentObservationService _studentObservationService;

        public ResignationReportController(IDataTablesService dataTablesService,
            IStudentObservationService studentObservationService) : base(dataTablesService)
        {
            _studentObservationService = studentObservationService;
        }


        /// <summary>
        /// Vista donde se muestra los alumnos con reservas de matrícula
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de alumnos con reserva de matrícula
        /// </summary>
        /// <param name="term">Identificador del periodo académico</param>
        /// <param name="faculty">Identificador de la facultad</param>
        /// <param name="career">Identificador de la carreeraa</param>
        /// <returns>Listado de alumno</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid? term = null, Guid? faculty = null, Guid? career = null)
        {
            var sentParameters = GetSentParameters();
            var result = await _studentObservationService.GetResignatedStudentsDatatable(sentParameters, null, null, faculty, career);
            return Ok(result);
        }
    }
}
