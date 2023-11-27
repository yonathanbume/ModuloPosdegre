using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.INTRANET.Areas.Student.Models.PerformanceEvaluationVIewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Filters.Permission;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{

    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/evaluacion-docente")]
    //[StudentAuthorizationAttribute(Order = 2)]
    public class PerformanceEvaluationController : BaseController
    {
        private readonly AkdemicContext _context;
        private readonly IPerformanceEvaluationCriterionService _performanceEvaluationCriterionService;
        private readonly IRoleService _roleService;
        private readonly ITermService _termService;
        private readonly IDataTablesService _dataTablesService;
        private readonly IPerformanceEvaluationService _performanceEvaluationService;
        private readonly IPerformanceEvaluationUserService _performanceEvaluationUserService;
        private readonly ISectionService _sectionService;
        private readonly ICourseTermService _courseTermService;
        private readonly ICourseService _courseService;
        private readonly IPerformanceEvaluationRatingScaleService _performanceEvaluationRatingScaleService;
        private readonly IConfigurationService _configurationService;
        private readonly IPerformanceEvaluationTemplateService _performanceEvaluationTemplateService;
        public PerformanceEvaluationController(
            AkdemicContext context,
            IPerformanceEvaluationCriterionService performanceEvaluationCriterionService,
            IRoleService roleService,
            ITermService termService,
            IDataTablesService dataTablesService,
            IPerformanceEvaluationService performanceEvaluationService,
            IPerformanceEvaluationUserService performanceEvaluationUserService,
            ISectionService sectionService,
            ICourseTermService courseTermService,
            ICourseService courseService,
            IUserService userService,
            IPerformanceEvaluationRatingScaleService performanceEvaluationRatingScaleService,
            IConfigurationService configurationService,
            IPerformanceEvaluationTemplateService performanceEvaluationTemplateService) : base(userService, dataTablesService)
        {
            _context = context;
            _performanceEvaluationCriterionService = performanceEvaluationCriterionService;
            _roleService = roleService;
            _termService = termService;
            _dataTablesService = dataTablesService;
            _performanceEvaluationService = performanceEvaluationService;
            _performanceEvaluationUserService = performanceEvaluationUserService;
            _sectionService = sectionService;
            _courseTermService = courseTermService;
            _courseService = courseService;
            _performanceEvaluationRatingScaleService = performanceEvaluationRatingScaleService;
            _configurationService = configurationService;
            _performanceEvaluationTemplateService = performanceEvaluationTemplateService;
        }

        /// <summary>
        /// Vista donde se muestran las evaluaciones docentes habilitadas para el alumno logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fromAccount = false;
            string userId = GetUserId();
            if (await _performanceEvaluationService.OnlyViewStudentPerformanceEvaluation(userId))
                fromAccount = true;

            ViewBag.FromAccount = fromAccount;
            return View();
        }

        /// <summary>
        /// Obtiene el listado de las evaluaciones docentes habilitadas para el alumno logueado
        /// </summary>
        /// <returns>Listado de encuestas</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            string userId = GetUserId();
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _performanceEvaluationService.GetTeachersByStudentDatatable(sentParameters, userId);

            if (!string.IsNullOrEmpty(result.Error))
                return Ok(result.Error);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene información adicional del evaluado
        /// </summary>
        /// <param name="id">Identificador del docente</param>
        /// <returns>Información del evaluado</returns>
        [HttpGet("encuesta/info/{id}")]
        public async Task<IActionResult> EvaluatorInfo(string id)
        {
            object result = await _performanceEvaluationService.GetEvaluatorInfo(id, ConstantHelpers.ROLES.TEACHERS);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene la cantidad de evaluación docente pendientes
        /// </summary>
        /// <returns>Cantidad de evaluación docente pendientes</returns>
        [HttpGet("get-cantidad-encuestas-pendientes")]
        public async Task<IActionResult> GetPendingSurveys()
        {
            string userId = GetUserId();
            var result = await _performanceEvaluationService.GetPendingSurveys(userId);
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se responderá la evaluación docente
        /// </summary>
        /// <param name="id">Identificador del docente a calificar</param>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Vista</returns>
        [HttpGet("{id}/encuesta/{sectionId}/seccion")]
        public async Task<IActionResult> Survey(string id, Guid sectionId)
        {
            var fromAccount = false;
            string userId = GetUserId();
            if (await _performanceEvaluationService.OnlyViewStudentPerformanceEvaluation(userId))
                fromAccount = true;

            ViewBag.FromAccount = fromAccount;

            PerformanceEvaluationTemplate result = await _performanceEvaluationTemplateService.GetActiveTemplateByRole(ConstantHelpers.ROLES.STUDENTS);
            var section = await _sectionService.Get(sectionId);
            var course = await _courseService.GetCourseBySectionId(sectionId);
            ViewBag.Id = id;

            var raitingScales = await _performanceEvaluationRatingScaleService.GetRaitingScaleByMax(result.Max);

            PerformanceEvaluationTemplateViewModel template = new PerformanceEvaluationTemplateViewModel()
            {
                ToId = id,
                FromId = userId,
                Max = result.Max,
                TemplateId = result.Id,
                SectionId = sectionId,
                Section = section.Code,
                Course = course.Name,
                Target = result.Target,
                Instructions = result.Instructions,
                Questions = result.Questions.Select(x => new PerformanceEvaluationQuestionViewModel
                {
                    CreatedAt = x.CreatedAt,
                    Id = null,
                    Value = 0,
                    QuestionId = x.Id,
                    Description = x.Description,
                    CriterionId = x.PerformanceEvaluationCriterionId
                }).ToList(),
            };

            if (raitingScales != null && raitingScales.Any())
            {
                template.Scales = raitingScales.Select(x => new PerformanceEvaluationScaleViewModel
                {
                    Description = x.Description,
                    Value = x.Value
                }).ToList();
            }

            if (result.EnabledCriterions)
            {
                var criterions = await _performanceEvaluationCriterionService.GetCriterions(result.Id);

                if (criterions.Any())
                {
                    template.Criterions = criterions.Select(y => new PerformanceEvaluationCriterionViewModel
                    {
                        CreatedAt = y.CreatedAt,
                        Id = y.Id,
                        Name = y.Name
                    }).ToList();
                }
            }

            var role = await _roleService.GetByName(ConstantHelpers.ROLES.STUDENTS);

            if (await _performanceEvaluationUserService.ValidatePerformanceEvaluationUser(role.Id, result.Id, userId, id, sectionId))
            {
                InfoToastMessage("Ya se ha realizado la evaluación al docente.", "Información");
                return RedirectToAction("Index");
            }

            return View(template);
        }

        /// <summary>
        /// Método para enviar la encuesta de evaluación docente
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de evaluación docente</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("encuesta/enviar")]
        public async Task<IActionResult> SendEvaluation(PerformanceEvaluationTemplateViewModel model)
        {
            var role = await _roleService.GetByName(ConstantHelpers.ROLES.STUDENTS);

            if (await _performanceEvaluationUserService.ValidatePerformanceEvaluationUser(role.Id, model.TemplateId, model.FromId, model.ToId, model.SectionId))
            {
                return BadRequest("Ya se ha realizado la evaluación al docente.");
            }

            if (model.Questions.Any(y => y.Value == 0))
                return BadRequest("Debes responder todas las preguntas.");

            List<PerformanceEvaluationResponse> responses = new List<PerformanceEvaluationResponse>();

            decimal value = 0;

            for (byte i = 0; i < model.Questions.Count; i++)
            {
                PerformanceEvaluationResponse response = new PerformanceEvaluationResponse()
                {
                    PerformanceEvaluationQuestionId = model.Questions[i].QuestionId,
                    Value = model.Questions[i].Value
                };
                value += model.Questions[i].Value;
                responses.Add(response);
            }

            if (value != 0)
            {
                value /= model.Questions.Count;
            }

            var term = await _termService.GetActiveTerm();
            var evaluation = await _performanceEvaluationService.GetPerformanceEvaluationInCourseByTerm();

            PerformanceEvaluationUser performanceEvaluationUser = new PerformanceEvaluationUser()
            {
                Value = value,
                TermId = term.Id,
                PerformanceEvaluationId = evaluation.Id,
                FromRoleId = role.Id,
                ToTeacherId = model.ToId,
                Responses = responses,
                FromUserId = model.FromId,
                DateTime = DateTime.UtcNow,
                PerformanceEvaluationTemplateId = model.TemplateId,
                SectionId = model.SectionId,
                Commentary = model.Commentary
            };

            await _performanceEvaluationUserService.Insert(performanceEvaluationUser);

            //var configuration = await _configurationService.GetByKey(ConstantHelpers.Configuration.TeacherManagement.PERFORMANCE_EVALUATION_REQUIRED);

            //if(configuration is null)
            //{
            //    configuration = new ENTITIES.Models.Configuration
            //    {
            //        Key = ConstantHelpers.Configuration.TeacherManagement.PERFORMANCE_EVALUATION_REQUIRED,
            //        Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.PERFORMANCE_EVALUATION_REQUIRED]
            //    };
            //}

            //var required = Convert.ToBoolean(configuration.Value);
            //if (required)
            //{
            //    var termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE && !x.IsSummer).Select(x => x.Id).FirstOrDefaultAsync();
            //    var student = await _context.Students.Where(x => x.UserId == model.FromId).FirstOrDefaultAsync();
            //    var evaluation = await _context.PerformanceEvaluations.Where(x => x.TermId == termId).FirstOrDefaultAsync();
            //    if (evaluation != null)
            //    {
            //        var evaluations = await _context.PerformanceEvaluationUsers.Where(x => x.FromUserId == student.UserId && x.TermId == termId && x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS).CountAsync();
            //        var sections = await _context.StudentSections.Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == termId).Select(x => x.SectionId).ToListAsync();
            //        var teacherSection = await _context.TeacherSections.Where(x => sections.Contains(x.SectionId)).CountAsync();
            //        if (teacherSection > evaluations)
            //            return Ok("/alumno/evaluacion-docente/true");
            //    }
            //}
            return Ok("/alumno/evaluacion-docente");
        }
    }
}
