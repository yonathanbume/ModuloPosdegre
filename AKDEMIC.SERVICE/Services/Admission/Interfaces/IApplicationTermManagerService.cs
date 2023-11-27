using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IApplicationTermManagerService
    {
        Task<List<ApplicationTermManager>> GetByApplicationTermId(Guid id);
        Task<DataTablesStructs.ReturnedData<ApplicationTermManager>> GetByApplicationTermIdDataTable(DataTablesStructs.SentParameters sentParameters, Guid id,string searchValue);
        Task Insert(ApplicationTermManager applicationTermManager);
        Task Update(ApplicationTermManager applicationTermManager);
        Task<ApplicationTermManager> Get(Guid id);
        Task DeleteById(Guid id);
        Task<bool> AnyByApplicationTerm(Guid id, string userId);
    }
}
