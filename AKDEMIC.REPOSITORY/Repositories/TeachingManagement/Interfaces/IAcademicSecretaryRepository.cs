using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface IAcademicSecretaryRepository : IRepository<AcademicSecretary>
    {
        Task<bool> AnyByTeacherIdAndFacultyId(Guid teacherId, Guid facultyId);
        Task<int> CountByTeacherId(Guid teacherId);
        Task<object> GetAllAsModelA(Guid? facultyId = null);
        Task<object> GetAsModelB(Guid? id = null);
    }
}