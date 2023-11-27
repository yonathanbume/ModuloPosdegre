using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class RegisterCulturalActivityService : IRegisterCulturalActivityService
    {
        private readonly IRegisterCulturalActivityRepository _registerCulturalActivityRepository;
        public RegisterCulturalActivityService(IRegisterCulturalActivityRepository registerCulturalActivityRepository)
        {
            _registerCulturalActivityRepository = registerCulturalActivityRepository;
        }

        public async Task<IEnumerable<MemberRegister>> GetParticipantsByCulturalActivityId(Guid id)
        {
            return await _registerCulturalActivityRepository.GetParticipantsByCulturalActivityId(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetParticipantsByCulturalActivityIdDataTable(DataTablesStructs.SentParameters parameters, Guid id, string search)
        {
            return await _registerCulturalActivityRepository.GetParticipantsByCulturalActivityIdDataTable(parameters, id, search);
        }

        public async Task Insert(RegisterCulturalActivity register)
        {
            await _registerCulturalActivityRepository.Insert(register);
        }

        public async Task<bool> IsRegistered(Guid activityId ,string dni, bool IsId = false)
        {
            return await _registerCulturalActivityRepository.IsRegistered(activityId, dni, IsId);
        }
    }
}
