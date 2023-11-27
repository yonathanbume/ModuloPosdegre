using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class SessionRecordFilesService : ISessionRecordFilesService
    {
        private readonly ISessionRecordFilesRepository _sessionRecordFilesRepository;
        public SessionRecordFilesService(ISessionRecordFilesRepository sessionRecordFilesRepository)
        {
            _sessionRecordFilesRepository = sessionRecordFilesRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _sessionRecordFilesRepository.DeleteById(id);
        }

        public async  Task<SessionRecordFile> Get(Guid id)
        {
            return await _sessionRecordFilesRepository.Get(id);
        }

        public Task<object> GetById(Guid id)
            => _sessionRecordFilesRepository.GetById(id);

        public async Task<List<SessionRecordFile>> GetBySessionRecordId(Guid id)
        {
            return await _sessionRecordFilesRepository.GetBySessionRecordId(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid sessionRecordId, Guid? facultyId, DateTime? startDate, DateTime? endDate, string search)
        {
            return await _sessionRecordFilesRepository.GetDatatable(parameters, sessionRecordId, facultyId, startDate, endDate, search);
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetFilesDatatable(DataTablesStructs.SentParameters parameters, Guid sessionRecordId, Guid? facultyId = null, DateTime? startDate = null, DateTime? endDate = null, string search = null)
            => _sessionRecordFilesRepository.GetFilesDatatable(parameters, sessionRecordId, facultyId, startDate, endDate, search);

        public async Task Insert(SessionRecordFile regulation)
        {
            await _sessionRecordFilesRepository.Insert(regulation);
        }

        public async Task Update(SessionRecordFile regulation)
        {
            await _sessionRecordFilesRepository.Update(regulation);
        }
    }
}
