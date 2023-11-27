using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.WorkerLaborInformation;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerLaborInformationService : IWorkerLaborInformationService
    {
        private readonly IWorkerLaborInformationRepository _workerLaborInformationRepository;

        public WorkerLaborInformationService(IWorkerLaborInformationRepository workerLaborInformationRepository)
        {
            _workerLaborInformationRepository = workerLaborInformationRepository;
        }

        public async Task<WorkerLaborInformation> GetByUserId(string userId)
        {
            return await _workerLaborInformationRepository.GetByUserId(userId);
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetLaborUserDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _workerLaborInformationRepository.GetLaborUserDatatable(sentParameters, searchValue);

        public async Task<List<LaborUserTemplate>> GetUserLaborReport(string search)
        {
            return await _workerLaborInformationRepository.GetUserLaborReport(search);
        }

        public async Task Update(WorkerLaborInformation workerLaborInformation)
            => await _workerLaborInformationRepository.Update(workerLaborInformation);

        public async Task<WorkerLaborInformation> Add(WorkerLaborInformation workerLaborInformation)
            => await _workerLaborInformationRepository.Add(workerLaborInformation);

        public async Task Delete(WorkerLaborInformation workerLaborInformation)
            => await _workerLaborInformationRepository.Delete(workerLaborInformation);

        public async Task Insert(WorkerLaborInformation workerLaborInformation)
            => await _workerLaborInformationRepository.Insert(workerLaborInformation);

        public async Task<bool> AnyPlaceCode(string placeCode,string userId = null)
            => await _workerLaborInformationRepository.AnyPlaceCode(placeCode,userId);
        

        public Task<object> GetUserDetailInformation(string userId)
            => _workerLaborInformationRepository.GetUserDetailInformation(userId);

        public void Remove(WorkerLaborInformation workerLaborInformation)
            => _workerLaborInformationRepository.Remove(workerLaborInformation); 
    }
}
