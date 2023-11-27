using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF + "," + ConstantHelpers.ROLES.REPORT_QUERIES)]
    [Area("AcademicRecord")]
    [Route("registrosacademicos/reporte-egresados")]
    public class ReportGraduateController : BaseController
    {
        private readonly IStudentService _studentService;

        public ReportGraduateController(IStudentService studentService, IDataTablesService datatableService) : base(datatableService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// VIsta inicial del reporte de grados
        /// </summary>
        /// <returns>Vista principal</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de egresados para ser usado en tablas
        /// </summary>
        /// <param name="gradeType">Tipo de grado</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="year">Año</param>
        /// <returns>Listado de egresados</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(int gradeType, Guid careerId, int year)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GraduatedListReportToAcademicRecord(sentParameters, User, gradeType, careerId, year);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de años de egreso
        /// </summary>
        /// <returns>Listado de años de egreso</returns>
        [HttpGet("get-años-egreso")]
        public IActionResult GetGraduteYears()
        {
            var list = new List<int>();

            for (int i = 2013; i <= DateTime.UtcNow.Year; i++)
            {
                list.Add(i);
            }

            return Ok(list);
        }
    }
}
