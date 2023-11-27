using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleReportHistoryRepository:Repository<ScaleReportHistory>, IScaleReportHistoryRepository
    {
        public ScaleReportHistoryRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetScaleReportHistoryDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
        {
            Expression<Func<ScaleReportHistory, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Charge); break;
                case "2":
                    orderByPredicate = ((x) => x.Destiny); break;
                case "3":
                    orderByPredicate = ((x) => x.Code); break;
                case "4":
                    orderByPredicate = ((x) => x.CreatedBy); break;
                case "5":
                    orderByPredicate = ((x) => x.CreatedAt); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var query = _context.ScaleReportHistories.Where(x => x.UserId == userId).AsQueryable();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Charge,
                    x.Destiny,
                    x.Code,
                    x.Url,
                    x.CreatedBy,
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat()
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
