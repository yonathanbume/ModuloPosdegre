using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Implementations
{
    public class TransparencyPortalInterestLinkRepository : Repository<TransparencyPortalInterestLink>, ITransparencyPortalInterestLinkRepository
    {
        public TransparencyPortalInterestLinkRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search, byte? type = null)
        {
            var query = _context.TransparencyPortalInterestLinks.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Text.ToLower().Trim().Contains(search.ToLower().Trim()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
              .Skip(sentParameters.PagingFirstRecord)
              .Take(sentParameters.RecordsPerDraw)
              .Select(x => new
              {
                  x.Id,
                  x.Text,
                  x.Icon,
                  x.Url
              })
              .ToListAsync();

            var recordsTotal = data.Count;

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
