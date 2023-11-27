using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IResearchDisciplineService
    {
        Task<bool> AnyResearchDisciplineByName(string name, Guid? id);
        Task<int> Count();
        Task<ResearchDiscipline> Get(Guid id);
        Task<object> GetResearchDiscipline(Guid id);
        Task<IEnumerable<ResearchDiscipline>> GetAll();
        Task<IEnumerable<object>> GetResearchDisciplines();
        Task<DataTablesStructs.ReturnedData<object>> GetResearchDisciplinesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? areaId, Guid? subareaId, string searchValue = null);
        Task DeleteById(Guid id);
        Task Insert(ResearchDiscipline researchDiscipline);
        Task Update(ResearchDiscipline researchDiscipline);
    }
}
