using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Areas.Admin.Models.CurriculumViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF)]
    [Area("Admin")]
    [Route("admin/planes-de-estudios")]
    public class CurriculumController : BaseController
    {
        public IConverter _dinkConverter { get; }
        private readonly IViewRenderService _viewRenderService;
        private readonly ICareerService _careerService;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly ICurriculumService _curriculumService;
        private readonly IAcademicYearCourseService _academicYearCourseService;
        private readonly IMapper _mapper;
        private readonly IFacultyService _facultyService;

        public CurriculumController(
            ICareerService careerService,
            IConverter dinkConverter,
            IDataTablesService dataTablesService,
            IWebHostEnvironment environment,
            IViewRenderService viewRenderService,
            ICurriculumService curriculumService,
            IAcademicYearCourseService academicYearCourseService,
            IMapper mapper,
            IFacultyService facultyService
        ) : base(dataTablesService)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _careerService = careerService;
            _hostingEnvironment = environment;
            _curriculumService = curriculumService;
            _academicYearCourseService = academicYearCourseService;
            _mapper = mapper;
            _facultyService = facultyService;
        }

        /// <summary>
        /// Vista donde se listan los planes de estudio
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales
        /// </summary>
        /// <param name="fid">Identificador de la facultad</param>
        /// <returns>Listado de escuelas profesionales</returns>
        [HttpGet("carreras/{fid}/get")]
        public async Task<IActionResult> GetCareers(Guid fid)
        {
            if (User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
            {
                var result = await _careerService.GetCareerSelect2ClientSide(null, fid, false, null, User);
                return Ok(result);
            }
            else
            {
                var result = await _careerService.GetCareerSelect2Curriculum(fid);
                return Ok(result);
            }

        }

        /// <summary>
        /// Obtiene el listado de planes de estudio
        /// </summary>
        /// <param name="faculty">Identificador de la facultad</param>
        /// <param name="career">Identificador de la escuela profesional</param>
        /// <param name="academicProgram">Identificador del progama académico</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de planes de estudio</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid faculty, Guid career, Guid academicProgram, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _curriculumService.GetCurriculumDatatable(sentParameters, faculty, career, academicProgram, search, User);
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se detalla los cursos asignados al plan de estudio ordenados por ciclo académico
        /// </summary>
        /// <param name="cid">Identificador del plan de estudio</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("detalle/{cid}")]
        public async Task<IActionResult> CurriculumDetails(Guid cid)
        {
            ViewBag.Id = cid;
            var model = await _academicYearCourseService.GetCurriculumDetail(cid);
            var result = _mapper.Map<List<CurriculumViewModel>>(model);

            var curriculum = await _curriculumService.Get(cid);
            var career = await _careerService.Get(curriculum.CareerId);

            ViewData["curriculum"] = $"{curriculum.Code} - {career.Name}";

            return View(result);
        }

        /// <summary>
        /// Genera el reporte del plan de estudio
        /// </summary>
        /// <param name="id">Identificador del plan de estudio</param>
        /// <returns>Archivo en formato PDF</returns>
        [Route("pdf/{id}")]
        public async Task<IActionResult> DownloadCurriculumPdf(Guid id)
        {
            var rr = await _academicYearCourseService.GetAllAsModelA(curriculumId: id);
            var curriculumList = rr.Select(
                    x => new CoursesCurriculumViewModel
                    {
                        Id = x.Id,
                        AcademicYearNumber = x.AcademicYearNumber,
                        Area = x.Area,
                        Code = x.Code,
                        Credit = x.Credit,
                        Cycle = x.Cycle,
                        Name = x.Name,
                        RequiredCredit = x.RequiredCredit,
                        PlannedHours = x.PlannedHours,
                        PracticalHours = x.PracticalHours,
                        Regularized = x.Regularized,
                        TotalHours = x.TotalHours,
                        Type = x.Type,
                        Requisites = x.Requisites.Count() > 0 ? string.Join(",", x.Requisites) + string.Join(",", x.Certificates) : "No tiene requisitos"
                    }
                ).ToList();
            var numbs = curriculumList.Select(x => x.AcademicYearNumber).Distinct().ToArray();

            var entity = await _curriculumService.GetAsModelA(id: id);

            var img = Path.Combine(_hostingEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png");
            var model = new CurriculumReport
            {
                CurriculumList = curriculumList,
                Logo = img,
                Year = entity?.Year ?? 0,
                Title = entity?.Title ?? "---",
                Cicles = numbs,
                Code = entity.Code ?? ""
            };

            var curriculum = await _curriculumService.Get(id);
            if (curriculum != null)
            {
                var career = await _careerService.Get(curriculum.CareerId);
                model.Career = career != null ? career.Name : "-";

                if (career != null)
                {
                    var faculty = await _facultyService.Get(career.FacultyId);
                    model.Faculty = faculty != null ? faculty.Name : "-";
                }
            }

            var Settings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },

            };

            var viewToString = await _viewRenderService.RenderToStringAsync("/Views/Pdf/CurriculumReport.cshtml", model);
            //var cssPtah = Path.Combine(_hostingEnvironment.WebRootPath, @"css/pages/pdf/curriculumreport.css");

            var objectSettings = new ObjectSettings()
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings =
                {
                    DefaultEncoding = "utf-8",
                    //UserStyleSheet = cssPtah
                },
                FooterSettings = {
                        FontName = "Arial",
                        FontSize = 8,
                        Line = false,
                        Left = $"{DateTime.UtcNow.ToDefaultTimeZone():dd/MM/yyyy} {DateTime.UtcNow.ToDefaultTimeZone().ToLongTimeString()}",
                        Right ="Página [page]"
                    }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = Settings,
                Objects = { objectSettings }
            };

            var fileByte = _dinkConverter.Convert(pdf);

            //return View("/Views/Pdf/CurriculumReport.cshtml", model);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileByte, "application/pdf", "Plan de Estudio.pdf");
        }
    }
}
