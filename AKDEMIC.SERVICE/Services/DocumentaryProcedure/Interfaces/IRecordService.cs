using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IRecordService
    {
        Task Insert(Record record);
        Task DeleteByName(string name);
        Task<bool> AnyByName(string value);
    }
}
