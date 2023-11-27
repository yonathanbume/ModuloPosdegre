using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IProcedureResolutionRepository : IRepository<ProcedureResolution>
    {
        Task<ProcedureResolution> GetProcedureResolution(Guid id);
        Task<IEnumerable<ProcedureResolution>> GetProcedureResolutionsByProcedure(Guid procedureId);
        Task<DataTablesStructs.ReturnedData<ProcedureResolution>> GetProcedureResolutionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<ProcedureResolution>> GetProcedureResolutionsDatatableByProcedure(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null);
    }
}
