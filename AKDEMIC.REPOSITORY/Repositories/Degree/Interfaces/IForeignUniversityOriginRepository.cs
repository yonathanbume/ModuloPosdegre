using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Degree.Interfaces
{
    public interface IForeignUniversityOriginRepository:IRepository<ForeignUniversityOrigin>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetForeignUniveristyOriginDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> GetSelect2();
    }
}
