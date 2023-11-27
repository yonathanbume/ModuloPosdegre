using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class ResearchCategoryService : IResearchCategoryService
    {
        private readonly IResearchCategoryRepository _researchCategoryRepository;
        public ResearchCategoryService(IResearchCategoryRepository researchCategoryRepository)
        {
            _researchCategoryRepository = researchCategoryRepository;
        }
        public async Task<bool> AnyResearchCategoryByName(string name, Guid? id)
        {
            return await _researchCategoryRepository.AnyResearchCategoryByName(name, id);
        }
        public async Task<int> Count()
        {
            return await _researchCategoryRepository.Count();
        }
        public async Task<ResearchCategory> Get(Guid id)
        {
            return await _researchCategoryRepository.Get(id);
        }
        public async Task<object> GetResearchCategory(Guid id)
        {
            return await _researchCategoryRepository.GetResearchCategory(id);
        }
        public async Task<IEnumerable<ResearchCategory>> GetAll()
        {
            return await _researchCategoryRepository.GetAll();
        }
        public async Task<IEnumerable<object>> GetResearchCategories()
        {
            return await _researchCategoryRepository.GetResearchCategories();
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetResearchCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _researchCategoryRepository.GetResearchCategoriesDatatable(sentParameters, searchValue);
        }
        public async Task DeleteById(Guid id)
        {
            await _researchCategoryRepository.DeleteById(id);
        }
        public async Task Insert(ResearchCategory researchCategory)
        {
            await _researchCategoryRepository.Insert(researchCategory);
        }
        public async Task Update(ResearchCategory researchCategory)
        {
            await _researchCategoryRepository.Update(researchCategory);
        }
    }
}
