using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Evaluation;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface IProjectMemberService
    {
        Task<IEnumerable<object>> GetProjectMembersByProject(Guid projectId);
        Task DeleteRange(IEnumerable<ProjectMember> projectMembers);
        Task InsertRange(IEnumerable<ProjectMember> projectMembers);
        Task<IEnumerable<ProjectMember>> GetProjectMembersByProjectId(Guid value);
    }
}
