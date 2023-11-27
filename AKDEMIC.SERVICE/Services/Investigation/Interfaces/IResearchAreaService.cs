using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IResearchAreaService
    {
        Task<bool> AnyResearchAreaByName(string name, Guid? id);
        Task<int> Count();
        Task<ResearchArea> Get(Guid id);
        Task<object> GetResearchArea(Guid id);
        Task<IEnumerable<ResearchArea>> GetAll();
        Task<IEnumerable<object>> GetResearchAreas();
        Task<DataTablesStructs.ReturnedData<object>> GetResearchAreasDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task DeleteById(Guid id);
        Task Insert(ResearchArea researchArea);
        Task Update(ResearchArea researchArea);
    }
}
