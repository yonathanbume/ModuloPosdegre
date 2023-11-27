using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_RECORD)]
    [Area("Report")]
    [Route("reporte/estudiantes-condicionados")]
    public class ConditionedStudentController : BaseController
    {
        private readonly IStudentSectionService _studentSectionService;

        public ConditionedStudentController(IDataTablesService dataTablesService, IStudentSectionService studentSectionService) : base(dataTablesService)
        {
            _studentSectionService = studentSectionService;
        }

        public IActionResult Index() => View();
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid termId, Guid careerId, byte? conditionType, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentSectionService.GetConditionedStudentSectionsDatatable(sentParameters, termId, careerId, conditionType, search);
            return Ok(result);
        }

        /// <summary>
        /// Genera el reporte Excel de alumnos condicionados
        /// </summary>
        /// <param name="termId">Id del periodo académico</param>
        /// <param name="careerId">Id de la carrera</param>
        /// <param name="conditionType">Nro de intento de la matrícula</param>
        /// <param name="search">Término a filtrar en estudiantes</param>
        /// <returns>Documento Excel de alumnos</returns>
        [HttpGet("reporte-excel")]
        public async Task<IActionResult> ExcelReport(Guid termId, Guid careerId, byte? conditionType, string search)
        {
            var students = await _studentSectionService.GetConditionedStudentSectionsData(termId, careerId, conditionType, search);

            var dt = new DataTable
            {
                TableName = "Estudiantes Condicionados"
            };

            dt.Columns.Add("Código");
            dt.Columns.Add("Nombre completo");
            dt.Columns.Add("Escuela profesional");
            dt.Columns.Add("Curso");
            dt.Columns.Add("Condición");
            dt.Columns.Add("Periodo");

            foreach (var item in students)
            {
                var condition = "11va matrícula o mayor";

                switch (item.Try)
                {
                    case 2: condition = "Segunda matrícula"; break;
                    case 3: condition = "Tercera matrícula"; break;
                    case 4: condition = "Cuarta matrícula"; break;
                    case 5: condition = "Quinta matrícula"; break;
                    case 6: condition = "Sexta matrícula"; break;
                    case 7: condition = "Séptima matrícula"; break;
                    case 8: condition = "Octava matrícula"; break;
                    case 9: condition = "Novena matrícula"; break;
                    case 10: condition = "Décima matrícula"; break;
                    default: condition = "11va matrícula o mayor"; break;
                }

                dt.Rows.Add(item.UserName, item.FullName, item.Career, item.Course, condition, item.Term);
            }

            dt.AcceptChanges();

            string fileName = $"Reporte Estudiantes Condicionados.xlsx";

            using (var wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Reporte Estudiantes Condicionados", null, 6);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //Return xlsx Excel File  
                    HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
