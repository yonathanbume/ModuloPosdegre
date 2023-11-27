using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.CORE.Helpers;
using System.Linq.Expressions;
using System;
using DocumentFormat.OpenXml.Wordprocessing;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkingTerm;
using DocumentFormat.OpenXml.Bibliography;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class WorkingTermRepository : Repository<WorkingTerm>, IWorkingTermRepository
    {
        public WorkingTermRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<WorkingTerm, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Year;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Year;
                    break;
                case "2":
                    orderByPredicate = (x) => x.MonthNumber;
                    break;
                case "3":
                    orderByPredicate = (x) => x.IsExtraTerm;
                    break;
                case "4":
                    orderByPredicate = ((x) => x.StartDate);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.EndDate);
                    break;
                case "6":
                    orderByPredicate = ((x) => x.Status);
                    break;
            }

            var query = _context.WorkingTerms.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchTrim = searchValue.Trim();
                query = query.Where(x => x.Year.ToString().Contains(searchTrim));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Year,
                    x.IsExtraTerm,
                    x.Status,
                    StatusText = ConstantHelpers.WORKINGTERM_STATUS.VALUES.ContainsKey(x.Status) ?
                        ConstantHelpers.WORKINGTERM_STATUS.VALUES[x.Status] : "-",
                    month = ConstantHelpers.MONTHS.VALUES.ContainsKey(x.MonthNumber) ?
                        ConstantHelpers.MONTHS.VALUES[x.MonthNumber] : "-",
                    Correlative = $"{x.Year}{x.MonthNumber}{x.Number}",
                    StartDate = x.StartDate.ToLocalDateFormat(),
                    EndDate = x.EndDate.ToLocalDateFormat()
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<WorkingTerm> GetActive()
            => await _context.WorkingTerms.FirstOrDefaultAsync(x => DateTime.UtcNow >= x.StartDate && DateTime.UtcNow <= x.EndDate);

        public async Task<(IEnumerable<WorkingTerm> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter)
        {
            var query = _context.WorkingTerms.AsQueryable();

            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
                query = query.Where(q => q.StartDate.ToDefaultTimeZone().Year.ToString().Contains(paginationParameter.SearchValue)
                                    || q.StartDate.ToDefaultTimeZone().Month.ToString().Contains(paginationParameter.SearchValue)
                                    || q.StartDate.ToDefaultTimeZone().Day.ToString().Contains(paginationParameter.SearchValue)
                                    || q.EndDate.ToDefaultTimeZone().Year.ToString().Contains(paginationParameter.SearchValue)
                                    || q.EndDate.ToDefaultTimeZone().Month.ToString().Contains(paginationParameter.SearchValue)
                                    || q.EndDate.ToDefaultTimeZone().Day.ToString().Contains(paginationParameter.SearchValue));

            var count = await query.CountAsync();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.Year)
                        : query.OrderBy(q => q.Year);
                    break;
                case "1":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.Number)
                        : query.OrderBy(q => q.Number);
                    break;
                case "2":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.StartDate)
                        : query.OrderBy(q => q.StartDate);
                    break;
                case "3":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.EndDate)
                        : query.OrderBy(q => q.EndDate);
                    break;
            }

            var result = await query
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage)
                .Select(q => new WorkingTerm
                {
                    Id = q.Id,
                    Year = q.Year,
                    Number = q.Number,
                    StartDate = q.StartDate,
                    EndDate = q.EndDate
                }).ToListAsync();

            return (result, count);
        }

        public async Task<int> MaxNumberByYearMonth(int year, int month)
        {
            var result = await _context.WorkingTerms.Where(x => x.Year == year && x.MonthNumber == month).Select(x => x.Number).DefaultIfEmpty().MaxAsync();

            return result;
        }

        public async Task<bool> AnyActive()
        {
            return await _context.WorkingTerms.AnyAsync(x=>x.Status == ConstantHelpers.WORKINGTERM_STATUS.ACTIVE);
        }

        public async Task<WorkingTermTemplate> GetLastActive()
        {
            var result = await _context.WorkingTerms.Where(x => x.Status == ConstantHelpers.WORKINGTERM_STATUS.ACTIVE)
                .Select(x => new WorkingTermTemplate
                { Id = x.Id,
                Correlative = $"{x.Year}{x.MonthNumber}{x.Number}",
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
