using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Implementations
{
    public class PreuniversitaryUserGroupRepository : Repository<PreuniversitaryUserGroup>, IPreuniversitaryUserGroupRepository
    {
        public PreuniversitaryUserGroupRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetPreuniversitaryUserGroupList(Guid termId,string studentId)
        {
            var term = await _context.PreuniversitaryTerms.FindAsync(termId);
            var weeks = Math.Ceiling((term.ClassEndDate - term.ClassStartDate).TotalDays / 7);
            var result = await _context.PreuniversitaryUserGroups
                .Where(x => x.ApplicationUserId == studentId)
                .Where(x => x.PreuniversitaryGroup.PreuniversitaryTermId == termId)
                .Select(x => new
                {
                    id = x.Id,
                    course = x.PreuniversitaryGroup.PreuniversitaryCourse.FullName,
                    group = x.PreuniversitaryGroup.Code,
                    total = x.PreuniversitaryGroup.PreuniversitarySchedules.Count() * weeks,
                    maxAbsent = Math.Round(x.PreuniversitaryGroup.PreuniversitarySchedules.Count() * weeks * 0.3).ToString("0.00"),
                    current = x.PreuniversitaryAssistanceStudents.Count(),
                    notAbsent = x.PreuniversitaryAssistanceStudents.Count(pas => !pas.IsAbsent),
                    absent = x.PreuniversitaryAssistanceStudents.Count(pas => pas.IsAbsent),
                    absentPercent = (x.PreuniversitaryAssistanceStudents.Any() ? (x.PreuniversitaryAssistanceStudents.Count(pas => pas.IsAbsent) / Math.Round(x.PreuniversitaryGroup.PreuniversitarySchedules.Count() * weeks) * 100) : 0.00).ToString("0.00") + "%"
                }).ToListAsync();

            return result;
        }

        public async Task<int> GetCountByGroupId(Guid GroupId)
            => await _context.PreuniversitaryUserGroups.Where(x => x.PreuniversitaryGroup.Id == GroupId).CountAsync();

        public async Task<object> GetUserGroupByStudent(string userId, Guid termId)
        {
            var userGroups = await _context.PreuniversitaryUserGroups.Include(x => x.PreuniversitaryGroup.PreuniversitaryCourse)
                .Where(x => x.ApplicationUserId == userId && x.PreuniversitaryGroup.PreuniversitaryTermId == termId).Select(x => new
                {
                    groupCode = x.PreuniversitaryGroup.Code,
                    courseCode = x.PreuniversitaryGroup.PreuniversitaryCourse.Code,
                    courseName = x.PreuniversitaryGroup.PreuniversitaryCourse.Name,
                    grade = x.Grade
                }).ToListAsync();

            return userGroups;
        }

        public async Task<object> GetStudentGroupToTeacher(Guid id)
        {
            var result = await _context.PreuniversitaryUserGroups
                .Include(x => x.ApplicationUser)
                .Include(x => x.PreuniversitaryGroup)
                .Where(x => x.PreuniversitaryGroupId == id)
                .Select(x => new {
                    id = x.Id,
                    fullName = x.ApplicationUser.FullName,
                    grade = x.Grade,
                    isQualified = x.IsQualified
                }).ToListAsync();

            return result;
        }
    }
}
