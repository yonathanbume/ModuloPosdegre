using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICourseSyllabusObservationService
    {
        Task Insert(CourseSyllabusObservation entity);
        Task<DataTablesStructs.ReturnedData<object>> GetCourseSyllabusObservationDatatable(DataTablesStructs.SentParameters parameters, Guid courseSyllabusId);
        Task<List<CourseSyllabusObservation>> GetCourseSyllabusObservations(Guid courseSyllabusId);
        Task UpdateRange(IEnumerable<CourseSyllabusObservation> entities);
        Task<bool> AnyPendingObservation(Guid courseSyllabusId);
    }
}
