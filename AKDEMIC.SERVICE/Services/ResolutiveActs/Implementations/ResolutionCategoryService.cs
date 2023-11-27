using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Interfaces;
using AKDEMIC.SERVICE.Services.ResolutiveActs.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ResolutiveActs.Implementations
{
    public class ResolutionCategoryService : IResolutionCategoryService
    {
        private readonly IResolutionCategoryRepository _resolutionCategoryRepository;

        public ResolutionCategoryService(IResolutionCategoryRepository resolutionCategoryRepository)
        {
            _resolutionCategoryRepository = resolutionCategoryRepository;
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _resolutionCategoryRepository.AnyByName(name, ignoredId);

        public async Task Delete(ResolutionCategory entity)
            => await _resolutionCategoryRepository.Delete(entity);

        public async Task<ResolutionCategory> Get(Guid id)
            => await _resolutionCategoryRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<ResolutionCategory>> GetResolutionCategoryDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _resolutionCategoryRepository.GetResolutionCategoryDatatable(sentParameters, search);

        public async Task<IEnumerable<Select2Structs.Result>> GetResolutionCategorySelect2ClientSide()
            => await _resolutionCategoryRepository.GetResolutionCategorySelect2ClientSide();

        public async Task Insert(ResolutionCategory entity)
            => await _resolutionCategoryRepository.Insert(entity);

        public async Task Update(ResolutionCategory entity)
            => await _resolutionCategoryRepository.Update(entity);
    }
}
