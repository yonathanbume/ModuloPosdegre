using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IEvaluationRepository _evaluationRepository;

        public EvaluationService(IEvaluationRepository evaluationRepository)
        {
            _evaluationRepository = evaluationRepository;
        }

        public Task DeleteAsync(ENTITIES.Models.Enrollment.Evaluation evaluation)
            => _evaluationRepository.Delete(evaluation);

        public async Task DeleteById(Guid id)
        {
            await _evaluationRepository.DeleteById(id);
        }

        public Task DeleteRangeAsync(IEnumerable<ENTITIES.Models.Enrollment.Evaluation> evaluations)
            => _evaluationRepository.DeleteRange(evaluations);

        public async Task<int> GenerateUnitsAndEvaluationsJob()
        {
            return await _evaluationRepository.GenerateUnitsAndEvaluationsJob();
        }

        public async Task<ENTITIES.Models.Enrollment.Evaluation> Get(Guid id) =>
            await _evaluationRepository.Get(id);

        public Task<object> GetAllAsModelA(Guid? termId = null, Guid? courseId = null)
            => _evaluationRepository.GetAllAsModelA(termId, courseId);

        public async Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseAndTerm(Guid courseId, Guid termId) =>
            await _evaluationRepository.GetAllByCourseAndTerm(courseId, termId);

        public async Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>>
            GetAllByCourseAndTermWithTaken(Guid courseId, Guid termId, Guid sectionId) =>
            await _evaluationRepository.GetAllByCourseAndTermWithTaken(courseId, termId, sectionId);

        public async Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseTerm(Guid courseTermId)
            => await _evaluationRepository.GetAllByCourseTerm(courseTermId);

        public async Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseUnit(Guid courseUnitId)
            => await _evaluationRepository.GetAllByCourseUnit(courseUnitId);

        public Task<object> GetAsModelB(Guid? id = null)
            => _evaluationRepository.GetAsModelB(id);

        public async Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetEvaluationsByClass(Guid classId)
            => await _evaluationRepository.GetEvaluationsByClass(classId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEvaluationsDatatable(DataTablesStructs.SentParameters parameters, Guid termId, Guid courseId)
            => await _evaluationRepository.GetEvaluationsDatatable(parameters, termId, courseId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEvaluationsDatatableByTeacherConfiguration(DataTablesStructs.SentParameters parameters, Guid termId, Guid courseId, Guid sectionId)
            => await _evaluationRepository.GetEvaluationsDatatableByTeacherConfiguration(parameters, termId, courseId, sectionId);

        public Task InsertAsync(ENTITIES.Models.Enrollment.Evaluation evaluation)
            => _evaluationRepository.Insert(evaluation);

        public Task InsertRangeAsync(IEnumerable<ENTITIES.Models.Enrollment.Evaluation> evaluations)
            => _evaluationRepository.InsertRange(evaluations);

        public Task UpdateAsync(ENTITIES.Models.Enrollment.Evaluation evaluation)
            => _evaluationRepository.Update(evaluation);

        public Task UpdateRangeAsync(IEnumerable<ENTITIES.Models.Enrollment.Evaluation> evaluations)
            => _evaluationRepository.UpdateRange(evaluations);

      
    }
}