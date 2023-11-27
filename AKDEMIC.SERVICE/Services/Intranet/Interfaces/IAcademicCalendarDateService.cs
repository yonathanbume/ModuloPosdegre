using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IAcademicCalendarDateService
    {
        Task Insert(AcademicCalendarDate academicCalendarDate);
        Task Delete(AcademicCalendarDate academicCalendarDate);
        Task<AcademicCalendarDate> Get(Guid id);
        Task<List<AcademicCalendarDate>> GetAcademicCalendarDateForActiveTerm();
        Task<object> GetAcademicCalendarDates(Guid termId, Term term);
        Task<object> GetDateTerm(Guid termId);
        Task<List<AcademicCalendarDate>> GetAcademicCalendarDatesForTerm(Guid termId);
        Task Update(AcademicCalendarDate academicCalendarDetail);
    }
}
