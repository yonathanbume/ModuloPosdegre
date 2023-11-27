using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Implementations
{
    public class ProjectMemberRepository : Repository<ProjectMember>, IProjectMemberRepository
    {
        public ProjectMemberRepository(AkdemicContext context) : base(context) { }

        #region PUBLIC

        public async Task<IEnumerable<ProjectMember>> GetProjectMembersByProject(Guid projectId)
        {
            var query = _context.InvestigationProjectMembers.Include(x => x.Member).Where(x => x.ProjectId == projectId);
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<string>> GetProjectMembersByProjectFullName(Guid projectId)
        {
            var query = _context.InvestigationProjectMembers.Where(x => x.ProjectId == projectId).Select(x=>x.Member.FullName);
            return await query.ToListAsync();
        }

        public async Task<object> GetRegisteredResearchersChart(string startSearchDate = null, string endSearchDate = null)
        {
            var query = _context.InvestigationProjectMembers.AsQueryable();

            if (!string.IsNullOrEmpty(startSearchDate))
            {
                var startDate = ConvertHelpers.DatepickerToUtcDateTime(startSearchDate);
                query = query.Where(x => x.Project.StartDate >= startDate);
            }

            if (!string.IsNullOrEmpty(endSearchDate))
            {
                var endDate = ConvertHelpers.DatepickerToUtcDateTime(endSearchDate);
                query = query.Where(x => x.Project.StartDate <= endDate);
            }

            var chartData = await _context.Faculties
                .Select(x => new
                {
                    x.Name,
                    y = query.Where(y => y.Project.Career.FacultyId == x.Id).Select(y => y.MemberId).Distinct().Count()
                })
                .ToListAsync();

            return chartData;
        }
        #endregion
    }
}
