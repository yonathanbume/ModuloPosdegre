using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class PayrollConceptRepository : Repository<PayrollConcept>, IPayrollConceptRepository
    {
        public PayrollConceptRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByCode(string code, Guid? id = null)
        {
            return await _context.PayrollConcepts.AnyAsync(x => x.Code.ToUpper() == code.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllPayrollConceptsDatatable(DataTablesStructs.SentParameters sentParameters, int type, string searchValue = null)
        {
            Expression<Func<PayrollConcept, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = (x) => x.Type;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.NormDescription);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.NormDetail);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.IsActive);
                    break;
            }

            var query = _context.PayrollConcepts.AsNoTracking();

            if (type != 0)
            {
                query = query.Where(x => x.Type == type);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.NormDescription.ToUpper().Contains(searchValue.ToUpper())
                                    || x.NormDetail.ToUpper().Contains(searchValue.ToUpper())
                                    || x.Code.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Type,
                    TypeText = ConstantHelpers.PAYROLLCONCEPT_TYPE.VALUES.ContainsKey(x.Type) ?
                        ConstantHelpers.PAYROLLCONCEPT_TYPE.VALUES[x.Type] : "",
                    x.NormDescription,
                    x.NormDetail,
                    x.IsActive
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
    }
}
