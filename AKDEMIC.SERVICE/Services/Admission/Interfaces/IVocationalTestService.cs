using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IVocationalTestService
    {
        Task<DataTablesStructs.ReturnedData<object>> VocationalTestDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<bool> ExistAnyActive(Guid? Id = null);
        Task Insert(VocationalTest vocationalTest);
        Task<VocationalTest> Get(Guid Id);
        Task DeleteById(Guid Id);
        Task Update(VocationalTest vocationalTest);
    }
}
