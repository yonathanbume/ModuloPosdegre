using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using System.Linq.Expressions;
using System;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class PayrollTypeRepository : Repository<PayrollType>, IPayrollTypeRepository
    {
        public PayrollTypeRepository(AkdemicContext context) : base(context){}

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<PayrollType, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.PayrollTypes.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(q => q.Name.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()) ||
                                    q.Code.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name

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
        public async Task<PayrollType> FindByCode(string code)
            => await _context.PayrollTypes.FirstOrDefaultAsync(x => x.Code.Equals(code));

        public async Task<(IEnumerable<PayrollType> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter)
        {
            var query = _context.PayrollTypes.AsQueryable();

            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
                query = query.Where(q => q.Name.Contains(paginationParameter.SearchValue) ||
                                    q.Code.Contains(paginationParameter.SearchValue));

            var count = await query.CountAsync();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.Code)
                        : query.OrderBy(q => q.Code);
                    break;
                case "1":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.Name)
                        : query.OrderBy(q => q.Name);
                    break;
            }

            var result = await query
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage)
                .Select(q => new PayrollType
                {
                    Id = q.Id,
                    Code = q.Code,
                    Name = q.Name
                }).ToListAsync();

            return (result, count);
        }

        public async Task<object> GetPayrollTypesJson(string term)
        {
            var qry = _context.PayrollTypes.AsQueryable();

            if (term != null) qry = qry.Where(x => x.Code.ToUpper().Contains(term.ToUpper()) || x.Name.ToUpper().Contains(term.ToUpper()));

            var concepts = await qry
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.Code}-{x.Name}",
                }).ToListAsync();

            return concepts;
        }

        public async Task<bool> AnyByCode(string code, Guid? id = null)
        {
            return await _context.PayrollTypes.AnyAsync(x => x.Code == code && x.Id != id);
        }
    }
}
