using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkerAssistance;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _workerRepository;

        public WorkerService(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<bool> Any(Guid workerId)
            => await _workerRepository.Any(workerId);

        public async Task Delete(Worker worker)
            => await _workerRepository.Delete(worker);

        public async Task DeleteBankAccount(Guid workerId)
            => await _workerRepository.DeleteBankAccount(workerId);

        public async Task DeleteById(Guid workerId)
            => await _workerRepository.DeleteById(workerId);

        public async Task<Worker> Get(Guid workerId)
            => await _workerRepository.Get(workerId);

        public async Task<IEnumerable<Guid>> GetActiveIds()
            => await _workerRepository.GetActiveIds();

        public async Task<IEnumerable<Worker>> GetAllActive()
            => await _workerRepository.GetAllActive();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, byte type ,string searchValue = null)
                    => _workerRepository.GetAllDatatable(sentParameters, type ,searchValue);

        public async Task<Worker> GetWithWorkerLaborInformationById(Guid workerId)
            => await _workerRepository.GetWithWorkerLaborInformationById(workerId);

        public async Task<IEnumerable<Worker>> GetWorkers()
            => await _workerRepository.GetWorkers();

        public async Task<(IList<Worker> pagedList, int count)> GetWorkersByPaginationParameters(PaginationParameter paginationParameter, string code, string names, string surnames, int? laborType, DateTime? entryDate = null, DateTime? outDate = null)
            => await _workerRepository.GetWorkersByPaginationParameters(paginationParameter, code, names, surnames, laborType, entryDate, outDate);

        public Task<DataTablesStructs.ReturnedData<object>> GetWorkersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _workerRepository.GetWorkersDatatable(sentParameters, searchValue);
        public Task<DataTablesStructs.ReturnedData<object>> GetWorkersAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _workerRepository.GetWorkersAssistanceDatatable(sentParameters, searchValue);

        public Task<DataTablesStructs.ReturnedData<object>> GetWorkerAssistanceDetailDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchStartDate = null, string searchEndDate = null)
            => _workerRepository.GetWorkerAssistanceDetailDatatable(sentParameters, userId, searchStartDate, searchEndDate);

        public Task Update(Worker worker)
            => _workerRepository.Update(worker);

        public Task<WorkerReportTemplate> GetWorkerAssistanceInformation(Guid id, string searchStartDate = null, string searchEndDate = null)
            => _workerRepository.GetWorkerAssistanceInformation(id, searchStartDate, searchEndDate);
    }
}
