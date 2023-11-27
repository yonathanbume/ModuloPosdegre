using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IUITRepository : IRepository<UIT>
    {
        Task<bool> AnyUITByYear(int year);
        Task<UIT> GetCurrentUIT();
        Task<DataTablesStructs.ReturnedData<UIT>> GetUITsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
