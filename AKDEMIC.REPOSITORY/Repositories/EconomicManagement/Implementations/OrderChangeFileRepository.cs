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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class OrderChangeFileRepository : Repository<OrderChangeFileHistory>, IOrderChangeFileRepository
    {
        public OrderChangeFileRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<OrderChangeFileHistory, dynamic>> GetOrderChangeFilesDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.OrderChangeId);
                case "1":
                    return ((x) => x.OrderChangeId);
                default:
                    return ((x) => x.OrderChangeId);
            }
        }

        private Func<OrderChangeFileHistory, string[]> GetOrderChangeFilesDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.FileName + "",
                x.Path + "",
                x.Size + ""
            };
        }

        private async Task<DataTablesStructs.ReturnedData<OrderChangeFileHistory>> GetOrderChangeFilesDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<OrderChangeFileHistory, OrderChangeFileHistory>> selectPredicate = null, Expression<Func<OrderChangeFileHistory, dynamic>> orderByPredicate = null, Func<OrderChangeFileHistory, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.OrderChangeFiles
                .WhereSearchValue(searchValuePredicate, searchValue)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<DataTablesStructs.ReturnedData<OrderChangeFileHistory>> GetOrderChangeFilesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetOrderChangeFilesDatatable(sentParameters, ExpressionHelpers.SelectOrderChangeFile(), GetOrderChangeFilesDatatableOrderByPredicate(sentParameters), GetOrderChangeFilesDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<IEnumerable<OrderChangeFileHistory>> CompareFilesFromOrder(Guid orderId, IEnumerable<Guid> ids)
        {
            var query = _context.OrderChangeFiles
                .Where(x => x.OrderChange.OrderId == orderId).AsQueryable();

            query = query.Where(x => ids.Contains(x.Id));

            return await query.ToListAsync();
        }

        public void DeleteRangeWithOutSaving(IEnumerable<OrderChangeFileHistory> orderChangeFiles)
        {
            _context.Set<OrderChangeFileHistory>().RemoveRange(orderChangeFiles);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderChangeFilesByOrderDatatable(DataTablesStructs.SentParameters sentParameters, Guid orderId)
        {
            Expression<Func<OrderChangeFileHistory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.FileName);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.OrderChangeFiles
                .Where(x => x.OrderChange.OrderId == orderId)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<OrderChangeFileHistory, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                id = x.Id,
                fileName = x.FileName,
                size = x.Size
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<IEnumerable<OrderChangeFileHistory>> GetAllByOrder(Guid orderId)
        {
            var query = _context.OrderChangeFiles
                    .Where(x => x.OrderChange.OrderId == orderId);

            return await query.ToListAsync();
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderChangeFileDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            Expression<Func<OrderChangeFileHistory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.FileName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Size;
                    break;
                    
                default:
                    orderByPredicate = (x) => x.FileName;
                    break;
            }


            var query = _context.OrderChangeFiles
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            query = _context.OrderChangeFiles.Where(x => x.OrderChange.OrderId == id).AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    id = x.Id,
                    fileName = x.FileName,
                    size = x.Size,
                    path = x.Path
                }).ToListAsync();

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
