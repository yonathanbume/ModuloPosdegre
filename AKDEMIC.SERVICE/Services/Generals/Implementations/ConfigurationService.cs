using AKDEMIC.ENTITIES.Models;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRepository _configurationRepository;

        public ConfigurationService(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public async Task Add(Configuration configuration) => await _configurationRepository.Add(configuration);

        public async Task<Configuration> Get(Guid id) => await _configurationRepository.Get(id);

        public async Task<Configuration> GetByKey(string key) => await _configurationRepository.GetByKey(key);

        public async Task<string> GetValueByKey(string key) => await _configurationRepository.GetValueByKey(key);

        public async Task<Dictionary<string, string>> GetDataDictionary() => await _configurationRepository.GetDataDictionary();

        public async Task Insert(Configuration configuration) => await _configurationRepository.Insert(configuration);

        public async Task Update(Configuration configuration) => await _configurationRepository.Update(configuration);

        public async Task<object> GetProceduresSelect2()
        {
            return await _configurationRepository.GetProceduresSelect2();
        }

        public async Task<object> GetConceptsSelect2()
        {
            return await _configurationRepository.GetConceptsSelect2();
        }

        public async Task<Configuration> FirstOrDefaultByKey(string key)
        {
            return await _configurationRepository.FirstOrDefaultByKey(key);
        }

        public async Task<ConfigurationTemplate> GetConfigurationTemplateAsync()
        {
            return await _configurationRepository.GetConfigurationTemplateAsync();
        }

        public async Task UpdateConfiguration(ConfigurationTemplate configurationTemplate)
        {
            await _configurationRepository.UpdateConfiguration(configurationTemplate);
        }

        public async Task<Configuration> FirstOrDefault()
            => await _configurationRepository.FirstOrDefault();

        public async Task<Configuration> GetConfigurationByGENIntegratedSystem()
            => await _configurationRepository.GetConfigurationByGENIntegratedSystem();

        public async Task<string> GetConfigurationByGRADTupaTypeBachelor()
            => await _configurationRepository.GetConfigurationByGRADTupaTypeBachelor();
        public async Task<Dictionary<string, string>> GetValuesDirection()
            => await _configurationRepository.GetValuesDirection();

        public async Task<object> ConfigurationGradeModality(bool isIntegrated, int gradeType)
        {
            return await _configurationRepository.ConfigurationGradeModality(isIntegrated, gradeType);
        }

        public async Task UpdateConfigurationValue(string key, string value)
        {
            await _configurationRepository.UpdateConfigurationValue(key, value);
        }
    }
}
