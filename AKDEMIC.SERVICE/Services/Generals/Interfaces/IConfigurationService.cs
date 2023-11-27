using AKDEMIC.ENTITIES.Models;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IConfigurationService
    {
        Task Add(Configuration configuration);
        Task<Configuration> Get(Guid id);
        Task<Configuration> GetByKey(string key);
        Task<string> GetValueByKey(string key);
        Task Insert(Configuration configuration);
        Task Update(Configuration configuration);
        Task<Dictionary<string, string>> GetDataDictionary();
        Task<Configuration> FirstOrDefaultByKey(string id);
        Task<ConfigurationTemplate> GetConfigurationTemplateAsync();
        Task UpdateConfiguration(ConfigurationTemplate configurationTemplate);
        Task<Configuration> FirstOrDefault();
        Task<object> GetProceduresSelect2();
        Task<object> GetConceptsSelect2();
        Task<Configuration> GetConfigurationByGENIntegratedSystem();
        Task<string> GetConfigurationByGRADTupaTypeBachelor();
        Task<Dictionary<string, string>> GetValuesDirection();
        Task<object> ConfigurationGradeModality(bool isIntegrated, int gradeType);
        Task UpdateConfigurationValue(string key, string value);
    }
}
