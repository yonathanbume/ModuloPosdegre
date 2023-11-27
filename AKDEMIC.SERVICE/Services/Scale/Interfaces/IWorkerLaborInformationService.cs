using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.WorkerLaborInformation;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerLaborInformationService
    {
        Task<WorkerLaborInformation> GetByUserId(string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetLaborUserDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<List<LaborUserTemplate>> GetUserLaborReport(string search);
        Task<object> GetUserDetailInformation(string userId);
        Task Update(WorkerLaborInformation workerLaborInformation);
        void Remove(WorkerLaborInformation workerLaborInformation);
        Task<WorkerLaborInformation> Add(WorkerLaborInformation workerLaborInformation);
        Task Delete(WorkerLaborInformation workerLaborInformation);
        Task Insert(WorkerLaborInformation workerLaborInformation);
        Task<bool> AnyPlaceCode(string placeCode, string userId = null);
    }
}
