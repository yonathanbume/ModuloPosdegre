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
    public class BuildingRepository : Repository<Building>, IBuildingRepository
    {
        public BuildingRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<Building>> GetAllWithData()
        {
            var result = await _context.Buildings
            .Include(x => x.Campus)
              .ToArrayAsync();

            return result;
        }

        public async Task<IEnumerable<Building>> GetAllByCampusId(Guid campusId)
            => await _context.Buildings
                .Where(x => x.CampusId == campusId)
                .ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid? campus)
        {
            Expression<Func<Building, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                default:
                    break;
            }

            var query = _context.Buildings
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            if (campus != null) query = query.Where(x => x.CampusId == campus);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name
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

        public async Task<IEnumerable<Select2Structs.Result>> GetBuildingsSelect2ClientSide(Guid? campusId = null)
        {
            var query = _context.Buildings.AsQueryable();

            if (campusId.HasValue)
                query = query.Where(x => x.CampusId == campusId);

            var result = await query
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<object> GetBuildingsJson(Guid id)
        {
            var result = await _context.Buildings
                .Where(b => b.CampusId == id)
                .Select(b => new
                {
                    id = b.Id,
                    text = b.Name
                }).ToListAsync();

            return result;
        }
    }
}
