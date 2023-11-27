using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class StudentInformationRepository : Repository<StudentInformation>, IStudentInformationRepository
    {
        public StudentInformationRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<StudentInformation> GetByStudentAndTerm(Guid studentId, Guid termId)
        {
            var studentInformation = await _context.StudentInformations
                .Include(x => x.PlaceOriginDistrict)
                    .ThenInclude(y => y.Province)
                        .ThenInclude(z => z.Department)
                .Where(x => x.StudentId == studentId && x.TermId == termId)
                .FirstOrDefaultAsync();

            return studentInformation;
        }

        public async Task<object> GetOriginSchoolSelect()
        {
            var result = await _context.StudentInformations
                .Where(x => !string.IsNullOrEmpty(x.OriginSchool))
                .Select(x => new
                {
                    id = x.OriginSchool.ToUpper().Trim(),
                    text = x.OriginSchool.ToUpper().Trim()
                })
                .Distinct()
                .OrderBy(x => x.text)
                .ToListAsync();

            return result;
        }

        public async Task<bool> HasStudentInformation(Guid studentId)
        {
            var result = await _context.StudentInformations
                .AnyAsync(x => x.StudentId == studentId && x.TermId != null);

            return result;
        }
    }
}
