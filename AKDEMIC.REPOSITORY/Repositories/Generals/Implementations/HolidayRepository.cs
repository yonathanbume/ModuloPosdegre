using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class HolidayRepository : Repository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> IsHoliday(DateTime date)
        {
            Holiday holiday = await _context.Holidays.Where(x => x.Date.Date == date.Date).FirstOrDefaultAsync();
            return holiday != null;
        }

        public async Task<bool> AnyHolidayByName(string name, Guid? id)
        {
            Holiday holiday = await _context.Holidays.Where(x => x.Name == name && x.Id != id).FirstOrDefaultAsync();
            return holiday != null;
        }

        public async Task<bool> AnyHolidayByDate(DateTime date, Guid? id)
        {
            Holiday holiday = await _context.Holidays.Where(x => x.Date.Date == date.Date && x.Id != id).FirstOrDefaultAsync();
            return holiday != null;
        }

        public async Task<object> GetHoliday(Guid id)
        {
            var result = await _context.Holidays.Where(x => x.Id == id).Select(x => new
            {
                id = x.Id,
                name = x.Name,
                type = x.Type,
                date = x.Date.ToLocalDateFormat(),
                x.NeedReschedule
            }).FirstAsync();
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetHolidaysDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            Expression<Func<Holiday, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Date); break;
                case "1":
                    orderByPredicate = ((x) => x.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Type); break;
                default:
                    orderByPredicate = ((x) => x.Date); break;
            }

            var query = _context.Holidays.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();
            var data = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .AsEnumerable()
                .Select(x => new
                {
                    id = x.Id,
                    type = x.Type,
                    name = x.Name,
                    date = x.Date.ToLocalDateFormat()
                }).ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<Holiday>> GetHolidayByRange(DateTime start, DateTime end)
        {
            var holidays = await _context.Holidays.Where(x => x.Date.Date >= start.Date && x.Date.Date <= end.Date).ToListAsync();
            return holidays;
        }

    }
}
