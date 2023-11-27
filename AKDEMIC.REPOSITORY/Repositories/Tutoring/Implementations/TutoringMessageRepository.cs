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
    public class TutoringMessageRepository : Repository<TutoringMessage>, ITutoringMessageRepository
    {
        public TutoringMessageRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TutoringMessage>> GetAllByTutoringStudentIdAndTutorId(Guid? tutoringStudentId = null, string tutorId = null)
        {
            var query = _context.TutoringMessages.AsQueryable();
            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutorId == tutorId);
            if (tutoringStudentId.HasValue)
                query = query.Where(x => x.TutoringStudentStudentId == tutoringStudentId.Value);
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<TutoringMessage>> GetAllByTutor(string tutorId)
            => await _context.TutoringMessages.Where(x => x.TutorId == tutorId).ToListAsync();
    }
}
