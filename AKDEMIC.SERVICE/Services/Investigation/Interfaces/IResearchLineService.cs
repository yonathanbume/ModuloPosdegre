using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IResearchLineService
    {
        Task<bool> AnyResearchLineByName(string name, Guid? id);
        Task<int> Count();
        Task<ResearchLine> Get(Guid id);
        Task<object> GetResearchLine(Guid id);
        Task<IEnumerable<ResearchLine>> GetAll();
        Task<IEnumerable<object>> GetResearchLines();
        Task<DataTablesStructs.ReturnedData<object>> GetResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId, Guid? categoryId, Guid? disciplineId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCompanyResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, string userId);
        Task DeleteById(Guid id);
        Task Insert(ResearchLine researchLine);
        Task Update(ResearchLine researchLine);
    }
}
