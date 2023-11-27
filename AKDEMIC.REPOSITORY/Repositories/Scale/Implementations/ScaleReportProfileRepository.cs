using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleReportProfileRepository : Repository<ScaleReportProfile>, IScaleReportProfileRepository
    {
        public ScaleReportProfileRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByName(string name, Guid? id = null)
        {
            return await _context.ScaleReportProfiles.AnyAsync(x => x.Name.ToUpper() == name.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportProfileDataTable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ScaleReportProfile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name; break;
            }

            var query = _context.ScaleReportProfiles.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name
                })
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
        public override async Task DeleteById(Guid id)
        {
            var reportProfile = await _context.ScaleReportProfiles.Where(x => x.Id == id).FirstOrDefaultAsync();
            var reportProfileDetail = await _context.ScaleReportProfileDetails.Where(x => x.ScaleReportProfileId == id).ToListAsync();

            _context.ScaleReportProfileDetails.RemoveRange(reportProfileDetail);
            _context.ScaleReportProfiles.Remove(reportProfile);
            await _context.SaveChangesAsync();
        }
    }
}
