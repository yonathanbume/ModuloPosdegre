using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IExtracurricularCourseGroupAssistanceStudentService
    {
        Task<ExtracurricularCourseGroupAssistanceStudent> Get(Guid id);
        Task Insert(ExtracurricularCourseGroupAssistanceStudent extracurricularCourseGroupAssistanceStudent);
        Task InsertRange(IEnumerable<ExtracurricularCourseGroupAssistanceStudent> extracurricularCourseGroupAssistanceStudents);
        Task Update(ExtracurricularCourseGroupAssistanceStudent extracurricularCourseGroupAssistanceStudent);
        Task UpdateRange(IEnumerable<ExtracurricularCourseGroupAssistanceStudent> extracurricularCourseGroupAssistanceStudents);
        Task DeleteById(Guid id);
        Task DeleteRange(IEnumerable<ExtracurricularCourseGroupAssistanceStudent> extracurricularCourseGroupAssistanceStudents);
        Task<IEnumerable<ExtracurricularCourseGroupAssistanceStudent>> GetAllByAssistance(Guid assistanceId);
    }
}
