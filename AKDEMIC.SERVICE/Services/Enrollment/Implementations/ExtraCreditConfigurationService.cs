using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class ExtraCreditConfigurationService : IExtraCreditConfigurationService
    {
        private readonly IExtraCreditConfigurationRepository _extraCreditConfigurationRepository;
        public ExtraCreditConfigurationService(IExtraCreditConfigurationRepository extraCreditConfigurationRepository)
        {
            _extraCreditConfigurationRepository = extraCreditConfigurationRepository;
        }

        public async Task DeleteAll()
        {
            var configurations = await _extraCreditConfigurationRepository.GetAll();
            await _extraCreditConfigurationRepository.DeleteRange(configurations);
        }

        public async Task<IEnumerable<ExtraCreditConfiguration>> GetAll()
            => await _extraCreditConfigurationRepository.GetAll();

        public async Task<object> GetDataDatatable() 
            => await _extraCreditConfigurationRepository.GetDataDatatable();

        public async Task InsertRange(List<ExtraCreditConfiguration> extraCreditConfigurations)
            => await _extraCreditConfigurationRepository.InsertRange(extraCreditConfigurations);
    }
}
