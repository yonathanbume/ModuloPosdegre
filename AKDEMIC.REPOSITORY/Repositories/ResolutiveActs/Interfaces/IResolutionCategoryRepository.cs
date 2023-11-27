using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Interfaces
{
    public interface IResolutionCategoryRepository : IRepository<ResolutionCategory>
    {
        Task<DataTablesStructs.ReturnedData<ResolutionCategory>> GetResolutionCategoryDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<IEnumerable<Select2Structs.Result>> GetResolutionCategorySelect2ClientSide();
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
    }
}
