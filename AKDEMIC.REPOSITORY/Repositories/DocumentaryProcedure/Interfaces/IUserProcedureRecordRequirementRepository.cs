using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IUserProcedureRecordRequirementRepository : IRepository<UserProcedureRecordRequirement>
    {
        Task<List<ProcedureRequirement>> GetRecordRequirementsByUserProcedureRecordId(Guid userProcedureRecordId);
        Task<List<UserProcedureRecordRequirement>> GetUserProcedureRecordRequirementsByUserProcedureRecordId(Guid userProcedureRecordId);
        Task<IEnumerable<UserProcedureRecordRequirement>> GetUserProcedureRecordRequirementByUserProcedureRecordAsync(Guid userProcedureRecordId);
    }
}
