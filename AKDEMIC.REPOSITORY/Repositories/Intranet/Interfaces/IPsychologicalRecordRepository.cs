using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IPsychologicalRecordRepository : IRepository<PsychologicalRecord>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDetailsDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<object> GetChartReport();
    }
}
