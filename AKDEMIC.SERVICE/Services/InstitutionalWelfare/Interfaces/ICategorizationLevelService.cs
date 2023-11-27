using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface ICategorizationLevelService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCategorizationLevelDatatable(DataTablesStructs.SentParameters sentParameters, Guid categorizationLevelHeaderId, string searchValue = null);        
        Task<CategorizationLevel> Get(Guid id);
        Task Update(CategorizationLevel studentRubricItem);
        Task Insert(CategorizationLevel studentRubricItem);
        Task Delete(CategorizationLevel studentRubricItem);
        Task<IEnumerable<CategorizationLevel>> GetAll();
        Task<bool> ValidateValues(Guid categorizationLevelHeaderId, int min, int max, Guid? categorizationLevelId = null);
        Task<CategorizationLevel> FirstElementWithMax(Guid categorizationLevelHeaderId, Guid? avoidId = null );

    }
}
