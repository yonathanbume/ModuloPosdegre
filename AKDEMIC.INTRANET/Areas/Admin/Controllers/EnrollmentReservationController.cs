using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Areas.Admin.Models.EnrollmentReservationViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/matricula/reservas")]
    public class EnrollmentReservationController : BaseController
    {
        private readonly IUserProcedureService _userProcedureService;
        private readonly IConverter _dinkConverter;
        private readonly IViewRenderService _viewRenderService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IStudentService _studentService;
        private readonly IEnrollmentReservationService _enrollmentReservationService;

        public EnrollmentReservationController(IDataTablesService dataTablesService,
            IUserProcedureService userProcedureService,
            IWebHostEnvironment hostingEnvironment,
            IViewRenderService viewRenderService,
            IStudentService studentService,
            IConverter dinkConverter,
            IEnrollmentReservationService enrollmentReservationService) : base(dataTablesService)
        {
            _userProcedureService = userProcedureService;
            _hostingEnvironment = hostingEnvironment;
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _studentService = studentService;
            _enrollmentReservationService = enrollmentReservationService;
        }

        /// <summary>
        /// Vista donde de listan las reservas de matrícula
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de reservas de matrícula
        /// </summary>
        /// <param name="term">Identificador del periodo académico</param>
        /// <param name="faculty">Identificador de la facultad</param>
        /// <param name="career">Identificador de la escuela profesional</param>
        /// <returns>Objeto con el listado de reservas</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid? term = null, Guid? faculty = null, Guid? career = null)
        {
            var sentParameters = GetSentParameters();
            var result = await _enrollmentReservationService.GetEnrollmentReservationsDatatable(sentParameters, null, term, faculty, career);
            return Ok(result);
        }

        /// <summary>
        /// Método para aceptar un trámite del usuario
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del trámite</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("aceptar/post")]
        [HttpPost]
        public async Task<IActionResult> AcceptUserProcedure(EnrollmentReservationViewModel model)
        {
            var userProcedure = await _userProcedureService.Get(model.Id);
            userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.ACCEPTED;
            await _userProcedureService.Update(userProcedure);
            return Ok();
        }

        /// <summary>
        /// Método para denegar un trámite del usuario
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del trámite</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("denegar/post")]
        [HttpPost]
        public async Task<IActionResult> DenyUserProcedure(EnrollmentReservationViewModel model)
        {
            var userProcedure = await _userProcedureService.Get(model.Id);
            userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.NOT_APPLICABLE;
            await _userProcedureService.Update(userProcedure);
            return Ok();
        }

        /// <summary>
        /// Método para descargar la constancia de reserva de matrícula
        /// </summary>
        /// <param name="id">Identificador de la reserva de matrícula</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("descargar-pdf/{id}")]
        public async Task<IActionResult> DownloadPDF(Guid id)
        {
            try
            {
                var viewModel = new EnrollmentReservationPDF();
                var enrollmentReservation = await _enrollmentReservationService.Get(id);
                var student = await _studentService.GetWithIncludes(enrollmentReservation.StudentId);
                viewModel.FullName = student.User.FullName;
                viewModel.Dni = student.User.Dni;
                viewModel.UserName = student.User.UserName;
                viewModel.Curriculum = student.Curriculum.Name;
                viewModel.AcademicProgram = student.AcademicProgram != null ? student.AcademicProgram.Name : "--";
                viewModel.CurrentAcademicYear = $"{student.CurrentAcademicYear}";
                viewModel.Observation = enrollmentReservation.Observation;
                viewModel.Date = enrollmentReservation.ExpirationDate.ToLocalDateFormat();

                var viewToString = "";

                viewToString = await _viewRenderService.RenderToStringAsync($"/Areas/Admin/Views/EnrollmentReservation/PDF.cshtml", viewModel);

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

                var globalSettings = new DinkToPdf.GlobalSettings();

                globalSettings.ColorMode = DinkToPdf.ColorMode.Color;
                globalSettings.Orientation = DinkToPdf.Orientation.Portrait;
                globalSettings.PaperSize = DinkToPdf.PaperKind.A4;
                globalSettings.Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 30, Left = 10, Right = 10 };

                var pdf = new DinkToPdf.HtmlToPdfDocument
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                var fileByte = _dinkConverter.Convert(pdf);

                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

                return File(fileByte, "application/pdf", "archivo.pdf");
            }
            catch (Exception e)
            {
                return BadRequest("No se pudo descargar el archivo");
            }
        }
    }
}
