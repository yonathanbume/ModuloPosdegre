using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class WelfareCategoryService : IWelfareCategoryService
    {
        private IWelfareCategoryRepository _welfareCategoryRepository;
        public WelfareCategoryService(IWelfareCategoryRepository welfareCategoryRepository)
        {
            _welfareCategoryRepository = welfareCategoryRepository;
        }

        public async Task Delete(WelfareCategory welfareCategory)
        {
            await _welfareCategoryRepository.Delete(welfareCategory);
        }

        public async Task<WelfareCategory> Get(Guid id)
        {
            return await _welfareCategoryRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetWelfareCategories(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _welfareCategoryRepository.GetWelfareCategories(sentParameters,searchValue);
        }

        public async Task Insert(WelfareCategory welfareCategory)
        {
            await _welfareCategoryRepository.Insert(welfareCategory);
        }

        public async Task Update(WelfareCategory welfareCategory)
        {
            await _welfareCategoryRepository.Update(welfareCategory);
        }

        public async Task<object> WelfareCategoriesSelect2(bool hasAll = false)
        {
            return await _welfareCategoryRepository.WelfareCategoriesSelect2(hasAll);
        }
    }
}
