using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IResearchSubAreaService
    {
        Task<bool> AnyResearchSubAreaByName(string name, Guid? id);
        Task<int> Count();
        Task<ResearchSubArea> Get(Guid id);
        Task<object> GetResearchSubArea(Guid id);
        Task<IEnumerable<ResearchSubArea>> GetAll();
        Task<IEnumerable<object>> GetResearchSubAreas();
        Task<DataTablesStructs.ReturnedData<object>> GetResearchSubAreasDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task DeleteById(Guid id);
        Task Insert(ResearchSubArea researchSubArea);
        Task Update(ResearchSubArea researchSubArea);
    }
}
