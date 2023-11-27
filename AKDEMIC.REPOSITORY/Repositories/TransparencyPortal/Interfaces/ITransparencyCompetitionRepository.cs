using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface ITransparencyCompetitionRepository : IRepository<TransparencyCompetition>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetListCompetition(DataTablesStructs.SentParameters paginationParameter, int year);
    }
}
