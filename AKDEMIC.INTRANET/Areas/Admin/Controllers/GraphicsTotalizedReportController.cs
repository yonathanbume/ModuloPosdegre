using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.TotalizedReportViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.INSTITUTIONAL_WELFARE + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE)]
    [Area("Admin")]
    [Route("admin/reporte-graficas-totales")]
    public class GraphicsTotalizedReportController : BaseController
    {
        public GraphicsTotalizedReportController(AkdemicContext context, UserManager<ApplicationUser> userManager) : base(context, userManager) { }

        /// <summary>
        /// Vista donde se muestra los reportes de gráficos totalizados
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Reporte de cantidad de hombres y mujeres
        /// </summary>
        /// <returns>Objeto que contiene los datos de hombres y mujeres</returns>
        [HttpGet("reporte-hombre-mujer")]
        public IActionResult GetChartReportManWoman()
        {
            var configurationsManWoman = new List<ConfigurationTemplate>();
            foreach (var item in AKDEMIC.CORE.Helpers.ConstantHelpers.SEX.VALUES)
            {
                var Man_WomanType = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationsManWoman.Add(Man_WomanType);
            }

            var groupingManWomanType = configurationsManWoman.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null).Where(x => x.User.Sex == ConfigurationTemplate.Id)).Select(x => new
            {
                Sexdescription = x.Key.DescriptionTemplate,
                Total = x.Value.Count()
            }).OrderBy(x => x.Total).ToArray();
            return Ok(groupingManWomanType);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por preparación universitaria
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-preparacion-universitaria")]
        public IActionResult GetChartReportUniversityPreparation()
        {

            var configurationsUniversityPreparation = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.PERSONAL_INFORMATION.PERSONAL_INFORMATION_UNIVERSITY_PREPARATION.UNIVERSITY_PREPARATION)
            {
                var universityPreparationType = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationsUniversityPreparation.Add(universityPreparationType);
            }
            var groupinguniversityPreparationType = configurationsUniversityPreparation.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null).Where(x => Convert.ToInt32(x.StudentInformation.UniversityPreparation) == ConfigurationTemplate.Id)).Select(x => new
            {
                Universitypreparationname = x.Key.DescriptionTemplate,
                Total = x.Value.Count()
            }).OrderBy(x => x.Total).ToArray();
            return Ok(groupinguniversityPreparationType);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por modalidad de ingreso
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-tipo-modalidad")]
        public async Task<IActionResult> GetChartReportModalityType()
        {

            var result = await _context.AdmissionTypes
               .Select(x => new
               {
                   Admissiontypename = x.Name,
                   Total = x.Students.Where(c => c.StudentInformationId != null).Count()
               }).OrderBy(x => x.Total).ToArrayAsync();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por escuela profesional
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-estudiante-por-carrera")]
        public async Task<IActionResult> GetChartReportStudentCareer()
        {

            var result = await _context.Careers
             .Select(x => new
             {
                 Careername = x.Name,
                 Total = x.Students.Where(c => c.StudentInformationId != null).Count()

             }).OrderBy(x => x.Total).ToArrayAsync();

            return Ok(result);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por rango de edades
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-rango-edades")]
        public IActionResult GetChartReportAgeRanges()
        {

            var configurations = new List<GroupConfiguration>{
                new GroupConfiguration{MinimumAge = 0, MaximumAge=15, Description="Menores de 15"},
                new GroupConfiguration{MinimumAge = 15, MaximumAge=17, Description="De 15 a 17"},
                new GroupConfiguration{MinimumAge = 17, MaximumAge=19, Description="De 17 a 19"},
                new GroupConfiguration{MinimumAge = 19, MaximumAge=21, Description="De 19 a 21"},
                new GroupConfiguration{MinimumAge = 21, MaximumAge=9999999, Description="De 21 a más"}
            };


            var currentYear = DateTime.UtcNow.Year;

            var groupingDictionary = configurations.ToDictionary(GroupConfiguration => GroupConfiguration, GroupConfiguration => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null).Where(x => GroupConfiguration.MinimumAge <= (currentYear - x.User.BirthDate.Year) && (currentYear - x.User.BirthDate.Year) < GroupConfiguration.MaximumAge)).Select(x => new
            {
                Rangeagedescription = x.Key.Description,
                Total = x.Value.Count()
            }).OrderBy(x => x.Total).ToArray();
            return Ok(groupingDictionary);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes según su dependencia económica
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-dependencia-economica")]
        public IActionResult GetChartReportDependencyEconomic()
        {

            var configurationDepedencyEconomic = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_STUDENT_DEPENDENCY.STUDENT_DEPENDENCY)
            {
                var dependency = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationDepedencyEconomic.Add(dependency);
            }

            var groupDependencyEconomic = configurationDepedencyEconomic.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null).Where(x => x.StudentInformation.StudentDependency == ConfigurationTemplate.Id))
                .Select(x => new
                {
                    Dependencyname = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).OrderBy(x => x.Total).ToArray();

            return Ok(groupDependencyEconomic);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por el tipo de escuela
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-tipo-escuela")]
        public IActionResult GetChartReportSchoolType()
        {

            var configurationSchoolType = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.PERSONAL_INFORMATION.PERSONAL_INFORMATION_TYPE_SCHOOL.TYPE_SCHOOL)
            {
                var schoolType = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationSchoolType.Add(schoolType);
            }
            var groupSchoolType = configurationSchoolType.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null).Where(x => x.StudentInformation.SchoolType == ConfigurationTemplate.Id))
                .Select(x => new
                {
                    Schooltypename = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).OrderBy(x => x.Total).ToArray();

            return Ok(groupSchoolType);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por nivel de educación
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-nivel-de-educacion")]
        public IActionResult GetChartReportLevelEducation()
        {

            var configurationLevelEducation = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.PERSONAL_INFORMATION.PERSONAL_INFORMATION_LEVEL_EDUCATION.LEVEL_EDUCATION)
            {
                var levelEducation = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationLevelEducation.Add(levelEducation);
            }

            var groupLevelEducation = configurationLevelEducation.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null))
                .Select(x => new
                {
                    leveleducationname = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).OrderBy(x => x.Total).ToArray();

            return Ok(groupLevelEducation);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por estado civil
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-estado-civil")]
        public IActionResult GetChartCivilStatus()
        {

            var configurationCivilStatus = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.CIVIL_STATUS.VALUES)
            {
                var civilStatus = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationCivilStatus.Add(civilStatus);
            }

            var groupCivilStatus = configurationCivilStatus.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Users.Where(x => x.UserRoles.Any(s => s.Role.Name == ConstantHelpers.ROLES.STUDENTS) && x.CivilStatus == ConfigurationTemplate.Id))
                .Select(x => new
                {
                    Civilstatusname = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).OrderBy(x => x.Total).ToArray();


            return Ok(groupCivilStatus);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por persona a cargo
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-persona-a-cargo")]
        public IActionResult GetChartPrincipalPerson()
        {

            var configurationPrincipalPerson = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_PRINCIPAL_PERSON.PRINCIPAL_PERSON)
            {
                var principalPerson = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationPrincipalPerson.Add(principalPerson);
            }

            var groupPrincipalPerson = configurationPrincipalPerson.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null).Where(x => x.StudentInformation.PrincipalPerson == ConfigurationTemplate.Id))
                .Select(x => new
                {
                    Principalpersondescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).OrderBy(x => x.Total).ToArray();


            return Ok(groupPrincipalPerson);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por remuneración total
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-remuneracion-total")]
        public IActionResult GetChartTotalRemuneration()
        {
            var configurationTotalRemuneration = new List<ConfigureTotalRemuneration>{
                new ConfigureTotalRemuneration{MinimumRemuneration = 0, MaximumRemuneration=950, Description="Menos de 950"},
                new ConfigureTotalRemuneration{MinimumRemuneration = 950, MaximumRemuneration=1500, Description="De 950 a 1500"},
                new ConfigureTotalRemuneration{MinimumRemuneration = 1500, MaximumRemuneration=2000, Description="De 1500 a 2000 "},
                new ConfigureTotalRemuneration{MinimumRemuneration = 2000, MaximumRemuneration=2500, Description="De 2000 a 2500"},
                new ConfigureTotalRemuneration{MinimumRemuneration = 2500, MaximumRemuneration=9999999, Description="De 2500 a más"}
            };


            var groupRemunerationTotal = configurationTotalRemuneration.ToDictionary(ConfigureTotalRemuneration => ConfigureTotalRemuneration,
                ConfigureTotalRemuneration => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null)
                .Where(x => ConfigureTotalRemuneration.MinimumRemuneration <= x.StudentInformation.TotalRemuneration && x.StudentInformation.TotalRemuneration < ConfigureTotalRemuneration.MaximumRemuneration))
                .Select(x => new
                {
                    Totalremunerationdescription = x.Key.Description,
                    Total = x.Value.Count()
                }).OrderBy(x => x.Total).ToArray();

            return Ok(groupRemunerationTotal);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por tipo de convivencia
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-convivencia-de-alumnos")]
        public IActionResult GetChartStudentCoexistence()
        {
            var configurationStudentCoexistence = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_STUDENT_COEXISTENCE.STUDENT_COEXISTENCE)
            {
                var studentCoexistence = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationStudentCoexistence.Add(studentCoexistence);
            }

            var groupStudentCoexistence = configurationStudentCoexistence.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null).Where(x => x.StudentInformation.StudentCoexistence == ConfigurationTemplate.Id))
                .Select(x => new
                {
                    Studentcoexistencedescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).OrderBy(x => x.Total).ToArray();


            return Ok(groupStudentCoexistence);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por riesgo familiar
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-riesgo-familiar")]
        public IActionResult GetChartFamilyRisk()
        {

            var configurationFamilyRisk = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_STUDENT_RISK.STUDENT_RISK)
            {
                var familyRisk = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationFamilyRisk.Add(familyRisk);
            }

            var groupFamilyRisk = configurationFamilyRisk.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null).Where(x => x.StudentInformation.FamilyRisk == ConfigurationTemplate.Id))
                .Select(x => new
                {
                    Familyriskdescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).OrderBy(x => x.Total).ToArray();

            return Ok(groupFamilyRisk);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por tenencia
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-tenencia")]
        public IActionResult GetStudentTenure()
        {

            var configurationTenure = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVING_PLACE_TENURE.TENURE)
            {
                var tenure = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationTenure.Add(tenure);
            }
            var groupTenure = configurationTenure.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null).Where(x => x.StudentInformation.Tenure == ConfigurationTemplate.Id))
                .Select(x => new
                {
                    Tenuredescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).OrderBy(x => x.Total).ToArray();

            return Ok(groupTenure);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por tipo de zona 
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-tipo-zona")]
        public IActionResult GetStudentZoneType()
        {

            var configurationZoneType = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVING_PLACE_TYPE.TYPE)
            {
                var zoneType = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationZoneType.Add(zoneType);
            }
            var groupZoneType = configurationZoneType.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null).Where(x => x.StudentInformation.ZoneType == ConfigurationTemplate.Id))
                .Select(x => new
                {
                    Zonetypedescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).OrderBy(x => x.Total).ToArray();


            return Ok(groupZoneType);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por el tipo de construcción de vivienda
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-tipo-construccion")]
        public IActionResult GetStudentConstructionType()
        {

            var configurationConstructionType = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVINGPLACE_CONSTRUCTION_TYPE.CONSTRUCTION_TYPE)
            {
                var constructionType = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationConstructionType.Add(constructionType);
            }
            var groupConstructionType = configurationConstructionType.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null).Where(x => x.StudentInformation.ContructionType == ConfigurationTemplate.Id))
                .Select(x => new
                {
                    Constructiontypedescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).OrderBy(x => x.Total).ToArray();

            return Ok(groupConstructionType);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por el tipo de acabado de vivienda
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-tipo-acabado")]
        public IActionResult GetStudentBuildType()
        {

            var configurationBuildType = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVINGPLACE_TYPEBUILD.TYPEBUILD)
            {
                var buildType = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationBuildType.Add(buildType);
            }

            var groupBuildType = configurationBuildType.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null).Where(x => x.StudentInformation.BuildType == ConfigurationTemplate.Id))
                .Select(x => new
                {
                    Buildtypedescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).OrderBy(x => x.Total).ToArray();


            return Ok(groupBuildType);

        }

        /// <summary>
        /// Obtiene la cantidad de estudiantes agrupados por la condición de construcción de vivienda
        /// </summary>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte-condicion-de-contruccion")]
        public IActionResult GetStudentConstructionCondition()
        {
            var configurationConstructionCondition = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVINGPLACE_CONSTRUCTION_CONDITION.CONSTRUCTION_CONDITION)
            {
                var constructionCondition = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationConstructionCondition.Add(constructionCondition);
            }

            var groupConstructionCondition = configurationConstructionCondition.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null))
                .Select(x => new
                {
                    Constructionconditiondescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).OrderBy(x => x.Total).ToArray();



            return Ok(groupConstructionCondition);

        }
    }


}
