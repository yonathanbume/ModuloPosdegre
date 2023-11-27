using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IResearchCategoryService
    {
        Task<bool> AnyResearchCategoryByName(string name, Guid? id);
        Task<int> Count();
        Task<ResearchCategory> Get(Guid id);
        Task<object> GetResearchCategory(Guid id);
        Task<IEnumerable<ResearchCategory>> GetAll();
        Task<IEnumerable<object>> GetResearchCategories();
        Task<DataTablesStructs.ReturnedData<object>> GetResearchCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task DeleteById(Guid id);
        Task Insert(ResearchCategory researchCategory);
        Task Update(ResearchCategory researchCategory);
    }
}
