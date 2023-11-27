using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class UserProcedureDerivationFileService : IUserProcedureDerivationFileService
    {
        private readonly IUserProcedureDerivationFileRepository _userProcedureDerivationFileRepository;

        public UserProcedureDerivationFileService(IUserProcedureDerivationFileRepository userProcedureDerivationFileRepository)
        {
            _userProcedureDerivationFileRepository = userProcedureDerivationFileRepository;
        }

        public async Task<IEnumerable<UserProcedureDerivationFile>> GetUserProcedureDerivationFilesByUserProcedure(Guid userProcedureId)
        {
            return await _userProcedureDerivationFileRepository.GetUserProcedureDerivationFilesByUserProcedure(userProcedureId);
        }
        public async Task<IEnumerable<UserProcedureDerivationFile>> GetUserProcedureDerivationFilesByUserProcedurept2(Guid userProcedureId)
            => await _userProcedureDerivationFileRepository.GetUserProcedureDerivationFilesByUserProcedurept2(userProcedureId);
        public async Task<IEnumerable<UserProcedureDerivationFile>> GetUserProcedureDerivationFilesByUserProcedureDerivation(Guid userProcedureDerivationId)
        {
            return await _userProcedureDerivationFileRepository.GetUserProcedureDerivationFilesByUserProcedureDerivation(userProcedureDerivationId);
        }

        public async Task Insert(UserProcedureDerivationFile userProcedureDerivationFile) =>
            await _userProcedureDerivationFileRepository.Insert(userProcedureDerivationFile);

        public async Task InsertRange(IEnumerable<UserProcedureDerivationFile> userProcedureDerivationFile) =>
            await _userProcedureDerivationFileRepository.InsertRange(userProcedureDerivationFile);
    }
}
