using AKDEMIC.CORE.Structs;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface INutritionalRecordRepository
    {
        Task<object> GetReportWaist(decimal n_minimo, decimal n_maximo);
        Task<object> GetDatatableArmsDetail(DataTablesStructs.SentParameters sentParameters, decimal n_minimo, decimal n_maximo);
    }
}
