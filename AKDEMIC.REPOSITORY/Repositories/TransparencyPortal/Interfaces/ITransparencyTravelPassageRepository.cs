using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface ITransparencyTravelPassageRepository : IRepository<TransparencyTravelPassage>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTransparencyTravelPassageDataTable(DataTablesStructs.SentParameters sentParameters);
    }
}
