using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryTemaryService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTemariesDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, string searchValue = null);
        Task Insert(PreuniversitaryTemary entity);
        Task<PreuniversitaryTemary> Get(Guid id);
        Task Delete(PreuniversitaryTemary entity);
        Task<object> GetTemariesListByCourseIdAndTermId(Guid courseId, Guid termId);
    }
}
