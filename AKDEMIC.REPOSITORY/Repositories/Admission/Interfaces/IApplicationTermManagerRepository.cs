using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IApplicationTermManagerRepository : IRepository<ApplicationTermManager>
    {
        Task<List<ApplicationTermManager>> GetByApplicationTermId(Guid id);
        Task<DataTablesStructs.ReturnedData<ApplicationTermManager>> GetByApplicationTermIdDataTable(DataTablesStructs.SentParameters sentParameters, Guid id,string searchValue);
        Task<bool> AnyByApplicationTerm(Guid id, string userId);
    }
}
