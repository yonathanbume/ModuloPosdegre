using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IUserProcedureDerivationRepository : IRepository<UserProcedureDerivation>
    {
        Task<IEnumerable<UserProcedureDerivation>> GetUserProcedureDerivationsByUserProcedure(Guid userProcedureId);
        Task<IEnumerable<UserProcedureDerivation>> GetByUserInternalProcedure(Guid id);
    }
}
