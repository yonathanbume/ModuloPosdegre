using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerBankAccountInformationService
    {
        //Generales
        Task<WorkerBankAccountInformation> Get(Guid id);
        Task<IEnumerable<WorkerBankAccountInformation>> GetAll();
        Task<List<WorkerBankAccountInformation>> GetAllByWorker(Guid workerLaborInformationId);
        void RemoveRange(List<WorkerBankAccountInformation> workerBankAccountInformations);
        Task AddRange(List<WorkerBankAccountInformation> workerBankAccountInformations);
        Task Insert(WorkerBankAccountInformation workerBankAccountInformation);
        Task Update(WorkerBankAccountInformation workerBankAccountInformation);
        Task Delete(WorkerBankAccountInformation workerBankAccountInformation);
    }
}
