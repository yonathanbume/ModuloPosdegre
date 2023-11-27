using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class NonTeachingLoadRepository : Repository<NonTeachingLoad>, INonTeachingLoadRepository
    {
        public NonTeachingLoadRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<IEnumerable<NonTeachingLoad>> GetAll(string userId, int? category = null, int? minHours = null, int? maxHours = null, Guid? teachingLoadTypeId = null)
        {
            var query = _context.NonTeachingLoads
                .Include(x => x.TeachingLoadType)
                .Include(x=>x.NonTeachingLoadSchedules)
                .Where(y=>y.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.TeacherId == userId);

            if (category.HasValue)
                query = query.Where(x => x.TeachingLoadType.Category == category.Value);

            if (minHours.HasValue)
                query = query.Where(x => TimeSpan.FromMinutes(x.Minutes).TotalHours >= minHours.Value);

            if (maxHours.HasValue)
                query = query.Where(x => TimeSpan.FromMinutes(x.Minutes).TotalHours <= maxHours.Value);

            if (teachingLoadTypeId.HasValue)
                query = query.Where(x => x.TeachingLoadTypeId == teachingLoadTypeId.Value);

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetNonTeachingLoadByTeacher(DataTablesStructs.SentParameters parameters, string userId, Guid? termId)
        {
            var query = _context.NonTeachingLoads
                .Where(x=>x.TeacherId == userId)
                .AsNoTracking();

            if (!termId.HasValue)
            {
                query = query.Where(x => x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            }
            else
            {
                query = query.Where(x => x.TermId == termId);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.TeachingLoadTypeId,
                    x.TeachingLoadSubTypeId,
                    TeachingLoadType = x.TeachingLoadType.Name,
                    x.Name,
                    x.Hours,
                    x.Minutes,
                    x.Location,
                    x.Resolution,
                    x.RelatedCourseId,
                    StartDate = x.StartDate.ToDateFormat(),
                    EndDate = x.EndDate.ToDateFormat(),
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }
    
        public async Task<bool> AnyByTeachingLoadType(Guid? teachingLoadTypeId, Guid? teachingLoadSubTypeId)
        {
            var query = _context.NonTeachingLoads.AsNoTracking();

            if(teachingLoadTypeId.HasValue)
                query = query.Where(x=>x.TeachingLoadTypeId == teachingLoadTypeId);

            if (teachingLoadSubTypeId.HasValue)
                query = query.Where(x => x.TeachingLoadSubTypeId == teachingLoadSubTypeId);

            return await query.AnyAsync();
        }
    }
}
