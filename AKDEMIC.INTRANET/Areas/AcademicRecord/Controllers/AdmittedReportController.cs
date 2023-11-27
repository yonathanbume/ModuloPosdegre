using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF + "," + ConstantHelpers.ROLES.REPORT_QUERIES)]
    [Area("AcademicRecord")]
    [Route("registrosacademicos/reporte-ingresantes")]
    public class AdmittedReportController : BaseController
    {
        private readonly IPostulantService _postulantService;
        private readonly IStudentService _studentService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ITermService _termService;
        private readonly ICareerService _careerService;

        public AdmittedReportController(IDataTablesService dataTablesService, IPostulantService postulantService,
            IWebHostEnvironment hostingEnvironment,
            IStudentService studentService,
            ITermService termService,
            ICareerService careerService) : base(dataTablesService)
        {
            _postulantService = postulantService;
            _hostingEnvironment = hostingEnvironment;
            _studentService = studentService;
            _termService = termService;
            _careerService = careerService;
        }

        /// <summary>
        /// Vista inicial del reporte de ingresantes
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de ingresantes para ser usado en tablas
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="admissionTypeId">Identificador del tipo de admisión</param>
        /// <param name="applicationTermId">Identificador del periodo de admisión</param>
        /// <returns>Listado de ingresantes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid? careerId, Guid? admissionTypeId, Guid? applicationTermId)
        {
            //var parameters = _dataTablesService.GetSentParameters();
            //var result2 = await _postulantService.GetAdmittedApplicants(parameters, careerId, admissionTypeId, applicationTermId, User);
            //return Ok(result);

            if (!applicationTermId.HasValue)
            {
                var term = await _termService.GetLastTerm();
                applicationTermId = term.Id;
            }
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetStudentsByAdmissionTermReportDatatable(sentParameters, applicationTermId.Value, null, careerId, null, User, null, admissionTypeId);
            return Ok(result);
        }

        /// <summary>
        /// Método que genera el reporte de ingresantes 
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="admissionTypeId">Identificador del tipo de admisión</param>
        /// <param name="applicationTermId">Identificador del periodo de admisión</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("reporte-excel")]
        public async Task<IActionResult> DownloadExcelReport(Guid? careerId, Guid? admissionTypeId, Guid? applicationTermId)
        {
            if (!applicationTermId.HasValue || applicationTermId == Guid.Empty)
            {
                var term = await _termService.GetLastTerm();
                applicationTermId = term.Id;
            }

            var students = await _studentService.GetStudentsByAdmissionTermPdfData(applicationTermId.Value, null, careerId, null, User, admissionTypeId);

            var title = "Todos";

            if (careerId.HasValue && careerId != Guid.Empty) title = (await _careerService.Get(careerId.Value)).Name;

            title += $" - {(await _termService.Get(applicationTermId.Value)).Name}";

            var dt = new DataTable
            {
                TableName = "Estudiantes Ingresantes"
            };

            dt.Columns.Add("N°");
            dt.Columns.Add("CÓDIGO");
            dt.Columns.Add("APELLIDOS Y NOMBRES");
            dt.Columns.Add("ESCUELA PROFESIONAL");
            dt.Columns.Add("MOD. INGRESO");

            var count = 1;
            foreach (var item in students)
                dt.Rows.Add(count++, item.User.UserName, item.User.FullName, item.Career.Name, item.AdmissionType.Name);

            dt.AcceptChanges();

            var img = Path.Combine(_hostingEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png");

            var fileName = $"Estudiantes Ingresantes - {title}.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                //ws.AddHeaderToWorkSheet($"REPORTE ESTUDIANTES INGRESANTES {title.ToUpper()}", img);
                ws.AddHeaderToWorkSheet($"REPORTE ESTUDIANTES INGRESANTES {title.ToUpper()}", null);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        /// <summary>
        /// Vista del reporte consolidado de ingresantes
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("consolidado")]
        public IActionResult SummaryReport() => View();

        /// <summary>
        /// Obtiene el reporte consolidado de ingresantes 
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Reporte consolidado</returns>
        [HttpGet("consolidado/get")]
        public async Task<IActionResult> GetTotalByCareer(Guid termId)
        {
            var students = (await _studentService.GetStudentsByAdmissionTermPdfData(termId, null, null, null, User, null));

            var careers = students
                .GroupBy(x => x.Career.Name)
                .Select(x => new
                {
                    Name = x.Key,
                    Count = x.Count()
                }).ToList();

            var result = new
            {
                Chart = new
                {
                    Name = "Cant. Ingresantes",
                    Data = careers
                    .Select(x => new object[] {
                        x.Name,
                        x.Count
                    }).ToList()
                },

                Table = careers
                    .Select(x => new
                    {
                        Name = x.Name,
                        x.Count
                    }).ToList()
            };

            return Ok(result);
        }
    }
}
