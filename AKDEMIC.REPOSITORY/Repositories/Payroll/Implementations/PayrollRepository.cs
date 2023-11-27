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
    public class PayrollRepository : Repository<ENTITIES.Models.Payroll.Payroll>, IPayrollRepository
    {
        public PayrollRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ENTITIES.Models.Payroll.Payroll, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = (x) => x.PayrollType.Name;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.PayrollClass.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Interval);
                    break;
            }

            var query = _context.Payrolls.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (!string.IsNullOrEmpty(searchValue))
                    query = query.Where(q => q.Code.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()) ||
                                        q.PayrollType.Name.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()) ||
                                        q.PayrollClass.Name.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()) ||
                                        ConstantHelpers.PAYROLL.PAYROLL_INTERVALS.VALUES[q.Interval].Trim().ToUpper().Contains(searchValue.Trim().ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(q => new
                {
                    Id = q.Id,
                    Code = q.Code,
                    PayrollType = new PayrollType
                    {
                        Id = q.PayrollTypeId,
                        Name = q.PayrollType.Name
                    },
                    PayrollClass = new PayrollClass
                    {
                        Id = q.PayrollClassId,
                        Name = q.PayrollClass.Name
                    },
                    Interval = q.Interval,
                    Processed = q.Processed
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

        public override async Task DeleteById(Guid id)
        {
            var payrollWorkers = await _context.PayrollWorkers
                .Where(x => x.PayrollId == id).ToListAsync();
            _context.PayrollWorkers.RemoveRange(payrollWorkers);
            await _context.SaveChangesAsync();
            await base.DeleteById(id);
        }

        public override async Task Delete(ENTITIES.Models.Payroll.Payroll payroll)
        {
            var payrollWorkers = await _context.PayrollWorkers
                .Where(x => x.PayrollId == payroll.Id).ToListAsync();
            _context.PayrollWorkers.RemoveRange(payrollWorkers);
            await _context.SaveChangesAsync();
            await base.Delete(payroll);
        }

        public async Task<ENTITIES.Models.Payroll.Payroll> FindByCode(string code)
            => await _context.Payrolls.FirstOrDefaultAsync(x => x.Code.Equals(code));

        public async Task<(IEnumerable<ENTITIES.Models.Payroll.Payroll> pagedList, int count)> 
            GetAllByPaginationParameter(PaginationParameter paginationParameter)
        {
            var query = _context.Payrolls.AsQueryable();

            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
                query = query.Where(q => q.Code.Contains(paginationParameter.SearchValue) ||
                                    q.PayrollType.Name.Contains(paginationParameter.SearchValue) ||
                                    q.PayrollClass.Name.Contains(paginationParameter.SearchValue) ||
                                    ConstantHelpers.PAYROLL.PAYROLL_INTERVALS.VALUES[q.Interval].Contains(paginationParameter.SearchValue));

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
                        ? query.OrderByDescending(q => q.PayrollType.Name)
                        : query.OrderBy(q => q.PayrollType.Name);
                    break;
                case "2":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.PayrollClass.Name)
                        : query.OrderBy(q => q.PayrollClass.Name);
                    break;
                case "3":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => ConstantHelpers.PAYROLL.PAYROLL_INTERVALS.VALUES[q.Interval])
                        : query.OrderBy(q => ConstantHelpers.PAYROLL.PAYROLL_INTERVALS.VALUES[q.Interval]);
                    break;
            }

            var result = await query
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage)
                .Select(q => new ENTITIES.Models.Payroll.Payroll
                {
                    Id = q.Id,
                    Code = q.Code,
                    PayrollType = new PayrollType
                    {
                        Id = q.PayrollTypeId,
                        Name = q.PayrollType.Name
                    },
                    PayrollClass = new PayrollClass
                    {
                        Id = q.PayrollClassId,
                        Name = q.PayrollClass.Name
                    },
                    Interval = q.Interval,
                    Processed = q.Processed
                }).ToListAsync();

            return (result, count);
        }
    }
}
