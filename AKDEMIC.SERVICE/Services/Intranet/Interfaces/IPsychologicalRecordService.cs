using AKDEMIC.CORE.Structs;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IPsychologicalRecordService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDetailsDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<object> GetChartReport();
    }
}
