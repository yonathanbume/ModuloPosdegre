using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class CategorizationLevelHeaderRepository : Repository<CategorizationLevelHeader>, ICategorizationLevelHeaderRepository
    {
        public CategorizationLevelHeaderRepository(AkdemicContext context) : base(context)
        {
                
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCategorizationLevelHeaderDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.CategorizationLevelHeaders.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.ToLower().Contains(searchValue.ToLower()));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new { x.Id, x.Description, x.Abbreviation }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetCategorizationLevelHeaderSelect2()
        {
            var result = await _context.CategorizationLevelHeaders.Select(x => new
            {
                x.Id,
                Text = x.Description
            }).ToListAsync();
            return result;
        }
    }
}
