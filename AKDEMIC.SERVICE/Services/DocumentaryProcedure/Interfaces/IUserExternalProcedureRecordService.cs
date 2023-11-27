using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IUserExternalProcedureRecordService
    {
        Task<UserExternalProcedureRecord> Get(Guid id);
        Task Insert(UserExternalProcedureRecord userExternalProcedure);
        Task Update(UserExternalProcedureRecord userExternalProcedure);
        Task Delete(UserExternalProcedureRecord userExternalProcedure);
        Task<int> CountByUserExternalProcedureId(Guid userExternalProcedureId);
        Task<Tuple<int, List<UserExternalProcedureRecord>>> GetUserExternalProcedureRecords(DateTime? beginDate, DateTime? endDate, DataTablesStructs.SentParameters sentParameters, Guid? dependecyId );
        Task<int> GetNextRecordNumberByDocumentaryRecordTypeId(Guid documentaryRecordTypeId);
        Task<UserExternalProcedureRecord> GetUserExternalProcedureRecordByUserExternalProcedure(Guid userExternalProcedureId);
        Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecords();
        Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsByDate(DateTime start, DateTime end);
        Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsByEndDate(DateTime end);
        Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsBySearchValue(string searchValue);
        Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsByStartDate(DateTime start);
        Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsByUserExternalProcedure(Guid userExternalProcedureId);
        Task<DataTablesStructs.ReturnedData<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
