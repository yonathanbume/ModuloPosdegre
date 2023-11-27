using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IAcademicCalendarDateRepository : IRepository<AcademicCalendarDate>
    {
        Task<List<AcademicCalendarDate>> GetAcademicCalendarDateForActiveTerm();
        Task<object> GetAcademicCalendarDates(Guid termId, Term term);
        Task<object> GetDateTerm(Guid termId);
        Task<List<AcademicCalendarDate>> GetAcademicCalendarDatesForTerm(Guid termId);
    }
}
