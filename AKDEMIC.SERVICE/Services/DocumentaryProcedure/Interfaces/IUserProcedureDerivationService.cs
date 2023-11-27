using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IUserProcedureDerivationService
    {
        Task<IEnumerable<UserProcedureDerivation>> GetUserProcedureDerivationsByUserProcedure(Guid userProcedureId);
        Task Insert(UserProcedureDerivation userProcedureDerivation);
        Task<IEnumerable<UserProcedureDerivation>> GetByUserInternalProcedure(Guid id);
    }
}
