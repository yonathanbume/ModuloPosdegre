using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class CriterionService : ICriterionService
    {
        private readonly ICriterionRepository _criterionRepository;

        public CriterionService(ICriterionRepository criterionRepository)
        {
            _criterionRepository = criterionRepository;
        }

        public async Task Delete(Criterion entity)
            => await _criterionRepository.Delete(entity);

        public async Task<bool> ExistCode(string code,Guid? currentCriterionId = null)
            => await _criterionRepository.ExistCode( code,currentCriterionId);

        public async Task<bool> ExistName(string name,Guid? currentCriterionId = null)
            => await _criterionRepository.ExistName( name,currentCriterionId);

        public async Task<Criterion> Get(Guid id)
            => await _criterionRepository.Get(id);

        public Task<Select2Structs.ResponseParameters> GetCriterionsByAcademicProgramIdSelect2(Select2Structs.RequestParameters requestParameters, string keyname)
        {
            return _criterionRepository.GetCriterionsByAcademicProgramIdSelect2(requestParameters,keyname);
        }

        public async Task<DataTablesStructs.ReturnedData<Criterion>> GetCriterionsDatatable(DataTablesStructs.SentParameters sentParameters, string name = null,string userId = null, string searchValue = null)
            => await _criterionRepository.GetCriterionsDatatable(sentParameters, name,userId, searchValue);

        public async Task Insert(Criterion entity)
            => await _criterionRepository.Insert(entity);

        public async Task Update(Criterion entity)
            => await _criterionRepository.Update(entity);
    }
}
