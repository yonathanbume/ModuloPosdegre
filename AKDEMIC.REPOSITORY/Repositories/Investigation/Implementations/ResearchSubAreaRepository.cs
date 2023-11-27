using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Implementations
{
    public class ResearchSubAreaRepository : Repository<ResearchSubArea>, IResearchSubAreaRepository
    {
        public ResearchSubAreaRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyResearchSubAreaByName(string name, Guid? id)
        {
            if (id == null)
            {
                return await _context.ResearchSubAreas.Where(x => x.Name == name).AnyAsync();
            }
            else
            {
                return await _context.ResearchSubAreas.Where(x => x.Name == name && x.Id != id).AnyAsync();
            }
        }

        public async Task<object> GetResearchSubArea(Guid id)
        {
            var query = _context.ResearchSubAreas.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                area = x.ResearchArea.Name,
                areaId = x.ResearchAreaId
            });
            return await query.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<IEnumerable<object>> GetResearchSubAreas()
        {
            var query = _context.ResearchSubAreas.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                area = x.ResearchArea.Name,
                areaId = x.ResearchAreaId
            });
            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetResearchSubAreasDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ResearchSubArea, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.ResearchArea.Name); break;
                default:
                    orderByPredicate = ((x) => x.Name); break;
            }

            IQueryable<ResearchSubArea> query = _context.ResearchSubAreas.AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    areaId = x.ResearchArea.Id,
                    area = x.ResearchArea.Name
                }).ToListAsync();

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
