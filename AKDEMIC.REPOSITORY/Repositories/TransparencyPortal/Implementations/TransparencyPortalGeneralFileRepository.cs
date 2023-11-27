using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Implementations
{
    public class TransparencyPortalGeneralFileRepository : Repository<TransparencyPortalGeneralFile>, ITransparencyPortalGeneralFileRepository
    {
        public TransparencyPortalGeneralFileRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TransparencyPortalGeneralFile>> GetAll(Guid generalId)
        {
            var result = await _context.TransparencyPortalGeneralFiles
                .Where(x => x.TransparencyPortalGeneralId == generalId)
                .ToListAsync();

            return result;
        }

        public async Task<object> GetDataTable(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            Expression<Func<TransparencyPortalGeneralFile, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name; break;
                default:
                    break;
            }

            var query = _context.TransparencyPortalGeneralFiles
                .Where(x => x.TransparencyPortalGeneralId == id)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    isLink = x.IsLink,
                    path = x.Path
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
