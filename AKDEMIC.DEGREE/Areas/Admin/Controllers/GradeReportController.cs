using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.DEGREE.Areas.Admin.Models.GradeReportViewModels;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)]
    [Area("Admin")]
    [Route("coordinador-academico/informes-de-grados")]
    public class GradeReportController : BaseController
    {

        private readonly IDataTablesService _dataTablesService;
        private readonly IStudentService _studentService;
        private readonly IGradeReportService _gradeReportService;
        private readonly IRegistryPatternService _registryPatternService;
        public GradeReportController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            IDataTablesService dataTablesService,
            IRegistryPatternService registryPatternService,
            IStudentService studentService,
            ICareerService careerService,
            IGradeReportService gradeReportService,
            IConfigurationService configurationService) : base(careerService, configurationService, userManager)
        {
            _dataTablesService = dataTablesService;
            _gradeReportService = gradeReportService;
            _registryPatternService = registryPatternService;
            _studentService = studentService;

        }

        /// <summary>
        /// Vista Principal donde se lista los informes de grados generados
        /// </summary>
        /// <returns>Retorna Vista</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de grados 
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Retorna un OK con los datos para ser usado en tablas</returns>
        [HttpGet("obtener-informes-grados")]
        public async Task<IActionResult> GetGradeReportDatatable(string searchValue)
        {
            var tupleValues = await GetCareersAndFacultiesByAcademicCoordinator();
            var career = tupleValues.Item1.Select(x => x.Id).FirstOrDefault();
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _gradeReportService.GetGradeReportDatatable(sentParameters, career, 0, searchValue);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de variables de configuración en base a que si el sistema esta integrado o no.
        /// </summary>
        /// <returns>Retorna un Ok</returns>
        [HttpGet("obtener-modalidades-grados")]
        public async Task<IActionResult> ModalitySelect()
        {
            var isIntegrated = await _configurationService.GetConfigurationByGENIntegratedSystem();
            var result = await _configurationService.ConfigurationGradeModality(Convert.ToBoolean(isIntegrated.Value), 0);
            return Ok(result);
        }

        /// <summary>
        /// ObtienE el detalle del informe de grado
        /// </summary>
        /// <param name="gradereportId">Identificado del informe de grado</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("detalle/{gradereportId}")]
        public async Task<IActionResult> Detail(Guid gradereportId)
        {
            var isIntegrated = await _configurationService.GetConfigurationByGENIntegratedSystem();
            var result = await _gradeReportService.GetGradeReportWithIncludes(gradereportId);
            var detail = new GradeReportDetailViewModel
            {
                AdmissionTermId = result.Student.AdmissionTermId,
                CareerName = result.Student.Career.Name,
                CurricularSystem = ConstantHelpers.CURRICULUM.STUDY_REGIME.VALUES[result.Student.Curriculum.StudyRegime],
                Curriculum = result.Student.Curriculum.Name,
                Date = result.Date.ToLocalDateFormat(),
                FacultyName = result.Student.Career.Faculty.Name,
                FullName = result.Student.User.FullName,
                Name = result.Student.User.Name,
                GraduationTermId = result.Student.GraduationTermId.HasValue ? result.Student.GraduationTermId.Value : Guid.Empty,
                IsIntegrated = bool.Parse(isIntegrated.Value),
                MaternalSurname = result.Student.User.MaternalSurname,
                Number = result.Number,
                Observation = result.Observation,
                PaternalSurname = result.Student.User.PaternalSurname,
                PromotionGrade = result.PromotionGrade,
                SemesterStudied = result.SemesterStudied,
                UserName = result.Student.User.UserName,
                YearsStudied = result.YearsStudied,
                Year = result.Year
            };
            if (result.Student.AcademicProgram != null)
            {
                detail.AcademicProgram = result.Student.AcademicProgram.Name;
            }
            if (result.ConceptId.HasValue)
            {
                detail.ConceptId = result.ConceptId;
            }
            if (result.ProcedureId.HasValue)
            {
                detail.ProcedureId = result.ProcedureId;
            }

            return View(detail);
        }
    }
}
