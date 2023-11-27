using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface IAcademicSecretaryService
    {
        Task<AcademicSecretary> GetAsync(Guid id);
        Task InsertAsync(AcademicSecretary academicSecretary);
        Task UpdateAsync(AcademicSecretary academicSecretary);
        Task DeleteAsync(AcademicSecretary academicSecretary);
        Task<bool> AnyByTeacherIdAndFacultyId(Guid teacherId, Guid facultyId);
        Task<int> CountByTeacherId(Guid teacherId);
        Task<object> GetAllAsModelA(Guid? facultyId = null);
        Task<object> GetAsModelB(Guid? id = null);
    }
}