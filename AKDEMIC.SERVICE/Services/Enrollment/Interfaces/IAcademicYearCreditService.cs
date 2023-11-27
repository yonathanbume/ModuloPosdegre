using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IAcademicYearCreditService
    {
        Task<AcademicYearCredit> Get(params object[] keyValues);
        Task InsertRange(List<AcademicYearCredit> academicYearCredits);
        Task UpdateRange(List<AcademicYearCredit> academicYearCredits);
        Task<List<AcademicYearCredit>> GetCurriculumAcademicYearCredits(Guid curriculumId);
    }
}
