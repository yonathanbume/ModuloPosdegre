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
    public class CampusCareerRepository : Repository<CampusCareer>, ICampusCareerRepository
    {
        public CampusCareerRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid? campus)
        {
            Expression<Func<CampusCareer, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Campus.Name;
                    break;
                default:
                    //orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.CampusCareers
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Career.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            if (campus != null && campus != Guid.Empty)
            {
                query = query.Where(x => x.CampusId == campus);
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Career.Id,
                    name = x.Career.Name
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

        public async Task<CampusCareer> GetCampusByIdAndCareer(Guid careerId, Guid campusId)
        {
            var result = await _context.CampusCareers.FirstOrDefaultAsync(x => x.CareerId == careerId && x.CampusId == campusId);

            return result;
        }

        public async Task<object> GetCareerSelect2ByCampusId(Guid campusId)
        {
            var result = await _context.CampusCareers.Include(x=>x.Career)
                .Where(x => x.CampusId == campusId)
                .Select(x => new 
                {
                    Id = x.CareerId,
                    Text = x.Career.Name
                })
                .ToListAsync();

            return result;
        }

        public override async Task<IEnumerable<CampusCareer>> GetAll()
        {
            return await _context.CampusCareers
                .Include(x => x.Career)
                .Include(x => x.Campus)
                .ToListAsync();
        }
    }
}
