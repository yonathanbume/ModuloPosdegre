using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IAverageGradeCreditConfigurationService
    {
        Task DeleteAll();
        Task InsertRange(List<AverageGradeCreditConfiguration> averageGradeCreditConfigurations);
        Task<IEnumerable<AverageGradeCreditConfiguration>> GetAll();
    }
}
