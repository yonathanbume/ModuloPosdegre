// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Areas.Report.ViewModels.MeritChartViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," + ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + CORE.Helpers.ConstantHelpers.ROLES.VICERRECTOR)]
    [Area("Report")]
    [Route("reporte/cuadro-merito")]
    public class MeritChartController : BaseController
    {
        private readonly AkdemicContext _context;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IConverter _dinkConverter;
        private readonly IViewRenderService _viewRenderService;
        private readonly ITextSharpService _pdfSharpService;
        private readonly IConfigurationService _configurationService;

        public MeritChartController(AkdemicContext context, IWebHostEnvironment hostingEnvironment,
            IConverter dinkConverter, IViewRenderService viewRenderService,
            ITextSharpService pdfSharpService, IConfigurationService configurationService)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _pdfSharpService = pdfSharpService;
            _configurationService = configurationService;
        }

        /// <summary>
        /// Vista donde se muestran los cuadros de mérito
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            var meritOrderByAcademicYear = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.MERIT_ORDER_BY_ACADEMIC_YEAR));
            var model = new IndexViewModel
            {
                OrderMeritByAcademicYear = meritOrderByAcademicYear,
                OrderMeritByCampus = ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM
            };

            return View(model);
        }

        /// <summary>
        /// Método para calcular el orden de mérito
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="academicYear">Año académico</param>
        /// <param name="type">Tipo</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpGet("calcular-orden-merito")]
        public async Task<IActionResult> CalculateMeritOrder(Guid termId, Guid careerId, int? academicYear = null, int type = 0, Guid? campusId = null, int? year = null)
        {
            if (termId == Guid.Empty || careerId == Guid.Empty)
                return BadRequest("Debe seleccionar todos los campos");

            var career = await _context.Careers
                .Where(x => x.Id == careerId)
                .Include(x => x.Faculty)
                .FirstOrDefaultAsync();

            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Id == termId);

            var academicYearCreditsList = await _context.AcademicYearCredits
                .Where(x => x.Curriculum.CareerId == career.Id && x.AcademicYear == academicYear)
                .AsNoTracking()
                .ToListAsync();

            var query = _context.AcademicSummaries
                .Where(x => x.CareerId == career.Id
                && x.TermId == term.Id
                //&& x.TermHasFinished
                //&& x.MeritOrder > 0
                //&& x.Student.AcademicProgramId.HasValue
                && !x.WasWithdrawn)
                .AsNoTracking();

            if (academicYear.HasValue)
                query = query.Where(x => x.StudentAcademicYear == academicYear);

            if (campusId.HasValue)
                query = query.Where(x => x.CampusId == campusId);

            if (year.HasValue)
                query = query.Where(x => x.Student.FirstEnrollmentTermId.HasValue && x.Student.FirstEnrollmentTerm.Year == year);

            var academicSummaries = await query
                .Select(x => new
                {
                    x.CurriculumId,
                    x.StudentId,
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                    x.Student.AcademicProgramId,
                    AcademicProgram = x.Student.AcademicProgram.Code,
                    x.ApprovedCredits,
                    x.TotalCredits,
                    Curriculum = x.Curriculum.Code,
                    x.MeritOrder,
                    x.MeritType,
                    x.TotalOrder,
                    x.WeightedAverageGrade,
                    x.WeightedAverageCumulative,
                    x.TermHasFinished
                })
                .ToListAsync();

            if (academicSummaries.Any(x => !x.TermHasFinished))
                return BadRequest("Alumnos presentan notas pendientes");

            if (academicSummaries.Any(x => x.MeritOrder <= 0))
                return BadRequest("Falta calcular el orden de merito");

            if (academicSummaries.Count == 0)
                return BadRequest("No se encontraron alumnos con notas completas");

            var academicHistories = await _context.AcademicHistories
                .Where(x => !x.Withdraw && x.TermId == term.Id && x.Student.AcademicSummaries.Any(y => y.CareerId == career.Id && y.TermId == term.Id) && ((x.Type == ConstantHelpers.AcademicHistory.Types.REGULAR && !x.Validated) || x.Type == ConstantHelpers.AcademicHistory.Types.DIRECTED))
                .Select(x => new
                {
                    x.StudentId,
                    x.Try
                })
                .ToListAsync();

            var meritOrderGradeType = byte.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.MERIT_ORDER_GRADE_TYPE));

            var studentMerits = new List<StudentMeritViewModel>();
            foreach (var academicSummary in academicSummaries)
            {
                var academicYearCredits = academicYearCreditsList.Where(x => x.CurriculumId == academicSummary.CurriculumId).Select(x => x.Credits).FirstOrDefault();

                var studentAcademicHistories = academicHistories.Where(x => x.StudentId == academicSummary.StudentId).ToList();
                if (studentAcademicHistories.Count == 0)
                    continue;

                var studentMerit = new StudentMeritViewModel
                {
                    StudentId = academicSummary.StudentId,
                    Code = academicSummary.UserName,
                    Name = academicSummary.FullName,
                    AcademicProgram = academicSummary.AcademicProgram,
                    ApprovedCredits = academicSummary.ApprovedCredits,
                    DisapprovedCredits = academicSummary.TotalCredits - academicSummary.ApprovedCredits,
                    AcademicYearCredits = academicYearCredits,
                    CurriculumCode = academicSummary.Curriculum,
                    MeritOrder = academicSummary.MeritOrder,
                    TotalOrder = academicSummary.TotalOrder,
                    MeritType = academicSummary.MeritType,
                    Grade = meritOrderGradeType == 2 ? academicSummary.WeightedAverageCumulative : academicSummary.WeightedAverageGrade
                };

                //PESO DE LAS MODALIDADES
                var canmat = 99;
                if (studentAcademicHistories.Any()) canmat = studentAcademicHistories.OrderByDescending(x => x.Try).FirstOrDefault().Try;
                if (canmat == 2) canmat = 1;
                studentMerit.MaxTry = canmat;
                studentMerits.Add(studentMerit);
            }

            var total = studentMerits.Count;
            var chartName = "CUADRO DE MÉRITOS";
            var additionalText = "";
            switch (type)
            {
                case 1:
                    studentMerits = studentMerits.Where(x => x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_HALF
                    || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD
                    || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH
                    || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH).ToList();
                    chartName = "MEDIO SUPERIOR";
                    additionalText = "Art. 15 inciso d): El Medio Superior lo constituirán la media parte superior de los estudiantes del Cuadro de Mérito Semestral o Promocional de cada Escuela Profesional.";
                    break;
                case 2:
                    studentMerits = studentMerits.Where(x => x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD
                    || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH
                    || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH).ToList();
                    chartName = "TERCIO SUPERIOR";
                    additionalText = "Art. 15 inciso d): El Tercio Superior lo constituirán la tercera parte superior de los estudiantes del Cuadro de Mérito Semestral o Promocional de cada Escuela Profesional.";
                    break;
                case 3:
                    studentMerits = studentMerits.Where(x => x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH
                    || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH).ToList();
                    chartName = "QUINTO SUPERIOR";
                    additionalText = "Art. 15 inciso d): El Quinto Superior lo constituirán la quinta parte superior de los estudiantes del Cuadro de Mérito Semestral o Promocional de cada Escuela Profesional.";
                    break;
                case 4:
                    studentMerits = studentMerits.Where(x => x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH).ToList();
                    chartName = "DÉCIMO SUPERIOR";
                    additionalText = "Art. 15 inciso d): El Décimo Superior lo constituirán la décima parte superior de los estudiantes del Cuadro de Mérito Semestral o Promocional de cada Escuela Profesional.";
                    break;
                default:
                    break;
            }

            var model = new MeritChartPdfViewModel
            {
                ChartName = chartName,
                ChartAdditionalText = additionalText,
                Image1 = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, @"images/peru-report.png"),
                Image2 = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, @"images/themes/" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                AcademicYear = academicYear.HasValue ? ConstantHelpers.ACADEMIC_YEAR.TEXT[academicYear.Value] : "---",
                Career = career.Name,
                Faculty = career.Faculty.Name,
                Term = term.Name,
                GradeType = meritOrderGradeType,
                TotalStudents = total,
                Students = studentMerits
                .OrderBy(x => x.TotalOrder)
                    .Select(x => new MeritChartPdfStudentViewModel
                    {
                        Number = x.TotalOrder,
                        MeritOrder = x.MeritOrder,
                        Code = x.Code,
                        Name = x.Name,
                        AcademicProgram = x.AcademicProgram,
                        AverageGrade = x.Grade,
                        Curriculum = x.CurriculumCode,
                        CurriculumCredits = x.AcademicYearCredits,
                        EnrolledCredits = x.ApprovedCredits + x.DisapprovedCredits,
                        ApprovedCredits = x.ApprovedCredits,
                        DisapprovedCredits = x.DisapprovedCredits,
                        Modality = ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES[x.MaxTry],
                        Condition = x.DisapprovedCredits == 0 ? "INVICTO" : $"{x.DisapprovedCredits} CRD. DESAP."
                    }).ToList()
            };

            if (campusId.HasValue)
            {
                var campus = await _context.Campuses.Where(x => x.Id == campusId).FirstOrDefaultAsync();
                model.Campus = campus.Name;
            }

            var header = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.DOCUMENT_HEADER_TEXT);
            model.HeaderText = header;

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 8, Bottom = 8, Left = 8, Right = 8 },
                DocumentTitle = $"CUADRO DE MERITOS {term.Name} {career.Code} {academicYear}",
                DPI = 380
            };

            if (type != 0) globalSettings.DocumentTitle = globalSettings.DocumentTitle + $" {chartName}";

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Report/Views/MeritChart/MeritChartPdf.cshtml", model);
            var printTime = DateTime.UtcNow.ToDefaultTimeZone();

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8"
                //, UserStyleSheet = cssPath
                },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 8,
                    Line = true,
                    Left = $"Fecha : {printTime.Day} de {ConstantHelpers.MONTHS.VALUES[printTime.Month]} del {printTime.Year} - Hora: {printTime:H:mm:ss} hrs.",
                    Center = "",
                    Right = "Pag: [page]/[toPage]"
                }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            var fileByte = _dinkConverter.Convert(pdf);

            if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAB && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAH && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNSM)
                _pdfSharpService.AddWatermarkToAllPages(ref fileByte, "NO OFICIAL", 120);

            return File(fileByte, "application/pdf", globalSettings.DocumentTitle + ".pdf");
        }

        [HttpGet("calcular-orden-merito-old")]
        public async Task<IActionResult> CalculateMeritOrderOld(Guid termId, Guid careerId, int academicYear, int type = 0)
        {
            if (termId == Guid.Empty || careerId == Guid.Empty)
                return BadRequest("Debe seleccionar todos los campos");

            var career = await _context.Careers
                .Where(x => x.Id == careerId)
                .Include(x => x.Faculty)
                .FirstOrDefaultAsync();
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Id == termId);

            var studentMerits = new List<StudentMeritViewModel>();

            var allAcademicYearCredits = await _context.AcademicYearCredits
                .Where(x => x.Curriculum.CareerId == career.Id)
                .AsNoTracking()
                .ToListAsync();

            var academicYearCreditsList = allAcademicYearCredits.Where(x => x.AcademicYear == academicYear).ToList();

            #region Verificar notas pendientes

            var pendingGrades = false;
            var students = await _context.Students
                .Where(x => x.CareerId == career.Id && x.AcademicSummaries.Any(y => y.TermId == term.Id && !y.TermHasFinished))
                .Select(x => new
                {
                    x.Id,
                    x.CurriculumId,
                    ApprovedCredits = x.AcademicHistories
                         .Where(y => y.Term.StartDate < term.StartDate && y.Course.AcademicYearCourses.Any(z => z.CurriculumId == x.CurriculumId) && y.Approved)
                         .Sum(y => y.Course.Credits),
                    AdditionalCredits = x.StudentSections
                         .Where(y => y.Section.CourseTerm.TermId == term.Id)
                         .Sum(y => y.Section.CourseTerm.Course.Credits)
                })
                .ToListAsync();

            foreach (var student in students)
            {
                var totalCredits = student.ApprovedCredits + student.AdditionalCredits;
                var studentAcademicYear = allAcademicYearCredits.Where(x => x.CurriculumId == student.CurriculumId && x.StartCredits <= totalCredits && totalCredits <= x.EndCredits).FirstOrDefault();

                if (studentAcademicYear == null)
                {
                    if (allAcademicYearCredits.Where(x => x.CurriculumId == student.CurriculumId).Count() == 0)
                    {
                        return BadRequest("Cuadro de mérito de la escuela no generado");
                    }

                    if (totalCredits > allAcademicYearCredits.Where(x => x.CurriculumId == student.CurriculumId).OrderByDescending(x => x.AcademicYear).First().EndCredits)
                        studentAcademicYear = allAcademicYearCredits.Where(x => x.CurriculumId == student.CurriculumId).OrderByDescending(x => x.AcademicYear).First();
                }

                if (studentAcademicYear.AcademicYear == academicYear) pendingGrades = true;
            }

            if (pendingGrades) return BadRequest("Alumnos presentan notas pendientes");

            #endregion

            var academicSummaries = await _context.AcademicSummaries
                .Where(x => x.CareerId == career.Id
                && x.StudentAcademicYear == academicYear
                && x.TermId == term.Id
                && x.TermHasFinished
                && x.MeritOrder > 0
                && x.Student.AcademicProgramId.HasValue)
                .Select(x => new
                {
                    x.CurriculumId,
                    x.StudentId,
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                    AcademicProgram = x.Student.AcademicProgram.Code,
                    x.ApprovedCredits,
                    x.TotalCredits,
                    Curriculum = x.Curriculum.Code,
                    x.MeritOrder,
                    x.MeritType,
                    x.TotalOrder,
                    x.WeightedAverageGrade,
                    x.TermHasFinished
                })
                .ToListAsync();

            if (academicSummaries.Any(x => !x.TermHasFinished))
                return BadRequest("Alumnos presentan notas pendientes");

            if (academicSummaries.Count == 0)
                return BadRequest("No se encontraron alumnos con notas completas");

            var academicHistories = await _context.AcademicHistories
                .Where(x => !x.Withdraw && x.TermId == term.Id && x.Student.AcademicSummaries.Any(y => y.CareerId == career.Id && y.TermId == term.Id) && ((x.Type == ConstantHelpers.AcademicHistory.Types.REGULAR && !x.Validated) || x.Type == ConstantHelpers.AcademicHistory.Types.DIRECTED))
                .Select(x => new
                {
                    x.StudentId,
                    x.Try
                })
                .ToListAsync();

            foreach (var academicSummary in academicSummaries)
            {
                var academicYearCredits = academicYearCreditsList.Where(x => x.CurriculumId == academicSummary.CurriculumId).Select(x => x.Credits).FirstOrDefault();

                var studentAcademicHistories = academicHistories.Where(x => x.StudentId == academicSummary.StudentId).ToList();
                if (studentAcademicHistories.Count == 0)
                    continue;

                var studentMerit = new StudentMeritViewModel
                {
                    StudentId = academicSummary.StudentId,
                    Code = academicSummary.UserName,
                    Name = academicSummary.FullName,
                    AcademicProgram = academicSummary.AcademicProgram,
                    ApprovedCredits = academicSummary.ApprovedCredits,
                    DisapprovedCredits = academicSummary.TotalCredits - academicSummary.ApprovedCredits,
                    AcademicYearCredits = academicYearCredits,
                    CurriculumCode = academicSummary.Curriculum,
                    MeritOrder = academicSummary.MeritOrder,
                    TotalOrder = academicSummary.TotalOrder,
                    MeritType = academicSummary.MeritType,
                    Grade = academicSummary.WeightedAverageGrade
                };

                //PESO DE LAS MODALIDADES
                var canmat = 99;
                if (studentAcademicHistories.Any()) canmat = studentAcademicHistories.OrderByDescending(x => x.Try).FirstOrDefault().Try;
                if (canmat == 2) canmat = 1;

                studentMerit.MaxTry = canmat;

                studentMerits.Add(studentMerit);
            }

            var chartName = "CUADRO DE MÉRITOS";
            var additionalText = "";
            switch (type)
            {
                case 1:
                    studentMerits = studentMerits.Where(x => x.MeritType >= ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD).ToList();
                    chartName = "TERCIO SUPERIOR";
                    additionalText = "Art. 15 inciso d): El Tercio Superior lo constituirán la tercera parte superior de los estudiantes del Cuadro de Mérito Semestral o Promocional de cada Escuela Profesional.";
                    break;
                case 2:
                    studentMerits = studentMerits.Where(x => x.MeritType >= ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH).ToList();
                    chartName = "QUINTO SUPERIOR";
                    additionalText = "Art. 15 inciso d): El Quinto Superior lo constituirán la quinta parte superior de los estudiantes del Cuadro de Mérito Semestral o Promocional de cada Escuela Profesional.";
                    break;
                case 3:
                    studentMerits = studentMerits.Where(x => x.MeritType >= ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH).ToList();
                    chartName = "DÉCIMO SUPERIOR";
                    additionalText = "Art. 15 inciso d): El Décimo Superior lo constituirán la décima parte superior de los estudiantes del Cuadro de Mérito Semestral o Promocional de cada Escuela Profesional.";
                    break;
                default:
                    break;
            }

            var model = new MeritChartPdfViewModel
            {
                ChartName = chartName,
                ChartAdditionalText = additionalText,
                Image1 = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, @"images/peru-report.png"),
                Image2 = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, @"images/themes/" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                AcademicYear = ConstantHelpers.ACADEMIC_YEAR.TEXT[academicYear],
                Career = career.Name,
                Faculty = career.Faculty.Name,
                Term = term.Name,
                Students = studentMerits
                .OrderBy(x => x.TotalOrder)
                    .Select(x => new MeritChartPdfStudentViewModel
                    {
                        Number = x.TotalOrder,
                        MeritOrder = x.MeritOrder,
                        Code = x.Code,
                        Name = x.Name,
                        AcademicProgram = x.AcademicProgram,
                        AverageGrade = x.Grade,
                        Curriculum = x.CurriculumCode,
                        CurriculumCredits = x.AcademicYearCredits,
                        EnrolledCredits = x.ApprovedCredits + x.DisapprovedCredits,
                        ApprovedCredits = x.ApprovedCredits,
                        DisapprovedCredits = x.DisapprovedCredits,
                        Modality = ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES[x.MaxTry],
                        Condition = x.DisapprovedCredits == 0 ? "INVICTO" : $"{x.DisapprovedCredits} CRD. DESAP."
                    }).ToList()
            };

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 8, Bottom = 8, Left = 8, Right = 8 },
                DocumentTitle = $"CUADRO DE MERITOS {term.Name} {career.Code} {academicYear}",
                DPI = 380
                //DPI = 96
            };

            if (type != 0) globalSettings.DocumentTitle = globalSettings.DocumentTitle + $" {chartName}";

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Report/Views/MeritChart/MeritChartPdf.cshtml", model);
            var printTime = DateTime.UtcNow.ToDefaultTimeZone();

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8"
                //, UserStyleSheet = cssPath
                },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 8,
                    Line = true,
                    Left = $"Fecha : {printTime.Day} de {ConstantHelpers.MONTHS.VALUES[printTime.Month]} del {printTime.Year} - Hora: {printTime:H:mm:ss} hrs.",
                    Center = "",
                    Right = "Pag: [page]/[toPage]"
                }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            var fileByte = _dinkConverter.Convert(pdf);

            if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAB && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAH)
                _pdfSharpService.AddWatermarkToAllPages(ref fileByte, "NO OFICIAL", 120);

            return File(fileByte, "application/pdf", globalSettings.DocumentTitle + ".pdf");
        }

        /// <summary>
        /// Obtiene el listado de periodos académicos
        /// </summary>
        /// <returns>Listado de periodos académicos</returns>
        [HttpGet("periodos/get")]
        public async Task<IActionResult> GetTerms()
        {
            var terms = await _context.Terms
                .Where(x => x.AcademicSummaries.Any(y => y.MeritOrder > 0))
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Number)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

            var active = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            return Ok(new { items = terms, selected = (active?.Id.ToString()) });
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales que tengan un cuadro de mérito generado
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Listado de escuelas profesionales</returns>
        [HttpGet("periodos/{termId}/carreras/get")]
        public async Task<IActionResult> GetCareers(Guid termId)
        {
            var query = _context.Careers
                .Where(x => x.AcademicSummaries.Any(y => y.TermId == termId && y.MeritOrder > 0))
                .AsNoTracking();

            if (User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                query = query.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId);
            }

            var careers = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

            return Ok(new { items = careers.OrderBy(x => x.text).ToList() });
        }

        /// <summary>
        /// Obtiene el listado de ciclos académicos que tengan al menos un cuadro de mérito generado
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesinal</param>
        /// <returns>Listado de ciclos académicos</returns>
        [HttpGet("periodos/{termId}/carrera/{careerId}/ciclos/get")]
        public async Task<IActionResult> GetCareerAcademicYears(Guid termId, Guid careerId)
        {
            var academicSummaries = await _context.AcademicSummaries
                .Where(x => x.TermId == termId && x.CareerId == careerId && x.MeritOrder > 0)
                .Select(x => x.StudentAcademicYear)
                .ToListAsync();

            var academicYears = academicSummaries
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return Ok(new { items = academicYears });
        }

        [HttpGet("periodos/{termId}/carrera/{careerId}/sedes/get")]
        public async Task<IActionResult> GetCareerCampuses(Guid termId, Guid careerId)
        {
            var campuses = await _context.AcademicSummaries
                .Where(x => x.TermId == termId && x.CareerId == careerId && x.MeritOrder > 0 && x.CampusId.HasValue)
                .Select(x =>
                new
                {
                    id = x.CampusId,
                    text = x.Campus.Name,
                })
                .ToListAsync();

            var data = campuses
                .Distinct()
                .OrderBy(x => x.text)
                .ToList();

            return Ok(new { items = data });
        }
        [HttpGet("periodos/{termId}/carrera/{careerId}/anios/get")]
        public async Task<IActionResult> GetCareerYears(Guid termId, Guid careerId)
        {
            var years = await _context.AcademicSummaries
                .Where(x => x.TermId == termId && x.CareerId == careerId && x.MeritOrder > 0 && x.Student.FirstEnrollmentTermId.HasValue)
                .Select(x => x.Student.FirstEnrollmentTerm.Year)
                .ToListAsync();

            var data = years
                .Distinct()
                .Select(x => new
                {
                    id = x,
                    text = x.ToString()
                })
                .OrderBy(x => x.text)
                .ToList();

            return Ok(new { items = data });
        }
    }
}
