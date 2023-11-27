using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IPsychologicalDiagnosticRepository : IRepository<PsychologicalDiagnostic>
    {
        Task<IEnumerable<Select2Structs.Result>> GetPychologicalDiagnosticsSelect2ClientSide();
        Task<DataTablesStructs.ReturnedData<object>> GetDetailsDatatable(DataTablesStructs.SentParameters sentParameters);
    }
}
