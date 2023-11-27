using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerTrainingRepository : IRepository<WorkerTraining>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerTrainingDatatble(DataTablesStructs.SentParameters parameters, string userId, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetUsersWorkerTrainingDatatble(DataTablesStructs.SentParameters parameters, int? userType = null, string searchValue = null);
    }
}
