using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IRecordHistoryRepository : IRepository<RecordHistory>
    {
        Task<int> GetLatestRecordNumberByType(int type);
        Task<int> GetLatestRecordNumberByType(int type, int year);
        Task<RecordHistory> GetLatestByType(int type);
        Task<RecordHistory> GetByNumberAndType(int number, int type);
        //Task<IEnumerable<RecordHistory>> GetAllByNameAndDniAndType(string name = null, string dni = null, int? type = null);
        Task<int> GetRecordByTypeAndYear(int type, int year);
        Task<object> GetReportQuantityByYearToAcademicRecord(int year);
        Task<object> GetReportStatusByYearToAcademicRecord(int year);
        Task<object> GetReportFinishedVsPendingByMonth(int month);
        Task<object> GetReportByAcademicRecordChart();
        Task<object> GetReportByMonthAndUserIdChart(int month, string userId);
        Task<IEnumerable<RecordHistory>> GetAllByStudentId(Guid studentId);
        Task<DataTablesStructs.ReturnedData<object>> GetRecordHistoriesDataTable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, int? status, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetRecordsByStudentDatatable(DataTablesStructs.SentParameters parameters, Guid studentId);
    }
}
