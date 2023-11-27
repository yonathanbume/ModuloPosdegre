using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class EvaluationTypeService : IEvaluationTypeService
    {
        private readonly IEvaluationTypeRepository _evaluationTypeRepository;

        public EvaluationTypeService(IEvaluationTypeRepository evaluationTypeRepository)
        {
            _evaluationTypeRepository = evaluationTypeRepository;
        }

        public async Task Delete(EvaluationType evaluationType) => await _evaluationTypeRepository.Delete(evaluationType);

        public async Task<EvaluationType> Get(Guid id) => await _evaluationTypeRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _evaluationTypeRepository.GetDataDatatable(sentParameters, searchValue);

        public async Task Insert(EvaluationType evaluationType) => await _evaluationTypeRepository.Insert(evaluationType);

        public async Task Update(EvaluationType evaluationType) => await _evaluationTypeRepository.Update(evaluationType);

        public async Task<object> GetEvaluationTypeJson()
            => await _evaluationTypeRepository.GetEvaluationTypeJson();

        public async Task<bool> AnyEvaluation(Guid id, Guid? termId = null)
            => await _evaluationTypeRepository.AnyEvaluation(id, termId);       
    }
}
