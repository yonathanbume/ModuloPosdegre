using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.DeanFaculty;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class DeanFacultyRepository : Repository<DeanFaculty>, IDeanFacultyRepository
    {
        public DeanFacultyRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<List<DeanFacultyTemplate>> GetByFaculty(Guid id)
        {
            return await _context.DeanFaculties
                .IgnoreQueryFilters()
                .Where(x => x.FacultyId == id)
                .Select(x => new DeanFacultyTemplate
                {
                    Dean = x.Dean.FullName,
                    Secretary =  x.Secretary.FullName,
                    Date = x.CreatedAt.ToLocalDateFormat(),
                    DateUtc = x.CreatedAt.Value,
                    DateEnd = x.DeletedAt.HasValue ? x.DeletedAt.Value.ToLocalDateTimeFormat() : "-",
                    Resolution = x.DeanResolution,
                    ResolutionFile = x.DeanResolutionFile

                })
                .OrderByDescending(x => x.DateUtc)
                .ToListAsync();
        }
    }
}
