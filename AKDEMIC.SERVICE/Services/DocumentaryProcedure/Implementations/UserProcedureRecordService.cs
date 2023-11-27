using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class UserProcedureRecordService : IUserProcedureRecordService
    {
        private readonly IUserProcedureRecordRepository _userProcedureRecordRepository;

        public UserProcedureRecordService(IUserProcedureRecordRepository userProcedureRecordRepository)
        {
            _userProcedureRecordRepository = userProcedureRecordRepository;
        }

        public async Task<UserProcedureRecord> Get(Guid id)
        {
            return await _userProcedureRecordRepository.Get(id);
        }

        public async Task Insert(UserProcedureRecord userProcedureRecord)
        {
            await _userProcedureRecordRepository.Insert(userProcedureRecord);
        }

        public async Task Update(UserProcedureRecord userProcedureRecord)
        {
            await _userProcedureRecordRepository.Update(userProcedureRecord);
        }

        public async Task Delete(UserProcedureRecord userProcedureRecord)
        {
            await _userProcedureRecordRepository.Delete(userProcedureRecord);
        }

        public async Task<int> CountByUserProcedureId(Guid userProcedureId)
        {
            return await _userProcedureRecordRepository.CountByUserProcedureId(userProcedureId);
        }

        public async Task<Tuple<int, List<UserProcedureRecord>>> GetUserProcedureRecords(DateTime? beginDate, DateTime? endDate, DataTablesStructs.SentParameters sentParameters)
        {
            return await _userProcedureRecordRepository.GetUserProcedureRecords(beginDate, endDate, sentParameters);
        }

        public async Task<Tuple<int, List<UserProcedureRecord>>> GetUserProcedureRecordsByDniOrRecordNumber(DataTablesStructs.SentParameters sentParameters)
        {
            return await _userProcedureRecordRepository.GetUserProcedureRecordsByDniOrRecordNumber(sentParameters);
        }

        public async Task<int> GetNextRecordNumberByDocumentaryRecordTypeId(Guid documentaryRecordTypeId)
        {
            return await _userProcedureRecordRepository.GetNextRecordNumberByDocumentaryRecordTypeId(documentaryRecordTypeId);
        }

        public async Task<UserProcedureRecord> GetUserProcedureRecordByUserProcedure(Guid userProcedureId)
        {
            return await _userProcedureRecordRepository.GetUserProcedureRecordByUserProcedure(userProcedureId);
        }

        public async Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecords()
        {
            return await _userProcedureRecordRepository.GetUserProcedureRecords();
        }

        public async Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsByDate(DateTime start, DateTime end)
        {
            return await _userProcedureRecordRepository.GetUserProcedureRecordsByDate(start, end);
        }

        public async Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsByEndDate(DateTime end)
        {
            return await _userProcedureRecordRepository.GetUserProcedureRecordsByEndDate(end);
        }

        public async Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsBySearchValue(string searchValue)
        {
            return await _userProcedureRecordRepository.GetUserProcedureRecordsBySearchValue(searchValue);
        }

        public async Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsByStartDate(DateTime start)
        {
            return await _userProcedureRecordRepository.GetUserProcedureRecordsByStartDate(start);
        }

        public async Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsByUserProcedure(Guid userProcedureId)
        {
            return await _userProcedureRecordRepository.GetUserProcedureRecordsByUserProcedure(userProcedureId);
        }
    }
}
