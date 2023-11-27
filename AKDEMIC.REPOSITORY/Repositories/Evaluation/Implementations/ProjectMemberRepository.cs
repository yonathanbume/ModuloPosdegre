using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Implementations
{
    public class ProjectMemberRepository : Repository<ProjectMember>, IProjectMemberRepository
    {
        public ProjectMemberRepository(AkdemicContext context) : base(context) { }

        #region PUBLIC

        public async Task<IEnumerable<object>> GetProjectMembersByProject(Guid projectId)
        {
            var query = _context.EvaluationProjectMembers.Where(x => x.ProjectId == projectId).Select(x => new 
            {
                member = x.Member.FullName
            });
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ProjectMember>> GetProjectMembersByProjectId(Guid projectId)
        {
            var query = _context.EvaluationProjectMembers.Where(x => x.ProjectId == projectId);
            return await query.ToListAsync();
        }
        #endregion
    }
}
