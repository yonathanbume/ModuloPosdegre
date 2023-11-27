using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Implementations
{
    public class TransparencyServiceOrderRepository : Repository<TransparencyServiceOrder>, ITransparencyServiceOrderRepository
    {
        public TransparencyServiceOrderRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<TransparencyServiceOrder, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Type); break;
                case "1":
                    orderByPredicate = ((x) => x.Year); break;
                case "2":
                    orderByPredicate = ((x) => x.Month); break;
                case "3":
                    orderByPredicate = ((x) => x.Ruc); break;
                case "4":
                    orderByPredicate = ((x) => x.Term); break;
                case "5":
                    orderByPredicate = ((x) => x.OrderNumber); break;
                case "6":
                    orderByPredicate = ((x) => x.Siaf); break;
                case "7":
                    orderByPredicate = ((x) => x.OrderDate); break;
                case "8":
                    orderByPredicate = ((x) => x.OrderAmount); break;
                case "9":
                    orderByPredicate = ((x) => x.Supplier); break;
                case "10":
                    orderByPredicate = ((x) => x.Description); break;
            }

            var query = _context.TransparencyServiceOrders.AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    Type = ConstantHelpers.TEMPLATES.ORDERTYPE.VALUES.ContainsKey(x.Type)
                            ? ConstantHelpers.TEMPLATES.ORDERTYPE.VALUES[x.Type] : "Desconocido",
                    x.Year,
                    x.Month,
                    x.Ruc,
                    x.Term,
                    x.OrderNumber,
                    x.Siaf,
                    OrderDate = x.OrderDate.ToLocalDateFormat(),
                    x.Supplier,
                    orderAmount = $"S/. {x.OrderAmount.ToString("0.00")}",
                    x.Description
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return result;
        }

    }
}
