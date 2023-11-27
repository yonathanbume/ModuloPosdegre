using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class UserExternalProcedureRecordService : IUserExternalProcedureRecordService
    {
        private readonly IUserExternalProcedureRecordRepository _userExternalProcedureRecordRepository;

        public UserExternalProcedureRecordService(IUserExternalProcedureRecordRepository userExternalProcedureRecordRepository)
        {
            _userExternalProcedureRecordRepository = userExternalProcedureRecordRepository;
        }

        public async Task<UserExternalProcedureRecord> Get(Guid id)
        {
            return await _userExternalProcedureRecordRepository.Get(id);
        }

        public async Task Insert(UserExternalProcedureRecord userExternalProcedure)
        {
            await _userExternalProcedureRecordRepository.Insert(userExternalProcedure);
        }

        public async Task Update(UserExternalProcedureRecord userExternalProcedure)
        {
            await _userExternalProcedureRecordRepository.Update(userExternalProcedure);
        }

        public async Task Delete(UserExternalProcedureRecord userExternalProcedure)
        {
            await _userExternalProcedureRecordRepository.Delete(userExternalProcedure);
        }

        public async Task<int> CountByUserExternalProcedureId(Guid userExternalProcedureId)
        {
            return await _userExternalProcedureRecordRepository.CountByUserExternalProcedureId(userExternalProcedureId);
        }

        public async Task<Tuple<int, List<UserExternalProcedureRecord>>> GetUserExternalProcedureRecords(DateTime? beginDate, DateTime? endDate, DataTablesStructs.SentParameters sentParameters, Guid? dependencyId)
        {
            return await _userExternalProcedureRecordRepository.GetUserExternalProcedureRecords(beginDate, endDate, sentParameters, dependencyId);
        }

        public async Task<int> GetNextRecordNumberByDocumentaryRecordTypeId(Guid documentaryRecordTypeId)
        {
            return await _userExternalProcedureRecordRepository.GetNextRecordNumberByDocumentaryRecordTypeId(documentaryRecordTypeId);
        }

        public async Task<UserExternalProcedureRecord> GetUserExternalProcedureRecordByUserExternalProcedure(Guid userExternalProcedureId)
        {
            return await _userExternalProcedureRecordRepository.GetUserExternalProcedureRecordByUserExternalProcedure(userExternalProcedureId);
        }

        public async Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecords()
        {
            return await _userExternalProcedureRecordRepository.GetUserExternalProcedureRecords();
        }

        public async Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsByDate(DateTime start, DateTime end)
        {
            return await _userExternalProcedureRecordRepository.GetUserExternalProcedureRecordsByDate(start, end);
        }

        public async Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsByEndDate(DateTime end)
        {
            return await _userExternalProcedureRecordRepository.GetUserExternalProcedureRecordsByEndDate(end);
        }

        public async Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsBySearchValue(string searchValue)
        {
            return await _userExternalProcedureRecordRepository.GetUserExternalProcedureRecordsBySearchValue(searchValue);
        }

        public async Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsByStartDate(DateTime start)
        {
            return await _userExternalProcedureRecordRepository.GetUserExternalProcedureRecordsByStartDate(start);
        }

        public async Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsByUserExternalProcedure(Guid userExternalProcedureId)
        {
            return await _userExternalProcedureRecordRepository.GetUserExternalProcedureRecordsByUserExternalProcedure(userExternalProcedureId);
        }

        public async Task<DataTablesStructs.ReturnedData<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _userExternalProcedureRecordRepository.GetUserExternalProcedureRecordsDatatable(sentParameters, searchValue);
    }
}
