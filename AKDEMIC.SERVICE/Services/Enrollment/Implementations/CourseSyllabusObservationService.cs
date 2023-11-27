using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CourseSyllabusObservationService : ICourseSyllabusObservationService
    {
        private readonly ICourseSyllabusObservationRepository _courseSyllabusObservationRepository;

        public CourseSyllabusObservationService(ICourseSyllabusObservationRepository courseSyllabusObservationRepository)
        {
            _courseSyllabusObservationRepository = courseSyllabusObservationRepository;
        }

        public async Task<bool> AnyPendingObservation(Guid courseSyllabusId)
            => await _courseSyllabusObservationRepository.AnyPendingObservation(courseSyllabusId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseSyllabusObservationDatatable(DataTablesStructs.SentParameters parameters, Guid courseSyllabusId)
            => await _courseSyllabusObservationRepository.GetCourseSyllabusObservationDatatable(parameters, courseSyllabusId);

        public async Task<List<CourseSyllabusObservation>> GetCourseSyllabusObservations(Guid courseSyllabusId)
            => await _courseSyllabusObservationRepository.GetCourseSyllabusObservations(courseSyllabusId);

        public async Task Insert(CourseSyllabusObservation entity)
            => await _courseSyllabusObservationRepository.Insert(entity);

        public async Task UpdateRange(IEnumerable<CourseSyllabusObservation> entities)
            => await _courseSyllabusObservationRepository.UpdateRange(entities);
    }
}
