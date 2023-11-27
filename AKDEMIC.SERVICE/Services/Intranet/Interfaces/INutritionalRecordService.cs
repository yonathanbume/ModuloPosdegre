using AKDEMIC.CORE.Structs;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface INutritionalRecordService
    {
        Task<object> GetReportWaist(decimal n_minimo, decimal n_maximo);
        Task<object> GetDatatableArmsDetail(DataTablesStructs.SentParameters sentParameters, decimal n_minimo, decimal n_maximo);
    }
}
