using AKDEMIC.CORE.Helpers;
using AKDEMIC.DEGREE.Areas.Admin.Models.ReportBachelorViewModels;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.DEGREE.ViewModels.DataViewModels.Select2ViewModel;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," + ConstantHelpers.ROLES.REPORT_QUERIES)]
    [Area("Admin")]
    [Route("admin/reporte-alumnos-egresados-sin-solicitud-bachiller")]
    public class ReportBachelorsWithOutTUPAController : BaseController
    {
        private readonly IFacultyService _facultyService;
        private readonly IStudentService _studentService;
        private readonly IAcademicProgramService _academicProgramService;
        public ReportBachelorsWithOutTUPAController(IConfigurationService configurationService,
             ICareerService careerService, IFacultyService facultyService,
             IStudentService studentService,
             IAcademicProgramService academicProgramService,
             UserManager<ApplicationUser> userManager) : base(careerService, configurationService, userManager)
        {
            _facultyService = facultyService;
            _studentService = studentService;
            _academicProgramService = academicProgramService;
        }

        /// <summary>
        /// Vista principal donde se muestra el reporte de trámites realizados vs trámites pendientes
        /// </summary>
        /// <returns>Retorna la vista</returns>
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(ConstantHelpers.ROLES.SUPERADMIN) || User.IsInRole(ConstantHelpers.ROLES.DEGREE_ADMIN))
            {
                ViewBag.CareerId = new Guid();
                ViewBag.FacultyId = new Guid();
            }
            else
            {
                var tupleValues = await GetCareersAndFacultiesByAcademicCoordinator();
                var career = tupleValues.Item1.Select(x => x.Id).ToList();
                var faculty = tupleValues.Item2.Select(x => x.Id).ToList();
                ViewBag.CareerList = career;
                ViewBag.FacultyList = faculty;
            }

            return View();

        }

        /// <summary>
        /// Obtiene el porncetaje de trámites realizados vs trámites pendientes
        /// </summary>
        /// <param name="model">Objeto que contiene los datos para filtrar (Facultades, escuelas profesionales y programas académicos)</param>
        /// <returns>Retorna un Ok con lo datos para su uso en gráficos</returns>
        [HttpPost("reporte-cantidad-bachilleres_sin_tramite_solicitado")]
        public async Task<IActionResult> GetBachelors(DataViewModel model)
        {
            //int studentsBachelorsNo, studentsBachelorsYes = 0;
            var result = await _studentService.GetBachelorsWithOutConfiguration(model.LstFaculties, model.LstCareers, model.LstPrograms);
            return Ok(result);

        }

        /// <summary>
        /// Obtiene el listado de facultades 
        /// </summary>
        /// <returns>Retorna un Ok con la información de las facultades para ser usado en select</returns>
        [HttpGet("facultades/get")]
        public async Task<IActionResult> GetFaculties()
        {
            if (User.IsInRole(ConstantHelpers.ROLES.SUPERADMIN) || User.IsInRole(ConstantHelpers.ROLES.DEGREE_ADMIN))
            {
                var result = await _facultyService.GetFacultiesSelect2ClientSide();
                return Ok(new { items = result });
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);

                var result = await _facultyService.GetFacultiesJson(null, User);
                return Ok(new { items = result });
            }
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales
        /// </summary>
        /// <param name="model">Objeto que contiene los identificadores de las facultades para su filtrado</param>
        /// <returns>Retorna un Ok con la información de las escuelas profesionales para ser usado en select</returns>
        [HttpPost("carreras")]
        public async Task<IActionResult> GetCareersByFaculties(SelectDataViewModel model)
        {
            var result = await _careerService.GetCareerSelect2Curriculum(new Guid(), model.Select2Data);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de programas académicos 
        /// </summary>
        /// <param name="model">Objeto que contiene los identificadores de las escuelas profesionales</param>
        /// <returns>Retorna un Ok con la información de los programas académicos</returns>
        [HttpPost("programas-academicos")]
        public async Task<IActionResult> GetAcademicProgrmas(SelectDataViewModel model)
        {
            var result = await _academicProgramService.GetAcademicProgramsSelect(new Guid(), false, model.Select2Data);
            return Ok(result);
        }
    }
}
