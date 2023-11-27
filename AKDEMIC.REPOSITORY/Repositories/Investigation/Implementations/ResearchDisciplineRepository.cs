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
    public class ResearchDisciplineRepository : Repository<ResearchDiscipline>, IResearchDisciplineRepository
    {
        public ResearchDisciplineRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyResearchDisciplineByName(string name, Guid? id)
        {
            if (id == null)
            {
                return await _context.ResearchDisciplines.Where(x => x.Name == name).AnyAsync();
            }
            else
            {
                return await _context.ResearchDisciplines.Where(x => x.Name == name && x.Id != id).AnyAsync();
            }
        }

        public async Task<object> GetResearchDiscipline(Guid id)
        {
            var query = _context.ResearchDisciplines.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                subareaId = x.ResearchSubAreaId,
                subarea = x.ResearchSubArea.Name,
                areaId = x.ResearchSubArea.ResearchAreaId
            });
            return await query.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<IEnumerable<object>> GetResearchDisciplines()
        {
            var query = _context.ResearchDisciplines.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                subarea = x.ResearchSubArea.Name,
                subareaId = x.ResearchSubAreaId
            });
            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetResearchDisciplinesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? areaId, Guid? subareaId, string searchValue = null)
        {
            Expression<Func<ResearchDiscipline, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.ResearchSubArea.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.ResearchSubArea.ResearchArea.Name); break;
                default:
                    orderByPredicate = ((x) => x.Name); break;
            }

            IQueryable<ResearchDiscipline> query = _context.ResearchDisciplines.AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue));
            }

            if (areaId.HasValue)
            {
                query = query.Where(x => x.ResearchSubArea.ResearchAreaId == areaId);
            }

            if (subareaId.HasValue)
            {
                query = query.Where(x => x.ResearchSubAreaId == subareaId);
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
                    subarea = x.ResearchSubArea.Name,
                    area = x.ResearchSubArea.ResearchArea.Name
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
