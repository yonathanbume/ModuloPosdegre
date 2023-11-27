using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class UserProcedureDerivationService : IUserProcedureDerivationService
    {
        private readonly IUserProcedureDerivationRepository _userProcedureDerivationRepository;

        public UserProcedureDerivationService(IUserProcedureDerivationRepository userProcedureDerivationRepository)
        {
            _userProcedureDerivationRepository = userProcedureDerivationRepository;
        }

        public Task<IEnumerable<UserProcedureDerivation>> GetUserProcedureDerivationsByUserProcedure(Guid userProcedureId)
        {
            return _userProcedureDerivationRepository.GetUserProcedureDerivationsByUserProcedure(userProcedureId);
        }

        public async Task Insert(UserProcedureDerivation userProcedureDerivation) =>
            await _userProcedureDerivationRepository.Insert(userProcedureDerivation);

        public async Task<IEnumerable<UserProcedureDerivation>> GetByUserInternalProcedure(Guid id)
            => await _userProcedureDerivationRepository.GetByUserInternalProcedure(id);
    }
}
