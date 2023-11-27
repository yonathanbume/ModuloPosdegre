using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public interface IProcedureResolutionService
    {
        Task<ProcedureResolution> Get(Guid id);
        Task<ProcedureResolution> GetProcedureResolution(Guid id);
        Task<IEnumerable<ProcedureResolution>> GetProcedureResolutionsByProcedure(Guid procedureId);
        Task<DataTablesStructs.ReturnedData<ProcedureResolution>> GetProcedureResolutionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<ProcedureResolution>> GetProcedureResolutionsDatatableByProcedure(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null);
        Task Delete(ProcedureResolution procedureResolution);
        Task Insert(ProcedureResolution procedureResolution);
        Task Update(ProcedureResolution procedureResolution);
    }
}
