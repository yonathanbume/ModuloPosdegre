using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
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
    public class WageItemRepository : Repository<WageItem>, IWageItemRepository
    {
        public WageItemRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<WageItem, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.VariableType);
                    break;
                case "1":
                    orderByPredicate = (x) => x.ConceptType.Name;
                    break;
                case "2":
                    orderByPredicate = ((x) => $"{x.PayrollConcept.Code}-{x.PayrollConcept.NormDescription}" );
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Amount);
                    break;
            }

            var query = _context.WageItems.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.VariableType,
                    VariableTypeText = ConstantHelpers.PAYROLL.WAGE_ITEM_VARIABLE_TYPES.VALUES.ContainsKey(x.VariableType) ?
                    ConstantHelpers.PAYROLL.WAGE_ITEM_VARIABLE_TYPES.VALUES[x.VariableType] : "",
                    conceptTypeName = x.ConceptType.Name,
                    PayrollConceptDesc = $"{x.PayrollConcept.Code}-{x.PayrollConcept.NormDescription}",
                    x.Name,
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

    }
}
