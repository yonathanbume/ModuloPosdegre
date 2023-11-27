// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation;
using AKDEMIC.INTRANET.Areas.AcademicRecord.Models.Request;
using AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.PDF.Services.CertificateGenerator;
using AKDEMIC.PDF.Services.CertificateMeritOrderGenerator;
using AKDEMIC.PDF.Services.CertificateOfStudiesGenerator;
using AKDEMIC.PDF.Services.CompleteCurriculumGenerator;
using AKDEMIC.PDF.Services.ReportCardGenerator;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Controllers
{
    [Area("AcademicRecord")]
    [Route("registrosacademicos/solicitudes")]
    [Authorize]
    //[Authorize(Roles =
    //    CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," +
    //    CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_RECORD + "," +
    //    CORE.Helpers.ConstantHelpers.ROLES.STUDENTS + "," +
    //    CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF)]
    public class RequestController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IViewRenderService _viewRenderService;
        private readonly IConverter _dinkConverter;
        private readonly IPostulantService _postulantService;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IRecordHistoryService _recordHistoryService;
        private readonly IInternalProcedureService _internalProcedureService;
        private readonly IDataTablesService _dataTablesService;
        private readonly IRecordsConceptService _recordsConceptService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IDocumentFormatService _documentFormatService;
        private readonly ICertificateMeritOrderGeneratorService _certificateMeritOrderGeneratorService;
        private readonly ICertificateOfStudiesGeneratorService _certificateOfStudiesGeneratorService;
        private readonly AKDEMIC.PDF.Services.AcademicRecordGenerator.IAcademicRecordGeneratorService _academicRecordGeneratorService;
        private readonly ICompleteCurriculumGeneratorService _completeCurriculumGeneratorService;
        private readonly IReportCardGeneratorService _reportCardGeneratorService;
        private readonly IPaymentService _paymentService;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly ITextSharpService _textSharpService;
        private readonly IUserInternalProcedureService _userInternalProcedureService;
        private readonly IConfigurationService _configurationService;
        private readonly IFacultyService _facultyService;
        private readonly IConceptService _conceptService;
        private readonly AkdemicContext _context;
        private readonly ICertificateGeneratorService _certificateGeneratorService;
        private readonly IRecordHistoryObservationService _recordHistoryObservationService;
        private readonly IUserService _userService;
        private readonly IYearInformationService _yearInformationService;

        public RequestController(IUserService userService,
            IStudentService studentService, IMapper mapper, IWebHostEnvironment hostingEnvironment,
            IViewRenderService viewRenderService, IConverter dinkConverter, IPostulantService postulantService,
            IAcademicSummariesService academicSummariesService, IAcademicHistoryService academicHistoryService,
            IRecordHistoryService recordHistoryService, IInternalProcedureService internalProcedureService,
            IDataTablesService dataTablesService, ITermService termService,
            IRecordsConceptService recordsConceptService,
            IStudentSectionService studentSectionService,
            IInvoiceService invoiceService,
            IDocumentFormatService documentFormatService,
            ICertificateMeritOrderGeneratorService certificateMeritOrderGeneratorService,
            ICertificateOfStudiesGeneratorService certificateOfStudiesGeneratorService,
            AKDEMIC.PDF.Services.AcademicRecordGenerator.IAcademicRecordGeneratorService academicRecordGeneratorService,
            ICompleteCurriculumGeneratorService completeCurriculumGeneratorService,
            IReportCardGeneratorService reportCardGeneratorService,
            IPaymentService paymentService,
            ICloudStorageService cloudStorageService,
            ITextSharpService textSharpService,
            IUserInternalProcedureService userInternalProcedureService,
            IConfigurationService configurationService,
            IFacultyService facultyService,
            IConceptService conceptService,
            AkdemicContext context,
            ICertificateGeneratorService certificateGeneratorService,
            IYearInformationService yearInformationService,
            IRecordHistoryObservationService recordHistoryObservationService) : base(termService)
        {
            _userService = userService;
            _studentService = studentService;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _viewRenderService = viewRenderService;
            _dinkConverter = dinkConverter;
            _postulantService = postulantService;
            _academicSummariesService = academicSummariesService;
            _academicHistoryService = academicHistoryService;
            _recordHistoryService = recordHistoryService;
            _internalProcedureService = internalProcedureService;
            _dataTablesService = dataTablesService;
            _recordsConceptService = recordsConceptService;
            _studentSectionService = studentSectionService;
            _documentFormatService = documentFormatService;
            _certificateMeritOrderGeneratorService = certificateMeritOrderGeneratorService;
            _certificateOfStudiesGeneratorService = certificateOfStudiesGeneratorService;
            _academicRecordGeneratorService = academicRecordGeneratorService;
            _completeCurriculumGeneratorService = completeCurriculumGeneratorService;
            _reportCardGeneratorService = reportCardGeneratorService;
            _paymentService = paymentService;
            _cloudStorageService = cloudStorageService;
            _textSharpService = textSharpService;
            _userInternalProcedureService = userInternalProcedureService;
            _configurationService = configurationService;
            _facultyService = facultyService;
            _conceptService = conceptService;
            _context = context;
            _certificateGeneratorService = certificateGeneratorService;
            _yearInformationService = yearInformationService;
            _recordHistoryObservationService = recordHistoryObservationService;
        }

        /// <summary>
        /// Vista del listado de solicitudes
        /// </summary>
        /// <returns>Vista principal del sistema</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de solicitudes para ser usado en tablas
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <param name="status">Estado</param>
        /// <returns>Listado de solicitudes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(string search, int? status)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _recordHistoryService.GetRecordHistoriesDataTable(parameters, User, status, search);
            return Ok(result);
        }

        /// <summary>
        /// Método para actualizar el recibo de una solicitud
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del recibo</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("actualizar-recibo")]
        public async Task<IActionResult> UpdateReceipt(UpdateRequestViewModel model)
        {
            //if (string.IsNullOrEmpty(model.Receipt))
            //    return BadRequest("Es necesario ingresar el recibo.");

            //var entity = await _recordHistoryService.Get(model.RecordHistoryId);

            //if (entity is null)
            //    return BadRequest("No se encontró la solicitud.");

            //if (entity.StudentId.HasValue)
            //{
            //    var conceptId = await _recordsConceptService.GetValueByRecordType(entity.Type);

            //    if (conceptId == null)
            //        return BadRequest("El tipo de solicitud no tiene un concepto asignado.");

            //    var student = await _studentService.Get(entity.StudentId.Value);
            //    var user = await _userService.Get(student.UserId);
            //    var payments = await _paymentService.GetAllByUser(user.Id, ConstantHelpers.PAYMENT.STATUS.PAID);
            //    var paymentsByConcept = payments.Where(x => x.ConceptId == conceptId).ToList();
            //    var records = await _recordHistoryService.GetAllByStudentId(entity.StudentId.Value);
            //    var paymentPendings = paymentsByConcept.Where(x => !records.Any(y => y.PaymentId == x.Id)).ToList();

            //    if (!paymentPendings.Any())
            //        return BadRequest($"No se han encontrado pagos realizados por el alumno {user.UserName}");

            //    var paymentPeding = paymentPendings.Where(y => $"{y.Invoice.Series}-{y.Invoice.Number:00000000}" == model.Receipt).FirstOrDefault();

            //    if (paymentPeding == null)
            //        return BadRequest($"No se encontró algún pago con las serie {model.Receipt}");

            //    entity.PaymentId = paymentPendings.FirstOrDefault().Id;
            //    entity.ReceiptCode = model.Receipt;
            //    await _recordHistoryService.Update(entity);
            //    return Ok();
            //}

            //return BadRequest("La solicitud no tiene un alumno relacionado.");

            //if (string.IsNullOrEmpty(model.Receipt))
            //    return BadRequest("Es necesario ingresar el recibo.");
            //
            //var entity = await _recordHistoryService.Get(model.RecordHistoryId);
            //
            //if (entity is null)
            //    return BadRequest("No se encontró la solicitud.");
            //
            //entity.ReceiptCode = model.Receipt;
            //await _recordHistoryService.Update(entity);
            return Ok();
        }

        /// <summary>
        /// Método para imprimir la constancia solicitada
        /// </summary>
        /// <param name="id">Identificador de la solicitud</param>
        /// <param name="preview">¿vista previa?</param>
        /// <param name="withBackground">¿Con fondo?</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("imprimir/{id}")]
        public async Task<IActionResult> PrintConstancy(Guid id, bool preview, bool withBackground, Guid? userProcedureId)
        {
            var record = await _recordHistoryService.Get(id);
            ////

            //var record = await _recordHistoryService.Get(id);
            var number = $"{record.Number.ToString().PadLeft(5, '0')}-{record.Date.Year}";
            //record.StudentId = Guid.Parse("C80F8AE8-3BC0-4643-85E1-000D9E7AD674");
            Tuple<string, byte[]> result = new Tuple<string, byte[]>(null, null);

            if (/*string.IsNullOrEmpty(record.FileURL) && */!preview)
            {
                switch (record.Type)
                {
                    case CORE.Helpers.ConstantHelpers.RECORDS.STUDYRECORD:
                        {
                            var resultGenerator = await _certificateGeneratorService.GeneratePdf(ConstantHelpers.RECORDS.STUDYRECORD, record.Id, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;

                    case CORE.Helpers.ConstantHelpers.RECORDS.PROOFONINCOME:
                        {
                            var resultGenerator = await _certificateGeneratorService.GeneratePdf(ConstantHelpers.RECORDS.PROOFONINCOME, record.Id, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;
                    case CORE.Helpers.ConstantHelpers.RECORDS.ENROLLMENT:
                        {
                            var resultGenerator = await _certificateGeneratorService.GeneratePdf(ConstantHelpers.RECORDS.ENROLLMENT, record.Id, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;
                    case CORE.Helpers.ConstantHelpers.RECORDS.REGULARSTUDIES:
                        {
                            var resultGenerator = await _certificateGeneratorService.GeneratePdf(ConstantHelpers.RECORDS.REGULARSTUDIES, record.Id, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;

                    case CORE.Helpers.ConstantHelpers.RECORDS.EGRESS:
                        {
                            var resultGenerator = await _certificateGeneratorService.GeneratePdf(ConstantHelpers.RECORDS.EGRESS, record.Id, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;

                    case CORE.Helpers.ConstantHelpers.RECORDS.MERITCHART:
                        {
                            result = await MeritChart(record.StudentId.Value, number, record.Id, record.ReceiptCode);
                        }
                        break;

                    case CORE.Helpers.ConstantHelpers.RECORDS.UPPERFIFTH:
                        {
                            var resultGenerator = await _certificateGeneratorService.GeneratePdf(ConstantHelpers.RECORDS.UPPERFIFTH, record.Id, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;

                    case CORE.Helpers.ConstantHelpers.RECORDS.UPPERTHIRD:
                        {
                            var resultGenerator = await _certificateGeneratorService.GeneratePdf(ConstantHelpers.RECORDS.UPPERTHIRD, record.Id, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;

                    case CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICRECORD:
                        {
                            var resultGenerator = await _academicRecordGeneratorService.GetAcademicRecordPDF(record.StudentId.Value, userProcedureId, record.Id);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }

                        //result = await AcademicRecord(record.StudentId.Value, number, record.Id, record.ReceiptCode);
                        break;

                    case CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICPERFORMANCESUMMARY:
                        {
                            result = await AcademicPerformanceSummary(record.StudentId.Value, number, record.Id, record.ReceiptCode);
                        }
                        break;

                    case CORE.Helpers.ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIES:
                        {
                            var resultGenerator = await _certificateOfStudiesGeneratorService.GetCertificateOfStudiesPDF(record.StudentId.Value, userProcedureId, record.Id);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        //result = await Certifcate(record.StudentId.Value, number, record.Id, record.ReceiptCode);
                        break;
                    case CORE.Helpers.ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIESPARTIAL:
                        {
                            var resultGenerator = await _certificateOfStudiesGeneratorService.GetCertificateOfStudiesPDF(record.StudentId.Value, userProcedureId, record.Id);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        //result = await CertifcatePartial(record.StudentId.Value, number, record.Id, record.StartAcademicYear, record.EndAcademicYear, record.StartTermId, record.EndTermId);
                        break;

                    case CORE.Helpers.ConstantHelpers.RECORDS.COMPLETE_CURRICULUM:
                        {
                            var resultGenerator = await _completeCurriculumGeneratorService.GetCompleteCurriculumPDF(record.StudentId.Value, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;

                    case CORE.Helpers.ConstantHelpers.RECORDS.FIRST_ENROLLMENT:
                        {
                            var resultGenerator = await _certificateGeneratorService.GeneratePdf(ConstantHelpers.RECORDS.FIRST_ENROLLMENT, record.Id, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;
                    case CORE.Helpers.ConstantHelpers.RECORDS.REPORT_CARD:
                        {
                            var resultGenerator = await _reportCardGeneratorService.GetReportCardPDF(record.StudentId.Value, record.RecordTermId.Value, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;
                    case CORE.Helpers.ConstantHelpers.RECORDS.CERTIFICATE_MERIT_ORDER:
                        {
                            var resultGenerator = await _certificateMeritOrderGeneratorService.GetCertificateMeritOrderPDF(record.Id, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;
                    case CORE.Helpers.ConstantHelpers.RECORDS.NODEBT:
                        {
                            var resultGenerator = await _certificateGeneratorService.GeneratePdf(ConstantHelpers.RECORDS.NODEBT, record.Id, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;
                    case CORE.Helpers.ConstantHelpers.RECORDS.CURRICULUM_REVIEW:
                        {
                            result = await CurriculumReview(record.StudentId.Value);
                        }
                        break;
                    case CORE.Helpers.ConstantHelpers.RECORDS.TENTH_HIGHER:
                        {
                            var resultGenerator = await _certificateGeneratorService.GeneratePdf(ConstantHelpers.RECORDS.TENTH_HIGHER, record.Id, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;

                    case CORE.Helpers.ConstantHelpers.RECORDS.UPPER_MIDDLE:
                        {
                            var resultGenerator = await _certificateGeneratorService.GeneratePdf(ConstantHelpers.RECORDS.UPPER_MIDDLE, record.Id, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;

                    case CORE.Helpers.ConstantHelpers.RECORDS.NOT_BE_PENALIZED:
                        {
                            var resultGenerator = await _certificateGeneratorService.GeneratePdf(ConstantHelpers.RECORDS.NOT_BE_PENALIZED, record.Id, userProcedureId);
                            result = new Tuple<string, byte[]>($"{resultGenerator.PdfName}.pdf", resultGenerator.Pdf);
                        }
                        break;
                    case CORE.Helpers.ConstantHelpers.RECORDS.ENROLLMENT_REPORT:
                        {
                            result = await EnrollmentReport(record.StudentId.Value, record.RecordTermId.Value );
                        }
                        break;
                    default:
                        return BadRequest("No se puede imprimir el tipo de constancia seleccionado.");
                }
            }

            if (withBackground)
            {
                var filePdf = result.Item2;
                _textSharpService.AddImageWatermarkToAllPages(ref filePdf, Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{@GeneralHelpers.GetTheme()}/logo-report.png"));
                result = new Tuple<string, byte[]>(result.Item1, filePdf);
            }

            if (!preview)
            {
                //record.FileURL = await _cloudStorageService.UploadFile(new MemoryStream(result.Item2), ConstantHelpers.CONTAINER_NAMES.ACADEMICRECORDS, ".pdf", ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                record.QuantityPrinted = record.QuantityPrinted++;
                await _recordHistoryService.Update(record);
                return ReturnFile(result.Item2, result.Item1);
            }

            return Ok(result.Item2);

        }

        /// <summary>
        /// Vista para guardar información extra de la solicitud
        /// </summary>
        /// <param name="id">Identificador de la solicitud</param>
        /// <returns>Vista</returns>
        [HttpGet("guardar-informacion/{id}")]
        public async Task<IActionResult> SaveInfo(Guid id)
        {
            var record = await _recordHistoryService.Get(id);
            var student = await _studentService.GetStudentWithCareerAcademicUser(record.StudentId.Value);
            var faculty = await _facultyService.Get(student.Career.FacultyId);
            var model = new SaveRequestDataViewModel
            {
                Student = student.User.FullName,
                Faculty = faculty.Name,
                Career = student.Career.Name,
                AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper(),
                Username = student.User.UserName,
                Id = record.Id,
                Type = record.Type
            };

            return View(model);
        }

        /// <summary>
        /// Método para guardar la información adicional de una constancia y descargarla
        /// </summary>
        /// <param name="model">Objeto que contiene los datos adicionales</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("guardar-archivo")]
        public async Task<IActionResult> SaveAndDowndloadFile(SaveRequestDataViewModel model)
        {
            var record = await _recordHistoryService.Get(model.Id);
            var number = $"{record.Number.ToString().PadLeft(5, '0')}-{record.Date.Year}";

            Tuple<string, byte[]> result = new Tuple<string, byte[]>(null, null);

            switch (record.Type)
            {
                case CORE.Helpers.ConstantHelpers.RECORDS.MERITCHART:
                    result = await MeritChart(record.StudentId.Value, number, Guid.Empty, null, model);
                    break;
                case CORE.Helpers.ConstantHelpers.RECORDS.UPPERFIFTH:
                    result = await UpperFifth(record.StudentId.Value, number, Guid.Empty, null, model);
                    break;
                case CORE.Helpers.ConstantHelpers.RECORDS.UPPERTHIRD:
                    result = await UpperThird(record.StudentId.Value, number, Guid.Empty, null, model);
                    break;
            }

            record.FileURL = await _cloudStorageService.UploadFile(new MemoryStream(result.Item2), ConstantHelpers.CONTAINER_NAMES.ACADEMICRECORDS, ".pdf", ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            record.QuantityPrinted = record.QuantityPrinted++;

            await _recordHistoryService.Update(record);
            return Ok(record.FileURL);
        }

        #region - CONSTANCIAS - 

        private async Task<Tuple<string, byte[]>> EnrollmentReport(Guid id, Guid termId)
        {
            var url = string.Empty;
            if (ConstantHelpers.Solution.Routes.Keys.Contains(ConstantHelpers.GENERAL.Institution.Value))
            {
                var baseUrl = ConstantHelpers.Solution.Routes[ConstantHelpers.GENERAL.Institution.Value][CORE.Helpers.ConstantHelpers.Solution.Enrollment];
                url = $"{baseUrl}admin/matricula/alumno/{id}/detalle-cursos-matriculados";
            }

            var template = await _studentSectionService.GetEnrollmentReportTemplate(id, termId, pronabec: false, qrUrl: url, includeExtraordinaryEvaluations: true);
            template.Image = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png");

            var signatureImageUrl = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IMAGE_CERTIFICATE_SIGNATURE);
            var signatureBase64 = string.Empty;

            if (!string.IsNullOrEmpty(signatureImageUrl))
            {
                using (var mem = new MemoryStream())
                {
                    await _cloudStorageService.TryDownload(mem, ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION, signatureImageUrl);
                    signatureBase64 = $"data:image/png;base64, {Convert.ToBase64String(mem.ToArray())}";
                }
            }

            template.SignatuareImgBase64 = signatureBase64;
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.Letter,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 15, Right = 15 },
                DPI = 290
            };

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/Pdf/EnrollmentReport.cshtml", template);
            var cssPtah = Path.Combine(_hostingEnvironment.WebRootPath, @"css/areas/academic/studentinformation/enrollmentreport.css");

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPtah },
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
            var fileDownloadName = HttpUtility.UrlEncode($"Ficha-Matricula.pdf");

            return new Tuple<string, byte[]>(fileDownloadName, fileByte);
        }


        //CERTIFICADO DE ESTUDIOS
        /// <summary>
        /// Genera el certificado de estudio
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <param name="number">Número de solicitud</param>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <param name="receiptCode">Código de recibo</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Nombre de archivo y arreglo de bytes del archivo</returns>
        private async Task<Tuple<string, byte[]>> StudyRecord(Guid id, string number, Guid recordId, string receiptCode, Guid? termId)
        {

            var student = await _studentService.GetStudentRecordEnrollment(id);
            var universityinfo = await GetUniversityInformation();


            var record = new RecordOfRegularStudiesViewModel
            {
                Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                Career = student.Career.Name,
                Faculty = student.Career.Faculty.Name,
                StudentSex = student.User.Sex,
                StudentName = student.User.RawFullName,
                EnrollmentNumber = student.User.UserName,
                IncomeYear = student.AdmissionTerm.Name,
                Semester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.TEXT[student.CurrentAcademicYear] : student.CurrentAcademicYear + "°",
                StartDate = student.AdmissionTerm.StartDate.ToString("dd/MM/yyyy"),
                StudentCondition = CORE.Helpers.ConstantHelpers.Student.States.VALUES[student.Status],
                PageCode = number,
                ImageQR = GetImageQR(recordId),
                University = universityinfo,
            };

            if (termId.HasValue)
            {
                var term = await _termService.Get(termId.Value);
                record.Term = term != null ? term.Name : "...";
            }

            var documentFormat = await _documentFormatService.GetParsedDocumentFormat(ConstantHelpers.RECORDS.STUDYRECORD, id, termId);

            if (documentFormat != null)
            {
                record.CustomTitle = documentFormat.Title;
                record.CustomContent = documentFormat.Content;
            }

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/RecordOfStudies.cshtml", record);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/recordofenrollment.css");
            string fullhtml = await GetFullHtmlContent(recordId, htmlContent);
            ObjectSettings objectSettings = GetObjectSettings(number, receiptCode, fullhtml, userStyleSheet);

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Bottom = 15,
                        Left = 15,
                        Right = 15,
                        Top = 10,
                    },
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);

            var confiImageWatermark = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IMAGE_WATERMARK_RECORD);

            if (!string.IsNullOrEmpty(confiImageWatermark))
                _textSharpService.AddImageWatermarkToAllPages(ref fileContents, $"{GeneralHelpers.GetApplicationRoute(ConstantHelpers.Solution.Intranet, false)}/imagenes/{confiImageWatermark}");

            var fileDownloadName = HttpUtility.UrlEncode($"Constancia-Estudios.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileContents);
        }

        /// <summary>
        /// Genera el comprobante de ingresos
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <param name="number">Número de solicitud</param>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <param name="receiptCode">Código de recibo</param>
        /// <returns>Nombre de archivo y arreglo de bytes del archivo</returns>
        private async Task<Tuple<string, byte[]>> GetProofOfIncome(Guid id, string number, Guid recordId, string receiptCode)
        {
            var student = await _studentService.GetStudentProofOfInCome(id);

            var postulant = await _postulantService.GetPostulantByDniAndTerm(student.User.Dni, student.AdmissionTermId);
            var studentsCount = await _postulantService.GetStudentCounttByTermAndCareer(student.AdmissionTermId, student.CareerId);

            var universityinfo = await GetUniversityInformation();

            var proof = new ProofOfIncomeViewModel
            {
                Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                Career = student.Career.Name,
                Faculty = student.Career.Faculty.Name,
                StudentSex = student.User.Sex,
                StudentName = student.User.RawFullName,
                EnrollmentNumber = student.User.UserName,
                Modality = student.AdmissionType.Name,
                IncomeYear = student.AdmissionTerm.Year,
                AmountOfStudents = studentsCount,
                Place = (postulant == null) ? 0 : postulant.OrderMerit,
                Score = (postulant == null) ? 0 : postulant.FinalScore,
                PageCode = number,
                ImageQR = GetImageQR(recordId),
                University = universityinfo
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/ProofOfIncome.cshtml", proof);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/recordofenrollment.css");

            string fullhtml = await GetFullHtmlContent(recordId, htmlContent);
            ObjectSettings objectSettings = GetObjectSettings(number, receiptCode, fullhtml, userStyleSheet);

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Bottom = 15,
                        Left = 15,
                        Right = 15,
                        Top = 10,
                    },
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);

            var confiImageWatermark = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IMAGE_WATERMARK_RECORD);

            if (!string.IsNullOrEmpty(confiImageWatermark))
                _textSharpService.AddImageWatermarkToAllPages(ref fileContents, $"{GeneralHelpers.GetApplicationRoute(ConstantHelpers.Solution.Intranet, false)}/imagenes/{confiImageWatermark}");

            var fileDownloadName = HttpUtility.UrlEncode($"Constancia-Ingreso.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileContents);
        }

        /// <summary>
        /// Genera el registro de inscripción
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <param name="number">Número de solicitud</param>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <param name="receiptCode">Código de recibo</param>
        /// <returns>Nombre de archivo y arreglo de bytes del archivo</returns>
        private async Task<Tuple<string, byte[]>> GetRecordEnrollment(Guid id, string number, Guid recordId, string receiptCode, Guid? recordTermId)
        {

            var term = new Term();

            if (recordTermId.HasValue)
                term = await _termService.Get(recordTermId.Value);
            else
                term = await _termService.GetActiveTerm();

            bool canParse = int.TryParse(term.Number, out int semesterNumber);

            var student = await _studentService.GetStudentRecordEnrollment(id);
            var universityinfo = await GetUniversityInformation();

            var incomeYearNumber = int.TryParse(term.Number, out var tryNumber);

            var record = new RecordOfEnrollmentViewModel
            {
                Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                Career = student.Career.Name,
                Faculty = student.Career.Faculty.Name,
                StudentSex = student.User.Sex,
                StudentName = student.User.RawFullName,
                EnrollmentNumber = student.User.UserName,
                IncomeYear = term.Year.ToString(),
                IncomeYearNumber = incomeYearNumber ? tryNumber : 0,
                Semester = ConstantHelpers.ACADEMIC_YEAR.TEXT.ContainsKey(semesterNumber)
                            ? ConstantHelpers.ACADEMIC_YEAR.TEXT[semesterNumber].ToUpper() : "",
                StartDate = term.EnrollmentStartDate.ToString("dd/MM/yyyy"),
                PageCode = number,
                ImageQR = GetImageQR(recordId),
                University = universityinfo
            };

            var documentFormat = await _documentFormatService.GetParsedDocumentFormat(ConstantHelpers.RECORDS.ENROLLMENT, id);

            if (documentFormat != null)
            {
                record.CustomTitle = documentFormat.Title;
                record.CustomContent = documentFormat.Content;
            }

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/RecordOfEnrollment.cshtml", record);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/recordofenrollment.css");

            string fullhtml = await GetFullHtmlContent(recordId, htmlContent);
            ObjectSettings objectSettings = GetObjectSettings(number, receiptCode, fullhtml, userStyleSheet);

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Bottom = 15,
                        Left = 15,
                        Right = 15,
                        Top = 10,
                    },
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { objectSettings }
            };

            //var htmlToPdfDocument = new HtmlToPdfDocument
            //{
            //    GlobalSettings = GetGlobalSettings(),
            //    Objects = { objectSettings }
            //};

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);

            var confiImageWatermark = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IMAGE_WATERMARK_RECORD);

            if (!string.IsNullOrEmpty(confiImageWatermark))
                _textSharpService.AddImageWatermarkToAllPages(ref fileContents, $"{GeneralHelpers.GetApplicationRoute(ConstantHelpers.Solution.Intranet, false)}/imagenes/{confiImageWatermark}");

            var fileDownloadName = HttpUtility.UrlEncode($"Constancia-Matricula.pdf");

            return new Tuple<string, byte[]>(fileDownloadName, fileContents);
        }

        /// <summary>
        /// Genera el registro de estudios regulares
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <param name="number">Número de solicitud</param>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <param name="receiptCode">Código de recibo</param>
        /// <returns>Nombre de archivo y arreglo de bytes del archivo</returns>
        private async Task<Tuple<string, byte[]>> GetRecordOfRegularStudies(Guid id, string number, Guid recordId, string receiptCode)
        {
            var student = await _studentService.GetStudentRecordEnrollment(id);
            var universityinfo = await GetUniversityInformation();

            var record = new RecordOfRegularStudiesViewModel
            {
                Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                Career = student.Career.Name,
                Faculty = student.Career.Faculty.Name,
                StudentSex = student.User.Sex,
                StudentName = student.User.RawFullName,
                EnrollmentNumber = student.User.UserName,
                IncomeYear = student.AdmissionTerm.Name,
                Semester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.TEXT[student.CurrentAcademicYear] : student.CurrentAcademicYear + "°",
                StartDate = student.AdmissionTerm.StartDate.ToString("dd/MM/yyyy"),
                StudentCondition = CORE.Helpers.ConstantHelpers.Student.States.VALUES[student.Status],
                PageCode = number,
                ImageQR = GetImageQR(recordId),
                University = universityinfo
            };

            var documentFormat = await _documentFormatService.GetParsedDocumentFormat(ConstantHelpers.RECORDS.REGULARSTUDIES, id);

            if (documentFormat != null)
            {
                record.CustomTitle = documentFormat.Title;
                record.CustomContent = documentFormat.Content;
            }


            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/RecordOfRegularStudies.cshtml", record);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/recordofenrollment.css");
            string fullhtml = await GetFullHtmlContent(recordId, htmlContent);
            ObjectSettings objectSettings = GetObjectSettings(number, receiptCode, fullhtml, userStyleSheet);

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Bottom = 15,
                        Left = 15,
                        Right = 15,
                        Top = 10,
                    },
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Constancia-Estudios-Regulares.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileContents);
        }

        /// <summary>
        /// Genera el registro de egreso
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <param name="number">Número de solicitud</param>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <param name="receiptCode">Código de recibo</param>
        /// <returns>Nombre de archivo y arreglo de bytes del archivo</returns>
        private async Task<Tuple<string, byte[]>> GetRecordOfEgress(Guid id, string number, Guid recordId, string receiptCode)
        {
            var student = await _studentService.GetStudentRecordEnrollment(id);
            var universityInfo = await GetUniversityInformation();

            var approvedCredits = await _studentService.GetApprovedCreditsByStudentId(student.Id);

            var record = new RecordOfEgressViewModel
            {
                Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                Career = student.Career.Name,
                Faculty = student.Career.Faculty.Name,
                StudentSex = student.User.Sex,
                StudentName = student.User.RawFullName,
                TotalCredits = approvedCredits,
                EnrollmentNumber = student.User.UserName,
                EndDate = student.GraduationTerm?.EndDate.ToString("dd/MM/yyyy"),
                PageCode = number,
                ImageQR = GetImageQR(recordId),
                University = universityInfo
            };

            if (student.GraduationTerm != null)
            {
                var tryPars = int.TryParse(student.GraduationTerm.Number, out var gradutionTermNumber);

                record.GraduatedYear = student.GraduationTerm.Year.ToString();
                record.GraduatedYearNumber = gradutionTermNumber;
            }

            var documentFormat = await _documentFormatService.GetParsedDocumentFormat(ConstantHelpers.RECORDS.EGRESS, id);

            if (documentFormat != null)
            {
                record.CustomTitle = documentFormat.Title;
                record.CustomContent = documentFormat.Content;
            }

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/RecordOfEgress.cshtml", record);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/recordofenrollment.css");
            //var generatedBy = await _userService.GetByUserName(HttpContext.User.Identity.Name);
            string fullhtml = await GetFullHtmlContent(recordId, htmlContent);
            ObjectSettings objectSettings = GetObjectSettings(number, receiptCode, fullhtml, userStyleSheet);

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Bottom = 15,
                        Left = 15,
                        Right = 15,
                        Top = 10,
                    },
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { objectSettings }
            };

            //var htmlToPdfDocument = new HtmlToPdfDocument
            //{
            //    GlobalSettings = GetGlobalSettings(),
            //    Objects = { objectSettings }
            //};

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);

            var confiImageWatermark = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IMAGE_WATERMARK_RECORD);

            if (!string.IsNullOrEmpty(confiImageWatermark))
                _textSharpService.AddImageWatermarkToAllPages(ref fileContents, $"{GeneralHelpers.GetApplicationRoute(ConstantHelpers.Solution.Intranet, false)}/imagenes/{confiImageWatermark}");

            var fileDownloadName = HttpUtility.UrlEncode($"Constancia-Egreso.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileContents);
        }

        /// <summary>
        /// Genera el cuadro de mérito
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <param name="number">Número de solicitud</param>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <param name="receiptCode">Código de recibo</param>
        /// <param name="modelTpm">Objeto que contiene los datos temporales</param>
        /// <returns>Nombre de archivo y arreglo de bytes del archivo</returns>
        private async Task<Tuple<string, byte[]>> MeritChart(Guid id, string number, Guid recordId, string receiptCode, SaveRequestDataViewModel modelTpm = null)
        {
            var model = new MeritChartViewModel();
            var student = await _studentService.GetStudentWithCareerAcademicUser(id);
            var universityInformation = await GetUniversityInformation();

            if (modelTpm != null)
            {
                model.Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES"));
                model.Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation();
                model.StudentName = student.User.FullName.ToUpper();
                model.StudentCode = student.User.UserName;
                model.Faculty = student.Career.Faculty?.Name.ToUpper();
                model.Career = student.Career.Name.ToUpper();
                model.AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper();
                model.WeightedAverage = modelTpm.WeightedAverageCumulative;
                model.Details = modelTpm
                .Detail.Select(x => new MeritChartDetailViewModel
                {
                    Term = x.Term,
                    ApprovedCredits = x.ApprovedCredits,
                    Average = x.WeightedAverage,
                    MeritOrder = x.MeritOrder,
                    Observations = x.Observation,
                    TotalStudents = x.TotalStudents
                })
                .ToList();
                model.IsGraduated = student.Status == CORE.Helpers.ConstantHelpers.Student.States.GRADUATED;
                model.StudentGender = student.User.Sex;
                model.CurrentSemester = modelTpm.Semester;
                model.PageCode = number;
            }
            else
            {
                var result = await _academicSummariesService.GetDetailMeritChart(id);
                var details = _mapper.Map<List<MeritChartDetailViewModel>>(result);
                var average = await _academicSummariesService.GetStudentAverageAcumulative(id);

                model.Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES"));
                model.Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation();
                model.StudentName = student.User.FullName.ToUpper();
                model.StudentCode = student.User.UserName;
                model.Faculty = student.Career.Faculty?.Name.ToUpper();
                model.Career = student.Career.Name.ToUpper();
                model.AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper();
                model.WeightedAverage = Convert.ToDouble(average);
                model.Details = details;
                model.IsGraduated = student.Status == CORE.Helpers.ConstantHelpers.Student.States.GRADUATED;
                model.StudentGender = student.User.Sex;
                model.CurrentSemester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.TEXT[student.CurrentAcademicYear] : student.CurrentAcademicYear + "°";
                model.PageCode = number;
            }

            model.ImageQR = GetImageQR(recordId);
            model.University = universityInformation;


            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/MeritChart.cshtml", model);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/meritchart.css");
            string fullhtml = await GetFullHtmlContent(recordId, htmlContent);
            ObjectSettings objectSettings = GetObjectSettings(number, receiptCode, fullhtml, userStyleSheet);

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Bottom = 15,
                        Left = 15,
                        Right = 15,
                        Top = 10,
                    },
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Reporte-de-cuadro-meritos.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileContents);
        }

        /// <summary>
        /// Genera el reporte de quinto superior
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <param name="number">Número de solicitud</param>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <param name="receiptCode">Código de recibo</param>
        /// <param name="modelTpm">Objeto que contiene los datos temporales</param>
        /// <returns>Nombre de archivo y arreglo de bytes del archivo</returns>
        private async Task<Tuple<string, byte[]>> UpperFifth(Guid id, string number, Guid recordId, string receiptCode, SaveRequestDataViewModel modelTpm = null)
        {
            var student = await _studentService.GetStudentWithCareerAcademicUser(id);
            var current = await _academicSummariesService.GetCurrent(id, student.GraduationTermId);

            var universityInformation = await GetUniversityInformation();

            var model = new UpperFifthViewModel();

            if (modelTpm is null)
            {
                var result = await _academicSummariesService.GetDetailUpperFith(id);

                var details = _mapper.Map<List<UpperFifthDetailsViewModel>>(result);

                model = new UpperFifthViewModel
                {
                    Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                    Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                    AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper(),
                    Career = student.Career.Name,
                    Faculty = student.Career?.Faculty.Name.ToUpper(),
                    StudentCode = student.User.UserName,
                    StudentName = student.User.FullName,
                    WeightedAverage = current,
                    Details = details,
                    StudentSex = student.User.Sex,
                    IsGraduated = student.Status == CORE.Helpers.ConstantHelpers.Student.States.GRADUATED,
                    CurrentSemester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[student.CurrentAcademicYear] : student.CurrentAcademicYear + "",
                    PageCode = number,
                    ImageQR = GetImageQR(recordId)
                };

                var rangeOfterms = "";
                for (int i = 0; i < details.Count; i++)
                {
                    rangeOfterms += details[i].Term;
                    if (i != details.Count)
                        rangeOfterms += "-";
                }

                model.RangeOfTerms = rangeOfterms;
            }
            else
            {
                model = new UpperFifthViewModel
                {
                    Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                    Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                    AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper(),
                    Career = student.Career.Name,
                    StudentCode = student.User.UserName,
                    StudentName = student.User.FullName,
                    Faculty = student.Career?.Faculty.Name.ToUpper(),
                    WeightedAverage = current,
                    Details = modelTpm.Detail.Select(x => new UpperFifthDetailsViewModel
                    {
                        Term = x.Term,
                        ApprovedCredits = Convert.ToInt32(x.ApprovedCredits),
                        Average = x.WeightedAverage,
                        MeritOrder = x.MeritOrder,
                        Observations = x.Observation,
                        TotalStudentsInUpperFifth = x.UpperFifthTotalStudents,
                        TotalStudents = x.TotalStudents
                    })
                    .ToList(),
                    StudentSex = student.User.Sex,
                    IsGraduated = student.Status == CORE.Helpers.ConstantHelpers.Student.States.GRADUATED,
                    CurrentSemester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[student.CurrentAcademicYear] : student.CurrentAcademicYear + "",
                    PageCode = number,
                    ImageQR = GetImageQR(recordId)
                };
            }

            var documentFormat = await _documentFormatService.GetParsedDocumentFormat(ConstantHelpers.RECORDS.UPPERFIFTH, id);

            if (documentFormat != null)
            {
                model.CustomTitle = documentFormat.Title;
                model.CustomContent = documentFormat.Content;
            }

            model.University = universityInformation;

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/UpperFifth.cshtml", model);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/upperfifth.css");
            string fullhtml = await GetFullHtmlContent(recordId, htmlContent);
            ObjectSettings objectSettings = GetObjectSettings(number, receiptCode, fullhtml, userStyleSheet);

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Bottom = 15,
                        Left = 15,
                        Right = 15,
                        Top = 10,
                    },
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Reporte-de-quinto-superior.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileContents);
        }

        private async Task<Tuple<string, byte[]>> TenthHigher(Guid id, string number, Guid recordId, string receiptCode, SaveRequestDataViewModel modelTpm = null)
        {
            var student = await _studentService.GetStudentWithCareerAcademicUser(id);
            var current = await _academicSummariesService.GetCurrent(id, student.GraduationTermId);

            var universityInformation = await GetUniversityInformation();

            var model = new UpperFifthViewModel();

            if (modelTpm is null)
            {
                var result = await _academicSummariesService.GetDetailTenthFith(id);

                var details = _mapper.Map<List<UpperFifthDetailsViewModel>>(result);

                model = new UpperFifthViewModel
                {
                    Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                    Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                    AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper(),
                    Career = student.Career.Name,
                    Faculty = student.Career?.Faculty.Name.ToUpper(),
                    StudentCode = student.User.UserName,
                    StudentName = student.User.FullName,
                    WeightedAverage = current,
                    Details = details,
                    StudentSex = student.User.Sex,
                    IsGraduated = student.Status == CORE.Helpers.ConstantHelpers.Student.States.GRADUATED,
                    CurrentSemester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[student.CurrentAcademicYear] : student.CurrentAcademicYear + "",
                    PageCode = number,
                    ImageQR = GetImageQR(recordId)
                };

                var rangeOfterms = "";
                for (int i = 0; i < details.Count; i++)
                {
                    rangeOfterms += details[i].Term;
                    if (i != details.Count)
                        rangeOfterms += "-";
                }

                model.RangeOfTerms = rangeOfterms;
            }
            else
            {
                model = new UpperFifthViewModel
                {
                    Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                    Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                    AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper(),
                    Career = student.Career.Name,
                    StudentCode = student.User.UserName,
                    StudentName = student.User.FullName,
                    Faculty = student.Career?.Faculty.Name.ToUpper(),
                    WeightedAverage = current,
                    Details = modelTpm.Detail.Select(x => new UpperFifthDetailsViewModel
                    {
                        Term = x.Term,
                        ApprovedCredits = Convert.ToInt32(x.ApprovedCredits),
                        Average = x.WeightedAverage,
                        MeritOrder = x.MeritOrder,
                        Observations = x.Observation,
                        TotalStudentsInUpperFifth = x.UpperFifthTotalStudents,
                        TotalStudents = x.TotalStudents
                    })
                    .ToList(),
                    StudentSex = student.User.Sex,
                    IsGraduated = student.Status == CORE.Helpers.ConstantHelpers.Student.States.GRADUATED,
                    CurrentSemester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[student.CurrentAcademicYear] : student.CurrentAcademicYear + "",
                    PageCode = number,
                    ImageQR = GetImageQR(recordId)
                };
            }

            model.University = universityInformation;

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/TenthFith.cshtml", model);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/upperfifth.css");
            string fullhtml = await GetFullHtmlContent(recordId, htmlContent);
            ObjectSettings objectSettings = GetObjectSettings(number, receiptCode, fullhtml, userStyleSheet);

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Bottom = 15,
                        Left = 15,
                        Right = 15,
                        Top = 10,
                    },
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Reporte-de-quinto-superior.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileContents);
        }

        /// <summary>
        /// Genera el reporte de tercio superior
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <param name="number">Número de solicitud</param>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <param name="receiptCode">Código de recibo</param>
        /// <param name="modelTpm">Objeto que contiene los datos temporales</param>
        /// <returns>Nombre de archivo y arreglo de bytes del archivo</returns>
        private async Task<Tuple<string, byte[]>> UpperThird(Guid id, string number, Guid recordId, string receiptCode, SaveRequestDataViewModel modelTpm = null)
        {
            var student = await _studentService.GetStudentWithCareerAcademicUser(id);
            var current = await _academicSummariesService.GetCurrent(id, student.GraduationTermId);
            var universityInformation = await GetUniversityInformation();

            var model = new UpperFifthViewModel();

            if (modelTpm is null)
            {
                var result = await _academicSummariesService.GetDetailUpperThird(id);
                var details = _mapper.Map<List<UpperFifthDetailsViewModel>>(result);

                model = new UpperFifthViewModel
                {
                    Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                    Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                    AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper(),
                    Career = student.Career.Name,
                    StudentCode = student.User.UserName,
                    StudentName = student.User.FullName,
                    Faculty = student.Career.Faculty.Name.ToUpper(),
                    WeightedAverage = current,
                    Details = details,
                    StudentSex = student.User.Sex,
                    IsGraduated = student.Status == CORE.Helpers.ConstantHelpers.Student.States.GRADUATED,
                    CurrentSemester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[student.CurrentAcademicYear] : student.CurrentAcademicYear + "",
                    PageCode = number,
                    ImageQR = GetImageQR(recordId)
                };

                var rangeOfterms = string.Join("-", model.Details.Select(y => y.Term).ToList());
                model.RangeOfTerms = rangeOfterms;
            }
            else
            {
                model = new UpperFifthViewModel
                {
                    Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                    Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                    AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper(),
                    Career = student.Career.Name,
                    StudentCode = student.User.UserName,
                    StudentName = student.User.FullName,
                    WeightedAverage = current,
                    Faculty = student.Career.Faculty.Name.ToUpper(),
                    Details = modelTpm.Detail.Select(x => new UpperFifthDetailsViewModel
                    {
                        ApprovedCredits = x.ApprovedCredits,
                        Average = x.WeightedAverage,
                        MeritOrder = x.MeritOrder,
                        Observations = x.Observation,
                        Term = x.Term,
                        TotalStudents = x.TotalStudents,
                        TotalStudentsInUpperFifth = x.UpperFifthTotalStudents
                    }).ToList(),
                    StudentSex = student.User.Sex,
                    IsGraduated = student.Status == CORE.Helpers.ConstantHelpers.Student.States.GRADUATED,
                    CurrentSemester = modelTpm.Semester,
                    PageCode = number,
                    ImageQR = GetImageQR(recordId)
                };

                var rangeOfterms = string.Join("-", modelTpm.Detail.Select(y => y.Term).ToList());
                model.RangeOfTerms = rangeOfterms;
            }

            model.University = universityInformation;

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/UpperThird.cshtml", model);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/upperfifth.css");
            string fullhtml = await GetFullHtmlContent(recordId, htmlContent);
            ObjectSettings objectSettings = GetObjectSettings(number, receiptCode, fullhtml, userStyleSheet);

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Bottom = 15,
                        Left = 15,
                        Right = 15,
                        Top = 10,
                    },
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { objectSettings }
            };


            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Reporte-de-tercio-superior.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileContents);
        }

        /// <summary>
        /// Genera el registro académico
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <param name="number">Número de solicitud</param>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <param name="receiptCode">Código de recibo</param>
        /// <returns>Nombre de archivo y arreglo de bytes del archivo</returns>
        private async Task<Tuple<string, byte[]>> AcademicRecord(Guid id, string number, Guid recordId, string receiptCode)
        {
            var student = await _studentService.GetStudentWithCareerAdmissionAcademicUser(id);
            var average = await _academicSummariesService.GetAverageBachelorsDegree(id, student.GraduationTermId);

            var coursesDisapproved = await _academicHistoryService.GetCoursesDisapprovedByStudentId(id);
            var coursesRecovered = await _academicHistoryService.GetCoursesRecoveredByStudentId(id);

            var model = new AcademicRecordViewModel
            {
                Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                Student = student.User.FullName,
                EnrollmentCode = student.User.UserName,
                StartYear = $"{student.AdmissionTerm.Year}",
                Speciality = student.AcademicProgram?.Name,
                StartDateSemester = student.AdmissionTerm.Name,
                GraduatedDateSemester = student.GraduationTerm is null ? "-" : student.GraduationTerm.Name,
                RegularStudies = true ? "Si" : "No",
                DisapprovedCourses = coursesDisapproved,
                RecoveredCourses = coursesRecovered,
                Average = average,
                Observations = "-",
                PageCode = number,
                ImageQR = GetImageQR(recordId)
            };

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 10,
                    Left = 12,
                    Right = 15,
                    Top = 10,
                },
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/AcademicRecord.cshtml", model);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/academicRecord.css");
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlContent,
                PagesCount = true,
                WebSettings = { DefaultEncoding = "UTF-8", UserStyleSheet = userStyleSheet },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Record-academic-titulacion.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileContents);
        }

        private async Task<Tuple<string, byte[]>> CurriculumReview(Guid id)
        {
            var student = await _context.Students.Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.User.FullName,
                    x.User.Document,
                    x.User.UserName,
                    x.CurriculumId,
                    Curriculum = x.Curriculum.Code,
                    x.Curriculum.RequiredCredits,
                    x.Curriculum.ElectiveCredits,
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name
                })
                .FirstOrDefaultAsync();

            var model = new CurriculumReviewPDFViewModel
            {
                Header = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.DOCUMENT_HEADER_TEXT),
                SubHeader = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.DOCUMENT_SUBHEADER_TEXT),
                Curriculum = student.Curriculum,
                Career = student.Career,
                Faculty = student.Faculty,
                UserName = student.UserName,
                FullName = student.FullName,
                Document = student.Document,
                RequiredCredits = student.RequiredCredits,
                RequiredCreditsElective = student.ElectiveCredits,
                LogoImg = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                Courses = new List<CurriculumReviewCoursesViewModel>()
            };

            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == student.Id && x.Approved)
                .Select(x => new
                {
                    x.CourseId,
                    Term = x.Term.Name,
                    x.Validated,
                    x.Grade
                })
                .ToListAsync();

            var courses = await _context.AcademicYearCourses.Where(x => x.CurriculumId == student.CurriculumId)
                .Select(x => new CurriculumReviewCoursesViewModel
                {
                    CourseId = x.CourseId,
                    AcademicYear = x.AcademicYear,
                    Code = x.Course.Code,
                    Name = x.Course.Name,
                    IsElective = x.IsElective,
                    Credits = x.Course.Credits
                })
                .ToListAsync();

            model.Courses = courses
                .Select(x => new CurriculumReviewCoursesViewModel
                {
                    CourseId = x.CourseId,
                    AcademicYear = x.AcademicYear,
                    Code = x.Code,
                    Name = x.Name,
                    Credits = x.Credits,
                    Validated = academicHistories.Where(y => y.CourseId == x.CourseId).OrderByDescending(y => y.Grade).Select(y => y.Validated).FirstOrDefault(),
                    IsElective = x.IsElective,
                    Approved = academicHistories.Any(y => y.CourseId == x.CourseId),
                    Grade = academicHistories.Any(y => y.CourseId == x.CourseId) ? academicHistories.Where(y => y.CourseId == x.CourseId).OrderByDescending(y => y.Grade).Select(y => y.Grade).FirstOrDefault() : null,
                    Term = academicHistories.Where(y => y.CourseId == x.CourseId).OrderByDescending(y => y.Grade).Select(y => y.Term).FirstOrDefault(),
                })
                .OrderBy(x => x.AcademicYear)
                .ToList();

            var htmlContent = await _viewRenderService.RenderToStringAsync("/Areas/AcademicRecord/Views/Request/CurriculumReviewPdf.cshtml", model);
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Revision Curricular"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileByte = _dinkConverter.Convert(pdf);
            var fileDownloadName = HttpUtility.UrlEncode($"Revision-curricular.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileByte);
        }

        /// <summary>
        /// Genera el resumen de rendimiento académico
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <param name="number">Número de solicitud</param>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <param name="receiptCode">Código de recibo</param>
        /// <returns>Nombre de archivo y arreglo de bytes del archivo</returns>
        private async Task<Tuple<string, byte[]>> AcademicPerformanceSummary(Guid id, string number, Guid recordId, string receiptCode)
        {
            var student = await _studentService.GetStudentWithCareerAcademicUser(id);

            var result = await _academicSummariesService.GetAcademicPerformanceSummary(id);

            var universityInformation = await GetUniversityInformation();

            var details = result.Select(x => new AcademicPerformanceSummaryDetailViewModel
            {
                ApprovedCredits = x.ApprovedCredits,
                Average = x.Average,
                DisapprovedCredits = x.DisapprovedCredits,
                Term = x.Term,
                TotalCredits = x.TotalCredits
            }).ToList();

            var model = new AcademicPerformanceSummaryViewModel
            {
                Date = DateTime.UtcNow.ToString("dd 'de' MMMM, yyyy", new CultureInfo("es-ES")),
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
                StudentName = student.User.FullName.ToUpper(),
                StudentCode = student.User.UserName,
                Career = student.Career.Name.ToUpper(),
                Faculty = student.Career.Faculty?.Name.ToUpper(),
                AcademicProgram = student.AcademicProgram is null ? "PROGRAMA ACADÉMICO" : student.AcademicProgram.Name.ToUpper(),
                Details = details,
                StudentGender = student.User.Sex,
                CurrentSemester = (student.CurrentAcademicYear <= 20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_YEAR.TEXT[student.CurrentAcademicYear] : student.CurrentAcademicYear + "°",
                PageCode = number,
                ImageQR = GetImageQR(recordId),
                University = universityInformation
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync(@"/Areas/Academic/Views/StudentInformation/Pdf/AcademicPerformanceSummary.cshtml", model);

            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/meritchart.css");
            string fullhtml = await GetFullHtmlContent(recordId, htmlContent);
            ObjectSettings objectSettings = GetObjectSettings(number, receiptCode, fullhtml, userStyleSheet);

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Bottom = 15,
                        Left = 15,
                        Right = 15,
                        Top = 10,
                    },
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Resumen-rendimiento-academico.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileContents);

        }

        /// <summary>
        /// Genera el certificado de estudios
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <param name="number">Número de solicitud</param>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <param name="receiptCode">Código de recibo</param>
        /// <returns>Nombre de archivo y arreglo de bytes del archivo</returns>
        private async Task<Tuple<string, byte[]>> Certifcate(Guid id, string number, Guid recordId, string receiptCode)
        {
            var universityName = GeneralHelpers.GetInstitutionName();
            var model = await _studentService.GetStudntCertificate(id, universityName);
            var universityInfo = await GetUniversityInformation();
            var curriculum = await _context.Curriculums.Where(x => x.Id == model.CurriculumId).FirstOrDefaultAsync();

            var electiveApprovedCredits = await _studentService.GetElectiveApprovedCredits(model.IdStudent);
            var requiredApprovedCredits = await _studentService.GetRequiredApprovedCredits(model.IdStudent);

            var studentSummaries = await _academicSummariesService.GetAllByStudent(model.IdStudent);
            var student = new HeadBoardCertificateViewModel
            {
                IdStudent = model.IdStudent,
                FacultyName = model.FacultyName,
                CareerName = model.CareerName,
                FullName = model.FullName,
                UserName = model.UserName,
                AdmissionYear = model.AdmissionYear,
                GraduationYear = model.GraduationYear,
                CurriculumId = model.CurriculumId,
                Curriculum = curriculum?.Name,
                Dni = model.Dni,
                AcademicProgram = model.AcademicProgram,
                StudentSex = model.StudentSex,
                TotalApprovedElectiveCredits = electiveApprovedCredits,
                TotalApprovedMandatoryCredits = requiredApprovedCredits,
                TotalApprovedCredits = electiveApprovedCredits + requiredApprovedCredits,
                WeightedAverageCumulative = studentSummaries.OrderByDescending(x => x.Term.Year).ThenBy(x => x.Term.Number).Select(y => y.WeightedAverageGrade).FirstOrDefault()
            };

            var user = await _userService.GetUserByClaim(User);
            var modelLstCertificate = await _academicHistoryService.GetListCertificateByStudentAndCurriculum(student.IdStudent, student.CurriculumId);
            var lstCertificate = _mapper.Map<List<CertificateViewModel>>(modelLstCertificate);

            CertificateCompleteViewModel certificateComplete = new CertificateCompleteViewModel
            {
                HeaderBoard = student,
                Certificate = lstCertificate.OrderBy(x => x.AcademicYear).ToList(),
                ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{@GeneralHelpers.GetTheme()}/logo-report.png"),
                Today = DateTime.UtcNow.ToShortDateString(),
                YearOfStudies = Math.Truncate((decimal)lstCertificate.GroupBy(x => new { x.TermName }).Count() / 2),
                ImgQR = GetImageQR(recordId),
                User = user.FullName,
                University = universityInfo
            };
            var htmlContent = "";

            if (ConstantHelpers.Institution.UNJBG == ConstantHelpers.GENERAL.Institution.Value)
                htmlContent = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/Certificate/CertificatePDF_UNJBG.cshtml", certificateComplete);
            else
                htmlContent = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/Certificate/CertificatePDF.cshtml", certificateComplete);

            var objectSettings = GetObjectSettings(number, receiptCode, htmlContent);

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Bottom = 15,
                        Left = 15,
                        Right = 15,
                        Top = 10,
                    },
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);

            var fileDownloadName = HttpUtility.UrlEncode($"Certificado estudios.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileContents);
        }

        /// <summary>
        /// Genera el certificado de estudios parcial
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <param name="number">Número de solicitud</param>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <param name="startAcademicYear">Ciclo de inicio</param>
        /// <param name="endAcademicYear">Ciclo fin</param>
        /// <param name="startTerm">Periodo de inicio</param>
        /// <param name="endTerm">Periodo fin</param>
        /// <returns>Nombre de archivo y arreglo de bytes del archivo</returns>
        private async Task<Tuple<string, byte[]>> CertifcatePartial(Guid id, string number, Guid recordId, int? startAcademicYear, int? endAcademicYear, Guid? startTerm, Guid? endTerm)
        {
            var universityName = GeneralHelpers.GetInstitutionName();
            var model = await _studentService.GetStudntCertificate(id, universityName);
            var universityInfo = await GetUniversityInformation();
            var student = new HeadBoardCertificateViewModel
            {
                IdStudent = model.IdStudent,
                FacultyName = model.FacultyName,
                CareerName = model.CareerName,
                FullName = model.FullName,
                UserName = model.UserName,
                AdmissionYear = model.AdmissionYear,
                GraduationYear = model.GraduationYear,
                CurriculumId = model.CurriculumId,
                Dni = model.Dni,
                AcademicProgram = model.AcademicProgram,
                StudentSex = model.StudentSex
            };

            var user = await _userService.GetUserByClaim(User);
            var modelLstCertificate = await _academicHistoryService.GetListCertificateByStudentAndCurriculum(student.IdStudent, student.CurriculumId);
            if (startAcademicYear.HasValue && endAcademicYear.HasValue)
            {
                modelLstCertificate = modelLstCertificate.Where(x => startAcademicYear <= x.AcademicYear && x.AcademicYear <= endAcademicYear).ToList();
            }
            else
            {
                var data = await _context.StudentSections.Where(x => x.StudentId == student.IdStudent)
                    .Select(x => new
                    {
                        id = x.Section.CourseTerm.TermId,
                        text = x.Section.CourseTerm.Term.Name
                    })
                    .Distinct()
                    .ToListAsync();

                data = data.OrderBy(x => x.text).ToList();

                var startIndexOf = data.IndexOf(data.Where(x => x.id == startTerm).FirstOrDefault());
                var endIndexOf = data.IndexOf(data.Where(x => x.id == endTerm).FirstOrDefault());

                var termIds = new List<Guid>();

                for (int i = startIndexOf; i <= endIndexOf; i++)
                {
                    termIds.Add(data[i].id);
                }

                modelLstCertificate = modelLstCertificate.Where(x => termIds.Contains(x.TermId)).ToList();
            }

            var lstCertificate = _mapper.Map<List<CertificateViewModel>>(modelLstCertificate);

            CertificateCompleteViewModel certificateComplete = new CertificateCompleteViewModel
            {
                HeaderBoard = student,
                Certificate = lstCertificate,
                ImagePathLogo = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{@GeneralHelpers.GetTheme()}/logo-report.png"),
                Today = DateTime.UtcNow.ToShortDateString(),
                YearOfStudies = Math.Truncate((decimal)lstCertificate.GroupBy(x => new { x.TermName }).Count() / 2),
                ImgQR = GetImageQR(recordId),
                User = user.FullName,
                University = universityInfo
            };
            var htmlContent = "";

            if (ConstantHelpers.Institution.UNJBG == ConstantHelpers.GENERAL.Institution.Value)
                htmlContent = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/Certificate/CertificatePDF_UNJBG.cshtml", certificateComplete);
            else
                htmlContent = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/Certificate/CertificatePDF.cshtml", certificateComplete);


            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "UTF-8", UserStyleSheet = "" },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Bottom = 15,
                        Left = 15,
                        Right = 15,
                        Top = 10,
                    },
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { objectSettings }
            };

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);
            var fileDownloadName = HttpUtility.UrlEncode($"Certificado estudios.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileContents);
        }


        private async Task<Tuple<string, byte[]>> NoDebtCertificate(Guid studentId, Guid? recordId, Guid? userProcedureId)
        {
            var student = await _studentService.GetStudentWithCareerAcademicUser(studentId);
            var user = await _userService.Get(student.UserId);

            var model = new ProofNoDebtViewModel
            {
                UserName = user.UserName,
                FullName = user.FullName,
                StudentSex = user.Sex,
                Image = Path.Combine(_hostingEnvironment.WebRootPath, $"images/themes/{GeneralHelpers.GetTheme()}/logo-report.png"),
                SubHeader = "OFICINA DE TESORERIA",
                Date = DateTime.UtcNow.ToDefaultTimeZone().ToString("dddd, dd MMMM yyyy", new CultureInfo("es-PE")),
                University = await GetUniversityInformation(),
                Career = student.Career.Name,
                Faculty = student.Career.Faculty.Name,
                Location = CORE.Helpers.GeneralHelpers.GetInstitutionLocation(),
            };

            if (recordId.HasValue)
            {
                var record = await _recordHistoryService.Get(recordId.Value);
                model.Correlative = record.Code;
            }

            if (userProcedureId.HasValue)
            {
                var userProcedure = await _context.UserProcedures.Where(x => x.Id == userProcedureId).FirstOrDefaultAsync();
                model.Correlative = userProcedure.Correlative;
            }

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/AcademicRecord/Views/Request/ProofNoDebtPdf.cshtml", model);
            var userStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/pdf/proofnodebt.css");

            string fullhtml = String.Empty;

            if (recordId.HasValue)
            {
                fullhtml = await GetFullHtmlContent(recordId.Value, viewToString);
            }
            else
            {
                fullhtml = viewToString;
            }

            ObjectSettings objectSettings = GetObjectSettings(model.Correlative, "", fullhtml, userStyleSheet);

            var htmlToPdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Bottom = 15,
                        Left = 15,
                        Right = 15,
                        Top = 10,
                    },
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { objectSettings }
            };

            //var htmlToPdfDocument = new HtmlToPdfDocument
            //{
            //    GlobalSettings = GetGlobalSettings(),
            //    Objects = { objectSettings }
            //};

            var fileContents = _dinkConverter.Convert(htmlToPdfDocument);

            var confiImageWatermark = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IMAGE_WATERMARK_RECORD);

            if (!string.IsNullOrEmpty(confiImageWatermark))
                _textSharpService.AddImageWatermarkToAllPages(ref fileContents, $"{GeneralHelpers.GetApplicationRoute(ConstantHelpers.Solution.Intranet, false)}/imagenes/{confiImageWatermark}");

            var fileDownloadName = HttpUtility.UrlEncode($"Constancia-no-adeudo.pdf");
            return new Tuple<string, byte[]>(fileDownloadName, fileContents);
        }

        /// <summary>
        /// Método para retornar el archivo
        /// </summary>
        /// <param name="fileByte">arreglo de bytes</param>
        /// <param name="name">Nombre del archivo</param>
        /// <returns>Archivo</returns>
        private IActionResult ReturnFile(byte[] fileByte, string name)
        {
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF, name);
        }

        //CODIGO QRSS

        /// <summary>
        /// Método para generar QR
        /// </summary>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <returns>Arreglo de bytes</returns>
        private byte[] GenerarQv2(Guid recordId)
        {
            QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            var URLAbsolute = Url.GenerateLink(nameof(DocumentVerifierController.ConstancyVerifier), "DocumentVerifier", Request.Scheme, new { id = recordId });
            QRCoder.QRCodeData qrCodeData = qrGenerator.CreateQrCode(URLAbsolute, QRCoder.QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCoder.PngByteQRCode(qrCodeData);
            //var stream = new MemoryStream();
            //qrCode.GetGraphic(5).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //return stream.ToArray();
            return qrCode.GetGraphic(5);
        }

        /// <summary>
        /// Método para generar QR
        /// </summary>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <returns>Qr en base 64</returns>
        private string GetImageQR(Guid recordId)
        {
            var bitMap = GenerarQv2(recordId);
            var finalQR = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(bitMap));
            return finalQR;
        }

        //GLBOAL SETTINGS
        /// <summary>
        /// Opciones generales de los certificados
        /// </summary>
        /// <returns>Objeto que contiene los datos de las opciones generales del PDF</returns>
        private static GlobalSettings GetGlobalSettings()
        {
            return new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings
                {
                    Bottom = 0,
                    Left = 15,
                    Right = 15,
                    Top = 10,
                },
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number">Número de solicitud</param>
        /// <param name="receiptCode">Código de recibo</param>
        /// <param name="fullhtml">Contenido del PDF</param>
        /// <param name="userStyleSheet">Estilos del pdf</param>
        /// <returns>Objeto que contiene la configuración del PDF</returns>
        private ObjectSettings GetObjectSettings(string number, string receiptCode, string fullhtml, string userStyleSheet = null)
        {
            return new ObjectSettings
            {
                HtmlContent = fullhtml,
                WebSettings = { DefaultEncoding = "UTF-8", UserStyleSheet = userStyleSheet },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Right = "Nro de Recibo: "+receiptCode,
                    Center= "REG. TRADOC: "+ number,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                 }
            };
        }

        /// <summary>
        /// Obtiene el pie de pagina del archivo PDF
        /// </summary>
        /// <param name="recordId">Identificador de la solicitud</param>
        /// <param name="htmlContent">Contenido HTML</param>
        /// <returns>Html del PDF</returns>
        private async Task<string> GetFullHtmlContent(Guid recordId, string htmlContent)
        {
            var generatedBy = await _userService.GetByUserName(HttpContext.User.Identity.Name);
            var signatureImageUrl = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IMAGE_CERTIFICATE_SIGNATURE);
            var academicRecordSigned = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.ACADEMIC_RECORD_SIGNING);

            var signatureBase64 = string.Empty;

            if (!string.IsNullOrEmpty(signatureImageUrl))
            {
                using (var mem = new MemoryStream())
                {
                    await _cloudStorageService.TryDownload(mem, ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION, signatureImageUrl);
                    signatureBase64 = $"data:image/png;base64, {Convert.ToBase64String(mem.ToArray())}";
                }
            }

            List<string> data = new List<string>()
            {
                generatedBy.FullName,
                GetImageQR(recordId),
                academicRecordSigned,
                signatureBase64
            };

            var htmlContentfooter = await _viewRenderService.RenderToStringAsync(@"/Views/Shared/Pdf/FooterPdf.cshtml", data);
            var head = "<div style='position: relative;min-height:1250px'>";
            var foot = "<div style='position: absolute; bottom: 0; left: 0;'>" + htmlContentfooter + "</div></div>";
            return head + htmlContent + foot;
        }

        #endregion

        /// <summary>
        /// Vista donde se genera el documento
        /// </summary>
        /// <returns>Vista</returns>
        //Generar Documento
        [HttpGet("generar-documento")]
        public async Task<IActionResult> GenerateDocument()
        {
            var isIntegrated = false;
            var configuration = await _configurationService.GetByKey(ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM);
            if (configuration != null)
            {
                isIntegrated = Convert.ToBoolean(configuration.Value);
            }

            ViewBag.IsIntegrated = isIntegrated;
            return View();
        }

        /// <summary>
        /// Obtiene el historial de solicitudes generadas por el estudiante
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <param name="type">Tipo</param>
        /// <returns>Listado de solicitudes</returns>
        [HttpGet("get-historial")]
        public async Task<IActionResult> GetRecordHistory(Guid studentId, int type)
        {
            var needData = new List<int>()
            {
                ConstantHelpers.RECORDS.MERITCHART,
                ConstantHelpers.RECORDS.UPPERFIFTH,
                ConstantHelpers.RECORDS.UPPERTHIRD
            };

            var data = await _context.RecordHistories
                .Include(x => x.Student).ThenInclude(x => x.User)
                .Where(x => x.StudentId == studentId && x.Type == type)
                .OrderByDescending(x => x.Date)
                .Select(x => new
                {
                    id = x.Id,
                    date = x.Date.ToLocalDateTimeFormat(),
                    number = $"{x.Number.ToString().PadLeft(5, '0')}-{x.Date.Year}",
                    userProcedure = "Sin Asignar",
                    isTypeBachelor = x.Type == ConstantHelpers.RECORDS.BACHELOR,
                    isTypeJobTitle = x.Type == ConstantHelpers.RECORDS.JOBTITLE,
                    type = ConstantHelpers.RECORDS.VALUES.ContainsKey(x.Type) ? ConstantHelpers.RECORDS.VALUES[x.Type] : "-",
                    status = ConstantHelpers.RECORD_HISTORY_STATUS.VALUES.ContainsKey(x.Status) ? ConstantHelpers.RECORD_HISTORY_STATUS.VALUES[x.Status] : "-",
                    //userProcedureStatus = ConstantHelpers.USER_PROCEDURES.STATUS.VALUES.ContainsKey(x.UserProcedure.Status) ? ConstantHelpers.USER_PROCEDURES.STATUS.VALUES[x.UserProcedure.Status] : "-",
                    needData = needData.Contains(x.Type),
                    needToSaveFile = string.IsNullOrEmpty(x.FileURL),
                    urlFile = x.FileURL,
                    withProcedure = x.Status == ConstantHelpers.RECORD_HISTORY_STATUS.WITH_PROCEDURE,
                    username = x.Student.User.UserName
                })
                .ToListAsync();

            var result = new
            {
                data = data
            };

            return Ok(result);
        }

        /// <summary>
        /// Método para genera la solicitud
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la nueva solicitud</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("generar")]
        public async Task<IActionResult> CreateDocument(CreateDocumentViewModel model)
        {
            var registerUser = await _userService.GetUserByClaim(User);

            var recordHistory = new RecordHistory
            {
                Type = model.RecordType,
                Date = DateTime.UtcNow,
                Number = await _recordHistoryService.GetLatestRecordNumberByType(model.RecordType, DateTime.UtcNow.Year) + 1,
                StudentId = model.StudentId,
                Status = model.UserProcedureId.HasValue ? ConstantHelpers.RECORD_HISTORY_STATUS.WITH_PROCEDURE : ConstantHelpers.RECORD_HISTORY_STATUS.GENERATED,
                DerivedUserId = registerUser.Id,
                RecordTermId = model.RecordType == ConstantHelpers.RECORDS.REGULARSTUDIES ? model.TermId : null,
                //UserProcedureId = model.UserProcedureId
            };

            if (model.RecordType == ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIESPARTIAL)
            {
                recordHistory.JsonAcademicYears = model.JsonAcademicYearPartials;
            }

            if (
                model.RecordType == ConstantHelpers.RECORDS.STUDYRECORD ||
                model.RecordType == ConstantHelpers.RECORDS.UPPERTHIRD ||
                model.RecordType == ConstantHelpers.RECORDS.UPPERFIFTH ||
                model.RecordType == ConstantHelpers.RECORDS.TENTH_HIGHER ||
                model.RecordType == ConstantHelpers.RECORDS.UPPER_MIDDLE ||
                model.RecordType == ConstantHelpers.RECORDS.ENROLLMENT_REPORT
                )
            {

                if (!model.TermId.HasValue)
                    return BadRequest("Es necesario seleccionar el periodo.");

                recordHistory.RecordTermId = model.TermId;
            }

            if (model.RecordType == ConstantHelpers.RECORDS.ENROLLMENT)
            {

                if (!model.TermId.HasValue)
                    return BadRequest("Es necesario seleccionar el periodo.");

                recordHistory.RecordTermId = model.TermId;
            }


            await _recordHistoryService.Insert(recordHistory);
            return Ok();
        }

        /// <summary>
        /// Obtiene los detalles de la solicitud
        /// </summary>
        /// <param name="recordHistoryId">Identificador de la solicitud</param>
        /// <returns>Detalles de la solicitud</returns>
        [HttpGet("get-record")]
        public async Task<IActionResult> GetRecordHistory(Guid recordHistoryId)
        {
            var entity = await _recordHistoryService.Get(recordHistoryId);
            return Ok(entity);
        }

        /// <summary>
        /// Método para actualizar el estado de la solicitud
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados de la solicitud</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("actualizar-estado")]
        public async Task<IActionResult> UpdateStatus(UpdateRequestViewModel model)
        {
            var entity = await _recordHistoryService.Get(model.RecordHistoryId);
            entity.Status = model.Status;
            await _recordHistoryService.Update(entity);

            var history = new ENTITIES.Models.Generals.RecordHistoryObservation
            {
                RecordHistoryId = model.RecordHistoryId,
                Observation = model.Observation,
                RecordHistoryStatus = model.Status
            };

            await _recordHistoryObservationService.Insert(history);
            return Ok();
        }

        /// <summary>
        /// Obtiene el listado trámites asociados al estudiante para ser usado en select
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <returns>Listado de trámites</returns>
        [HttpGet("tramites-asociados")]
        public async Task<IActionResult> GetUserProcedures(Guid studentId)
        {
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();
            var userProcedures = await _context.UserProcedures.Where(x => x.User.Id == student.UserId && x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.CreatedAt.ToLocalDateTimeFormat()} {x.Procedure.Name}"
                })
                .ToListAsync();

            return Ok(userProcedures);
        }

        /// <summary>
        /// Obtiene el listado de observaciones de una solicitud
        /// </summary>
        /// <param name="recordHistoryId">Identificador de la solicitud</param>
        /// <returns>Listado de observaciones</returns>
        [HttpGet("get-observaciones")]
        public async Task<IActionResult> GetObservations(Guid recordHistoryId)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _recordHistoryObservationService.GetObservationsDatatableByRecordHistoryId(parameters, recordHistoryId);
            return Ok(result);
        }

        #region UniversityInfo

        /// <summary>
        /// Obtiene el valor de la configuración
        /// </summary>
        /// <param name="list">Listado de configuraciones</param>
        /// <param name="key">Identificador de la configuración</param>
        /// <returns>Valor de la configuración</returns>
        private string GetConfigurationValue(Dictionary<string, string> list, string key)
        {
            return list.ContainsKey(key) ? list[key] :

                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES.ContainsKey(key) ?
                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[key] : "";
        }

        /// <summary>
        /// Obtiene los datos de la universidad para la impresión del PDF
        /// </summary>
        /// <returns>Objeto que contiene los datos de la universidad</returns>
        private async Task<UniversityInformation> GetUniversityInformation()
        {
            var values = await _configurationService.GetDataDictionary();
            int currentYear = DateTime.UtcNow.AddHours(-5).Year;
            var yearInformation = await _yearInformationService.GetNameByYear(currentYear);

            var universityinfo = new UniversityInformation
            {
                Address = GetConfigurationValue(values, ConstantHelpers.Configuration.General.INSTITUTION_ADDRESS),
                PhoneNumber = GetConfigurationValue(values, ConstantHelpers.Configuration.General.INSTITUTION_PHONENUMBER),
                Campus = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_MAIN_CAMPUS),
                HeaderOffice = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_MAIN_OFFICE),
                Office = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_OFFICE),
                Sender = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_SENDER),
                WebSite = GetConfigurationValue(values, ConstantHelpers.Configuration.General.INSTITUTION_WEBSITE),
                YearInformation = yearInformation
            };

            return universityinfo;
        }
        #endregion
    }
}
