using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IProcedureTaskService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid procedureId, string search);
        Task Insert(ProcedureTask entity);
        Task Update(ProcedureTask entity);
        Task Delete(ProcedureTask entity);
        Task<List<ProcedureTask>> GetProcedureTasks(Guid procedureId);
        Task<ProcedureTask> Get(Guid id);
    }
}
