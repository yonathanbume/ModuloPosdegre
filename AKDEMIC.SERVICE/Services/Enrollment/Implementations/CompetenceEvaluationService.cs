using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CompetenceEvaluationService : ICompetenceEvaluationService
    {
        private readonly ICompetenceEvaluationRepository _competenceEvaluationRepository;

        public CompetenceEvaluationService(ICompetenceEvaluationRepository competenceEvaluationRepository)
        {
            _competenceEvaluationRepository = competenceEvaluationRepository;
        }

        public async Task Delete(CompetenceEvaluation competenceEvaluation) => await _competenceEvaluationRepository.Delete(competenceEvaluation);

        public async Task DeleteById(Guid id) => await _competenceEvaluationRepository.DeleteById(id);

        public async Task DeleteRange(IEnumerable<CompetenceEvaluation> competenceEvaluations) => await _competenceEvaluationRepository.DeleteRange(competenceEvaluations);

        public async Task<CompetenceEvaluation> Get(Guid id) => await _competenceEvaluationRepository.Get(id);

        public async Task<IEnumerable<CompetenceEvaluation>> GetAll() => await _competenceEvaluationRepository.GetAll();

        public async Task Insert(CompetenceEvaluation competenceEvaluation) => await _competenceEvaluationRepository.Insert(competenceEvaluation);

        public async Task InsertRange(IEnumerable<CompetenceEvaluation> competenceEvaluations) => await _competenceEvaluationRepository.InsertRange(competenceEvaluations);

        public async Task Update(CompetenceEvaluation competenceEvaluation) => await _competenceEvaluationRepository.Update(competenceEvaluation);
    }
}
