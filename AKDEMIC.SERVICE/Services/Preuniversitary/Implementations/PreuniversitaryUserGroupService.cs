using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Implementations
{
    public class PreuniversitaryUserGroupService : IPreuniversitaryUserGroupService
    {
        private readonly IPreuniversitaryUserGroupRepository _preuniversitaryUserGroupRepository;

        public PreuniversitaryUserGroupService(IPreuniversitaryUserGroupRepository preuniversitaryUserGroupRepository)
        {
            _preuniversitaryUserGroupRepository = preuniversitaryUserGroupRepository;
        }

        public async Task<PreuniversitaryUserGroup> Get(Guid id)
            => await _preuniversitaryUserGroupRepository.Get(id);

        public async Task<int> GetCountByGroupId(Guid GroupId)
            => await _preuniversitaryUserGroupRepository.GetCountByGroupId(GroupId);

        public async Task<object> GetPreuniversitaryUserGroupList(Guid termId, string studentId)
            => await _preuniversitaryUserGroupRepository.GetPreuniversitaryUserGroupList(termId, studentId);

        public async Task<object> GetStudentGroupToTeacher(Guid id)
            => await _preuniversitaryUserGroupRepository.GetStudentGroupToTeacher(id);

        public async Task<object> GetUserGroupByStudent(string userId, Guid termId)
            => await _preuniversitaryUserGroupRepository.GetUserGroupByStudent(userId, termId);

        public async Task InsertRange(IEnumerable<PreuniversitaryUserGroup> entities)
            => await _preuniversitaryUserGroupRepository.InsertRange(entities);

        public async Task Update(PreuniversitaryUserGroup entity)
            => await _preuniversitaryUserGroupRepository.Update(entity);
    }
}
