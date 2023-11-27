using AKDEMIC.ENTITIES.Models.Geo;
using AKDEMIC.REPOSITORY.Repositories.Geo.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Geo.Templates;
using AKDEMIC.SERVICE.Services.Geo.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Geo.Implementations
{
    public class LaboratoryRequestService : ILaboratoryRequestService
    {
        private readonly ILaboratoryRequestRepository _laboratoyRequestRepository;

        public LaboratoryRequestService(ILaboratoryRequestRepository laboratoyRequestRepository)
        {
            _laboratoyRequestRepository = laboratoyRequestRepository;
        }

        public async Task InsertLaboratoyRequest(LaboratoyRequest laboratoyRequest) =>
            await _laboratoyRequestRepository.Insert(laboratoyRequest);

        public async Task UpdateLaboratoyRequest(LaboratoyRequest laboratoyRequest) =>
            await _laboratoyRequestRepository.Update(laboratoyRequest);

        public async Task DeleteLaboratoyRequest(LaboratoyRequest laboratoyRequest) =>
            await _laboratoyRequestRepository.Delete(laboratoyRequest);

        public async Task<LaboratoyRequest> GetLaboratoyRequestById(Guid id) =>
            await _laboratoyRequestRepository.Get(id);

        public async Task<IEnumerable<LaboratoyRequest>> GetaLLLaboratoyRequests() =>
            await _laboratoyRequestRepository.GetAll();

        public async Task<ATSTemplate> GetATS(Guid id)
            => await _laboratoyRequestRepository.GetATS(id);

        public async Task<object> GetRequestsByUser(string userId)
            => await _laboratoyRequestRepository.GetRequestsByUser(userId);
    }
}
