using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IUserProcedureDerivationFileService
    {
        Task<IEnumerable<UserProcedureDerivationFile>> GetUserProcedureDerivationFilesByUserProcedure(Guid userProcedureId);
        Task<IEnumerable<UserProcedureDerivationFile>> GetUserProcedureDerivationFilesByUserProcedurept2(Guid userProcedureId);
        Task<IEnumerable<UserProcedureDerivationFile>> GetUserProcedureDerivationFilesByUserProcedureDerivation(Guid userProcedureDerivationId);
        Task Insert(UserProcedureDerivationFile userProcedureDerivationFile);
        Task InsertRange(IEnumerable<UserProcedureDerivationFile> userProcedureDerivationFile);
    }
}
