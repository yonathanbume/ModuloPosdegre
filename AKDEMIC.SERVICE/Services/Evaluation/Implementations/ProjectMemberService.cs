using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        public ProjectMemberService(IProjectMemberRepository projectMemberRepository)
        {
            _projectMemberRepository = projectMemberRepository;
        }
        public async Task<IEnumerable<object>> GetProjectMembersByProject(Guid projectId)
        {
            return await _projectMemberRepository.GetProjectMembersByProject(projectId);
        }
        public async Task DeleteRange(IEnumerable<ProjectMember> projectMembers)
        {
            await _projectMemberRepository.DeleteRange(projectMembers);
        }
        public async Task InsertRange(IEnumerable<ProjectMember> projectMembers)
        {
            await _projectMemberRepository.InsertRange(projectMembers);
        }

        public async Task<IEnumerable<ProjectMember>> GetProjectMembersByProjectId(Guid value)
        {
            return await _projectMemberRepository.GetProjectMembersByProjectId(value);
        }
    }
}
