using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class AreaRepository : Repository<Area>, IAreaRepository
    {
        public AreaRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            Expression<Func<Area, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.IsSpecialty;
                    break;
                default:
                    //orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.Areas
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsSpecialty = x.IsSpecialty
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetAreasSelect2ClientSide()
        {
            var result = await _context.Areas
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<object> GetAllAsSelect2ClientSide()
        {
            var areas = await _context.Areas
                .OrderBy(x => x.Name)
                .ThenBy(x => x.IsSpecialty)
                  .Select(a => new
                  {
                      id = a.Id,
                      text = a.Name + (a.IsSpecialty ? "*" : ""),
                      specialty = a.IsSpecialty
                  }).ToListAsync();

            return areas;
        }
        public async Task<object> GetAreaJson(Guid id)
        {
            var area = await _context.Areas
                .Where(a => a.Id.Equals(id))
                .Select(a => new
                {
                    id = a.Id,
                    text = a.Name
                }).FirstOrDefaultAsync();

            return area;
        }

        public async Task<object> GetAreaWithDataJson()
        {
            var areas = await _context.Areas
                .OrderBy(x => x.Name)
                .ThenBy(x => x.IsSpecialty)
                .Select(a => new
                {
                    id = a.Id,
                    text = a.Name + (a.IsSpecialty ? "*" : ""),
                    specialty = a.IsSpecialty
                }).ToListAsync();

            return areas;
        }

        public async Task<Area> GetFirst()
        {
            return await _context.Areas.FirstOrDefaultAsync();
        }

        public async Task<Area> GetByName(string name)
        {
            return await _context.Areas.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Name.ToUpper() == name.ToUpper());
        }
    }
}
