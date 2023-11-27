using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Implementations
{
    public class TransparencyAdvertisingRepository : Repository<TransparencyAdvertising>, ITransparencyAdvertisingRepository
    {
        public TransparencyAdvertisingRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<TransparencyAdvertising, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Ruc); break;
                case "1":
                    orderByPredicate = ((x) => x.FinancialSource); break;
                case "2":
                    orderByPredicate = ((x) => x.Year); break;
                case "3":
                    orderByPredicate = ((x) => x.Month); break;
                case "4":
                    orderByPredicate = ((x) => x.ExpenseType); break;
                case "5":
                    orderByPredicate = ((x) => x.ProcessType); break;
                case "6":
                    orderByPredicate = ((x) => x.ContractNumber); break;
                case "7":
                    orderByPredicate = ((x) => x.Reason); break;
                case "8":
                    orderByPredicate = ((x) => x.Amount); break;
                case "9":
                    orderByPredicate = ((x) => x.Supplier); break;
                case "10":
                    orderByPredicate = ((x) => x.SupplierRuc); break;
                case "11":
                    orderByPredicate = ((x) => x.ContractAmount); break;
                case "12":
                    orderByPredicate = ((x) => x.PenaltyAmount); break;
                case "13":
                    orderByPredicate = ((x) => x.FinalAmount); break;
                case "14":
                    orderByPredicate = ((x) => x.Observation); break;
            }

            var query = await _context.TransparencyAdvertisings
                .ToListAsync();

            int recordsFiltered = query.Count();

            var data = query
                //.OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Ruc,
                    FinancialSource = ConstantHelpers.TEMPLATES.FINANCIALSOURCE.VALUES.ContainsKey(x.FinancialSource)
                                    ? ConstantHelpers.TEMPLATES.FINANCIALSOURCE.VALUES[x.FinancialSource] : "Desconocido",
                    x.Year,
                    x.Month,
                    ExpenseType = ConstantHelpers.TEMPLATES.EXPENSETYPE.VALUES.ContainsKey(x.ExpenseType)
                                    ? ConstantHelpers.TEMPLATES.EXPENSETYPE.VALUES[x.ExpenseType] : "Desconocido",
                    x.ProcessType,
                    x.ContractNumber,
                    x.Reason,
                    Amount = $"S/. {x.Amount.ToString("0.00")}",
                    x.Supplier,
                    x.SupplierRuc,
                    ContractAmount = $"S/. {x.ContractAmount.ToString("0.00")}",
                    PenaltyAmount = $"S/. {x.PenaltyAmount.ToString("0.00")}",
                    FinalAmount = $"S/. {x.FinalAmount.ToString("0.00")}",
                    x.Observation
                })
                .ToList();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return result;
        }

    }
}
