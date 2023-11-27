using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class PsychologicalDiagnosticRepository : Repository<PsychologicalDiagnostic>, IPsychologicalDiagnosticRepository
    {
        public PsychologicalDiagnosticRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<Select2Structs.Result>> GetPychologicalDiagnosticsSelect2ClientSide()
        {
            var result = await _context.PsychologicalDiagnostics
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Description
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDetailsDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            var query = _context.PsychologicalDiagnostics.Include(x=>x.PsychologicRecordDiagnostic)
                .AsEnumerable()
                .Where(x=>x.PsychologicRecordDiagnostic.Any(y=>y.IsCurrent)).ToList();

            int recordsFiltered =  query.Count();

            var data =  query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    Name = x.Description,
                    Count = x.PsychologicRecordDiagnostic.Count()
                })
                .ToList();


            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
