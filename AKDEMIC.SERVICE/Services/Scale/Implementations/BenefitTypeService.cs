using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class BenefitTypeService: IBenefitTypeService
    {
        private readonly IBenefitTypeRepository _benefitTypeRepository;

        public BenefitTypeService(IBenefitTypeRepository benefitTypeRepository)
        {
            _benefitTypeRepository = benefitTypeRepository;
        }

        public async Task<bool> AnyByName(string name, Guid? id = null)
            => await _benefitTypeRepository.AnyByName(name,id);

        public async Task Delete(BenefitType benefitType)
            => await _benefitTypeRepository.Delete(benefitType);

        public async Task<BenefitType> Get(Guid id)
            => await _benefitTypeRepository.Get(id);

        public async Task<IEnumerable<BenefitType>> GetAll()
            => await _benefitTypeRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllBenefitsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _benefitTypeRepository.GetAllBenefitsDatatable(sentParameters,searchValue);

        public async Task Insert(BenefitType benefitType)
            => await _benefitTypeRepository.Insert(benefitType);

        public async Task Update(BenefitType benefitType)
            => await _benefitTypeRepository.Update(benefitType);
    }
}
