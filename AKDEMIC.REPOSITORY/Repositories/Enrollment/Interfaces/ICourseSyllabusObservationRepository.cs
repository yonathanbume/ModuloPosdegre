using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ICourseSyllabusObservationRepository : IRepository<CourseSyllabusObservation>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCourseSyllabusObservationDatatable(DataTablesStructs.SentParameters parameters, Guid courseSyllabusId);
        Task<List<CourseSyllabusObservation>> GetCourseSyllabusObservations(Guid courseSyllabusId);
        Task<bool> AnyPendingObservation(Guid courseSyllabusId);
    }
}
