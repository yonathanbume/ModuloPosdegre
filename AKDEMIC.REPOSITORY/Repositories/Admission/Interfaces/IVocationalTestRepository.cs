using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IVocationalTestRepository : IRepository<VocationalTest>
    {
        Task<DataTablesStructs.ReturnedData<object>> VocationalTestDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<bool> ExistAnyActive(Guid? Id = null);
        
    }
}
