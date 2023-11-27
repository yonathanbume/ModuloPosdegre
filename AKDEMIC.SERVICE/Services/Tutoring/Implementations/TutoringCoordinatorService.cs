using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringCoordinatorService : ITutoringCoordinatorService
    {
        private readonly ITutoringCoordinatorRepository _tutoringCoordinatorRepository;

        public TutoringCoordinatorService(ITutoringCoordinatorRepository tutoringCoordinatorRepository)
        {
            _tutoringCoordinatorRepository = tutoringCoordinatorRepository;
        }

        public async Task Delete(TutoringCoordinator tutoringCoordinator)
            => await _tutoringCoordinatorRepository.Delete(tutoringCoordinator);

        public async Task DeleteById(string tutoringCoordinatorId)
            => await _tutoringCoordinatorRepository.DeleteById(tutoringCoordinatorId);

        public async Task<TutoringCoordinator> Get(string tutoringCoordinatorId)
            => await _tutoringCoordinatorRepository.Get(tutoringCoordinatorId);

        public async Task<TutoringCoordinator> GetByCareerId(Guid careerId)
            => await _tutoringCoordinatorRepository.GetByCareerId(careerId);
        public async Task<DataTablesStructs.ReturnedData<TutoringCoordinator>> GetTutoringCoordinatorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? careerId = null)
            => await _tutoringCoordinatorRepository.GetTutoringCoordinatorsDatatable(sentParameters, searchValue, careerId);

        public async Task<TutoringCoordinator> GetWithUserById(string tutoringCoordinatorId)
            => await _tutoringCoordinatorRepository.GetWithUserById(tutoringCoordinatorId);

        public async Task Insert(TutoringCoordinator tutoringCoordinator)
            => await _tutoringCoordinatorRepository.Insert(tutoringCoordinator);

        public async Task Update(TutoringCoordinator tutoringCoordinator)
            => await _tutoringCoordinatorRepository.Update(tutoringCoordinator);

        public async Task<bool> Any(string tutorId)
            => await _tutoringCoordinatorRepository.Any(tutorId);
        public async Task<bool> AnyByCareerId(Guid careerId)
            => await _tutoringCoordinatorRepository.AnyByCareerId(careerId);
        public SelectList GetTypeTime()
            => _tutoringCoordinatorRepository.GetTypeTime();
        public async Task<TutoringCoordinator> GetWithData(string id)
            => await _tutoringCoordinatorRepository.GetWithData(id);

        public Task<TutoringCoordinator> Add(TutoringCoordinator tutoringCoordinator)
            => _tutoringCoordinatorRepository.Add(tutoringCoordinator);

        public Task<string> GetCareerByUserId(string userId)
            => _tutoringCoordinatorRepository.GetCareerByUserId(userId);
    }
}
