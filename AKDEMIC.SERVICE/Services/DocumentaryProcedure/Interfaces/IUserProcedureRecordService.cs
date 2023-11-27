using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IUserProcedureRecordService
    {
        Task<UserProcedureRecord> Get(Guid id);
        Task Insert(UserProcedureRecord userProcedureRecord);
        Task Update(UserProcedureRecord userProcedureRecord);
        Task Delete(UserProcedureRecord userProcedureRecord);
        Task<int> CountByUserProcedureId(Guid userProcedureId);
        Task<Tuple<int, List<UserProcedureRecord>>> GetUserProcedureRecords(DateTime? beginDate, DateTime? endDate, DataTablesStructs.SentParameters sentParameters);
        Task<Tuple<int, List<UserProcedureRecord>>> GetUserProcedureRecordsByDniOrRecordNumber(DataTablesStructs.SentParameters sentParameters);
        Task<int> GetNextRecordNumberByDocumentaryRecordTypeId(Guid documentaryRecordTypeId);
        Task<UserProcedureRecord> GetUserProcedureRecordByUserProcedure(Guid userProcedureId);
        Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecords();
        Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsByDate(DateTime start, DateTime end);
        Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsByEndDate(DateTime end);
        Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsBySearchValue(string searchValue);
        Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsByStartDate(DateTime start);
        Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsByUserProcedure(Guid userProcedureId);
    }
}
