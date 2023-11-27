using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICalificationCriteriaService
    {
        Task InsertCalificationCriteria(CalificationCriteria calificationCriteria);
        Task UpdateCalificationCriteria(CalificationCriteria calificationCriteria);
        Task DeleteCalificationCriteria(CalificationCriteria calificationCriteria);
        Task<CalificationCriteria> GetCalificationCriteriaById(Guid id);
        Task<IEnumerable<CalificationCriteria>> GetAllCalificationCriterias();
        Task<object> GetCalificationCriteriasObj();
        Task<bool> GetAnyCalificationCriterias(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetCalificationsCriteriasDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
