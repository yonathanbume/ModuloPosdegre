using AKDEMIC.ENTITIES.Models.Preuniversitary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryUserGroupService
    {
        Task<object> GetPreuniversitaryUserGroupList(Guid termId, string studentId);
        Task<int> GetCountByGroupId(Guid GroupId);
        Task InsertRange(IEnumerable<PreuniversitaryUserGroup> entities);
        Task<object> GetUserGroupByStudent(string userId, Guid termId);
        Task<object> GetStudentGroupToTeacher(Guid id);
        Task<PreuniversitaryUserGroup> Get(Guid id);
        Task Update(PreuniversitaryUserGroup entity);
    }
}
