using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryTemaryRepository : IRepository<PreuniversitaryTemary>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTemariesDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, string searchValue = null);
        Task<object> GetTemariesListByCourseIdAndTermId(Guid courseId, Guid termId);
    }
}
