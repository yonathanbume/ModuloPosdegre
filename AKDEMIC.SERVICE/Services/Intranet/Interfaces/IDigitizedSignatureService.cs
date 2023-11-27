using AKDEMIC.ENTITIES.Models.Intranet;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IDigitizedSignatureService
    {
        Task Insert(DigitizedSignature entity);
        Task Update(DigitizedSignature entity);
        Task<DigitizedSignature> GetFirst();
    }
}
