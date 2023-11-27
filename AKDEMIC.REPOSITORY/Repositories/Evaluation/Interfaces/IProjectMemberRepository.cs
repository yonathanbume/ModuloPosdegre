using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface IProjectMemberRepository : IRepository<ProjectMember>
    {
        Task<IEnumerable<object>> GetProjectMembersByProject(Guid projectId);
        Task<IEnumerable<ProjectMember>> GetProjectMembersByProjectId(Guid projectId);
    }
}
