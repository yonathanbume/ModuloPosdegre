using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class PsychologicalDiagnosticService : IPsychologicalDiagnosticService
    {
        private readonly IPsychologicalDiagnosticRepository _psychologicalDiagnosticRepository;

        public PsychologicalDiagnosticService(IPsychologicalDiagnosticRepository psychologicalDiagnosticRepository)
        {
            _psychologicalDiagnosticRepository = psychologicalDiagnosticRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDetailsDatatable(DataTablesStructs.SentParameters sentParameters)
            => await _psychologicalDiagnosticRepository.GetDetailsDatatable(sentParameters);

        public async Task<IEnumerable<Select2Structs.Result>> GetPychologicalDiagnosticsSelect2ClientSide()
            => await _psychologicalDiagnosticRepository.GetPychologicalDiagnosticsSelect2ClientSide();
    }
}
