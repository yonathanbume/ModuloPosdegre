using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IRecordRepository : IRepository<Record>
    {
        Task DeleteByName(string name);
        Task<bool> AnyByName(string value);
    }
}
