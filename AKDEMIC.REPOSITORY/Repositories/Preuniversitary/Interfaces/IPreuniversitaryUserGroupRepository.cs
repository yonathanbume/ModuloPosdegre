using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryUserGroupRepository : IRepository<PreuniversitaryUserGroup>
    {
        Task<object> GetPreuniversitaryUserGroupList(Guid termId, string studentId);
        Task<int> GetCountByGroupId(Guid GroupId);
        Task<object> GetUserGroupByStudent(string userId, Guid termId);
        Task<object> GetStudentGroupToTeacher(Guid id);
    }
}
