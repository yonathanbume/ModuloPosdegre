using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IWelfareCategoryRepository : IRepository<WelfareCategory>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetWelfareCategories(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> WelfareCategoriesSelect2(bool hasAll = false);
    }
}
