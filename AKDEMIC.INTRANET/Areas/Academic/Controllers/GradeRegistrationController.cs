using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Academic.Controllers
{
    [Area("Academic")]
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Route("academico/registro-notas")]
    public class GradeRegistrationController : BaseController
    {
        private readonly ISectionService _sectionService;
        private readonly ICourseComponentService _courseComponentService;
        private IWebHostEnvironment _hostingEnvironment;
        public GradeRegistrationController(
            ISectionService sectionService,
            IDataTablesService dataTablesService,
            ICourseComponentService courseComponentService,
            IWebHostEnvironment hostingEnvironment) : base(dataTablesService)
        {
            _sectionService = sectionService;
            _courseComponentService = courseComponentService;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// Vista inicial del reporte de registro de notas para el área académica
        /// </summary>
        /// <returns>Vista inicial</returns>
        public IActionResult Index() => View();
        /// <summary>
        /// Retorna la lista de secciones con la información sobre el registro de sus notas
        /// </summary>
        /// <param name="component">Id de componentes del curso</param>
        /// <param name="term">Id del periodo académico</param>
        /// <param name="faculty">Id de la facultad</param>
        /// <param name="search">Término a filtrar</param>
        /// <returns>Objeto con la lista de secciones</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid component, Guid term, Guid faculty, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _sectionService.GetAllByCourseComponentTermFacultyAndPaginationParameters(sentParameters, component, term, faculty, search);
            return Ok(result);
        }

        /// <summary>
        /// Retorna la lista de secciones con los últimos filtros aplicados
        /// </summary>
        /// <param name="term">Id del periodo académico</param>
        /// <param name="lateFilter">último filtro aplicado</param>
        /// <param name="search">Término a filtrar</param>
        /// <returns>Objeto con la lista de secciones</returns>
        [HttpGet("get2")]
        public async Task<IActionResult> Get2(Guid term, int lateFilter, string search)
        {// escuela / tipo componentes / mostrar columnas en base a iltro / carrera plan de estudios

            var sentParameters = _dataTablesService.GetSentParameters();

            var result = await _sectionService.GetAllByCourseTermLatFilterAndPaginationParameters(sentParameters, term, lateFilter, search);
            return Ok(result);
        }
        /// <summary>
        /// Retorna la lista de componentes de cursos existentes
        /// </summary>
        /// <returns>Objeto con la lista de componentes</returns>
        [HttpGet("componentes/get")]
        public async Task<IActionResult> GetCourseComponents()
        {
            var components = await _courseComponentService.GetCourseComponents();
            var result = new
            {
                items = components
            };

            return Ok(result);
        }
        /// <summary>
        /// Genera el reporte de registros de notas por sección en formato Excel
        /// </summary>
        /// <param name="component">Id del componente de curso</param>
        /// <param name="term">Id del periodo académico</param>
        /// <param name="faculty">Id de la facultad</param>
        /// <returns>Documento Excel</returns>
        [HttpGet("reporte-excel/{component}/{term}/{faculty}")]
        public async Task<IActionResult> DownloadExcelReport(Guid component, Guid term, Guid faculty)
        {

            var result = await _sectionService.GetGradeRegistrationReport(component, term, faculty);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };
            var cont = 1;
            dt.Columns.Add("Nro");
            dt.Columns.Add("Curso");
            dt.Columns.Add("Sección");
            dt.Columns.Add("Periodo");

            foreach (var item in result)
                dt.Rows.Add(
                    cont++,
                    item.Course,
                    item.Section,
                    item.Term
                    );

            dt.AcceptChanges();

            var img = Path.Combine(_hostingEnvironment.WebRootPath, @"images/themes/" + CORE.Helpers.GeneralHelpers.GetTheme() + @"/logo-report.png");

            var fileName = $"Reporte de cumplimiento del registro de Notas.xlsx";
            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Listado de Cumplimiento del Registro de Notas", img);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
