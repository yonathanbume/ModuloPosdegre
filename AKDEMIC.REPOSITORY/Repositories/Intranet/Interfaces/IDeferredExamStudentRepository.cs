using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IDeferredExamStudentRepository : IRepository<DeferredExamStudent>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDeferredExamStudentsDatatable(DataTablesStructs.SentParameters parameters, Guid deferredExamId, string search);
        Task<List<DeferredExamStudent>> GetAll(Guid deferredExamId);
    }
}
