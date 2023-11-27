using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces
{
    public interface ICategorizationLevelHeaderRepository : IRepository<CategorizationLevelHeader>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCategorizationLevelHeaderDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> GetCategorizationLevelHeaderSelect2();
    }
}
