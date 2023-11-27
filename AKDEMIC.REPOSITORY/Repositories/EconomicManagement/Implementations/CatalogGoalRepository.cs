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
    public class CatalogGoalRepository : Repository<CatalogGoal>, ICatalogGoalRepository
    {
        public CatalogGoalRepository(AkdemicContext contex) : base(contex) { }

        public async Task<DateTime> GetLastDate()
        {
            var result = await _context.CatalogGoals.OrderByDescending(x => x.CreateAt).Select(x => x.CreateAt).FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCatalogGoalDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<CatalogGoal, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.SecFunc;
                    break;
                default:
                    orderByPredicate = (x) => x.SecFunc;
                    break;
            }


            var query = _context.CatalogGoals
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.SecFunc.ToUpper().Contains(search)
                    || x.Description.ToUpper().Contains(search));

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    secFunc = x.SecFunc,
                    program = x.Program,
                    prodPry = x.ProdPry,
                    actWork = x.ActWork,
                    function = x.Function,
                    divisionFunc = x.DivisionFunc,
                    description = x.Description,
                    year = !string.IsNullOrEmpty(x.Year) ? x.Year : ""
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

        public async Task<object> GetAllSecFunctions()
            => await _context.CatalogGoals.Select(x => x.SecFunc).ToListAsync();
        public async Task<object> GetCatalogGoalBySecFunction(string secFunction)
            => await _context.CatalogGoals.Where(x => x.SecFunc == secFunction).FirstOrDefaultAsync();
        public async Task<CatalogGoal> GetCatalogGoalBySecFunctionClass(string secFunction)
            => await _context.CatalogGoals.Where(x => x.SecFunc == secFunction).FirstOrDefaultAsync();
    }
}
