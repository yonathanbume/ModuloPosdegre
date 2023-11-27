using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class RecordHistoryService : IRecordHistoryService
    {
        private readonly IRecordHistoryRepository _recordHistoryRepository;

        public RecordHistoryService(IRecordHistoryRepository recordHistoryRepository)
        {
            _recordHistoryRepository = recordHistoryRepository;
        }

        //public async Task<IEnumerable<RecordHistory>> GetAllByNameAndDniAndType(string name = null, string dni = null, int? type = null)
        //    => await _recordHistoryRepository.GetAllByNameAndDniAndType(name, dni, type);

        public async Task<RecordHistory> GetByNumberAndType(int number, int type)
            => await _recordHistoryRepository.GetByNumberAndType(number, type);

        public async Task<RecordHistory> GetLatestByType(int type)
            => await _recordHistoryRepository.GetLatestByType(type);

        public async Task<int> GetLatestRecordNumberByType(int type)
            => await _recordHistoryRepository.GetLatestRecordNumberByType(type);

        public async Task<int> GetLatestRecordNumberByType(int type, int year)
            => await _recordHistoryRepository.GetLatestRecordNumberByType(type, year);

        public async Task Insert(RecordHistory recordHistory)
            => await _recordHistoryRepository.Insert(recordHistory);
        public async Task<int> GetRecordByTypeAndYear(int type, int year)
            => await _recordHistoryRepository.GetRecordByTypeAndYear(type, year);

        public async Task<RecordHistory> Get(Guid id) => await _recordHistoryRepository.Get(id);

        public async Task<object> GetReportQuantityByYearToAcademicRecord(int year)
            => await _recordHistoryRepository.GetReportQuantityByYearToAcademicRecord(year);

        public async Task<object> GetReportStatusByYearToAcademicRecord(int year)
            => await _recordHistoryRepository.GetReportStatusByYearToAcademicRecord(year);

        public async Task<object> GetReportFinishedVsPendingByMonth(int month)
            => await _recordHistoryRepository.GetReportFinishedVsPendingByMonth(month);

        public async Task<object> GetReportByAcademicRecordChart()
            => await _recordHistoryRepository.GetReportByAcademicRecordChart();

        public async Task<object> GetReportByMonthAndUserIdChart(int month, string userId)
            => await _recordHistoryRepository.GetReportByMonthAndUserIdChart(month, userId);

        public async Task Update(RecordHistory recordHistory)
            => await _recordHistoryRepository.Update(recordHistory);

        public async Task<IEnumerable<RecordHistory>> GetAllByStudentId(Guid studentId)
            => await _recordHistoryRepository.GetAllByStudentId(studentId);

        public async Task AddAsync(RecordHistory entity)
            => await _recordHistoryRepository.Add(entity);

        public async Task<DataTablesStructs.ReturnedData<object>> GetRecordHistoriesDataTable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, int? status, string searchValue)
            => await _recordHistoryRepository.GetRecordHistoriesDataTable(parameters, user, status, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetRecordsByStudentDatatable(DataTablesStructs.SentParameters parameters, Guid studentId)
            => await _recordHistoryRepository.GetRecordsByStudentDatatable(parameters, studentId);
    }
}
