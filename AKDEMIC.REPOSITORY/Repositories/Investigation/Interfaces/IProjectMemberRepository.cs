using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces
{
    public interface IProjectMemberRepository : IRepository<ProjectMember>
    {
        Task<IEnumerable<ProjectMember>> GetProjectMembersByProject(Guid projectId);
        Task<IEnumerable<string>> GetProjectMembersByProjectFullName(Guid projectId);
        Task<object> GetRegisteredResearchersChart(string startSearchDate = null, string endSearchDate = null);
    }
}
