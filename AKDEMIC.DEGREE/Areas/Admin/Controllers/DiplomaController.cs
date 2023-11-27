using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.DEGREE.Areas.Admin.Models.RegistryPatternViewModels;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.DEGREE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DinkToPdf.Contracts;
using iTextSharp.text.pdf.qrcode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/diplomas")]
    public class DiplomaController : BaseController
    {
        private readonly IConverter _dinkConverter;
        private readonly IViewRenderService _viewRenderService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly AkdemicContext _context;
        private readonly IRegistryPatternService _registryPatternService;
        private readonly ICareerAccreditationRepository _careerAccreditationRepository;
        private readonly IDataTablesService _dataTablesService;
        private readonly IPaymentService _paymentService;
        private readonly IUserProcedureService _userProcedureService;
        public DiplomaController(
            IConfigurationService configurationService,
            IDataTablesService dataTablesService,
            IWebHostEnvironment hostingEnvironment,
            AkdemicContext context,
            IRegistryPatternService registryPatternService,
            ICareerAccreditationRepository careerAccreditationRepository,
            ICareerService careerService,
            IUserProcedureService userProcedureService,
            IPaymentService paymentService,
            IViewRenderService viewRenderService,
            IConverter dinkConverter
        ) : base(careerService, configurationService)
        {
            _dataTablesService = dataTablesService;

            _registryPatternService = registryPatternService;
            _careerAccreditationRepository = careerAccreditationRepository;
            _paymentService = paymentService;
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _userProcedureService = userProcedureService;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        /// <summary>
        /// Vista principal donde se encuentra el listado de diplomas
        /// </summary>
        /// <returns>Retorna la vista</returns>
        public async Task<IActionResult> Index()
        {
            var configurationSystemIntegrated = Boolean.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM));
            return View(configurationSystemIntegrated);
            //return View("/Areas/Admin/Views/Diploma/unica/DiplomaPdfView.cshtml");    
        }

        /// <summary>
        /// Obtiene el listado de carreras
        /// </summary>
        /// <returns>Retorna un Ok</returns>
        [HttpGet("carreras/get")]
        public async Task<IActionResult> GetCareers()
        {
            var result = await _careerService.GetCareersJson(null);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de padrones generados en base a los siguientes parámetros
        /// </summary>
        /// <param name="searchValue">Texto búsqueda</param>
        /// <param name="searchDiplomaNumber">Texto búsqueda número de diploma</param>
        /// <param name="seachBookNumber">Texto búsqueda número de libro</param>
        /// <param name="careerId">Identificador de la carrera</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="academicProgramId">Identificador del programa académico</param>
        /// <param name="officeNumber">Texto búsqueda número de oficio</param>
        /// <returns>Retorna un Ok</returns>
        [HttpGet("lista-registro-padrones")]
        public async Task<IActionResult> GetUserProcedures(string searchValue, string searchDiplomaNumber, string seachBookNumber, Guid? careerId, Guid? facultyId, Guid? academicProgramId, string officeNumber)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _registryPatternService.GetRegistryPatternDatatableByConfiguration(sentParameters, facultyId, careerId, academicProgramId, seachBookNumber, null, null, searchValue, null, null, ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED, null, null, searchDiplomaNumber, officeNumber);
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se encuentran los detalles del diploma
        /// </summary>
        /// <param name="registryPatternId">Identificador de registro de padrón</param>
        /// <returns>Retorna la vista</returns>
        [HttpGet("{registryPatternId}/diploma")]
        public async Task<IActionResult> CreateDiploma(Guid registryPatternId)
        {
            var template = await _registryPatternService.GetRegistryPatternTemplate(registryPatternId);

            var result = new RegistryPatternViewModel()
            {

                Id = template.Id,
                UniversityCode = template.UniversityCode,
                BussinesSocialReason = template.BussinesSocialReason,
                RegistrationDate = template.RegistrationDate,
                RegistrationEnd = template.RegistrationEnd,
                FacultyName = template.FacultyName,
                CareerName = template.CareerName,
                PostGraduateSchool = template.PostGraduateSchool,
                PaternalSurname = template.PaternalSurname,
                MaternalSurname = template.MaternalSurname,
                StudentName = template.StudentName,
                Sex = template.Sex,
                DocumentType = template.DocumentType,
                DNI = template.DNI,
                BachelorOrigin = template.BachelorOrigin,
                AcademicDegree = template.AcademicDegree,
                AcademicDegreeDenomination = template.AcademicDegreeDenomination,
                Specialty = template.Specialty,
                ResearchWork = template.ResearchWork,
                Credits = template.Credits,
                ResearchWorkURL = template.ResearchWorkURL,
                DegreeProgram = template.DegreeProgram,
                PedagogicalTitleOrigin = template.PedagogicalTitleOrigin,
                ObtainingDegreeModality = template.ObtainingDegreeModality,
                StudyModality = template.StudyModality,
                GradeAbbreviation = template.GradeAbbreviation,
                OriginDegreeCountry = template.OriginDegreeCountry,
                OriginDegreeUniversity = template.OriginDegreeUniversity,
                GradDenomination = template.GradDenomination,
                ResolutionNumber = template.ResolutionNumber,
                ResolutionDateByUniversityCouncil = template.ResolutionDateByUniversityCouncil,
                OriginDiplomatDate = template.OriginDiplomatDate,
                DuplicateDiplomatDate = template.DuplicateDiplomatDate,
                DiplomatNumber = template.DiplomatNumber,
                EmissionTypeOfDiplomat = template.EmissionTypeOfDiplomat,
                BookCode = template.BookCode,
                FolioCode = template.FolioCode,
                RegistryNumber = template.RegistryNumber,
                ManagingDirector = template.ManagingDirector,
                ManagingDirectorFullName = template.ManagingDirectorFullName,
                GeneralSecretary = template.GeneralSecretary,
                GeneralSecretaryFullName = template.GeneralSecretaryFullName,
                AcademicResponsible = template.AcademicResponsible,
                AcademicResponsibleFullName = template.AcademicResponsibleFullName,
                OriginPreRequisiteDegreeCountry = template.OriginPreRequisiteDegreeCountry,
                ForeignUniversityOriginId = template.ForeignUniversityOriginId,
                OriginPreRequisiteDegreeDenomination = template.OriginPreRequisiteDegreeDenomination,
                OfficeNumber = template.OfficeNumber,
                DateEnrollmentProgram = template.DateEnrollmentProgram,
                StartDateEnrollmentProgram = template.StartDateEnrollmentProgram,
                EndDateEnrollmentProgram = template.EndDateEnrollmentProgram,
                AcademicProgram = template.AcademicProgram,
                Relation = template.Relation,
                SpecialtyMention = template.SpecialtyMention,
                UniversityCouncilType = template.UniversityCouncilType,
                FacultyCouncilDate = template.FacultyCouncilDate,
                UniversityCouncilDate = template.UniversityCouncilDate,
                Initial = template.Initial,
                Correlative = template.Correlative,
                Year = template.Year

            };
            return View(result);

        }

        /// <summary>
        /// Método para obteenr el diploma en formato PDF
        /// </summary>
        /// <param name="id">Identificador del registro de padrón</param>
        /// <returns>Retorna un archivo en formato PDF</returns>
        [HttpGet("descargar/Pdf/{id}")]
        public async Task<IActionResult> DownloadPDF(Guid id)
        {
            try
            {
                var viewModel = await _registryPatternService.GetPdfReport(id);

                var cssPath = "";

                if (!viewModel.OriginDiplomatDateBoolean)
                {
                    return BadRequest("Verificar fecha del diploma");
                }

                if (String.IsNullOrEmpty(viewModel.AcademicDegreeDenomination))
                {
                    return BadRequest("No se ha consigado la denominación de grado.");
                }


                var abreviationTheme = ConstantHelpers.Institution.Values[ConstantHelpers.GENERAL.Institution.Value];

                string viewToString = "";

                var URLAbsolute = Url.GenerateLink(nameof(DiplomaDetailController.Index), "DiplomaDetail", Request.Scheme, new { id });

                var execute = GenerarQv2(URLAbsolute);

                var finalQR = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(execute));

                viewModel.QRCode = finalQR;

                if (abreviationTheme == ConstantHelpers.Institution.Values[ConstantHelpers.Institution.UNJBG]
                    || abreviationTheme == ConstantHelpers.Institution.Values[ConstantHelpers.Institution.UNF]
                    || abreviationTheme == ConstantHelpers.Institution.Values[ConstantHelpers.Institution.UNFV]
                    || abreviationTheme == ConstantHelpers.Institution.Values[ConstantHelpers.Institution.UNTRM]
                    || abreviationTheme == ConstantHelpers.Institution.Values[ConstantHelpers.Institution.UNAH]
                    || abreviationTheme == ConstantHelpers.Institution.Values[ConstantHelpers.Institution.UNSM]
                    || abreviationTheme == ConstantHelpers.Institution.Values[ConstantHelpers.Institution.UNSCH]
                    || abreviationTheme == ConstantHelpers.Institution.Values[ConstantHelpers.Institution.UNICA])
                {
                    viewModel.Shield = Path.Combine(_hostingEnvironment.WebRootPath, @$"images/themes/{abreviationTheme}/logo-sm.png");
                    viewToString = await _viewRenderService.RenderToStringAsync($"/Areas/Admin/Views/Diploma/{abreviationTheme}/DiplomaPdfView.cshtml", viewModel);
                }
                else
                {
                    viewToString = await _viewRenderService.RenderToStringAsync($"/Areas/Admin/Views/Diploma/akdemic/DiplomaPdfView.cshtml", viewModel);
                }


                if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNF
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNFV
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNTRM
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAH
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSCH
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNICA)
                {
                    cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @$"css/areas/admin/{abreviationTheme}/diploma.css");
                }
                else
                {
                    cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css/areas/admin/diplomaReport.css");
                }

                var objectSettings = new DinkToPdf.ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = viewToString,
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPath },

                };

                var globalSettings = new DinkToPdf.GlobalSettings();


                if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNF
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNFV
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNTRM
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNTRM
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAH
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSCH
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNICA)
                {

                    globalSettings.ColorMode = DinkToPdf.ColorMode.Color;
                    globalSettings.Orientation = DinkToPdf.Orientation.Landscape;
                    globalSettings.PaperSize = DinkToPdf.PaperKind.A4;
                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG)
                    {
                        globalSettings.Margins = new DinkToPdf.MarginSettings { Top = 60, Bottom = 10, Left = 30, Right = 30 };
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNF)
                    {
                        globalSettings.DPI = 398;
                        globalSettings.Margins = new DinkToPdf.MarginSettings { Top = 34, Bottom = 14, Left = 30, Right = 30 };
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNFV)
                    {
                        globalSettings.Margins = new DinkToPdf.MarginSettings { Top = 0.84, Bottom = 0.84, Left = 0.8, Right = 0.8, Unit = DinkToPdf.Unit.Inches};
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNICA)
                    {
                        globalSettings.PaperSize = DinkToPdf.PaperKind.Legal;
                        globalSettings.Margins = new DinkToPdf.MarginSettings { Top = 0.5, Bottom = 0.5, Left = 3, Right = 3, Unit = DinkToPdf.Unit.Centimeters };
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNTRM)
                    {
                        globalSettings.DPI = 398;
                        globalSettings.Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 23, Left = 32, Right = 32, Unit = DinkToPdf.Unit.Millimeters };
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM)
                    {
                        globalSettings.Margins = new DinkToPdf.MarginSettings { Top = 3, Bottom = 3, Left = 5, Right = 5 };
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSCH)
                    {
                        globalSettings.Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 5, Left =20, Right = 20 };
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAH)
                    {
                        globalSettings.Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 };
                    }
                }
                else
                {
                    globalSettings.ColorMode = DinkToPdf.ColorMode.Color;
                    globalSettings.Orientation = DinkToPdf.Orientation.Portrait;
                    globalSettings.PaperSize = DinkToPdf.PaperKind.A4;

                    globalSettings.Margins = new DinkToPdf.MarginSettings { Top = 40, Bottom = 30, Left = 10, Right = 10 };
                }

                var pdf = new DinkToPdf.HtmlToPdfDocument
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                var fileByte = _dinkConverter.Convert(pdf);

                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

                return File(fileByte, "application/pdf", "diploma.pdf");
            }
            catch (Exception e)
            {
                return BadRequest("No se pudo descargar el archivo");
            }
        }

        /// <summary>
        /// Guarda los diplomas en formato PDF en un archivo ZIP en base a los siguientes parámetros
        /// </summary>
        /// <param name="searchBookNumber">Texto búsqueda número de libro</param>
        /// <returns>Retorna un archivo en formato ZIP</returns>
        [HttpGet("diploma-zip")]
        public async Task<IActionResult> DonwloadZip(string searchBookNumber)
        {
            try
            {

                var registryPatternList = await _registryPatternService.GetRegistryPatternsListByBookNumber(searchBookNumber);
                if (registryPatternList.Any(x => x.OriginDiplomatDate.HasValue == false))
                {
                    return BadRequest("Verificar que todos los diplomas tengan una fecha correspondiente");
                }
                byte[] fileBytesOutPut = null;
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    // create a zip
                    using (System.IO.Compression.ZipArchive zip = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
                    {
                        // interate through the source files
                        foreach (var rp in registryPatternList)
                        {
                            var viewModel = await _registryPatternService.GetPdfReport(rp.Id);
                            var abreviationTheme = ConstantHelpers.Institution.Values[ConstantHelpers.GENERAL.Institution.Value];
                            string viewToString = "";

                            //QRCodeGenerator qrGenerator = new QRCodeGenerator();

                            var URLAbsolute = Url.GenerateLink(nameof(DiplomaDetailController.Index), "DiplomaDetail", Request.Scheme, new { rp.Id });

                            //QRCodeData qrCodeData = qrGenerator.CreateQrCode(URLAbsolute,
                            //QRCodeGenerator.ECCLevel.Q);
                            //QRCode qrCode = new QRCode(qrCodeData);
                            //Bitmap qrCodeImage = qrCode.GetGraphic(3);

                            var bitMap = GenerarQv2(URLAbsolute);

                            var finalQR = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(bitMap));

                            viewModel.QRCode = finalQR;


                            if (abreviationTheme != ConstantHelpers.Institution.Values[ConstantHelpers.Institution.UNJBG])
                            {
                                viewToString = await _viewRenderService.RenderToStringAsync($"/Areas/Admin/Views/Diploma/akdemic/DiplomaPdfView.cshtml", viewModel);
                            }
                            else
                            {
                                viewToString = await _viewRenderService.RenderToStringAsync($"/Areas/Admin/Views/Diploma/{abreviationTheme}/DiplomaPdfView.cshtml", viewModel);
                            }


                            var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css/areas/admin/diplomaReport.css");

                            var objectSettings = new DinkToPdf.ObjectSettings
                            {
                                PagesCount = true,
                                HtmlContent = viewToString,
                                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPath },

                            };

                            var globalSettings = new DinkToPdf.GlobalSettings();



                            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG)
                            {

                                globalSettings.ColorMode = DinkToPdf.ColorMode.Color;
                                globalSettings.Orientation = DinkToPdf.Orientation.Landscape;
                                globalSettings.PaperSize = DinkToPdf.PaperKind.A4;
                                globalSettings.Margins = new DinkToPdf.MarginSettings { Top = 60, Bottom = 10, Left = 30, Right = 30 };

                            }
                            else
                            {
                                globalSettings.ColorMode = DinkToPdf.ColorMode.Color;
                                globalSettings.Orientation = DinkToPdf.Orientation.Portrait;
                                globalSettings.PaperSize = DinkToPdf.PaperKind.A4;
                                globalSettings.Margins = new DinkToPdf.MarginSettings { Top = 40, Bottom = 30, Left = 10, Right = 10 };
                            }

                            var pdf = new DinkToPdf.HtmlToPdfDocument
                            {
                                GlobalSettings = globalSettings,
                                Objects = { objectSettings }
                            };

                            var fileByte = _dinkConverter.Convert(pdf);

                            System.IO.Compression.ZipArchiveEntry zipItem = zip.CreateEntry($"{rp.Student.User.UserName}.pdf");
                            // add the item bytes to the zip entry by opening the original file and copying the bytes
                            using (System.IO.MemoryStream originalFileMemoryStream = new System.IO.MemoryStream(fileByte))
                            {
                                using (System.IO.Stream entryStream = zipItem.Open())
                                {
                                    originalFileMemoryStream.CopyTo(entryStream);
                                }
                            }
                        }

                    }
                    fileBytesOutPut = memoryStream.ToArray();
                }


                // download the constructed zip
                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                //Response.AddHeader("Content-Disposition", "attachment; filename=download.zip");
                return File(fileBytesOutPut, "application/zip", "diplomas.zip");

                //return File(fileByte, "application/pdf", "diploma.pdf");
                ////return View($"/Areas/Admin/Views/Diploma/{abreviationTheme}/DiplomaPdfView.cshtml", viewModel);
                ///
            }
            catch (Exception e)
            {
                return BadRequest("No se pudo descargar el archivo");
            }


        }

        /// <summary>
        /// Genera el código QR
        /// </summary>
        /// <param name="informacionQR">Texto del código QR</param>
        /// <returns>Retorna un arreglo de bytes</returns>
        private byte[] GenerarQv2(string informacionQR)
        {
            string valorQR = $"{informacionQR}";
            QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            QRCoder.QRCodeData qrCodeData = qrGenerator.CreateQrCode(valorQR, QRCoder.QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCoder.PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(5);
        }
    }
}
