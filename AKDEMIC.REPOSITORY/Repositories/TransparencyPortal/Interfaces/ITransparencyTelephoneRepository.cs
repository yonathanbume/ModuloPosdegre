using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface ITransparencyTelephoneRepository : IRepository<TransparencyTelephone>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTransparencyTelephoneDataTable(DataTablesStructs.SentParameters sentParameters);
    }
}
