using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.RankingViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.DEAN + "," +
        ConstantHelpers.ROLES.DEAN_SECRETARY + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)]
    [Area("CareerDirector")]
    [Route("director-carrera/ranking")]
    public class RankingController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConverter _dinkConverter;
        private readonly IViewRenderService _viewRenderService;
        private readonly IDataTablesService _dataTablesService;
        private readonly ISelect2Service _select2Service;

        private readonly IRecordHistoryService _recordHistoryService;
        private readonly IStudentService _studentService;
        private readonly ICareerService _careerService;
        private readonly ICampusService _campusService;
        private readonly IAcademicProgramService _academicProgramService;

        public RankingController(
            UserManager<ApplicationUser> userManager,
            IDataTablesService dataTablesService,
            ISelect2Service select2Service,
            IWebHostEnvironment hostingEnvironment,
            IViewRenderService viewRenderService,
            IConverter converter,
            IStudentService studentService,
            ICampusService campusService,
            ICareerService careerService,
            IRecordHistoryService recordHistoryService,
            ITermService termService,
            IAcademicProgramService academicProgramService
            ) : base(userManager, termService)
        {
            _dataTablesService = dataTablesService;
            _select2Service = select2Service;
            _hostingEnvironment = hostingEnvironment;
            _dinkConverter = converter;
            _viewRenderService = viewRenderService;
            _studentService = studentService;
            _careerService = careerService;
            _campusService = campusService;
            _recordHistoryService = recordHistoryService;
            _academicProgramService = academicProgramService;
        }

        /// <summary>
        /// Vista donde se muestra el cuadro de mérito de los alumnos
        /// </summary>
        /// <returns>Vista cuadro de mérito</returns>
        [HttpGet("cuadro-meritos")]
        public IActionResult MeritTable()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de alumnos donde se muestra su orden de mérito académico para ser usado en tablas.
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificadro de la escuela profesional</param>
        /// <param name="academicProgramId">Identificador del programa académico</param>
        /// <param name="campusId">Identificador del campus</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de alumnos</returns>
        [HttpGet("cuadro-meritos/get")]
        public async Task<IActionResult> GetStudents(Guid termId, Guid? careerId = null, Guid? academicProgramId = null, Guid? campusId = null, string search = null)
        {
            var parameters = _dataTablesService.GetSentParameters();
            if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var resultdean = await _studentService.GetStudentRankingByTermDataTable(parameters, _userManager.GetUserId(User), termId, careerId, academicProgramId, campusId, search, User);
                return Ok(resultdean);
            }

            var result = await _studentService.GetStudentRankingByTermDataTable(parameters, _userManager.GetUserId(User), termId, careerId, academicProgramId, campusId, search);
            return Ok(result);
        }

        /// <summary>
        /// Genera un reporte donde se detalla el orden de mérito de los estudiantes
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">identificador de la escuela profesional</param>
        /// <param name="academicProgramId">Identificador del progama académico</param>
        /// <param name="campusId">Identificador del campus</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("cuadro-meritos/pdf/get")]
        public async Task<IActionResult> GetStudentRankingPdf(Guid termId, Guid? careerId = null, Guid? academicProgramId = null, Guid? campusId = null)
        {
            var students = await _studentService.GetStudentRankingByTerm(termId, careerId, campusId);
            var academicPrograms = academicProgramId.HasValue
                ? await _academicProgramService.Get(academicProgramId.Value)
                : null;
            var careers = await _careerService.GetCareerByUserCoordinatorId(_userManager.GetUserId(User));
            var term = await _termService.Get(termId);
            var campus = campusId.HasValue ? await _campusService.Get(campusId.Value) : null;
            var career = careerId.HasValue ? await _careerService.Get(careerId.Value) : null;

            if (User.IsInRole(ConstantHelpers.ROLES.DEAN))
            {
                var userId = _userManager.GetUserId(User);
                students = students
               .Where(x => (x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId) && academicPrograms?.Id == x.AcademicProgramId);
            }
            else
            {
                students = students
              .Where(x => careers.Any(y => y.Id == x.CareerId) && academicPrograms?.Id == x.AcademicProgramId);
            }

            var model = new StudentRankingPdfViewModel
            {
                FilterInformation = new StudentFilterInformationViewModel
                {
                    Term = term.Name,
                    Career = career?.Name,
                    Campus = campus?.Name.Substring(0, 1).Insert(1, ".")
                },
                StudentSummaries = students
                .OrderByDescending(x => x.AcademicSummaries.Select(asm => asm.WeightedAverageGrade).FirstOrDefault())
                .ThenBy(x => x.Career?.Id)
                .Select((x, i) =>
                {
                    var academicSummary = x.AcademicSummaries.FirstOrDefault();
                    return new StudentSummaryViewModel
                    {
                        Position = i + 1,
                        AcademicYear = academicSummary?.StudentAcademicYear.ToString("D2") ?? "-",
                        Code = x.User.UserName,
                        Campus = x.Campus?.Name,
                        Career = x.Career?.Name,
                        Name = x.User.FullName,
                        WeightedAverageGrade = academicSummary?.WeightedAverageGrade ?? 0,
                        Credits = academicSummary?.TotalCredits ?? 0,
                        MeritOrder = academicSummary?.MeritOrder ?? 0,
                        MeritType = academicSummary?.MeritType
                    };
                }).ToList()
            };
            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/Ranking/StudentRakingPdf.cshtml", model);
            var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css\areas\admin\ranking\rankingreportspdf.css");
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 0, Left = 10, Right = 10 }
            };
            var objectSettings = new DinkToPdf.ObjectSettings
            {
                HtmlContent = viewToString,
                WebSettings =
                {
                    DefaultEncoding = "utf-8",
                    UserStyleSheet = cssPath
                },
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
            return File(fileByte, "application/pdf");
        }

        /// <summary>
        /// Vista donde se muestra el raking de estudiantes por número de créditos
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("estudiantes-por-creditos")]
        public IActionResult StudentRankingForCredits()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de alumnos detallando la cantidad de créditos finalizados filtrados por los siguientes parámetros para ser usado en tablas.
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="academicProgramId">Identificador del programa académico</param>
        /// <param name="campusId">Identificador del campus</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Alumnos filtrados</returns>
        [HttpGet("estudiantes-por-creditos/get")]
        public async Task<IActionResult> GetStudentRankingForCredits(Guid termId, Guid? careerId = null, Guid? academicProgramId = null, Guid? campusId = null, string search = null)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetStudentRankingForCreditsDataTable(parameters, User, termId, careerId, academicProgramId, campusId, search);
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se muestra el listado de egresados
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("egresados")]
        public IActionResult Graduateds()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de egresados filtrados por los siguientes parámetros para ser usado en tablas.
        /// </summary>
        /// <param name="admissionTermId">Identificador del periodo de admisión</param>
        /// <param name="graduationTermId">Identificador del periodo de grdauación</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="academicProgramId">Identificador del programa académico</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de egresados</returns>
        [HttpGet("egresados/get")]
        public async Task<IActionResult> GetGraduateds(Guid? admissionTermId = null, Guid? graduationTermId = null, Guid? careerId = null, Guid? academicProgramId = null, string search = null)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetGraduatedsDataTable(parameters, User, admissionTermId, graduationTermId, careerId, academicProgramId, search);
            return Ok(result);
        }

        /// <summary>
        /// Genera un reporte de los alumnos egresados
        /// </summary>
        /// <param name="admissionTermId">Identificador del periodo de admisión</param>
        /// <param name="graduationTermId">Identificador del periodo de graduación</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="academicProgramId">Identificador del programa académico</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("egresados/pdf/get")]
        public async Task<IActionResult> GraduatedRankingPdf(Guid? admissionTermId = null, Guid? graduationTermId = null, Guid? careerId = null, Guid? academicProgramId = null)
        {
            var graduateds = await _studentService.GetGraduatedsRankingByTerms(admissionTermId, graduationTermId, careerId);
            var admissionTerm = admissionTermId.HasValue ? await _termService.Get(admissionTermId.Value) : null;
            var graduationTerm = graduationTermId.HasValue ? await _termService.Get(graduationTermId.Value) : null;
            var career = careerId.HasValue ? await _careerService.Get(careerId.Value) : null;
            var academicPrograms = academicProgramId.HasValue ? await _academicProgramService.Get(academicProgramId.Value) : null;

            var userId = _userManager.GetUserId(User);
            var careers = await _careerService.GetCareerByUserCoordinatorId(userId);

            if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                graduateds = graduateds.Where(x => (x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId) && academicPrograms?.Id == x.AcademicProgramId);
            }
            else
            {
                graduateds = graduateds.Where(x => careers.Any(y => y.Id == x.CareerId) && academicPrograms?.Id == x.AcademicProgramId);
            }

            var model = new GraduatedRankingPdfViewModel
            {
                GraduatedFilterInformation = new GraduatedFilterInformationViewModel
                {
                    Career = career?.Name,
                    AdmissionTerm = admissionTerm?.Name,
                    GraduationTerm = graduationTerm?.Name,
                    AdmissionTermHyphenated = admissionTerm?.Name,
                    GraduationTermHyphenated = graduationTerm?.Name
                },
                GraduatedSummaries = graduateds
                .OrderByDescending(x => x.AcademicSummaries.Select(asm => asm.WeightedAverageGrade).FirstOrDefault())
                .ThenBy(x => x.Career?.Id)
                .Select((x, i) =>
                {
                    var academicSummary = x.AcademicSummaries.FirstOrDefault();
                    return new GraduatedSummaryViewModel
                    {
                        Position = i + 1,
                        Code = x.User.UserName,
                        Dni = x.User.Dni,
                        Career = x.Career?.Name,
                        Name = x.User.FullName,
                        AdmissionTerm = x.AdmissionTerm?.Name,
                        GraduationTerm = x.GraduationTerm?.Name,
                        WeightedAverageGrade = academicSummary?.WeightedAverageGrade ?? 0,
                        MeritOrder = academicSummary?.MeritOrder ?? 0,
                        MeritType = academicSummary?.MeritType
                    };
                }).ToList()
            };
            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/Ranking/GraduatedRankingPdf.cshtml", model);
            var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css\areas\admin\ranking\rankingreportspdf.css");
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
            };
            var objectSettings = new DinkToPdf.ObjectSettings
            {
                HtmlContent = viewToString,
                WebSettings =
                {
                    DefaultEncoding = "utf-8",
                    UserStyleSheet = cssPath
                },
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
            return File(fileByte, "application/pdf", $"ranking_graduados{career?.Name.Insert(0, "_").Trim()}{admissionTerm?.Name.Insert(0, "_").Trim()}{graduationTerm?.Name.Insert(0, "_").Trim()}.pdf");
        }

        /// <summary>
        /// Vista donde se muestra el listado de ingresantes
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("ingresantes")]
        public IActionResult NewStudents()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de alumnos ingresantes filtrados por los siguientes parámetros para ser usado en tablas
        /// </summary>
        /// <param name="admissionTermId">Identificador del periodo de admisión</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="academicProgramId">Identificador del programa académico</param>
        /// <param name="status">Identificador del estado</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de alumnos</returns>
        [HttpGet("ingresantes/get")]
        public async Task<IActionResult> GetNewStudents(Guid? admissionTermId = null, Guid? careerId = null, Guid? academicProgramId = null, int? status = null, string search = null)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetNewStudentsDataTable(parameters, User, admissionTermId, careerId, academicProgramId, status, search);
            return Ok(result);
        }

        /// <summary>
        /// Genera un reporte de los alumnos ingresantes
        /// </summary>
        /// <param name="admissionTermId">Identificador del periodo de admisión</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="academicProgramId">Identificador del programa académico</param>
        /// <param name="status">Identificador del estado</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("ingresantes/pdf/get")]
        public async Task<IActionResult> NewStudentsRankingPdf(Guid? admissionTermId = null, Guid? careerId = null, Guid? academicProgramId = null, int? status = null)
        {
            var newStudents = await _studentService.GetNewStudentsRankingByTerm(admissionTermId, careerId, status);
            var admissionTerm = admissionTermId.HasValue ? await _termService.Get(admissionTermId.Value) : null;
            var academicPrograms = academicProgramId.HasValue ? await _academicProgramService.Get(academicProgramId.Value) : null;
            var userid = _userManager.GetUserId(User);
            var careers = await _careerService.GetCareerByUserCoordinatorId(userid);

            var career = careerId.HasValue ? await _careerService.Get(careerId.Value) : null;
            var statusName = status.HasValue ? ConstantHelpers.Student.States.VALUES.GetValueOrDefault(status.Value) : null;

            if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                newStudents = newStudents
                                .Where(x => (x.Career.Faculty.DeanId == userid || x.Career.Faculty.SecretaryId == userid) && academicPrograms?.Id == x.AcademicProgramId);
            }
            else
            {
                newStudents = newStudents
                .Where(x => careers.Any(y => y.Id == x.CareerId) && academicPrograms?.Id == x.AcademicProgramId);
            }

            var model = new NewStudentRankingPdfViewModel
            {
                NewStudentFilterInformation = new NewStudentFilterInformationViewModel
                {
                    Career = career?.Name,
                    CareerCode = career?.Code,
                    AdmissionTermHyphenated = admissionTerm?.Name,
                    AdmissionTerm = admissionTerm?.Name,
                    Status = statusName
                },
                NewStudentSummaries = newStudents
                .OrderByDescending(x => x.LastAcademicSummary?.MeritType)
                .ThenBy(x => x.Career?.Id)
                .ThenBy(x => x.User.FullName)
                .Select((x, i) => new NewStudentSummaryViewModel
                {
                    Position = i + 1,
                    Code = x.User.UserName,
                    Dni = x.User.Dni,
                    CareerCode = x.Career?.Code,
                    CurriculumCode = x.Curriculum.Code,
                    Name = x.User.FullName,
                    FirstCampus = x.Campus?.Name.Substring(0, 1).Insert(1, "."),
                    CurrentCampus = x.Campus?.Name.Substring(0, 1).Insert(1, "."),
                    AdmissionTerm = x.AdmissionTerm?.Name,
                    LastTerm = x.LastAcademicSummary?.Term.Name,
                    LastWeightedAverageGrade = x.LastAcademicSummary?.WeightedAverageGrade,
                    GraduationTerm = x.Status == ConstantHelpers.Student.States.GRADUATED
                        ? x.GraduationTerm?.Name : null,
                    GraduationWeightedAverageGrade = x.Status == ConstantHelpers.Student.States.GRADUATED
                        ? x.LastAcademicSummary?.WeightedAverageGrade : null,
                    Status = ConstantHelpers.Student.States.VALUES.GetValueOrDefault(x.Status),
                    CurrentAcademicYear = x.CurrentAcademicYear.ToString("D2"),
                    MeritType = x.LastAcademicSummary?.MeritType
                }).ToList()
            };
            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/Ranking/NewStudentRankingPdf.cshtml", model);
            var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css\areas\admin\ranking\rankingreportspdf.css");
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Landscape,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
            };
            var objectSettings = new DinkToPdf.ObjectSettings
            {
                HtmlContent = viewToString,
                WebSettings =
                {
                    DefaultEncoding = "utf-8",
                    UserStyleSheet = cssPath
                },
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
            return File(fileByte, "application/pdf", $"ranking_ingresantes{career?.Name.Insert(0, "_").Trim()}{admissionTerm?.Name.Insert(0, "_").Trim()}{statusName?.Insert(0, "_").Trim()}.pdf");
        }

        #region SELECT's 
        /// <summary>
        /// Obtiene el listado de escuelas profesionales para ser usado en select
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <param name="fid">Identificador de la facultad</param>
        /// <returns>Listado de escuelas profesionales</returns>
        [HttpGet("carreras/get")]
        public async Task<IActionResult> GetCareers(string q, [FromQuery] Guid? fid)
        {
            var requestParameters = _select2Service.GetRequestParameters();

            if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var resultdean = await _careerService.GetCareerSelect2(requestParameters, q, fid, User);
                return Ok(resultdean);
            }

            var result = await _careerService.GetCareerByUserCoordinatorIdSelect2(requestParameters, _userManager.GetUserId(User), q, null, fid);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de programas académicos para ser usado en select
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <returns>Listado de programas académicos</returns>
        [HttpGet("especialidades/get")]
        public async Task<IActionResult> GetAcademicPrograms(string q, [FromQuery] Guid? cid)
        {
            var requestParameters = _select2Service.GetRequestParameters();
            var result = await _academicProgramService.GetAcademicProgramByCareerSelect2(requestParameters, null, cid, q);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de periodos académicos para ser usado en select
        /// </summary>
        /// <returns>Listado de periodos académicos</returns>
        [HttpGet("periodos/get")]
        public async Task<IActionResult> GetTerms()
        {
            var result = await _termService.GetTermsSelect2ClientSide();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de sedes para ser usado en select
        /// </summary>
        /// <returns>Listado de sedes</returns>
        [HttpGet("sedes/get")]
        public async Task<IActionResult> GetCampuses()
        {
            var requestParameters = _select2Service.GetRequestParameters();
            var result = await _campusService.GetAllSelect2(requestParameters);
            return Ok(result);
        }
        #endregion
    }
}
