using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Areas.Academic.ViewModels.UnbeatenStudentViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Academic.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Academic")]
    [Route("academico/estudiantes-invictos")]
    public class UnbeatenStudentController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly AkdemicContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UnbeatenStudentController(
            IStudentService studentService,
            AkdemicContext context,
            IWebHostEnvironment webHostEnvironment,
            IDataTablesService dataTablesService
            ) : base(dataTablesService)
        {
            _studentService = studentService;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// Vista inicial del reporte de estudiantes invictos a la fecha
        /// </summary>
        /// <returns>Vista inicial</returns>
        public IActionResult Index() => View();
        /// <summary>
        /// Retorna la lista de estudiantes invictos según los filtros del usuario
        /// </summary>
        /// <param name="faculty">Id de la facultad</param>
        /// <param name="career">Id de la escuela</param>
        /// <param name="search">Término a filtrar</param>
        /// <returns>Objeto con la lista de estudiantes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStudents(Guid faculty, Guid career, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetUndefeatedStudentDatatable(sentParameters, faculty, career, search);

            return Ok(result);
        }

        [HttpGet("get-excel")]
        public async Task<IActionResult> GetstudentsDatatable(Guid? faculty, Guid? career)
        {
            var query = _context.Students
               .Where(x => x.Status == ConstantHelpers.Student.States.UNBEATEN).AsNoTracking();


            if (faculty != Guid.Empty && faculty.HasValue) query = query.Where(x => x.Career.FacultyId == faculty);

            if (career != Guid.Empty && career.HasValue) query = query.Where(x => x.CareerId == career);

            var data = await query
               .Select(x => new
               {
                   code = x.User.UserName,
                   name = x.User.FullName,
                   career = x.Career.Name,
                   faculty = x.Career.Faculty.Name,
                   academicYear = x.CurrentAcademicYear
               }).ToListAsync();


            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Código");
            dt.Columns.Add("Nombre Completo");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Facultad");
            dt.Columns.Add("Ciclo");

            foreach (var item in data)
                dt.Rows.Add(item.code, item.name, item.career, item.faculty, item.academicYear);

            dt.AcceptChanges();

            var img = Path.Combine(_webHostEnvironment.WebRootPath, @"images/themes/" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png");

            var fileName = $"Listado de estudiantes invictos.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Estudiantes", null);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        //[HttpGet("get-pdf")]
        //public async Task<ActionResult> GetStudentsPDF(Guid? faculty, Guid? career)
        //{
        //    var query = _context.Students
        //       .Where(x => x.Status == ConstantHelpers.Student.States.UNBEATEN).AsNoTracking();


        //    if (faculty != Guid.Empty && faculty.HasValue) query = query.Where(x => x.Career.FacultyId == faculty);

        //    if (career != Guid.Empty && career.HasValue) query = query.Where(x => x.CareerId == career);

        //    var data = await query
        //       .Select(x => new StudentViewModel
        //       {
        //           Username = x.User.UserName,
        //           FullName = x.User.FullName,
        //           Career = x.Career.Name,
        //           Faculty = x.Career.Faculty.Name,
        //           AcademicYear = x.CurrentAcademicYear
        //       }).ToListAsync();


        //}
    }
}
