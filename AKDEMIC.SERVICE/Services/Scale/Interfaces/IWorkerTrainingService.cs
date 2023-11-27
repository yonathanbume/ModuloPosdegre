using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerTrainingService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerTrainingDatatble(DataTablesStructs.SentParameters parameters, string userId, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetUsersWorkerTrainingDatatble(DataTablesStructs.SentParameters parameters,int ? userType = null, string searchValue = null);
        Task Insert(WorkerTraining entity);
        Task Update(WorkerTraining entity);
        Task<WorkerTraining> Get(Guid id);
        Task Delete(WorkerTraining entity);
    }
}
