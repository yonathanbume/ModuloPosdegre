using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IDeferredExamStudentService
    {
        Task<DeferredExamStudent> Get(Guid id);
        Task Delete(DeferredExamStudent entity);
        Task<DataTablesStructs.ReturnedData<object>> GetDeferredExamStudentsDatatable(DataTablesStructs.SentParameters parameters, Guid deferredExamId, string search);
        Task<List<DeferredExamStudent>> GetAll(Guid deferredExamId);
        Task DeleteRange(IEnumerable<DeferredExamStudent> entities);
        Task InsertRange(IEnumerable<DeferredExamStudent> entities);
    }
}
