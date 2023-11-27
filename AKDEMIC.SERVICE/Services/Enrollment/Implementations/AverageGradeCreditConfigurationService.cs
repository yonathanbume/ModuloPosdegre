using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class AverageGradeCreditConfigurationService : IAverageGradeCreditConfigurationService
    {
        private readonly IAverageGradeCreditConfigurationRepository _averageGradeCreditConfigurationRepository;
        public AverageGradeCreditConfigurationService(IAverageGradeCreditConfigurationRepository averageGradeCreditConfigurationRepository)
        {
            _averageGradeCreditConfigurationRepository = averageGradeCreditConfigurationRepository;
        }

        public async Task DeleteAll()
        {
            var data = await _averageGradeCreditConfigurationRepository.GetAll();
            await _averageGradeCreditConfigurationRepository.DeleteRange(data);
        }
        
        public async Task<IEnumerable<AverageGradeCreditConfiguration>> GetAll()
            => await _averageGradeCreditConfigurationRepository.GetAll();

        public async Task InsertRange(List<AverageGradeCreditConfiguration> averageGradeCreditConfigurations)
            => await _averageGradeCreditConfigurationRepository.InsertRange(averageGradeCreditConfigurations);
    }
}
