using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IExtraCreditConfigurationService
    {
        Task<object> GetDataDatatable();
        Task DeleteAll();
        Task InsertRange(List<ExtraCreditConfiguration> extraCreditConfigurations);
        Task<IEnumerable<ExtraCreditConfiguration>> GetAll();
    }
}
