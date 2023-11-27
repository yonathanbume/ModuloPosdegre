using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IUserProcedureDerivationFileRepository : IRepository<UserProcedureDerivationFile>
    {
        Task<IEnumerable<UserProcedureDerivationFile>> GetUserProcedureDerivationFilesByUserProcedure(Guid userProcedureId);
        Task<IEnumerable<UserProcedureDerivationFile>> GetUserProcedureDerivationFilesByUserProcedurept2(Guid userProcedureId);
        Task<IEnumerable<UserProcedureDerivationFile>> GetUserProcedureDerivationFilesByUserProcedureDerivation(Guid userProcedureDerivationId);
    }
}
