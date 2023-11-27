
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class DebtRepository : Repository<Debt>, IDebtRepository
    {
        public DebtRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<Debt, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.DocumentSerie;
                    break;
                case "1":
                    orderByPredicate = (x) => x.DocumentNumber;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Concept;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Amount;
                    break;
                default:
                    break;
            }

            var query = _context.Debts
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.DocumentSerie.ToUpper().Contains(search.ToUpper()) 
                || x.DocumentNumber.ToString().Contains(search) 
                || x.Concept.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    documentSerie = x.DocumentSerie,
                    documentNumber = x.DocumentNumber,
                    concept = x.Concept,
                    amount = x.Amount
                }).ToListAsync();

            var recordsTotal = data.Count;

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
