using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class SessionRecordService : ISessionRecordService
    {
        private readonly ISessionRecordRepository _sessionRecordRepository;
        public SessionRecordService(ISessionRecordRepository sessionRecordRepository)
        {
            _sessionRecordRepository = sessionRecordRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _sessionRecordRepository.DeleteById(id);
        }

        public async Task<bool> ExistAnyWithName(Guid id, string name)
        {
            return await _sessionRecordRepository.ExistAnyWithName(id, name);
        }

        public async  Task<SessionRecord> Get(Guid id)
        {
            return await _sessionRecordRepository.Get(id);
        }

        public async Task<IEnumerable<SessionRecord>> GetAll()
        {
            return await _sessionRecordRepository.GetAll();
        }

        public async Task Insert(SessionRecord regulation)
        {
            await _sessionRecordRepository.Insert(regulation);
        }

        public async Task Update(SessionRecord regulation)
        {
            await _sessionRecordRepository.Update(regulation);
        }

        public async Task<SessionRecord> GetBySlug(string slug)
            => await _sessionRecordRepository.GetBySlug(slug);
    }
}
