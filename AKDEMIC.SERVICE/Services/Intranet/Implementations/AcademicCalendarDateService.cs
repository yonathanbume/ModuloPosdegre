using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class AcademicCalendarDateService : IAcademicCalendarDateService
    {
        private readonly IAcademicCalendarDateRepository _academicCalendarDateRepository;

        public AcademicCalendarDateService(IAcademicCalendarDateRepository academicCalendarDateRepository)
        {
            _academicCalendarDateRepository = academicCalendarDateRepository;
        }

        public async Task Insert(AcademicCalendarDate academicCalendarDate)
            => await _academicCalendarDateRepository.Insert(academicCalendarDate);

        public async Task Delete(AcademicCalendarDate academicCalendarDate)
            => await _academicCalendarDateRepository.Delete(academicCalendarDate);

        public async Task<AcademicCalendarDate> Get(Guid id)
            => await _academicCalendarDateRepository.Get(id);
        public async Task<List<AcademicCalendarDate>> GetAcademicCalendarDateForActiveTerm()
        {
            return await _academicCalendarDateRepository.GetAcademicCalendarDateForActiveTerm();
        }

        public async Task<object> GetAcademicCalendarDates(Guid termId, Term term)
            => await _academicCalendarDateRepository.GetAcademicCalendarDates(termId, term);

        public async Task<object> GetDateTerm(Guid termId)
            => await _academicCalendarDateRepository.GetDateTerm(termId);

        public async Task<List<AcademicCalendarDate>> GetAcademicCalendarDatesForTerm(Guid termId)
            => await _academicCalendarDateRepository.GetAcademicCalendarDatesForTerm(termId);

        public async Task Update(AcademicCalendarDate academicCalendarDetail)
        {
            await _academicCalendarDateRepository.Update(academicCalendarDetail);
        }
    }
}
