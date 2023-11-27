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
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles =
        CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," +
        CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_SECRETARY + "," +
        CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        CORE.Helpers.ConstantHelpers.ROLES.VICERRECTOR)]
    [Area("Report")]
    [Route("reporte/notas-curso")]
    public class CourseGradeStatisticController : BaseController
    {
        private readonly ICourseTermService _courseTermService;

        public CourseGradeStatisticController(IDataTablesService dataTablesService, ICourseTermService courseTermService) : base(dataTablesService)
        {
            _courseTermService = courseTermService;
        }

        public IActionResult Index() => View();

        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid termId, Guid? careerId, Guid? curriculumId, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _courseTermService.GetCourseGradeStatisticsDatatable(sentParameters, termId, careerId, curriculumId, User, search);
            return Ok(result);
        }

        [HttpGet("get-excel")]
        public async Task<IActionResult> GetExcel(Guid termId, Guid? careerId, Guid? curriculumId)
        {
            var result = await _courseTermService.GetCourseGradeStatisticsTemplate(termId, careerId, curriculumId, User);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Código");
            dt.Columns.Add("Curso");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Nro. Notas");
            dt.Columns.Add("Media");
            dt.Columns.Add("Mediana");
            dt.Columns.Add("Des. Estándar");
            dt.Columns.Add("Perc. 25");
            dt.Columns.Add("Perc. 50");
            dt.Columns.Add("Perc. 75");

            foreach (var item in result)
                dt.Rows.Add(item.Code, item.Course, item.Career, item.GradeCount, item.Mean, item.Median, item.StandardDeviation, item.Percentile25, item.Percentile50, item.Percentile75);

            dt.AcceptChanges();

            string fileName = $"Estadística de notas según remuestreo.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Estadística de notas según remuestreo", null);

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
