using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class CulturalActivityTypeService : ICulturalActivityTypeService
    {
        private readonly ICulturalActivityTypeRepository _culturalActivityTypeRepository;

        public CulturalActivityTypeService(ICulturalActivityTypeRepository culturalActivityTypeRepository)
        {
            _culturalActivityTypeRepository = culturalActivityTypeRepository;
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _culturalActivityTypeRepository.AnyByName(name, ignoredId);

        public async Task<bool> AnyByTypeId(Guid typeId)
            => await _culturalActivityTypeRepository.AnyByTypeId(typeId);

        public async Task Delete(CulturalActivityType entity)
            => await _culturalActivityTypeRepository.Delete(entity);

        public async Task<CulturalActivityType> Get(Guid id)
            => await _culturalActivityTypeRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<CulturalActivityType>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _culturalActivityTypeRepository.GetDatatable(sentParameters, search);

        public async Task<IEnumerable<Select2Structs.Result>> GetTypeSelect2ClientSide()
            => await _culturalActivityTypeRepository.GetTypeSelect2ClientSide();

        public async Task Insert(CulturalActivityType entity)
            => await _culturalActivityTypeRepository.Insert(entity);

        public async Task Update(CulturalActivityType entity)
            => await _culturalActivityTypeRepository.Update(entity);
    }
}
