using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class StudentIncomeScoreRepository : Repository<StudentIncomeScore>, IStudentIncomeScoreRepository
    {
        public StudentIncomeScoreRepository(AkdemicContext context) : base(context) { }

        public async Task<StudentIncomeScore> GetByStudent(Guid studentId)
            => await _context.StudentIncomeScores.Where(x => x.StudentId == studentId).FirstOrDefaultAsync();
            

    }
}
