using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IProcedureRequirementService
    {
        Task<ProcedureRequirement> Get(Guid id);
        Task<ProcedureRequirement> GetProcedureRequirement(Guid id);
        Task<decimal> GetProcedureRequirementsCostSumByProcedure(Guid procedureId);
        Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirements();
        Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirementsByProcedure(Guid procedureId);
        Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirementsByUserProcedure(Guid userProcedureId);
        Task<DataTablesStructs.ReturnedData<ProcedureRequirement>> GetProcedureRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<ProcedureRequirement>> GetProcedureRequirementsDatatableByProcedure(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<ProcedureRequirement>> GetProcedureRequirementsDatatableByUserProcedure(DataTablesStructs.SentParameters sentParameters, Guid userProcedureId, string searchValue = null);
        Task Delete(ProcedureRequirement procedureRequirement);
        Task Insert(ProcedureRequirement procedureRequirement);
        Task Update(ProcedureRequirement procedureRequirement);








        Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirementsByUserProcedureRecord(Guid userProcedureRecordId);
        Task<decimal> GetProcedureAmount(Guid id);
    }
}
