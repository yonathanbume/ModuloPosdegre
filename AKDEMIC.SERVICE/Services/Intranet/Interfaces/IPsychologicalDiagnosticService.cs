using AKDEMIC.CORE.Structs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IPsychologicalDiagnosticService
    {
        Task<IEnumerable<Select2Structs.Result>> GetPychologicalDiagnosticsSelect2ClientSide();
        Task<DataTablesStructs.ReturnedData<object>> GetDetailsDatatable(DataTablesStructs.SentParameters sentParameters);
    }
}
