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
    public class TransparencySelectionCommitteeRepository : Repository<TransparencySelectionCommittee>, ITransparencySelectionCommitteeRepository
    {
        public TransparencySelectionCommitteeRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<TransparencySelectionCommittee, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CorrelativeNumber); break;
                case "1":
                    orderByPredicate = ((x) => x.Year); break;
                case "2":
                    orderByPredicate = ((x) => x.Month); break;
                case "3":
                    orderByPredicate = ((x) => x.Area); break;
                case "4":
                    orderByPredicate = ((x) => x.CommitteMembers); break;
                case "5":
                    orderByPredicate = ((x) => x.Number); break;
            }

            var query = _context.TransparencySelectionCommittees.AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    x.CorrelativeNumber,
                    x.Year,
                    x.Month,
                    x.Area,
                    x.CommitteMembers,
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
