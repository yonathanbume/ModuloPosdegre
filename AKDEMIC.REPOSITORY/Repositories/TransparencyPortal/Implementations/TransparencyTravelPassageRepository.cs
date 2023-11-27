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
    public class TransparencyTravelPassageRepository : Repository<TransparencyTravelPassage>, ITransparencyTravelPassageRepository
    {
        public TransparencyTravelPassageRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTransparencyTravelPassageDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<TransparencyTravelPassage, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.FinancialSource); break;
                case "1":
                    orderByPredicate = ((x) => x.Ruc); break;
                case "2":
                    orderByPredicate = ((x) => x.TravelType); break;
                case "3":
                    orderByPredicate = ((x) => x.TravelModality); break;
                case "4":
                    orderByPredicate = ((x) => x.Year); break;
                case "5":
                    orderByPredicate = ((x) => x.Month); break;
                case "6":
                    orderByPredicate = ((x) => x.Area); break;
                case "7":
                    orderByPredicate = ((x) => x.UserFullName); break;
                case "8":
                    orderByPredicate = ((x) => x.DepartDate); break;
                case "9":
                    orderByPredicate = ((x) => x.ReturnDate); break;
                case "10":
                    orderByPredicate = ((x) => x.Route); break;
                case "11":
                    orderByPredicate = ((x) => x.AreaAuthorization); break;
                case "12":
                    orderByPredicate = ((x) => x.CostPassageAmount); break;
                case "13":
                    orderByPredicate = ((x) => x.CostTravelAmount); break;
                case "14":
                    orderByPredicate = ((x) => x.TotalCost); break;
                case "15":
                    orderByPredicate = ((x) => x.ResolutionAuthorization); break;
            }

            var query = _context.TransparencyTravelPassages.AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    FinancialSource = ConstantHelpers.TEMPLATES.FINANCIALSOURCE.VALUES.ContainsKey(x.FinancialSource)
                            ? ConstantHelpers.TEMPLATES.FINANCIALSOURCE.VALUES[x.FinancialSource] : "Desconocido",
                    x.Ruc,
                    TravelType = ConstantHelpers.TEMPLATES.TRAVELTYPE.VALUES.ContainsKey(x.TravelType)
                            ? ConstantHelpers.TEMPLATES.TRAVELTYPE.VALUES[x.TravelType] : "Desconocido",
                    TravelModality = ConstantHelpers.TEMPLATES.TRAVELMODALITY.VALUES.ContainsKey(x.TravelModality)
                            ? ConstantHelpers.TEMPLATES.TRAVELMODALITY.VALUES[x.TravelModality] : "Desconocido",
                    x.Year,
                    x.Month,
                    x.Area,
                    x.UserFullName,
                    DepartDate = x.DepartDate.ToLocalDateFormat(),
                    ReturnDate = x.ReturnDate.ToLocalDateFormat(),
                    x.Route,
                    x.AreaAuthorization,
                    CostPassageAmount = $"S/. {x.CostPassageAmount.ToString("0.00")}",
                    CostTravelAmount = $"S/. {x.CostTravelAmount.ToString("0.00")}",
                    TotalCost = $"S/. {x.TotalCost.ToString("0.00")}",
                    x.ResolutionAuthorization,

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
