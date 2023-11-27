using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class StudentLanguageRepository:Repository<StudentLanguage> , IStudentLanguageRepository
    {
        public StudentLanguageRepository(AkdemicContext context): base (context) { }

        public async Task<bool> ExistByLanguage(Guid id)
        {
            return await _context.StudentLanguages.AnyAsync(x => x.LanguageId == id);
        }

        public async Task<IEnumerable<StudentLanguage>> GetAllByStudent(Guid studentId)
        {
            var query = _context.StudentLanguages
                    .Where(x => x.StudentId == studentId);

            return await query.ToListAsync();
        }

        public async Task<List<LanguageDate>> GetAllByStudentTemplate(Guid studentId)
        {
            var result = await _context.StudentLanguages
                .Where(x => x.StudentId == studentId)
                .Select(x => new LanguageDate
                {
                    Language = x.Language.Name,
                    Level = x.Level,
                    StringLevel = ConstantHelpers.LEVEL_EXPERIENCE.VALUES[Convert.ToInt32(x.Level)]
                })
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<StudentLanguage>> GetAllWithIncludesByStudent(Guid studentId)
        {
            var query = _context.StudentLanguages
                    .Include(x => x.Language)
                    .Include(x => x.Student)
                        .ThenInclude(x => x.User)
                    .Where(x => x.StudentId == studentId);

            return await query.ToListAsync();
        }
    }
}
