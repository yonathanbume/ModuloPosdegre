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
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleExtraDisplacementFieldRepository : Repository<ScaleExtraDisplacementField>, IScaleExtraDisplacementFieldRepository
    {
        public ScaleExtraDisplacementFieldRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<ScaleExtraDisplacementField>> GetByScaleResolutionTypeAndUser(string userId, string resolutionTypeName)
        {
            return await _context.ScaleExtraDisplacementFields
                .Include(x => x.ScaleResolution)
                .Where(x => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == resolutionTypeName
                        && x.ScaleResolution.UserId == userId).ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDisplacementRecordByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            Expression<Func<ScaleExtraDisplacementField, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ScaleResolution.ResolutionNumber); break;
                case "1":
                    orderByPredicate = ((x) => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.ScaleResolution.BeginDate); break;
                case "3":
                    orderByPredicate = ((x) => x.ScaleResolution.EndDate); break;
                case "4":
                    orderByPredicate = ((x) => x.LaborPosition); break;
                case "5":
                    orderByPredicate = ((x) => x.ScaleResolution.Observation); break;
            }

            var query = _context.ScaleExtraDisplacementFields
                .Where(x => x.ScaleResolution.UserId == userId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    beginDate = x.ScaleResolution.BeginDate.ToLocalDateFormat() ?? "-",
                    scaleResolutionType = x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name ?? "-",
                    endDate = x.ScaleResolution.EndDate.ToLocalDateFormat() ?? "-",
                    resolutionNumber = x.ScaleResolution.ResolutionNumber ?? "-",
                    observations = x.ScaleResolution.Observation ?? "-",
                    laborPosition = x.LaborPosition ?? "-",
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

        public async Task<ScaleExtraDisplacementField> GetScaleExtraDisplacementFieldByResolutionId(Guid resolutionId)
        {
            return await _context.ScaleExtraDisplacementFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == resolutionId);
        }

        public async Task<string> GetScalefieldByUserId(string id)
        {
            var scalefield = await _context.ScaleExtraDisplacementFields
                .Where(x => x.ScaleResolution.UserId == id)
                .OrderBy(x => x.ScaleResolution.ExpeditionDate)
                .FirstOrDefaultAsync();

            if (scalefield != null)
                return scalefield.LaborPosition;
            return string.Empty;
        }
    }
}
