using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IWelfareCategoryService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetWelfareCategories(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> WelfareCategoriesSelect2(bool hasAll = false);
        Task<WelfareCategory> Get(Guid id);
        Task Update(WelfareCategory welfareCategory);
        Task Insert(WelfareCategory welfareCategory);
        Task Delete(WelfareCategory welfareCategory);

    }
}
