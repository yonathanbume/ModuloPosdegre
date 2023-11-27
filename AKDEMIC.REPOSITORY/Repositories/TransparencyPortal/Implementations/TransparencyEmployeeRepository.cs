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
    public class TransparencyEmployeeRepository : Repository<TransparencyEmployee>, ITransparencyEmployeeRepository
    {
        public TransparencyEmployeeRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<TransparencyEmployee, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Ruc); break;
                case "1":
                    orderByPredicate = ((x) => x.Year); break;
                case "2":
                    orderByPredicate = ((x) => x.Month); break;
                case "3":
                    orderByPredicate = ((x) => x.Dni); break;
                case "4":
                    orderByPredicate = ((x) => x.Regime); break;
                case "5":
                    orderByPredicate = ((x) => x.PaternalSurname); break;
                case "6":
                    orderByPredicate = ((x) => x.MaternalSurname); break;
                case "7":
                    orderByPredicate = ((x) => x.Name); break;
                case "8":
                    orderByPredicate = ((x) => x.Charge); break;
                case "9":
                    orderByPredicate = ((x) => x.Dependency); break;
                case "10":
                    orderByPredicate = ((x) => x.Remuneration); break;
                case "11":
                    orderByPredicate = ((x) => x.Honorary); break;
                case "12":
                    orderByPredicate = ((x) => x.Incentive); break;
                case "13":
                    orderByPredicate = ((x) => x.Gratification); break;
                case "14":
                    orderByPredicate = ((x) => x.Benefit); break;
                case "15":
                    orderByPredicate = ((x) => x.Total); break;
                case "16":
                    orderByPredicate = ((x) => x.Observations); break;

            }

            var query = _context.TransparencyEmployees.AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    x.Ruc,
                    x.Year,
                    x.Month,
                    x.Dni,
                    x.Regime,
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Name,
                    x.Charge,
                    x.Dependency,
                    Remuneration = $"S/. {x.Remuneration.ToString("0.00")}",
                    Honorary = $"S/. {x.Honorary.ToString("0.00")}",
                    Incentive = $"S/. {x.Incentive.ToString("0.00")}",
                    Gratification = $"S/. {x.Gratification.ToString("0.00")}",
                    Benefit = $"S/. {x.Benefit.ToString("0.00")}",
                    Total = $"S/. {x.Total.ToString("0.00")}",
                    x.Observations
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

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
