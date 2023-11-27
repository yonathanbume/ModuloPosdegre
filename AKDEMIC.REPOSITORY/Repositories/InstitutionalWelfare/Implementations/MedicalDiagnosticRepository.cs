using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class MedicalDiagnosticRepository : Repository<MedicalDiagnostic>, IMedicalDiagnosticRepository
    {
        public MedicalDiagnosticRepository(AkdemicContext akdemicContext) : base(akdemicContext)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetMedicalDiagnosticDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.MedicalDiagnostics.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.Contains(searchValue) || x.Code.Contains(searchValue));
            }

            Expression<Func<MedicalDiagnostic, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                id = x.Id,
                code = x.Code,
                description = x.Description
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<object> GetMedicalDiagnosticSelect2ClientSide()
        {
            var result = await _context.MedicalDiagnostics
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.Code}-{x.Description}"
                })
                .ToListAsync();

            return result.OrderBy(x => x.text);
        }


    }
}
