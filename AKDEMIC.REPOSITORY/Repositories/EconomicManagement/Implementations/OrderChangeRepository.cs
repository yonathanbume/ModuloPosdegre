using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class OrderChangeRepository : Repository<OrderChangeHistory>, IOrderChangeRepository
    {
        public OrderChangeRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<OrderChangeHistory, dynamic>> GetOrderChangesDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Description);
                case "1":
                    return ((x) => x.Description);
                default:
                    return ((x) => x.Description);
            }
        }

        private Func<OrderChangeHistory, string[]> GetOrderChangesDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Description + ""
            };
        }

        private async Task<DataTablesStructs.ReturnedData<OrderChangeHistory>> GetOrderChangesDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<OrderChangeHistory, OrderChangeHistory>> selectPredicate = null, Expression<Func<OrderChangeHistory, dynamic>> orderByPredicate = null, Func<OrderChangeHistory, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.OrderChanges
                .WhereSearchValue(searchValuePredicate, searchValue)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<DataTablesStructs.ReturnedData<OrderChangeHistory>> GetOrderChangesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetOrderChangesDatatable(sentParameters, ExpressionHelpers.SelectOrderChange(), GetOrderChangesDatatableOrderByPredicate(sentParameters), GetOrderChangesDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderHistory(DataTablesStructs.SentParameters sentParameters, Guid orderId)
        {
            Expression<Func<OrderChangeHistory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Order.Title);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.StatusPre);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.StatusPost);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var query = _context.OrderChanges.Where(x => x.OrderId == orderId).AsNoTracking();


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
                    title = x.Order.Title,
                    statusPre = ConstantHelpers.ORDERS.STATUS.VALUES[x.StatusPre],
                    statusPost = ConstantHelpers.ORDERS.STATUS.VALUES[x.StatusPost],
                    createAt = x.ParsedCreatedAt,
                    path = _context.OrderChangeFiles.Where(y => y.OrderChangeId == x.Id).Select(y => y.Path).FirstOrDefault()
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
        #endregion
    }
}
