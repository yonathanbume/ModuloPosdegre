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

namespace AKDEMIC.DEGREE.Areas.Admin
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," + ConstantHelpers.ROLES.REPORT_QUERIES)]
    [Area("Admin")]
    [Route("admin/reporte-alumnos-bachilleres-y-titulados")]
    public class ReportBachelorController : BaseController
    {
        private readonly IFacultyService _facultyService;
        private readonly IStudentService _studentService;
        private readonly IAcademicProgramService _academicProgramService;

        public ReportBachelorController(IFacultyService facultyService,
            IStudentService studentService,
            ICareerService careerService,
            IAcademicProgramService academicProgramService,
            UserManager<ApplicationUser> userManager
            ) : base(careerService, userManager)
        {
            _facultyService = facultyService;
            _studentService = studentService;
            _academicProgramService = academicProgramService;
        }

        /// <summary>
        /// Vista donde se muestra la cantidad de número de bachilleres y titulados
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
        /// Obtiene la cantidad de bachilleres y titulados por escuela profesional
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la escuela profesional, facultades y tipo para generar el reporte</param>
        /// <returns>Retorna un Ok con la información para su uso en reportes gráficos</returns>
        [HttpPost("reporte-cantidad")]
        public async Task<IActionResult> GetBachelors(DataViewModel model)
        {
            var studensBachelors = await _studentService.GetBachelors(model.LstFaculties, model.LstCareers, model.LstPrograms, model.GradeType);
            return Ok(studensBachelors);
        }

        /// <summary>
        /// Obtiene el listado de facultades
        /// </summary>
        /// <returns>Retorna un OK con la información de las facultades para ser usado en select</returns>
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
        /// <param name="model">Objeto que contiene los datos de facultades para el filtrado</param>
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
        /// <param name="model">Objeto que contiene los datos de las escuelas profesionales para el filtrado</param>
        /// <returns>Retorna un Ok con la información de los programas académicos para ser usado en select</returns>
        [HttpPost("programas-academicos")]
        public async Task<IActionResult> GetAcademicProgrmas(SelectDataViewModel model)
        {
            var result = await _academicProgramService.GetAcademicProgramsSelect(new Guid(), false, model.Select2Data);
            return Ok(result);
        }

    }
}
