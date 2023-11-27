// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.EvaluationReportViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)]
    [Area("CareerDirector")]
    [Route("coordinador-academico/visualizador-actas")]
    public class VisualizationRecordController : BaseController
    {

        private readonly ISectionService _sectionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITeacherSectionService _teacherSectionService;
        private readonly ICurriculumService _curriculumService;
        private readonly IConfigurationService _configurationService;
        private readonly ICourseUnitService _courseUnitService;
        private readonly AkdemicContext _context;
        private readonly IEvaluationReportService _evaluationReportService;

        public VisualizationRecordController(ISectionService sectionService,
            ITeacherSectionService teacherSectionService,
            IDataTablesService dataTablesService,
            ITermService termService,
            ICurriculumService curriculumService,
            IConfigurationService configurationService,
            ICourseUnitService courseUnitService,
            AkdemicContext context,
            IEvaluationReportService evaluationReportService,
            UserManager<ApplicationUser> userManager) : base(termService, dataTablesService)
        {

            _sectionService = sectionService;
            _userManager = userManager;
            _teacherSectionService = teacherSectionService;
            _curriculumService = curriculumService;
            _configurationService = configurationService;
            _courseUnitService = courseUnitService;
            _context = context;
            _evaluationReportService = evaluationReportService;
        }

        /// <summary>
        /// Visualizador de Actas
        /// </summary>
        /// <returns>Vista pinricpal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de actas para ser usado en tablas 
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="courseName">Texto de búsqueda</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="evaluationCode">Código de acta</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="academicProgramId">Identificador del programa académico</param>
        /// <returns>Listado de actas</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid termId, string courseName, Guid? curriculumId = null, string evaluationCode = null, Guid? careerId = null, Guid? academicProgramId = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            sentParameters.OrderColumn = "3";
            sentParameters.OrderDirection = "asc";
            var result = await _sectionService.GetEvaluationReportDatatableV2(sentParameters, termId, null, null, null, careerId, curriculumId, courseName, User, null);
            return Ok(result);
        }

        /// <summary>
        /// Vista parcial donde se muestra la vista previa del acta
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("obtener-vista-previa/{sectionId}")]
        public async Task<IActionResult> _PreviewEvaluationReport(Guid sectionId)
        {
            var sectionStudents = await _context.StudentSections
                .Include(x => x.Student.User)
                .Include(x => x.Section)
                .Where(x => x.SectionId == sectionId && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .OrderBy(x => x.Student.User.FullName)
                .ToListAsync();
            var section = await _sectionService.GetSectionWithTermAndCareer(sectionId);

            if (section == null)
                section = await _teacherSectionService.GetTeacherSectionsWithTermAndCareer(sectionId);


            var model = new ActaScoresViewModel()
            {
                BasicInformation = new BasicInformation(),
                Rows = new List<Row>(),
                Img = ""
            };

            var evaluationReport = await _evaluationReportService.GetEvaluationReportBySectionId(sectionId);
            if (evaluationReport != null)
            {
                model.Number = evaluationReport.Code;
                model.Date = evaluationReport.LastReportGeneratedDate ?? DateTime.UtcNow;
            }

            var curriculumId = section.CourseTerm.Course.AcademicYearCourses.Select(x => x.CurriculumId).FirstOrDefault();
            var teacherSections = await _teacherSectionService.GetAllBySection(sectionId);
            var curriculum = await _curriculumService.Get(curriculumId);

            model.BasicInformation.Teacher = teacherSections.Any() ? teacherSections.Where(y => y.IsPrincipal).Count() > 1 ? "CARGA COMPARTIDA" : teacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).FirstOrDefault() : "--";
            model.BasicInformation.Course = $"[{curriculum?.Code}-{section.CourseTerm?.Course?.Code}] {section.CourseTerm?.Course?.Name}";
            model.BasicInformation.Career = section.CourseTerm?.Course?.Career?.Name;
            model.BasicInformation.Faculty = section.CourseTerm?.Course?.Career?.Faculty?.Name;
            model.Type = section.IsDirectedCourse ? ConstantHelpers.Intranet.EvaluationReportType.DIRECTED_COURSE : ConstantHelpers.Intranet.EvaluationReportType.REGULAR;
            model.BasicInformation.Term = section.CourseTerm?.Term.Name;
            model.BasicInformation.Credits = section.CourseTerm?.Course?.Credits.ToString();
            model.BasicInformation.Section = section.Code;
            model.BasicInformation.IsSummer = section.CourseTerm?.Term?.IsSummer;

            var confiEvaluationsByUnits = await _configurationService.GetByKey(ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT);

            if (confiEvaluationsByUnits is null)
            {
                confiEvaluationsByUnits = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT,
                    Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT]
                };
            }

            var evaluationsByUnits = Convert.ToBoolean(confiEvaluationsByUnits.Value);

            model.BasicInformation.EvaluationByUnits = evaluationsByUnits;

            if (evaluationsByUnits)
            {
                var courseUnits = await _courseUnitService.GetQuantityCourseUnits(section.CourseTerm.Course.Id, section.CourseTerm.Term.Id);
                model.BasicInformation.CourseUnits = courseUnits;

                for (int i = 0; i < sectionStudents.Count; i++)
                {
                    var courseUnitGrades = await _courseUnitService.GetCourseUnitGradesByStudentIdAndSectionId(sectionStudents[i].StudentId, sectionId);
                    var susti = await _context.SubstituteExams.Where(x => x.SectionId == sectionStudents[i].SectionId && x.StudentId == sectionStudents[i].StudentId && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED).Select(x => x.ExamScore).FirstOrDefaultAsync();
                    var averagesByUnits = courseUnitGrades is null ? null : courseUnitGrades.Select(x => x.Average).ToList();

                    int finalAverage = sectionStudents[i].FinalGrade;

                    if (section.CourseTerm.Term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                    {
                        finalAverage = await _context.AcademicHistories.Where(x => x.SectionId == section.Id && x.StudentId == sectionStudents[i].StudentId).Select(x => x.Grade).FirstOrDefaultAsync();
                    }

                    if (averagesByUnits is null)
                    {
                        averagesByUnits = new List<int?>();
                        for (int a = 0; a < courseUnits; a++)
                        {
                            averagesByUnits.Add(null);
                        }
                    }

                    var row = new Row()
                    {
                        Order = i + 1,
                        Code = sectionStudents[i].Student.User.UserName,
                        Surnames = $"{sectionStudents[i].Student.User.PaternalSurname} {sectionStudents[i].Student.User.MaternalSurname}",
                        Names = sectionStudents[i].Student.User.Name,
                        Try = sectionStudents[i].Try == 0 ? 1 : sectionStudents[i].Try,
                        PartialAverages = averagesByUnits.ToArray(),
                        FinalEvaluation = finalAverage,
                        FinalEvaluationNumber = finalAverage,
                        FinalEvaluationText = NUMBERS.VALUES[finalAverage],
                        HasSusti = susti.HasValue && susti.Value >= section.CourseTerm.Term.MinGrade && ((susti <= 14 && finalAverage == susti.Value) || (susti > 14 && finalAverage == 14)),
                        StudentStatus = sectionStudents[i].Status
                    };

                    model.Rows.Add(row);
                }

            }
            else
            {
                var evaluations = await _context.Evaluations.Where(x => x.CourseTermId == section.CourseTermId).OrderBy(x => x.Week).ThenBy(x => x.Percentage).ToListAsync();
                model.BasicInformation.Evaluations = evaluations.Count();
                model.BasicInformation.EvaluationsList = evaluations;

                for (int i = 0; i < sectionStudents.Count; i++)
                {
                    var grades = await _context.Grades.Where(x => x.StudentSectionId == sectionStudents[i].Id && x.EvaluationId.HasValue)
                        .OrderBy(x => x.Evaluation.Week).ThenBy(x => x.Evaluation.Percentage)
                        .Select(x => new
                        {
                            x.EvaluationId,
                            x.Value
                        })
                        .ToListAsync();

                    var averagesByEvaluations = new List<decimal>();

                    foreach (var evaluation in evaluations)
                    {
                        if (grades.Any(y => y.EvaluationId == evaluation.Id))
                        {
                            averagesByEvaluations.Add(grades.Where(x => x.EvaluationId == evaluation.Id).Select(y => y.Value).FirstOrDefault());
                        }
                        else
                        {
                            averagesByEvaluations.Add(0);
                        }
                    }

                    var susti = await _context.SubstituteExams.Where(x => x.SectionId == sectionStudents[i].SectionId && x.StudentId == sectionStudents[i].StudentId && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED).Select(x => x.ExamScore).FirstOrDefaultAsync();

                    int finalAverage = sectionStudents[i].FinalGrade;

                    if (section.CourseTerm.Term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                    {
                        finalAverage = await _context.AcademicHistories.Where(x => x.SectionId == section.Id && x.StudentId == sectionStudents[i].StudentId).Select(x => x.Grade).FirstOrDefaultAsync();
                    }

                    var row = new Row()
                    {
                        Order = i + 1,
                        Code = sectionStudents[i].Student.User.UserName,
                        Surnames = $"{sectionStudents[i].Student.User.PaternalSurname} {sectionStudents[i].Student.User.MaternalSurname}",
                        Names = sectionStudents[i].Student.User.Name,
                        Try = sectionStudents[i].Try == 0 ? 1 : sectionStudents[i].Try,
                        PartialEvaluationAverages = averagesByEvaluations.ToArray(),
                        FinalEvaluation = finalAverage,
                        FinalEvaluationNumber = finalAverage,
                        FinalEvaluationText = NUMBERS.VALUES[finalAverage],
                        HasSusti = susti.HasValue && susti.Value >= section.CourseTerm.Term.MinGrade && ((susti <= 14 && finalAverage == susti.Value) || (susti > 14 && finalAverage == 14)),
                        StudentStatus = sectionStudents[i].Status
                    };

                    model.Rows.Add(row);
                }
            }

            return PartialView(model);
        }

        /// <summary>
        /// Obtiene el listado de periodos académicos para ser usado en select
        /// </summary>
        /// <returns>Listado de periodos académicos</returns>
        [HttpGet("get-periodos")]
        public async Task<IActionResult> GetTerms()
        {
            var result = await _context.Terms.Where(x => x.Name == "2020-I" || x.Name == "2020-II")
                .Select(x => new
                {
                    x.Id,
                    text = x.Name
                })
                .ToListAsync();

            return Ok(result);
        }


        #region Private Functions 
        private static class NUMBERS
        {
            public static List<string> VALUES = new List<string>()
            {
                "CERO",
                "UNO",
                "DOS",
                "TRES",
                "CUATRO",
                "CINCO",
                "SEIS",
                "SIETE",
                "OCHO",
                "NUEVE",
                "DIEZ",
                "ONCE",
                "DOCE",
                "TRECE",
                "CATORCE",
                "QUINCE",
                "DIECISEIS",
                "DIECISIETE",
                "DIECIOCHO",
                "DIECINUEVE",
                "VEINTE"
            };
        }
        #endregion
    }



}
