using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Implementations
{
    public class ScholarshipFileRepository : Repository<ScholarshipFile>, IScholarshipFileRepository
    {
        public ScholarshipFileRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<ScholarshipFile>> GetAllByScholarshipId(Guid scholarshipId)
            => await _context.ScholarshipFiles.Where(x => x.ScholarshipId == scholarshipId).ToArrayAsync();
    }
}
