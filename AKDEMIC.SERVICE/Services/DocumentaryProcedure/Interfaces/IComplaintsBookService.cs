using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IComplaintsBookService
    {
        Task Insert(Complaint newComplaint);
    }
}
