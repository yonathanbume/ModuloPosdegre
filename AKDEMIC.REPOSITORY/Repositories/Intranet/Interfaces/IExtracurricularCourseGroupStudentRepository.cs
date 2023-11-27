using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IExtracurricularCourseGroupStudentRepository : IRepository<ExtracurricularCourseGroupStudent>
    {
        Task<bool> AnyStudentInCourse(Guid studentId, Guid extracurricularCourseId);
        Task<IEnumerable<ExtracurricularCourseGroupStudent>> GetAllByStudent(Guid studentId);
        Task<IEnumerable<ExtracurricularCourseGroupStudent>> GetAllByGroup(Guid groupId, byte? paymentStatus = null);
    }
}
