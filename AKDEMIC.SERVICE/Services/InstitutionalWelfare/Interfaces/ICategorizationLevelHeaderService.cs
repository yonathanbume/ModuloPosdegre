using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface ICategorizationLevelHeaderService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCategorizationLevelHeaderDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<CategorizationLevelHeader> Get(Guid id);
        Task Update(CategorizationLevelHeader categorizationLevelHeader);
        Task Insert(CategorizationLevelHeader categorizationLevelHeader);
        Task Delete(CategorizationLevelHeader categorizationLevelHeader);
        Task<IEnumerable<CategorizationLevelHeader>> GetAll();
        Task<object> GetCategorizationLevelHeaderSelect2();
    }
}
