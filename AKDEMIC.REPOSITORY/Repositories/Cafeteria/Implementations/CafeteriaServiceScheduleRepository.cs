using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{

    public class CafeteriaServiceScheduleRepository : Repository<CafeteriaServiceTermSchedule>, ICafeteriaServiceScheduleRepository
    {
        public CafeteriaServiceScheduleRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<CafeteriaServiceTermSchedule>> GetCafeteriaServiceTermScheduleDataTable(DataTablesStructs.SentParameters sentParameters, string search)
        {

            return await GetCafeteriaServiceTermScheduleDataTable(sentParameters, (x) => new CafeteriaServiceTermSchedule
            {
                Id = x.Id,
                WeekNumber = x.WeekNumber,
                CafeteriaServiceTermId = x.CafeteriaServiceTermId,
                DateBegin = x.DateBegin,
                DateEnd = x.DateEnd,
                Description = x.Description,
            }, (x) => new[] { x.Description }, search);
        }

        public async Task<DataTablesStructs.ReturnedData<CafeteriaWeeklySchedule>> GetDaysDataTable(DataTablesStructs.SentParameters sentParameters, Guid weekId, string search)
        {
            Expression<Func<CafeteriaWeeklySchedule, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.DayOfWeek);
                    break;
            }

            return await GetDaysDataTable(sentParameters, weekId, (x) => new CafeteriaWeeklySchedule
            {
                Id = x.Id,
                DayOfWeek = x.DayOfWeek,
                //Type = x.Type,
                //StartTime = x.StartTime,
                //EndTime = x.EndTime,
                //MenuPlateId = x.MenuPlateId,
                //MenuPlate = new MenuPlate
                //{
                //    Name = x.MenuPlate.Name
                //},
                CafeteriaServiceTermScheduleId = x.CafeteriaServiceTermScheduleId,
            }, orderByPredicate, (x) => new[] { ConstantHelpers.WEEKDAY.VALUES[x.DayOfWeek] }, search);
        }
        public async Task<CafeteriaServiceTermSchedule> GetDates()
        {
            var term = await GetActiveTerm();
            var weeks = await _context.CafeteriaServiceTermSchedules.ToListAsync();
            var week = weeks.Where(x => x.CafeteriaServiceTermId == term.Id).OrderByDescending(e => e.Id).FirstOrDefault();

            var cafeteriaServiceTermSchedule = new CafeteriaServiceTermSchedule
            {
                WeekNumber = weeks.Count() + 1
            };
            if (week != null)
            {
                var nextMonday = week.DateEnd;
                while (nextMonday.DayOfWeek != DayOfWeek.Monday)
                    nextMonday = nextMonday.AddDays(1);

                cafeteriaServiceTermSchedule.DateBegin = nextMonday;
                cafeteriaServiceTermSchedule.DateEnd = GetEndDate(nextMonday, term.DateEnd); ;
            }
            else
            {
                var start = GetStartDate();
                var end = GetEndDate(start, term.DateEnd);
                cafeteriaServiceTermSchedule.DateBegin = start;
                cafeteriaServiceTermSchedule.DateEnd = end;
            }

            if (cafeteriaServiceTermSchedule.DateEnd > term.DateEnd || cafeteriaServiceTermSchedule.DateBegin <= term.DateBegin)
                return null;

            return cafeteriaServiceTermSchedule;
        }
        public async Task<CafeteriaServiceTermSchedule> InsertWeek(CafeteriaServiceTermSchedule cafeteriaServiceTermSchedule)
        {
            var term = await GetActiveTerm();

            var weeks = await _context.CafeteriaServiceTermSchedules.ToListAsync();

            cafeteriaServiceTermSchedule.WeekNumber = weeks.Count() + 1;
            cafeteriaServiceTermSchedule.CafeteriaServiceTermId = term.Id;

            await _context.CafeteriaServiceTermSchedules.AddAsync(cafeteriaServiceTermSchedule);
            await _context.SaveChangesAsync();
            return cafeteriaServiceTermSchedule;
        }

        public async Task InsertDays(CafeteriaWeeklySchedule dayofweek)
        {
            await _context.CafeteriaWeeklySchedules.AddAsync(dayofweek);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDays(CafeteriaWeeklySchedule dayofweek)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDayById(Guid id)
        {
            var day = await _context.CafeteriaWeeklySchedules.FirstOrDefaultAsync(x => x.Id == id);
            _context.CafeteriaWeeklySchedules.Remove(day);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateDayOfWeek(Guid weekId, int day)
        {
            var week = await _context.CafeteriaServiceTermSchedules.FirstOrDefaultAsync(x => x.Id == weekId);
            var dayOfWeek = ConstantHelpers.WEEKDAY.TO_ENUM(day);

            var aux = week.DateBegin;
            while (aux <= week.DateEnd)
            {
                if (aux.DayOfWeek == dayOfWeek)
                    return true;

                aux = aux.AddDays(1);
            }
            return false;
        }

        public async Task InsertAssistance(UserCafeteriaDailyAssistance newassistance)
        {
            await _context.UserCafeteriaDailyAssistances.AddAsync(newassistance);
            await _context.SaveChangesAsync();
        }

        public async Task<CafeteriaServiceTermSchedule> GetActiveWeek()
        {
            return await _context.CafeteriaServiceTermSchedules.FirstOrDefaultAsync(x => (x.DateBegin <= DateTime.UtcNow && x.DateEnd > DateTime.UtcNow));
        }

        public async Task<CafeteriaWeeklyScheduleTurnDetail> GetActiveDayAndTurn(Guid CafeteriaTermScheduleId)
        {
            //return await _context.CafeteriaWeeklySchedules.FirstOrDefaultAsync(x => x.IsActive);
            var today = ConvertHelpers.DatetimepickerToDateTime(DateTime.UtcNow.ToLocalDateTimeFormat());

            var entity = await _context.CafeteriaWeeklyScheduleTurnDetails.Include(x=>x.CafeteriaWeeklySchedule).Where(x => x.CafeteriaWeeklySchedule.CafeteriaServiceTermScheduleId == CafeteriaTermScheduleId).ToArrayAsync();

            var result = entity.Where(x=> ConstantHelpers.WEEKDAY.TO_ENUM(x.CafeteriaWeeklySchedule.DayOfWeek) == today.DayOfWeek
            && x.StartTime.ToLocalTimeSpanUtc() <= today.TimeOfDay
            && x.EndTime.ToLocalTimeSpanUtc() >= today.TimeOfDay).FirstOrDefault();

            return result;
        }

        public async Task<Select2Structs.ResponseParameters> GetWeeksSelect2ClientSide(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetWeeksSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = "Semana " + x.WeekNumber,
            }, (x) => new[] { x.Description }, searchValue);
        }

        public async Task<CafeteriaWeeklySchedule> GetDayData(Guid id)
        {
            return await _context.CafeteriaWeeklySchedules.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDayByWeekGroupBy(DataTablesStructs.SentParameters sentParameters, Guid weekId, string searchValue = null)
        {
            var query = _context.CafeteriaWeeklySchedules.Where(x=>x.CafeteriaServiceTermScheduleId == weekId).AsNoTracking();

            var list = await query.ToListAsync();

            if (!string.IsNullOrEmpty(searchValue))
            {
                list = list.Where(x => ConstantHelpers.WEEKDAY.VALUES[x.DayOfWeek].Trim().ToLower().Contains(searchValue.Trim().ToLower())).ToList();
            }

            var groupByDayOfWeek = list.GroupBy(x =>x.DayOfWeek).ToList();

            var recordsFiltered = groupByDayOfWeek.Count();
            var data = groupByDayOfWeek
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(s => new
                {                 
                    DayOfWeek = ConstantHelpers.WEEKDAY.VALUES[s.Key],
                    DayOfWeekInt = s.Key
                })
                .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTurnsByWeeklyScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid cafeteriaWeeklyScheduleId, int dayOfWeek, string searchValue = null)
        {
            var query = _context.CafeteriaWeeklyScheduleTurnDetails.Include(x=>x.CafeteriaWeeklySchedule)
                .Include(x=>x.MenuPlate.menuPlateSupplies)
                .Where(x => x.CafeteriaWeeklySchedule.CafeteriaServiceTermScheduleId == cafeteriaWeeklyScheduleId  && x.CafeteriaWeeklySchedule.DayOfWeek == dayOfWeek).AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => ConstantHelpers.WEEKDAY.VALUES[x.CafeteriaWeeklySchedule.DayOfWeek].Contains(searchValue));
            }

            var recordsFiltered = await query.CountAsync();
            var data = query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(s => new
                {
                    s.Id,
                    MenuPlateId = s.MenuPlateId, 
                    Turn = ConstantHelpers.TURN_TYPE.VALUES[s.Type],
                    CanRegister = (s.MenuPlateId.HasValue) ? (s.MenuPlate.menuPlateSupplies.Count > 0) ? true : false  : false
                })
                .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<MenuPlateSupply>> GetMenuPlateByWeeklySchedule(Guid MenuPlateId)
        {
            //var query = await _context.CafeteriaWeeklySchedules.Where(x => x.Id == WeeklyScheduleId).Select(x => x.MenuPlateId).FirstOrDefaultAsync();
            //var result = await _context.MenuPlateSupplies.Include(x=>x.Supply.UnitMeasurement).Where(x => x.MenuPlateId == query).ToListAsync();
            //var result = await _context.MenuPlateSupplies.Include(x => x.Supply.UnitMeasurement).ToListAsync();
            var result = await _context.MenuPlateSupplies.Where(x=>x.MenuPlateId == MenuPlateId).Include(x=>x.ProviderSupply.Supply.UnitMeasurement).ToListAsync();
            return result;
        }


        #region PRIVATE
        private async Task<Select2Structs.ResponseParameters> GetWeeksSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<CafeteriaServiceTermSchedule, Select2Structs.Result>> selectPredicate, Func<CafeteriaServiceTermSchedule, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var term = await GetActiveTerm();
            var query = _context.CafeteriaServiceTermSchedules
                .Where(x => x.CafeteriaServiceTermId == term.Id)
                .WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        //dias del periodo
        private async Task<DataTablesStructs.ReturnedData<CafeteriaWeeklySchedule>> GetDaysDataTable(DataTablesStructs.SentParameters sentParameters, Guid weekId, Expression<Func<CafeteriaWeeklySchedule, CafeteriaWeeklySchedule>> selectPredicate = null, Expression<Func<CafeteriaWeeklySchedule, dynamic>> orderByPredicate = null, Func<CafeteriaWeeklySchedule, string[]> searchValuePredicate = null, string searchValue = null)
        {
            //REVISAR
            var query = _context.CafeteriaWeeklySchedules
                //.Include(x => x.MenuPlate)
                .Where(x => x.CafeteriaServiceTermScheduleId == weekId)
                .WhereSearchValue(searchValuePredicate, searchValue)
                .OrderByConditionThenBy(sentParameters.OrderDirection, orderByPredicate, (x => x.DayOfWeek))
                .AsNoTracking();

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        //semanas del periodo activo
        private async Task<DataTablesStructs.ReturnedData<CafeteriaServiceTermSchedule>> GetCafeteriaServiceTermScheduleDataTable(DataTablesStructs.SentParameters sentParameters, Expression<Func<CafeteriaServiceTermSchedule, CafeteriaServiceTermSchedule>> selectPredicate = null, Func<CafeteriaServiceTermSchedule, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var activeTermId = await GetActiveTerm();

            var query = _context.CafeteriaServiceTermSchedules
                .Where(x => x.CafeteriaServiceTermId == activeTermId.Id)
                //.WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Description.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        /// <summary>
        /// periodo activo
        /// </summary>
        /// <returns></returns>
        private async Task<CafeteriaServiceTerm> GetActiveTerm()
        {
            return await _context.CafeteriaServiceTerms.Where(x => (x.DateBegin <= DateTime.UtcNow && x.DateEnd > DateTime.UtcNow)).FirstOrDefaultAsync();
        }

        private DateTime GetStartDate()
        {
            var date = DateTime.Now.AddDays(1);
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                return date;

            while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(1);

            return date;
        }
        private DateTime GetEndDate(DateTime start, DateTime termEnd)
        {
            var lastFriday = start;
            while (lastFriday.DayOfWeek != DayOfWeek.Friday)
            {
                if (lastFriday == termEnd)
                    break;
                else
                    lastFriday = lastFriday.AddDays(1);
            }

            return lastFriday;
        }

        public async Task<bool> ValidateWeekByTerm(DateTime dateBegin, DateTime dateEnd)
        {
            var termCurrent = await GetActiveTerm();
            var condition = false;
            if ((termCurrent.DateBegin.Date <= dateBegin.Date) && (dateBegin.Date <= termCurrent.DateEnd.Date))
            {
                condition = true;
            }
            else
            {
                condition = false;
            }
            if ((termCurrent.DateBegin.Date <= dateEnd.Date) && (dateEnd.Date <= termCurrent.DateEnd.Date))
            {
                condition = true;
            }
            else {
                condition = false;
            }
            return condition;
        }

        public async Task<bool> ValidateDayByWeek(Guid cafeteriaServiceTermScheduleId, int dayOfWeek, Guid? cafeteriaWeeklyScheduleId = null)
        {
            var query = _context.CafeteriaWeeklySchedules.AsQueryable();
            if (cafeteriaWeeklyScheduleId.HasValue)
            {
                query = query.Where(x => x.Id != cafeteriaWeeklyScheduleId.Value);
            }

            return await query
                .Where(x => x.CafeteriaServiceTermScheduleId == cafeteriaServiceTermScheduleId).AnyAsync(x => x.DayOfWeek == dayOfWeek);
        }

        public async Task<byte> GetTurnByMenuPlate(Guid MenuPlateId)
        {
            return await _context.CafeteriaWeeklyScheduleTurnDetails.Where(x => x.MenuPlateId == MenuPlateId).Select(x=>x.Type).FirstOrDefaultAsync();
        }
        #endregion
    }
}
