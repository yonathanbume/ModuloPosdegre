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
    public class RegisterCourseConferenceService : IRegisterCourseConferenceService
    {
        private readonly IRegisterCourseConferenceRepository _registerCourseConferenceRepository;
        public RegisterCourseConferenceService(IRegisterCourseConferenceRepository registerCourseConferenceRepository)
        {
            _registerCourseConferenceRepository = registerCourseConferenceRepository;
        }

        public async Task<IEnumerable<MemberRegister>> GetRegisterConference(Guid id)
        {
            return await _registerCourseConferenceRepository.GetRegisterConference(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRegisterConferenceDataTable(DataTablesStructs.SentParameters parameters, Guid id, string search)
        {
            return await _registerCourseConferenceRepository.GetRegisterConferenceDataTable(parameters, id, search);
        }

        public async Task Insert(RegisterCourseConference register)
        {
            await _registerCourseConferenceRepository.Insert(register);
        }

        public Task<bool> IsRegistered(Guid couseConferenceId, string dni, bool isId = false)
            => _registerCourseConferenceRepository.IsRegistered(couseConferenceId, dni, isId);
    }
}
