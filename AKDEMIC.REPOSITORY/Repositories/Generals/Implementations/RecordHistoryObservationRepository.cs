using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class RecordHistoryObservationRepository : Repository<RecordHistoryObservation> , IRecordHistoryObservationRepository
    {
        public RecordHistoryObservationRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetObservationsDatatableByRecordHistoryId(DataTablesStructs.SentParameters sentParameters, Guid recordHistoryId)
        {
            Expression<Func<RecordHistoryObservation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var query = _context.RecordHistoryObservations.Where(x => x.RecordHistoryId == recordHistoryId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    date = x.CreatedAt.ToLocalDateTimeFormat(),
                    status = ConstantHelpers.RECORD_HISTORY_STATUS.VALUES[x.RecordHistoryStatus],
                    observation = x.Observation,
                }).ToListAsync();

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
