using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IExtracurricularAreaRepository : IRepository<ExtracurricularArea>
    {
        Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);

    }
}
