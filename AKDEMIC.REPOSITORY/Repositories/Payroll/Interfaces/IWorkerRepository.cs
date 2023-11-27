using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkerAssistance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IWorkerRepository : IRepository<Worker>
    {
        Task<IEnumerable<Worker>> GetAllActive();
        Task<IEnumerable<Guid>> GetActiveIds();
        Task<IEnumerable<Worker>> GetWorkers();
        Task<Worker> GetWithWorkerLaborInformationById(Guid workerId);
        Task<(IList<Worker> pagedList, int count)> GetWorkersByPaginationParameters(PaginationParameter paginationParameter,
            string code, string names, string surnames, int? laborType, DateTime? entryDate = null, DateTime? outDate = null);

        Task<DataTablesStructs.ReturnedData<object>> GetWorkersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetWorkersAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task DeleteBankAccount(Guid workerId);

        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, byte type ,string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerAssistanceDetailDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchStartDate = null, string searchEndDate = null);
        Task<WorkerReportTemplate> GetWorkerAssistanceInformation(Guid id, string searchStartDate = null, string searchEndDate = null);
    }
}
