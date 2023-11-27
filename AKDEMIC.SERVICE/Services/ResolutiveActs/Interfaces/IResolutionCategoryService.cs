using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ResolutiveActs.Interfaces
{
    public interface IResolutionCategoryService
    {
        Task<DataTablesStructs.ReturnedData<ResolutionCategory>> GetResolutionCategoryDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<IEnumerable<Select2Structs.Result>> GetResolutionCategorySelect2ClientSide();
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task Insert(ResolutionCategory entity);
        Task Delete(ResolutionCategory entity);
        Task<ResolutionCategory> Get(Guid id);
        Task Update(ResolutionCategory entity);
    }
}
