using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IProcedureRequirementRepository : IRepository<ProcedureRequirement>
    {
        Task<ProcedureRequirement> GetProcedureRequirement(Guid id);
        Task<decimal> GetProcedureRequirementsCostSumByProcedure(Guid procedureId);
        Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirements();
        Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirementsByProcedure(Guid procedureId);
        Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirementsByUserProcedure(Guid userProcedureId);
        Task<DataTablesStructs.ReturnedData<ProcedureRequirement>> GetProcedureRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<ProcedureRequirement>> GetProcedureRequirementsDatatableByProcedure(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<ProcedureRequirement>> GetProcedureRequirementsDatatableByUserProcedure(DataTablesStructs.SentParameters sentParameters, Guid userProcedureId, string searchValue = null);








        Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirementsByUserProcedureRecord(Guid userProcedureRecordId);
        Task<decimal> GetProcedureAmount(Guid id);
    }
}
