using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class CategorizationLevelRepository : Repository<CategorizationLevel>, ICategorizationLevelRepository
    {
        public CategorizationLevelRepository(AkdemicContext context): base(context)
        {

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCategorizationLevelDatatable(DataTablesStructs.SentParameters sentParameters, Guid categorizationLevelHeaderId, string searchValue = null)
        {
            var query = _context.CategorizationLevels.Where(x=>x.CategorizationLevelHeaderId == categorizationLevelHeaderId).AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new { x.Id, x.Name, x.Min, x.Max }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> ValidateValues(Guid categorizationLevelHeaderId, int min, int max, Guid? categorizationLevelId)
        {
            var validate = true;
            var query =  _context.CategorizationLevels.Where(x => x.CategorizationLevelHeaderId == categorizationLevelHeaderId).AsQueryable();
            if (categorizationLevelId != null)
            {
                query = query.Where(x => x.Id != categorizationLevelId);
            }
            var listQuery = await query.ToListAsync();
            foreach (var item in listQuery)
            {
                if ((min <= item.Max && min >= item.Min) || (max <= item.Max && max >= item.Min))
                {
                    validate = false;
                    break;
                }               
            }
            return validate;
        }

        public async Task<CategorizationLevel> FirstElementWithMax(Guid categorizationLevelHeaderId, Guid? avoidId = null)
        {
            var query = _context.CategorizationLevels.Where(x=>x.CategorizationLevelHeaderId == categorizationLevelHeaderId).AsQueryable();
            if (avoidId.HasValue)
            {
                query = query.Where(x => x.Id != avoidId);
            }
            
             return await query.OrderByDescending(x => x.Max).FirstOrDefaultAsync();
        }
    }
}
