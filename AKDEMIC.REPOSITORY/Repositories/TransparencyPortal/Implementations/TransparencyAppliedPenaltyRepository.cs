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
    public class TransparencyAppliedPenaltyRepository : Repository<TransparencyAppliedPenalty>, ITransparencyAppliedPenaltyRepository
    {
        public TransparencyAppliedPenaltyRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<TransparencyAppliedPenalty, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CorrelativeNumber); break;
                case "1":
                    orderByPredicate = ((x) => x.ContractNumber); break;
                case "2":
                    orderByPredicate = ((x) => x.ContractorName); break;
                case "3":
                    orderByPredicate = ((x) => x.ContractReason); break;
                case "4":
                    orderByPredicate = ((x) => x.Siaf); break;
                case "5":
                    orderByPredicate = ((x) => x.PenalyAmount); break;
            }

            var query = _context.TransparencyAppliedPenalties.AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    x.CorrelativeNumber,
                    x.ContractNumber,
                    x.ContractorName,
                    x.ContractReason,
                    x.Siaf,
                    PenalyAmount = $"S/. {x.PenalyAmount.ToString("0.00")}"
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
