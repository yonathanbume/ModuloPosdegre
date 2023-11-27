using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringPlanService : ITutoringPlanService
    {
        private readonly ITutoringPlanRepository _tutoringPlanRepository;

        public TutoringPlanService(ITutoringPlanRepository tutoringPlanRepository)
        {
            _tutoringPlanRepository = tutoringPlanRepository;
        }

        public async Task DeleteById(Guid tutoringPlanId)
            => await _tutoringPlanRepository.DeleteById(tutoringPlanId);

        public async Task<TutoringPlan> Get(Guid tutoringPlanId)
            => await _tutoringPlanRepository.Get(tutoringPlanId);

        public async Task<IEnumerable<TutoringPlan>> GetAll()
            => await _tutoringPlanRepository.GetAll();

        public async Task<TutoringPlan> GetByCareerId(Guid careerId)
            => await _tutoringPlanRepository.GetByCareerId(careerId);

        public async Task Insert(TutoringPlan tutoringPlan)
            => await _tutoringPlanRepository.Insert(tutoringPlan);

        public async Task Update(TutoringPlan tutoringPlan)
            => await _tutoringPlanRepository.Update(tutoringPlan);

        public async Task<TutoringPlan> GetFirst()
            => await _tutoringPlanRepository.First();

        public async Task<DataTablesStructs.ReturnedData<object>> GetPlansDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _tutoringPlanRepository.GetPlansDatatable(sentParameters, search);


        public async Task<TutoringPlan> GetWithData(Guid id)
            => await _tutoringPlanRepository.GetWithData(id);
        public async Task<bool> AnyByCoordinator(string tutorCoordinatorId)
            => await _tutoringPlanRepository.AnyByCoordinator(tutorCoordinatorId);
        public async Task<TutoringPlan> GetByCoordinator(string tutorCoordinatorId)
            => await _tutoringPlanRepository.GetByCoordinator(tutorCoordinatorId);

        public Task<bool> AnyByCareer(Guid careerId)
            => _tutoringPlanRepository.AnyByCareer(careerId);

        public Task Delete(TutoringPlan tutoringPlan)
            => _tutoringPlanRepository.Delete(tutoringPlan);

        public Task<TutoringPlan> AddAsync(TutoringPlan tutoringPlan)
            => _tutoringPlanRepository.Add(tutoringPlan);

        public Task SaveChangesAsync()
            => _tutoringPlanRepository.SaveChangesAsync();
    }
}
