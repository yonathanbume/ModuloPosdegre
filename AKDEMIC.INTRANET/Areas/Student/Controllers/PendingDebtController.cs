using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Hosting;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.INTRANET.Areas.Student.Models.PendingDebtsViewModels;
using System.IO;
using AKDEMIC.CORE.Services;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Hosting;
using System;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/deudas_pendientes")]
    public class PendingDebtController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfigurationService _configurationService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IStudentService _studentService;
        private readonly ITermService _termService;
        private readonly IViewRenderService _viewRenderService;
        private readonly IConverter _dinkConverter;

        public PendingDebtController(
            IUserService userService,
            IConfigurationService configurationService,
            IPaymentService paymentService,
            IWebHostEnvironment hostEnvironment,
            IStudentService studentService,
            ITermService termService,
            IViewRenderService viewRenderService,
            IConverter dinkConverter) : base(userService)
        {
            _paymentService = paymentService;
            _hostingEnvironment = hostEnvironment;
            _studentService = studentService;
            _termService = termService;
            _configurationService = configurationService;
            _viewRenderService = viewRenderService;
            _dinkConverter = dinkConverter;
        }

        /// <summary>
        /// Vista donde se muestran las deudas pendientes del alumno logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de las deudas pendientes del alumno logueado
        /// </summary>
        /// <returns>Listado de deudas pendientes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetPendingDebts()
        {
            var userId = _userService.GetUserIdByClaim(User);

            var configurationStudentPayment = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.IntranetManagement.ENABLE_STUDENT_PAYMENT);
            bool studentPaymentFeature = false;
            if (configurationStudentPayment != null)
            {
                studentPaymentFeature = bool.Parse(configurationStudentPayment.Value);
            }
            bool isDevelopment = _hostingEnvironment.IsDevelopment();
            var url = CORE.Helpers.GeneralHelpers.GetApplicationRoute(CORE.Helpers.ConstantHelpers.Solution.EconomicManagement, isDevelopment);
            var payments = await _paymentService.GetAllByUser(userId, ConstantHelpers.PAYMENT.STATUS.PENDING);

            var result = payments
                .Select(x => new
                {
                    type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.DESCRIPTION[x.Type],
                    code = x.ConceptId.HasValue ? x.Concept.Code : "-",
                    concept = x.Description,
                    import = x.Total,
                    discount = x.Discount,
                    issueDate = x.IssueDate.ToShortDateString(),
                    studentPaymentFeature = studentPaymentFeature,
                    paymentUrl = url,
                    userProcedureId = x.UserProcedures.Any() ? x.UserProcedures.Select(y=>y.Id).FirstOrDefault() : (Guid?)null
                }).ToList();

            return Ok(result);
        }


        /// <summary>
        /// Genera un reporte de las deudas pendientes del alumno logueado
        /// </summary>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("matricula/pdf")]
        public async Task<IActionResult> EnrollmentPaymentsPdf()
        {
            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);
            var term = await _termService.GetActiveTerm();

            var model = new ReportPdfViewModel
            {
                Image = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png"),
                Term = term != null ? term.Name.ToUpper() : "-",
                UserName = student.User.UserName.ToUpper(),
                FullName = student.User.FullName.ToUpper(),
                Career = student.Career.Name.ToUpper()
            };

            var payments = await _paymentService.GetAllByUser(userId, CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PENDING);
            payments = payments.Where(x => x.Type == CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT || x.Type == CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT).ToList();

            model.Payments = payments
                .Select(x => new PaymentViewModel
                {
                    Code = x.ConceptId.HasValue ? x.Concept.Code : "-",
                    Description = x.Description,
                    Amount = x.Total
                }).ToList();

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = $"ESCALA DE MATRICULA"
            };

            //return View(model);

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Student/Views/PendingDebt/EnrollmentPaymentsPdf.cshtml", model);

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileByte = _dinkConverter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileByte, "application/pdf", $"{model.UserName} - PAGOS.pdf");
        }


    }
}
