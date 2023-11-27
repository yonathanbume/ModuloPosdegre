using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IProcedureRoleService
    {
        Task<ProcedureRole> Get(Guid id);
        Task<ProcedureRole> GetProcedureRole(Guid id);
        Task<bool> ToggleProcedureRole(Guid procedureId, string roleId);
        Task Delete(ProcedureRole procedureRole);
        Task Insert(ProcedureRole procedureRole);
        Task<bool> ValidRoleToProcedure(Guid procedureId, string roleId);
        Task DeleteRange(IEnumerable<ProcedureRole> entities);
        Task<List<ProcedureRole>> GetProcedureRolesByProcedureId(Guid procedureId);
        Task<DataTablesStructs.ReturnedData<object>> GetProcedureRolesAssignedDatatable(DataTablesStructs.SentParameters sentParameters, Guid procedureId);
    }
}
