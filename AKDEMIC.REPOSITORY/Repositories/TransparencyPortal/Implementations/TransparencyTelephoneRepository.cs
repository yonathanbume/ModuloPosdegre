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
    public class TransparencyTelephoneRepository : Repository<TransparencyTelephone>, ITransparencyTelephoneRepository
    {
        public TransparencyTelephoneRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTransparencyTelephoneDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<TransparencyTelephone, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Ruc); break;
                case "1":
                    orderByPredicate = ((x) => x.Year); break;
                case "2":
                    orderByPredicate = ((x) => x.Month); break;
                case "3":
                    orderByPredicate = ((x) => x.Type); break;
                case "4":
                    orderByPredicate = ((x) => x.Area); break;
                case "5":
                    orderByPredicate = ((x) => x.UserAsigned); break;
                case "6":
                    orderByPredicate = ((x) => x.Supplier); break;
                case "7":
                    orderByPredicate = ((x) => x.Charge); break;
                case "8":
                    orderByPredicate = ((x) => x.Amount); break;
            }

            var query = _context.TransparencyTelephones.AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Ruc,
                    x.Year,
                    x.Month,
                    Type = ConstantHelpers.TEMPLATES.TELEPHONETYPE.VALUES.ContainsKey(x.Type)
                            ? ConstantHelpers.TEMPLATES.TELEPHONETYPE.VALUES[x.Type] : "Desconocido",
                    x.Area,
                    x.UserAsigned,
                    x.Supplier,
                    x.Charge,
                    Amount = $"S/. {x.Amount.ToString("0.00")}"
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
