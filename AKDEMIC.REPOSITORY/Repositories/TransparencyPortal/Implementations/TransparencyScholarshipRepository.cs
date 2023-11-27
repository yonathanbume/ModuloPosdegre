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
    public class TransparencyScholarshipRepository : Repository<TransparencyScholarship> , ITransparencyScholarshipRepository
    {
        public TransparencyScholarshipRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetScholarshipDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            var query = _context.TransparencyScholarships.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Title.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.Title,
                      x.IsFile,
                      x.Url
                  })
                  .ToListAsync();

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
