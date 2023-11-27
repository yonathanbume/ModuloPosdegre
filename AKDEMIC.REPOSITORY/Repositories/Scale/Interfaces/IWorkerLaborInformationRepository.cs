using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.WorkerLaborInformation;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerLaborInformationRepository : IRepository<WorkerLaborInformation>
    {
        Task<WorkerLaborInformation> GetByUserId(string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetLaborUserDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<List<LaborUserTemplate>> GetUserLaborReport(string search);
        Task<bool> AnyPlaceCode(string placeCode,string userId);
        void Remove(WorkerLaborInformation workerLaborInformation);
        Task<object> GetUserDetailInformation(string userId);
    }
}
