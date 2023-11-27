using AKDEMIC.ENTITIES.Models;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IConfigurationRepository : IRepository<Configuration>
    {
        Task<Dictionary<string,string>> GetDataDictionary();
        Task<Configuration> GetByKey(string key);
        Task<string> GetValueByKey(string key);
        Task<Configuration> FirstOrDefault();
        Task<Configuration> FirstOrDefaultByKey(string key);
        Task<ConfigurationTemplate> GetConfigurationTemplateAsync();
        Task UpdateConfiguration(ConfigurationTemplate configurationTemplate);
        Task<object> GetProceduresSelect2();
        Task<object> GetConceptsSelect2();
        Task<Configuration> GetConfigurationByGENIntegratedSystem();
        Task<string> GetConfigurationByGRADTupaTypeBachelor();
        Task<Dictionary<string, string>> GetValuesDirection();
        Task<object> ConfigurationGradeModality(bool isIntegrated, int gradeType);
        Task UpdateConfigurationValue(string key, string value);
    }
}
                