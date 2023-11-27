using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkerAssistance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IWorkerService
    {
        Task<IEnumerable<Worker>> GetAllActive();
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, byte type, string searchValue = null);
        Task<IEnumerable<Guid>> GetActiveIds();
        Task<Worker> Get(Guid workerId);
        Task<bool> Any(Guid workerId);
        Task DeleteById(Guid workerId);
        Task Delete(Worker worker);
        Task Update(Worker worker);
        Task<IEnumerable<Worker>> GetWorkers();
        Task<Worker> GetWithWorkerLaborInformationById(Guid workerId);
        Task<(IList<Worker> pagedList, int count)> GetWorkersByPaginationParameters(PaginationParameter paginationParameter,
            string code, string names, string surnames, int? laborType, DateTime? entryDate = null, DateTime? outDate = null);

        Task<DataTablesStructs.ReturnedData<object>> GetWorkersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetWorkersAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);

        Task<DataTablesStructs.ReturnedData<object>> GetWorkerAssistanceDetailDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchStartDate = null, string searchEndDate = null);
        Task DeleteBankAccount(Guid workerId);
        Task<WorkerReportTemplate> GetWorkerAssistanceInformation(Guid id, string searchStartDate = null, string searchEndDate = null);
    }
}
