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
    public class TransparencyVisitRecordRepository : Repository<TransparencyVisitsRecord>, ITransparencyVisitRecordRepository
    {
        public TransparencyVisitRecordRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<TransparencyVisitsRecord, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Number); break;
                case "1":
                    orderByPredicate = ((x) => x.Date); break;
                case "2":
                    orderByPredicate = ((x) => x.Visitor); break;
                case "3":
                    orderByPredicate = ((x) => x.DocumentType); break;
                case "4":
                    orderByPredicate = ((x) => x.DocumentNumber); break;
                case "5":
                    orderByPredicate = ((x) => x.Entity); break;
                case "6":
                    orderByPredicate = ((x) => x.Reason); break;
                case "7":
                    orderByPredicate = ((x) => x.Sede); break;
                case "8":
                    orderByPredicate = ((x) => x.PublicEmployee); break;
                case "9":
                    orderByPredicate = ((x) => x.Office); break;
                case "10":
                    orderByPredicate = ((x) => x.MeetingPlace); break;
                case "11":
                    orderByPredicate = ((x) => x.EnterTime); break;
                case "12":
                    orderByPredicate = ((x) => x.LeaveTime); break;
            }

            var query = _context.TransparencyVisitsRecords.AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    x.Number,
                    Date = x.Date.ToLocalDateFormat(),
                    x.Visitor,
                    x.DocumentType,
                    x.DocumentNumber,
                    x.Entity,
                    x.Reason,
                    x.Sede,
                    x.PublicEmployee,
                    x.Office,
                    x.MeetingPlace,
                    EnterTime = x.EnterTime.ToString(@"hh\:mm"),
                    LeaveTime = x.LeaveTime.ToString(@"hh\:mm")
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
