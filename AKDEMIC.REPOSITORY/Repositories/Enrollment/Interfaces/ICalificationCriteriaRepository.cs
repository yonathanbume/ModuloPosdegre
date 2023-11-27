using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ICalificationCriteriaRepository : IRepository<CalificationCriteria>
    {
        Task<object> GetCalificationCriteriasObj();
        Task<bool> GetAnyCalificationCriterias(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetCalificationsCriteriasDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
