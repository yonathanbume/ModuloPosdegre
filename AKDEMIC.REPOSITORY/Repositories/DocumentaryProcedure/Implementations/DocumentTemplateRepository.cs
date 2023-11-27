using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class DocumentTemplateRepository : Repository<DocumentTemplate>, IDocumentTemplateRepository
    {
        public DocumentTemplateRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search = null, byte? system = null)
        {
            Expression<Func<DocumentTemplate, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                default:
                    break;
            }

            var query = _context.DocumentTemplates
                .AsNoTracking();

            if (system.HasValue && system != 0)
                query = query.Where(x => x.System == system);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()));

            var recordsTotal = await query.CountAsync();

            var data = await query
                           .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                           .Skip(sentParameters.PagingFirstRecord)
                           .Take(sentParameters.RecordsPerDraw)
                           .Select(x => new
                           {
                               id = x.Id,
                               name = x.Name,
                               date = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : ""
                           }).ToListAsync();


            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }
    }
}
