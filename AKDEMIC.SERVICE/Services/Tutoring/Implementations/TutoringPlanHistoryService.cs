using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringPlanHistoryService : ITutoringPlanHistoryService
    {
        private readonly ITutoringPlanHistoryRepository _tutoringPlanHistoryRepository;

        public TutoringPlanHistoryService(ITutoringPlanHistoryRepository tutoringPlanHistoryRepository)
        {
            _tutoringPlanHistoryRepository = tutoringPlanHistoryRepository;
        }
        public Task<TutoringPlanHistory> AddAsync(TutoringPlanHistory tutoringPlanHistory)
            => _tutoringPlanHistoryRepository.Add(tutoringPlanHistory);

        public Task Delete(TutoringPlanHistory tutoringPlanHistory)
            => _tutoringPlanHistoryRepository.Delete(tutoringPlanHistory);

        public Task<TutoringPlanHistory> Get(Guid id)
            => _tutoringPlanHistoryRepository.Get(id);

        public Task<IEnumerable<TutoringPlanHistory>> GetAll()
            => _tutoringPlanHistoryRepository.GetAll();

        public Task Insert(TutoringPlanHistory tutoringPlanHistory)
            => _tutoringPlanHistoryRepository.Insert(tutoringPlanHistory);

        public Task SaveChangesAsync()
            => _tutoringPlanHistoryRepository.SaveChangesAsync();

        public Task<List<TutoringPlanHistory>> GetHistoriesByTutoringPlan(Guid tutoringPlanId)
            => _tutoringPlanHistoryRepository.GetHistoriesByTutoringPlan(tutoringPlanId);

        public Task Update(TutoringPlanHistory tutoringPlanHistory)
            => _tutoringPlanHistoryRepository.Update(tutoringPlanHistory);

        public Task DeleteRange(List<TutoringPlanHistory> tutoringPlanHistories)
            => _tutoringPlanHistoryRepository.DeleteRange(tutoringPlanHistories);
    }
}
