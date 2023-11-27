using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IUserProcedureRecordRequirementService
    {
        Task InsertRange(IEnumerable<UserProcedureRecordRequirement> requirements);
        Task DeleteRange(IEnumerable<UserProcedureRecordRequirement> requirements);
        Task<List<ProcedureRequirement>> GetRecordRequirementsByUserProcedureRecordId(Guid userProcedureRecordId);
        Task<List<UserProcedureRecordRequirement>> GetUserProcedureRecordRequirementsByUserProcedureRecordId(Guid userProcedureRecordId);
        Task<IEnumerable<UserProcedureRecordRequirement>> GetUserProcedureRecordRequirementByUserProcedureRecord(Guid userProcedureRecordId);
    }
}
