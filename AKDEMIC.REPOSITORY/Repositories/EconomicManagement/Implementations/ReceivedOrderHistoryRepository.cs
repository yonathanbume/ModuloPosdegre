using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class ReceivedOrderHistoryRepository : Repository<ReceivedOrderHistory>, IReceivedOrderHistoryRepository
    {
        public ReceivedOrderHistoryRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatableByReceivedOrderId(DataTablesStructs.SentParameters sentParameters, Guid receivedOrderId)
        {
            Expression<Func<ReceivedOrderHistory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.CreatedAt);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.CreatedAt.ToLocalDateTimeFormat());

                    break;
                case "3":
                    orderByPredicate = ((x) => x.QuantityReceivedPrev);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.QuantityReceivedPost);
                    break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var query = _context.ReceivedOrderHistories.Where(x => x.ReceivedOrderId == receivedOrderId).AsNoTracking();


            var recordsFiltered = await query.CountAsync();

            query = query
                .AsQueryable();


            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    user = x.User.FullName,
                    createdAt = x.CreatedAt,
                    createdAtFormat = x.CreatedAt.ToLocalDateTimeFormat(),
                    quantityItem = _context.UserRequirementItems.Where(i => i.UserRequirementId == x.ReceivedOrder.UserRequirementId).Select(a => a.Quantity.ToString()).FirstOrDefault(),
                    quantityItemPre = x.QuantityReceivedPrev,
                    quantityItemPost = x.QuantityReceivedPost,
                    observation = x.Observation

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
