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
    public class TransparencyConciliationActRepository : Repository<TransparencyConciliationAct>, ITransparencyConciliationActRepository
    {
        public TransparencyConciliationActRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<TransparencyConciliationAct, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CorrelativeNumber); break;
                case "1":
                    orderByPredicate = ((x) => x.Record); break;
                case "2":
                    orderByPredicate = ((x) => x.Petitioner); break;
                case "3":
                    orderByPredicate = ((x) => x.Guest); break;
                case "4":
                    orderByPredicate = ((x) => x.ConciliationCenter); break;
                case "5":
                    orderByPredicate = ((x) => x.Contract); break;
                case "6":
                    orderByPredicate = ((x) => x.Topic); break;
                case "7":
                    orderByPredicate = ((x) => x.ContractAmount); break;
                case "8":
                    orderByPredicate = ((x) => x.RequestedAmount); break;
                case "9":
                    orderByPredicate = ((x) => x.State); break;
                case "10":
                    orderByPredicate = ((x) => x.Number); break;
            }

            var query = _context.TransparencyConciliationActs.AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    x.CorrelativeNumber,
                    x.Record,
                    x.Petitioner,
                    x.Guest,
                    x.ConciliationCenter,
                    x.Contract,
                    x.Topic,
                    x.ContractAmount,
                    x.RequestedAmount,
                    x.State,
                    x.Number
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
