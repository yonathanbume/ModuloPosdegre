using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IExtracurricularCourseGroupAssistanceStudentRepository : IRepository<ExtracurricularCourseGroupAssistanceStudent>
    {
        Task<IEnumerable<ExtracurricularCourseGroupAssistanceStudent>> GetAllByAssistance(Guid assistanceId);
    }
}
