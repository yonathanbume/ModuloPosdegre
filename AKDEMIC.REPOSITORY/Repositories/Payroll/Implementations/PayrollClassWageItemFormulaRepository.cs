using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class PayrollClassWageItemFormulaRepository : Repository<PayrollClassWageItemFormula>, IPayrollClassWageItemFormulaRepository
    {
        public PayrollClassWageItemFormulaRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<PayrollClassWageItemFormula>> GetAllByWageItem(Guid wageItemId)
            => await _context.PayrollClassWageItemFormulas
                .Where(x => x.WageItemId == wageItemId)
                .ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, Guid wageItemId, string searchValue = null)
        {
            Expression<Func<PayrollClassWageItemFormula, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.PayrollClass.Name);
                    break;
                case "1":
                    orderByPredicate = (x) => x.Formula;
                    break;
            }

            var query = _context.PayrollClassWageItemFormulas.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.WageItem.Name.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()) ||
                    x.PayrollClass.Name.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()) ||
                    x.Formula.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new PayrollClassWageItemFormula
                {
                    Id = x.Id,
                    WageItemId = x.WageItemId,
                    WageItem = new WageItem
                    {
                        Name = x.WageItem.Name
                    },
                    PayrollClassId = x.PayrollClassId,
                    PayrollClass = new PayrollClass
                    {
                        Code = x.PayrollClass.Code,
                        Name = x.PayrollClass.Name
                    },
                    Formula = x.Formula
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


        public async Task<(IEnumerable<PayrollClassWageItemFormula> pagedList, int count)> GetAllByWageItemAndPaginationParameter(Guid wageItemId, PaginationParameter paginationParameter)
        {
            var query = _context.PayrollClassWageItemFormulas.AsQueryable();

            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
                query = query.Where(x => x.WageItem.Name.Contains(paginationParameter.SearchValue) ||
                                    x.PayrollClass.Name.Contains(paginationParameter.SearchValue) ||
                                    x.Formula.Contains(paginationParameter.SearchValue));

            switch(paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.PayrollClass.Name)
                        : query.OrderBy(x => x.PayrollClass.Name);
                    break;
                case "1":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.Formula)
                        : query.OrderBy(x => x.Formula);
                    break;
            }

            var count = await query.CountAsync();

            var result = await query
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage)
                .Select(x => new PayrollClassWageItemFormula
                {
                    Id = x.Id,
                    WageItemId = x.WageItemId,
                    WageItem = new WageItem
                    {
                        Name = x.WageItem.Name
                    },
                    PayrollClassId = x.PayrollClassId,
                    PayrollClass = new PayrollClass
                    {
                        Code = x.PayrollClass.Code,
                        Name = x.PayrollClass.Name
                    },
                    Formula = x.Formula
                }).ToListAsync();

            return (result, count);
        }
    }
}
