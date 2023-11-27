using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IExtracurricularCourseGroupStudentService
    {
        Task<ExtracurricularCourseGroupStudent> Get(Guid id);
        Task Insert(ExtracurricularCourseGroupStudent extracurricularCourseGroupStudent);
        Task InsertRange(IEnumerable<ExtracurricularCourseGroupStudent> extracurricularCourseGroupStudents);
        Task Update(ExtracurricularCourseGroupStudent extracurricularCourseGroupStudent);
        Task UpdateRange(IEnumerable<ExtracurricularCourseGroupStudent> extracurricularCourseGroupStudents);
        Task Delete(ExtracurricularCourseGroupStudent extracurricularCourseGroupStudent);
        Task DeleteById(Guid id);
        Task DeleteRange(IEnumerable<ExtracurricularCourseGroupStudent> extracurricularCourseGroupStudents);
        Task<bool> AnyStudentInCourse(Guid studentId, Guid extracurricularCourseId);
        Task<IEnumerable<ExtracurricularCourseGroupStudent>> GetAllByStudent(Guid studentId);
        Task<IEnumerable<ExtracurricularCourseGroupStudent>> GetAllByGroup(Guid groupId, byte? paymentStatus = null);
    }
}
