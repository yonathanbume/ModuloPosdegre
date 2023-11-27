using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Implementations
{
    public class PreuniversitaryAssistanceRepository : Repository<PreuniversitaryAssistance> , IPreuniversitaryAssistanceRepository
    {
        public PreuniversitaryAssistanceRepository(AkdemicContext context) : base(context) { }

        public async Task<PreuniversitaryAssistance> GetByScheduleAndCurrentDate(Guid scheduleId)
            => await _context.PreuniversitaryAssistances.Where(x => x.DateTime.Date == DateTime.UtcNow.Date && x.PreuniversitaryScheduleId == scheduleId).Include(x=>x.PreuniversitaryAssistanceStudents).FirstOrDefaultAsync();

        public async Task<List<PreuniversitaryUserGroupTemplate>> GetAssistancesStudentByAssistanceId(PreuniversitaryAssistance entity, string userId, Guid scheduleId, Guid assistanceId)
        {
            var result = entity == null
                ? await _context.PreuniversitaryUserGroups
                .Where(x => x.PreuniversitaryGroup.TeacherId == userId)
                .Where(x => x.PreuniversitaryGroup.PreuniversitarySchedules.Any(ps => ps.Id == scheduleId))
                .Select(x => new PreuniversitaryUserGroupTemplate
                {
                    Id = x.Id,
                    Name = x.ApplicationUser.FullName,
                    Dni = x.ApplicationUser.Dni,
                    UserName = x.ApplicationUser.UserName,
                    IsAbsent = false
                }).AsNoTracking().ToListAsync()
                : await _context.PreuniversitaryAssistanceStudents
                .Where(x => x.PreuniversitaryAssistanceId == assistanceId)
                .Select(x => new PreuniversitaryUserGroupTemplate
                {
                    Id = x.PreuniversitaryUserGroupId,
                    Dni = x.PreuniversitaryUserGroup.ApplicationUser.Dni,
                    Name = x.PreuniversitaryUserGroup.ApplicationUser.FullName,
                    UserName = x.PreuniversitaryUserGroup.ApplicationUser.UserName,
                    IsAbsent = x.IsAbsent
                }).AsNoTracking().ToListAsync();

            return result;
        }

        public async Task InsertRangeStudentAssistance(IEnumerable<PreuniversitaryAssistanceStudent> entities)
        {
            await _context.PreuniversitaryAssistanceStudents.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
    }
}
