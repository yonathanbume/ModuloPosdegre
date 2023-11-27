using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class StudentComplementaryStudyRepository:Repository<StudentComplementaryStudy> , IStudentComplementaryStudyRepository
    {
        public StudentComplementaryStudyRepository(AkdemicContext context) : base(context) { }

        public async Task<List<StudentComplementaryStudy>> GetByStudentId(Guid studentId, int? type = null)
        {
            var query = _context.StudentComplementaryStudies
                .Where(x => x.StudentId == studentId);

            if (type != null)
                query = query.Where(x => x.Type == type);

            return await query.ToListAsync();
        }
    }
}
