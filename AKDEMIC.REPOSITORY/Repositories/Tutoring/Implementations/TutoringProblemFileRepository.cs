using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutoringProblemFileRepository : Repository<TutoringProblemFile>, ITutoringProblemFileRepository
    {
        public TutoringProblemFileRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TutoringProblemFile>> GetAllByTutoringProblemId(Guid tutoringProblemId)
            => await _context.TutoringProblemFiles
            .Where(x => x.TutoringProblemId == tutoringProblemId)
            .ToListAsync();
    }
}
