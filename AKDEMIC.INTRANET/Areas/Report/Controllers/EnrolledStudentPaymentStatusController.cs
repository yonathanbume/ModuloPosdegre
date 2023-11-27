// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.INSTITUTIONAL_WELFARE)]
    [Area("Report")]
    [Route("reporte/estado-pagos")]
    public class EnrolledStudentPaymentStatusController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IPaymentService _paymentService;
        public EnrolledStudentPaymentStatusController(IDataTablesService dataTablesService,
            IPaymentService paymentService,
            IStudentService studentService) : base(dataTablesService)
        {
            _paymentService = paymentService;
            _studentService = studentService;
        }

        public IActionResult Index() => View();

        [HttpGet("get")]
        public async Task<IActionResult> GetPostPaymentEnrolledStudents(Guid termId, Guid? facultyId, Guid? careerId, byte? type, string search, int status)
        {
            if (termId == Guid.Empty)
            {
                var term = await _termService.GetActiveTerm();
                if (term == null) term = await _termService.GetLastTerm();

                termId = term != null ? term.Id : Guid.Empty;
            }
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetEnrolledStudentPaymentStatusDatatable(sentParameters, User, termId, facultyId, careerId, type, status, search);

            return Ok(result);
        }

        [HttpGet("pagos/get")]
        public async Task<IActionResult> GetStudentPostPayments(Guid termId, Guid studentId, byte? type)
        {
            if (termId == Guid.Empty)
            {
                var term = await _termService.GetActiveTerm();
                if (term == null) term = await _termService.GetLastTerm();

                termId = term != null ? term.Id : Guid.Empty;
            }

            var student = await _studentService.Get(studentId);
            var payments = await _paymentService.GetAllByUser(student.UserId);
            if (type.HasValue)
            {
                if (type.Value == 1)
                {
                    payments = payments
                        .Where(x => x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT
                        || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)
                        .ToList();
                }

                if (type.Value == 2)
                {
                    payments = payments
                        .Where(x => x.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT
                        && x.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)
                        .ToList();
                }
            }

            var result = payments
                .Select(x => new
                {
                    description = x.Description,
                    status = x.PaymentDate.HasValue ? "Pagado" : "Por pagar",
                    total = x.Total,
                    type = x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT
                    || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT ? "Matrícula" : "Otro",
                }).ToList();

            return Ok(result);
        }

        [HttpGet("reporte-excel")]
        public async Task<IActionResult> ExcelReport(Guid termId, Guid? facultyId, Guid? careerId, byte? type, string search, int status)
        {
            var students = await _studentService.GetEnrolledStudentPaymentStatusData(User, termId, facultyId, careerId, type, status, search);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Código");
            dt.Columns.Add("Nombre completo");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Plan");
            dt.Columns.Add("Créditos");
            dt.Columns.Add("Estado");

            foreach (var item in students.OrderBy(x => x.Career).ThenBy(x => x.Name).ToList())
                dt.Rows.Add(item.Code, item.Name, item.Career, item.Curriculum, item.Credits, item.Paid ? "Pagado" : "Por pagar");

            dt.AcceptChanges();

            var fileName = $"Reporte de Estado Pagos.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Listado de Estudiantes Matrículados", null, 6);
                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
