using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Templates;
using AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Implementations
{
    public class PreuniversitaryAssistanceService : IPreuniversitaryAssistanceService
    {
        private readonly IPreuniversitaryAssistanceRepository _preuniversitaryAssistanceRepository;

        public PreuniversitaryAssistanceService(IPreuniversitaryAssistanceRepository preuniversitaryAssistanceRepository)
        {
            _preuniversitaryAssistanceRepository = preuniversitaryAssistanceRepository;
        }

        public async Task<List<PreuniversitaryUserGroupTemplate>> GetAssistancesStudentByAssistanceId(PreuniversitaryAssistance entity, string userId, Guid scheduleId, Guid assistanceId)
            => await _preuniversitaryAssistanceRepository.GetAssistancesStudentByAssistanceId(entity, userId, scheduleId, assistanceId);

        public async Task<PreuniversitaryAssistance> GetByScheduleAndCurrentDate(Guid scheduleId)
            => await _preuniversitaryAssistanceRepository.GetByScheduleAndCurrentDate(scheduleId);

        public async Task Insert(PreuniversitaryAssistance entity)
            => await _preuniversitaryAssistanceRepository.Insert(entity);

        public async Task InsertRangeStudentAssistance(IEnumerable<PreuniversitaryAssistanceStudent> entities)
            => await _preuniversitaryAssistanceRepository.InsertRangeStudentAssistance(entities);

        public async Task Update(PreuniversitaryAssistance entity)
            => await _preuniversitaryAssistanceRepository.Update(entity);
    }
}
