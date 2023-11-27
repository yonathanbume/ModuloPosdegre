using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class RemunerationPayrollTypeRepository : Repository<RemunerationPayrollType>, IRemunerationPayrollTypeRepository
    {
        public RemunerationPayrollTypeRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, Guid? payrollTypeId = null, string searchValue = null)
        {
            Expression<Func<RemunerationPayrollType, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.WageItem.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PayrollType.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Amount;
                    break;
            }

            var query = _context.RemunerationPayrollTypes.AsNoTracking();

            if (payrollTypeId != null)
                query = query.Where(x => x.PayrollTypeId == payrollTypeId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchTrim = searchValue.Trim();
                query = query.Where(x => x.WageItem.Name.ToString().Contains(searchTrim) ||
                                    x.PayrollType.Code.ToString().Contains(searchTrim) ||
                                    x.PayrollType.Name.ToString().Contains(searchTrim));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    wageItemName = x.WageItem.Name,
                    payrollTypeName = x.PayrollType.Name,
                    x.Amount
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

        public async Task<RemunerationPayrollType> GetWithIncludes(Guid id)
        {
            var result = await _context.RemunerationPayrollTypes
                .Include(x => x.WageItem)
                .Include(x => x.PayrollType)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
