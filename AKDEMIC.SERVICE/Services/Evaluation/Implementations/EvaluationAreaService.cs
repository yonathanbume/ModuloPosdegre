using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class EvaluationAreaService : IEvaluationAreaService
    {
        private readonly IEvaluationAreaRepository _evaluationAreaRepository;

        public EvaluationAreaService(IEvaluationAreaRepository evaluationAreaRepository)
        {
            _evaluationAreaRepository = evaluationAreaRepository;
        }

        public async Task<bool> AnyByAreaId(Guid areaId)
            => await _evaluationAreaRepository.AnyByAreaId(areaId);

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _evaluationAreaRepository.AnyByName(name, ignoredId);

        public async Task Delete(EvaluationArea entity)
            => await _evaluationAreaRepository.Delete(entity);

        public async Task<EvaluationArea> Get(Guid id)
            => await _evaluationAreaRepository.Get(id);

        public async Task<IEnumerable<Select2Structs.Result>> GetAreasSelect2ClientSide()
            => await _evaluationAreaRepository.GetAreasSelect2ClientSide();

        public async Task<DataTablesStructs.ReturnedData<EvaluationArea>> GetEvaluationDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _evaluationAreaRepository.GetEvaluationDatatable(sentParameters, search);

        public async Task Insert(EvaluationArea entity)
            => await _evaluationAreaRepository.Insert(entity);

        public async Task Update(EvaluationArea entity)
            => await _evaluationAreaRepository.Update(entity);
    }
}
