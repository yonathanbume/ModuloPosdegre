using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Investigation;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IProjectMemberService
    {
        Task<IEnumerable<ProjectMember>> GetProjectMembersByProject(Guid projectId);
        Task<IEnumerable<string>> GetProjectMembersByProjectFullName(Guid projectId);
        Task DeleteRange(IEnumerable<ProjectMember> projectMembers);
        Task InsertRange(IEnumerable<ProjectMember> projectMembers);
        Task<object> GetRegisteredResearchersChart(string startSearchDate = null, string endSearchDate = null);
    }
}
