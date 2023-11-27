using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        public ProjectMemberService(IProjectMemberRepository projectMemberRepository)
        {
            _projectMemberRepository = projectMemberRepository;
        }
        public async Task<IEnumerable<ProjectMember>> GetProjectMembersByProject(Guid projectId)
        {
            return await _projectMemberRepository.GetProjectMembersByProject(projectId);
        }
        public async Task<IEnumerable<string>> GetProjectMembersByProjectFullName(Guid projectId)
        {
            return await _projectMemberRepository.GetProjectMembersByProjectFullName(projectId);
        }
        public async Task DeleteRange(IEnumerable<ProjectMember> projectMembers)
        {
            await _projectMemberRepository.DeleteRange(projectMembers);
        }
        public async Task InsertRange(IEnumerable<ProjectMember> projectMembers)
        {
            await _projectMemberRepository.InsertRange(projectMembers);
        }

        public async Task<object> GetRegisteredResearchersChart(string startSearchDate = null, string endSearchDate = null)
            => await _projectMemberRepository.GetRegisteredResearchersChart(startSearchDate, endSearchDate);

    }
}
