using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
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
    public class ScaleReportProfileDetailRepository:Repository<ScaleReportProfileDetail>, IScaleReportProfileDetailRepository
    {
        public ScaleReportProfileDetailRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyBySectionNumber(int sectionNumber, Guid reportProfileId , Guid? id = null)
        {
            return await _context.ScaleReportProfileDetails.AnyAsync(x => x.ScalePdfSectionNumber == sectionNumber && x.ScaleReportProfileId == reportProfileId && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportProfileDetailDataTable(DataTablesStructs.SentParameters sentParameters,Guid profileId, string searchValue = null)
        {
            Expression<Func<ScaleReportProfileDetail, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.ScalePdfSectionNumber; break;
            }

            var query = _context.ScaleReportProfileDetails
                .Where(x => x.ScaleReportProfileId == profileId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                var sections = ConstantHelpers.SCALEPDFSECTIONS.VALUES.Where(x => x.Value.ToUpper().Contains(searchValue.ToUpper())).Select(x => x.Key).ToList();

                query = query.Where(x => sections.Contains(x.ScalePdfSectionNumber));
            }


            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = ConstantHelpers.SCALEPDFSECTIONS.VALUES.ContainsKey(x.ScalePdfSectionNumber) ? ConstantHelpers.SCALEPDFSECTIONS.VALUES[x.ScalePdfSectionNumber] : "-"
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

        public async Task<List<int>> GetSectionsByProfile(Guid profileId)
        {
            return await _context.ScaleReportProfileDetails.Where(x => x.ScaleReportProfileId == profileId).Select(x => x.ScalePdfSectionNumber).Distinct().ToListAsync();
        }
    }
}
