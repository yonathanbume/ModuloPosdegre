using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class ExamWeekRepository : Repository<ExamWeek>, IExamWeekRepository
    {
        public ExamWeekRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExamWeekDatatable(DataTablesStructs.SentParameters parameters)
        {
            Expression<Func<ExamWeek, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Term.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Week;
                    break;
                case "3":
                    orderByPredicate = (x) => x.WeekStart;
                    break;
                case "4":
                    orderByPredicate = (x) => x.WeekEnd;
                    break;
                default:
                    break;
            }

            var query = _context.ExamWeeks.AsNoTracking();
            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.TermId,
                    term = x.Term.Name,
                    x.Week,
                    weekStart = x.WeekStart.ToLocalDateFormat(),
                    weekEnd = x.WeekEnd.ToLocalDateFormat()
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> AnyByTermAndType(Guid termId, byte type)
            => await _context.ExamWeeks.AnyAsync(x => x.TermId == termId && x.Type == type);
    }
}
