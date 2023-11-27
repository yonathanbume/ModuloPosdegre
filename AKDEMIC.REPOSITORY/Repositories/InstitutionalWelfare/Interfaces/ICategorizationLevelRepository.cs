using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces
{
    public interface ICategorizationLevelRepository : IRepository<CategorizationLevel>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCategorizationLevelDatatable(DataTablesStructs.SentParameters sentParameters, Guid categorizationLevelHeaderId, string searchValue = null);
        Task<bool> ValidateValues(Guid categorizationLevelHeaderId, int min, int max, Guid ? categorizationLevelId);
        Task<CategorizationLevel> FirstElementWithMax(Guid categorizationLevelHeaderId, Guid? avoidId = null);


    }
}
