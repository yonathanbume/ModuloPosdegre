using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareDisabilityService : IInstitutionalWelfareDisabilityService
    {
        private readonly IInstitutionalWelfareDisabilityRepository _institutionalWelfareDisabilityRepository;

        public InstitutionalWelfareDisabilityService(IInstitutionalWelfareDisabilityRepository institutionalWelfareDisabilityRepository)
        {
            _institutionalWelfareDisabilityRepository = institutionalWelfareDisabilityRepository;
        }
        public Task Delete(Disability disability)
            => _institutionalWelfareDisabilityRepository.Delete(disability);

        public async Task<bool> AnyByCode(string code, Guid? id = null)
    => await _institutionalWelfareDisabilityRepository.AnyByCode(code, id);

        public Task<Disability> Get(Guid id)
            => _institutionalWelfareDisabilityRepository.Get(id);

        public Task<IEnumerable<Disability>> GetAll()
            => _institutionalWelfareDisabilityRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDisabilitiesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
=>             _institutionalWelfareDisabilityRepository.GetAllDisabilitiesDatatable(sentParameters, searchValue);

        public Task Insert(Disability disability)
            => _institutionalWelfareDisabilityRepository.Insert(disability);

        public Task Update(Disability disability)
            => _institutionalWelfareDisabilityRepository.Update(disability);
    }
}
