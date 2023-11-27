using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class UserProcedureRecordRequirementService : IUserProcedureRecordRequirementService
    {
        private readonly IUserProcedureRecordRequirementRepository _userProcedureRecordRequirementRepository;

        public UserProcedureRecordRequirementService(IUserProcedureRecordRequirementRepository userProcedureRecordRequirementRepository)
        {
            _userProcedureRecordRequirementRepository = userProcedureRecordRequirementRepository;
        }

        public async Task InsertRange(IEnumerable<UserProcedureRecordRequirement> requirements)
        {
            await _userProcedureRecordRequirementRepository.InsertRange(requirements);
        }

        public async Task DeleteRange(IEnumerable<UserProcedureRecordRequirement> requirements)
        {
            await _userProcedureRecordRequirementRepository.DeleteRange(requirements);
        }

        public async Task<List<ProcedureRequirement>> GetRecordRequirementsByUserProcedureRecordId(Guid userProcedureRecordId)
        {
            return await _userProcedureRecordRequirementRepository.GetRecordRequirementsByUserProcedureRecordId(userProcedureRecordId);
        }

        public async Task<List<UserProcedureRecordRequirement>> GetUserProcedureRecordRequirementsByUserProcedureRecordId(Guid userProcedureRecordId)
        {
            return await _userProcedureRecordRequirementRepository.GetUserProcedureRecordRequirementsByUserProcedureRecordId(userProcedureRecordId);
        }

        public async Task<IEnumerable<UserProcedureRecordRequirement>> GetUserProcedureRecordRequirementByUserProcedureRecord(Guid userProcedureRecordId)
        {
            return await _userProcedureRecordRequirementRepository.GetUserProcedureRecordRequirementByUserProcedureRecordAsync(userProcedureRecordId);
        }
    }
}
