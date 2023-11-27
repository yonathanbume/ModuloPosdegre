using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class DidacticalMaterialRepository : Repository<DidacticalMaterial>, IDidacticalMaterialRepository
    {
        public DidacticalMaterialRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDidacticalMaterialsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<DidacticalMaterial, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Title);
                    break;
            }

            var query = _context.DidacticalMaterials.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Title.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Title
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
