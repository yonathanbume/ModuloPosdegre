using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.INTRANET.Areas.Admin.Models.UserAssistanceViewModels;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using ClosedXML.Excel;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE)]
    [Area("Admin")]
    [Route("admin/asistencia-personal")]
    public class UserAssistanceController : BaseController
    {
        private readonly IWorkingDayService _workingDayServices;

        public UserAssistanceController(IWorkingDayService workingDayService,
            IUserService userService) : base(userService)
        {
            _workingDayServices = workingDayService;
        }

        /// <summary>
        /// Método para cargar la asistencia del personal masivamente
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Método para cargar masivamente la asistencia del personal
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de asistencia del personal</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("guardar/post")]
        public async Task<IActionResult> SaveWorkingDays(WorkingDayViewModel[] model)
        {
            var list = new List<WorkingDay>();
            var term = await GetActiveTerm();

            if (model == null || !model.Any())
                return BadRequest("Ningún registro enviado.");
            return Ok();
        }

        /// <summary>
        /// Método para descargar la plantilla de carga masiva
        /// </summary>
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("plantilla/get")]
        public async Task GetTemplate()
        {
            using (var mem = new MemoryStream())
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Asistencia de Personal");
                    worksheet.Cell("A1").Value = "Código";
                    worksheet.Cell("B1").Value = "Personal";
                    worksheet.Cell("C1").Value = "Roles Administrativos";
                    worksheet.Cell("D1").Value = "Fecha de Registro";
                    worksheet.Cell("E1").Value = "Hora de Entrada (Turno Mañana)";
                    worksheet.Cell("F1").Value = "Hora de Salida (Turno Mañana)";
                    worksheet.Cell("G1").Value = "Marcado de Entrada (Turno Mañana)";
                    worksheet.Cell("H1").Value = "Marcado de Salida (Turno Mañana)";
                    worksheet.Cell("I1").Value = "Hora de Entrada (Turno Tarde)";
                    worksheet.Cell("J1").Value = "Hora de Salida (Turno Tarde)";
                    worksheet.Cell("K1").Value = "Marcado de Entrada (Turno Tarde)";
                    worksheet.Cell("L1").Value = "Marcado de Salida (Turno Tarde)";


                    worksheet.Row(1).Style.Font.Bold = true;
                    worksheet.Columns(1, 12).AdjustToContents();
                    workbook.SaveAs(mem);
                }

                // Download file                
                var text = string.Format("attachment;filename=\"{0}.xlsx\"", "Plantilla de Asistencia de Personal");
                Response.Headers["Content-Disposition"] = text;
                Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                mem.Position = 0;
                await mem.CopyToAsync(HttpContext.Response.Body);
            }
        }
    }
}
