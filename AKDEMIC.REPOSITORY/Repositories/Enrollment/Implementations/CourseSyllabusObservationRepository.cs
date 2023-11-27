using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CourseSyllabusObservationRepository : Repository<CourseSyllabusObservation>, ICourseSyllabusObservationRepository
    {
        public CourseSyllabusObservationRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseSyllabusObservationDatatable(DataTablesStructs.SentParameters parameters, Guid courseSyllabusId)
        {
            var query = _context.CourseSyllabusObservations.Where(x => x.CourseSyllabusId == courseSyllabusId).AsNoTracking();
            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    x.Completed,
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

        public async Task<List<CourseSyllabusObservation>> GetCourseSyllabusObservations(Guid courseSyllabusId)
            => await _context.CourseSyllabusObservations.Where(x => x.CourseSyllabusId == courseSyllabusId).ToListAsync();

        public async Task<bool> AnyPendingObservation(Guid courseSyllabusId)
            => await _context.CourseSyllabusObservations.AnyAsync(x => x.CourseSyllabusId == courseSyllabusId && !x.Completed);
    }
}
