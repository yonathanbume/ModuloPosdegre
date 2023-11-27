using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IExtracurricularCourseGroupAssistanceService
    {
        Task<ExtracurricularCourseGroupAssistance> Get(Guid id);
        Task Insert(ExtracurricularCourseGroupAssistance extracurricularCourseGroupAssistance);
        Task Update(ExtracurricularCourseGroupAssistance extracurricularCourseGroupAssistance);
        Task DeleteById(Guid id);
        Task<IEnumerable<ExtracurricularCourseGroupAssistance>> GetAllByGroup(Guid groupId);
    }
}
