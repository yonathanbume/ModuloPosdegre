using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class ConfigurationRepository : Repository<Configuration>, IConfigurationRepository
    {
        public ConfigurationRepository(AkdemicContext context) : base(context) { }

        public async Task<Configuration> GetByKey(string key) => await _context.Configurations.FirstOrDefaultAsync(x => x.Key == key);

        public async Task<Dictionary<string, string>> GetDataDictionary() => await _context.Configurations.ToDictionaryAsync(x => x.Key, x => x.Value);

        public async Task<string> GetValueByKey(string key)
        {
            var value = await GetConfigurationValue(key);
            return value;
        }

        public async Task UpdateConfigurationValue(string key, string value)
        {
            if (value != null)
            {
                var configuration = await _context.Configurations.FirstOrDefaultAsync(x => x.Key == key);

                if (configuration != null)
                {
                    configuration.Value = value;
                }
                else
                {
                    configuration = new Configuration
                    {
                        Key = key,
                        Value = value
                    };
                    await _context.Configurations.AddAsync(configuration);
                }

                await _context.SaveChangesAsync();
            }
        }                

        public async Task<Configuration> FirstOrDefaultByKey(string key)
        {
            var query = _context.Configurations.Where(x => x.Key == key).AsQueryable();
            return await query.FirstOrDefaultAsync();
        }

        public async Task<ConfigurationTemplate> GetConfigurationTemplateAsync()
        {
            var template = new ConfigurationTemplate();
            var configurationSystemIntegrated = Boolean.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM));
            template.StaticTypeSystemIntegrated = configurationSystemIntegrated;

            if (configurationSystemIntegrated)
            {
                template.TupaStaticTypeBachelor = byte.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.TUPA_TYPE_BACHELOR));
                switch (template.TupaStaticTypeBachelor)
                {
                    case ConstantHelpers.Configuration.BachelorTypeConfiguration.TUPA.AUTOMATIC:
                        template.TupaBachelorAutomatic = await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.TUPA_BACHELOR);
                        break;
                    case ConstantHelpers.Configuration.BachelorTypeConfiguration.TUPA.BY_REQUEST:
                        template.TupaBachelorRequested = await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.TUPA_BACHELOR);
                        break;
                }
                template.TupaTitleDegreeProfessionalExperience = await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE);
                template.TupaTitleDegreeSufficiencyExam = await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM);
                template.TupaTitleDegreeSupportTesis = await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUPPORT_TESIS);
                template.MethodTypeRegistryPatternCreation = Boolean.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.METHOD_TYPE_REGISTRY_PATTERN_CREATION));
            }
            else
            {
                template.ConceptStaticTypeBachelor = byte.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.CONCEPT_TYPE_BACHELOR));

                switch (template.ConceptStaticTypeBachelor)
                {
                    case ConstantHelpers.Configuration.BachelorTypeConfiguration.CONCEPT.AUTOMATIC:
                        template.ConceptBachelorAutomatic = await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.CONCEPT_BACHELOR);
                        break;
                    case ConstantHelpers.Configuration.BachelorTypeConfiguration.CONCEPT.BY_REQUEST:
                        template.ConceptBachelorRequested = await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.CONCEPT_BACHELOR);
                        break;
                }
                template.ConceptTitleDegreeProfessionalExperience = await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE);
                template.ConceptTitleDegreeSufficiencyExam = await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM);
                template.ConceptTitleDegreeSupportTesis = await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUPPORT_TESIS);
                template.MethodTypeRegistryPatternCreation = Boolean.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.METHOD_TYPE_REGISTRY_PATTERN_CREATION));
            }
            return template;
        }

        public async Task UpdateConfiguration(ConfigurationTemplate configurationTemplate)
        {
            await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.METHOD_TYPE_REGISTRY_PATTERN_CREATION, configurationTemplate.MethodTypeRegistryPatternCreation.ToString());
            var configurationSystemIntegrated = Boolean.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM));
            if (configurationSystemIntegrated)
            {
                await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.TUPA_TYPE_BACHELOR, configurationTemplate.TupaStaticTypeBachelor.ToString());
                switch (configurationTemplate.TupaStaticTypeBachelor)
                {
                    case ConstantHelpers.Configuration.BachelorTypeConfiguration.TUPA.AUTOMATIC:
                        await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.TUPA_BACHELOR, configurationTemplate.TupaBachelorAutomatic);
                        break;
                    case ConstantHelpers.Configuration.BachelorTypeConfiguration.TUPA.BY_REQUEST:
                        await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.TUPA_BACHELOR, configurationTemplate.TupaBachelorRequested);
                        break;
                }
                await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE, configurationTemplate.TupaTitleDegreeProfessionalExperience);
                await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM, configurationTemplate.TupaTitleDegreeSufficiencyExam);
                await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUPPORT_TESIS, configurationTemplate.TupaTitleDegreeSupportTesis);
                await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.METHOD_TYPE_REGISTRY_PATTERN_CREATION, configurationTemplate.MethodTypeRegistryPatternCreation.ToString());

                await _context.SaveChangesAsync();

            }
            else
            {
                await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.CONCEPT_TYPE_BACHELOR, configurationTemplate.ConceptStaticTypeBachelor.ToString());

                switch (configurationTemplate.ConceptStaticTypeBachelor)
                {
                    case ConstantHelpers.Configuration.BachelorTypeConfiguration.CONCEPT.AUTOMATIC:
                        await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.CONCEPT_BACHELOR, configurationTemplate.ConceptBachelorAutomatic);
                        break;
                    case ConstantHelpers.Configuration.BachelorTypeConfiguration.CONCEPT.BY_REQUEST:
                        await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.CONCEPT_BACHELOR, configurationTemplate.ConceptBachelorRequested);
                        break;
                }
                await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE, configurationTemplate.ConceptTitleDegreeProfessionalExperience);
                await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM, configurationTemplate.ConceptTitleDegreeSufficiencyExam);
                await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUPPORT_TESIS, configurationTemplate.ConceptTitleDegreeSupportTesis);
                await UpdateConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.METHOD_TYPE_REGISTRY_PATTERN_CREATION, configurationTemplate.MethodTypeRegistryPatternCreation.ToString());
                await _context.SaveChangesAsync();

            }
        }

        public async Task<object> GetConceptsSelect2()
        {
            var query = await _context.Concepts.Select(x => new {
                Id = x.Id,
                Text = x.Description
            }).ToListAsync();
            return query;
        }

        public async Task<object> GetProceduresSelect2()
        {
            var query = await _context.Procedures.Where(x => x.ProcedureCategory.StaticType == AKDEMIC.CORE.Helpers.ConstantHelpers.PROCEDURE_CATEGORIES.STATIC_TYPE.DEGREES_AND_TITLES)
                .Select(x => new
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();
            return query;
        }

        public async Task<Configuration> FirstOrDefault()
            => await _context.Configurations.FirstOrDefaultAsync();

        public async Task<Configuration> GetConfigurationByGENIntegratedSystem()
        {
            var integrated = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM).FirstOrDefaultAsync();
            return integrated;
        }

        public async Task<string> GetConfigurationByGRADTupaTypeBachelor()
        {
            var bachelor = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_TYPE_BACHELOR).Select(x => x.Value).FirstOrDefaultAsync();
            return bachelor;
        }

        public async Task<Dictionary<string, string>> GetValuesDirection()
        {
            var values = await _context.Configurations
                .ToDictionaryAsync(x => x.Key, x => x.Value);

            return values;
        }

        public async Task<object> ConfigurationGradeModality(bool isIntegrated, int gradeType)
        {
            IQueryable<Configuration> query = null;
            var lstGuids = new List<Guid>();

            if (isIntegrated)
            {
                if (gradeType == 0)
                {
                    query = _context.Configurations.Where(x => (x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_BACHELOR || x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUPPORT_TESIS ||
                                       x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM ||
                                       x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE) && !String.IsNullOrEmpty(x.Value)).AsQueryable();
                }
                if (gradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
                {
                    query = _context.Configurations.Where(x => (x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_BACHELOR) && !String.IsNullOrEmpty(x.Value)).AsQueryable();
                }
                if (gradeType == ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE)
                {
                    query = _context.Configurations.Where(x => (x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUPPORT_TESIS ||
                                       x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM ||
                                       x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE) && !String.IsNullOrEmpty(x.Value)).AsQueryable();
                }
                
                lstGuids = await query.Select(x => Guid.Parse(x.Value)).ToListAsync();                
                var lstProcedureSelect = await _context.Procedures.Where(x => lstGuids.Contains(x.Id)).Select(x => new
                {
                    x.Id,
                    Text = x.Name
                }).ToListAsync();
                return lstProcedureSelect;                

            }
            else
            {
                if (gradeType == 0)
                {
                    query = _context.Configurations.Where(x => (x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_BACHELOR || x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUPPORT_TESIS ||
                                       x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM ||
                                       x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE) && !String.IsNullOrEmpty(x.Value)).AsQueryable();
                }
                if (gradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
                {
                    query = _context.Configurations.Where(x => (x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_BACHELOR) && !String.IsNullOrEmpty(x.Value)).AsQueryable();
                }
                if (gradeType == ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE)
                {
                    query = _context.Configurations.Where(x => (x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUPPORT_TESIS ||
                                       x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM ||
                                       x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE) && !String.IsNullOrEmpty(x.Value)).AsQueryable();
                }



                lstGuids = await query.Select(x => Guid.Parse(x.Value)).ToListAsync();                
                var lstConceptSelect = await _context.Concepts.Where(x => lstGuids.Contains(x.Id)).Select(x => new
                {
                    x.Id,
                    Text = x.Description
                }).ToListAsync();
                return lstConceptSelect;
            }
            

        }
    }
}
