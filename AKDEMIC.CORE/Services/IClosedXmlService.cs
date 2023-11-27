using AKDEMIC.CORE.Structs;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public interface IClosedXmlService
    {
        Task GenerateBasicXml<T>(ClosedXmlStruct<T> xmlStruct);
    }
}