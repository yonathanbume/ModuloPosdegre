using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Areas.Admin.Models.TotalizedReportViewModels;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.INSTITUTIONAL_WELFARE + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE)]
    [Area("Admin")]
    [Route("admin/reportes_totales")]
    public class TotalizedReportController : BaseController
    {
        public TotalizedReportController(AkdemicContext context) : base(context) { }

        /// <summary>
        /// Vista donde se muestran los reportes totalizados
        /// - Hombres vs Mujeres
        /// - Estudiantes por preparación universitaria
        /// - Estudiantes por modalidad de ingreso
        /// - Estudiantes por escuela profesional
        /// - Estudiantes por rango de edades
        /// - Estudiantes por dependencia económica
        /// - Estudiantes por rango de edades
        /// - Estudiantes por dependencia económica
        /// - Estudiantes por tipo de colegio de procedencia
        /// - Estudiantes según estado civil
        /// - Estudiantes según quien sostiene el hogar
        /// - Estudiantes por ingreso familiar
        /// - Estudiantes por tipo de convivencia
        /// - Estudiantes por riesgo familiar
        /// - Estudiantes por tipo de tenencia de vivienda
        /// - Estudiantes por tipo de vivienda
        /// - Estudiantes por tipo de construcción de vivienda
        /// - Estudiantes por estructura de vivienda.
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            var lstStudents = _context.Students.Include(x => x.User).Where(x => x.StudentInformationId != null).AsNoTracking();

            //var lstUsers = _context.Users.Where(x => x.UserRoles.Any(s => s.Role.Name == ConstantHelpers.ROLES.STUDENTS)).AsQueryable();

            var configurationsUniversityPreparation = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.PERSONAL_INFORMATION.PERSONAL_INFORMATION_UNIVERSITY_PREPARATION.UNIVERSITY_PREPARATION)
            {
                var universityPreparationType = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationsUniversityPreparation.Add(universityPreparationType);
            }

            //var Result_Totalized_Student_UniversityPreparation = new List<TotalizedStudentUniversityPreparationViewModel>();
            var Result_Totalized_Student_UniversityPreparation = configurationsUniversityPreparation.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => lstStudents.Where(x => Convert.ToInt32(x.StudentInformation.UniversityPreparation) == ConfigurationTemplate.Id))
                .Select(x => new TotalizedStudentUniversityPreparationViewModel
                {
                    UniversityPreparationName = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).ToList();



            var Result_Totalized_Man_Woman = await lstStudents.GroupBy(x => new { x.User.Sex })
                .Select(x => new TotalizedManWomanViewModel
                {
                    Concept = x.Key.Sex == CORE.Helpers.ConstantHelpers.SEX.MALE ? "Hombres" : "Mujeres",
                    Total = x.Count()
                }).ToListAsync();




            var Result_Totalized_Student_Admission = await _context.AdmissionTypes
                .Select(x => new TotalizedStudentAdmissionTypeViewModel
                {
                    AmissionTypeName = x.Name,
                    Total = x.Students.Where(c => c.StudentInformationId != null).Count()
                }).ToListAsync();

            var Result_Totalized_Student_Career = await _context.Careers
                .Select(x => new TotalizedStudentCareerViewModel
                {
                    CareerName = x.Name,
                    Total = x.Students.Where(c => c.StudentInformationId != null).Count()

                }).ToListAsync();



            var configurations = new List<GroupConfiguration>{
                new GroupConfiguration{MinimumAge = 0, MaximumAge=15, Description="Menores de 15"},
                new GroupConfiguration{MinimumAge = 15, MaximumAge=17, Description="De 15 a 17"},
                new GroupConfiguration{MinimumAge = 17, MaximumAge=19, Description="De 17 a 19"},
                new GroupConfiguration{MinimumAge = 19, MaximumAge=21, Description="De 19 a 21"},
                new GroupConfiguration{MinimumAge = 21, MaximumAge=9999999, Description="De 21 a más"}
            };


            var CurrentYear = DateTime.UtcNow.Year;

            var Result_Totalized_Student_Range = configurations.ToDictionary(GroupConfiguration => GroupConfiguration, GroupConfiguration => lstStudents.Where(x => GroupConfiguration.MinimumAge <= (CurrentYear - x.User.BirthDate.Year) && (CurrentYear - x.User.BirthDate.Year) < GroupConfiguration.MaximumAge))
                .Select(x => new TotalizedStudentRangeAge
                {
                    RangeAgeDescription = x.Key.Description,
                    Total = x.Value.Count()
                }).ToList();




            var configurationDepedencyEconomic = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_STUDENT_DEPENDENCY.STUDENT_DEPENDENCY)
            {
                var dependency = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationDepedencyEconomic.Add(dependency);
            }

            var Result_Totalized_Student_DependencyEconomic = configurationDepedencyEconomic.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => lstStudents.Where(x => x.StudentInformation.StudentDependency == ConfigurationTemplate.Id))
                .Select(x => new TotalizedStudentDependencyEconomicViewModel
                {
                    DependencyName = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).ToList();



            var configurationLevelEducation = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.PERSONAL_INFORMATION.PERSONAL_INFORMATION_LEVEL_EDUCATION.LEVEL_EDUCATION)
            {
                var levelEducation = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationLevelEducation.Add(levelEducation);
            }

            //var Result_Totalized_Student_LevelEducation = configurationLevelEducation.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => lstStudents.Where(x => x.StudentInformation.EducationLevel == ConfigurationTemplate.Id))
            //    .Select(x => new TotalizedStudentLevelEducationViewModel {
            //        LevelEducationName = x.Key.DescriptionTemplate,
            //        Total = x.Value.Count()
            //    }).ToList();





            var configurationSchoolType = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.PERSONAL_INFORMATION.PERSONAL_INFORMATION_TYPE_SCHOOL.TYPE_SCHOOL)
            {
                var schoolType = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationSchoolType.Add(schoolType);
            }

            var Result_Totalized_Student_SchoolType = configurationSchoolType.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => lstStudents.Where(x => x.StudentInformation.SchoolType == ConfigurationTemplate.Id))
                .Select(x => new TotalizedStudentSchoolTypeViewModel
                {
                    SchoolTypeName = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).ToList();




            var configurationCivilStatus = new List<ConfigurationTemplate2>();
            foreach (var item in ConstantHelpers.CIVIL_STATUS.VALUES)
            {
                var civilStatus = new ConfigurationTemplate2 { DescriptionTemplate2 = item.Value, Id = item.Key };
                configurationCivilStatus.Add(civilStatus);
            }

            var Result_Totalized_Student_CivilStatus = configurationCivilStatus
                .ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate =>
                lstStudents.Where(x => x.User.CivilStatus == ConfigurationTemplate.Id))
                .Select(x => new TotalizedStudentCivilStatusViewModel
                {
                    CivilStatusName = x.Key.DescriptionTemplate2,
                    Total = x.Value.Count()
                }).ToList();








            var configurationPrincipalPerson = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_PRINCIPAL_PERSON.PRINCIPAL_PERSON)
            {
                var principalPerson = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationPrincipalPerson.Add(principalPerson);
            }

            var Result_Totalized_Student_Principal_Person = configurationPrincipalPerson.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationPrincipalPerson => lstStudents.Where(x => x.StudentInformation.PrincipalPerson == ConfigurationPrincipalPerson.Id))
                .Select(x => new TotalizedStudentPrincipalPersonViewModel
                {
                    PrincipalPersonDescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).ToList();




            var configurationStudentCoexistence = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_STUDENT_COEXISTENCE.STUDENT_COEXISTENCE)
            {
                var studentCoexistence = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationStudentCoexistence.Add(studentCoexistence);
            }

            var Result_Totalized_Student_Coexistence = configurationStudentCoexistence.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationStudentCoexistence => lstStudents.Where(x => x.StudentInformation.StudentCoexistence == ConfigurationStudentCoexistence.Id))
                .Select(x => new TotalizedStudentCoexistenceViewModel
                {
                    StudentCoexistenceDescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).ToList();




            var configurationFamilyRisk = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_STUDENT_RISK.STUDENT_RISK)
            {
                var familyRisk = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationFamilyRisk.Add(familyRisk);
            }

            var Result_Totalized_Student_Family_Risk = configurationFamilyRisk.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => lstStudents.Where(x => x.StudentInformation.FamilyRisk == ConfigurationTemplate.Id))
                .Select(x => new TotalizedStudentFamilyRiskViewModel
                {
                    FamiliRiskDescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).ToList();



            var configurationConstructionType = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVINGPLACE_CONSTRUCTION_TYPE.CONSTRUCTION_TYPE)
            {
                var constructionType = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationConstructionType.Add(constructionType);
            }
            var Result_Totalized_Student_Construction_Type = configurationConstructionType.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => lstStudents.Where(x => x.StudentInformation.ContructionType == ConfigurationTemplate.Id))
    .Select(x => new TotalizedStudentConstructionTypeViewModel
    {
        ConstructionTypeDescription = x.Key.DescriptionTemplate,
        Total = x.Value.Count()
    }).ToList();





            var configurationTenure = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVING_PLACE_TENURE.TENURE)
            {
                var tenure = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationTenure.Add(tenure);
            }
            var Result_Totalized_Student_Tenure = configurationTenure.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => lstStudents.Where(x => x.StudentInformation.Tenure == ConfigurationTemplate.Id))
                .Select(x => new TotalizedStudentTenureViewModel
                {

                    TenureDescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()

                }).ToList();





            var configurationZoneType = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVING_PLACE_TYPE.TYPE)
            {
                var zoneType = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationZoneType.Add(zoneType);
            }

            var Result_Totalized_Student_ZoneType = configurationZoneType.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => lstStudents.Where(x => x.StudentInformation.ZoneType == ConfigurationTemplate.Id))
                .Select(x => new TotalizedStudenZoneTypeViewModel
                {
                    ZoneTypeDescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()
                }).ToList();








            var configurationBuildType = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVINGPLACE_TYPEBUILD.TYPEBUILD)
            {
                var buildType = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationBuildType.Add(buildType);
            }

            var Result_Totalized_Student_BuildType = configurationBuildType.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => lstStudents.Where(x => x.StudentInformation.BuildType == ConfigurationTemplate.Id))
                .Select(x => new TotalizedStudenBuildTypeViewModel
                {

                    BuildTypeDescription = x.Key.DescriptionTemplate,
                    Total = x.Value.Count()

                }).ToList();


            var configurationConstructionCondition = new List<ConfigurationTemplate>();
            foreach (var item in ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVINGPLACE_CONSTRUCTION_CONDITION.CONSTRUCTION_CONDITION)
            {
                var constructionCondition = new ConfigurationTemplate { DescriptionTemplate = item.Value, Id = item.Key };
                configurationConstructionCondition.Add(constructionCondition);
            }
            //var Result_Totalized_Student_ConstructionCondition = configurationConstructionCondition.ToDictionary(ConfigurationTemplate => ConfigurationTemplate, ConfigurationTemplate => lstStudents.Where(x => x.StudentInformation.ContructionCondition == ConfigurationTemplate.Id))
            //    .Select(x=> new TotalizedStudentConstructionConditionViewModel {
            //        ConstructionConditionDescription = x.Key.DescriptionTemplate,
            //        Total = x.Value.Count()
            //    }).ToList();



            var configurationTotalRemuneration = new List<ConfigureTotalRemuneration>{
                new ConfigureTotalRemuneration{MinimumRemuneration = 0, MaximumRemuneration=950, Description="Menos de 950"},
                new ConfigureTotalRemuneration{MinimumRemuneration = 950, MaximumRemuneration=1500, Description="De 950 a 1500"},
                new ConfigureTotalRemuneration{MinimumRemuneration = 1500, MaximumRemuneration=2000, Description="De 1500 a 2000 "},
                new ConfigureTotalRemuneration{MinimumRemuneration = 2000, MaximumRemuneration=2500, Description="De 2000 a 2500"},
                new ConfigureTotalRemuneration{MinimumRemuneration = 2500, MaximumRemuneration=9999999, Description="De 2500 a más"}
            };




            var Result_Totalized_Student_TotalRemuneration = configurationTotalRemuneration.ToDictionary(ConfigureTotalRemuneration => ConfigureTotalRemuneration, ConfigureTotalRemuneration => lstStudents.Where(x => ConfigureTotalRemuneration.MinimumRemuneration <= x.StudentInformation.TotalRemuneration && x.StudentInformation.TotalRemuneration < ConfigureTotalRemuneration.MaximumRemuneration))
                .Select(x => new TotalizedStudentTotalRemunerationViewModel
                {
                    TotalRemunerationDescription = x.Key.Description,
                    Total = x.Value.Count()
                }).ToList();




            var Result = new TotalizedReportViewModel
            {
                LstResult_Totalized_Man_Woman = Result_Totalized_Man_Woman,
                LstResult_Totalized_Student_Admission = Result_Totalized_Student_Admission,
                LstResult_Totalized_Student_Career = Result_Totalized_Student_Career,
                //LstResult_Totalized_Student_LevelEducation = Result_Totalized_Student_LevelEducation,
                LstResult_Totalized_Student_SchoolType = Result_Totalized_Student_SchoolType,
                LstResult_Totalized_Student_UniversityPreparation = Result_Totalized_Student_UniversityPreparation,
                LstResult_Totalized_Student_Range = Result_Totalized_Student_Range,
                LstResult_Totalized_Student_Dependency_Economic = Result_Totalized_Student_DependencyEconomic,
                LstResult_Totalized_Student_Civil_Status = Result_Totalized_Student_CivilStatus,
                LstResult_Totalized_Student_Principal_Person = Result_Totalized_Student_Principal_Person,
                LstResult_Totalized_Student_Coexistence = Result_Totalized_Student_Coexistence,
                LstResult_Totalized_Student_Family_Risk = Result_Totalized_Student_Family_Risk,
                LstResult_Totalized_Student_Tenure = Result_Totalized_Student_Tenure,
                LstResult_Totalized_Student_Construction_Type = Result_Totalized_Student_Construction_Type,
                LstResult_Totalized_Student_ZoneType = Result_Totalized_Student_ZoneType,
                LstResult_Totalized_Student_BuildType = Result_Totalized_Student_BuildType,
                //LstResult_Totalized_Student_Construction_Condition = Result_Totalized_Student_ConstructionCondition,
                LstResult_Totalized_Student_RemunerationTotal = Result_Totalized_Student_TotalRemuneration
            };

            return View(Result);
        }
    }
}
