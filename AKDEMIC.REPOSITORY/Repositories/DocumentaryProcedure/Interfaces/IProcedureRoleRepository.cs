using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IProcedureRoleRepository : IRepository<ProcedureRole>
    {
        Task<ProcedureRole> GetProcedureRole(Guid id);
        Task<List<ProcedureRole>> GetProcedureRolesByProcedureId(Guid procedureId);
        Task<bool> ToggleProcedureRole(Guid procedureId, string roleId);
        Task<bool> ValidRoleToProcedure(Guid procedureId, string roleId);
        Task<DataTablesStructs.ReturnedData<object>> GetProcedureRolesAssignedDatatable(DataTablesStructs.SentParameters sentParameters, Guid procedureId);
    }
}
